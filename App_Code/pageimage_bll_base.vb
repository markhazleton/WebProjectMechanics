Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.Collections.Generic
Imports System.IO
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports EW.Data
Imports EW.Web
Namespace PMGEN

	''' <summary>
	''' Summary description for PageImagebll
	''' Bussiness Layer (PageImage)
	''' </summary>

	Public MustInherit Class PageImagebll_base
		Public Sub New() 
		End Sub

		' ******************************
		' *  Handler for Data Inserting
		' ******************************

		Public Shared Function Inserting(ByVal row1 As PageImagerow) As Boolean
			Return True
		End Function

		' *****************************
		' *  Handler for Data Inserted
		' *****************************

		Public Shared Sub Inserted(ByVal row1 As PageImagerow)
			WriteAuditTrail(Nothing, row1, Core.PageType.Add)
		End Sub

		' *****************************
		' *  Handler for Data Updating
		' *****************************

		Public Shared Function Updating(ByVal row1 As PageImagerow, ByVal row2 As PageImagerow) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Updated
		' ****************************

		Public Shared Sub Updated(ByVal row1 As PageImagerow, ByVal row2 As PageImagerow)
			WriteAuditTrail(row1, row2, Core.PageType.Edit)
		End Sub

		' *****************************
		' *  Handler for Data Deleting
		' *****************************

		Public Shared Function Deleting(ByVal rows As Collection(Of PageImagerow)) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Deleted
		' ****************************

		Public Shared Sub Deleted(ByVal rows As Collection(Of PageImagerow))
			Dim i As Integer = 0
			Do While (i < rows.Count)
				WriteAuditTrail(DirectCast(rows(i), PageImagerow), Nothing, Core.PageType.Delete)
				i += 1
			Loop
		End Sub

		' **************************************************
		' *  Write Audit Trail For Inserted/Updated/Deleted
		' **************************************************

		Private Shared Sub WriteAuditTrail(ByVal row1 As PageImagerow, ByVal row2 As PageImagerow, ByVal pt As Core.PageType)
			Dim pfx As String = "event"
			Dim strKeys As String = String.Empty
			Dim strDate As String = DateTime.Today.ToString("yyyy-MM-dd")
			Dim strTime As String = DateTime.Now.ToString("HH:mm:ss")
			Dim strAction As String = Core.PageTypeAbbr(CType(pt,Integer))
			Dim strTable As String = "PageImage"
			Dim strID As String = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToString() ' get current file name
			Dim strUser As String = "-1" ' get current user name
			Dim strLogFolder As String = HttpContext.Current.Server.MapPath(Share.AuditPath)
			Dim strNewValue As String = Nothing
			Dim strOldValue As String = Nothing 
			Dim bAudit As Boolean
			Dim info As PageImageinf = New PageImageinf()
			Dim tbl As EW.Data.Table = info.TableInfo
			Dim fields As ICollection
			If (row1 IsNot Nothing) Then fields = row1.GetFields().Keys Else fields = row2.GetFields().Keys
			Dim row1Fields As Dictionary(Of String, Object), row2Fields As Dictionary(Of String, Object)
			If (row1 IsNot Nothing) Then row1Fields = row1.GetFields() Else row1Fields = Nothing
			If (row2 IsNot Nothing) Then row2Fields = row2.GetFields() Else row2Fields = Nothing

			' Key
			If (strKeys.Length > 0) Then  strKeys &= ", " 
			If (row1 IsNot Nothing) Then strKeys &= row1.PageImageID.ToString() Else strKeys &= row2.PageImageID.ToString()
			For Each strFieldName As String In fields
				bAudit = False
				If (row1 IsNot Nothing) Then
					If (row1Fields(strFieldName) Is Nothing) Then strOldValue = "null" Else strOldValue = Convert.ToString(row1Fields(strFieldName))
				Else 
					strOldValue = ""
				End If
				If (row2 IsNot Nothing) Then
					If (row2Fields(strFieldName) Is Nothing) Then strNewValue = "null" Else strNewValue = Convert.ToString(row2Fields(strFieldName))
				Else
					strNewValue = ""
				End If
				Select Case pt
					Case Core.PageType.Add
						bAudit = ((row2.Attribute(strFieldName).AllowUpdate OrElse tbl.Fields(strFieldName).IsAutoIncrement) AndAlso (tbl.Fields(strFieldName).FieldType <> 205))
					Case Core.PageType.Edit
						bAudit = ((row2.Attribute(strFieldName).AllowUpdate AndAlso (strOldValue <> strNewValue)) AndAlso (tbl.Fields(strFieldName).FieldType <> 205))
					Case Core.PageType.Delete
						bAudit =  (tbl.Fields(strFieldName).FieldType <>205)
				End Select
				If (bAudit) Then
					If (tbl.Fields(strFieldName).FieldType = 203) Then
						If (strOldValue IsNot Nothing AndAlso strOldValue.Length > 0) Then strOldValue = "<Memo>" Else strOldValue = ""
						If (strNewValue IsNot Nothing AndAlso strNewValue.Length > 0) Then strNewValue = "<Memo>" Else strNewValue = ""
					End If
					Share.AuditTrail.WriteAuditTrail(strLogFolder, pfx, strDate, strTime, strID, strUser, strAction, strTable, strFieldName, strKeys, strOldValue, strNewValue)
				End If
			Next
		End Sub
	End class
End Namespace
