<%@ Page ClassName="pagealias_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pagealias_edit.aspx.vb" Inherits="pagealias_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageAlias_edit = new ew_Page("PageAlias_edit");
// page properties
PageAlias_edit.PageID = "edit"; // page ID
PageAlias_edit.FormID = "fPageAliasedit"; // form ID 
var EW_PAGE_ID = PageAlias_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
PageAlias_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
PageAlias_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageAlias.TableCaption %><br /><br />
<a href="<%= PageAlias.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageAlias_edit.DebugMsg) %>
<% PageAlias_edit.ShowMessage() %>
<form name="fPageAliasedit" id="fPageAliasedit" method="post" onsubmit="this.action=location.pathname;return PageAlias_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="PageAlias" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageAlias.PageAliasID.Visible Then ' PageAliasID %>
	<tr<%= PageAlias.RowAttributes %>>
		<td class="ewTableHeader"><%= PageAlias.PageAliasID.FldCaption %></td>
		<td<%= PageAlias.PageAliasID.CellAttributes %>><span id="el_PageAliasID">
<div<%= PageAlias.PageAliasID.ViewAttributes %>><%= PageAlias.PageAliasID.EditValue %></div>
<input type="hidden" name="x_PageAliasID" id="x_PageAliasID" value="<%= ew_HTMLEncode(PageAlias.PageAliasID.CurrentValue) %>" />
</span><%= PageAlias.PageAliasID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.zPageURL.Visible Then ' PageURL %>
	<tr<%= PageAlias.RowAttributes %>>
		<td class="ewTableHeader"><%= PageAlias.zPageURL.FldCaption %></td>
		<td<%= PageAlias.zPageURL.CellAttributes %>><span id="el_zPageURL">
<input type="text" name="x_zPageURL" id="x_zPageURL" title="<%= PageAlias.zPageURL.FldTitle %>" size="30" maxlength="255" value="<%= PageAlias.zPageURL.EditValue %>"<%= PageAlias.zPageURL.EditAttributes %> />
</span><%= PageAlias.zPageURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.TargetURL.Visible Then ' TargetURL %>
	<tr<%= PageAlias.RowAttributes %>>
		<td class="ewTableHeader"><%= PageAlias.TargetURL.FldCaption %></td>
		<td<%= PageAlias.TargetURL.CellAttributes %>><span id="el_TargetURL">
<input type="text" name="x_TargetURL" id="x_TargetURL" title="<%= PageAlias.TargetURL.FldTitle %>" size="30" maxlength="255" value="<%= PageAlias.TargetURL.EditValue %>"<%= PageAlias.TargetURL.EditAttributes %> />
</span><%= PageAlias.TargetURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.AliasType.Visible Then ' AliasType %>
	<tr<%= PageAlias.RowAttributes %>>
		<td class="ewTableHeader"><%= PageAlias.AliasType.FldCaption %></td>
		<td<%= PageAlias.AliasType.CellAttributes %>><span id="el_AliasType">
<input type="text" name="x_AliasType" id="x_AliasType" title="<%= PageAlias.AliasType.FldTitle %>" size="30" maxlength="10" value="<%= PageAlias.AliasType.EditValue %>"<%= PageAlias.AliasType.EditAttributes %> />
</span><%= PageAlias.AliasType.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageAlias.CompanyID.Visible Then ' CompanyID %>
	<tr<%= PageAlias.RowAttributes %>>
		<td class="ewTableHeader"><%= PageAlias.CompanyID.FldCaption %></td>
		<td<%= PageAlias.CompanyID.CellAttributes %>><span id="el_CompanyID">
<% If PageAlias.CompanyID.SessionValue <> "" Then %>
<div<%= PageAlias.CompanyID.ViewAttributes %>><%= PageAlias.CompanyID.ViewValue %></div>
<input type="hidden" id="x_CompanyID" name="x_CompanyID" value="<%= ew_HtmlEncode(PageAlias.CompanyID.CurrentValue) %>">
<% Else %>
<select id="x_CompanyID" name="x_CompanyID" title="<%= PageAlias.CompanyID.FldTitle %>"<%= PageAlias.CompanyID.EditAttributes %>>
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
<% End If %>
</span><%= PageAlias.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("EditBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
