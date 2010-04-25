<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteTemplate_edit.aspx.vb" Inherits="SiteTemplate_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_edit = new ew_Page("SiteTemplate_edit");
// page properties
SiteTemplate_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = SiteTemplate_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteTemplate_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_zName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Name");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Presentation Template (skin)<br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>">Go Back</a></span></p>
<% SiteTemplate_edit.ShowMessage() %>
<form name="fSiteTemplateedit" id="fSiteTemplateedit" method="post" onsubmit="this.action=location.pathname;return SiteTemplate_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteTemplate" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteTemplate.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= SiteTemplate.TemplatePrefix.RowAttributes %>>
		<td class="ewTableHeader">Template Prefix</td>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>><span id="el_TemplatePrefix">
<div<%= SiteTemplate.TemplatePrefix.ViewAttributes %>><%= SiteTemplate.TemplatePrefix.EditValue %></div>
<input type="hidden" name="x_TemplatePrefix" id="x_TemplatePrefix" value="<%= ew_HTMLEncode(SiteTemplate.TemplatePrefix.CurrentValue) %>" />
</span><%= SiteTemplate.TemplatePrefix.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.zName.Visible Then ' Name %>
	<tr<%= SiteTemplate.zName.RowAttributes %>>
		<td class="ewTableHeader">Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= SiteTemplate.zName.CellAttributes %>><span id="el_zName">
<input type="text" name="x_zName" id="x_zName" size="50" maxlength="50" value="<%= SiteTemplate.zName.EditValue %>"<%= SiteTemplate.zName.EditAttributes %> />
</span><%= SiteTemplate.zName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.Top.Visible Then ' Top %>
	<tr<%= SiteTemplate.Top.RowAttributes %>>
		<td class="ewTableHeader">Top</td>
		<td<%= SiteTemplate.Top.CellAttributes %>><span id="el_Top">
<textarea name="x_Top" id="x_Top" cols="80" rows="20"<%= SiteTemplate.Top.EditAttributes %>><%= SiteTemplate.Top.EditValue %></textarea>
</span><%= SiteTemplate.Top.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteTemplate.Bottom.Visible Then ' Bottom %>
	<tr<%= SiteTemplate.Bottom.RowAttributes %>>
		<td class="ewTableHeader">Bottom</td>
		<td<%= SiteTemplate.Bottom.CellAttributes %>><span id="el_Bottom">
<textarea name="x_Bottom" id="x_Bottom" cols="80" rows="20"<%= SiteTemplate.Bottom.EditAttributes %>><%= SiteTemplate.Bottom.EditValue %></textarea>
</span><%= SiteTemplate.Bottom.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
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
