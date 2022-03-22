Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Partial Class admin_maint_PageAlias
    Inherits ApplicationUserControl
    ' Page Alias
    Public Const STR_PageAliasID As String = "PageAliasID"
    Public Const STR_SelectPageAliasList As String = "SELECT [PageAliasID],[PageURL], [TargetURL], [AliasType], [CompanyID] FROM [PageAlias]"
    Public Const STR_SelectPageAliasByPageAliasID As String = "SELECT [PageAliasID],[PageURL], [TargetURL], [AliasType], [CompanyID] FROM [PageAlias] where [PageAliasID] ={0} "
    Public Const STR_UPDATE_PageAlias As String = "UPDATE [PageAlias] SET [PageURL] = @PageURL, [TargetURL] = @TargetURL, [AliasType] = @AliasType, [CompanyID] = @CompanyID WHERE [PageAliasID] = @PageAliasID "
    Public Const STR_INSERT_PageAlias As String = "INSERT INTO [PageAlias] ([PageURL], [TargetURL], [AliasType], [CompanyID]) VALUES (@PageURL, @TargetURL, @AliasType, @CompanyID)"
    Public Const STR_DELETE_PageAlias As String = "DELETE FROM [PageAlias] WHERE [PageAliasID] = {0}"

    Private Property reqPageAlaisID As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqPageAlaisID = GetProperty(STR_PageAliasID, String.Empty)
        If Not IsPostBack Then
            If reqPageAlaisID <> String.Empty Then
                If reqPageAlaisID = "NEW" Then
                    PageAliasIDLabel1.Text = -1
                    pnlEdit.Visible = True
                    pnlList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                    GetPageAliasForEdit(reqPageAlaisID)
                Else
                    ' Edit Mode
                    pnlEdit.Visible = True
                    pnlList.Visible = False
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    GetPageAliasForEdit(reqPageAlaisID)
                End If
            Else
                ' Show the list
                pnlEdit.Visible = False
                pnlList.Visible = True

                Dim myWebHomePath As String = HttpContext.Current.Server.MapPath(wpm_SiteConfig.AdminFolder)
                Dim myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/PageAlias.xsl")
                myContent.Text = myXML.getXMLTransform()

            End If
        End If
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_PageAlias}
                    wpm_AddParameterStringValue("@PageURL", PageURLTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@TargetURL", TargetURLTextBox.Text, cmd)
                    wpm_AddParameterStringValue("@AliasType", "301", cmd)
                    wpm_AddParameterValue("@CompanyID", ddlCompany.SelectedValue, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@PageAliasID", PageAliasIDLabel1.Text, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_PageAlias, "PageAlias.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Private Sub GetPageAliasForEdit(ByVal myPageAlaisID As String)
        Dim myLocationAlias As New LocationAlias
        wmp_LoadCompanyDropDow(ddlCompany, wpm_DomainConfig.CompanyID, True)
        ddlCompany.Enabled = True

        For Each myRow In wpm_GetDataTable(String.Format(STR_SelectPageAliasByPageAliasID, myPageAlaisID), "PageAlais").Rows
            myLocationAlias.SetLocationAliasValue(myRow)
            With myLocationAlias
                PageAliasIDLabel1.Text = .PageAliasID
                TargetURLTextBox.Text = .TargetURL
                PageURLTextBox.Text = .PageURL
                AliasTypeTextBox.Text = .AliasType
                ddlCompany.SelectedValue = .CompanyID
            End With
        Next
    End Sub

End Class
