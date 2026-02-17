<%@ WebHandler Language="VB" Class="CompanyApiHandler" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.SessionState

Public Class CompanyApiHandler
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

            ' Handle specific method calls
            If Not String.IsNullOrEmpty(methodName) Then
                Select Case methodName.ToLower()
                    Case "getcompany"
                        Dim id As Integer = If(context.Request.QueryString("id") IsNot Nothing, Integer.Parse(context.Request.QueryString("id")), 0)
                        result = GetCompany(id)
                    Case "getcompanyconfig"
                        Dim domain As String = If(context.Request.QueryString("domain"), "")
                        result = GetCompanyConfig(domain)
                    Case "getcompanyanalytics"
                        If Not context.Request.IsAuthenticated Then
                            result = New With {.success = False, .message = "Authentication required"}
                        Else
                            Dim id As Integer = If(context.Request.QueryString("id") IsNot Nothing, Integer.Parse(context.Request.QueryString("id")), 0)
                            Dim days As Integer = If(context.Request.QueryString("days") IsNot Nothing, Integer.Parse(context.Request.QueryString("days")), 30)
                            result = GetCompanyAnalytics(id, days)
                        End If
                    Case "searchcompanies"
                        Dim query As String = If(context.Request.QueryString("query"), "")
                        Dim isActive As Boolean? = If(context.Request.QueryString("isActive") IsNot Nothing, Boolean.Parse(context.Request.QueryString("isActive")), Nothing)
                        Dim limit As Integer = If(context.Request.QueryString("limit") IsNot Nothing, Integer.Parse(context.Request.QueryString("limit")), 20)
                        result = SearchCompanies(query, isActive, limit)
                    Case Else
                        result = New With {.success = False, .message = "Unknown method"}
                End Select
            Else
                ' Handle standard HTTP verbs
                Select Case method
                    Case "GET"
                        result = HandleGet(context)
                    Case "POST"
                        result = HandlePost(context)
                    Case "PUT"
                        result = HandlePut(context)
                    Case "DELETE"
                        result = HandleDelete(context)
                    Case "OPTIONS"
                        Return
                    Case Else
                        result = New With {.success = False, .message = "Method not allowed"}
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

    Private Function HandleGet(context As HttpContext) As Object
        Try
            Dim page As Integer = If(context.Request.QueryString("page") IsNot Nothing, Integer.Parse(context.Request.QueryString("page")), 1)
            Dim pageSize As Integer = If(context.Request.QueryString("pageSize") IsNot Nothing, Integer.Parse(context.Request.QueryString("pageSize")), 10)
            Dim search As String = If(context.Request.QueryString("search"), "")
            Dim active As Boolean? = If(context.Request.QueryString("active") IsNot Nothing, Boolean.Parse(context.Request.QueryString("active")), Nothing)

            ' Generate sample data
            Dim companies As New List(Of Object)
            For i As Integer = 1 To Math.Min(pageSize, 20)
                companies.Add(New With {
                    .id = i,
                    .name = "Sample Company " & i.ToString(),
                    .domain = "company" & i.ToString() & ".com",
                    .isActive = (i Mod 2 = 0),
                    .created = DateTime.Now.AddDays(-i * 30),
                    .contactInfo = New With {
                        .email = "info@company" & i.ToString() & ".com",
                        .phone = "555-" & i.ToString("D4"),
                        .address = (i * 100).ToString() & " Business Ave"
                    }
                })
            Next

            Return New With {
                .success = True,
                .data = companies,
                .pagination = New With {
                    .page = page,
                    .pageSize = pageSize,
                    .totalResults = 100,
                    .totalPages = Math.Ceiling(100 / pageSize)
                },
                .filters = New With {
                    .search = search,
                    .active = active
                }
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error retrieving companies",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function HandlePost(context As HttpContext) As Object
        Try
            Dim name As String = If(context.Request.Form("name"), "")
            Dim domain As String = If(context.Request.Form("domain"), "")
            Dim email As String = If(context.Request.Form("email"), "")
            Dim phone As String = If(context.Request.Form("phone"), "")
            Dim address As String = If(context.Request.Form("address"), "")

            ' Validation
            Dim errors As New List(Of String)
            If String.IsNullOrEmpty(name) Then errors.Add("Company name is required")
            If String.IsNullOrEmpty(domain) Then errors.Add("Domain is required")
            If Not String.IsNullOrEmpty(email) AndAlso Not IsValidEmail(email) Then errors.Add("Invalid email format")

            If errors.Count > 0 Then
                Return New With {
                    .success = False,
                    .message = "Validation failed",
                    .errors = errors
                }
            End If

            Dim newCompany = New With {
                .id = New Random().Next(1000, 9999),
                .name = name,
                .domain = domain,
                .isActive = True,
                .created = DateTime.Now,
                .contactInfo = New With {
                    .email = email,
                    .phone = phone,
                    .address = address
                }
            }

            Return New With {
                .success = True,
                .message = "Company created successfully",
                .data = newCompany
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error creating company",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function HandlePut(context As HttpContext) As Object
        Try
            Dim id As Integer = If(context.Request.Form("id") IsNot Nothing, Integer.Parse(context.Request.Form("id")), 0)
            Dim name As String = If(context.Request.Form("name"), "")
            Dim domain As String = If(context.Request.Form("domain"), "")
            Dim isActive As Boolean = If(context.Request.Form("isActive") IsNot Nothing, Boolean.Parse(context.Request.Form("isActive")), True)

            If id <= 0 Then
                Return New With {
                    .success = False,
                    .message = "Valid company ID is required"
                }
            End If

            Return New With {
                .success = True,
                .message = "Company updated successfully",
                .data = New With {
                    .id = id,
                    .name = name,
                    .domain = domain,
                    .isActive = isActive,
                    .modified = DateTime.Now
                }
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error updating company",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function HandleDelete(context As HttpContext) As Object
        Try
            Dim id As Integer = If(context.Request.Form("id") IsNot Nothing, Integer.Parse(context.Request.Form("id")), 0)

            If id <= 0 Then
                Return New With {
                    .success = False,
                    .message = "Valid company ID is required"
                }
            End If

            Return New With {
                .success = True,
                .message = "Company " & id.ToString() & " deleted successfully",
                .timestamp = DateTime.Now
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error deleting company",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function GetCompany(id As Integer) As Object
        If id <= 0 Then
            Return New With {
                .success = False,
                .message = "Valid company ID is required"
            }
        End If

        Return New With {
            .success = True,
            .data = New With {
                .id = id,
                .name = "Company " & id.ToString(),
                .domain = "company" & id.ToString() & ".com",
                .isActive = True,
                .created = DateTime.Now.AddDays(-id * 10),
                .modified = DateTime.Now,
                .contactInfo = New With {
                    .email = "info@company" & id.ToString() & ".com",
                    .phone = "555-" & id.ToString("D4"),
                    .address = (id * 100).ToString() & " Business Ave"
                },
                .statistics = New With {
                    .totalLocations = id * 3,
                    .activeProjects = id,
                    .lastActivity = DateTime.Now.AddHours(-id)
                }
            }
        }
    End Function

    Private Function GetCompanyConfig(domain As String) As Object
        If String.IsNullOrEmpty(domain) Then
            Return New With {
                .success = False,
                .message = "Domain is required"
            }
        End If

        Return New With {
            .success = True,
            .data = New With {
                .domain = domain,
                .companyName = "Company for " & domain,
                .configuration = New With {
                    .removeWWW = True,
                    .applicationHome = "/",
                    .defaultTheme = "default",
                    .enableSSL = True,
                    .maintenanceMode = False
                },
                .features = New With {
                    .locationsEnabled = True,
                    .articlesEnabled = True,
                    .userRegistration = True,
                    .publicAPI = True
                }
            }
        }
    End Function

    Private Function GetCompanyAnalytics(id As Integer, Optional days As Integer = 30) As Object
        Return New With {
            .success = True,
            .data = New With {
                .companyId = id,
                .period = New With {
                    .days = days,
                    .startDate = DateTime.Now.AddDays(-days),
                    .endDate = DateTime.Now
                },
                .metrics = New With {
                    .totalVisitors = New Random().Next(1000, 10000),
                    .pageViews = New Random().Next(5000, 50000),
                    .newLocations = New Random().Next(1, 50),
                    .activeUsers = New Random().Next(10, 100)
                },
                .topPages = New List(Of Object) From {
                    New With {.page = "/locations", .views = 1500},
                    New With {.page = "/articles", .views = 1200},
                    New With {.page = "/", .views = 800}
                },
                .trafficSources = New With {
                    .direct = 45,
                    .search = 30,
                    .social = 15,
                    .referral = 10
                }
            }
        }
    End Function

    Private Function SearchCompanies(query As String, Optional isActive As Boolean? = Nothing, Optional limit As Integer = 20) As Object
        Return New With {
            .success = True,
            .searchQuery = query,
            .filters = New With {
                .isActive = isActive,
                .limit = limit
            },
            .results = New List(Of Object) From {
                New With {
                    .id = 1,
                    .name = "Search Match: " & query,
                    .domain = query.ToLower().Replace(" ", "") & ".com",
                    .relevance = 0.95,
                    .isActive = True
                },
                New With {
                    .id = 2,
                    .name = "Related Company: " & query,
                    .domain = "related-" & query.ToLower().Replace(" ", "") & ".com",
                    .relevance = 0.78,
                    .isActive = True
                }
            },
            .totalResults = 2
        }
    End Function

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr As New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Function SerializeToJson(obj As Object) As String
        Dim serializer As New JavaScriptSerializer()
        Return serializer.Serialize(obj)
    End Function

End Class