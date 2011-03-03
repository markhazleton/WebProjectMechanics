Imports System.Web
Imports System.Text

Public Class wpmUser
    Public Shared Function ProcessLogin(ByVal sUserName As String, ByVal sPassword As String, ByRef mySession As wpmSession) As Boolean
        Dim bReturn As Boolean = False
        If GetContact(sUserName, sPassword, mySession) Then
            UpdateUserOptions(mySession.ContactID())
            wpmLogging.AccessLog("wpmUser.ProcessLogin", String.Format("LOGIN - {0}", sUserName))
            bReturn = True
        End If
        Return bReturn
    End Function
    '********************************************************************************
    Public Shared Function GetContact(ByVal sLogonName As String, ByVal sPassword As String, ByRef mySession As wpmSession) As Boolean
        Dim bGetContact As Boolean = False
        Dim strSQL As String = (String.Format("SELECT Contact.ContactID, Contact.LogonName, Contact.PrimaryContact,  Contact.GroupID,  Contact.LogonPassword,  Contact.Active,  Contact.EMail,  Contact.TemplatePrefix,  Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate, Company.DefaultArticleID, Company.HomePageID, Company.DefaultSiteTemplate, '' as RoleTitle, '' as RoleID, 'False' as FilterMenu FROM Contact, Company  WHERE Contact.CompanyID=Company.CompanyID AND Contact.LogonName = '{0}' AND Contact.LogonPassword = '{1}' ", _
                                    Replace(sLogonName, " ", ""), _
                                    Replace(sPassword, " ", "")))

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
    Public Shared Function UpdateUserOptions(ByVal ContactID As String) As String
        Dim sReturn As Integer
        If CStr(ContactID) <> "" Then
            sReturn = wpmDB.RunUpdateSQL(String.Format("UPDATE [contact] SET [contact].[CreateDT]='{0}'  WHERE [contact].[ContactID]={1} ", FormatDateTime(Now), CStr(ContactID)), "wpmUser.UpdateUserOptions")
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
    Public Shared Function GetUserInformation(ByRef sUserOptions As String) As String
        Dim sReturn As String = If(wpmSession.GetContactID() <> "", "" & (String.Format("<b>Name:</b>{0}<br />&nbsp;&nbsp;{1}", sUserOptions, wpmUtil.FormatLink(wpmSession.GetUserName(), "Contact", "/mhwcm/login/contact_edit.aspx"))), "")
        GetUserInformation = sReturn
    End Function
    '********************************************************************************
    Public Shared Function GetUserOptions() As String
        Dim sReturn As String = ""
        If wpmSession.GetContactID() <> "" Then
            sReturn = String.Format("{0}<a href=""{1}login/logout.aspx"" >Sign Out</a>", sReturn, wpmApp.Config.wpmWebHome())
        Else
            sReturn = String.Format("{0}<a href=""{1}login/login.aspx"">Sign On</a>", sReturn, wpmApp.Config.wpmWebHome())
        End If
        GetUserOptions = sReturn
    End Function
    '********************************************************************************
    Public Shared Sub wpmProcessLogin(ByVal UserName As String, ByVal UserPwd As String)
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
            myStringBuilder.Append(String.Format("<script language=""JavaScript"" type=""text/JavaScript"">{0}", vbCrLf))
            myStringBuilder.Append("<!--" & vbCrLf)
            myStringBuilder.Append("function MM_openBrWindow(theURL,winName,features) { window.open(theURL,winName,features); }" & vbCrLf)
            myStringBuilder.Append("//-->" & vbCrLf)
            myStringBuilder.Append(String.Format("</script>{0}", vbCrLf))
            myStringBuilder.Append("<h1>Login Failed</h1><h2>The combination of email address and password were not found.</h2>")
            ' **************************************************************************
            ' Update the Session User Object
            ' **************************************************************************
            wpmSession.SetContactID("")
            wpmSession.SetUserName("UNKNOWN")
            wpmSession.SetUserGroup("4")
        End If
        'myMessage.Text = myStringBuilder.ToString
        'Session("CurrentPageID") = "LoginPage"
    End Sub

End Class
