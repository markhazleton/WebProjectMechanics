<%@ Page ClassName="image_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="image_srch.aspx.vb" Inherits="image_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_search = new ew_Page("Image_search");
// page properties
Image_search.PageID = "search"; // page ID
Image_search.FormID = "fImagesearch"; // form ID 
var EW_PAGE_ID = Image_search.PageID; // for backward compatibility
// extend page with validate function for search
Image_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_ImageID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Image.ImageID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_VersionNo"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Image.VersionNo.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_ContactID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Image.ContactID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(Image.CompanyID.FldErrMsg) %>");
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
Image_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Image_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Image.TableCaption %><br /><br />
<a href="<%= Image.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Image_search.DebugMsg) %>
<% Image_search.ShowMessage() %>
<form name="fImagesearch" id="fImagesearch" method="post" onsubmit="this.action=location.pathname;return Image_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="Image" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageID.FldCaption %></td>
		<td<%= Image.ImageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ImageID" id="z_ImageID" value="=" /></span></td>
		<td<%= Image.ImageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageID" id="x_ImageID" title="<%= Image.ImageID.FldTitle %>" value="<%= Image.ImageID.EditValue %>"<%= Image.ImageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageName.FldCaption %></td>
		<td<%= Image.ImageName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ImageName" id="z_ImageName" value="LIKE" /></span></td>
		<td<%= Image.ImageName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageName" id="x_ImageName" title="<%= Image.ImageName.FldTitle %>" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageFileName.FldCaption %></td>
		<td<%= Image.ImageFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ImageFileName" id="z_ImageFileName" value="LIKE" /></span></td>
		<td<%= Image.ImageFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageFileName" id="x_ImageFileName" title="<%= Image.ImageFileName.FldTitle %>" size="30" maxlength="254" value="<%= Image.ImageFileName.EditValue %>"<%= Image.ImageFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageThumbFileName.FldCaption %></td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ImageThumbFileName" id="z_ImageThumbFileName" value="LIKE" /></span></td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageThumbFileName" id="x_ImageThumbFileName" title="<%= Image.ImageThumbFileName.FldTitle %>" size="30" maxlength="254" value="<%= Image.ImageThumbFileName.EditValue %>"<%= Image.ImageThumbFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageDescription.FldCaption %></td>
		<td<%= Image.ImageDescription.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ImageDescription" id="z_ImageDescription" value="LIKE" /></span></td>
		<td<%= Image.ImageDescription.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_ImageDescription" id="x_ImageDescription" title="<%= Image.ImageDescription.FldTitle %>" cols="35" rows="4"<%= Image.ImageDescription.EditAttributes %>><%= Image.ImageDescription.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageComment.FldCaption %></td>
		<td<%= Image.ImageComment.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ImageComment" id="z_ImageComment" value="LIKE" /></span></td>
		<td<%= Image.ImageComment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_ImageComment" id="x_ImageComment" title="<%= Image.ImageComment.FldTitle %>" cols="35" rows="4"<%= Image.ImageComment.EditAttributes %>><%= Image.ImageComment.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageDate.FldCaption %></td>
		<td<%= Image.ImageDate.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ImageDate" id="z_ImageDate" value="=" /></span></td>
		<td<%= Image.ImageDate.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ImageDate" id="x_ImageDate" title="<%= Image.ImageDate.FldTitle %>" value="<%= Image.ImageDate.EditValue %>"<%= Image.ImageDate.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.Active.FldCaption %></td>
		<td<%= Image.Active.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Active" id="z_Active" value="=" /></span></td>
		<td<%= Image.Active.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Image.Active.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= Image.Active.FldTitle %>" value="1"<%= selwrk %><%= Image.Active.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ModifiedDT.FldCaption %></td>
		<td<%= Image.ModifiedDT.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ModifiedDT" id="z_ModifiedDT" value="=" /></span></td>
		<td<%= Image.ModifiedDT.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" title="<%= Image.ModifiedDT.FldTitle %>" value="<%= Image.ModifiedDT.EditValue %>"<%= Image.ModifiedDT.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.VersionNo.FldCaption %></td>
		<td<%= Image.VersionNo.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_VersionNo" id="z_VersionNo" value="=" /></span></td>
		<td<%= Image.VersionNo.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_VersionNo" id="x_VersionNo" title="<%= Image.VersionNo.FldTitle %>" size="30" value="<%= Image.VersionNo.EditValue %>"<%= Image.VersionNo.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ContactID.FldCaption %></td>
		<td<%= Image.ContactID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ContactID" id="z_ContactID" value="=" /></span></td>
		<td<%= Image.ContactID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ContactID" id="x_ContactID" title="<%= Image.ContactID.FldTitle %>" size="30" value="<%= Image.ContactID.EditValue %>"<%= Image.ContactID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.CompanyID.FldCaption %></td>
		<td<%= Image.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= Image.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CompanyID" id="x_CompanyID" title="<%= Image.CompanyID.FldTitle %>" size="30" value="<%= Image.CompanyID.EditValue %>"<%= Image.CompanyID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.title.FldCaption %></td>
		<td<%= Image.title.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_title" id="z_title" value="LIKE" /></span></td>
		<td<%= Image.title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_title" id="x_title" title="<%= Image.title.FldTitle %>" size="30" maxlength="255" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.medium.FldCaption %></td>
		<td<%= Image.medium.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_medium" id="z_medium" value="LIKE" /></span></td>
		<td<%= Image.medium.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_medium" id="x_medium" title="<%= Image.medium.FldTitle %>" size="30" maxlength="255" value="<%= Image.medium.EditValue %>"<%= Image.medium.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.size.FldCaption %></td>
		<td<%= Image.size.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_size" id="z_size" value="LIKE" /></span></td>
		<td<%= Image.size.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_size" id="x_size" title="<%= Image.size.FldTitle %>" size="30" maxlength="255" value="<%= Image.size.EditValue %>"<%= Image.size.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.price.FldCaption %></td>
		<td<%= Image.price.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_price" id="z_price" value="LIKE" /></span></td>
		<td<%= Image.price.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_price" id="x_price" title="<%= Image.price.FldTitle %>" size="30" maxlength="255" value="<%= Image.price.EditValue %>"<%= Image.price.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.color.FldCaption %></td>
		<td<%= Image.color.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_color" id="z_color" value="LIKE" /></span></td>
		<td<%= Image.color.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_color" id="x_color" title="<%= Image.color.FldTitle %>" size="30" maxlength="255" value="<%= Image.color.EditValue %>"<%= Image.color.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.subject.FldCaption %></td>
		<td<%= Image.subject.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_subject" id="z_subject" value="LIKE" /></span></td>
		<td<%= Image.subject.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_subject" id="x_subject" title="<%= Image.subject.FldTitle %>" size="30" maxlength="255" value="<%= Image.subject.EditValue %>"<%= Image.subject.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.sold.FldCaption %></td>
		<td<%= Image.sold.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_sold" id="z_sold" value="=" /></span></td>
		<td<%= Image.sold.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(Image.sold.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_sold" id="x_sold" title="<%= Image.sold.FldTitle %>" value="1"<%= selwrk %><%= Image.sold.EditAttributes %> />
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
