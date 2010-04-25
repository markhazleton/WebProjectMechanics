﻿
Partial Class wpm_admin_wpmPageOrder
    Inherits AspNetMaker7_WPMGen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        GetList(Request.QueryString.Item("PageID"), mySB)
        mySort.Text = mySB.ToString
        If Not IsPostBack Then
            Dim strSQL As String = ("SELECT [page].[PageID],[page].[PageName],[page].[PageDescription],[page].[ParentPageID],[page].[PageFileName],[pageType].[PageFileName],[page].[CompanyID],[page].[PageOrder] FROM [page],[pagetype] where [page].[PageTypeID] = [PageType].[PageTypeID] and [page].[CompanyID]=" & wpmSession.GetCompanyID() & " ORDER BY CompanyID, PageOrder, PageName ")
            Dim myDT As DataTable = wpmDB.GetDataTable(strSQL, "mhPageOrder.GetList")
            For Each myrow As DataRow In wpmDB.GetDataTable(strSQL, "mhPageOrder.GetList").Rows
                Dim myItem As New System.Web.UI.WebControls.ListItem
                myItem.Value = wpmUTIL.GetDBString(myrow.Item("PageID"))
                myItem.Text = wpmUTIL.GetDBString(myrow.Item("PageName"))
            Next
        End If
    End Sub

    Public Function GetList(ByVal EditPageID As String, ByRef mySB As StringBuilder) As Boolean
        Dim I As Integer
        Dim strIDs As String
        Dim strSQL As String
        Dim arItems() As String
        Dim ArrayIndex As Short
        strIDs = Request.Form.Item("myList")
        arItems = Split(strIDs, ", ")
        If strIDs <> "" Then
            For I = 0 To UBound(arItems)
                strSQL = "UPDATE [Page] "
                strSQL = strSQL & " SET"
                strSQL = strSQL & " [Page].[PageOrder] =" & I & ""
                strSQL = strSQL & " WHERE [Page].[PageID] =" & arItems(I)
                strSQL = strSQL & " and [Page].[CompanyID] =" & wpmSession.GetCompanyID() & " "
                wpmDB.RunUpdateSQL(strSQL, "mhPageOrder.GetList-UpdatePageOrder")
            Next
        End If

        strSQL = ("SELECT [page].[PageID],[page].[PageName],[page].[PageDescription],[page].[ParentPageID],[page].[PageFileName],[pageType].[PageFileName],[page].[CompanyID],[page].[PageOrder] FROM [page],[pagetype] where [page].[PageTypeID] = [PageType].[PageTypeID] and [page].[CompanyID]=" & wpmSession.GetCompanyID() & " ORDER BY CompanyID, PageOrder, PageName ")

        Dim myDT As DataTable = wpmDB.GetDataTable(strSQL, "mhPageOrder.GetList")

        If myDT.Rows.Count = 0 Then
            mySB.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            mySB.Append("No Pages Found!" & vbCrLf)
            mySB.Append("</P>" & vbCrLf)
        Else
            mySB.Append("<form method=post onsubmit=""processForm(this)"">")
            mySB.Append((vbCrLf & "<script>" & vbCrLf & Chr(13) & Chr(10)))
            mySB.Append(("var arrList = new Array()" & Chr(13) & Chr(10)))
            ArrayIndex = 0
            Call BuildPageListBox(0, 0, myDT, ArrayIndex, mySB)

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

    Private Function BuildPageListBox(ByVal ParentID As Object, ByVal intLevel As Short, ByRef myDT As DataTable, ByRef ArrayIndex As Short, ByRef mysb As StringBuilder) As Boolean
        Dim j As Integer
        Dim PageDesc As Object
        Dim pageID As Object
        Dim PageTitle As Object
        Dim PageParentID As Object
        Dim pageName As Object
        For j = 0 To myDT.Rows.Count - 1
            Dim myrow As DataRow = myDT.Rows.Item(j)
            pageID = wpmUTIL.GetDBString(myrow.Item(0))
            pageName = wpmUTIL.GetDBString(myrow.Item(1))
            PageTitle = wpmUTIL.GetDBString(myrow.Item(2))

            If Trim(wpmUTIL.GetDBString(myrow.Item(3))) & "*" = "*" Then
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
                Call BuildPageListBox(pageID, intLevel + 1, myDT, ArrayIndex, mysb)
            End If
        Next
        Return True
    End Function

    Private Function BuildPageStructure(ByVal ParentID As Object, ByVal intLevel As Double, ByRef myDT As DataTable, ByRef mySB As StringBuilder) As Boolean
        Dim pageName As Object
        Dim PageID As Object
        Dim PageTitle As Object
        Dim CompanyID As Object
        Dim PageParentID As Object
        Dim j As Integer
        For j = 0 To myDT.Rows.Count - 1
            Dim myRow As DataRow = myDT.Rows.Item(j)
            PageID = wpmUTIL.GetDBString(myRow.Item(0))
            pageName = wpmUTIL.GetDBString(myRow.Item(1))
            PageTitle = wpmUTIL.GetDBString(myRow.Item(2))
            If Trim(wpmUTIL.GetDBString(myRow.Item(3))) & "*" = "*" Then
                PageParentID = 0
            Else
                PageParentID = wpmUTIL.GetDBString(myRow.Item(3))
            End If
            CompanyID = wpmUTIL.GetDBString(myRow.Item(6))

            If CShort(ParentID) = CShort(PageParentID) Then
                If wpmUser.IsAdmin() Then
                    mySB.Append("<img src=""/wpm/images/spacer.gif"" width=""" & intLevel * 10 & """ height=""1"">" & wpmUTIL.FormatLink(PageID, pageName, "Page", "/mhwcm/admin/page_edit.aspx?key=" & PageID) & "<br />")
                Else
                    mySB.Append("<img src=""/wpm/images/spacer.gif"" width=""" & intLevel * 10 & """ height=""1"">" & pageName & "<br />")
                End If
                Call BuildPageStructure(PageID, intLevel + 1, myDT, mySB)
            End If
        Next
        Return True
    End Function

    Public Function BuildStructure(ByRef mySB As StringBuilder) As Boolean
        Dim strSQL As String
        strSQL = "SELECT [page].[PageID],[page].[PageName],[page].[PageDescription],[page].[ParentPageID],[page].[PageFileName],[pageType].[PageFileName], [page].[CompanyID]  FROM [page],[pagetype]  where [page].[PageTypeID] = [PageType].[PageTypeID] and [page].[CompanyID]=" & wpmSession.GetCompanyID() & " ORDER BY CompanyID, PageOrder, PageName "
        Dim myDT As DataTable = wpmDB.GetDataTable(strSQL, "mhPageOrder.BuildStructure")

        If myDT.Rows.Count = 0 Then
            mySB.Append("<P STYLE=""font-size:10pt;"">" & vbCrLf)
            mySB.Append("No Page Results Found!" & vbCrLf)
            mySB.Append("</P>" & vbCrLf)
        End If
        Call BuildPageStructure(0, 0, myDT, mySB)
        Return True
    End Function

End Class