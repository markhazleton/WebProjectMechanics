<%@ WebHandler Language="VB" Class="KeywordJavaScript" %>

Imports System
Imports System.Web
Imports WebProjectMechanics

Public Class KeywordJavaScript : Implements IHttpHandler : Implements IRequiresSessionState
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        context.Response.Write(ProcessKeywordSearch("/util/sites/informationcenter/"))
    End Sub
     
    Private Function ProcessKeywordSearch(ByVal SiteGallery As String) As String
        Dim myCompany As ActiveCompany
        myCompany = New ActiveCompany()

        Dim myReturn As New StringBuilder(String.Empty)
        For Each myString As LocationKeyword In myCompany.LocationList.KeywordList
            myReturn.Append(myString.Code & ",")
        Next
        Dim myContents As New StringBuilder
        FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/search/searchfield.js", SiteGallery)), myContents)
        myContents.Replace("<keywordlist>", myReturn.ToString)
        Return myContents.ToString
    End Function
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class