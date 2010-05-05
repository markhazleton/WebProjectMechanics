Imports System.Web

Public Class wpmSession
#Region "Private Properties"
    Private mySession As System.Web.SessionState.HttpSessionState
    Private myPageHistory As wpmPageHistoryList
    Private _siteGallery As String
    Private _SessionTracker As wpmSessionTracker

#End Region
#Region "Public Properties"
    Public Property CompanyID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("CompanyID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("CompanyID") = value
        End Set
    End Property
    Public Property SiteCategoryTypeID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("SiteCategoryTypeID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteCategoryTypeID") = value
        End Set
    End Property
    Public Property CurrentPageID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("CurrentPageID"))
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            mySession.Item("CurrentPageID") = value
        End Set
    End Property
    Public Property CurrentArticleID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("CurrentArticleID"))
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            mySession.Item("CurrentArticleID") = value
        End Set
    End Property
    Public Property SiteTemplatePrefix() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("SiteTemplatePrefix"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteTemplatePrefix") = value
        End Set
    End Property
    Public Property GroupID() As String
        Get
            If mySession.Item("GroupID") Is Nothing Then
                mySession.Item("GroupID") = "4"
            End If
            Return wpmUTIL.GetDBString(mySession.Item("GroupID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("GroupID") = value
        End Set
    End Property
    Public Property ContactID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactID") = value
        End Set
    End Property
    Public Property ContactName() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactName"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactName") = value
        End Set
    End Property
    Public Property ContactEmail() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactEmail"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactEmail") = value
        End Set
    End Property
    Public Property ContactRoleTitle() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactRoleTitle"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleTitle") = value
        End Set
    End Property
    Public Property ContactRoleID() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactRoleID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleID") = value
        End Set
    End Property
    Public Property ContactRoleFilterMenu() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ContactRoleFilterMenu"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleFilterMenu") = value
        End Set
    End Property
    Public Property SiteDB() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("SiteDB"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteDB") = value
        End Set
    End Property
    Public Property ListPageURL() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("ListPageURL"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ListPageURL") = value
        End Set
    End Property
    Public Property RightContent() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("RightContent"))
        End Get
        Set(ByVal value As String)
            mySession.Item("RightContent") = value
        End Set
    End Property
    Public Property AddHTMLHead() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("AddHTMLHead"))
        End Get
        Set(ByVal value As String)
            If value = "RESET" Then
                mySession.Item("AddHTMLHead") = String.Empty
            Else
                Dim curValue As New StringBuilder(wpmUTIL.GetDBString(mySession.Item("AddHTMLHead")))
                curValue.Append(value)
                mySession.Item("AddHTMLHead") = curValue.ToString
            End If
        End Set
    End Property
    Public Property SiteGallery() As String
        Get
            Return wpmUTIL.GetDBString(mySession.Item("SiteGallery"))
        End Get
        Set(ByVal Value As String)
            mySession.Item("SiteGallery") = Value
        End Set
    End Property
    Public ReadOnly Property SiteMapFilePath() As String
        Get
            Return wpmFileIO.GetValidPath(wpmConfig.wpmConfigFile & "\index\" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").Replace("www.", "") & " - " & CompanyID & " - " & GroupID & ".xml", "GetSiteMapFilePath")
        End Get
    End Property

#End Region
#Region "Page History"
    Private Function SetPageHistory() As Boolean
        Try
            myPageHistory = CType(mySession("PageHistory"), wpmPageHistoryList)
        Catch ex As Exception
            wpmLog.AuditLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "wpmSession.New")
        End Try
        If myPageHistory Is Nothing Then
            myPageHistory = New wpmPageHistoryList
        End If
        Return True
    End Function
    Public Function AddPageHistory(ByVal PageName As String) As Boolean
        myPageHistory.AddPageHistory(PageName)
        mySession("PageHistory") = myPageHistory
        Return True
    End Function
    Public Function GetPageHistory() As String
        Dim mysb As New StringBuilder("<hr/><br/><table cellspacing=""0"" rowhighlightclass=""ewTableHighlightRow"" rowselectclass=""ewTableSelectRow"" roweditclass=""ewTableEditRow"" class=""ewTable ewTableSeparate"">")
        For Each ph As wpmPageHistory In myPageHistory
            mysb.Append("<tr><td>" & ph.ViewTime.ToString() & "</td><td>" & ph.PageName & "</td><td>" & ph.RequestURL & "</td><td>" & ph.PageSource & "</td></tr>")
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function

#End Region
    Sub New(ByVal ThisSession As System.Web.SessionState.HttpSessionState)
        mySession = ThisSession
        If (SiteDB.Trim) = String.Empty Then
            GetConfig()
        End If
        CheckCommandParameters()
        SetPageHistory()
    End Sub

    Private Sub CheckCommandParameters()
        Try
            If Not IsNothing(HttpContext.Current.Request.QueryString.GetValues("c")) Then
                CurrentPageID = HttpContext.Current.Request.QueryString.Item("c")
            End If
            If (HttpContext.Current.Request.QueryString.Item("a") <> "") Then
                CurrentArticleID = HttpContext.Current.Request.QueryString.Item("a")
            Else
                CurrentArticleID = ""
            End If
        Catch ex As Exception
            wpmLog.AuditLog("Problem checking parameters - " & ex.ToString, "wpmSession.CheckcommandParameters")
        End Try
    End Sub
    Public Shared Function GetSiteDB() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("SiteDB"))
    End Function
    Public Shared Function GetCompanyID() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("CompanyID"))
    End Function
    Public Shared Function GetLogin_Link() As String
        If CStr(System.Web.HttpContext.Current.Session.Item("Login_Link")) = "" Then
            Return CStr(System.Web.HttpContext.Current.Session.Item("ListPage"))
        Else
            Return CStr(System.Web.HttpContext.Current.Session.Item("Login_Link"))
        End If
    End Function
    Public Shared Function SetLogin_Link(ByVal sLogin_Link As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("Login_Link") = sLogin_Link
        Return True
    End Function
    Public Shared Function AppendRightContent(ByVal sText As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("RightContent") = System.Web.HttpContext.Current.Session.Item("RightContent").ToString & sText
        Return True
    End Function
    Public Shared Function SetRightContent(ByVal sText As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("RightContent") = sText
        Return True
    End Function
    Public Shared Function GetRightContent() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("RightContent"))
    End Function
    Public Shared Function GetUserGroup() As String
        If CStr(System.Web.HttpContext.Current.Session.Item("GroupID")) = "" Then
            System.Web.HttpContext.Current.Session.Item("GroupID") = "4"
        End If
        Return CStr(System.Web.HttpContext.Current.Session.Item("GroupID"))
    End Function
    Public Shared Function SetUserGroup(ByVal sGroupID As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("GroupID") = sGroupID
        Return True
    End Function
    Public Shared Function GetContactID() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("ContactID"))
    End Function
    Public Shared Function SetContactID(ByVal sContactID As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("ContactID") = sContactID
        Return True
    End Function
    Public Shared Function GetUserName() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("UserName"))
    End Function
    Public Shared Function SetUserName(ByVal sUserName As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("UserName") = sUserName
        Return True
    End Function
    Public Shared Function GetUserEmail() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("UserEmail"))
    End Function
    Public Shared Function SetUserEmail(ByVal sUserEmail As Object) As Boolean
        System.Web.HttpContext.Current.Session.Item("UserEmail") = sUserEmail
        Return True
    End Function
    Public Shared Function GetUserRoleTitle() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("UserRoleTitle"))
    End Function
    Public Shared Function SetUserRoleTitle(ByVal sUserRoleTitle As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("UserRoleTitle") = sUserRoleTitle
        SetUserRoleTitle = True
    End Function
    Public Shared Function GetUserRoleID() As String
        GetUserRoleID = CStr(System.Web.HttpContext.Current.Session.Item("UserRoleID"))
    End Function
    Public Shared Function SetUserRoleID(ByVal sUserRoleID As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("UserRoleID") = sUserRoleID
        SetUserRoleID = True
    End Function
    Public Shared Function GetCurrentSQL() As String
        GetCurrentSQL = CStr(System.Web.HttpContext.Current.Session.Item("CurrentSQL"))
    End Function
    Public Shared Function SetCurrentSQL(ByVal sCurrentSQL As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("CurrentSQL") = sCurrentSQL
        SetCurrentSQL = True
    End Function
    Public Shared Function GetUserRoleFilterMenu() As Boolean
        Return CBool(System.Web.HttpContext.Current.Session.Item("UserRoleFilterMenu"))
    End Function
    Public Shared Function SetUserRoleFilterMenu(ByVal sUserRoleFilterMenu As String) As Boolean
        System.Web.HttpContext.Current.Session.Item("UserRoleFilterMenu") = sUserRoleFilterMenu
        SetUserRoleFilterMenu = True
    End Function
    Public Shared Function GetDefaultSitePrefix() As String
        If IsDBNull(System.Web.HttpContext.Current.Session.Item("DefaultSitePrefix")) Then
            GetDefaultSitePrefix = "ff"
        Else
            GetDefaultSitePrefix = CStr(System.Web.HttpContext.Current.Session.Item("DefaultSitePrefix"))
        End If
    End Function
    Public Shared Function GetSiteHomePageID() As String
        GetSiteHomePageID = CStr(System.Web.HttpContext.Current.Session.Item("SiteHomePageID"))
    End Function
    Private Function GetConfig() As Boolean
        Try
            CompanyID = App.SiteList.GetSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")).CompanyID
            SiteDB = App.SiteList.GetSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")).SQLDBConnString
            If CompanyID = String.Empty Then
                If Not IsNothing(wpmConfig.wpmConfigFile) Then
                    Dim SiteConfig As wpmSiteSettings = wpmSiteSettings.Load(wpmConfig.wpmConfigFile)
                    CompanyID = (SiteConfig.wpmSite.CompanyID.ToString)
                    SiteDB = (SiteConfig.wpmSite.SQLDBConnString.ToString)
                    App.SiteList.AddSite(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), CompanyID, SiteDB)
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            wpmLog.AuditLog(ex.ToString, "GetConfig")
        End Try
    End Function

    Public Function GetSessionDebug() As String
        Dim mysb As New StringBuilder("")
        mysb.Append("<br/><hr/><table border=""1"">")
        For Each item As Object In mySession.Contents
            mysb.Append("<tr><td>")
            mysb.Append(item.ToString)
            mysb.Append("</td><td>&nbsp;")
            mysb.Append(mySession.Contents.Item(item.ToString))
            mysb.Append("</td></tr>")
        Next item
        mysb.Append("</table><br/><hr/>")
        mysb.Append("<br/><hr/><table border=""1"">")
        For Each item As Object In HttpContext.Current.Application.Contents
            mysb.Append("<tr><td>")
            mysb.Append(item.ToString)
            mysb.Append("</td><td>&nbsp;")
            mysb.Append(HttpContext.Current.Application.Contents.Item(item.ToString))
            mysb.Append("</td></tr>")
        Next item
        mysb.Append("</table><br/><hr/>")

        mysb.Append("<br><strong>Caching Enabled:</strong>" & App.Config.CachingEnabled & "<br/>")
        mysb.Append("<br><strong>Use 404 Processing Enabled:</strong>" & App.Config.Use404Processing & "<br/>")
        mysb.Append(GetPageHistory())

        Return mysb.ToString
    End Function




End Class


Public Class wpmPageHistory
    Private _PageSource As String
    Public Property PageSource() As String
        Get
            Return _PageSource
        End Get
        Set(ByVal value As String)
            _PageSource = value
        End Set
    End Property
    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _ViewTime As Date
    Public Property ViewTime() As Date
        Get
            Return _ViewTime
        End Get
        Set(ByVal value As Date)
            _ViewTime = value
        End Set
    End Property
    Private _RequestURL As String
    Public Property RequestURL() As String
        Get
            Return _RequestURL
        End Get
        Set(ByVal value As String)
            _RequestURL = value
        End Set
    End Property

End Class

<Serializable()> Public Class wpmPageHistoryList
    Inherits List(Of wpmPageHistory)
    Public Function AddPageHistory(ByVal PageName As String) As Boolean
        Dim myPH As New wpmPageHistory
        myPH.PageName = PageName

        myPH.RequestURL = HttpContext.Current.Request.Url.AbsoluteUri
        If myPH.RequestURL.Contains("?404;") Then
            myPH.RequestURL = myPH.RequestURL.Substring(myPH.RequestURL.LastIndexOf("?404;") + 5)
            myPH.RequestURL = myPH.RequestURL.Replace(":80", "")
        End If
        If IsNothing(HttpContext.Current.Request.UrlReferrer) Then
            myPH.PageSource = "unknown"
        Else
            myPH.PageSource = HttpContext.Current.Request.UrlReferrer.AbsoluteUri
        End If
        myPH.ViewTime = Date.Now()
        If Me.Count = 0 Then
            If Not IsNothing(HttpContext.Current.Request.UrlReferrer) Then
                If Not HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Contains(HttpContext.Current.Request.Url.Host) Then
                    wpmLog.SiteReferLog("Referrer", HttpContext.Current.Request.UrlReferrer.AbsoluteUri)
                End If
            End If
        End If
        Me.Add(myPH)
        Return True
    End Function

   
End Class
