<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Company_add.aspx.vb" Inherits="Company_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_add = new ew_Page("Company_add");
// page properties
Company_add.PageID = "add"; // page ID
var EW_PAGE_ID = Company_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Company_add.ValidateForm = function(fobj) {
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
			return ew_OnError(this, elm, "Please enter required field - Company Name");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Company_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site<br /><br />
<a href="<%= Company.ReturnUrl %>">Go Back</a></span></p>
<% Company_add.ShowMessage() %>
<form name="fCompanyadd" id="fCompanyadd" method="post" onsubmit="this.action=location.pathname;return Company_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Company" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Company.CompanyName.Visible Then ' CompanyName %>
	<tr<%= Company.CompanyName.RowAttributes %>>
		<td class="ewTableHeader">Company Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Company.CompanyName.CellAttributes %>><span id="el_CompanyName">
<input type="text" name="x_CompanyName" id="x_CompanyName" size="50" maxlength="50" value="<%= Company.CompanyName.EditValue %>"<%= Company.CompanyName.EditAttributes %> />
</span><%= Company.CompanyName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteTitle.Visible Then ' SiteTitle %>
	<tr<%= Company.SiteTitle.RowAttributes %>>
		<td class="ewTableHeader">Site Title</td>
		<td<%= Company.SiteTitle.CellAttributes %>><span id="el_SiteTitle">
<input type="text" name="x_SiteTitle" id="x_SiteTitle" size="50" maxlength="255" value="<%= Company.SiteTitle.EditValue %>"<%= Company.SiteTitle.EditAttributes %> />
</span><%= Company.SiteTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteURL.Visible Then ' SiteURL %>
	<tr<%= Company.SiteURL.RowAttributes %>>
		<td class="ewTableHeader">Site URL</td>
		<td<%= Company.SiteURL.CellAttributes %>><span id="el_SiteURL">
<input type="text" name="x_SiteURL" id="x_SiteURL" size="50" maxlength="255" value="<%= Company.SiteURL.EditValue %>"<%= Company.SiteURL.EditAttributes %> />
</span><%= Company.SiteURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.GalleryFolder.Visible Then ' GalleryFolder %>
	<tr<%= Company.GalleryFolder.RowAttributes %>>
		<td class="ewTableHeader">Gallery Folder</td>
		<td<%= Company.GalleryFolder.CellAttributes %>><span id="el_GalleryFolder">
<input type="text" name="x_GalleryFolder" id="x_GalleryFolder" size="50" maxlength="50" value="<%= Company.GalleryFolder.EditValue %>"<%= Company.GalleryFolder.EditAttributes %> />
</span><%= Company.GalleryFolder.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= Company.SiteCategoryTypeID.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= Company.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID"<%= Company.SiteCategoryTypeID.EditAttributes %>>
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
<% If Company.HomePageID.Visible Then ' HomePageID %>
	<tr<%= Company.HomePageID.RowAttributes %>>
		<td class="ewTableHeader">Home Page</td>
		<td<%= Company.HomePageID.CellAttributes %>><span id="el_HomePageID">
<select id="x_HomePageID" name="x_HomePageID"<%= Company.HomePageID.EditAttributes %>>
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
	<tr<%= Company.DefaultArticleID.RowAttributes %>>
		<td class="ewTableHeader">Default Article</td>
		<td<%= Company.DefaultArticleID.CellAttributes %>><span id="el_DefaultArticleID">
<select id="x_DefaultArticleID" name="x_DefaultArticleID"<%= Company.DefaultArticleID.EditAttributes %>>
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
	<tr<%= Company.SiteTemplate.RowAttributes %>>
		<td class="ewTableHeader">Template</td>
		<td<%= Company.SiteTemplate.CellAttributes %>><span id="el_SiteTemplate">
<select id="x_SiteTemplate" name="x_SiteTemplate"<%= Company.SiteTemplate.EditAttributes %>>
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
	<tr<%= Company.DefaultSiteTemplate.RowAttributes %>>
		<td class="ewTableHeader">Default Template</td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>><span id="el_DefaultSiteTemplate">
<select id="x_DefaultSiteTemplate" name="x_DefaultSiteTemplate"<%= Company.DefaultSiteTemplate.EditAttributes %>>
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
<% If Company.UseBreadCrumbURL.Visible Then ' UseBreadCrumbURL %>
	<tr<%= Company.UseBreadCrumbURL.RowAttributes %>>
		<td class="ewTableHeader">Use Bread Crumb URL</td>
		<td<%= Company.UseBreadCrumbURL.CellAttributes %>><span id="el_UseBreadCrumbURL">
<%
If ew_SameStr(Company.UseBreadCrumbURL.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_UseBreadCrumbURL" id="x_UseBreadCrumbURL" value="1"<%= selwrk %><%= Company.UseBreadCrumbURL.EditAttributes %> />
</span><%= Company.UseBreadCrumbURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SingleSiteGallery.Visible Then ' SingleSiteGallery %>
	<tr<%= Company.SingleSiteGallery.RowAttributes %>>
		<td class="ewTableHeader">Single Site Gallery</td>
		<td<%= Company.SingleSiteGallery.CellAttributes %>><span id="el_SingleSiteGallery">
<%
If ew_SameStr(Company.SingleSiteGallery.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_SingleSiteGallery" id="x_SingleSiteGallery" value="1"<%= selwrk %><%= Company.SingleSiteGallery.EditAttributes %> />
</span><%= Company.SingleSiteGallery.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.ActiveFL.Visible Then ' ActiveFL %>
	<tr<%= Company.ActiveFL.RowAttributes %>>
		<td class="ewTableHeader">Active FL</td>
		<td<%= Company.ActiveFL.CellAttributes %>><span id="el_ActiveFL">
<%
If ew_SameStr(Company.ActiveFL.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_ActiveFL" id="x_ActiveFL" value="1"<%= selwrk %><%= Company.ActiveFL.EditAttributes %> />
</span><%= Company.ActiveFL.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.Address.Visible Then ' Address %>
	<tr<%= Company.Address.RowAttributes %>>
		<td class="ewTableHeader">Address</td>
		<td<%= Company.Address.CellAttributes %>><span id="el_Address">
<input type="text" name="x_Address" id="x_Address" size="30" maxlength="255" value="<%= Company.Address.EditValue %>"<%= Company.Address.EditAttributes %> />
</span><%= Company.Address.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.City.Visible Then ' City %>
	<tr<%= Company.City.RowAttributes %>>
		<td class="ewTableHeader">City</td>
		<td<%= Company.City.CellAttributes %>><span id="el_City">
<input type="text" name="x_City" id="x_City" size="30" maxlength="50" value="<%= Company.City.EditValue %>"<%= Company.City.EditAttributes %> />
</span><%= Company.City.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.StateOrProvince.Visible Then ' StateOrProvince %>
	<tr<%= Company.StateOrProvince.RowAttributes %>>
		<td class="ewTableHeader">State/Province</td>
		<td<%= Company.StateOrProvince.CellAttributes %>><span id="el_StateOrProvince">
<input type="text" name="x_StateOrProvince" id="x_StateOrProvince" size="30" maxlength="20" value="<%= Company.StateOrProvince.EditValue %>"<%= Company.StateOrProvince.EditAttributes %> />
</span><%= Company.StateOrProvince.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.PostalCode.Visible Then ' PostalCode %>
	<tr<%= Company.PostalCode.RowAttributes %>>
		<td class="ewTableHeader">Postal Code</td>
		<td<%= Company.PostalCode.CellAttributes %>><span id="el_PostalCode">
<input type="text" name="x_PostalCode" id="x_PostalCode" size="30" maxlength="20" value="<%= Company.PostalCode.EditValue %>"<%= Company.PostalCode.EditAttributes %> />
</span><%= Company.PostalCode.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.Country.Visible Then ' Country %>
	<tr<%= Company.Country.RowAttributes %>>
		<td class="ewTableHeader">Country</td>
		<td<%= Company.Country.CellAttributes %>><span id="el_Country">
<input type="text" name="x_Country" id="x_Country" size="30" maxlength="50" value="<%= Company.Country.EditValue %>"<%= Company.Country.EditAttributes %> />
</span><%= Company.Country.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.Component.Visible Then ' Component %>
	<tr<%= Company.Component.RowAttributes %>>
		<td class="ewTableHeader">Component</td>
		<td<%= Company.Component.CellAttributes %>><span id="el_Component">
<input type="text" name="x_Component" id="x_Component" size="30" maxlength="50" value="<%= Company.Component.EditValue %>"<%= Company.Component.EditAttributes %> />
</span><%= Company.Component.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.FromEmail.Visible Then ' FromEmail %>
	<tr<%= Company.FromEmail.RowAttributes %>>
		<td class="ewTableHeader">From Email</td>
		<td<%= Company.FromEmail.CellAttributes %>><span id="el_FromEmail">
<input type="text" name="x_FromEmail" id="x_FromEmail" size="30" maxlength="50" value="<%= Company.FromEmail.EditValue %>"<%= Company.FromEmail.EditAttributes %> />
</span><%= Company.FromEmail.CustomMsg %></td>
	</tr>
<% End If %>
<% If Company.SMTP.Visible Then ' SMTP %>
	<tr<%= Company.SMTP.RowAttributes %>>
		<td class="ewTableHeader">SMTP</td>
		<td<%= Company.SMTP.CellAttributes %>><span id="el_SMTP">
<input type="text" name="x_SMTP" id="x_SMTP" size="30" maxlength="50" value="<%= Company.SMTP.EditValue %>"<%= Company.SMTP.EditAttributes %> />
</span><%= Company.SMTP.CustomMsg %></td>
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
