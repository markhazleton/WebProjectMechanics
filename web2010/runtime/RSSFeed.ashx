<%@ WebHandler Language="VB" Class="RSSFeed" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Imports WebProjectMechanics
Imports System.Xml

Public Class RSSFeed
    Inherits BaseHandler
    
    Public Function GetRSS() As Object
        SkipContentTypeEvaluation = True
        ' the handler won't try to identify the content type automatically
        SkipDefaultSerialization = True
        ' the handler won't serialize the result to JSON automatically
        context.Response.ContentType = "text/xml"
        Dim myCompany As New ActiveCompany()
        Return myCompany.LocationList.GetRSS().ToString()
    End Function
   
    Public Function GetArticleRSS() As Object
        SkipContentTypeEvaluation = True
        ' the handler won't try to identify the content type automatically
        SkipDefaultSerialization = True
        ' the handler won't serialize the result to JSON automatically
        context.Response.ContentType = "text/xml"
        Dim myCompany As New ActiveCompany()
        Return myCompany.ArticleList.GetRSS().ToString()
    End Function
    
      
    
End Class