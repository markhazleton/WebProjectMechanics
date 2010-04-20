<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="LinkRank_srch.aspx.vb" Inherits="LinkRank_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkRank_search = new ew_Page("LinkRank_search");
// page properties
LinkRank_search.PageID = "search"; // page ID
var EW_PAGE_ID = LinkRank_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkRank_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_ID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - ID");
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
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Part Rank<br /><br />
<a href="<%= LinkRank.ReturnUrl %>">Back to List</a></span></p>
<% LinkRank_search.ShowMessage() %>
<form name="fLinkRanksearch" id="fLinkRanksearch" method="post" onsubmit="this.action=location.pathname;return LinkRank_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkRank" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkRank.ID.RowAttributes %>>
		<td class="ewTableHeader">ID</td>
		<td<%= LinkRank.ID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_ID" id="z_ID" value="=" /></span></td>
		<td<%= LinkRank.ID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ID" id="x_ID" value="<%= LinkRank.ID.EditValue %>"<%= LinkRank.ID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.LinkID.RowAttributes %>>
		<td class="ewTableHeader">Link ID</td>
		<td<%= LinkRank.LinkID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_LinkID" id="z_LinkID" value="=" /></span></td>
		<td<%= LinkRank.LinkID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkID" id="x_LinkID" size="30" value="<%= LinkRank.LinkID.EditValue %>"<%= LinkRank.LinkID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.UserID.RowAttributes %>>
		<td class="ewTableHeader">User ID</td>
		<td<%= LinkRank.UserID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_UserID" id="z_UserID" value="=" /></span></td>
		<td<%= LinkRank.UserID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_UserID" id="x_UserID" size="30" value="<%= LinkRank.UserID.EditValue %>"<%= LinkRank.UserID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RankNum.RowAttributes %>>
		<td class="ewTableHeader">Rank Num</td>
		<td<%= LinkRank.RankNum.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_RankNum" id="z_RankNum" value="=" /></span></td>
		<td<%= LinkRank.RankNum.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RankNum" id="x_RankNum" size="30" value="<%= LinkRank.RankNum.EditValue %>"<%= LinkRank.RankNum.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.CateID.RowAttributes %>>
		<td class="ewTableHeader">Cate ID</td>
		<td<%= LinkRank.CateID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_CateID" id="z_CateID" value="=" /></span></td>
		<td<%= LinkRank.CateID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CateID" id="x_CateID" size="30" value="<%= LinkRank.CateID.EditValue %>"<%= LinkRank.CateID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.Comment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= LinkRank.Comment.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_Comment" id="z_Comment" value="LIKE" /></span></td>
		<td<%= LinkRank.Comment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Comment" id="x_Comment" size="30" maxlength="255" value="<%= LinkRank.Comment.EditValue %>"<%= LinkRank.Comment.EditAttributes %> />
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
