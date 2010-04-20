<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="LinkRank_edit.aspx.vb" Inherits="LinkRank_edit" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkRank_edit = new ew_Page("LinkRank_edit");
// page properties
LinkRank_edit.PageID = "edit"; // page ID
var EW_PAGE_ID = LinkRank_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
LinkRank_edit.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
		elm = fobj.elements["x" + infix + "_LinkID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Link ID");
		elm = fobj.elements["x" + infix + "_UserID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - User ID");
		elm = fobj.elements["x" + infix + "_RankNum"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Rank Num");
		elm = fobj.elements["x" + infix + "_CateID"];
		if (elm && !ew_CheckInteger(elm.value))
			return ew_OnError(this, elm, "Incorrect integer - Cate ID");
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Edit TABLE: Part Rank<br /><br />
<a href="<%= LinkRank.ReturnUrl %>">Go Back</a></span></p>
<% LinkRank_edit.ShowMessage() %>
<form name="fLinkRankedit" id="fLinkRankedit" method="post" onsubmit="this.action=location.pathname;return LinkRank_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="LinkRank" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If LinkRank.ID.Visible Then ' ID %>
	<tr<%= LinkRank.ID.RowAttributes %>>
		<td class="ewTableHeader">ID</td>
		<td<%= LinkRank.ID.CellAttributes %>><span id="el_ID">
<div<%= LinkRank.ID.ViewAttributes %>><%= LinkRank.ID.EditValue %></div>
<input type="hidden" name="x_ID" id="x_ID" value="<%= ew_HTMLEncode(LinkRank.ID.CurrentValue) %>" />
</span><%= LinkRank.ID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkRank.LinkID.Visible Then ' LinkID %>
	<tr<%= LinkRank.LinkID.RowAttributes %>>
		<td class="ewTableHeader">Link ID</td>
		<td<%= LinkRank.LinkID.CellAttributes %>><span id="el_LinkID">
<input type="text" name="x_LinkID" id="x_LinkID" size="30" value="<%= LinkRank.LinkID.EditValue %>"<%= LinkRank.LinkID.EditAttributes %> />
</span><%= LinkRank.LinkID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkRank.UserID.Visible Then ' UserID %>
	<tr<%= LinkRank.UserID.RowAttributes %>>
		<td class="ewTableHeader">User ID</td>
		<td<%= LinkRank.UserID.CellAttributes %>><span id="el_UserID">
<input type="text" name="x_UserID" id="x_UserID" size="30" value="<%= LinkRank.UserID.EditValue %>"<%= LinkRank.UserID.EditAttributes %> />
</span><%= LinkRank.UserID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkRank.RankNum.Visible Then ' RankNum %>
	<tr<%= LinkRank.RankNum.RowAttributes %>>
		<td class="ewTableHeader">Rank Num</td>
		<td<%= LinkRank.RankNum.CellAttributes %>><span id="el_RankNum">
<input type="text" name="x_RankNum" id="x_RankNum" size="30" value="<%= LinkRank.RankNum.EditValue %>"<%= LinkRank.RankNum.EditAttributes %> />
</span><%= LinkRank.RankNum.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkRank.CateID.Visible Then ' CateID %>
	<tr<%= LinkRank.CateID.RowAttributes %>>
		<td class="ewTableHeader">Cate ID</td>
		<td<%= LinkRank.CateID.CellAttributes %>><span id="el_CateID">
<input type="text" name="x_CateID" id="x_CateID" size="30" value="<%= LinkRank.CateID.EditValue %>"<%= LinkRank.CateID.EditAttributes %> />
</span><%= LinkRank.CateID.CustomMsg %></td>
	</tr>
<% End If %>
<% If LinkRank.Comment.Visible Then ' Comment %>
	<tr<%= LinkRank.Comment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= LinkRank.Comment.CellAttributes %>><span id="el_Comment">
<input type="text" name="x_Comment" id="x_Comment" size="30" maxlength="255" value="<%= LinkRank.Comment.EditValue %>"<%= LinkRank.Comment.EditAttributes %> />
</span><%= LinkRank.Comment.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="Save Changes" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
