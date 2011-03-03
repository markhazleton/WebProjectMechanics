<%@ Page ClassName="sitecategory_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategory_srch.aspx.vb" Inherits="sitecategory_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_search = new ew_Page("SiteCategory_search");
// page properties
SiteCategory_search.PageID = "search"; // page ID
SiteCategory_search.FormID = "fSiteCategorysearch"; // form ID 
var EW_PAGE_ID = SiteCategory_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteCategory_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_SiteCategoryID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategory.SiteCategoryID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_GroupOrder"];
		if (elm && elm.type != "hidden" && !ew_CheckNumber(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategory.GroupOrder.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategory.SiteCategoryTypeID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteCategory.SiteCategoryGroupID.FldErrMsg) %>");
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
SiteCategory_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategory.TableCaption %><br /><br />
<a href="<%= SiteCategory.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategory_search.DebugMsg) %>
<% SiteCategory_search.ShowMessage() %>
<form name="fSiteCategorysearch" id="fSiteCategorysearch" method="post" onsubmit="this.action=location.pathname;return SiteCategory_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.SiteCategoryID.FldCaption %></td>
		<td<%= SiteCategory.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= SiteCategory.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryID" id="x_SiteCategoryID" title="<%= SiteCategory.SiteCategoryID.FldTitle %>" value="<%= SiteCategory.SiteCategoryID.EditValue %>"<%= SiteCategory.SiteCategoryID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryKeywords.FldCaption %></td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CategoryKeywords" id="z_CategoryKeywords" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryKeywords" id="x_CategoryKeywords" title="<%= SiteCategory.CategoryKeywords.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryKeywords.EditValue %>"<%= SiteCategory.CategoryKeywords.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryName.FldCaption %></td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CategoryName" id="z_CategoryName" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryName" id="x_CategoryName" title="<%= SiteCategory.CategoryName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryTitle.FldCaption %></td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CategoryTitle" id="z_CategoryTitle" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryTitle" id="x_CategoryTitle" title="<%= SiteCategory.CategoryTitle.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryTitle.EditValue %>"<%= SiteCategory.CategoryTitle.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryDescription.FldCaption %></td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CategoryDescription" id="z_CategoryDescription" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryDescription" id="x_CategoryDescription" title="<%= SiteCategory.CategoryDescription.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryDescription.EditValue %>"<%= SiteCategory.CategoryDescription.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.GroupOrder.FldCaption %></td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_GroupOrder" id="z_GroupOrder" value="=" /></span></td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_GroupOrder" id="x_GroupOrder" title="<%= SiteCategory.GroupOrder.FldTitle %>" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.ParentCategoryID.FldCaption %></td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ParentCategoryID" id="z_ParentCategoryID" value="LIKE" /></span></td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ParentCategoryID" id="x_ParentCategoryID" title="<%= SiteCategory.ParentCategoryID.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.ParentCategoryID.EditValue %>"<%= SiteCategory.ParentCategoryID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryFileName.FldCaption %></td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_CategoryFileName" id="z_CategoryFileName" value="LIKE" /></span></td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CategoryFileName" id="x_CategoryFileName" title="<%= SiteCategory.CategoryFileName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" title="<%= SiteCategory.SiteCategoryTypeID.FldTitle %>" size="30" value="<%= SiteCategory.SiteCategoryTypeID.EditValue %>"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= SiteCategory.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= SiteCategory.SiteCategoryGroupID.EditValue %>"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %> />
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
