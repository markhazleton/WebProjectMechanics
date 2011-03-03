Imports System.Web
Imports System.Text

Public Class wpmSession
#Region "Private Properties"
    Private mySession As SessionState.HttpSessionState
    Private myPageHistory As wpmPageHistoryList
    Private _siteGallery As String
    Private _SessionTracker As wpmSessionTracker

#End Region
#Region "Public Properties"
    Public Property CompanyID() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("CompanyID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("CompanyID") = value
        End Set
    End Property
    Public Property SiteCategoryTypeID() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("SiteCategoryTypeID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteCategoryTypeID") = value
        End Set
    End Property
    Public Property CurrentPageID() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("CurrentPageID"))
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
            Return wpmUtil.GetDBString(mySession.Item("CurrentArticleID"))
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
            Return wpmUtil.GetDBString(mySession.Item("SiteTemplatePrefix"))
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
            Return wpmUtil.GetDBString(mySession.Item("GroupID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("GroupID") = value
        End Set
    End Property
    Public Property ContactID() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactID") = value
        End Set
    End Property
    Public Property ContactName() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactName"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactName") = value
        End Set
    End Property
    Public Property ContactEmail() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactEmail"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactEmail") = value
        End Set
    End Property
    Public Property ContactRoleTitle() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactRoleTitle"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleTitle") = value
        End Set
    End Property
    Public Property ContactRoleID() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactRoleID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleID") = value
        End Set
    End Property
    Public Property ContactRoleFilterMenu() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ContactRoleFilterMenu"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleFilterMenu") = value
        End Set
    End Property
    Public Property ListPageURL() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("ListPageURL"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ListPageURL") = value
        End Set
    End Property
    Public Property RightContent() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("RightContent"))
        End Get
        Set(ByVal value As String)
            mySession.Item("RightContent") = value
        End Set
    End Property
    Public Property AddHTMLHead() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("AddHTMLHead"))
        End Get
        Set(ByVal value As String)
            If value = "RESET" Then
                mySession.Item("AddHTMLHead") = String.Empty
            Else
                Dim curValue As New StringBuilder(wpmUtil.GetDBString(mySession.Item("AddHTMLHead")))
                curValue.Append(value)
                mySession.Item("AddHTMLHead") = curValue.ToString
            End If
        End Set
    End Property
    Public Property SiteGallery() As String
        Get
            Return wpmUtil.GetDBString(mySession.Item("SiteGallery"))
        End Get
        Set(ByVal Value As String)
            mySession.Item("SiteGallery") = Value
        End Set
    End Property
    Public ReadOnly Property SiteMapFilePath() As String
        Get
            Return wpmFileProcessing.GetValidPath(String.Format("{0}\index\{1} - {2} - {3}.xml", wpmApp.wpmConfigFile, HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").Replace("www.", ""), CompanyID, GroupID), "GetSiteMapFilePath")
        End Get
    End Property

#End Region
#Region "Page History"
    Private Function SetPageHistory() As Boolean
        Try
            myPageHistory = CType(mySession("PageHistory"), wpmPageHistoryList)
        Catch ex As Exception
            wpmLogging.AuditLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "wpmSession.New")
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
            mysb.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", ph.ViewTime, ph.PageName, ph.RequestURL, ph.PageSource))
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function

#End Region
    Sub New(ByVal ThisSession As System.Web.SessionState.HttpSessionState)
        mySession = ThisSession
        If CompanyID.Trim = String.Empty Then
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
            wpmLogging.AuditLog("Problem checking parameters - " & ex.ToString, "wpmSession.CheckcommandParameters")
        End Try
    End Sub
    Public Shared Function GetSiteDB() As String
        Return CStr(HttpContext.Current.Session.Item("SiteDB"))
    End Function
    Public Shared Function GetCompanyID() As String
        Return CStr(HttpContext.Current.Session.Item("CompanyID"))
    End Function
    Public Shared Function GetLogin_Link() As String
        If CStr(HttpContext.Current.Session.Item("Login_Link")) = "" Then
            Return CStr(HttpContext.Current.Session.Item("ListPage"))
        Else
            Return CStr(HttpContext.Current.Session.Item("Login_Link"))
        End If
    End Function
    Public Shared Function SetLogin_Link(ByVal sLogin_Link As String) As Boolean
        HttpContext.Current.Session.Item("Login_Link") = sLogin_Link
        Return True
    End Function
    Public Shared Function AppendRightContent(ByVal sText As String) As Boolean
        HttpContext.Current.Session.Item("RightContent") = HttpContext.Current.Session.Item("RightContent").ToString & sText
        Return True
    End Function
    Public Shared Function SetRightContent(ByVal sText As String) As Boolean
        HttpContext.Current.Session.Item("RightContent") = sText
        Return True
    End Function
    Public Shared Function GetRightContent() As String
        Return CStr(HttpContext.Current.Session.Item("RightContent"))
    End Function
    Public Shared Function GetUserGroup() As String
        If CStr(HttpContext.Current.Session.Item("GroupID")) = "" Then
            HttpContext.Current.Session.Item("GroupID") = "4"
        End If
        Return CStr(HttpContext.Current.Session.Item("GroupID"))
    End Function
    Public Shared Function SetUserGroup(ByVal sGroupID As String) As Boolean
        HttpContext.Current.Session.Item("GroupID") = sGroupID
        Return True
    End Function
    Public Shared Function GetContactID() As String
        Return CStr(HttpContext.Current.Session.Item("ContactID"))
    End Function
    Public Shared Function SetContactID(ByVal sContactID As String) As Boolean
        HttpContext.Current.Session.Item("ContactID") = sContactID
        Return True
    End Function
    Public Shared Function GetUserName() As String
        Return CStr(HttpContext.Current.Session.Item("UserName"))
    End Function
    Public Shared Function SetUserName(ByVal sUserName As String) As Boolean
        HttpContext.Current.Session.Item("UserName") = sUserName
        Return True
    End Function
    Public Shared Function GetUserEmail() As String
        Return CStr(HttpContext.Current.Session.Item("UserEmail"))
    End Function
    Public Shared Function SetUserEmail(ByVal sUserEmail As Object) As Boolean
        HttpContext.Current.Session.Item("UserEmail") = sUserEmail
        Return True
    End Function
    Public Shared Function GetUserRoleTitle() As String
        Return CStr(HttpContext.Current.Session.Item("UserRoleTitle"))
    End Function
    Public Shared Function SetUserRoleTitle(ByVal sUserRoleTitle As String) As Boolean
        HttpContext.Current.Session.Item("UserRoleTitle") = sUserRoleTitle
        SetUserRoleTitle = True
    End Function
    Public Shared Function GetUserRoleID() As String
        GetUserRoleID = CStr(HttpContext.Current.Session.Item("UserRoleID"))
    End Function
    Public Shared Function SetUserRoleID(ByVal sUserRoleID As String) As Boolean
        HttpContext.Current.Session.Item("UserRoleID") = sUserRoleID
        SetUserRoleID = True
    End Function
    Public Shared Function GetCurrentSQL() As String
        GetCurrentSQL = CStr(HttpContext.Current.Session.Item("CurrentSQL"))
    End Function
    Public Shared Function SetCurrentSQL(ByVal sCurrentSQL As String) As Boolean
        HttpContext.Current.Session.Item("CurrentSQL") = sCurrentSQL
        SetCurrentSQL = True
    End Function
    Public Shared Function GetUserRoleFilterMenu() As Boolean
        Return CBool(HttpContext.Current.Session.Item("UserRoleFilterMenu"))
    End Function
    Public Shared Function SetUserRoleFilterMenu(ByVal sUserRoleFilterMenu As String) As Boolean
        HttpContext.Current.Session.Item("UserRoleFilterMenu") = sUserRoleFilterMenu
        SetUserRoleFilterMenu = True
    End Function
    Public Shared Function GetDefaultSitePrefix() As String
        If IsDBNull(HttpContext.Current.Session.Item("DefaultSitePrefix")) Then
            GetDefaultSitePrefix = "ff"
        Else
            GetDefaultSitePrefix = CStr(HttpContext.Current.Session.Item("DefaultSitePrefix"))
        End If
    End Function
    Public Shared Function GetSiteHomePageID() As String
        GetSiteHomePageID = CStr(HttpContext.Current.Session.Item("SiteHomePageID"))
    End Function
    Private Function GetConfig() As Boolean
        Dim bReturn As Boolean = False
        Try
            CompanyID = wpmApp.SiteList.FindSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")).CompanyID
            If CompanyID = String.Empty Then
                If Not IsNothing(wpmApp.wpmConfigFile) Then
                    Dim SiteConfig As wpmSiteSettings = wpmSiteSettings.Load(wpmApp.wpmConfigFile)
                    CompanyID = SiteConfig.mySite.CompanyID
                    wpmApp.SiteList.Update(New wpmSite With {.DomainName = Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), .CompanyID = CompanyID, .AccessDatabasePath = .AccessDatabasePath})
                    bReturn = True
                End If
            Else
                bReturn = True
            End If
        Catch ex As Exception
            ' wpmLogging.AuditLog(ex.ToString, "GetConfig")
        End Try
        Return bReturn
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

        mysb.Append(String.Format("<br><strong>Caching Enabled:</strong>{0}<br/>", wpmApp.Config.CachingEnabled))
        mysb.Append(String.Format("<br><strong>Use 404 Processing Enabled:</strong>{0}<br/>", wpmApp.Config.Use404Processing))
        mysb.Append(GetPageHistory())

        Return mysb.ToString
    End Function




End Class
