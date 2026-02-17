<%@ WebHandler Language="VB" Class="SampleApiHandler" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.SessionState

Public Class SampleApiHandler
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
            Dim result As Object = Nothing

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
                    ' Handle preflight CORS requests
                    Return
                Case Else
                    result = New With {
                        .success = False,
                        .message = "Method not allowed"
                    }
            End Select

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
        ' Check if a specific method is being called
        Dim methodName As String = context.Request.QueryString("method")
        
        If Not String.IsNullOrEmpty(methodName) Then
            Select Case methodName.ToLower()
                Case "getitem"
                    Dim id As Integer = If(context.Request.QueryString("id") IsNot Nothing, Integer.Parse(context.Request.QueryString("id")), 0)
                    Return GetItem(id)
                Case "searchitems"
                    Dim query As String = If(context.Request.QueryString("query"), "")
                    Return SearchItems(query)
                Case Else
                    Return New With {.success = False, .message = "Unknown method"}
            End Select
        End If

        ' Default GET behavior - return list of items
        Return New With {
            .success = True,
            .message = "Hello from Sample API",
            .data = New List(Of Object) From {
                New With {.id = 1, .name = "Item 1", .description = "First sample item"},
                New With {.id = 2, .name = "Item 2", .description = "Second sample item"}
            },
            .timestamp = DateTime.Now
        }
    End Function

    Private Function HandlePost(context As HttpContext) As Object
        Dim name As String = If(context.Request.Form("name"), "")
        Dim description As String = If(context.Request.Form("description"), "")
        
        Return New With {
            .success = True,
            .message = "Item created successfully",
            .data = New With {
                .id = New Random().Next(100, 999),
                .name = name,
                .description = description,
                .created = DateTime.Now
            }
        }
    End Function

    Private Function HandlePut(context As HttpContext) As Object
        Dim id As String = If(context.Request.Form("id"), "0")
        Dim name As String = If(context.Request.Form("name"), "")
        
        Return New With {
            .success = True,
            .message = "Item updated successfully",
            .data = New With {
                .id = Integer.Parse(id),
                .name = name,
                .modified = DateTime.Now
            }
        }
    End Function

    Private Function HandleDelete(context As HttpContext) As Object
        Dim id As String = If(context.Request.Form("id"), "0")
        
        Return New With {
            .success = True,
            .message = "Item " & id & " deleted successfully",
            .timestamp = DateTime.Now
        }
    End Function

    Private Function GetItem(id As Integer) As Object
        Return New With {
            .success = True,
            .data = New With {
                .id = id,
                .name = "Item " & id.ToString(),
                .description = "Description for item " & id.ToString(),
                .created = DateTime.Now.AddDays(-id)
            }
        }
    End Function

    Private Function SearchItems(query As String) As Object
        Return New With {
            .success = True,
            .query = query,
            .results = New List(Of Object) From {
                New With {.id = 1, .name = "Result for: " & query, .relevance = 0.95},
                New With {.id = 2, .name = "Another match: " & query, .relevance = 0.87}
            },
            .totalResults = 2
        }
    End Function

    Private Function SerializeToJson(obj As Object) As String
        Dim serializer As New JavaScriptSerializer()
        Return serializer.Serialize(obj)
    End Function

End Class