'
' ASP.NET code-behind class (Email)
'

Partial Class _ewemail
	Inherits System.Web.UI.UserControl

	Public ParentPage As AspNetMaker8_wpmWebsite

	'
	' User control Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If TypeOf Page Is AspNetMaker8_wpmWebsite Then
			ParentPage = CType(Page, AspNetMaker8_wpmWebsite)
		End If
	End Sub
End Class
