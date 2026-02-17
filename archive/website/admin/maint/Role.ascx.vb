Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_Role
    Inherits ApplicationUserControl
    ' Role (Page) Type  
    Public Const STR_RoleID As String = "RoleID"
    Public Const STR_SELECTRoleList As String = "SELECT Role.[RoleID], Role.[RoleName], Role.[RoleTitle], Role.[RoleComment] FROM Role;"
    Public Const STR_SELECT_RoleByRoleID As String = "SELECT Role.[RoleID], Role.[RoleName], Role.[RoleTitle], Role.[RoleComment] FROM Role where [Role].[RoleID]={0};"
    Public Const STR_UPDATE_Role As String = "UPDATE Role SET Role.RoleName = @RoleName , Role.RoleTitle = @RoleTitle, Role.RoleComment = @RoleComment WHERE (((Role.RoleID)=@RoleID));"
    Public Const STR_INSERT_Role As String = "INSERT INTO Role ( RoleName , RoleTitle , RoleComment ) VALUES ( @RoleName ,@RoleTitle, @RoleComment ); "
    Public Const STR_DELETE_Role As String = "DELETE FROM [Role] WHERE [Role].[RoleID]=@RoleID;"
    Private Property reqRoleID As String
    Public Class Role
        Public Property RoleID As String
        Public Property RoleName As String
        Public Property RoleTitle As String
        Public Property RoleComment As String
    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqRoleID = GetProperty(STR_RoleID, String.Empty)
        If Not IsPostBack Then

            If reqRoleID = String.Empty Then
                dtList.Visible = True
                pnlEdit.Visible = False
                ' Show the list
                Dim myRoleList As New List(Of Role)
                For Each myRow In wpm_GetDataTable(STR_SELECTRoleList, "Role").Rows
                    myRoleList.Add(New Role With {.RoleID = wpm_GetDBInteger(myRow("RoleID")),
                                                          .RoleName = wpm_GetDBString(myRow("RoleName")),
                                                          .RoleComment = wpm_GetDBString(myRow("RoleComment")),
                                                          .RoleTitle = wpm_GetDBString(myRow("RoleTitle"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Role", .DetailKeyName = "RoleID", .DetailFieldName = "RoleName", .DetailPath = "/admin/maint/default.aspx?type=Role&RoleID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "RoleDS", .Value = "RoleTitle"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "RoleComment", .Value = "RoleComment"})
                Dim myList As New List(Of Object)
                myList.AddRange(myRoleList)
                dtList.BuildTable(myListHeader, myList)

            ElseIf reqRoleID = "0" Or Not (IsNumeric(reqRoleID)) Then
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                dtList.Visible = False
                pnlEdit.Visible = True
                GetRoleForEdit(reqRoleID)
            Else
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetRoleForEdit(reqRoleID)
            End If
        End If
    End Sub

    Private Function GetRoleByRoleID(ByVal reqRoleID As String) As Role
        Dim myRole As New Role With {.RoleID = reqRoleID}
        If IsNumeric(reqRoleID) AndAlso CInt(reqRoleID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_RoleByRoleID, reqRoleID), "Role").Rows
                With myRole
                    .RoleComment = wpm_GetDBString(myRow("RoleComment"))
                    .RoleName = wpm_GetDBString(myRow("RoleName"))
                    .RoleTitle = wpm_GetDBString(myRow("RoleTitle"))
                    .RoleID = wpm_GetDBString(myRow("RoleID"))
                End With
                Exit For
            Next
        Else
            myRole.RoleID = "0"
        End If
        Return myRole
    End Function
    Private Sub GetRoleForEdit(reqRoleID As String)
        Dim myRole As Role = GetRoleByRoleID(reqRoleID)
        tbRoleName.Text = myRole.RoleName
        tbRoleTitle.Text = myRole.RoleTitle
        tbRoleNM.Text = myRole.RoleComment
        litRoleID.Text = myRole.RoleID
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim myRole As Role = GetRoleByRoleID(litRoleID.Text)
        With myRole
            .RoleName = wpm_GetDBString(tbRoleName.text)
            .RoleTitle = wpm_GetDBString(tbRoleTitle.text)
            .RoleComment = wpm_GetDBString(tbRoleNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_Role}
                    wpm_AddParameterStringValue("@RoleName", myRole.RoleName, cmd)
                    wpm_AddParameterStringValue("@RoleTitle", myRole.RoleTitle, cmd)
                    wpm_AddParameterStringValue("@RoleComment", myRole.RoleComment, cmd)
                    wpm_AddParameterStringValue("@RoleID", myRole.RoleID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_UPDATE_Role, "Role.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myRole As Role = GetRoleByRoleID(litRoleID.Text)
        With myRole
            .RoleName = wpm_GetDBString(tbRoleName.text)
            .RoleTitle = wpm_GetDBString(tbRoleTitle.text)
            .RoleComment = wpm_GetDBString(tbRoleNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_Role}
                    wpm_AddParameterStringValue("@RoleName", myRole.RoleName, cmd)
                    wpm_AddParameterStringValue("@RoleTitle", myRole.RoleTitle, cmd)
                    wpm_AddParameterStringValue("@RoleComment", myRole.RoleComment, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_INSERT_Role, "Role.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myRole As Role = GetRoleByRoleID(litRoleID.Text)
        With myRole
            .RoleName = wpm_GetDBString(tbRoleName.text)
            .RoleTitle = wpm_GetDBString(tbRoleTitle.text)
            .RoleComment = wpm_GetDBString(tbRoleNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_Role}
                    wpm_AddParameterStringValue("@RoleID", myRole.RoleID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_DELETE_Role, "Role.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
End Class
