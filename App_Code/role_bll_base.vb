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
	''' Summary description for rolebll
	''' Bussiness Layer (role)
	''' </summary>

	Public MustInherit Class rolebll_base
		Public Sub New() 
		End Sub

		' ******************************
		' *  Handler for Data Inserting
		' ******************************

		Public Shared Function Inserting(ByVal row1 As rolerow) As Boolean
			Return True
		End Function

		' *****************************
		' *  Handler for Data Inserted
		' *****************************

		Public Shared Sub Inserted(ByVal row1 As rolerow)
		End Sub

		' *****************************
		' *  Handler for Data Updating
		' *****************************

		Public Shared Function Updating(ByVal row1 As rolerow, ByVal row2 As rolerow) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Updated
		' ****************************

		Public Shared Sub Updated(ByVal row1 As rolerow, ByVal row2 As rolerow)
		End Sub

		' *****************************
		' *  Handler for Data Deleting
		' *****************************

		Public Shared Function Deleting(ByVal rows As Collection(Of rolerow)) As Boolean
			Return True
		End Function

		' ****************************
		' *  Handler for Data Deleted
		' ****************************

		Public Shared Sub Deleted(ByVal rows As Collection(Of rolerow))
		End Sub
	End class
End Namespace
