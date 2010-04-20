<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteTemplate_srch.aspx.vb" Inherits="SiteTemplate_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_search = new ew_Page("SiteTemplate_search");
// page properties
SiteTemplate_search.PageID = "search"; // page ID
var EW_PAGE_ID = SiteTemplate_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteTemplate_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_search.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Search TABLE: Presentation Template (skin)<br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>">Back to List</a></span></p>
<% SiteTemplate_search.ShowMessage() %>
<form name="fSiteTemplatesearch" id="fSiteTemplatesearch" method="post" onsubmit="this.action=location.pathname;return SiteTemplate_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteTemplate" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
