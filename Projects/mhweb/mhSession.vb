Public Class mhSession
    Private mySession As System.Web.SessionState.HttpSessionState
    Public Property CompanyID() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("CompanyID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("CompanyID") = value
        End Set
    End Property
    Public Property SiteCategoryTypeID() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("SiteCategoryTypeID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteCategoryTypeID") = value
        End Set
    End Property

    'Public Property SingleSiteGallery() As Boolean
    '    Get
    '        Return mhUTIL.GetDBBoolean(mySession.Item("SingleSiteGallery"))
    '    End Get
    '    Set(ByVal value As Boolean)
    '        mySession.Item("SingleSiteGallery") = value
    '    End Set
    'End Property

    Public Property CurrentPageID() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("CurrentPageID"))
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
            Return mhUTIL.GetDBString(mySession.Item("CurrentArticleID"))
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
            Return mhUTIL.GetDBString(mySession.Item("SiteTemplatePrefix"))
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
            Return mhUTIL.GetDBString(mySession.Item("GroupID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("GroupID") = value
        End Set
    End Property
    Public Property ContactID() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactID") = value
        End Set
    End Property
    Public Property ContactName() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactName"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactName") = value
        End Set
    End Property
    Public Property ContactEmail() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactEmail"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactEmail") = value
        End Set
    End Property
    Public Property ContactRoleTitle() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactRoleTitle"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleTitle") = value
        End Set
    End Property
    Public Property ContactRoleID() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactRoleID"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleID") = value
        End Set
    End Property
    Public Property ContactRoleFilterMenu() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ContactRoleFilterMenu"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ContactRoleFilterMenu") = value
        End Set
    End Property
    Public Property SiteDB() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("SiteDB"))
        End Get
        Set(ByVal value As String)
            mySession.Item("SiteDB") = value
        End Set
    End Property
    Public Property ListPageURL() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("ListPageURL"))
        End Get
        Set(ByVal value As String)
            mySession.Item("ListPageURL") = value
        End Set
    End Property
    Public Property RightContent() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("RightContent"))
        End Get
        Set(ByVal value As String)
            mySession.Item("RightContent") = value
        End Set
    End Property
    Public Property AddHTMLHead() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("AddHTMLHead"))
        End Get
        Set(ByVal value As String)
            mySession.Item("AddHTMLHead") = value
        End Set
    End Property
    Private _siteGallery As String
    Public Property SiteGallery() As String
        Get
            Return mhUTIL.GetDBString(mySession.Item("SiteGallery"))
        End Get
        Set(ByVal Value As String)
            mySession.Item("SiteGallery") = Value
        End Set
    End Property
    
    Public ReadOnly Property SiteMapFilePath() As String
        Get
            Return mhfio.GetValidPath(mhConfig.mhWebConfig & "\index\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & " - " & CompanyID & " - " & GroupID & ".xml", "GetSiteMapFilePath")
        End Get
    End Property
    Sub New(ByVal ThisSession As System.Web.SessionState.HttpSessionState)
        mySession = ThisSession
        If Trim(SiteDB) = "" Then
            GetConfig()
        End If
        CheckCommandParameters()
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

        End Try
    End Sub
    Public Shared Function GetSiteDB() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("SiteDB"))
    End Function
    Public Shared Function GetCompanyID() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("CompanyID"))
    End Function
    Public Shared Function GetListPage() As String
        Return CStr(System.Web.HttpContext.Current.Session.Item("ListPage"))
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
        If Not IsNothing(mhConfig.mhWebConfig) Then
            Dim SiteConfig As MHSiteSettings = MHSiteSettings.Load(mhConfig.mhWebConfig)
            CompanyID = (SiteConfig.mhSite.CompanyID.ToString)
            SiteDB = (SiteConfig.mhSite.SQLDBConnString.ToString)
            'If (SiteConfig.mhSite.SingleSiteGallery.ToString = "TRUE") Then
            '    SingleSiteGallery = True
            'Else
            '    SingleSiteGallery = False
            'End If

            Return True
        Else
            Return False
        End If
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
        Return mysb.ToString
    End Function

End Class
