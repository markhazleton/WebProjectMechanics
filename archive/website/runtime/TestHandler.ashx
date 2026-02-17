<%@ WebHandler Language="VB" Class="TestHandler" %>

Imports System
Imports System.Web

Public Class TestHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim PageName = WebProjectMechanics.wpm_GetProperty("PageName",String.Empty)
        Dim PageKeywords = WebProjectMechanics.wpm_GetProperty("Keywords",String.Empty)
        
        
        context.Response.ContentType = "text/plain"
        context.Response.Write(String.Format("<h2>Page Name:{0}</h2>",PageName))
        context.Response.Write(String.Format("<h2>Page Keywords:{0}</h2>",PageKeywords))
                               
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class