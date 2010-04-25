<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="Image_add.aspx.vb" Inherits="Image_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_add = new ew_Page("Image_add");
// page properties
Image_add.PageID = "add"; // page ID
var EW_PAGE_ID = Image_add.PageID; // for backward compatibility
// extend page with ValidateForm function
Image_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_CompanyID"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Company");
		elm = fobj.elements["x" + infix + "_ImageName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - Name");
		elm = fobj.elements["x" + infix + "_ImageFileName"];
		if (elm && !ew_HasValue(elm))
			return ew_OnError(this, elm, "Please enter required field - File Name");
		elm = fobj.elements["x" + infix + "_ImageDate"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Created");
		elm = fobj.elements["x" + infix + "_ModifiedDT"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Modified");
		elm = fobj.elements["x" + infix + "_VersionNo"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Version No");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
Image_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Site Image<br /><br />
<a href="<%= Image.ReturnUrl %>">Go Back</a></span></p>
<% Image_add.ShowMessage() %>
<form name="fImageadd" id="fImageadd" method="post" onsubmit="this.action=location.pathname;return Image_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="Image" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If Image.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Image.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Image.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID"<%= Image.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Image.CompanyID.EditValue) Then
	arwrk = Image.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Image.CompanyID.CurrentValue) Then
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
</span><%= Image.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.title.Visible Then ' title %>
	<tr<%= Image.title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Image.title.CellAttributes %>><span id="el_title">
<input type="text" name="x_title" id="x_title" size="30" maxlength="50" value="<%= Image.title.EditValue %>"<%= Image.title.EditAttributes %> />
</span><%= Image.title.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageName.Visible Then ' ImageName %>
	<tr<%= Image.ImageName.RowAttributes %>>
		<td class="ewTableHeader">Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Image.ImageName.CellAttributes %>><span id="el_ImageName">
<input type="text" name="x_ImageName" id="x_ImageName" size="30" maxlength="50" value="<%= Image.ImageName.EditValue %>"<%= Image.ImageName.EditAttributes %> />
</span><%= Image.ImageName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageDescription.Visible Then ' ImageDescription %>
	<tr<%= Image.ImageDescription.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Image.ImageDescription.CellAttributes %>><span id="el_ImageDescription">
<textarea name="x_ImageDescription" id="x_ImageDescription" cols="35" rows="4"<%= Image.ImageDescription.EditAttributes %>><%= Image.ImageDescription.EditValue %></textarea>
</span><%= Image.ImageDescription.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageComment.Visible Then ' ImageComment %>
	<tr<%= Image.ImageComment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= Image.ImageComment.CellAttributes %>><span id="el_ImageComment">
<textarea name="x_ImageComment" id="x_ImageComment" cols="35" rows="4"<%= Image.ImageComment.EditAttributes %>><%= Image.ImageComment.EditValue %></textarea>
</span><%= Image.ImageComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageFileName.Visible Then ' ImageFileName %>
	<tr<%= Image.ImageFileName.RowAttributes %>>
		<td class="ewTableHeader">File Name<span class="ewRequired">&nbsp;*</span></td>
		<td<%= Image.ImageFileName.CellAttributes %>><span id="el_ImageFileName">
<input type="text" name="x_ImageFileName" id="x_ImageFileName" size="30" maxlength="254" value="<%= Image.ImageFileName.EditValue %>"<%= Image.ImageFileName.EditAttributes %> />
</span><%= Image.ImageFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageThumbFileName.Visible Then ' ImageThumbFileName %>
	<tr<%= Image.ImageThumbFileName.RowAttributes %>>
		<td class="ewTableHeader">Thumb File Name</td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>><span id="el_ImageThumbFileName">
<input type="text" name="x_ImageThumbFileName" id="x_ImageThumbFileName" size="30" maxlength="254" value="<%= Image.ImageThumbFileName.EditValue %>"<%= Image.ImageThumbFileName.EditAttributes %> />
</span><%= Image.ImageThumbFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ImageDate.Visible Then ' ImageDate %>
	<tr<%= Image.ImageDate.RowAttributes %>>
		<td class="ewTableHeader">Created</td>
		<td<%= Image.ImageDate.CellAttributes %>><span id="el_ImageDate">
<input type="text" name="x_ImageDate" id="x_ImageDate" value="<%= Image.ImageDate.EditValue %>"<%= Image.ImageDate.EditAttributes %> />
</span><%= Image.ImageDate.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.Active.Visible Then ' Active %>
	<tr<%= Image.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Image.Active.CellAttributes %>><span id="el_Active">
<%
If ew_SameStr(Image.Active.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Active" id="x_Active" value="1"<%= selwrk %><%= Image.Active.EditAttributes %> />
</span><%= Image.Active.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= Image.ModifiedDT.RowAttributes %>>
		<td class="ewTableHeader">Modified</td>
		<td<%= Image.ModifiedDT.CellAttributes %>><span id="el_ModifiedDT">
<input type="text" name="x_ModifiedDT" id="x_ModifiedDT" value="<%= Image.ModifiedDT.EditValue %>"<%= Image.ModifiedDT.EditAttributes %> />
</span><%= Image.ModifiedDT.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.VersionNo.Visible Then ' VersionNo %>
	<tr<%= Image.VersionNo.RowAttributes %>>
		<td class="ewTableHeader">Version No</td>
		<td<%= Image.VersionNo.CellAttributes %>><span id="el_VersionNo">
<input type="text" name="x_VersionNo" id="x_VersionNo" size="30" value="<%= Image.VersionNo.EditValue %>"<%= Image.VersionNo.EditAttributes %> />
</span><%= Image.VersionNo.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.ContactID.Visible Then ' ContactID %>
	<tr<%= Image.ContactID.RowAttributes %>>
		<td class="ewTableHeader">UserName</td>
		<td<%= Image.ContactID.CellAttributes %>><span id="el_ContactID">
<select id="x_ContactID" name="x_ContactID"<%= Image.ContactID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(Image.ContactID.EditValue) Then
	arwrk = Image.ContactID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), Image.ContactID.CurrentValue) Then
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
</span><%= Image.ContactID.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.medium.Visible Then ' medium %>
	<tr<%= Image.medium.RowAttributes %>>
		<td class="ewTableHeader">Medium</td>
		<td<%= Image.medium.CellAttributes %>><span id="el_medium">
<input type="text" name="x_medium" id="x_medium" size="30" maxlength="50" value="<%= Image.medium.EditValue %>"<%= Image.medium.EditAttributes %> />
</span><%= Image.medium.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.size.Visible Then ' size %>
	<tr<%= Image.size.RowAttributes %>>
		<td class="ewTableHeader">Size</td>
		<td<%= Image.size.CellAttributes %>><span id="el_size">
<input type="text" name="x_size" id="x_size" size="30" maxlength="50" value="<%= Image.size.EditValue %>"<%= Image.size.EditAttributes %> />
</span><%= Image.size.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.price.Visible Then ' price %>
	<tr<%= Image.price.RowAttributes %>>
		<td class="ewTableHeader">Price</td>
		<td<%= Image.price.CellAttributes %>><span id="el_price">
<input type="text" name="x_price" id="x_price" size="30" maxlength="255" value="<%= Image.price.EditValue %>"<%= Image.price.EditAttributes %> />
</span><%= Image.price.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.color.Visible Then ' color %>
	<tr<%= Image.color.RowAttributes %>>
		<td class="ewTableHeader">Color</td>
		<td<%= Image.color.CellAttributes %>><span id="el_color">
<input type="text" name="x_color" id="x_color" size="30" maxlength="255" value="<%= Image.color.EditValue %>"<%= Image.color.EditAttributes %> />
</span><%= Image.color.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.subject.Visible Then ' subject %>
	<tr<%= Image.subject.RowAttributes %>>
		<td class="ewTableHeader">Subject</td>
		<td<%= Image.subject.CellAttributes %>><span id="el_subject">
<input type="text" name="x_subject" id="x_subject" size="30" maxlength="255" value="<%= Image.subject.EditValue %>"<%= Image.subject.EditAttributes %> />
</span><%= Image.subject.CustomMsg %></td>
	</tr>
<% End If %>
<% If Image.sold.Visible Then ' sold %>
	<tr<%= Image.sold.RowAttributes %>>
		<td class="ewTableHeader">Sold</td>
		<td<%= Image.sold.CellAttributes %>><span id="el_sold">
<%
If ew_SameStr(Image.sold.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_sold" id="x_sold" value="1"<%= selwrk %><%= Image.sold.EditAttributes %> />
</span><%= Image.sold.CustomMsg %></td>
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
