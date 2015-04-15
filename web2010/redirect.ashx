<%@ WebHandler Language="VB" Class="redirect" %>
Imports System
Imports System.Web

Public Class redirect : Implements IHttpHandler
    Dim RedirectURL As String
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If Not IsNothing(context.Request.QueryString.GetValues("u")) Then
            RedirectURL = context.Request.QueryString.Item("u")
        End If
        context.Response.Redirect(RedirectURL)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class