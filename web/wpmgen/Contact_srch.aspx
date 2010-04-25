<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Contact_srch.aspx.vb" Inherits="Contact_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_search = new ew_Page("Contact_search");
// page properties
Contact_search.PageID = "search"; // page ID
var EW_PAGE_ID = Contact_search.PageID; // for backward compatibility
// extend page with validate function for search
Contact_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_zEMail"];
	if (elm && !ew_CheckEmail(elm.value))
		return ew_OnError(this, elm, "Incorrect email - EMail");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Contact_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Site Contact<br /><br />
<a href="<%= Contact.ReturnUrl %>">Back to List</a></span></p>
<% Contact_search.ShowMessage() %>
<form name="fContactsearch" id="fContactsearch" method="post" onsubmit="this.action=location.pathname;return Contact_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Contact.LogonName.RowAttributes %>>
		<td class="ewTableHeader">Logon Name</td>
		<td<%= Contact.LogonName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_LogonName" id="z_LogonName" value="LIKE" /></span></td>
		<td<%= Contact.LogonName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LogonName" id="x_LogonName" size="30" maxlength="50" value="<%= Contact.LogonName.EditValue %>"<%= Contact.LogonName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.PrimaryContact.RowAttributes %>>
		<td class="ewTableHeader">Full Name</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PrimaryContact" id="z_PrimaryContact" value="LIKE" /></span></td>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PrimaryContact" id="x_PrimaryContact" size="30" maxlength="255" value="<%= Contact.PrimaryContact.EditValue %>"<%= Contact.PrimaryContact.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.zEMail.RowAttributes %>>
		<td class="ewTableHeader">EMail</td>
		<td<%= Contact.zEMail.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_zEMail" id="z_zEMail" value="LIKE" /></span></td>
		<td<%= Contact.zEMail.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zEMail" id="x_zEMail" size="30" maxlength="50" value="<%= Contact.zEMail.EditValue %>"<%= Contact.zEMail.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Contact.Active.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
		<td<%= Contact.Active.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Contact.Active.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= Contact.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Contact.CompanyID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Contact.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID"<%= Contact.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.CompanyID.EditValue) Then
	arwrk = Contact.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.CompanyID.AdvancedSearch.SearchValue) Then
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group</td>
		<td<%= Contact.GroupID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_GroupID" id="z_GroupID" value="=" /></span></td>
		<td<%= Contact.GroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_GroupID" name="x_GroupID"<%= Contact.GroupID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.GroupID.EditValue) Then
	arwrk = Contact.GroupID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.GroupID.AdvancedSearch.SearchValue) Then
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.TemplatePrefix.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_TemplatePrefix" id="z_TemplatePrefix" value="=" /></span></td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_TemplatePrefix" name="x_TemplatePrefix"<%= Contact.TemplatePrefix.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.TemplatePrefix.EditValue) Then
	arwrk = Contact.TemplatePrefix.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.TemplatePrefix.AdvancedSearch.SearchValue) Then
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
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Contact.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= Contact.RoleID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_RoleID" id="z_RoleID" value="=" /></span></td>
		<td<%= Contact.RoleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_RoleID" name="x_RoleID"<%= Contact.RoleID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Contact.RoleID.EditValue) Then
	arwrk = Contact.RoleID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Contact.RoleID.AdvancedSearch.SearchValue) Then
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
