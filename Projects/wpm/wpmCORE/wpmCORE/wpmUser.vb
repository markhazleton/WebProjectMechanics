Imports System.Web

Public Class wpmUser
    Public Shared Function ProcessLogin(ByVal sUserName As String, ByVal sPassword As String, ByRef mySession As wpmSession) As Boolean
        Dim bProcessLogin As Boolean = False
        If GetContact(sUserName, sPassword, mySession) Then
            UpdateUserOptions(mySession.ContactID(), mySession)
            wpmUTIL.AccessLog("wpmUser.ProcessLogin", "LOGIN - " & sUserName)
            Return True
        End If
    End Function
    '********************************************************************************
    Public Shared Function GetContact(ByVal sLogonName As String, ByVal sPassword As String, ByRef mySession As wpmSession) As Boolean
        Dim bGetContact As Boolean = False
        Dim strSQL As String = ("SELECT Contact.ContactID, " & _
                        "Contact.LogonName, " & _
                        "Contact.PrimaryContact,  " & _
                        "Contact.GroupID,  " & _
                        "Contact.LogonPassword,  " & _
                        "Contact.Active,  " & _
                        "Contact.EMail,  " & _
                        "Contact.TemplatePrefix,  " & _
                        "Company.CompanyID,  " & _
                        "Company.CompanyName,  " & _
                        "Company.GalleryFolder,  " & _
                        "Company.SiteURL,  " & _
                        "Company.SiteTitle,  " & _
                        "Company.SiteTemplate, " & _
                        "Company.DefaultArticleID, " & _
                        "Company.HomePageID, " & _
                        "Company.DefaultSiteTemplate, " & _
                        "'' as RoleTitle, " & _
                        "'' as RoleID, " & _
                        "'False' as FilterMenu " & _
                   "FROM Contact, Company  " & _
                  "WHERE Contact.CompanyID=Company.CompanyID " & _
                    "AND Contact.LogonName = '" & Replace(sLogonName, " ", "") & "' " & _
                    "AND Contact.LogonPassword = '" & Replace(sPassword, " ", "") & "' ")

        For Each myRow As DataRow In wpmDB.GetDataTable(strSQL, "wpmUser.GetContact for Login").Rows
            If CBool(myRow.Item("Active")) Then
                bGetContact = True
                ' **************************************************************************
                ' Update the Session User Object
                ' **************************************************************************
                mySession.ContactID = myRow.Item("ContactID").ToString
                mySession.ContactName = myRow.Item("PrimaryContact").ToString
                mySession.GroupID = myRow.Item("GroupID").ToString
                If IsDBNull(myRow.Item("EMail")) Then
                    mySession.ContactEmail = ""
                Else
                    mySession.ContactEmail = myRow.Item("EMail").ToString
                End If
                mySession.ContactRoleTitle = myRow.Item("RoleTitle").ToString
                mySession.ContactRoleID = myRow.Item("RoleID").ToString
                mySession.ContactRoleFilterMenu = myRow.Item("FilterMenu").ToString
                mySession.CompanyID = myRow.Item("CompanyID").ToString
                mySession.SiteTemplatePrefix = myRow.Item("TemplatePrefix").ToString
            End If
            Exit For
        Next
        Return bGetContact
    End Function
    Public Shared Function UpdateUserOptions(ByVal ContactID As String, ByRef mySession As wpmSession) As String
        Dim strSQL As String
        Dim sReturn As Integer
        If CStr(ContactID) <> "" Then
            strSQL = "UPDATE [contact] "
            strSQL = strSQL & " SET"
            strSQL = strSQL & " [contact].[CreateDT]='" & FormatDateTime(Now) & "' "
            strSQL = strSQL & " WHERE [contact].[ContactID]=" & CStr(ContactID)
            sReturn = wpmDB.RunUpdateSQL(strSQL, "wpmUser.UpdateUserOptions")
        End If
        Return sReturn.ToString
    End Function
    '********************************************************************************
    Public Shared Function IsAdmin() As Boolean
        If (wpmSession.GetUserGroup() = "1") Then
            Return True
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function IsEditor() As Boolean
        Dim sUserGroup As String = wpmSession.GetUserGroup()
        If (sUserGroup = "1" Or sUserGroup = "2") Then
            Return True
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function IsUser() As Boolean
        Dim sUserGroup As String = wpmSession.GetUserGroup()
        If (sUserGroup <> "4") Then
            Return True
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Private Function GetUserInformation(ByRef sUserOptions As String) As String
        Dim sReturn As String
        sReturn = ""
        If (wpmSession.GetContactID() <> "") Then
            sReturn = sReturn & ("<b>Name:</b>" & sUserOptions & "<br />&nbsp;&nbsp;" & wpmUTIL.FormatLink(wpmSession.GetContactID(), wpmSession.GetUserName(), "Contact", "/mhwcm/login/contact_edit.aspx"))
        End If
        GetUserInformation = sReturn
    End Function
    '********************************************************************************
    Public Shared Function GetUserOptions() As String
        Dim sReturn As String
        sReturn = ""
        If wpmSession.GetContactID() <> "" Then
            sReturn = sReturn & "<a href=""" & App.Config.wpmWebHome() & "login/logout.aspx"" >Sign Out</a>"
        Else
            sReturn = sReturn & "<a href=""" & App.Config.wpmWebHome() & "login/login.aspx"">Sign On</a>"
        End If
        GetUserOptions = sReturn
    End Function
    '********************************************************************************
    Public Sub wpmProcessLogin(ByVal UserName As String, ByVal UserPwd As String)
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim mySession As New wpmSession(HttpContext.Current.Session)
        myStringBuilder.Append(HttpContext.Current.Session("msg"))
        HttpContext.Current.Session("msg") = ""
        If wpmUser.ProcessLogin(UserName, UserPwd, mySession) Then
            'We are logged In
            If wpmUser.IsAdmin Then
            End If

            'If (wpmSession.GetLogin_Link() = "") Then
            '    Response.Redirect("/")
            'Else
            '    Response.Redirect(wpmSession.GetLogin_Link())
            'End If
        Else
            'We are NOT logged In
            myStringBuilder.Append("<sc" & "ript language=""JavaScript"" type=""text/JavaScript"">" & vbCrLf)
            myStringBuilder.Append("<!--" & vbCrLf)
            myStringBuilder.Append("function MM_openBrWindow(theURL,winName,features) { window.open(theURL,winName,features); }" & vbCrLf)
            myStringBuilder.Append("//-->" & vbCrLf)
            myStringBuilder.Append("</scr" & "ipt>" & vbCrLf)
            myStringBuilder.Append("<h1>Login Failed</h1><h2>The combination of email address and password were not found.</h2>")
            ' **************************************************************************
            ' Update the Session User Object
            ' **************************************************************************
            wpmSession.SetContactID("")
            wpmSession.SetUserName("")
            wpmSession.SetUserGroup("4")
        End If
        'myMessage.Text = myStringBuilder.ToString
        'Session("CurrentPageID") = "LoginPage"
    End Sub

End Class
