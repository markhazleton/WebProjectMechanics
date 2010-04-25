'
' ASP.NET code-behind class (Master Record) 
'

Partial Class SiteParameterType_master
 	Inherits System.Web.UI.UserControl

	Public ParentPage As Object

	Public SiteParameterType As Object

	'
	' ASP.NET user control Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not wpmUser.IsAdmin() Then
			Session("Login_Link") = Request.RawUrl
			Response.Redirect("/wpm/login/login.aspx")
		End If
		If TypeOf Page Is AspNetMaker7_WPMGen Then
			ParentPage = CType(Page, AspNetMaker7_WPMGen)
			SiteParameterType = ParentPage.SiteParameterType
		End If
	End Sub
End Class
