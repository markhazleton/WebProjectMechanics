Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_LinkType
    Inherits ApplicationUserControl
    ' Location (Page) Type  
    Public Const STR_LinkTypeCD As String = "LinkTypeCD"
    Public Const STR_SELECTLinkTypeList As String = "SELECT LinkType.LinkTypeCD, LinkType.LinkTypeDesc, LinkType.LinkTypeComment, LinkType.LinkTypeTarget FROM LinkType;"
    Public Const STR_SELECT_LinkTypeByLinkTypeCD As String = "SELECT LinkType.LinkTypeCD, LinkType.LinkTypeDesc, LinkType.LinkTypeComment, LinkType.LinkTypeTarget FROM LinkType where LinkTypeCD='{0}';"
    Public Const STR_UPDATE_LinkType As String = "UPDATE LinkType SET LinkType.LinkTypeCD = @LinkTypeCD , LinkType.LinkTypeDesc = @LinkTypeDesc, LinkType.LinkTypeComment = @LinkTypeComment, LinkType.LinkTypeTarget=@LinkTypeTarget WHERE (((LinkType.LinkTypeCD)=@LinkTypeCD));"
    Public Const STR_INSERT_LinkType As String = "INSERT INTO LinkType ( LinkTypeCD , LinkTypeDesc , LinkTypeComment ) VALUES ( @LinkTypeCD ,@LinkTypeDesc, @LinkTypeComment ); "
    Public Const STR_DELETE_LinkType As String = "DELETE FROM [LinkType] WHERE [LinkType].[LinkTypeCD]=@LinkTypeCD;"
    Private Property reqLinkTypeCD As String
    Public Class LinkType
        Public Property LinkTypeCD As String
        Public Property LinkTypeDesc As String
        Public Property LinkTypeComment As String
        Public Property LinkTypeTarget As String

    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqLinkTypeCD = GetProperty(STR_LinkTypeCD, String.Empty)
        If Not IsPostBack Then

            If reqLinkTypeCD = String.Empty Then
                dtList.Visible = True
                pnlEdit.Visible = False
                ' Show the list
                Dim myLinkTypeList As New List(Of LinkType)
                For Each myRow In wpm_GetDataTable(STR_SELECTLinkTypeList, "LinkType").Rows
                    myLinkTypeList.Add(New LinkType With {.LinkTypeCD = wpm_GetDBString(myRow("LinkTypeCD")),
                                                          .LinkTypeComment = wpm_GetDBString(myRow("LinkTypeComment")),
                                                          .LinkTypeDesc = wpm_GetDBString(myRow("LinkTypeDesc")),
                                                          .LinkTypeTarget = wpm_GetDBString(myRow("LinkTypeTarget"))})
                Next
                dtList.TableHeader = New DisplayTableHeader() With {.TableTitle = "Part Type",
                                                                   .DetailKeyName = "LinkTypeCD",
                                                                   .DetailFieldName = "LinkTypeCD",
                                                                   .DetailPath = "/admin/maint/default.aspx?type=LinkType&LinkTypeCD={0}"}
                dtList.TableHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "LinkTypeDesc", .Value = "LinkTypeDesc"})
                dtList.TableHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "LinkTypeComment", .Value = "LinkTypeComment"})
                dtList.TableHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "LinkTypeTarget", .Value = "LinkTypeTarget"})
                Dim myList As New List(Of Object)
                myList.AddRange(myLinkTypeList)
                dtList.BuildTable(dtList.TableHeader, myList)

            ElseIf reqLinkTypeCD = "NEW" Then
                ' Insert Mode
                tbLinkTypeCD.Enabled=True
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                dtList.Visible = False
                pnlEdit.Visible = True
                GetLinkTypeForEdit(reqLinkTypeCD)
            Else
                ' Edit Mode
                tbLinkTypeCD.Enabled=False
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetLinkTypeForEdit(reqLinkTypeCD)
            End If
        End If
    End Sub

    Private Function GetLinkTypeByLinkTypeCD(ByVal reqLinkTypeCD As String) As LinkType
        Dim myLinkType As New LinkType With {.LinkTypeCD = reqLinkTypeCD}
        For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_LinkTypeByLinkTypeCD, reqLinkTypeCD), "LinkType").Rows
            With myLinkType
                .LinkTypeComment = wpm_GetDBString(myRow("LinkTypeComment"))
                .LinkTypeCD = wpm_GetDBString(myRow("LinkTypeCD"))
                .LinkTypeDesc = wpm_GetDBString(myRow("LinkTypeDesc"))
                .LinkTypeCD = wpm_GetDBString(myRow("LinkTypeCD"))
                .LinkTypeTarget = wpm_GetDBString(myRow("LinkTypeTarget"))
            End With
            Exit For
        Next
        Return myLinkType
    End Function
    Private Sub GetLinkTypeForEdit(reqLinkTypeCD As String)
        Dim myLinkType As LinkType = GetLinkTypeByLinkTypeCD(reqLinkTypeCD)
        tbLinkTypeCD.Text = myLinkType.LinkTypeCD
        tbLinkTypeDS.Text = myLinkType.LinkTypeDesc
        tbLinkTypeComment.Text = myLinkType.LinkTypeComment
       ddlLinkTypeTarget.SelectedValue = myLinkType.LinkTypeTarget
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim myLinkType As LinkType = GetLinkTypeByLinkTypeCD(tbLinkTypeCD.Text)
        With myLinkType
            .LinkTypeCD = wpm_GetDBString(tbLinkTypeCD.Text)
            .LinkTypeDesc = wpm_GetDBString(tbLinkTypeDS.Text)
            .LinkTypeComment = wpm_GetDBString(tbLinkTypeComment.Text)
            .LinkTypeTarget = wpm_GetDBString(ddlLinkTypeTarget.SelectedValue )
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_LinkType}
                    wpm_AddParameterStringValue("@LinkTypeCD", myLinkType.LinkTypeCD, cmd)
                    wpm_AddParameterStringValue("@LinkTypeDesc", myLinkType.LinkTypeDesc, cmd)
                    wpm_AddParameterStringValue("@LinkTypeComment", myLinkType.LinkTypeComment, cmd)
                    wpm_AddParameterStringValue("@LinkTypeTarget", myLinkType.LinkTypeTarget, cmd)
                    wpm_AddParameterStringValue("@LinkTypeCD", myLinkType.LinkTypeCD, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_LinkType, "LinkType.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myLinkType As LinkType = GetLinkTypeByLinkTypeCD(tbLinkTypeCD.Text)
        With myLinkType
            .LinkTypeCD = wpm_GetDBString(tbLinkTypeCD.Text)
            .LinkTypeDesc = wpm_GetDBString(tbLinkTypeDS.Text)
            .LinkTypeComment = wpm_GetDBString(tbLinkTypeComment.Text)
            .LinkTypeTarget = wpm_GetDBString(ddlLinkTypeTarget.SelectedValue )
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_LinkType}
                    wpm_AddParameterStringValue("@LinkTypeCD", myLinkType.LinkTypeCD, cmd)
                    wpm_AddParameterStringValue("@LinkTypeDesc", myLinkType.LinkTypeDesc, cmd)
                    wpm_AddParameterStringValue("@LinkTypeComment", myLinkType.LinkTypeComment, cmd)
                    wpm_AddParameterStringValue("@LinkTypeTarget", myLinkType.LinkTypeTarget, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_LinkType, "LinkType.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myLinkType As LinkType = GetLinkTypeByLinkTypeCD(tbLinkTypeCD.Text)
        With myLinkType
            .LinkTypeCD = wpm_GetDBString(tbLinkTypeCD.Text)
            .LinkTypeDesc = wpm_GetDBString(tbLinkTypeDS.Text)
            .LinkTypeComment = wpm_GetDBString(tbLinkTypeComment.Text)
            .LinkTypeTarget = wpm_GetDBString(ddlLinkTypeTarget.SelectedValue )
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_LinkType}
                    wpm_AddParameterStringValue("@LinkTypeCD", myLinkType.LinkTypeCD, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_LinkType, "LinkType.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
End Class
