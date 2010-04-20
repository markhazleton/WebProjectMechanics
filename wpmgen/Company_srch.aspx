<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Company_srch.aspx.vb" Inherits="Company_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_search = new ew_Page("Company_search");
// page properties
Company_search.PageID = "search"; // page ID
var EW_PAGE_ID = Company_search.PageID; // for backward compatibility
// extend page with validate function for search
Company_search.ValidateSearch = function(fobj) {
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
Company_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site<br /><br />
<a href="<%= Company.ReturnUrl %>">Back to List</a></span></p>
<% Company_search.ShowMessage() %>
<form name="fCompanysearch" id="fCompanysearch" method="post" onsubmit="this.action=location.pathname;return Company_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Company" />
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
