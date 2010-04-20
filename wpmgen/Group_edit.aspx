<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Group_edit.aspx.vb" Inherits="Group_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Group_edit = new ew_Page("Group_edit");
// page properties
Group_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = Group_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
Group_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Group_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Group_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Contact Group<br /><br />
<a href="<%= Group.ReturnUrl %>">Go Back</a></span></p>
<% Group_edit.ShowMessage() %>
<form name="fGroupedit" id="fGroupedit" method="post" onsubmit="this.action=location.pathname;return Group_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="Group" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Group.GroupName.Visible Then ' GroupName %>
	<tr<%= Group.GroupName.RowAttributes %>>
		<td class="ewTableHeader">Group Name</td>
		<td<%= Group.GroupName.CellAttributes %>><span id="el_GroupName">
<input type="text" name="x_GroupName" id="x_GroupName" size="30" maxlength="50" value="<%= Group.GroupName.EditValue %>"<%= Group.GroupName.EditAttributes %> />
</span><%= Group.GroupName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Group.GroupComment.Visible Then ' GroupComment %>
	<tr<%= Group.GroupComment.RowAttributes %>>
		<td class="ewTableHeader">Group Comment</td>
		<td<%= Group.GroupComment.CellAttributes %>><span id="el_GroupComment">
<input type="text" name="x_GroupComment" id="x_GroupComment" size="30" maxlength="50" value="<%= Group.GroupComment.EditValue %>"<%= Group.GroupComment.EditAttributes %> />
</span><%= Group.GroupComment.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_GroupID" id="x_GroupID" value="<%= ew_HTMLEncode(Group.GroupID.CurrentValue) %>" />
<p />
<input type="submit" name="btnAction" id="btnAction" value="Save Changes" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
