
Partial Class mhweb_admin_mhPageOrder
    Inherits mhPage
    Public mySB As New StringBuilder("")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetList(ByVal EditPageID As String) As Boolean
        Dim I As Integer
        Dim strIDs As String
        Dim strSQL As String
        Dim arItems() As String
        Dim ArrayIndex As Short
        strIDs = Request.Form.Item("mylist")
        arItems = Split(strIDs, ", ")
        If strIDs <> "" Then
            For I = 0 To UBound(arItems)
                strSQL = "UPDATE [Page] "
                strSQL = strSQL & " SET"
                strSQL = strSQL & " [Page].[PageOrder] =" & I & ""
                strSQL = strSQL & " WHERE [Page].[PageID] =" & arItems(I)
                strSQL = strSQL & " and [Page].[CompanyID] =" & mhSession.GetCompanyID() & " "
                mhDB.RunUpdateSQL(strSQL, "mhPageOrder.GetList-UpdatePageOrder")
            Next
        End If

        strSQL = ("SELECT [page].[PageID],[page].[PageName],[page].[PageDescription],[page].[ParentPageID],[page].[PageFileName],[pageType].[PageFileName],[page].[CompanyID],[page].[PageOrder] FROM [page],[pagetype] where [page].[PageTypeID] = [PageType].[PageTypeID] and [page].[CompanyID]=" & mhSession.GetCompanyID() & " ORDER BY CompanyID, PageOrder, PageName ")

        Dim myDT As DataTable = mhDB.GetDataTable(strSQL, "mhPageOrder.GetList")

        If myDT.Rows.Count = 0 Then
            mySB.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            mySB.Append("No Pages Found!" & vbCrLf)
            mySB.Append("</P>" & vbCrLf)
        Else
            mySB.Append("<form method=post onsubmit=""processForm(this)"">")
            mySB.Append((vbCrLf & "<script>" & vbCrLf & Chr(13) & Chr(10)))
            mySB.Append(("var arrList = new Array()" & Chr(13) & Chr(10)))
            ArrayIndex = 0
            Call BuildPageListBox(0, 0, myDT, ArrayIndex)

            With mySB
                .Append("" & vbCrLf)
                .Append("      document.write(outputButton(true,""myList"",""Move Up"") + ""<br />"")" & vbCrLf)
                .Append("      document.write(outputList(arrList,""myList"",25) + ""<br />"")" & vbCrLf)
                .Append("      document.write(outputButton(false,""myList"",""Move Down""))" & vbCrLf)
                .Append("      </" & "script>" & vbCrLf)
                .Append("      <br />" & vbCrLf)
                .Append("      <input type=submit VALUE=""Submit New Order"">" & vbCrLf)
                .Append("      </form>" & vbCrLf)
                .Append("    " & vbCrLf)
            End With
        End If
        GetList = True
    End Function

    Private Function BuildPageListBox(ByVal ParentID As Object, ByVal intLevel As Short, ByRef myDT As DataTable, ByRef ArrayIndex As Short) As Boolean
        Dim j As Integer
        Dim PageDesc As Object
        Dim pageID As Object
        Dim PageTitle As Object
        Dim PageParentID As Object
        Dim pageName As Object
        For j = 0 To myDT.Rows.Count - 1
            Dim myrow As DataRow = myDT.Rows.Item(j)
            pageID = mhUTIL.GetDBString(myrow.Item(0))
            pageName = mhUTIL.GetDBString(myrow.Item(1))
            PageTitle = mhUTIL.GetDBString(myrow.Item(2))

            If Trim(mhUTIL.GetDBString(myrow.Item(3))) & "*" = "*" Then
                PageParentID = 0
            Else
                PageParentID = myrow.Item(3)
            End If

            If (intLevel > 0) Then
                If (intLevel > 1) Then
                    If (intLevel > 2) Then
                        PageDesc = ("  -  -  - " & pageName & " (" & myrow.Item(7) & ") ")
                    Else
                        PageDesc = ("  -  -  " & pageName & " (" & myrow.Item(7) & ") ")
                    End If
                Else
                    PageDesc = ("  -  " & pageName & " (" & myrow.Item(7) & ") ")
                End If
            Else
                PageDesc = pageName
            End If

            If CShort(ParentID) = CShort(PageParentID) Then
                mySB.Append(("arrList[" & ArrayIndex & "] = new Array(""" & pageID & """,""" & PageDesc & """)" & Chr(13) & Chr(10)))
                ArrayIndex = ArrayIndex + 1
                Call BuildPageListBox(pageID, intLevel + 1, myDT, ArrayIndex)
            End If
        Next
        Return True
    End Function

    Private Function BuildPageStructure(ByVal ParentID As Object, ByVal intLevel As Double, ByRef myDT As DataTable) As Boolean
        Dim pageName As Object
        Dim PageID As Object
        Dim PageTitle As Object
        Dim CompanyID As Object
        Dim PageParentID As Object
        Dim j As Integer
        For j = 0 To myDT.Rows.Count - 1
            Dim myRow As DataRow = myDT.Rows.Item(j)
            PageID = mhUTIL.GetDBString(myRow.Item(0))
            pageName = mhUTIL.GetDBString(myRow.Item(1))
            PageTitle = mhUTIL.GetDBString(myRow.Item(2))
            If Trim(mhUTIL.GetDBString(myRow.Item(3))) & "*" = "*" Then
                PageParentID = 0
            Else
                PageParentID = mhUTIL.GetDBString(myRow.Item(3))
            End If
            CompanyID = mhUTIL.GetDBString(myRow.Item(6))

            If CShort(ParentID) = CShort(PageParentID) Then
                If mhUser.IsAdmin() Then
                    mySB.Append("<img src=""/images/spacer.gif"" width=""" & intLevel * 10 & """ height=""1"">" & mhUTIL.FormatLink(PageID, pageName, "Page", "/mhwcm/admin/page_edit.aspx?key=" & PageID) & "<br />")
                Else
                    mySB.Append("<img src=""/images/spacer.gif"" width=""" & intLevel * 10 & """ height=""1"">" & pageName & "<br />")
                End If
                Call BuildPageStructure(PageID, intLevel + 1, myDT)
            End If
        Next
        BuildPageStructure = True
    End Function

    Public Sub BuildStructure()
        Dim strSQL As String
        strSQL = "SELECT [page].[PageID],[page].[PageName],[page].[PageDescription],[page].[ParentPageID],[page].[PageFileName],[pageType].[PageFileName], [page].[CompanyID]  FROM [page],[pagetype]  where [page].[PageTypeID] = [PageType].[PageTypeID] and [page].[CompanyID]=" & mhSession.GetCompanyID() & " ORDER BY CompanyID, PageOrder, PageName "
        Dim myDT As DataTable = mhDB.GetDataTable(strSQL, "mhPageOrder.BuildStructure")

        If myDT.Rows.Count = 0 Then
            mySB.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            mySB.Append("No Page Results Found!" & vbCrLf)
            mySB.Append("</P>" & vbCrLf)
        End If
        Call BuildPageStructure(0, 0, myDT)
    End Sub

End Class
