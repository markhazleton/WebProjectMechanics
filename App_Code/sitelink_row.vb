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
    Public Class SiteLinkrows 
		Inherits Collection(Of SiteLinkrow)
	End Class
	<Serializable()> _
    Public class SiteLinkrow 
		Inherits SiteLinkrow_base
        Public Sub New() 
		End Sub
    End Class
End Namespace
