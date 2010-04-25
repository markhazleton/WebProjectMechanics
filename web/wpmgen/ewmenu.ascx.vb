'
' ASP.NET code-behind class (Menu)
'

Partial Class ewmenu
	Inherits System.Web.UI.UserControl

	Public ParentPage As AspNetMaker7_WPMGen

	Public RootMenu As AspNetMaker7_WPMGen.cMenu

	'
	' User control Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If TypeOf Page Is AspNetMaker7_WPMGen Then
			ParentPage = CType(Page, AspNetMaker7_WPMGen)
			RootMenu = New AspNetMaker7_WPMGen.cMenu("RootMenu", True)
			RootMenu.ParentPage = ParentPage

			' Generate menu items
			RootMenu.AddMenuItem(28, "Administration", "", -1, "", True)
            RootMenu.AddMenuItem(25, "Home", "/wpm/admin", 28, "", True)
			RootMenu.AddMenuItem(29, "Report", "", 28, "", True)
			RootMenu.AddMenuItem(37, "Navigation Report", "/wpm/admin/wpmSiteReport.aspx?ReportID=NavigationAdmin", 29, "", True)
			RootMenu.AddMenuItem(38, "Link Report", "/wpm/admin/wpmSiteReport.aspx?ReportID=LinkAdmin", 29, "", True)
			RootMenu.AddMenuItem(39, "Parameter Report", "/wpm/admin/wpmSiteReport.aspx?ReportID=ParameterAdmin", 29, "", True)
			RootMenu.AddMenuItem(40, "Image Report", "/wpm/admin/wpmSiteReport.aspx?ReportID=ImageAdmin", 29, "", True)
			RootMenu.AddMenuItem(41, "Page Alias", "/wpm/admin/wpmSiteReport.aspx?ReportID=PageAlias", 29, "", True)
			RootMenu.AddMenuItem(30, "Company/Site", "", 28, "", True)
			RootMenu.AddMenuItem(2, "Company/Site List", "Company_list.aspx", 30, "", true)
			RootMenu.AddMenuItem(5, "Contact", "Contact_list.aspx", 30, "", true)
			RootMenu.AddMenuItem(36, "Site Switch", "/wpm/admin/site_switch.aspx", 30, "", True)
			RootMenu.AddMenuItem(42, "Global", "", 28, "", True)
			RootMenu.AddMenuItem(18, "Role", "role_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(16, "Page Role", "PageRole_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(12, "Message", "zMessage_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(6, "Group", "Group_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(17, "Page Type", "PageType_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(10, "Link Rank", "LinkRank_list.aspx", 42, "", true)
			RootMenu.AddMenuItem(26, "Location", "", -1, "", True)
			RootMenu.AddMenuItem(13, "Site Location", "zPage_list.aspx", 26, "", true)
			RootMenu.AddMenuItem(34, "Site Types", "", 26, "", True)
			RootMenu.AddMenuItem(21, "Site Type", "SiteCategoryType_list.aspx", 34, "", true)
			RootMenu.AddMenuItem(19, "Site Type Location", "SiteCategory_list.aspx", 34, "", true)
			RootMenu.AddMenuItem(20, "Location Group", "SiteCategoryGroup_list.aspx", 34, "", true)
			RootMenu.AddMenuItem(14, "Location Alias", "PageAlias_list.aspx", 26, "", true)
			RootMenu.AddMenuItem(27, "Site Parts", "", -1, "", True)
			RootMenu.AddMenuItem(43, "All Parts", "/wpm/admin/AdminLink.aspx", 27, "", True)
			RootMenu.AddMenuItem(8, "Location Parts", "Link_list.aspx", 27, "", true)
			RootMenu.AddMenuItem(22, "Category Parts", "SiteLink_list.aspx", 27, "", true)
			RootMenu.AddMenuItem(1, "Site Articles", "Article_list.aspx", 27, "", true)
			RootMenu.AddMenuItem(11, "Link Type", "LinkType_list.aspx", 27, "", true)
			RootMenu.AddMenuItem(9, "Link Category", "LinkCategory_list.aspx", 27, "", true)
			RootMenu.AddMenuItem(31, "Presentation", "", -1, "", True)
			RootMenu.AddMenuItem(35, "Skin Switch", "/wpm/admin/skin_switch.aspx", 31, "", True)
			RootMenu.AddMenuItem(24, "Site Template", "SiteTemplate_list.aspx", 31, "", true)
			RootMenu.AddMenuItem(32, "Images|Graphics", "", -1, "", True)
			RootMenu.AddMenuItem(7, "Image", "Image_list.aspx", 32, "", true)
			RootMenu.AddMenuItem(15, "Page Image", "PageImage_list.aspx", 32, "", true)
			RootMenu.AddMenuItem(33, "Site Parameter", "", -1, "", True)
			RootMenu.AddMenuItem(23, "Parameter Type", "SiteParameterType_list.aspx", 33, "", true)
			RootMenu.AddMenuItem(4, "Site Type Parameter", "CompanySiteTypeParameter_list.aspx?cmd=resetall", 33, "", true)
			RootMenu.AddMenuItem(3, "Site Parameter", "CompanySiteParameter_list.aspx?cmd=resetall", 33, "", true)
		End If
	End Sub
End Class
