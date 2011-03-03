<%@ Page ClassName="image_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="image_edit.aspx.vb" Inherits="image_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_edit = new ew_Page("Image_edit");
// page properties
Image_edit.PageID = "edit"; // page ID
Image_edit.FormID = "fImageedit"; // form ID 
var EW_PAGE_ID = Image_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
Image_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_ImageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(Image.ImageName.FldCaption) %>");
		elm = fobj.elements["x" + infix + "_ImageFileName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, ewLanguage.Phrase("EnterRequiredField") + " - <%= ew_JsEncode2(Image.ImageFileName.FldCaption) %>");
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
	return true;
}
// extend page with Form_CustomValidate function
Image_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Image_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Image.TableCaption %><br /><br />
<a href="<%= Image.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Image_edit.DebugMsg) %>
<% Image_edit.ShowMessage() %>
<form name="fImageedit" id="fImageedit" method="post" onsubmit="this.action=location.pathname;return Image_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="Image" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Image.ImageID.Visible Then ' ImageID %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageID.FldCaption %></td>
		<td<%= Image.ImageID.CellAttributes %>><span id="el_ImageID">
<div<%= Image.ImageID.ViewAttributes %>><%= Image.ImageID.EditValue %></div>
<input type="hidden" name="x_ImageID" id="x_ImageID" value="<%= ew_HTMLEncode(Image.ImageID.CurrentValue) %>" />
</span><%= Image.ImageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageName.Visible Then ' ImageName %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Image.ImageName.CellAttributes %>><span id="el_ImageName">
<input type="text" name="x_ImageName" id="x_ImageName" title="<%= Image.ImageName.FldTitle %>" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
</span><%= Image.ImageName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageFileName.Visible Then ' ImageFileName %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageFileName.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= Image.ImageFileName.CellAttributes %>><span id="el_ImageFileName">
<input type="text" name="x_ImageFileName" id="x_ImageFileName" title="<%= Image.ImageFileName.FldTitle %>" size="30" maxlength="254" value="<%= Image.ImageFileName.EditValue %>"<%= Image.ImageFileName.EditAttributes %> />
</span><%= Image.ImageFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageThumbFileName.Visible Then ' ImageThumbFileName %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageThumbFileName.FldCaption %></td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>><span id="el_ImageThumbFileName">
<input type="text" name="x_ImageThumbFileName" id="x_ImageThumbFileName" title="<%= Image.ImageThumbFileName.FldTitle %>" size="30" maxlength="254" value="<%= Image.ImageThumbFileName.EditValue %>"<%= Image.ImageThumbFileName.EditAttributes %> />
</span><%= Image.ImageThumbFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageDescription.Visible Then ' ImageDescription %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageDescription.FldCaption %></td>
		<td<%= Image.ImageDescription.CellAttributes %>><span id="el_ImageDescription">
<textarea name="x_ImageDescription" id="x_ImageDescription" title="<%= Image.ImageDescription.FldTitle %>" cols="35" rows="4"<%= Image.ImageDescription.EditAttributes %>><%= Image.ImageDescription.EditValue %></textarea>
</span><%= Image.ImageDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageComment.Visible Then ' ImageComment %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageComment.FldCaption %></td>
		<td<%= Image.ImageComment.CellAttributes %>><span id="el_ImageComment">
<textarea name="x_ImageComment" id="x_ImageComment" title="<%= Image.ImageComment.FldTitle %>" cols="35" rows="4"<%= Image.ImageComment.EditAttributes %>><%= Image.ImageComment.EditValue %></textarea>
</span><%= Image.ImageComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageDate.Visible Then ' ImageDate %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ImageDate.FldCaption %></td>
		<td<%= Image.ImageDate.CellAttributes %>><span id="el_ImageDate">
<input type="text" name="x_ImageDate" id="x_ImageDate" title="<%= Image.ImageDate.FldTitle %>" value="<%= Image.ImageDate.EditValue %>"<%= Image.ImageDate.EditAttributes %> />
</span><%= Image.ImageDate.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.Active.Visible Then ' Active %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.Active.FldCaption %></td>
		<td<%= Image.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Image.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" title="<%= Image.Active.FldTitle %>" value="1"<%= selwrk %><%= Image.Active.EditAttributes %> />
</span><%= Image.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ModifiedDT.FldCaption %></td>
		<td<%= Image.ModifiedDT.CellAttributes %>><span id="el_ModifiedDT">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" title="<%= Image.ModifiedDT.FldTitle %>" value="<%= Image.ModifiedDT.EditValue %>"<%= Image.ModifiedDT.EditAttributes %> />
</span><%= Image.ModifiedDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.VersionNo.Visible Then ' VersionNo %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.VersionNo.FldCaption %></td>
		<td<%= Image.VersionNo.CellAttributes %>><span id="el_VersionNo">
<input type="text" name="x_VersionNo" id="x_VersionNo" title="<%= Image.VersionNo.FldTitle %>" size="30" value="<%= Image.VersionNo.EditValue %>"<%= Image.VersionNo.EditAttributes %> />
</span><%= Image.VersionNo.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ContactID.Visible Then ' ContactID %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.ContactID.FldCaption %></td>
		<td<%= Image.ContactID.CellAttributes %>><span id="el_ContactID">
<input type="text" name="x_ContactID" id="x_ContactID" title="<%= Image.ContactID.FldTitle %>" size="30" value="<%= Image.ContactID.EditValue %>"<%= Image.ContactID.EditAttributes %> />
</span><%= Image.ContactID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.CompanyID.FldCaption %></td>
		<td<%= Image.CompanyID.CellAttributes %>><span id="el_CompanyID">
<% If Image.CompanyID.SessionValue <> "" Then %>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ViewValue %></div>
<input type="hidden" id="x_CompanyID" name="x_CompanyID" value="<%= ew_HtmlEncode(Image.CompanyID.CurrentValue) %>">
<% Else %>
<input type="text" name="x_CompanyID" id="x_CompanyID" title="<%= Image.CompanyID.FldTitle %>" size="30" value="<%= Image.CompanyID.EditValue %>"<%= Image.CompanyID.EditAttributes %> />
<% End If %>
</span><%= Image.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.title.Visible Then ' title %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.title.FldCaption %></td>
		<td<%= Image.title.CellAttributes %>><span id="el_title">
<input type="text" name="x_title" id="x_title" title="<%= Image.title.FldTitle %>" size="30" maxlength="255" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
</span><%= Image.title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.medium.Visible Then ' medium %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.medium.FldCaption %></td>
		<td<%= Image.medium.CellAttributes %>><span id="el_medium">
<input type="text" name="x_medium" id="x_medium" title="<%= Image.medium.FldTitle %>" size="30" maxlength="255" value="<%= Image.medium.EditValue %>"<%= Image.medium.EditAttributes %> />
</span><%= Image.medium.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.size.Visible Then ' size %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.size.FldCaption %></td>
		<td<%= Image.size.CellAttributes %>><span id="el_size">
<input type="text" name="x_size" id="x_size" title="<%= Image.size.FldTitle %>" size="30" maxlength="255" value="<%= Image.size.EditValue %>"<%= Image.size.EditAttributes %> />
</span><%= Image.size.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.price.Visible Then ' price %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.price.FldCaption %></td>
		<td<%= Image.price.CellAttributes %>><span id="el_price">
<input type="text" name="x_price" id="x_price" title="<%= Image.price.FldTitle %>" size="30" maxlength="255" value="<%= Image.price.EditValue %>"<%= Image.price.EditAttributes %> />
</span><%= Image.price.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.color.Visible Then ' color %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.color.FldCaption %></td>
		<td<%= Image.color.CellAttributes %>><span id="el_color">
<input type="text" name="x_color" id="x_color" title="<%= Image.color.FldTitle %>" size="30" maxlength="255" value="<%= Image.color.EditValue %>"<%= Image.color.EditAttributes %> />
</span><%= Image.color.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.subject.Visible Then ' subject %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.subject.FldCaption %></td>
		<td<%= Image.subject.CellAttributes %>><span id="el_subject">
<input type="text" name="x_subject" id="x_subject" title="<%= Image.subject.FldTitle %>" size="30" maxlength="255" value="<%= Image.subject.EditValue %>"<%= Image.subject.EditAttributes %> />
</span><%= Image.subject.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.sold.Visible Then ' sold %>
	<tr<%= Image.RowAttributes %>>
		<td class="ewTableHeader"><%= Image.sold.FldCaption %></td>
		<td<%= Image.sold.CellAttributes %>><span id="el_sold">
<%
If ew_SameStr(Image.sold.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_sold" id="x_sold" title="<%= Image.sold.FldTitle %>" value="1"<%= selwrk %><%= Image.sold.EditAttributes %> />
</span><%= Image.sold.CustomMsg %></td>
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
