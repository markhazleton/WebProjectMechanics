<%@ Page ClassName="linkrank_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linkrank_srch.aspx.vb" Inherits="linkrank_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkRank_search = new ew_Page("LinkRank_search");
// page properties
LinkRank_search.PageID = "search"; // page ID
LinkRank_search.FormID = "fLinkRanksearch"; // form ID 
var EW_PAGE_ID = LinkRank_search.PageID; // for backward compatibility
// extend page with validate function for search
LinkRank_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_ID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkRank.ID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_LinkID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkRank.LinkID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_UserID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkRank.UserID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_RankNum"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkRank.RankNum.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_CateID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(LinkRank.CateID.FldErrMsg) %>");
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
LinkRank_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkRank.TableCaption %><br /><br />
<a href="<%= LinkRank.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkRank_search.DebugMsg) %>
<% LinkRank_search.ShowMessage() %>
<form name="fLinkRanksearch" id="fLinkRanksearch" method="post" onsubmit="this.action=location.pathname;return LinkRank_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="LinkRank" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.ID.FldCaption %></td>
		<td<%= LinkRank.ID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ID" id="z_ID" value="=" /></span></td>
		<td<%= LinkRank.ID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ID" id="x_ID" title="<%= LinkRank.ID.FldTitle %>" value="<%= LinkRank.ID.EditValue %>"<%= LinkRank.ID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.LinkID.FldCaption %></td>
		<td<%= LinkRank.LinkID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_LinkID" id="z_LinkID" value="=" /></span></td>
		<td<%= LinkRank.LinkID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_LinkID" id="x_LinkID" title="<%= LinkRank.LinkID.FldTitle %>" size="30" value="<%= LinkRank.LinkID.EditValue %>"<%= LinkRank.LinkID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.UserID.FldCaption %></td>
		<td<%= LinkRank.UserID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_UserID" id="z_UserID" value="=" /></span></td>
		<td<%= LinkRank.UserID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_UserID" id="x_UserID" title="<%= LinkRank.UserID.FldTitle %>" size="30" value="<%= LinkRank.UserID.EditValue %>"<%= LinkRank.UserID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.RankNum.FldCaption %></td>
		<td<%= LinkRank.RankNum.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_RankNum" id="z_RankNum" value="=" /></span></td>
		<td<%= LinkRank.RankNum.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_RankNum" id="x_RankNum" title="<%= LinkRank.RankNum.FldTitle %>" size="30" value="<%= LinkRank.RankNum.EditValue %>"<%= LinkRank.RankNum.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.CateID.FldCaption %></td>
		<td<%= LinkRank.CateID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CateID" id="z_CateID" value="=" /></span></td>
		<td<%= LinkRank.CateID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_CateID" id="x_CateID" title="<%= LinkRank.CateID.FldTitle %>" size="30" value="<%= LinkRank.CateID.EditValue %>"<%= LinkRank.CateID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= LinkRank.RowAttributes %>>
		<td class="ewTableHeader"><%= LinkRank.Comment.FldCaption %></td>
		<td<%= LinkRank.Comment.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Comment" id="z_Comment" value="LIKE" /></span></td>
		<td<%= LinkRank.Comment.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Comment" id="x_Comment" title="<%= LinkRank.Comment.FldTitle %>" size="30" maxlength="255" value="<%= LinkRank.Comment.EditValue %>"<%= LinkRank.Comment.EditAttributes %> />
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
