<%@ Page ClassName="sitecategory_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitecategory_add.aspx.vb" Inherits="sitecategory_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_add = new ew_Page("SiteCategory_add");
// page properties
SiteCategory_add.PageID = "add"; // page ID
SiteCategory_add.FormID = "fSiteCategoryadd"; // form ID 
var EW_PAGE_ID = SiteCategory_add.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategory_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
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
	return true;
}
// extend page with Form_CustomValidate function
SiteCategory_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategory.TableCaption %><br /><br />
<a href="<%= SiteCategory.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategory_add.DebugMsg) %>
<% SiteCategory_add.ShowMessage() %>
<form name="fSiteCategoryadd" id="fSiteCategoryadd" method="post" onsubmit="this.action=location.pathname;return SiteCategory_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategory.CategoryKeywords.Visible Then ' CategoryKeywords %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryKeywords.FldCaption %></td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>><span id="el_CategoryKeywords">
<input type="text" name="x_CategoryKeywords" id="x_CategoryKeywords" title="<%= SiteCategory.CategoryKeywords.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryKeywords.EditValue %>"<%= SiteCategory.CategoryKeywords.EditAttributes %> />
</span><%= SiteCategory.CategoryKeywords.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryName.Visible Then ' CategoryName %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryName.FldCaption %></td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>><span id="el_CategoryName">
<input type="text" name="x_CategoryName" id="x_CategoryName" title="<%= SiteCategory.CategoryName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryName.EditValue %>"<%= SiteCategory.CategoryName.EditAttributes %> />
</span><%= SiteCategory.CategoryName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryTitle.Visible Then ' CategoryTitle %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryTitle.FldCaption %></td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>><span id="el_CategoryTitle">
<input type="text" name="x_CategoryTitle" id="x_CategoryTitle" title="<%= SiteCategory.CategoryTitle.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryTitle.EditValue %>"<%= SiteCategory.CategoryTitle.EditAttributes %> />
</span><%= SiteCategory.CategoryTitle.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryDescription.Visible Then ' CategoryDescription %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryDescription.FldCaption %></td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>><span id="el_CategoryDescription">
<input type="text" name="x_CategoryDescription" id="x_CategoryDescription" title="<%= SiteCategory.CategoryDescription.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryDescription.EditValue %>"<%= SiteCategory.CategoryDescription.EditAttributes %> />
</span><%= SiteCategory.CategoryDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.GroupOrder.Visible Then ' GroupOrder %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.GroupOrder.FldCaption %></td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>><span id="el_GroupOrder">
<input type="text" name="x_GroupOrder" id="x_GroupOrder" title="<%= SiteCategory.GroupOrder.FldTitle %>" size="30" value="<%= SiteCategory.GroupOrder.EditValue %>"<%= SiteCategory.GroupOrder.EditAttributes %> />
</span><%= SiteCategory.GroupOrder.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.ParentCategoryID.Visible Then ' ParentCategoryID %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.ParentCategoryID.FldCaption %></td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>><span id="el_ParentCategoryID">
<input type="text" name="x_ParentCategoryID" id="x_ParentCategoryID" title="<%= SiteCategory.ParentCategoryID.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.ParentCategoryID.EditValue %>"<%= SiteCategory.ParentCategoryID.EditAttributes %> />
</span><%= SiteCategory.ParentCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.CategoryFileName.Visible Then ' CategoryFileName %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.CategoryFileName.FldCaption %></td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>><span id="el_CategoryFileName">
<input type="text" name="x_CategoryFileName" id="x_CategoryFileName" title="<%= SiteCategory.CategoryFileName.FldTitle %>" size="30" maxlength="255" value="<%= SiteCategory.CategoryFileName.EditValue %>"<%= SiteCategory.CategoryFileName.EditAttributes %> />
</span><%= SiteCategory.CategoryFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<input type="text" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" title="<%= SiteCategory.SiteCategoryTypeID.FldTitle %>" size="30" value="<%= SiteCategory.SiteCategoryTypeID.EditValue %>"<%= SiteCategory.SiteCategoryTypeID.EditAttributes %> />
</span><%= SiteCategory.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategory.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteCategory.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteCategory.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= SiteCategory.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= SiteCategory.SiteCategoryGroupID.EditValue %>"<%= SiteCategory.SiteCategoryGroupID.EditAttributes %> />
</span><%= SiteCategory.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("AddBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
