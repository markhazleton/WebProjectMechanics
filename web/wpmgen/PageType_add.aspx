<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageType_add.aspx.vb" Inherits="PageType_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageType_add = new ew_Page("PageType_add");
// page properties
PageType_add.PageID = "add"; // page ID
var EW_PAGE_ID = PageType_add.PageID; // for backward compatibility
// extend page with ValidateForm function
PageType_add.ValidateForm = function(fobj) {
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
PageType_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Location Type<br /><br />
<a href="<%= PageType.ReturnUrl %>">Go Back</a></span></p>
<% PageType_add.ShowMessage() %>
<form name="fPageTypeadd" id="fPageTypeadd" method="post" onsubmit="this.action=location.pathname;return PageType_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="PageType" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageType.PageTypeCD.Visible Then ' PageTypeCD %>
	<tr<%= PageType.PageTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Page Type CD</td>
		<td<%= PageType.PageTypeCD.CellAttributes %>><span id="el_PageTypeCD">
<input type="text" name="x_PageTypeCD" id="x_PageTypeCD" size="30" maxlength="50" value="<%= PageType.PageTypeCD.EditValue %>"<%= PageType.PageTypeCD.EditAttributes %> />
</span><%= PageType.PageTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageType.PageTypeDesc.Visible Then ' PageTypeDesc %>
	<tr<%= PageType.PageTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Page Type Desc</td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>><span id="el_PageTypeDesc">
<input type="text" name="x_PageTypeDesc" id="x_PageTypeDesc" size="30" maxlength="50" value="<%= PageType.PageTypeDesc.EditValue %>"<%= PageType.PageTypeDesc.EditAttributes %> />
</span><%= PageType.PageTypeDesc.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageType.PageFileName.Visible Then ' PageFileName %>
	<tr<%= PageType.PageFileName.RowAttributes %>>
		<td class="ewTableHeader">Page File Name</td>
		<td<%= PageType.PageFileName.CellAttributes %>><span id="el_PageFileName">
<input type="text" name="x_PageFileName" id="x_PageFileName" size="30" maxlength="50" value="<%= PageType.PageFileName.EditValue %>"<%= PageType.PageFileName.EditAttributes %> />
</span><%= PageType.PageFileName.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
