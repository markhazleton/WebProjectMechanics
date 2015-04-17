Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_PageType
    Inherits ApplicationUserControl
    ' Location (Page) Type  
    Public Const STR_PageTypeID As String = "PageTypeID"
    Public Const STR_SELECTPageTypeList As String = "SELECT PageType.[PageTypeID], PageType.[PageTypeCD], PageType.[PageTypeDesc], PageType.[PageFileName] FROM PageType;"
    Public Const STR_SELECT_PageTypeByPageTypeID As String = "SELECT PageType.[PageTypeID], PageType.[PageTypeCD], PageType.[PageTypeDesc], PageType.[PageFileName] FROM PageType where [PageType].[PageTypeID]={0};"
    Public Const STR_UPDATE_PageType As String = "UPDATE PageType SET PageType.PageTypeCD = @PageTypeCD , PageType.PageTypeDesc = @PageTypeDesc, PageType.PageFileName = @PageFileName WHERE (((PageType.PageTypeID)=@PageTypeID));"
    Public Const STR_INSERT_PageType As String = "INSERT INTO PageType ( PageTypeCD , PageTypeDesc , PageFileName ) VALUES ( @PageTypeCD ,@PageTypeDesc, @PageFileName ); "
    Public Const STR_DELETE_PageType As String = "DELETE FROM [PageType] WHERE [PageType].[PageTypeID]=@PageTypeID;"
    Private Property reqPageTypeID As String
    Public Class PageType
        Public Property PageTypeID As String
        Public Property PageTypeCD As String
        Public Property PageTypeDesc As String
        Public Property PageFileName As String
    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqPageTypeID = GetProperty(STR_PageTypeID, String.Empty)
        If Not IsPostBack Then

            If reqPageTypeID = String.Empty Then
                dtList.Visible = True
                pnlEdit.Visible = False
                ' Show the list
                Dim myPageTypeList As New List(Of PageType)
                For Each myRow In wpm_GetDataTable(STR_SELECTPageTypeList, "PageType").Rows
                    myPageTypeList.Add(New PageType With {.PageTypeID = wpm_GetDBInteger(myRow("PageTypeID")),
                                                          .PageTypeCD = wpm_GetDBString(myRow("PageTypeCD")),
                                                          .PageFileName = wpm_GetDBString(myRow("PageFileName")),
                                                          .PageTypeDesc = wpm_GetDBString(myRow("PageTypeDesc"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Location Type", .DetailKeyName = "PageTypeID", .DetailFieldName = "PageTypeCD", .DetailPath = "/admin/maint/default.aspx?type=LocationType&PageTypeID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "PageTypeDS", .Value = "PageTypeDesc"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "PageFileName", .Value = "PageFileName"})
                Dim myList As New List(Of Object)
                myList.AddRange(myPageTypeList)
                dtList.BuildTable(myListHeader, myList)

            ElseIf reqPageTypeID = "0" Or Not (IsNumeric(reqPageTypeID)) Then
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                dtList.Visible = False
                pnlEdit.Visible = True
                GetPageTypeForEdit(reqPageTypeID)
            Else
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetPageTypeForEdit(reqPageTypeID)
            End If
        End If
    End Sub

    Private Function GetPageTypeByPageTypeID(ByVal reqPageTypeID As String) As PageType
        Dim myPageType As New PageType With {.PageTypeID = reqPageTypeID}
        If IsNumeric(reqPageTypeID) AndAlso CInt(reqPageTypeID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_PageTypeByPageTypeID, reqPageTypeID), "PageType").Rows
                With myPageType
                    .PageFileName = wpm_GetDBString(myRow("PageFileName"))
                    .PageTypeCD = wpm_GetDBString(myRow("PageTypeCD"))
                    .PageTypeDesc = wpm_GetDBString(myRow("PageTypeDesc"))
                    .PageTypeID = wpm_GetDBString(myRow("PageTypeID"))
                End With
                Exit For
            Next
        Else
            myPageType.PageTypeID = "0"
        End If
        Return myPageType
    End Function
    Private Sub GetPageTypeForEdit(reqPageTypeID As String)
        Dim myPageType As PageType = GetPageTypeByPageTypeID(reqPageTypeID)
        tbPageTypeCD.Text = myPageType.PageTypeCD
        tbPageTypeDS.Text = myPageType.PageTypeDesc
        tbPageTypeNM.Text = myPageType.PageFileName
        litPageTypeID.Text = myPageType.PageTypeID
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim myPageType As PageType = GetPageTypeByPageTypeID(litPageTypeID.Text)
        With myPageType
            .PageTypeCD = wpm_GetDBString(tbPageTypeCD.text)
            .PageTypeDesc = wpm_GetDBString(tbPageTypeDS.text)
            .PageFileName = wpm_GetDBString(tbPageTypeNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_PageType}
                    wpm_AddParameterStringValue("@PageTypeCD", myPageType.PageTypeCD, cmd)
                    wpm_AddParameterStringValue("@PageTypeDesc", myPageType.PageTypeDesc, cmd)
                    wpm_AddParameterStringValue("@PageFileName", myPageType.PageFileName, cmd)
                    wpm_AddParameterStringValue("@PageTypeID", myPageType.PageTypeID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_UPDATE_PageType, "PageType.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myPageType As PageType = GetPageTypeByPageTypeID(litPageTypeID.Text)
        With myPageType
            .PageTypeCD = wpm_GetDBString(tbPageTypeCD.text)
            .PageTypeDesc = wpm_GetDBString(tbPageTypeDS.text)
            .PageFileName = wpm_GetDBString(tbPageTypeNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_PageType}
                    wpm_AddParameterStringValue("@PageTypeCD", myPageType.PageTypeCD, cmd)
                    wpm_AddParameterStringValue("@PageTypeDesc", myPageType.PageTypeDesc, cmd)
                    wpm_AddParameterStringValue("@PageFileName", myPageType.PageFileName, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_INSERT_PageType, "PageType.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myPageType As PageType = GetPageTypeByPageTypeID(litPageTypeID.Text)
        With myPageType
            .PageTypeCD = wpm_GetDBString(tbPageTypeCD.text)
            .PageTypeDesc = wpm_GetDBString(tbPageTypeDS.text)
            .PageFileName = wpm_GetDBString(tbPageTypeNM.text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_PageType}
                    wpm_AddParameterStringValue("@PageTypeID", myPageType.PageTypeID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_DELETE_PageType, "PageType.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
End Class
