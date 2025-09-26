<%@ WebHandler Language="VB" Class="LocationApiHandler" %>

Imports System
Imports System.Web
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.SessionState

Public Class LocationApiHandler
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
                .message = "An error occurred processing the request",
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
                Case "getlocation"
                    Dim id As Integer = If(context.Request.QueryString("id") IsNot Nothing, Integer.Parse(context.Request.QueryString("id")), 0)
                    Return GetLocation(id)
                Case "searchlocations"
                    Dim query As String = If(context.Request.QueryString("query"), "")
                    Dim country As String = If(context.Request.QueryString("country"), "")
                    Dim state As String = If(context.Request.QueryString("state"), "")
                    Return SearchLocations(query, country, state)
                Case "getnearbylocations"
                    Dim lat As Double = If(context.Request.QueryString("latitude") IsNot Nothing, Double.Parse(context.Request.QueryString("latitude")), 0)
                    Dim lng As Double = If(context.Request.QueryString("longitude") IsNot Nothing, Double.Parse(context.Request.QueryString("longitude")), 0)
                    Dim radius As Double = If(context.Request.QueryString("radius") IsNot Nothing, Double.Parse(context.Request.QueryString("radius")), 10)
                    Return GetNearbyLocations(lat, lng, radius)
                Case Else
                    Return New With {.success = False, .message = "Unknown method"}
            End Select
        End If

        ' Default GET behavior - return paginated list of locations
        Dim page As Integer = If(context.Request.QueryString("page") IsNot Nothing, Integer.Parse(context.Request.QueryString("page")), 1)
        Dim pageSize As Integer = If(context.Request.QueryString("pageSize") IsNot Nothing, Integer.Parse(context.Request.QueryString("pageSize")), 10)
        Dim search As String = If(context.Request.QueryString("search"), "")

        Return New With {
            .success = True,
            .data = New List(Of Object) From {
                New With {.id = 1, .name = "New York", .state = "NY", .country = "USA", .latitude = 40.7128, .longitude = -74.0060},
                New With {.id = 2, .name = "Los Angeles", .state = "CA", .country = "USA", .latitude = 34.0522, .longitude = -118.2437},
                New With {.id = 3, .name = "Chicago", .state = "IL", .country = "USA", .latitude = 41.8781, .longitude = -87.6298}
            },
            .pagination = New With {
                .page = page,
                .pageSize = pageSize,
                .totalResults = 3,
                .totalPages = 1
            },
            .searchQuery = search
        }
    End Function

    Private Function HandlePost(context As HttpContext) As Object
        Try
            Dim name As String = If(context.Request.Form("name"), "")
            Dim state As String = If(context.Request.Form("state"), "")
            Dim country As String = If(context.Request.Form("country"), "")
            Dim latitude As Double = If(context.Request.Form("latitude") IsNot Nothing, Double.Parse(context.Request.Form("latitude")), 0)
            Dim longitude As Double = If(context.Request.Form("longitude") IsNot Nothing, Double.Parse(context.Request.Form("longitude")), 0)

            If String.IsNullOrEmpty(name) Then
                Return New With {
                    .success = False,
                    .message = "Location name is required",
                    .errors = New List(Of String) From {"Name field cannot be empty"}
                }
            End If

            Return New With {
                .success = True,
                .message = "Location created successfully",
                .data = New With {
                    .id = New Random().Next(1000, 9999),
                    .name = name,
                    .state = state,
                    .country = country,
                    .latitude = latitude,
                    .longitude = longitude,
                    .created = DateTime.Now
                }
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error creating location",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function HandlePut(context As HttpContext) As Object
        Try
            Dim id As Integer = If(context.Request.Form("id") IsNot Nothing, Integer.Parse(context.Request.Form("id")), 0)
            Dim name As String = If(context.Request.Form("name"), "")
            Dim state As String = If(context.Request.Form("state"), "")
            Dim country As String = If(context.Request.Form("country"), "")

            If id <= 0 Then
                Return New With {
                    .success = False,
                    .message = "Valid location ID is required"
                }
            End If

            Return New With {
                .success = True,
                .message = "Location updated successfully",
                .data = New With {
                    .id = id,
                    .name = name,
                    .state = state,
                    .country = country,
                    .modified = DateTime.Now
                }
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error updating location",
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
                    .message = "Valid location ID is required"
                }
            End If

            Return New With {
                .success = True,
                .message = "Location " & id.ToString() & " deleted successfully",
                .timestamp = DateTime.Now
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = "Error deleting location",
                .error = ex.Message
            }
        End Try
    End Function

    Private Function GetLocation(id As Integer) As Object
        If id <= 0 Then
            Return New With {
                .success = False,
                .message = "Valid location ID is required"
            }
        End If

        Return New With {
            .success = True,
            .data = New With {
                .id = id,
                .name = "Location " & id.ToString(),
                .state = "Sample State",
                .country = "Sample Country",
                .latitude = 40.7128 + (id * 0.1),
                .longitude = -74.0060 + (id * 0.1),
                .created = DateTime.Now.AddDays(-id),
                .modified = DateTime.Now
            }
        }
    End Function

    Private Function SearchLocations(query As String, Optional country As String = "", Optional state As String = "") As Object
        Return New With {
            .success = True,
            .searchCriteria = New With {
                .query = query,
                .country = country,
                .state = state
            },
            .results = New List(Of Object) From {
                New With {.id = 1, .name = "Search result for: " & query, .state = state, .country = country, .relevance = 0.95},
                New With {.id = 2, .name = "Another match: " & query, .state = state, .country = country, .relevance = 0.87}
            },
            .totalResults = 2
        }
    End Function

    Private Function GetNearbyLocations(latitude As Double, longitude As Double, Optional radiusKm As Double = 10) As Object
        Return New With {
            .success = True,
            .center = New With {.latitude = latitude, .longitude = longitude},
            .radiusKm = radiusKm,
            .locations = New List(Of Object) From {
                New With {
                    .id = 1,
                    .name = "Nearby Location 1",
                    .latitude = latitude + 0.01,
                    .longitude = longitude + 0.01,
                    .distanceKm = 1.2
                },
                New With {
                    .id = 2,
                    .name = "Nearby Location 2",
                    .latitude = latitude - 0.02,
                    .longitude = longitude + 0.02,
                    .distanceKm = 2.8
                }
            }
        }
    End Function

    Private Function SerializeToJson(obj As Object) As String
        Dim serializer As New JavaScriptSerializer()
        Return serializer.Serialize(obj)
    End Function

End Class