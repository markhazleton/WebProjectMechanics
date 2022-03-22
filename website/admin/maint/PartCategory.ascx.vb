Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_LinkCategory
    Inherits ApplicationUserControl
    ' Location (Page) Type  
    Public Const STR_LinkCategoryID As String = "LinkCategoryID"
    Public Const STR_SELECTLinkCategoryList As String = "SELECT LinkCategory.ID, LinkCategory.Title, LinkCategory.Description, LinkCategory.ParentID, LinkCategory.PageID, Page.PageName, Parent_LinkCategory.Title as Parent_Title  FROM (LinkCategory LEFT JOIN Page ON LinkCategory.PageID = Page.PageID) LEFT JOIN LinkCategory AS Parent_LinkCategory ON LinkCategory.ParentID = Parent_LinkCategory.ID;"
    Public Const STR_SELECT_LinkCategoryByLinkCategoryID As String = "SELECT ID, Title, Description, ParentID, PageID FROM LinkCategory where ID={0};"

    Public Const STR_UPDATE_LinkCategory As String = "UPDATE LinkCategory SET Title = @LinkCategoryNM , Description = @LinkCagegoryDS, ParentID = @ParentID, PageID = @PageID WHERE ID=@LinkCategoryID;"
    Public Const STR_INSERT_LinkCategory As String = "INSERT INTO LinkCategory ( Title , Description, ParentID, PageID ) VALUES ( @LinkCategoryNM ,@LinkCategoryDS, @ParentID, @PageID); "
    Public Const STR_DELETE_LinkCategory As String = "DELETE FROM LinkCategory WHERE ID=@LinkCategoryID;"
    Private Property reqLinkCategoryID As String
    Public Class LinkCategory
        Public Property LinkCategoryID As String
        Public Property LinkCategoryNM As String
        Public Property LinkCategoryDS As String
        Public Property ParentID As String
        Public Property PageID As String
    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqLinkCategoryID = GetProperty(STR_LinkCategoryID, String.Empty)
        If Not IsPostBack Then

            wpm_LoadCMB(ddlParentID,String.Empty,"SELECT ID, Title, Description, ParentID, PageID FROM LinkCategory order by Title;","Title","ID",False)

            wpm_LoadCMB(ddlPageID,String.Empty,"SELECT Page.PageID, Page.PageName FROM Page ORDER BY Page.PageName;","PageName","PageID",False)

            If reqLinkCategoryID = String.Empty Then
                dtList.Visible = True
                pnlEdit.Visible = False
                ' Show the list
                Dim myLinkCategoryList As New List(Of LinkCategory)
                For Each myRow In wpm_GetDataTable(STR_SELECTLinkCategoryList, "LinkCategory").Rows
                    myLinkCategoryList.Add(New LinkCategory With {.LinkCategoryID = wpm_GetDBInteger(myRow("ID")),
                                                                  .LinkCategoryNM = wpm_GetDBString(myRow("Title")),
                                                                  .LinkCategoryDS = wpm_GetDBString(myRow("Description")),
                                                                  .ParentID = wpm_GetDBString(myRow("ParentID")),
                                                                  .PageID = wpm_GetDBString(myRow("PageID"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Part Category",
                                                                   .DetailKeyName = "LinkCategoryID",
                                                                   .DetailFieldName = "LinkCategoryNM",
                                                                   .DetailPath = "/admin/maint/default.aspx?type=PartCategory&LinkCategoryID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "LinkCategoryDS", .Value = "LinkCategoryDS"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "Parent_Title", .Value = "Parent_Title"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "PageName", .Value = "PageName"})
                Dim myList As New List(Of Object)
                myList.AddRange(myLinkCategoryList)
                dtList.BuildTable(myListHeader, myList)

            ElseIf reqLinkCategoryID = "0" Or Not (IsNumeric(reqLinkCategoryID)) Then
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                dtList.Visible = False
                pnlEdit.Visible = True
                GetLinkCategoryForEdit(reqLinkCategoryID)
            Else
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetLinkCategoryForEdit(reqLinkCategoryID)
            End If
        End If
    End Sub

    Private Function GetLinkCategoryByLinkCategoryID(ByVal reqLinkCategoryID As String) As LinkCategory
        Dim myLinkCategory As New LinkCategory With {.LinkCategoryID = reqLinkCategoryID}
        If IsNumeric(reqLinkCategoryID) AndAlso CInt(reqLinkCategoryID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_LinkCategoryByLinkCategoryID, reqLinkCategoryID), "LinkCategory").Rows
                With myLinkCategory
                    .LinkCategoryID = wpm_GetDBInteger(myRow("ID"))
                    .LinkCategoryNM = wpm_GetDBString(myRow("Title"))
                    .LinkCategoryDS = wpm_GetDBString(myRow("Description"))
                    .ParentID = wpm_GetDBString(myRow("ParentID"))
                    .PageID = wpm_GetDBString(myRow("PageID"))
                End With
                Exit For
            Next
        Else
            myLinkCategory.LinkCategoryID = "0"
        End If
        Return myLinkCategory
    End Function
    Private Sub GetLinkCategoryForEdit(reqLinkCategoryID As String)
        Dim myLinkCategory As LinkCategory = GetLinkCategoryByLinkCategoryID(reqLinkCategoryID)
        With myLinkCategory
            tbLinkCategoryNM.Text = myLinkCategory.LinkCategoryNM
            tbLinkCategoryDS.Text = myLinkCategory.LinkCategoryDS
            ddlParentID.SelectedValue = myLinkCategory.ParentID
            ddlPageID.SelectedValue = myLinkCategory.PageID
            litLinkCategoryID.Text = myLinkCategory.LinkCategoryID

        End With
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim myLinkCategory As LinkCategory = GetLinkCategoryByLinkCategoryID(litLinkCategoryID.Text)
        With myLinkCategory
            .LinkCategoryNM = wpm_GetDBString(tbLinkCategoryNM.Text)
            .LinkCategoryDS = wpm_GetDBString(tbLinkCategoryDS.Text)
            .ParentID = wpm_GetDBString(ddlParentID.SelectedValue)
            .PageID = wpm_GetDBString(ddlPageID.SelectedValue)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                With myLinkCategory
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_LinkCategory}
                        wpm_AddParameterStringValue("@LinkCategoryNM", .LinkCategoryNM, cmd)
                        wpm_AddParameterStringValue("@LinkCategoryDS", .LinkCategoryDS, cmd)
                        wpm_AddParameterStringValue("@ParentID", .ParentID, cmd)
                        wpm_AddParameterStringValue("@PageID", .PageID, cmd)
                        wpm_AddParameterStringValue("@LinkCategoryID", .LinkCategoryID, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using

                End With
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_LinkCategory, "LinkCategory.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myLinkCategory As LinkCategory = GetLinkCategoryByLinkCategoryID(litLinkCategoryID.Text)
        With myLinkCategory
            .LinkCategoryNM = wpm_GetDBString(tbLinkCategoryNM.Text)
            .LinkCategoryDS = wpm_GetDBString(tbLinkCategoryDS.Text)
            .ParentID = wpm_GetDBString(ddlParentID.SelectedValue)
            .PageID = wpm_GetDBString(ddlPageID.SelectedValue)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                With myLinkCategory
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_LinkCategory}
                        wpm_AddParameterStringValue("@LinkCategoryNM", .LinkCategoryNM, cmd)
                        wpm_AddParameterStringValue("@LinkCategoryDS", .LinkCategoryDS, cmd)
                        wpm_AddParameterStringValue("@ParentID", .ParentID, cmd)
                        wpm_AddParameterStringValue("@PageID", .PageID, cmd)
                        wpm_AddParameterStringValue("@LinkCategoryID", .LinkCategoryID, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                End With
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_LinkCategory, "LinkCategory.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myLinkCategory As LinkCategory = GetLinkCategoryByLinkCategoryID(litLinkCategoryID.Text)
        With myLinkCategory
            .LinkCategoryNM = wpm_GetDBString(tbLinkCategoryNM.Text)
            .LinkCategoryDS = wpm_GetDBString(tbLinkCategoryDS.Text)
            .ParentID = wpm_GetDBString(ddlParentID.SelectedValue)
            .PageID = wpm_GetDBString(ddlPageID.SelectedValue)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_LinkCategory}
                    wpm_AddParameterStringValue("@LinkCategoryID", myLinkCategory.LinkCategoryID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_LinkCategory, "LinkCategory.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
End Class
