<%@ Page ClassName="linkcategory_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linkcategory_delete.aspx.vb" Inherits="linkcategory_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkCategory_delete = new ew_Page("LinkCategory_delete");
// page properties
LinkCategory_delete.PageID = "delete"; // page ID
LinkCategory_delete.FormID = "fLinkCategorydelete"; // form ID 
var EW_PAGE_ID = LinkCategory_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
LinkCategory_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkCategory_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkCategory_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = LinkCategory_delete.LoadRecordset()
If LinkCategory_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	LinkCategory_delete.Page_Terminate("linkcategory_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkCategory.TableCaption %><br /><br />
<a href="<%= LinkCategory.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkCategory_delete.DebugMsg) %>
<% LinkCategory_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="LinkCategory" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to LinkCategory_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(LinkCategory_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= LinkCategory.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= LinkCategory.ID.FldCaption %></td>
		<td valign="top"><%= LinkCategory.Title.FldCaption %></td>
		<td valign="top"><%= LinkCategory.ParentID.FldCaption %></td>
		<td valign="top"><%= LinkCategory.zPageID.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
LinkCategory_delete.lRecCnt = 0
Do While Rs.Read()
	LinkCategory_delete.lRecCnt = LinkCategory_delete.lRecCnt + 1

	' Set row properties
	LinkCategory.CssClass = ""
	LinkCategory.CssStyle = ""
	LinkCategory.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	LinkCategory_delete.LoadRowValues(Rs)

	' Render row
	LinkCategory_delete.RenderRow()
%>
	<tr<%= LinkCategory.RowAttributes %>>
		<td<%= LinkCategory.ID.CellAttributes %>>
<div<%= LinkCategory.ID.ViewAttributes %>><%= LinkCategory.ID.ListViewValue %></div>
</td>
		<td<%= LinkCategory.Title.CellAttributes %>>
<div<%= LinkCategory.Title.ViewAttributes %>><%= LinkCategory.Title.ListViewValue %></div>
</td>
		<td<%= LinkCategory.ParentID.CellAttributes %>>
<div<%= LinkCategory.ParentID.ViewAttributes %>><%= LinkCategory.ParentID.ListViewValue %></div>
</td>
		<td<%= LinkCategory.zPageID.CellAttributes %>>
<div<%= LinkCategory.zPageID.ViewAttributes %>><%= LinkCategory.zPageID.ListViewValue %></div>
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
