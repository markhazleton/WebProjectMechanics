Imports System.IO
Imports System.Text
Imports System.Web.UI.WebControls


Public Interface ILookupItemList
    WriteOnly Property LookupList As List(Of LookupItem)
    ReadOnly Property value As String
End Interface
