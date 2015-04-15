<%@ WebHandler Language="VB" Class="_404" %>

Imports System
Imports System.Web
Imports WebProjectMechanics

Public Class _404 : Implements IHttpHandler : Implements IRequiresSessionState
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim myCompany As ActiveCompany = New ActiveCompany()
        Dim sbTest As New StringBuilder(String.Empty)
        Dim sPath As String = context.Request.Path
        Dim strNotFound As New String(String.Empty)
        Dim bFoundLocation As Boolean = True
        Dim sTransferURL As String = String.Empty
        Dim sRedirectURL As String = String.Empty
       
        With myCompany.CurLocation
            .LocationTypeCD = "404"
            .LocationID = ""
            .ArticleID = 0
            .TransferURL = ""
            .MainMenuLocationID = ""
            .LocationName = wpm_FileNotFound
            .LocationTitle = wpm_FileNotFound
        End With

        Dim myURIBuilder As UriBuilder = New UriBuilder(context.Request.Url)
        
        HttpContext.Current.Session("wpm_RequestURL") = context.Request.Url
        strNotFound = HttpContext.Current.Request.Url.ToString()
        
        Dim strHost As String = String.Format("{0}:{1}", myURIBuilder.Host, myURIBuilder.Port)
                        
        Dim sModuleURL As String
        If myURIBuilder.Port = 80 Then
            sModuleURL = String.Format("{0}{1}{2}/404.ashx?404;{0}{1}{2}:{3}", myURIBuilder.Uri.Scheme, Uri.SchemeDelimiter, myURIBuilder.Host, myURIBuilder.Port)
        Else
            sModuleURL = String.Format("{0}{1}{2}:{3}/404.ashx?404;{0}{1}{2}:{3}", myURIBuilder.Uri.Scheme, Uri.SchemeDelimiter, myURIBuilder.Host, myURIBuilder.Port)
        End If
                
        strNotFound = Replace(strNotFound, sModuleURL, String.Empty)
        strNotFound = RemoveQueryString(strNotFound)
                
        If HttpContext.Current.Session("wpm_Last404") = strNotFound Then
            HttpContext.Current.Response.Redirect("/")
        Else
            HttpContext.Current.Session("wpm_Last404") = strNotFound
                    
            If strNotFound = "/l.php" Then
                context.Response.Redirect(Replace(Replace(Replace(context.Request.QueryString.Item(0), "404;", ""), "{", ""), "}", ""))
            End If
            sTransferURL = myCompany.SetCurrentLocation(strNotFound, True)
        End If
            
        If sTransferURL = String.Empty Then
            bFoundLocation = False
            sRedirectURL = myCompany.SetCurrentLocation(strNotFound, False)
                
                
            If (sRedirectURL = String.Empty) Then
                ' Check for Location Alias
                sRedirectURL = myCompany.LocationAliasList.LookupTargetURL(strNotFound)
            End If
            
            If sRedirectURL = String.Empty Then
                
                If (Right(strNotFound, 11) = "sitemap.xml") Then
                    myCompany.CurLocation.LocationTypeCD = "sitemap.xml"
                    sRedirectURL = String.Format("{0}sitemap.aspx", wpm_SiteConfig.ApplicationHome())
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 12) = "sitemap_view") Then
                    myCompany.CurLocation.LocationTypeCD = "sitemap_view"
                    sRedirectURL = String.Format("{0}sitemap_view.aspx", wpm_SiteConfig.ApplicationHome())
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 7) = "sitemap") Then
                    myCompany.CurLocation.LocationTypeCD = "sitemap"
                    sRedirectURL = String.Format("{0}site.aspx", wpm_SiteConfig.ApplicationHome())
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 12) = "rss_menu.xml") Then
                    myCompany.CurLocation.LocationTypeCD = "rss_menu.xml"
                    sRedirectURL = String.Format("{0}rss_menu.aspx", wpm_SiteConfig.ApplicationHome())
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 4) = ".php") Then
                    myCompany.CurLocation.LocationTypeCD = "PHP"
                    sRedirectURL = "http://php.net/"
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 4) = ".asp") Then
                    myCompany.CurLocation.LocationTypeCD = "ASP"
                    sRedirectURL = "http://asp.net/"
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 4) = ".cfc") Then
                    myCompany.CurLocation.LocationTypeCD = "CFC"
                    sRedirectURL = "http://en.wikipedia.org/wiki/Adobe_ColdFusion"
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 4) = ".cfm") Then
                    myCompany.CurLocation.LocationTypeCD = "CFM"
                    sRedirectURL = "http://en.wikipedia.org/wiki/Adobe_ColdFusion"
                    myCompany.CurLocation.TransferURL = sRedirectURL
                ElseIf (Right(strNotFound, 4) = ".gif") Then
                    context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
                ElseIf (Right(strNotFound, 4) = ".png") Then
                    context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
                ElseIf (Right(strNotFound, 4) = ".jpg") Then
                    context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
                ElseIf (Right(strNotFound, 5) = ".jpeg") Then
                    context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
                End If
            Else
                If (sRedirectURL <> String.Empty) Then
                    wpm_Build301Redirect(sRedirectURL)
                Else
                    context.Response.Status = "404 Not Found"
                    context.Response.StatusCode = 404
                    Dim myContents As New StringBuilder
                    Dim sFileName As New StringBuilder
                    If Not (HttpContext.Current.Request.QueryString.Count = 0) Then
                        sFileName.Append(Replace(HttpContext.Current.Request.QueryString.Item(0), "404;", ""))
                    Else
                        sFileName.Append("?")
                    End If
                    ApplicationLogging.FileNotFoundLog("404.ProcessRequest", HttpContext.Current.Request.ServerVariables.Item("QUERY_STRING").Replace("404;", ""))
                    FileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("/{0}404-Text.html", wpm_SiteConfig.ApplicationHome)), myContents)
                    myContents.Replace("<RequestURL>", sFileName.ToString)
                    myContents.Replace("<UserHostAddress>", HttpContext.Current.Request.UserHostAddress)
                    myContents.Replace("<UserLanguages>", HttpContext.Current.Request.UserAgent)
                    myContents.Replace("<RequestBrowser>", HttpContext.Current.Request.Browser.Browser)
                    wpm_AddHTMLHead = myContents.ToString
                    context.Response.Write(myCompany.GetHTML(String.Format("<blockquote>The page you were looking for can not be found</blockquote><br/><strong>{0}</strong><br/><br/><form><div align=""center""><textarea rows=8 cols=60 wrap=soft></textarea></div></form><br/><br/><hr><sitemap>", sFileName), False, wpm_SiteTemplatePrefix))
                End If
            End If
        Else
            HttpContext.Current.Session("wpm_Last404") = String.Empty
            myCompany.WriteCurrentLocation()
        End If
    End Sub
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Shared Function RemoveQueryString(ByVal strPath As String) As String
        Dim iQuestionMark As Integer = InStr(strPath, "?")
        If (iQuestionMark > 0) Then
            strPath = Left(strPath, iQuestionMark - 1)
        End If
        Return strPath
    End Function

    Private Function SetMainPage(ByVal sMainPageID As String, ByVal sMainPageName As String, ByRef myCompany As ActiveCompany) As Boolean
        myCompany.CurLocation.MainMenuLocationID = sMainPageID
        myCompany.CurLocation.MainMenuLocationName = sMainPageName
        Return True
    End Function
    
End Class