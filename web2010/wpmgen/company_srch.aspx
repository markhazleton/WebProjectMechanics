<%@ Page ClassName="company_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="company_srch.aspx.vb" Inherits="company_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_search = new ew_Page("Company_search");
// page properties
Company_search.PageID = "search"; // page ID
Company_search.FormID = "fCompanysearch"; // form ID 
var EW_PAGE_ID = Company_search.PageID; // for backward compatibility
// extend page with validate function for search
Company_search.ValidateSearch = function(fobj) {
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
Company_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Company_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Company.TableCaption %><br /><br />
<a href="<%= Company.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Company_search.DebugMsg) %>
<% Company_search.ShowMessage() %>
<form name="fCompanysearch" id="fCompanysearch" method="post" onsubmit="this.action=location.pathname;return Company_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Company" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.CompanyID.FldCaption %></td>
		<td<%= Company.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Company.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CompanyID" id="x_CompanyID" title="<%= Company.CompanyID.FldTitle %>" value="<%= Company.CompanyID.EditValue %>"<%= Company.CompanyID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.CompanyName.FldCaption %></td>
		<td<%= Company.CompanyName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CompanyName" id="z_CompanyName" value="LIKE" /></span></td>
		<td<%= Company.CompanyName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CompanyName" id="x_CompanyName" title="<%= Company.CompanyName.FldTitle %>" size="30" maxlength="50" value="<%= Company.CompanyName.EditValue %>"<%= Company.CompanyName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteCategoryTypeID.FldCaption %></td>
		<td<%= Company.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= Company.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteCategoryTypeID" name="x_SiteCategoryTypeID" title="<%= Company.SiteCategoryTypeID.FldTitle %>"<%= Company.SiteCategoryTypeID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.SiteCategoryTypeID.EditValue) Then
	arwrk = Company.SiteCategoryTypeID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.SiteCategoryTypeID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteTitle.FldCaption %></td>
		<td<%= Company.SiteTitle.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteTitle" id="z_SiteTitle" value="LIKE" /></span></td>
		<td<%= Company.SiteTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteTitle" id="x_SiteTitle" title="<%= Company.SiteTitle.FldTitle %>" size="30" maxlength="255" value="<%= Company.SiteTitle.EditValue %>"<%= Company.SiteTitle.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteURL.FldCaption %></td>
		<td<%= Company.SiteURL.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteURL" id="z_SiteURL" value="LIKE" /></span></td>
		<td<%= Company.SiteURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteURL" id="x_SiteURL" title="<%= Company.SiteURL.FldTitle %>" size="30" maxlength="255" value="<%= Company.SiteURL.EditValue %>"<%= Company.SiteURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.GalleryFolder.FldCaption %></td>
		<td<%= Company.GalleryFolder.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_GalleryFolder" id="z_GalleryFolder" value="LIKE" /></span></td>
		<td<%= Company.GalleryFolder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GalleryFolder" id="x_GalleryFolder" title="<%= Company.GalleryFolder.FldTitle %>" size="30" maxlength="50" value="<%= Company.GalleryFolder.EditValue %>"<%= Company.GalleryFolder.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.HomePageID.FldCaption %></td>
		<td<%= Company.HomePageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_HomePageID" id="z_HomePageID" value="=" /></span></td>
		<td<%= Company.HomePageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_HomePageID" name="x_HomePageID" title="<%= Company.HomePageID.FldTitle %>"<%= Company.HomePageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.HomePageID.EditValue) Then
	arwrk = Company.HomePageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.HomePageID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultArticleID.FldCaption %></td>
		<td<%= Company.DefaultArticleID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_DefaultArticleID" id="z_DefaultArticleID" value="=" /></span></td>
		<td<%= Company.DefaultArticleID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_DefaultArticleID" name="x_DefaultArticleID" title="<%= Company.DefaultArticleID.FldTitle %>"<%= Company.DefaultArticleID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.DefaultArticleID.EditValue) Then
	arwrk = Company.DefaultArticleID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.DefaultArticleID.AdvancedSearch.SearchValue) Then
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
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SiteTemplate.FldCaption %></td>
		<td<%= Company.SiteTemplate.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SiteTemplate" id="z_SiteTemplate" value="LIKE" /></span></td>
		<td<%= Company.SiteTemplate.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_SiteTemplate" name="x_SiteTemplate" title="<%= Company.SiteTemplate.FldTitle %>"<%= Company.SiteTemplate.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.SiteTemplate.EditValue) Then
	arwrk = Company.SiteTemplate.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.SiteTemplate.AdvancedSearch.SearchValue) Then
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
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultSiteTemplate.FldCaption %></td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_DefaultSiteTemplate" id="z_DefaultSiteTemplate" value="LIKE" /></span></td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_DefaultSiteTemplate" name="x_DefaultSiteTemplate" title="<%= Company.DefaultSiteTemplate.FldTitle %>"<%= Company.DefaultSiteTemplate.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Company.DefaultSiteTemplate.EditValue) Then
	arwrk = Company.DefaultSiteTemplate.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Company.DefaultSiteTemplate.AdvancedSearch.SearchValue) Then
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
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.Address.FldCaption %></td>
		<td<%= Company.Address.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Address" id="z_Address" value="LIKE" /></span></td>
		<td<%= Company.Address.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Address" id="x_Address" title="<%= Company.Address.FldTitle %>" size="30" maxlength="255" value="<%= Company.Address.EditValue %>"<%= Company.Address.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.City.FldCaption %></td>
		<td<%= Company.City.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_City" id="z_City" value="LIKE" /></span></td>
		<td<%= Company.City.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_City" id="x_City" title="<%= Company.City.FldTitle %>" size="30" maxlength="50" value="<%= Company.City.EditValue %>"<%= Company.City.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.StateOrProvince.FldCaption %></td>
		<td<%= Company.StateOrProvince.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_StateOrProvince" id="z_StateOrProvince" value="LIKE" /></span></td>
		<td<%= Company.StateOrProvince.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_StateOrProvince" id="x_StateOrProvince" title="<%= Company.StateOrProvince.FldTitle %>" size="30" maxlength="20" value="<%= Company.StateOrProvince.EditValue %>"<%= Company.StateOrProvince.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.PostalCode.FldCaption %></td>
		<td<%= Company.PostalCode.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PostalCode" id="z_PostalCode" value="LIKE" /></span></td>
		<td<%= Company.PostalCode.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PostalCode" id="x_PostalCode" title="<%= Company.PostalCode.FldTitle %>" size="30" maxlength="20" value="<%= Company.PostalCode.EditValue %>"<%= Company.PostalCode.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.Country.FldCaption %></td>
		<td<%= Company.Country.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Country" id="z_Country" value="LIKE" /></span></td>
		<td<%= Company.Country.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Country" id="x_Country" title="<%= Company.Country.FldTitle %>" size="30" maxlength="50" value="<%= Company.Country.EditValue %>"<%= Company.Country.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.PhoneNumber.FldCaption %></td>
		<td<%= Company.PhoneNumber.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_PhoneNumber" id="z_PhoneNumber" value="LIKE" /></span></td>
		<td<%= Company.PhoneNumber.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PhoneNumber" id="x_PhoneNumber" title="<%= Company.PhoneNumber.FldTitle %>" size="30" maxlength="30" value="<%= Company.PhoneNumber.EditValue %>"<%= Company.PhoneNumber.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.FaxNumber.FldCaption %></td>
		<td<%= Company.FaxNumber.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_FaxNumber" id="z_FaxNumber" value="LIKE" /></span></td>
		<td<%= Company.FaxNumber.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_FaxNumber" id="x_FaxNumber" title="<%= Company.FaxNumber.FldTitle %>" size="30" maxlength="30" value="<%= Company.FaxNumber.EditValue %>"<%= Company.FaxNumber.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultPaymentTerms.FldCaption %></td>
		<td<%= Company.DefaultPaymentTerms.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_DefaultPaymentTerms" id="z_DefaultPaymentTerms" value="LIKE" /></span></td>
		<td<%= Company.DefaultPaymentTerms.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_DefaultPaymentTerms" id="x_DefaultPaymentTerms" title="<%= Company.DefaultPaymentTerms.FldTitle %>" size="30" maxlength="255" value="<%= Company.DefaultPaymentTerms.EditValue %>"<%= Company.DefaultPaymentTerms.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.DefaultInvoiceDescription.FldCaption %></td>
		<td<%= Company.DefaultInvoiceDescription.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_DefaultInvoiceDescription" id="z_DefaultInvoiceDescription" value="LIKE" /></span></td>
		<td<%= Company.DefaultInvoiceDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_DefaultInvoiceDescription" id="x_DefaultInvoiceDescription" title="<%= Company.DefaultInvoiceDescription.FldTitle %>" cols="35" rows="4"<%= Company.DefaultInvoiceDescription.EditAttributes %>><%= Company.DefaultInvoiceDescription.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.Component.FldCaption %></td>
		<td<%= Company.Component.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Component" id="z_Component" value="LIKE" /></span></td>
		<td<%= Company.Component.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Component" id="x_Component" title="<%= Company.Component.FldTitle %>" size="30" maxlength="50" value="<%= Company.Component.EditValue %>"<%= Company.Component.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.FromEmail.FldCaption %></td>
		<td<%= Company.FromEmail.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_FromEmail" id="z_FromEmail" value="LIKE" /></span></td>
		<td<%= Company.FromEmail.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_FromEmail" id="x_FromEmail" title="<%= Company.FromEmail.FldTitle %>" size="30" maxlength="50" value="<%= Company.FromEmail.EditValue %>"<%= Company.FromEmail.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SMTP.FldCaption %></td>
		<td<%= Company.SMTP.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_SMTP" id="z_SMTP" value="LIKE" /></span></td>
		<td<%= Company.SMTP.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SMTP" id="x_SMTP" title="<%= Company.SMTP.FldTitle %>" size="30" maxlength="50" value="<%= Company.SMTP.EditValue %>"<%= Company.SMTP.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.ActiveFL.FldCaption %></td>
		<td<%= Company.ActiveFL.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ActiveFL" id="z_ActiveFL" value="=" /></span></td>
		<td<%= Company.ActiveFL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Company.ActiveFL.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_ActiveFL" id="x_ActiveFL" title="<%= Company.ActiveFL.FldTitle %>" value="1"<%= selwrk %><%= Company.ActiveFL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.UseBreadCrumbURL.FldCaption %></td>
		<td<%= Company.UseBreadCrumbURL.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_UseBreadCrumbURL" id="z_UseBreadCrumbURL" value="=" /></span></td>
		<td<%= Company.UseBreadCrumbURL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Company.UseBreadCrumbURL.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_UseBreadCrumbURL" id="x_UseBreadCrumbURL" title="<%= Company.UseBreadCrumbURL.FldTitle %>" value="1"<%= selwrk %><%= Company.UseBreadCrumbURL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Company.RowAttributes %>>
		<td class="ewTableHeader"><%= Company.SingleSiteGallery.FldCaption %></td>
		<td<%= Company.SingleSiteGallery.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SingleSiteGallery" id="z_SingleSiteGallery" value="=" /></span></td>
		<td<%= Company.SingleSiteGallery.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Company.SingleSiteGallery.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_SingleSiteGallery" id="x_SingleSiteGallery" title="<%= Company.SingleSiteGallery.FldTitle %>" value="1"<%= selwrk %><%= Company.SingleSiteGallery.EditAttributes %> />
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
