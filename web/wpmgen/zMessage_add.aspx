<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="zMessage_add.aspx.vb" Inherits="zMessage_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zMessage_add = new ew_Page("zMessage_add");
// page properties
zMessage_add.PageID = "add"; // page ID
var EW_PAGE_ID = zMessage_add.PageID; // for backward compatibility
// extend page with ValidateForm function
zMessage_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_zPageID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Page ID");
		elm = fobj.elements["x" + infix + "_ParentMessageID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Parent Message ID");
		elm = fobj.elements["x" + infix + "_MessageDate"];
		if (elm && !ew_CheckUSDate(elm.value))
			return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Message Date");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
zMessage_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zMessage_add.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Add to TABLE: Message<br /><br />
<a href="<%= zMessage.ReturnUrl %>">Go Back</a></span></p>
<% zMessage_add.ShowMessage() %>
<form name="fzMessageadd" id="fzMessageadd" method="post" onsubmit="this.action=location.pathname;return zMessage_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="zMessage" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If zMessage.zPageID.Visible Then ' PageID %>
	<tr<%= zMessage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= zMessage.zPageID.CellAttributes %>><span id="el_zPageID">
<input type="text" name="x_zPageID" id="x_zPageID" size="30" value="<%= zMessage.zPageID.EditValue %>"<%= zMessage.zPageID.EditAttributes %> />
</span><%= zMessage.zPageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.ParentMessageID.Visible Then ' ParentMessageID %>
	<tr<%= zMessage.ParentMessageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Message ID</td>
		<td<%= zMessage.ParentMessageID.CellAttributes %>><span id="el_ParentMessageID">
<input type="text" name="x_ParentMessageID" id="x_ParentMessageID" size="30" value="<%= zMessage.ParentMessageID.EditValue %>"<%= zMessage.ParentMessageID.EditAttributes %> />
</span><%= zMessage.ParentMessageID.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.Subject.Visible Then ' Subject %>
	<tr<%= zMessage.Subject.RowAttributes %>>
		<td class="ewTableHeader">Subject</td>
		<td<%= zMessage.Subject.CellAttributes %>><span id="el_Subject">
<input type="text" name="x_Subject" id="x_Subject" size="30" maxlength="255" value="<%= zMessage.Subject.EditValue %>"<%= zMessage.Subject.EditAttributes %> />
</span><%= zMessage.Subject.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.Author.Visible Then ' Author %>
	<tr<%= zMessage.Author.RowAttributes %>>
		<td class="ewTableHeader">Author</td>
		<td<%= zMessage.Author.CellAttributes %>><span id="el_Author">
<input type="text" name="x_Author" id="x_Author" size="30" maxlength="50" value="<%= zMessage.Author.EditValue %>"<%= zMessage.Author.EditAttributes %> />
</span><%= zMessage.Author.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.zEmail.Visible Then ' Email %>
	<tr<%= zMessage.zEmail.RowAttributes %>>
		<td class="ewTableHeader">Email</td>
		<td<%= zMessage.zEmail.CellAttributes %>><span id="el_zEmail">
<input type="text" name="x_zEmail" id="x_zEmail" size="30" maxlength="100" value="<%= zMessage.zEmail.EditValue %>"<%= zMessage.zEmail.EditAttributes %> />
</span><%= zMessage.zEmail.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.City.Visible Then ' City %>
	<tr<%= zMessage.City.RowAttributes %>>
		<td class="ewTableHeader">City</td>
		<td<%= zMessage.City.CellAttributes %>><span id="el_City">
<input type="text" name="x_City" id="x_City" size="30" maxlength="50" value="<%= zMessage.City.EditValue %>"<%= zMessage.City.EditAttributes %> />
</span><%= zMessage.City.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.URL.Visible Then ' URL %>
	<tr<%= zMessage.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= zMessage.URL.CellAttributes %>><span id="el_URL">
<input type="text" name="x_URL" id="x_URL" size="30" maxlength="50" value="<%= zMessage.URL.EditValue %>"<%= zMessage.URL.EditAttributes %> />
</span><%= zMessage.URL.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.MessageDate.Visible Then ' MessageDate %>
	<tr<%= zMessage.MessageDate.RowAttributes %>>
		<td class="ewTableHeader">Message Date</td>
		<td<%= zMessage.MessageDate.CellAttributes %>><span id="el_MessageDate">
<input type="text" name="x_MessageDate" id="x_MessageDate" value="<%= zMessage.MessageDate.EditValue %>"<%= zMessage.MessageDate.EditAttributes %> />
</span><%= zMessage.MessageDate.CustomMsg %></td>
	</tr>
<% End If %>
<% If zMessage.Body.Visible Then ' Body %>
	<tr<%= zMessage.Body.RowAttributes %>>
		<td class="ewTableHeader">Body</td>
		<td<%= zMessage.Body.CellAttributes %>><span id="el_Body">
<textarea name="x_Body" id="x_Body" cols="35" rows="4"<%= zMessage.Body.EditAttributes %>><%= zMessage.Body.EditValue %></textarea>
</span><%= zMessage.Body.CustomMsg %></td>
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
