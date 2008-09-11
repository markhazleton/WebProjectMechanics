Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Namespace PMGEN
    Public Class rolerows 
		Inherits Collection(Of rolerow)
	End Class
	<Serializable()> _
    Public class rolerow 
		Inherits rolerow_base
        Public Sub New() 
		End Sub
    End Class
End Namespace
