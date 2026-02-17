<%@ WebHandler Language="VB" Class="UserApiHandler" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.SessionState
Imports System.Web.Security

Public Class UserApiHandler
    Implements IHttpHandler, IRequiresSessionState

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.AddHeader("Access-Control-Allow-Origin", "*")
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
        context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization")

        Try
            Dim method As String = context.Request.HttpMethod.ToUpper()
            Dim methodName As String = context.Request.QueryString("method")
            Dim result As Object = Nothing

            ' Handle specific method calls regardless of HTTP verb
            If Not String.IsNullOrEmpty(methodName) Then
                Select Case methodName.ToLower()
                    Case "getcurrentuser"
                        result = GetCurrentUser(context)
                    Case "getprofile"
                        result = GetProfile(context)
                    Case "login"
                        result = Login(context)
                    Case "logout"
                        result = Logout(context)
                    Case "register"
                        result = Register(context)
                    Case "changepassword"
                        result = ChangePassword(context)
                    Case "updateprofile"
                        result = UpdateProfile(context)
                    Case Else
                        result = New With {
                            .success = False,
                            .message = "Unknown method"
                        }
                End Select
            Else
                ' Handle standard HTTP verbs
                Select Case method
                    Case "GET"
                        result = GetCurrentUser(context)
                    Case "POST"
                        result = Login(context)
                    Case "OPTIONS"
                        Return
                    Case Else
                        result = New With {
                            .success = False,
                            .message = "Method not allowed"
                        }
                End Select
            End If

            Dim json As String = SerializeToJson(result)
            context.Response.Write(json)

        Catch ex As Exception
            Dim errorResult = New With {
                .success = False,
                .message = "An error occurred",
                .error = ex.Message
            }
            context.Response.Write(SerializeToJson(errorResult))
        End Try
    End Sub

    Private Function GetCurrentUser(context As HttpContext) As Object
        If context.Request.IsAuthenticated Then
            Return New With {
                .success = True,
                .data = New With {
                    .username = context.User.Identity.Name,
                    .isAuthenticated = True,
                    .roles = GetUserRoles(context),
                    .lastLogin = DateTime.Now
                }
            }
        Else
            Return New With {
                .success = False,
                .message = "User not authenticated"
            }
        End If
    End Function

    Private Function GetProfile(context As HttpContext) As Object
        If Not context.Request.IsAuthenticated Then
            Return New With {
                .success = False,
                .message = "Authentication required"
            }
        End If

        Return New With {
            .success = True,
            .data = New With {
                .username = context.User.Identity.Name,
                .email = context.User.Identity.Name & "@example.com",
                .fullName = "User " & context.User.Identity.Name,
                .lastLogin = DateTime.Now.AddHours(-2),
                .profileComplete = True,
                .preferences = New With {
                    .theme = "light",
                    .language = "en-US",
                    .notifications = True
                }
            }
        }
    End Function

    Private Function Login(context As HttpContext) As Object
        Dim username As String = If(context.Request.Form("username"), "")
        Dim password As String = If(context.Request.Form("password"), "")
        
        If String.IsNullOrEmpty(username) Or String.IsNullOrEmpty(password) Then
            Return New With {
                .success = False,
                .message = "Username and password are required"
            }
        End If

        ' Simple validation example - replace with real authentication logic
        If username = "demo" AndAlso password = "password" Then
            ' Set authentication cookie
            FormsAuthentication.SetAuthCookie(username, False)
            
            Return New With {
                .success = True,
                .message = "Login successful",
                .data = New With {
                    .username = username,
                    .token = GenerateSessionToken(),
                    .expiresAt = DateTime.Now.AddHours(8)
                }
            }
        Else
            Return New With {
                .success = False,
                .message = "Invalid username or password"
            }
        End If
    End Function

    Private Function Logout(context As HttpContext) As Object
        If Not context.Request.IsAuthenticated Then
            Return New With {
                .success = False,
                .message = "User not logged in"
            }
        End If

        FormsAuthentication.SignOut()
        
        Return New With {
            .success = True,
            .message = "Logged out successfully"
        }
    End Function

    Private Function Register(context As HttpContext) As Object
        Dim username As String = If(context.Request.Form("username"), "")
        Dim email As String = If(context.Request.Form("email"), "")
        Dim password As String = If(context.Request.Form("password"), "")
        Dim confirmPassword As String = If(context.Request.Form("confirmPassword"), "")
        
        ' Validation
        Dim errors As New List(Of String)
        
        If String.IsNullOrEmpty(username) Then errors.Add("Username is required")
        If String.IsNullOrEmpty(email) Then errors.Add("Email is required")
        If String.IsNullOrEmpty(password) Then errors.Add("Password is required")
        If password <> confirmPassword Then errors.Add("Passwords do not match")
        
        If errors.Count > 0 Then
            Return New With {
                .success = False,
                .message = "Validation failed",
                .errors = errors
            }
        End If

        ' In a real implementation, you would check if user exists and create new user
        Return New With {
            .success = True,
            .message = "User registered successfully",
            .data = New With {
                .username = username,
                .email = email,
                .created = DateTime.Now,
                .emailVerified = False
            }
        }
    End Function

    Private Function ChangePassword(context As HttpContext) As Object
        If Not context.Request.IsAuthenticated Then
            Return New With {
                .success = False,
                .message = "Authentication required"
            }
        End If

        Dim currentPassword As String = If(context.Request.Form("currentPassword"), "")
        Dim newPassword As String = If(context.Request.Form("newPassword"), "")
        Dim confirmPassword As String = If(context.Request.Form("confirmPassword"), "")
        
        Dim errors As New List(Of String)
        
        If String.IsNullOrEmpty(currentPassword) Then errors.Add("Current password is required")
        If String.IsNullOrEmpty(newPassword) Then errors.Add("New password is required")
        If newPassword <> confirmPassword Then errors.Add("New passwords do not match")
        
        If errors.Count > 0 Then
            Return New With {
                .success = False,
                .message = "Validation failed",
                .errors = errors
            }
        End If

        Return New With {
            .success = True,
            .message = "Password changed successfully",
            .timestamp = DateTime.Now
        }
    End Function

    Private Function UpdateProfile(context As HttpContext) As Object
        If Not context.Request.IsAuthenticated Then
            Return New With {
                .success = False,
                .message = "Authentication required"
            }
        End If

        Dim fullName As String = If(context.Request.Form("fullName"), "")
        Dim email As String = If(context.Request.Form("email"), "")
        Dim theme As String = If(context.Request.Form("theme"), "light")
        
        Return New With {
            .success = True,
            .message = "Profile updated successfully",
            .data = New With {
                .username = context.User.Identity.Name,
                .fullName = fullName,
                .email = email,
                .preferences = New With {
                    .theme = theme
                },
                .modified = DateTime.Now
            }
        }
    End Function

    Private Function GetUserRoles(context As HttpContext) As List(Of String)
        Dim roles As New List(Of String)
        
        If context.User.Identity.IsAuthenticated Then
            roles.Add("User")
            If context.User.Identity.Name = "admin" Then
                roles.Add("Administrator")
            End If
        End If
        
        Return roles
    End Function

    Private Function GenerateSessionToken() As String
        Return Guid.NewGuid().ToString().Replace("-", "")
    End Function

    Private Function SerializeToJson(obj As Object) As String
        Dim serializer As New JavaScriptSerializer()
        Return serializer.Serialize(obj)
    End Function

End Class