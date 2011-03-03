'
' ASP.NET code-behind class (Master Record) 
'

Partial Class zpage_master
 	Inherits System.Web.UI.UserControl

	Public ParentPage As Object

	Public zPage As Object

	'
	' ASP.NET user control Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If TypeOf Page Is AspNetMaker8_wpmWebsite Then
			ParentPage = CType(Page, AspNetMaker8_wpmWebsite)
			zPage = ParentPage.zPage
		End If
	End Sub
End Class
