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
	''' Summary description for PageAliasbll
	''' Bussiness Layer (PageAlias)
	''' </summary>

	Public MustInherit Class PageAliasbll_base
		Public Sub New() 
		End Sub

		' ******************************
		' *  Handler for Data Inserting
		' ******************************

		Public Shared Function Inserting(ByVal row1 As PageAliasrow) As Boolean
			Return True
		End Function

		' *****************************
		' *  Handler for Data Inserted
		' *****************************

		Public Shared Sub Inserted(ByVal row1 As PageAliasrow)
		End Sub

		' *****************************
		' *  Handler for Data Updating
		' *****************************

		Public Shared Function Updating(ByVal row1 As PageAliasrow, ByVal row2 As PageAliasrow) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Updated
		' ****************************

		Public Shared Sub Updated(ByVal row1 As PageAliasrow, ByVal row2 As PageAliasrow)
		End Sub

		' *****************************
		' *  Handler for Data Deleting
		' *****************************

		Public Shared Function Deleting(ByVal rows As Collection(Of PageAliasrow)) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Deleted
		' ****************************

		Public Shared Sub Deleted(ByVal rows As Collection(Of PageAliasrow))
		End Sub
	End class
End Namespace