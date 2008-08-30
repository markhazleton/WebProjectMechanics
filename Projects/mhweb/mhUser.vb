Public Class mhUser
    Public Shared Function ProcessLogin(ByVal sUserName As String, ByVal sPassword As String, ByRef mySession As mhSession) As Boolean
        Dim bProcessLogin As Boolean = False
        If GetContact(sUserName, sPassword, mySession) Then
            UpdateUserOptions(mySession.ContactID(), mySession)
            mhUTIL.AuditLog("ProcessLogin", "LOGIN")
            Return True
        End If
    End Function
    '********************************************************************************
    Public Shared Function GetContact(ByVal sLogonName As String, ByVal sPassword As String, ByRef mySession As mhSession) As Boolean
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

        For Each myRow As DataRow In mhweb.mhDB.GetDataTable(strSQL, "GetContact for Login").Rows
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
    Public Shared Function UpdateUserOptions(ByVal ContactID As String, ByRef mySession As mhSession) As String
        Dim strSQL As String
        Dim sReturn As Integer
        If CStr(ContactID) <> "" Then
            strSQL = "UPDATE [contact] "
            strSQL = strSQL & " SET"
            strSQL = strSQL & " [contact].[CreateDT]='" & FormatDateTime(Now) & "' "
            strSQL = strSQL & " WHERE [contact].[ContactID]=" & CStr(ContactID)
            sReturn = mhDB.RunUpdateSQL(strSQL, "UpdateUserOptions")
        End If
        Return sReturn.ToString
    End Function
    '********************************************************************************
    Public Shared Function IsAdmin() As Boolean
        If (mhSession.GetUserGroup() = "1") Then
            Return True
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function IsEditor() As Boolean
        Dim sUserGroup As String = mhSession.GetUserGroup()
        If (sUserGroup = "1" Or sUserGroup = "2") Then
            Return True
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function IsUser() As Boolean
        Dim sUserGroup As String = mhSession.GetUserGroup()
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
        If (mhSession.GetContactID() <> "") Then
            sReturn = sReturn & ("<b>Name:</b>" & sUserOptions & "<br />&nbsp;&nbsp;" & mhutil.FormatLink(mhSession.GetContactID(), mhSession.GetUserName(), "Contact", "/mhwcm/login/contact_edit.aspx"))
        End If
        GetUserInformation = sReturn
    End Function
    '********************************************************************************
    Public Shared Function GetUserOptions() As String
        Dim sReturn As String
        sReturn = ""
        If mhSession.GetContactID() <> "" Then
            sReturn = sReturn & "<a href=""" & mhConfig.mhWebHome & "login/logout.aspx"" >Sign Out</a>"
        Else
            sReturn = sReturn & "<a href=""" & mhConfig.mhWebHome & "login/login.aspx"">Sign On</a>"
        End If
        GetUserOptions = sReturn
    End Function
    '********************************************************************************
End Class
