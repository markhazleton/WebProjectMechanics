
Public Module ApplicationUser
    Public Function wpm_ProcessUserLogin(ByVal sUserName As String, ByVal sPassword As String) As Boolean
        Dim bReturn As Boolean = False
        If wpm_GetContact(sUserName, sPassword).IsActive Then
            wpm_UpdateUserOptions(wpm_ContactID)
            ApplicationLogging.AccessLog("ApplicationUser.ProcessLogin", String.Format("LOGIN - {0}", sUserName))
            bReturn = True
        End If
        Return bReturn
    End Function
    Public Function wpm_GetContact(ByVal sLogonName As String, ByVal sPassword As String) As UserInfo
        wpm_CheckCommandParameters()
        wpm_SetPageHistory()
        Dim myUser As New UserInfo With {.UserID = -1, .IsActive = False, .DisplayName = "Guest"}
        Dim strSQL As String = (String.Format("SELECT Contact.ContactID, Contact.LogonName, Contact.PrimaryContact,  Contact.GroupID,  Contact.LogonPassword,  Contact.Active,  Contact.EMail,  Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate, Company.DefaultArticleID, Company.HomePageID, Company.DefaultSiteTemplate FROM Contact, Company  WHERE Contact.CompanyID=Company.CompanyID AND Contact.LogonName = '{0}' AND Contact.LogonPassword = '{1}' ", _
                                    Replace(sLogonName, " ", String.Empty), _
                                    Replace(sPassword, " ", String.Empty)))

        For Each myRow As DataRow In wpm_GetDataTable(strSQL, "ApplicationUser.GetContact for Login").Rows
            If CBool(myRow.Item("Active")) Then
                With myUser
                    .IsActive = True
                    .UserID = CInt(myRow.Item("ContactID"))
                    .DisplayName = myRow.Item("PrimaryContact").ToString
                    .LastloginDate = Now()
                    .EmailAddress = myRow.Item("EMail").ToString
                    .GroupID = wpm_GetDBInteger(myRow.Item("GroupID"))
                    .companyID = wpm_GetDBInteger(myRow.Item("CompanyID"))
                End With


                ' **************************************************************************
                ' Update the Session User Object
                ' **************************************************************************
                wpm_SetContactID(myRow.Item("ContactID").ToString)

                wpm_ContactName = myRow.Item("PrimaryContact").ToString
                wpm_CurrentUserGroupID = myRow.Item("GroupID").ToString
                If IsDBNull(myRow.Item("EMail")) Then
                    wpm_ContactEmail = String.Empty
                Else
                    wpm_ContactEmail = myRow.Item("EMail").ToString
                End If
            End If
            Exit For
        Next
        Return myUser
    End Function
    Public Function wpm_UpdateUserOptions(ByVal ContactID As String) As String
        Dim sReturn As Integer
        If CStr(ContactID) <> String.Empty Then
            sReturn = wpm_RunUpdateSQL(String.Format("UPDATE [contact] SET [contact].[CreateDT]='{0}'  WHERE [contact].[ContactID]={1} ", FormatDateTime(Now), CStr(ContactID)), "ApplicationUser.UpdateUserOptions")
        End If
        Return sReturn.ToString
    End Function
End Module
