Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Module App
    ' Public Shared Config As New FileConfig
    Public Const wpm_FileNotFound As String = "404 - File Not Found"
    Public Const wpm_STR_CatalogPath As String = "/runtime/catalog/"
    Private Const wpm_STR_SiteConfig As String = "wpm_SiteConfig"
    Private Const wpm_STR_ListPageURL As String = "wpm_ListPageURL"
    Private Const wpm_STR_LoginRedirectURL As String = "wpm_LoginRedirectURL"
    Private Const wpm_STR_DefaultSitePrefix As String = "wpm_DefaultSitePrefix"
    Private Const wpm_STR_SiteTemplatePrefix As String = "wpm_SiteTemplatePrefix"
    Private Const wpm_STR_RightContent As String = "wpm_RightContent"

    Private Const wpm_STR_UserGroupID As String = "wpm_UserGroupID"
    Private Const wpm_STR_ContactID As String = "wpm_UserID"
    Private Const wpm_STR_UserName As String = "wpm_UserName"
    Private Const wpm_STR_UserEmail As String = "wpm_UserEmail"
    Private Const wpm_STR_ContactName As String = "wpm_UserName"
    Private Const wpm_STR_ContactEmail As String = "wpm_UserEmail"

    Private Const wpm_STR_CurrentSQL As String = "wpm_CurrentSQL"
    Private Const wpm_STR_SiteHomePageID As String = "wpm_SiteHomePageID"
    Private Const wpm_STR_PageHistory As String = "wpm_PageHistory"
    Private Const wpm_STR_CurrentSiteName As String = "wpm_CurrentSiteName"
    Private Const wpm_STR_CurrentSiteID As String = "wpm_CurrentSiteID"
    Private Const wpm_STR_CurrentPageID As String = "wpm_CurrentPageID"
    Private Const wpm_STR_CurrentArticleID As String = "wpm_CurrentArticleID"
    Private Const wpm_STR_HTMLHead As String = "wpm_AddHTMLHead"
    Private Const wpm_STR_SiteGallery As String = "wpm_SiteGallery"
    Private Const wpm_STR_ConfigFolder As String = "wpm_ConfigFolder"

    Public ReadOnly Property wpm_SiteConfig As SiteConfig
        Get
            Try
                If CType(HttpContext.Current.Application(wpm_STR_SiteConfig), SiteConfig) Is Nothing Then
                    HttpContext.Current.Application(wpm_STR_SiteConfig) = New SiteConfig
                    Return CType(HttpContext.Current.Application(wpm_STR_SiteConfig), SiteConfig)
                Else
                    Return CType(HttpContext.Current.Application(wpm_STR_SiteConfig), SiteConfig)
                End If
            Catch
                HttpContext.Current.Application(wpm_STR_SiteConfig) = New SiteConfig
                Return CType(HttpContext.Current.Application(wpm_STR_SiteConfig), SiteConfig)
            End Try

            Return New SiteConfig
        End Get
    End Property
    Public Function wpm_SetSiteConfig(ByVal myConfig As SiteConfig) As SiteConfig
        HttpContext.Current.Session.Item(wpm_STR_SiteConfig) = myConfig
        Return wpm_SiteConfig
    End Function

    'Public Function wpm_SetDomainConfig(ByVal myDomainConfig As DomainConfiguration) As DomainConfiguration
    '    HttpContext.Current.Session.Item(wpm_STR_DomainConfig) = myDomainConfig
    '    Return wpm_DomainConfig
    'End Function
    Public ReadOnly Property wpm_DomainConfig() As DomainConfiguration
        Get
            If IsNothing(WebConfigurationManager.AppSettings.Item(wpm_STR_ConfigFolder)) Then
                WebConfigurationManager.AppSettings.Item(wpm_STR_ConfigFolder) = "~/App_Data/"
            End If
            Dim ConfigFile = String.Format("{0}sites\{1}.xml", HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item(wpm_STR_ConfigFolder)), Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty))
            Dim mySite As New DomainConfiguration
            Dim mySiteSettings As DomainConfigurations
            If CType(HttpContext.Current.Application(wpm_HostName), DomainConfiguration) Is Nothing Then
                Try
                    If Not IsNothing(ConfigFile) Then
                        mySiteSettings = DomainConfigurations.Load(ConfigFile)
                        If (mySiteSettings?.Configuration?.CompanyID Is Nothing) Then
                            ' Add New Site Configuration with default values
                            mySiteSettings = New DomainConfigurations With {
                                .Configuration = New DomainConfiguration
                            }
                            mySiteSettings.Configuration.CompanyID = "17"
                            mySiteSettings.Configuration.SQLDBConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|projectmechanics.mdb;"
                            mySiteSettings.Configuration.AccessDatabasePath = "/App_Data/projectmechanics.mdb"
                        End If


                        If Not (mySiteSettings Is Nothing) Then
                                mySite.CompanyID = CStr(mySiteSettings.Configuration.CompanyID)
                                mySite.SQLDBConnString = CStr(mySiteSettings.Configuration.SQLDBConnString)
                                mySite.AccessDatabasePath = CStr(mySiteSettings.Configuration.AccessDatabasePath)
                                HttpContext.Current.Application(wpm_HostName) = mySite
                            End If
                        Else
                            ApplicationLogging.ErrorLog(String.Format("Invalid Site Config - {0}", ConfigFile), "App.wpm_DomainConfig")
                        wpm_AddGenericError(String.Format("App.wpm_DomainConfig - Invalid Site Config - {0}", ConfigFile))

                    End If
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Invalid Site Config - {0} - {1}", ConfigFile, ex.Message), "App.wpm_DomainConfig")
                End Try
            End If
            Return TryCast(HttpContext.Current.Application(wpm_HostName), DomainConfiguration)
        End Get
    End Property
    Public Sub wpm_CheckCommandParameters()
        Try
            If Not IsNothing(HttpContext.Current.Request.QueryString.GetValues("c")) Then
                wpm_CurrentPageID = HttpContext.Current.Request.QueryString.Item("c")
            End If
            If (HttpContext.Current.Request.QueryString.Item("a") <> String.Empty) Then
                wpm_CurrentArticleID = wpm_GetDBInteger(HttpContext.Current.Request.QueryString.Item("a"))
            Else
                wpm_CurrentArticleID = 0
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Problem checking parameters - " & ex.ToString, "App.wpm_CheckCommandParameters")
        End Try
    End Sub
    Public ReadOnly Property wpm_SQLDBConnString() As String
        Get
            Return CType(HttpContext.Current.Application(wpm_HostName), DomainConfiguration).SQLDBConnString
        End Get
    End Property
    Public ReadOnly Property wpm_AccessDatabasePath As String
        Get
            Return wpm_DomainConfig.AccessDatabasePath
        End Get
    End Property
    Public ReadOnly Property wpm_IsAdmin As Boolean
        Get
            If CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID)) = String.Empty Then
                HttpContext.Current.Session.Item(wpm_STR_UserGroupID) = "4"
            End If
            If CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID)) = "1" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property wpm_IsEditor As Boolean
        Get
            Dim sUserGroup As String = CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID))
            If (sUserGroup = "1" Or sUserGroup = "2") Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property wpm_IsUser As Boolean
        Get
            If CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID)) <> "4" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property wpm_HostName As String
        Get
            Return Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty)
        End Get
    End Property
    Public Function wpm_SetListPageURL(ByVal sQueryString As String, ByVal sServerName As String, ByVal sURL As String) As Boolean
        If (Left(sQueryString, 4) = "404;") Then
            wpm_ListPageURL = (Right(sQueryString, Len(CStr(sQueryString)) - 4))
        Else
            wpm_ListPageURL = (String.Format("http://{0}{1}?{2}", sServerName, sURL, sQueryString))
        End If
        Return True
    End Function
    Public Property wpm_LoginRedirectURL() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_LoginRedirectURL))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_LoginRedirectURL) = value
        End Set
    End Property
    Public Property wpm_ListPageURL() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_ListPageURL))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_ListPageURL) = value
        End Set
    End Property
    Public Property wpm_DefaultSitePrefix() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_DefaultSitePrefix))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_DefaultSitePrefix) = value
        End Set
    End Property
    Public Property wpm_SiteTemplatePrefix() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_SiteTemplatePrefix))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_SiteTemplatePrefix) = value
        End Set
    End Property

    Public Function wpm_IsValidURL(ByVal URL As String) As Boolean
        If String.IsNullOrEmpty(URL) Then
            Return False
        Else
            Return Regex.IsMatch(URL, "([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?")
        End If
    End Function
    Public Function wpm_AppendRightContent(ByVal sText As String) As Boolean
        HttpContext.Current.Session.Item(wpm_STR_RightContent) = HttpContext.Current.Session.Item(wpm_STR_RightContent).ToString & sText
        Return True
    End Function
    Public Function wpm_SetRightContent(ByVal sText As String) As Boolean
        HttpContext.Current.Session.Item(wpm_STR_RightContent) = sText
        Return True
    End Function
    Public Function wpm_GetRightContent() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_RightContent))
    End Function
    Public Function wpm_GetUserGroup() As String
        If CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID)) = String.Empty Then
            HttpContext.Current.Session.Item(wpm_STR_UserGroupID) = "4"
        End If
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_UserGroupID))
    End Function
    Public Function wpm_SetUserGroup(ByVal sGroupID As String) As Boolean
        HttpContext.Current.Session.Item(wpm_STR_UserGroupID) = sGroupID
        Return True
    End Function
    Public Function wpm_GetContactID() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_ContactID))
    End Function
    Public Function wpm_GetUserName() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_UserName))
    End Function
    Public Function wpm_SetUserName(ByVal sUserName As String) As Boolean
        HttpContext.Current.Session.Item(wpm_STR_UserName) = sUserName
        Return True
    End Function
    Public Function wpm_GetUserEmail() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_UserEmail))
    End Function
    Public Function wpm_SetUserEmail(ByVal sUserEmail As Object) As Boolean
        HttpContext.Current.Session.Item(wpm_STR_UserEmail) = sUserEmail
        Return True
    End Function
    Public Function wpm_GetCurrentSQL() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_CurrentSQL))
    End Function
    Public Function wpm_SetCurrentSQL(ByVal sCurrentSQL As String) As String
        HttpContext.Current.Session.Item(wpm_STR_CurrentSQL) = sCurrentSQL
        Return HttpContext.Current.Session.Item(wpm_STR_CurrentSQL).ToString
    End Function
    Public Function wpm_GetDefaultSitePrefix() As String
        If IsDBNull(HttpContext.Current.Session.Item(wpm_STR_DefaultSitePrefix)) Then
            Return wpm_SiteTemplatePrefix
        Else
            Return CStr(HttpContext.Current.Session.Item(wpm_STR_DefaultSitePrefix))
        End If
    End Function
    Public Function wpm_GetSiteHomePageID() As String
        Return CStr(HttpContext.Current.Session.Item(wpm_STR_SiteHomePageID))
    End Function
    Public Function wpm_GetSessionDebug() As String
        Dim mysb As New StringBuilder(String.Empty)
        mysb.Append("<div class='wpmADMIN'><br/><br/><h2>Current Session</h2><table border=""1"">")
        For Each item As Object In HttpContext.Current.Session
            mysb.Append("<tr><td>")
            mysb.Append(item.ToString)
            mysb.Append("</td><td>&nbsp;")
            mysb.Append(HttpContext.Current.Session.Item(item.ToString))
            mysb.Append("</td></tr>")
        Next
        mysb.Append("</table><br/><h2>Current Application</h2><table border=""1"">")
        For Each item In HttpContext.Current.Application.Contents
            mysb.Append("<tr><td>")
            mysb.Append(item.ToString)
            mysb.Append("</td><td>&nbsp;")
            mysb.Append(HttpContext.Current.Application.Contents(item.ToString))
            mysb.Append("</td></tr>")
        Next
        mysb.Append("</table><br/>")

        mysb.Append(String.Format("<br><strong>Caching Enabled:</strong>{0}<br/>", wpm_SiteConfig.CachingEnabled))
        mysb.Append(String.Format("<br><strong>Use 404 Processing Enabled:</strong>{0}<br/>", wpm_SiteConfig.Use404Processing))
        mysb.Append(wpm_GetPageHistory())
        mysb.Append("</div><br/>")

        Return mysb.ToString
    End Function
    Public Function wpm_SetPageHistory() As Boolean
        Try
            wpm_PageHistory = CType(HttpContext.Current.Session(wpm_STR_PageHistory), LocationHistoryList)
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Error when reading Session variable (PageHistory) - " & ex.ToString, "ApplicationSession.New")

        End Try
        If wpm_PageHistory Is Nothing Then
            wpm_PageHistory = New LocationHistoryList
        End If
        Return True
    End Function
    Public Property wpm_PageHistory As LocationHistoryList
        Get
            Return TryCast(HttpContext.Current.Session(wpm_STR_PageHistory), LocationHistoryList)
        End Get
        Set(value As LocationHistoryList)
            HttpContext.Current.Session(wpm_STR_PageHistory) = value
        End Set
    End Property
    Public Function pm_AddPageHistory(ByVal PageName As String) As Boolean
        Try
            wpm_PageHistory.AddPageHistory(PageName)
            Return True
        Catch
            Return False
        End Try
    End Function
    Public Function wpm_GetPageHistory() As String
        Dim mysb As New StringBuilder("<hr/><br/><table border=""1"">")
        For Each ph As LocationHistory In wpm_PageHistory
            mysb.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", ph.ViewTime, ph.PageName, ph.RequestURL, ph.PageSource))
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function
    Public Property wpm_CurrentSiteName As String
        Get
            Return CStr(HttpContext.Current.Session.Item(wpm_STR_CurrentSiteName))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_CurrentSiteName) = value
        End Set
    End Property
    Public Property wpm_CurrentSiteID() As String
        Get
            Return CStr(HttpContext.Current.Session.Item(wpm_STR_CurrentSiteID))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_CurrentSiteID) = value
        End Set
    End Property
    Public Property wpm_CurrentPageID() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_CurrentPageID))
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = String.Empty
            End If
            HttpContext.Current.Session.Item(wpm_STR_CurrentPageID) = value
        End Set
    End Property
    Public Property wpm_CurrentArticleID() As Integer
        Get
            Return wpm_GetDBInteger(HttpContext.Current.Session.Item(wpm_STR_CurrentArticleID))
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session.Item(wpm_STR_CurrentArticleID) = value
        End Set
    End Property
    Public Property wpm_CurrentUserGroupID() As String
        Get
            If HttpContext.Current.Session.Item(wpm_STR_UserGroupID) Is Nothing Then
                HttpContext.Current.Session.Item(wpm_STR_UserGroupID) = "4"
            End If
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_UserGroupID))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_UserGroupID) = value
        End Set
    End Property
    Public ReadOnly Property wpm_ContactID() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_ContactID))
        End Get
    End Property
    Public Function wpm_SetContactID(ByVal value As String) As String
        HttpContext.Current.Session.Item(wpm_STR_ContactID) = value
        Return HttpContext.Current.Session.Item(wpm_STR_ContactID).ToString()
    End Function
    Public Property wpm_ContactName() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_ContactName))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_ContactName) = value
        End Set
    End Property
    Public Property wpm_ContactEmail() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_ContactEmail))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session.Item(wpm_STR_ContactEmail) = value
        End Set
    End Property

    Public Property wpm_AddHTMLHead() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_HTMLHead))
        End Get
        Set(ByVal value As String)
            If value = "RESET" Then
                HttpContext.Current.Session.Item(wpm_STR_HTMLHead) = String.Empty
            Else
                Dim curValue As New StringBuilder(wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_HTMLHead)))
                curValue.Append(value)
                HttpContext.Current.Session.Item(wpm_STR_HTMLHead) = curValue.ToString
            End If
        End Set
    End Property
    Public Property wpm_SiteGallery() As String
        Get
            Return wpm_GetDBString(HttpContext.Current.Session.Item(wpm_STR_SiteGallery))
        End Get
        Set(ByVal Value As String)
            HttpContext.Current.Session.Item(wpm_STR_SiteGallery) = Value
        End Set
    End Property

    ' Return URL
    Public ReadOnly Property wpm_ReturnUrl() As String
        Get ' Get referer URL automatically
            If HttpContext.Current.Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then
                If wpm_ReferPage() <> wpm_CurrentPage() AndAlso wpm_ReferPage() <> "login.aspx" Then ' Referer not same page or login page
                    Dim url As String = HttpContext.Current.Request.ServerVariables("HTTP_REFERER")
                    If url.Contains("?a=") Then ' Remove action
                        Dim p1 As Integer = url.LastIndexOf("?a=")
                        Dim p2 As Integer = url.IndexOf("&", p1)
                        If p2 > -1 Then
                            url = url.Substring(0, p1 + 1) & url.Substring(p2 + 1)
                        Else
                            url = url.Substring(0, p1)
                        End If
                    End If
                    wpm_ListPageURL = url ' Save to Session
                End If
            End If
            If wpm_NotEmpty(wpm_ListPageURL) Then
                Return CStr(wpm_ListPageURL)
            Else
                Return "/"
            End If
        End Get
    End Property

    Public Function wpm_GetIntegerProperty(ByVal myProperty As String, ByVal curValue As Integer) As Integer
        Dim myValue As Integer
        If Len(HttpContext.Current.Request.QueryString(myProperty)) > 0 Then
            myValue = wpm_GetDBInteger(HttpContext.Current.Request.QueryString(myProperty), curValue)
        ElseIf Len(HttpContext.Current.Request.Form.Item(myProperty)) > 0 Then
            myValue = wpm_GetDBInteger(HttpContext.Current.Request.Form.Item(myProperty), curValue)
        Else
            myValue = curValue
        End If
        Return myValue
    End Function
    Public Function wpm_GetFormProperty(ByVal myProperty As String, ByVal curValue As String, ByVal ControlPrefix As String) As String
        Dim myValue As String = String.Empty
        If Len(HttpContext.Current.Request.QueryString(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.QueryString(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request.Form.Item(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.Form.Item(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request.Form(String.Format("{0}{1}", ControlPrefix, myProperty))) > 0 Then
            myValue = HttpContext.Current.Request.Form(String.Format("{0}{1}", ControlPrefix, myProperty))
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function

    Public Function wpm_GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = String.Empty
        If Len(HttpContext.Current.Request.QueryString(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.QueryString(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request.Form.Item(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.Form.Item(myProperty).ToString
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function
    ' Write the paths for config/debug only
    'Public Shared Sub wpm_WriteUploadPaths()
    '    wpm_Write(String.Format("APPL_PHYSICAL_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_PHYSICAL_PATH")))
    '    wpm_Write(String.Format("APPL_MD_PATH = {0}<br>", HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")))
    'End Sub

    ' Get current page name
    Public Function wpm_CurrentPage() As String
        Return wpm_GetPageName(HttpContext.Current.Request.ServerVariables("SCRIPT_NAME"))
    End Function

    ' Get refer page name
    Public Function wpm_ReferPage() As String
        Return wpm_GetPageName(HttpContext.Current.Request.ServerVariables("HTTP_REFERER"))
    End Function

    ' Get page name
    Public Function wpm_GetPageName(ByVal url As String) As String
        If url <> String.Empty Then
            If url.Contains("?") Then
                url = url.Substring(0, url.LastIndexOf("?")) ' Remove querystring first
            End If
            Return url.Substring(url.LastIndexOf("/") + 1) ' Remove path
        Else
            Return String.Empty
        End If
    End Function

    ' Get full URL
    Public Function wpm_FullUrl() As String
        Return wpm_DomainUrl() & HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    End Function

    ' Get domain URL
    Public Function wpm_DomainUrl() As String
        Dim sUrl As String
        Dim bSSL As Boolean = Not wpm_SameText(HttpContext.Current.Request.ServerVariables("HTTPS"), "off")
        Dim sPort As String = HttpContext.Current.Request.ServerVariables("SERVER_PORT")
        Dim defPort As String = CStr(IIf(bSSL, "443", "80"))
        If sPort = defPort Then sPort = String.Empty Else sPort = ":" & sPort
        If bSSL Then
            sUrl = "http" & "s"
        Else
            sUrl = "http"
        End If
        Return String.Format("{0}://{1}{2}", sUrl, HttpContext.Current.Request.ServerVariables("SERVER_NAME"), sPort)
    End Function

    ' Get current URL
    Public Function wpm_CurrentUrl() As String
        Dim s As String = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
        Dim q As String = HttpContext.Current.Request.ServerVariables("QUERY_STRING")
        If q <> String.Empty Then s &= "?" & q
        Return s
    End Function

    ' Get application root path (relative to domain)
    Public Function wpm_AppPath() As String
        Dim pos As String
        Dim Path As String = HttpContext.Current.Request.ServerVariables("APPL_MD_PATH")
        pos = CStr(Path.IndexOf("Root", StringComparison.InvariantCultureIgnoreCase))
        If CDbl(pos) > 0 Then Path = Path.Substring(CInt(CDbl(pos) + 4))
        Return Path
    End Function

    ' Convert to full URL
    Public Function wpm_ConvertFullUrl(ByVal url As String) As String
        If url = String.Empty Then
            Return String.Empty
        ElseIf url.Contains("://") Then
            Return url
        Else
            Dim sUrl As String = wpm_FullUrl()
            Return sUrl.Substring(0, sUrl.LastIndexOf("/") + 1) & url
        End If
    End Function

    ' Check if folder exists
    Public Function wpm_FolderExists(ByVal folder As String) As Boolean
        Return Directory.Exists(folder)
    End Function

    ' Check if file exists
    Public Function wpm_FileExists(ByVal folder As String, ByVal fn As String) As Boolean
        Return File.Exists(folder & fn)
    End Function

    Public Function wpm_DeleteFile(ByVal folder As String, ByVal fn As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            If File.Exists(folder & fn) Then
                File.Delete(folder & fn)
            End If
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function


    ' Response.Write
    Public Sub wpm_Write(ByVal value As Object)
        HttpContext.Current.Response.Write(value)
    End Sub

    ' Response.End
    Public Sub wpm_End()
        HttpContext.Current.Response.End()
    End Sub


    ' Compare object as string
    Public Function wpm_SameStr(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim(), Convert.ToString(v2).Trim())
    End Function

    ' Compare object as string (case insensitive)
    Public Function wpm_SameText(ByVal v1 As Object, ByVal v2 As Object) As Boolean
        Return String.Equals(Convert.ToString(v1).Trim().ToLower(), Convert.ToString(v2).Trim().ToLower())
    End Function

    ' Check if empty string
    Public Function wpm_Empty(ByVal value As Object) As Boolean
        Return String.Equals(Convert.ToString(value).Trim(), String.Empty)
    End Function

    ' Check if not empty string
    Public Function wpm_NotEmpty(ByVal value As Object) As Boolean
        Return Not wpm_Empty(value)
    End Function

    ' Convert object to integer
    Public Function wpm_ConvertToInt(ByVal value As Object) As Integer
        Try
            Return Convert.ToInt32(value)
        Catch
            Return 0
        End Try
    End Function

    ' Convert object to double
    Public Function wpm_ConvertToDouble(ByVal value As Object) As Double
        Try
            Return Convert.ToDouble(value)
        Catch
            Return 0
        End Try
    End Function

    ' Convert object to bool
    Public Function wpm_ConvertToBool(ByVal value As Object) As Boolean
        Try
            If Information.IsNumeric(value) Then
                Return Convert.ToBoolean(wpm_ConvertToDouble(value))
            Else
                Return Convert.ToBoolean(value)
            End If
        Catch
            Return False
        End Try
    End Function

    ' Get user IP
    Public Function wpm_CurrentUserIP() As String
        Return HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
    End Function

    ' Get current host name, e.g. "www.mycompany.com"
    Public Function wpm_CurrentHost() As String
        Return HttpContext.Current.Request.ServerVariables("HTTP_HOST")
    End Function


    Public Function wpm_GetUserOptions() As String
        Dim sReturn As String = String.Empty
        If wpm_GetContactID() <> String.Empty Then
            sReturn = String.Format("<a href=""{0}login/logout.aspx"" >Sign Out</a>", wpm_SiteConfig.AdminFolder())
        Else
            sReturn = String.Format("<a href=""{0}login/login.aspx"">Sign On</a>", wpm_SiteConfig.AdminFolder())
        End If
        Return sReturn
    End Function

    Public Function wpm_CheckCurrentSettings() As Boolean
        Dim sbError As New StringBuilder
        If wpm_CurrentSiteID = String.Empty Then
            sbError.Append("&NoCompanyID=Failed")
        End If
        If wpm_SQLDBConnString = String.Empty Then
            sbError.Append("&NoSiteDB=Failed")
        End If
        If sbError.Length > 0 Then
            HttpContext.Current.Response.Redirect(String.Format("~{0}debug.aspx?Error=TRUE{1}", wpm_SiteConfig.ApplicationHome(), sbError))
            Return False
        Else
            Return True
        End If
    End Function

    Public Function wpm_TagExists(ByRef Content As StringBuilder, ByVal TagName As String) As Boolean
        Return Content.ToString.Contains(TagName)
    End Function
    Public ReadOnly Property wpm_SiteMapName As String
        Get
            Return (String.Format("wpm_cache_{0}_{1}_{2}_{3}", Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty), wpm_CurrentSiteID, wpm_GetUserGroup(), "ORDER"))
        End Get
    End Property
    Public Sub wpm_ResetSessionVariables()
        wpm_CurrentArticleID = 0
        wpm_CurrentPageID = String.Empty
        ' Session.RightContent = ""
        wpm_AddHTMLHead = "RESET"
    End Sub
    Public Function wpm_SetMissingParts(ByRef sRightLinks As String, ByRef sLeftLinks As String, ByRef sCenterLinks As String) As Boolean
        If wpm_IsAdmin Then
            sRightLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO RIGHT LINKS</a>", wpm_SiteConfig.AdminFolder)
            sLeftLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO LEFT LINKS</a>", wpm_SiteConfig.AdminFolder)
            sCenterLinks = String.Format("<a href=""{0}maint/default.aspx?type=Part"">NO CENTER LINKS</a>", wpm_SiteConfig.AdminFolder)
        Else
            sRightLinks = String.Empty
            sLeftLinks = String.Empty
            sCenterLinks = String.Empty
        End If
        Return True
    End Function
    Public Function wpm_RemoveQueryString(ByVal strPath As String) As String
        Dim iQuestionMark As Integer = InStr(strPath, "?")
        If (iQuestionMark > 0) Then
            strPath = Left(strPath, iQuestionMark - 1)
        End If
        Return strPath
    End Function

    Public Sub wpm_AddGenericError(ByVal sReason As String)
        If Not HttpContext.Current.Session Is Nothing Then
            Dim sReturn As New StringBuilder(sReason)
            sReturn.AppendLine("<br/>")
            sReturn.AppendLine(wpm_GetDBString(HttpContext.Current.Session("wpm_GenericError")))
            sReturn.AppendLine("<br/>")
            HttpContext.Current.Session("wpm_GenericError") = sReturn.ToString()
        End If
    End Sub
    Public Sub wpm_SetGenericError(ByVal sReason As String)
        If Not HttpContext.Current.Session Is Nothing Then
            Dim sReturn As New StringBuilder(sReason)
            sReturn.AppendLine("<br/>")
            sReturn.AppendLine(wpm_GetDBString(HttpContext.Current.Session("wpm_GenericError")))
            sReturn.AppendLine("<br/>")
            sReturn.AppendLine(String.Format("CompanyID: {0}<br/>", wpm_DomainConfig.CompanyID))
            sReturn.AppendLine(String.Format("AccessDatabasePath: {0}<br/>", wpm_DomainConfig.AccessDatabasePath))
            sReturn.AppendLine(String.Format("Connection String: {0}<br/>", wpm_DomainConfig.SQLDBConnString))
            HttpContext.Current.Session("wpm_GenericError") = sReturn.ToString()
        End If
    End Sub

End Module
