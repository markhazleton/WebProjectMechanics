<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zMessage_srch.aspx.vb" Inherits="zMessage_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zMessage_search = new ew_Page("zMessage_search");
// page properties
zMessage_search.PageID = "search"; // page ID
var EW_PAGE_ID = zMessage_search.PageID; // for backward compatibility
// extend page with validate function for search
zMessage_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_MessageID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Message ID");
	elm = fobj.elements["x" + infix + "_zPageID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Page ID");
	elm = fobj.elements["x" + infix + "_ParentMessageID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Parent Message ID");
	elm = fobj.elements["x" + infix + "_MessageDate"];
	if (elm && !ew_CheckUSDate(elm.value))
		return ew_OnError(this, elm, "Incorrect date, format = mm/dd/yyyy - Message Date");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
zMessage_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zMessage_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Message<br /><br />
<a href="<%= zMessage.ReturnUrl %>">Back to List</a></span></p>
<% zMessage_search.ShowMessage() %>
<form name="fzMessagesearch" id="fzMessagesearch" method="post" onsubmit="this.action=location.pathname;return zMessage_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="zMessage" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= zMessage.MessageID.RowAttributes %>>
		<td class="ewTableHeader">Message ID</td>
		<td<%= zMessage.MessageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_MessageID" id="z_MessageID" value="=" /></span></td>
		<td<%= zMessage.MessageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_MessageID" id="x_MessageID" value="<%= zMessage.MessageID.EditValue %>"<%= zMessage.MessageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= zMessage.zPageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_zPageID" id="z_zPageID" value="=" /></span></td>
		<td<%= zMessage.zPageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zPageID" id="x_zPageID" size="30" value="<%= zMessage.zPageID.EditValue %>"<%= zMessage.zPageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.ParentMessageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Message ID</td>
		<td<%= zMessage.ParentMessageID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ParentMessageID" id="z_ParentMessageID" value="=" /></span></td>
		<td<%= zMessage.ParentMessageID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ParentMessageID" id="x_ParentMessageID" size="30" value="<%= zMessage.ParentMessageID.EditValue %>"<%= zMessage.ParentMessageID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.Subject.RowAttributes %>>
		<td class="ewTableHeader">Subject</td>
		<td<%= zMessage.Subject.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Subject" id="z_Subject" value="LIKE" /></span></td>
		<td<%= zMessage.Subject.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Subject" id="x_Subject" size="30" maxlength="255" value="<%= zMessage.Subject.EditValue %>"<%= zMessage.Subject.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.Author.RowAttributes %>>
		<td class="ewTableHeader">Author</td>
		<td<%= zMessage.Author.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Author" id="z_Author" value="LIKE" /></span></td>
		<td<%= zMessage.Author.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Author" id="x_Author" size="30" maxlength="50" value="<%= zMessage.Author.EditValue %>"<%= zMessage.Author.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.zEmail.RowAttributes %>>
		<td class="ewTableHeader">Email</td>
		<td<%= zMessage.zEmail.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_zEmail" id="z_zEmail" value="LIKE" /></span></td>
		<td<%= zMessage.zEmail.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_zEmail" id="x_zEmail" size="30" maxlength="100" value="<%= zMessage.zEmail.EditValue %>"<%= zMessage.zEmail.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.City.RowAttributes %>>
		<td class="ewTableHeader">City</td>
		<td<%= zMessage.City.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_City" id="z_City" value="LIKE" /></span></td>
		<td<%= zMessage.City.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_City" id="x_City" size="30" maxlength="50" value="<%= zMessage.City.EditValue %>"<%= zMessage.City.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= zMessage.URL.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_URL" id="z_URL" value="LIKE" /></span></td>
		<td<%= zMessage.URL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_URL" id="x_URL" size="30" maxlength="50" value="<%= zMessage.URL.EditValue %>"<%= zMessage.URL.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.MessageDate.RowAttributes %>>
		<td class="ewTableHeader">Message Date</td>
		<td<%= zMessage.MessageDate.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_MessageDate" id="z_MessageDate" value="=" /></span></td>
		<td<%= zMessage.MessageDate.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_MessageDate" id="x_MessageDate" value="<%= zMessage.MessageDate.EditValue %>"<%= zMessage.MessageDate.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= zMessage.Body.RowAttributes %>>
		<td class="ewTableHeader">Body</td>
		<td<%= zMessage.Body.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Body" id="z_Body" value="LIKE" /></span></td>
		<td<%= zMessage.Body.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Body" id="x_Body" cols="35" rows="4"<%= zMessage.Body.EditAttributes %>><%= zMessage.Body.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
