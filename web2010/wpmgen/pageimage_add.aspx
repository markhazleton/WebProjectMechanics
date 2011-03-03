<%@ Page ClassName="pageimage_add" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="pageimage_add.aspx.vb" Inherits="pageimage_add" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageImage_add = new ew_Page("PageImage_add");
// page properties
PageImage_add.PageID = "add"; // page ID
PageImage_add.FormID = "fPageImageadd"; // form ID 
var EW_PAGE_ID = PageImage_add.PageID; // for backward compatibility
// extend page with ValidateForm function
PageImage_add.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_PageImagePosition"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(PageImage.PageImagePosition.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	}
	return true;
}
// extend page with Form_CustomValidate function
PageImage_add.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageImage_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Add") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageImage.TableCaption %><br /><br />
<a href="<%= PageImage.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageImage_add.DebugMsg) %>
<% PageImage_add.ShowMessage() %>
<form name="fPageImageadd" id="fPageImageadd" method="post" onsubmit="this.action=location.pathname;return PageImage_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="PageImage" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If PageImage.zPageID.Visible Then ' PageID %>
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.zPageID.FldCaption %></td>
		<td<%= PageImage.zPageID.CellAttributes %>><span id="el_zPageID">
<% If PageImage.zPageID.SessionValue <> "" Then %>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ViewValue %></div>
<input type="hidden" id="x_zPageID" name="x_zPageID" value="<%= ew_HtmlEncode(PageImage.zPageID.CurrentValue) %>">
<% Else %>
<select id="x_zPageID" name="x_zPageID" title="<%= PageImage.zPageID.FldTitle %>"<%= PageImage.zPageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.zPageID.EditValue) Then
	arwrk = PageImage.zPageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.zPageID.CurrentValue) Then
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
</span><%= PageImage.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageImage.ImageID.Visible Then ' ImageID %>
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.ImageID.FldCaption %></td>
		<td<%= PageImage.ImageID.CellAttributes %>><span id="el_ImageID">
<% If PageImage.ImageID.SessionValue <> "" Then %>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ViewValue %></div>
<input type="hidden" id="x_ImageID" name="x_ImageID" value="<%= ew_HtmlEncode(PageImage.ImageID.CurrentValue) %>">
<% Else %>
<select id="x_ImageID" name="x_ImageID" title="<%= PageImage.ImageID.FldTitle %>"<%= PageImage.ImageID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(PageImage.ImageID.EditValue) Then
	arwrk = PageImage.ImageID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), PageImage.ImageID.CurrentValue) Then
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
</span><%= PageImage.ImageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
	<tr<%= PageImage.RowAttributes %>>
		<td class="ewTableHeader"><%= PageImage.PageImagePosition.FldCaption %></td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>><span id="el_PageImagePosition">
<input type="text" name="x_PageImagePosition" id="x_PageImagePosition" title="<%= PageImage.PageImagePosition.FldTitle %>" size="30" value="<%= PageImage.PageImagePosition.EditValue %>"<%= PageImage.PageImagePosition.EditAttributes %> />
</span><%= PageImage.PageImagePosition.CustomMsg %></td>
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
