'
' ASP.NET code-behind class (Menu)
'

Partial Class ewmenu
	Inherits System.Web.UI.UserControl

	Public ParentPage As AspNetMaker8_wpmWebsite

	Public RootMenu As AspNetMaker8_wpmWebsite.cMenu

	'
	' User control Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If TypeOf Page Is AspNetMaker8_wpmWebsite Then
			ParentPage = CType(Page, AspNetMaker8_wpmWebsite)
			RootMenu = New AspNetMaker8_wpmWebsite.cMenu("RootMenu", True)
			RootMenu.ParentPage = ParentPage
			If (ParentPage.Language Is Nothing) Then
				ParentPage.Language = New AspNetMaker8_wpmWebsite.cLanguage(New AspNetMaker8_wpmWebsite.AspNetMakerPage())
			End If

			' Generate menu items
			RootMenu.AddMenuItem(25, ParentPage.Language.MenuPhrase("25", "MenuText"), "", -1, "", "", True)
			RootMenu.AddMenuItem(2, ParentPage.Language.MenuPhrase("2", "MenuText"), "company_list.aspx", 25, "", "", true)
			RootMenu.AddMenuItem(6, ParentPage.Language.MenuPhrase("6", "MenuText"), "group_list.aspx", 25, "", "", true)
			RootMenu.AddMenuItem(5, ParentPage.Language.MenuPhrase("5", "MenuText"), "contact_list.aspx", 25, "", "", true)
			RootMenu.AddMenuItem(26, ParentPage.Language.MenuPhrase("26", "MenuText"), "", -1, "", "", True)
			RootMenu.AddMenuItem(14, ParentPage.Language.MenuPhrase("14", "MenuText"), "pageimage_list.aspx?cmd=resetall", 26, "", "", true)
			RootMenu.AddMenuItem(7, ParentPage.Language.MenuPhrase("7", "MenuText"), "image_list.aspx?cmd=resetall", 26, "", "", true)
			RootMenu.AddMenuItem(27, ParentPage.Language.MenuPhrase("27", "MenuText"), "", -1, "", "", True)
			RootMenu.AddMenuItem(8, ParentPage.Language.MenuPhrase("8", "MenuText"), "link_list.aspx?cmd=resetall", 27, "", "", true)
			RootMenu.AddMenuItem(9, ParentPage.Language.MenuPhrase("9", "MenuText"), "linkcategory_list.aspx", 27, "", "", true)
			RootMenu.AddMenuItem(10, ParentPage.Language.MenuPhrase("10", "MenuText"), "linkrank_list.aspx", 27, "", "", true)
			RootMenu.AddMenuItem(11, ParentPage.Language.MenuPhrase("11", "MenuText"), "linktype_list.aspx", 27, "", "", true)
			RootMenu.AddMenuItem(24, ParentPage.Language.MenuPhrase("24", "MenuText"), "", -1, "", "", True)
			RootMenu.AddMenuItem(13, ParentPage.Language.MenuPhrase("13", "MenuText"), "pagealias_list.aspx?cmd=resetall", 24, "", "", true)
			RootMenu.AddMenuItem(1, ParentPage.Language.MenuPhrase("1", "MenuText"), "article_list.aspx?cmd=resetall", 24, "", "", true)
			RootMenu.AddMenuItem(12, ParentPage.Language.MenuPhrase("12", "MenuText"), "zpage_list.aspx?cmd=resetall", 24, "", "", true)
			RootMenu.AddMenuItem(15, ParentPage.Language.MenuPhrase("15", "MenuText"), "pagerole_list.aspx", 24, "", "", true)
			RootMenu.AddMenuItem(16, ParentPage.Language.MenuPhrase("16", "MenuText"), "pagetype_list.aspx", 24, "", "", true)
			RootMenu.AddMenuItem(17, ParentPage.Language.MenuPhrase("17", "MenuText"), "role_list.aspx", 24, "", "", true)
			RootMenu.AddMenuItem(28, ParentPage.Language.MenuPhrase("28", "MenuText"), "", -1, "", "", True)
			RootMenu.AddMenuItem(18, ParentPage.Language.MenuPhrase("18", "MenuText"), "sitecategory_list.aspx", 28, "", "", true)
			RootMenu.AddMenuItem(19, ParentPage.Language.MenuPhrase("19", "MenuText"), "sitecategorygroup_list.aspx", 28, "", "", true)
			RootMenu.AddMenuItem(20, ParentPage.Language.MenuPhrase("20", "MenuText"), "sitecategorytype_list.aspx", 28, "", "", true)
			RootMenu.AddMenuItem(21, ParentPage.Language.MenuPhrase("21", "MenuText"), "sitelink_list.aspx", 28, "", "", true)
			RootMenu.AddMenuItem(22, ParentPage.Language.MenuPhrase("22", "MenuText"), "siteparametertype_list.aspx", 28, "", "", true)
			RootMenu.AddMenuItem(23, ParentPage.Language.MenuPhrase("23", "MenuText"), "sitetemplate_list.aspx", 28, "", "", true)
		End If
	End Sub
End Class
