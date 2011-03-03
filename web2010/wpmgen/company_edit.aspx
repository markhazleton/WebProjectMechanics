<%@ Page ClassName="company_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="company_edit.aspx.vb" Inherits="company_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_edit = new ew_Page("Company_edit");
// page properties
Company_edit.PageID = "edit"; // page ID
Company_edit.FormID = "fCompanyedit"; // form ID 
var EW_PAGE_ID = Company_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
Company_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_CompanyName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(Company.CompanyName.FldCaption) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
Company_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Company_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Company.TableCaption %><br /><br />
<a href="<%= Company.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Company_edit.DebugMsg) %>
<% Company_edit.ShowMessage() %>
<form name="fCompanyedit" id="fCompanyedit" method="post" onsubmit="this.action=location.pathname;return Company_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="Company" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<input type="hidden" name="x_CompanyID" id="x_CompanyID" value="<%= ew_HTMLEncode(Company.CompanyID.CurrentValue) %>" />
<% If Company.CompanyName.Visible Then ' CompanyName %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.CompanyName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Company.CompanyName.CellAttributes %>><span id="el_CompanyName">
<input type="text" name="x_CompanyName" id="x_CompanyName" title="<%= Company.CompanyName.FldTitle %>" size="30" maxlength="50" value="<%= Company.CompanyName.EditValue %>"<%= Company.CompanyName.EditAttributes %> />
</span><%= Company.CompanyName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteCategoryTypeID.FldCaption %></td>
		<td<%= Company.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" title="<%= Company.SiteCategoryTypeID.FldTitle %>"<%= Company.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.SiteCategoryTypeID.EditValue) Then
	arwrk = Company.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.SiteCategoryTypeID.CurrentValue) Then
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
</span><%= Company.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteTitle.Visible Then ' SiteTitle %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteTitle.FldCaption %></td>
		<td<%= Company.SiteTitle.CellAttributes %>><span id="el_SiteTitle">
<input type="text" name="x_SiteTitle" id="x_SiteTitle" title="<%= Company.SiteTitle.FldTitle %>" size="30" maxlength="255" value="<%= Company.SiteTitle.EditValue %>"<%= Company.SiteTitle.EditAttributes %> />
</span><%= Company.SiteTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteURL.Visible Then ' SiteURL %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteURL.FldCaption %></td>
		<td<%= Company.SiteURL.CellAttributes %>><span id="el_SiteURL">
<input type="text" name="x_SiteURL" id="x_SiteURL" title="<%= Company.SiteURL.FldTitle %>" size="30" maxlength="255" value="<%= Company.SiteURL.EditValue %>"<%= Company.SiteURL.EditAttributes %> />
</span><%= Company.SiteURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.GalleryFolder.Visible Then ' GalleryFolder %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.GalleryFolder.FldCaption %></td>
		<td<%= Company.GalleryFolder.CellAttributes %>><span id="el_GalleryFolder">
<input type="text" name="x_GalleryFolder" id="x_GalleryFolder" title="<%= Company.GalleryFolder.FldTitle %>" size="30" maxlength="50" value="<%= Company.GalleryFolder.EditValue %>"<%= Company.GalleryFolder.EditAttributes %> />
</span><%= Company.GalleryFolder.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.HomePageID.Visible Then ' HomePageID %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.HomePageID.FldCaption %></td>
		<td<%= Company.HomePageID.CellAttributes %>><span id="el_HomePageID">
<select id="x_HomePageID" name="x_HomePageID" title="<%= Company.HomePageID.FldTitle %>"<%= Company.HomePageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.HomePageID.EditValue) Then
	arwrk = Company.HomePageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.HomePageID.CurrentValue) Then
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
</span><%= Company.HomePageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.DefaultArticleID.Visible Then ' DefaultArticleID %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultArticleID.FldCaption %></td>
		<td<%= Company.DefaultArticleID.CellAttributes %>><span id="el_DefaultArticleID">
<select id="x_DefaultArticleID" name="x_DefaultArticleID" title="<%= Company.DefaultArticleID.FldTitle %>"<%= Company.DefaultArticleID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.DefaultArticleID.EditValue) Then
	arwrk = Company.DefaultArticleID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.DefaultArticleID.CurrentValue) Then
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
</span><%= Company.DefaultArticleID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteTemplate.Visible Then ' SiteTemplate %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteTemplate.FldCaption %></td>
		<td<%= Company.SiteTemplate.CellAttributes %>><span id="el_SiteTemplate">
<select id="x_SiteTemplate" name="x_SiteTemplate" title="<%= Company.SiteTemplate.FldTitle %>"<%= Company.SiteTemplate.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.SiteTemplate.EditValue) Then
	arwrk = Company.SiteTemplate.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.SiteTemplate.CurrentValue) Then
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
</span><%= Company.SiteTemplate.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.DefaultSiteTemplate.Visible Then ' DefaultSiteTemplate %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultSiteTemplate.FldCaption %></td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>><span id="el_DefaultSiteTemplate">
<select id="x_DefaultSiteTemplate" name="x_DefaultSiteTemplate" title="<%= Company.DefaultSiteTemplate.FldTitle %>"<%= Company.DefaultSiteTemplate.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.DefaultSiteTemplate.EditValue) Then
	arwrk = Company.DefaultSiteTemplate.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.DefaultSiteTemplate.CurrentValue) Then
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
</span><%= Company.DefaultSiteTemplate.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.Component.Visible Then ' Component %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.Component.FldCaption %></td>
		<td<%= Company.Component.CellAttributes %>><span id="el_Component">
<input type="text" name="x_Component" id="x_Component" title="<%= Company.Component.FldTitle %>" size="30" maxlength="50" value="<%= Company.Component.EditValue %>"<%= Company.Component.EditAttributes %> />
</span><%= Company.Component.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.FromEmail.Visible Then ' FromEmail %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.FromEmail.FldCaption %></td>
		<td<%= Company.FromEmail.CellAttributes %>><span id="el_FromEmail">
<input type="text" name="x_FromEmail" id="x_FromEmail" title="<%= Company.FromEmail.FldTitle %>" size="30" maxlength="50" value="<%= Company.FromEmail.EditValue %>"<%= Company.FromEmail.EditAttributes %> />
</span><%= Company.FromEmail.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SMTP.Visible Then ' SMTP %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SMTP.FldCaption %></td>
		<td<%= Company.SMTP.CellAttributes %>><span id="el_SMTP">
<input type="text" name="x_SMTP" id="x_SMTP" title="<%= Company.SMTP.FldTitle %>" size="30" maxlength="50" value="<%= Company.SMTP.EditValue %>"<%= Company.SMTP.EditAttributes %> />
</span><%= Company.SMTP.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.ActiveFL.Visible Then ' ActiveFL %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.ActiveFL.FldCaption %></td>
		<td<%= Company.ActiveFL.CellAttributes %>><span id="el_ActiveFL">
<%
If ew_SameStr(Company.ActiveFL.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_ActiveFL" id="x_ActiveFL" title="<%= Company.ActiveFL.FldTitle %>" value="1"<%= selwrk %><%= Company.ActiveFL.EditAttributes %> />
</span><%= Company.ActiveFL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.UseBreadCrumbURL.Visible Then ' UseBreadCrumbURL %>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.UseBreadCrumbURL.FldCaption %></td>
		<td<%= Company.UseBreadCrumbURL.CellAttributes %>><span id="el_UseBreadCrumbURL">
<%
If ew_SameStr(Company.UseBreadCrumbURL.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_UseBreadCrumbURL" id="x_UseBreadCrumbURL" title="<%= Company.UseBreadCrumbURL.FldTitle %>" value="1"<%= selwrk %><%= Company.UseBreadCrumbURL.EditAttributes %> />
</span><%= Company.UseBreadCrumbURL.CustomMsg %></td>
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
