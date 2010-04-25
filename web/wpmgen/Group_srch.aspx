<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Group_srch.aspx.vb" Inherits="Group_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Group_search = new ew_Page("Group_search");
// page properties
Group_search.PageID = "search"; // page ID
var EW_PAGE_ID = Group_search.PageID; // for backward compatibility
// extend page with validate function for search
Group_search.ValidateSearch = function(fobj) {
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
Group_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Group_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Contact Group<br /><br />
<a href="<%= Group.ReturnUrl %>">Back to List</a></span></p>
<% Group_search.ShowMessage() %>
<form name="fGroupsearch" id="fGroupsearch" method="post" onsubmit="this.action=location.pathname;return Group_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Group" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Group.GroupName.RowAttributes %>>
		<td class="ewTableHeader">Group Name</td>
		<td<%= Group.GroupName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_GroupName" id="z_GroupName" value="LIKE" /></span></td>
		<td<%= Group.GroupName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupName" id="x_GroupName" size="30" maxlength="50" value="<%= Group.GroupName.EditValue %>"<%= Group.GroupName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Group.GroupComment.RowAttributes %>>
		<td class="ewTableHeader">Group Comment</td>
		<td<%= Group.GroupComment.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_GroupComment" id="z_GroupComment" value="LIKE" /></span></td>
		<td<%= Group.GroupComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupComment" id="x_GroupComment" size="30" maxlength="50" value="<%= Group.GroupComment.EditValue %>"<%= Group.GroupComment.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
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
