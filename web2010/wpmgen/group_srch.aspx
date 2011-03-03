<%@ Page ClassName="group_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="group_srch.aspx.vb" Inherits="group_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Group_search = new ew_Page("Group_search");
// page properties
Group_search.PageID = "search"; // page ID
Group_search.FormID = "fGroupsearch"; // form ID 
var EW_PAGE_ID = Group_search.PageID; // for backward compatibility
// extend page with validate function for search
Group_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	} 
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
// extend page with Form_CustomValidate function
Group_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Group.TableCaption %><br /><br />
<a href="<%= Group.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Group_search.DebugMsg) %>
<% Group_search.ShowMessage() %>
<form name="fGroupsearch" id="fGroupsearch" method="post" onsubmit="this.action=location.pathname;return Group_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Group" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Group.RowAttributes %>>
		<td class="ewTableHeader"><%= Group.GroupID.FldCaption %></td>
		<td<%= Group.GroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_GroupID" id="z_GroupID" value="=" /></span></td>
		<td<%= Group.GroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupID" id="x_GroupID" title="<%= Group.GroupID.FldTitle %>" value="<%= Group.GroupID.EditValue %>"<%= Group.GroupID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Group.RowAttributes %>>
		<td class="ewTableHeader"><%= Group.GroupName.FldCaption %></td>
		<td<%= Group.GroupName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_GroupName" id="z_GroupName" value="LIKE" /></span></td>
		<td<%= Group.GroupName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupName" id="x_GroupName" title="<%= Group.GroupName.FldTitle %>" size="30" maxlength="50" value="<%= Group.GroupName.EditValue %>"<%= Group.GroupName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Group.RowAttributes %>>
		<td class="ewTableHeader"><%= Group.GroupComment.FldCaption %></td>
		<td<%= Group.GroupComment.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_GroupComment" id="z_GroupComment" value="LIKE" /></span></td>
		<td<%= Group.GroupComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupComment" id="x_GroupComment" title="<%= Group.GroupComment.FldTitle %>" size="30" maxlength="50" value="<%= Group.GroupComment.EditValue %>"<%= Group.GroupComment.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("Search")) %>" />
<input type="button" name="Reset" id="Reset" value="<%= ew_BtnCaption(Language.Phrase("Reset")) %>" onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
