<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="PageAlias_edit.aspx.vb" Inherits="PageAlias_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageAlias_edit = new ew_Page("PageAlias_edit");
// page properties
PageAlias_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = PageAlias_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
PageAlias_edit.ValidateForm = function(fobj) {
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
PageAlias_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Location Alias<br /><br />
<a href="<%= PageAlias.ReturnUrl %>">Go Back</a></span></p>
<% PageAlias_edit.ShowMessage() %>
<form name="fPageAliasedit" id="fPageAliasedit" method="post" onsubmit="this.action=location.pathname;return PageAlias_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="PageAlias" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageAlias.zPageURL.Visible Then ' PageURL %>
	<tr<%= PageAlias.zPageURL.RowAttributes %>>
		<td class="ewTableHeader">Page URL</td>
		<td<%= PageAlias.zPageURL.CellAttributes %>><span id="el_zPageURL">
<input type="text" name="x_zPageURL" id="x_zPageURL" size="30" maxlength="255" value="<%= PageAlias.zPageURL.EditValue %>"<%= PageAlias.zPageURL.EditAttributes %> />
</span><%= PageAlias.zPageURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.TargetURL.Visible Then ' TargetURL %>
	<tr<%= PageAlias.TargetURL.RowAttributes %>>
		<td class="ewTableHeader">Target URL</td>
		<td<%= PageAlias.TargetURL.CellAttributes %>><span id="el_TargetURL">
<input type="text" name="x_TargetURL" id="x_TargetURL" size="30" maxlength="255" value="<%= PageAlias.TargetURL.EditValue %>"<%= PageAlias.TargetURL.EditAttributes %> />
</span><%= PageAlias.TargetURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.AliasType.Visible Then ' AliasType %>
	<tr<%= PageAlias.AliasType.RowAttributes %>>
		<td class="ewTableHeader">Alias Type</td>
		<td<%= PageAlias.AliasType.CellAttributes %>><span id="el_AliasType">
<input type="text" name="x_AliasType" id="x_AliasType" size="30" maxlength="10" value="<%= PageAlias.AliasType.EditValue %>"<%= PageAlias.AliasType.EditAttributes %> />
</span><%= PageAlias.AliasType.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageAlias.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= PageAlias.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID"<%= PageAlias.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageAlias.CompanyID.EditValue) Then
	arwrk = PageAlias.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageAlias.CompanyID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span><%= PageAlias.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<input type="hidden" name="x_PageAliasID" id="x_PageAliasID" value="<%= ew_HTMLEncode(PageAlias.PageAliasID.CurrentValue) %>" />
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
