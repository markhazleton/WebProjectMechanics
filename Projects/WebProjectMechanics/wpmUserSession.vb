Imports System.Web.SessionState
Imports System.Text

Public Class wpmUserSession
    Protected mySession As SessionState.HttpSessionState
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
    Public Property ListPageURL() As String
        Get
            If IsNothing(mySession("ListPageURL")) Then
                Return ("~/")
            Else
                Return wpmUtil.GetDBString(mySession.Item("ListPageURL"))
            End If
        End Get
        Set(ByVal value As String)
            mySession.Item("ListPageURL") = value
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

    Public Shared Function GetSiteDB() As String
        Return CStr(HttpContext.Current.Session.Item("SiteDB"))
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
        Return mysb.ToString
    End Function

End Class
