Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_SiteTemplate
    Inherits ApplicationUserControl

    ' Site Template
    Public Const STR_SELECTSiteTemplateList As String = "SELECT SiteTemplate.[TemplatePrefix] as TemplatePrefix , SiteTemplate.[Name] as TemplateName FROM SiteTemplate;"
    Public Const STR_TemplateCD As String = "TemplatePrefix"
    Public Const STR_SELECT_SiteTemplateByTemplateCD As String = "SELECT * FROM SiteTemplate where SiteTemplate.[TemplatePrefix]='{0}';"
    Public Const STR_UPDATE_SiteTemplate As String = "UPDATE SiteTemplate SET [Top] = @Top, [Bottom]=@Bottom, SiteTemplate.[Name]=@TemplateName  WHERE SiteTemplate.[TemplatePrefix] = @TemplatePrefix "
    Public Const STR_INSERT_SiteTemplate As String = "INSERT INTO [SiteTemplate] ([Top],[Bottom],[Name],[TemplatePrefix]) values (@Top, @Bottom, @TemplateName,  @TemplatePrefix  ) "
    Public Const STR_DELETE_SiteTemplate As String = "DELETE FROM [SiteTemplate] WHERE [SiteTemplate].[TemplatePrefix]='{0}';"

    Public Property reqTemplateCD As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqTemplateCD = GetProperty(STR_TemplateCD, String.Empty)
        If Not IsPostBack Then
            If reqTemplateCD <> String.Empty Then
                If reqTemplateCD.ToUpper = "NEW" Then
                    tbTemplateCD.Enabled=True
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                Else
                    tbTemplateCD.Enabled=False
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Edit Mode
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    GetSiteTemplateForEdit()
                End If
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True


                Dim myTemplateList As New List(Of SiteTemplate)
                For Each myRow In wpm_GetDataTable(STR_SELECTSiteTemplateList, "SiteTemplate").Rows
                    myTemplateList.Add(New SiteTemplate() With {.TemplatePrefix = wpm_GetDBString(myRow("TemplatePrefix")),
                                                                .TemplateName = wpm_GetDBString(myRow("TemplateName"))})
                Next

                Dim myListHeader As New DisplayTableHeader() With {
                    .TableTitle = "Site Templates (<a href='/admin/maint/default.aspx?type=SiteTemplate&TemplatePrefix=NEW'>New Template</a>)", 
                    .DetailKeyName = "TemplatePrefix", 
                    .DetailFieldName = "TemplatePrefix", 
                    .DetailPath = "/admin/maint/default.aspx?type=SiteTemplate&TemplatePrefix={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "TemplateName", .Value = "TemplateName"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "TemplatePrefix", .Value = "TemplatePrefix"})
                Dim myList As New List(Of Object)
                myList.AddRange(myTemplateList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_SiteTemplate}
                    wpm_AddParameterStringValue("@Top", tbTop.Text, cmd)
                    wpm_AddParameterStringValue("@Bottom", tbBottom.Text, cmd)
                    wpm_AddParameterStringValue("@TemplateName", tbTemplateNM.Text, cmd)
                    wpm_AddParameterStringValue("@TemplatePrefix", tbTemplateCD.Text, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_SiteTemplate, "SiteTemplate.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteTemplate}
                    wpm_AddParameterStringValue("@Top", tbTop.Text, cmd)
                    wpm_AddParameterStringValue("@Bottom", tbBottom.Text, cmd)
                    wpm_AddParameterStringValue("@TemplateName", tbTemplateNM.Text, cmd)
                    wpm_AddParameterStringValue("@TemplatePrefix", tbTemplateCD.Text, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_SiteTemplate, "SiteTemplate.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        wpm_RunDeleteSQL(String.Format(STR_DELETE_SiteTemplate, litTemplateCD.Text), "SiteTemplate")
        OnUpdated(Me)
    End Sub

    Private Sub GetSiteTemplateForEdit()
        For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_SiteTemplateByTemplateCD, reqTemplateCD), "SiteTemplate").Rows
            litTemplateCD.Text = wpm_GetDBString(myRow("TemplatePrefix"))
            tbTemplateCD.Text = wpm_GetDBString(myRow("TemplatePrefix"))
            tbTemplateNM.Text = wpm_GetDBString(myRow("Name"))
            tbTop.Text = wpm_GetDBString(myRow("Top"))
            tbBottom.Text = wpm_GetDBString(myRow("Bottom"))
        Next
    End Sub
End Class
