<%@ Page ClassName="pageimage_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pageimage_srch.aspx.vb" Inherits="pageimage_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageImage_search = new ew_Page("PageImage_search");
// page properties
PageImage_search.PageID = "search"; // page ID
PageImage_search.FormID = "fPageImagesearch"; // form ID 
var EW_PAGE_ID = PageImage_search.PageID; // for backward compatibility
// extend page with validate function for search
PageImage_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_PageImagePosition"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageImage.PageImagePosition.FldErrMsg) %>");
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
PageImage_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageImage_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageImage.TableCaption %><br /><br />
<a href="<%= PageImage.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageImage_search.DebugMsg) %>
<% PageImage_search.ShowMessage() %>
<form name="fPageImagesearch" id="fPageImagesearch" method="post" onsubmit="this.action=location.pathname;return PageImage_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageImage" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.zPageID.FldCaption %></td>
		<td<%= PageImage.zPageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= PageImage.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_zPageID" name="x_zPageID" title="<%= PageImage.zPageID.FldTitle %>"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.AdvancedSearch.SearchValue) Then
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
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.ImageID.FldCaption %></td>
		<td<%= PageImage.ImageID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ImageID" id="z_ImageID" value="=" /></span></td>
		<td<%= PageImage.ImageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_ImageID" name="x_ImageID" title="<%= PageImage.ImageID.FldTitle %>"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.AdvancedSearch.SearchValue) Then
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
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.PageImagePosition.FldCaption %></td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_PageImagePosition" id="z_PageImagePosition" value="=" /></span></td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageImagePosition" id="x_PageImagePosition" title="<%= PageImage.PageImagePosition.FldTitle %>" size="30" value="<%= PageImage.PageImagePosition.EditValue %>"<%= PageImage.PageImagePosition.EditAttributes %> />
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
