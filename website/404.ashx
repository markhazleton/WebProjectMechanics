<%@ WebHandler Language="VB" Class="_404" %>

Imports System
Imports System.Web
Imports WebProjectMechanics

Public Class _404 : Implements IHttpHandler : Implements IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim myCompany As ActiveCompany = New ActiveCompany()
        Dim sPageURL As String = String.Empty
        Dim strNotFound As String = GetStringNotFound(context)

        ' **********************************************************************************************
        ' ***                                                                                        ***
        ' ***       Check For Known Paths with ProcessServerTransfers                                ***
        ' ***                                                                                        ***
        ' **********************************************************************************************
        If Not ProcessServerTransfers(context, strNotFound) Then
            ' Check for loop, make sure same page is not returned more than once
            If context.Session("wpm_Last404") = strNotFound Then
                sPageURL = String.Empty
                Create404Response(context, myCompany)
            Else
                context.Session("wpm_Last404") = strNotFound
                sPageURL = myCompany.SetCurrentLocation(strNotFound, True)

            End If
            ' **********************************************************************************************
            ' ***                                                                                        ***
            ' **********************************************************************************************
            If sPageURL <> String.Empty Then
                HttpContext.Current.Session("wpm_Last404") = String.Empty
                If myCompany.CurLocation.LocationTypeCD = "NOFILENAME" Then
                    With myCompany
                        If Left(.CurLocation.TransferURL, 4) = "http" Then
                            HttpContext.Current.Response.Redirect(.CurLocation.TransferURL, True)
                        Else
                            If String.IsNullOrEmpty(.CurLocation.TransferURL) Then
                                ' Need to check for global Page Alias
                                .CurLocation.TransferURL = .LocationAliasList.LookupTargetURL(String.Empty)
                                If String.IsNullOrEmpty(.CurLocation.TransferURL) Then
                                    HttpContext.Current.Response.Write(.GetArticlePageHTML())
                                Else
                                    HttpContext.Current.Response.Redirect(.CurLocation.TransferURL, True)
                                End If
                            Else
                                HttpContext.Current.Server.Transfer(.CurLocation.TransferURL, True)
                            End If
                        End If
                    End With
                Else
                    myCompany.WriteCurrentLocation()
                End If
            Else
                If Not ProcessRedirect(context, myCompany, strNotFound) Then
                    If Not ProcessServerTransfers(context, strNotFound) Then
                        Create404Response(context, myCompany)
                    End If
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Shared Function GetStringNotFound(ByVal context As HttpContext) As String
        Dim strNotFound As String
        Dim myURIBuilder As UriBuilder = New UriBuilder(context.Request.Url)
        context.Session("wpm_RequestURL") = context.Request.Url
        strNotFound = context.Request.Url.ToString()
        Dim strHost As String = String.Format("{0}:{1}", myURIBuilder.Host, myURIBuilder.Port)
        Dim sModuleURL As String
        If myURIBuilder.Port = 80 Then
            sModuleURL = String.Format("{0}{1}{2}/404.ashx?404;{0}{1}{2}:{3}", myURIBuilder.Uri.Scheme, Uri.SchemeDelimiter, myURIBuilder.Host, myURIBuilder.Port)
        Else
            sModuleURL = String.Format("{0}{1}{2}:{3}/404.ashx?404;{0}{1}{2}:{3}", myURIBuilder.Uri.Scheme, Uri.SchemeDelimiter, myURIBuilder.Host, myURIBuilder.Port)
        End If
        strNotFound = Replace(strNotFound, sModuleURL, String.Empty)

        If (strNotFound = context.Request.Url.ToString) Then
            strNotFound = strNotFound.Substring(strNotFound.LastIndexOf(":80/") + 3)
        End If


        strNotFound = RemoveQueryString(strNotFound)
        Return strNotFound
    End Function

    Private Shared Function RemoveQueryString(ByVal strPath As String) As String
        Dim iQuestionMark As Integer = InStr(strPath, "?")
        If (iQuestionMark > 0) Then
            strPath = Left(strPath, iQuestionMark - 1)
        End If
        Return strPath
    End Function

    Private Shared Function SetMainPage(ByVal sMainPageID As String, ByVal sMainPageName As String, ByRef myCompany As ActiveCompany) As Boolean
        myCompany.CurLocation.MainMenuLocationID = sMainPageID
        myCompany.CurLocation.MainMenuLocationName = sMainPageName
        Return True
    End Function

    Private Shared Function ProcessServerTransfers(ByRef context As HttpContext, ByVal strNotFound As String) As Boolean
        Dim bReturn As Boolean = True
        If (Right(strNotFound, 4) = ".gif") Then
            context.Server.Transfer("/images/FileNotFound.png")
            ' context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
        ElseIf (Right(strNotFound, 4) = ".png") Then
            context.Server.Transfer("/images/FileNotFound.png")
            ' context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
        ElseIf (Right(strNotFound, 4) = ".jpg") Then
            context.Server.Transfer("/images/FileNotFound.png")
            ' context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
            ' context.Server.TransferRequest(String.Format("/runtime/catalog/findimage.ashx?img={0}", strNotFound))
        ElseIf (Right(strNotFound, 5) = ".jpeg") Then
            context.Server.Transfer("/images/FileNotFound.png")
            'context.Response.Redirect("/images/FileNotFound.png")
            ' context.Server.Transfer(String.Format("/runtime/catalog/GenericImage.aspx?Path={0}", strNotFound))
        ElseIf (Right(strNotFound, 4) = ".css") Then
            context.Server.Transfer("/runtime/css/blank.css")
        ElseIf (Right(strNotFound, 11) = "sitemap.xml") Then
            context.Server.Transfer(String.Format("{0}sitemap.aspx", wpm_SiteConfig.ApplicationHome()))
        ElseIf (Right(strNotFound, 12) = "sitemap_view") Then
            context.Server.Transfer(String.Format("{0}sitemap_view.aspx", wpm_SiteConfig.ApplicationHome()))
        ElseIf (Right(strNotFound, 7) = "sitemap") Then
            context.Server.Transfer(String.Format("{0}sitemap.aspx", wpm_SiteConfig.ApplicationHome()))
        ElseIf (Right(strNotFound, 12) = "rss_menu.xml") Then
            context.Server.Transfer(String.Format("{0}rss_menu.aspx", wpm_SiteConfig.ApplicationHome()))
        Else
            bReturn = False
        End If
        Return bReturn
    End Function

    Private Shared Function ProcessRedirect(ByRef context As HttpContext, ByRef myCompany As ActiveCompany, ByVal strNotFound As String) As Boolean
        Dim sRedirectURL As String = String.Empty
        Dim bReturn As Boolean = True

        If strNotFound = "/l.php" Then
            context.Response.Redirect(Replace(Replace(Replace(context.Request.QueryString.Item(0), "404;", ""), "{", ""), "}", ""))
        End If

        sRedirectURL = myCompany.LocationAliasList.LookupTargetURL(strNotFound)

        If String.IsNullOrEmpty(sRedirectURL) Then
            sRedirectURL = myCompany.SetCurrentLocation(strNotFound, False)
        End If


        If strNotFound = "/l.php" Then
            sRedirectURL = Replace(Replace(Replace(context.Request.QueryString.Item(0), "404;", ""), "{", ""), "}", "")
        End If

        If sRedirectURL = String.Empty Then
            If (Right(strNotFound, 4) = ".php") Then
                sRedirectURL = "http://php.net/"
            ElseIf (Right(strNotFound, 4) = ".asp") Then
                sRedirectURL = "http://asp.net/"
            ElseIf (Right(strNotFound, 4) = ".cfc") Then
                sRedirectURL = "http://en.wikipedia.org/wiki/Adobe_ColdFusion"
            ElseIf (Right(strNotFound, 4) = ".cfm") Then
                sRedirectURL = "http://en.wikipedia.org/wiki/Adobe_ColdFusion"
            End If
        End If

        If sRedirectURL <> String.Empty Then
            wpm_Build301Redirect(sRedirectURL)
        Else
            bReturn = False
        End If
        Return bReturn
    End Function
    Private Sub Create404Response(ByVal context As HttpContext, ByVal myCompany As ActiveCompany)
        Dim strLookup As String = context.Request.Url.ToString
        Dim strNotFound As String = strLookup.Substring(strLookup.LastIndexOf(":80/") + 4)



        context.Response.Status = "404 Not Found"
        context.Response.StatusCode = 404
        Dim myContents As New StringBuilder
        Dim sFileName As New StringBuilder
        If Not (HttpContext.Current.Request.QueryString.Count = 0) Then
            sFileName.Append(Replace(HttpContext.Current.Request.QueryString.Item(0), "404;", ""))
        Else
            sFileName.Append("?")
        End If
        myContents.Replace("<RequestURL>", sFileName.ToString)
        myContents.Replace("<UserHostAddress>", HttpContext.Current.Request.UserHostAddress)
        myContents.Replace("<UserLanguages>", HttpContext.Current.Request.UserAgent)
        myContents.Replace("<RequestBrowser>", HttpContext.Current.Request.Browser.Browser)
        wpm_AddHTMLHead = myContents.ToString
        context.Response.Write(myCompany.GetHTML(String.Format("<blockquote>The page you were looking for can not be found</blockquote><br/><strong>{0}</strong><br/><br/>Not Found:{1}<br/>Lookup:{2}<sitemap>", sFileName, strNotFound, strLookup), False, wpm_SiteTemplatePrefix))
    End Sub
End Class