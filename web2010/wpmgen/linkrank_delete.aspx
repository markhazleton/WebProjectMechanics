<%@ Page ClassName="linkrank_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linkrank_delete.aspx.vb" Inherits="linkrank_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkRank_delete = new ew_Page("LinkRank_delete");
// page properties
LinkRank_delete.PageID = "delete"; // page ID
LinkRank_delete.FormID = "fLinkRankdelete"; // form ID 
var EW_PAGE_ID = LinkRank_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
LinkRank_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkRank_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkRank_delete.ValidateRequired = false; // no JavaScript validation
<% End If %>
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
<%

' Load records for display
Rs = LinkRank_delete.LoadRecordset()
If LinkRank_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	LinkRank_delete.Page_Terminate("linkrank_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkRank.TableCaption %><br /><br />
<a href="<%= LinkRank.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkRank_delete.DebugMsg) %>
<% LinkRank_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="LinkRank" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to LinkRank_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(LinkRank_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= LinkRank.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= LinkRank.ID.FldCaption %></td>
		<td valign="top"><%= LinkRank.LinkID.FldCaption %></td>
		<td valign="top"><%= LinkRank.UserID.FldCaption %></td>
		<td valign="top"><%= LinkRank.RankNum.FldCaption %></td>
		<td valign="top"><%= LinkRank.CateID.FldCaption %></td>
		<td valign="top"><%= LinkRank.Comment.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
LinkRank_delete.lRecCnt = 0
Do While Rs.Read()
	LinkRank_delete.lRecCnt = LinkRank_delete.lRecCnt + 1

	' Set row properties
	LinkRank.CssClass = ""
	LinkRank.CssStyle = ""
	LinkRank.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	LinkRank_delete.LoadRowValues(Rs)

	' Render row
	LinkRank_delete.RenderRow()
%>
	<tr<%= LinkRank.RowAttributes %>>
		<td<%= LinkRank.ID.CellAttributes %>>
<div<%= LinkRank.ID.ViewAttributes %>><%= LinkRank.ID.ListViewValue %></div>
</td>
		<td<%= LinkRank.LinkID.CellAttributes %>>
<div<%= LinkRank.LinkID.ViewAttributes %>><%= LinkRank.LinkID.ListViewValue %></div>
</td>
		<td<%= LinkRank.UserID.CellAttributes %>>
<div<%= LinkRank.UserID.ViewAttributes %>><%= LinkRank.UserID.ListViewValue %></div>
</td>
		<td<%= LinkRank.RankNum.CellAttributes %>>
<div<%= LinkRank.RankNum.ViewAttributes %>><%= LinkRank.RankNum.ListViewValue %></div>
</td>
		<td<%= LinkRank.CateID.CellAttributes %>>
<div<%= LinkRank.CateID.ViewAttributes %>><%= LinkRank.CateID.ListViewValue %></div>
</td>
		<td<%= LinkRank.Comment.CellAttributes %>>
<div<%= LinkRank.Comment.ViewAttributes %>><%= LinkRank.Comment.ListViewValue %></div>
</td>
	</tr>
<%
Loop
Rs.Close()
Rs.Dispose()
%>
	</tbody>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("DeleteBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
