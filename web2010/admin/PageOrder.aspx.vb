Imports System
Imports WebProjectMechanics
Imports System.Data

Partial Class VB
    Inherits ApplicationPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strSQL As String = String.Empty
        If IsPostBack Then
        Else
            lstLeft.Items.Clear()
            strSQL = (String.Format("SELECT [page].[PageID],[page].[PageName],[page].[PageOrder], [page].[ParentPageID] FROM [page] where [page].[CompanyID]={0} ORDER BY CompanyID, PageOrder, PageName ", wpm_CurrentSiteID))
            Dim myDT As DataTable = wpm_GetDataTable(strSQL, "vb.GetList")
            BuildPageListBox(0, 0, myDT, 0)
        End If
    End Sub

    Private Function BuildPageListBox(ByVal ParentID As Object, ByVal intLevel As Short, ByRef myDT As DataTable, ByRef ArrayIndex As Short) As Boolean
        Dim j As Integer
        Dim pageID As String = String.Empty
        Dim PageParentID As String = String.Empty
        Dim pageName As String = String.Empty
        Dim pageDesc As String = String.Empty
        For j = 0 To myDT.Rows.Count - 1
            Dim myrow As DataRow = myDT.Rows.Item(j)
            pageID = wpm_GetDBString(myrow.Item(0))
            pageName = wpm_GetDBString(myrow.Item(1))

            If Trim(wpm_GetDBString(myrow.Item(3))) & "*" = "*" Then
                PageParentID = 0
            Else
                PageParentID = myrow.Item(3)
            End If

            If (intLevel > 0) Then
                If (intLevel > 1) Then
                    If (intLevel > 2) Then
                        pageDesc = (String.Format("  -  -  - {0} ({1}) ", pageName, myrow.Item(2)))
                    Else
                        pageDesc = (String.Format("  -  -  {0} ({1}) ", pageName, myrow.Item(2)))
                    End If
                Else
                    pageDesc = (String.Format("  -  {0} ({1}) ", pageName, myrow.Item(2)))
                End If
            Else
                pageDesc = pageName
            End If
            If CShort(ParentID) = CShort(PageParentID) Then
                lstLeft.Items.Add(New ListItem With {.Value = pageID, .Text = pageDesc})
                ArrayIndex = ArrayIndex + 1
                Call BuildPageListBox(pageID, intLevel + 1, myDT, ArrayIndex)
            End If
        Next
        Return True
    End Function

    'Move up
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Make sure our item is not the first one on the list.
        If lstLeft.SelectedIndex > 0 Then
            Dim I = lstLeft.SelectedIndex - 1
            lstLeft.Items.Insert(I, lstLeft.SelectedItem)
            lstLeft.Items.RemoveAt(lstLeft.SelectedIndex + 2)
        End If
        Dim iOrder As Integer = 0
        Dim strSQL As String = String.Empty

        For Each myItem As listItem In lstLeft.Items
            iOrder = iOrder + 1
            strSQL = String.Format("UPDATE [Page]  SET [Page].[PageOrder] ={0} WHERE [Page].[PageID] ={1} and [Page].[CompanyID] ={2} ", iOrder, myItem.Value, wpm_CurrentSiteID)
            wpm_RunUpdateSQL(strSQL, "vb.submit")
        Next
        lstLeft.Items.Clear()
        strSQL = (String.Format("SELECT [page].[PageID],[page].[PageName],[page].[PageOrder], [page].[ParentPageID] FROM [page] where [page].[CompanyID]={0} ORDER BY CompanyID, PageOrder, PageName ", wpm_CurrentSiteID))
        Dim myDT As DataTable = wpm_GetDataTable(strSQL, "vb.GetList")
        BuildPageListBox(0, 0, myDT, 0)
    End Sub

    'Move down
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Make sure our item is not the last one on the list.
        If lstLeft.SelectedIndex < lstLeft.Items.Count - 1 Then
            'Insert places items above the index you supply, since we want
            'to move it down the list we have to do + 2
            Dim I = lstLeft.SelectedIndex + 2
            lstLeft.Items.Insert(I, lstLeft.SelectedItem)
            lstLeft.Items.RemoveAt(lstLeft.SelectedIndex)
            lstLeft.SelectedIndex = I - 1
        End If
        Dim iOrder As Integer = 0
        Dim strSQL As String = String.Empty

        For Each myItem As listItem In lstLeft.Items
            iOrder = iOrder + 1
            strSQL = String.Format("UPDATE [Page]  SET [Page].[PageOrder] ={0} WHERE [Page].[PageID] ={1} and [Page].[CompanyID] ={2} ", iOrder, myItem.Value, wpm_CurrentSiteID)
            wpm_RunUpdateSQL(strSQL, "vb.submit")
        Next
        lstLeft.Items.Clear()
        strSQL = (String.Format("SELECT [page].[PageID],[page].[PageName],[page].[PageOrder], [page].[ParentPageID] FROM [page] where [page].[CompanyID]={0} ORDER BY CompanyID, PageOrder, PageName ", wpm_CurrentSiteID))
        Dim myDT As DataTable = wpm_GetDataTable(strSQL, "vb.GetList")
        BuildPageListBox(0, 0, myDT, 0)
    End Sub

End Class
