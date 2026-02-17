Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Partial Class admin_maint_Contact
    Inherits ApplicationUserControl
    Private Property reqContactID As String

    ' Contact
    Public Const STR_ContactID As String = "ContactID"

    Public Const STR_SelectContactList As String = "SELECT [Contact].[ContactID], [Contact].[GroupID], [Contact].[PrimaryContact], [Contact].[FirstName], [Contact].[MiddleInitial], [Contact].[LastName], [Contact].[EMail], [Contact].[LogonName], [Contact].[LogonPassword], [Contact].[CompanyID], [Contact].[Active] FROM Contact  "

    Public Const STR_SelectContactByContactID As String = "SELECT [Contact].[ContactID], [Contact].[GroupID], [Contact].[PrimaryContact], [Contact].[FirstName], [Contact].[MiddleInitial], [Contact].[LastName], [Contact].[EMail], [Contact].[LogonName], [Contact].[LogonPassword], [Contact].[CompanyID], [Contact].[Active] FROM Contact WHERE [Contact].[ContactID]={0} "

    Public Const STR_UPDATE_Contact As String = "UPDATE [Contact] SET [PrimaryContact] = @PrimaryContact, [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [LogonPassword] = @LogonPassword, [LogonName] = @LogonName, [GroupID] = @GroupID, [CompanyID] = @CompanyID WHERE [ContactID] = @ContactID "

    Public Const STR_INSERT_Contact As String = "INSERT INTO Contact ([PrimaryContact] , [FirstName] , [LastName] , [Email] , [LogonPassword] , [LogonName] , [GroupID] , [CompanyID] ) VALUES (@PrimaryContact, @FirstName, @LastName, @Email, @LogonPassword, @LogonName, @GroupID, @CompanyID) "

    Public Const STR_DELETE_Contact As String = "DELETE FROM [Contact] WHERE [ContactID]= @ContactID "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqContactID = GetProperty(STR_ContactID, String.Empty)
        hfCompanyID.Value = wpm_CurrentSiteID()
        If Not IsPostBack Then
            If reqContactID <> String.Empty Then
                If reqContactID=0 Then
                    ContactIDLabel.Text = -1
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                Else
                    ' Edit Mode
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    GetContactForEdit(reqContactID)
                End If
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Dim myUserList As New List(Of UserInfo)
                For Each myRow In wpm_GetDataTable(STR_SelectContactList, "Contacts").Rows
                    myUserList.Add(New UserInfo With {
                                   .UserID = wpm_GetDBInteger(myRow("ContactID")),
                                   .companyID = wpm_GetDBInteger(myRow("CompanyID")),
                                   .FirstName = wpm_GetDBString(myRow("FirstName")),
                                   .LastName = wpm_GetDBString(myRow("LastName")),
                                   .EmailAddress = wpm_GetDBString(myRow("Email")),
                                   .LogonName = wpm_GetDBString(myRow("LogonName")),
                                   .Password = wpm_GetDBString(myRow("LogonPassword")),
                                   .DisplayName = wpm_GetDBString(myRow("PrimaryContact"))})
               Next

                Dim myListHeader As New DisplayTableHeader() With {
                    .TableTitle = "Site/Contact (<a href='/admin/maint/default.aspx?type=Contact&ContactID=0'>Add New Contact</a>)",
                    .DetailKeyName = "UserID",
                    .DetailFieldName = "LogonName",
                    .DetailPath = "/admin/maint/default.aspx?type=Contact&ContactID={0}"}
                myListHeader.AddHeaderItem("FirstName","FirstName")
                myListHeader.AddHeaderItem("LastName","LastName")
                myListHeader.AddHeaderItem("EmailAddress","EmailAddress")
                Dim myList As New List(Of Object)
                myList.AddRange(myUserList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_Contact}
                    wpm_AddParameterStringValue("@PrimaryContact", PrimaryContactTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@FirstName", FirstNameTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LastName", LastNameTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@EMail", EMailTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LogonPassword", LogonPasswordTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LogonName", LogonNameTextBox.Text, cmd)
                    wpm_AddParameterValue("@GroupID", ddlGroup.SelectedValue, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@CompanyID", hfCompanyID.Value, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@ContactID", ContactIDLabel.Text, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_Contact, "Contact.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_Contact}
                    wpm_AddParameterStringValue("@PrimaryContact", PrimaryContactTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@FirstName", FirstNameTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LastName", LastNameTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@EMail", EMailTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LogonPassword", LogonPasswordTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@LogonName", LogonNameTextBox.Text, cmd)
                    wpm_AddParameterValue("@GroupID", ddlGroup.SelectedValue, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@CompanyID", hfCompanyID.Value, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_Contact, "Contact.acsx - cmd_Update_Click")
                strErrorMessage = String.Format("Update Error:<br/>{0}", ex.Message)
            End Try
        End Using
        If String.IsNullOrWhiteSpace(strErrorMessage) Then
            OnUpdated(Me)
        Else
            litError.Text = String.Format("<blockquote>{0}</blockquote>", strErrorMessage)
        End If
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_Contact}
                    wpm_AddParameterValue("@ContactID", ContactIDLabel.Text, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_Contact, "Contact.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub
    Private Sub GetContactForEdit(ByVal myContactID As String)
        ContactIDLabel.Text = myContactID
        For Each myRow As DataRow In wpm_GetDataTable(String.Format(STR_SelectContactByContactID, myContactID), "Contact").Rows
            PrimaryContactTextBox.Text = wpm_GetDBString(myRow("PrimaryContact"))
            FirstNameTextBox.Text = wpm_GetDBString(myRow("FirstName"))
            LastNameTextBox.Text = wpm_GetDBString(myRow("LastName"))
            EMailTextBox.Text = wpm_GetDBString(myRow("Email"))
            LogonNameTextBox.Text = wpm_GetDBString(myRow("LogonName"))
            LogonPasswordTextBox.Text = wpm_GetDBString(myRow("LogonPassword"))
            ddlGroup.SelectedValue = wpm_GetDBInteger(myRow("GroupID"))
            ActiveCheckBox.Checked = wpm_GetDBBoolean(myRow("Active"))
            Exit For
        Next
    End Sub

End Class
