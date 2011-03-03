<%@ Page ClassName="link_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="link_delete.aspx.vb" Inherits="link_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Link_delete = new ew_Page("Link_delete");
// page properties
Link_delete.PageID = "delete"; // page ID
Link_delete.FormID = "fLinkdelete"; // form ID 
var EW_PAGE_ID = Link_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Link_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Link_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Link_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Link_delete.LoadRecordset()
If Link_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Link_delete.Page_Terminate("link_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Link.TableCaption %><br /><br />
<a href="<%= Link.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Link_delete.DebugMsg) %>
<% Link_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Link" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Link_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Link_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Link.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= Link.Title.FldCaption %></td>
		<td valign="top"><%= Link.LinkTypeCD.FldCaption %></td>
		<td valign="top"><%= Link.CategoryID.FldCaption %></td>
		<td valign="top"><%= Link.CompanyID.FldCaption %></td>
		<td valign="top"><%= Link.SiteCategoryGroupID.FldCaption %></td>
		<td valign="top"><%= Link.zPageID.FldCaption %></td>
		<td valign="top"><%= Link.Views.FldCaption %></td>
		<td valign="top"><%= Link.Ranks.FldCaption %></td>
		<td valign="top"><%= Link.UserID.FldCaption %></td>
		<td valign="top"><%= Link.ASIN.FldCaption %></td>
		<td valign="top"><%= Link.DateAdd.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
Link_delete.lRecCnt = 0
Do While Rs.Read()
	Link_delete.lRecCnt = Link_delete.lRecCnt + 1

	' Set row properties
	Link.CssClass = ""
	Link.CssStyle = ""
	Link.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Link_delete.LoadRowValues(Rs)

	' Render row
	Link_delete.RenderRow()
%>
	<tr<%= Link.RowAttributes %>>
		<td<%= Link.Title.CellAttributes %>>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ListViewValue %></div>
</td>
		<td<%= Link.LinkTypeCD.CellAttributes %>>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ListViewValue %></div>
</td>
		<td<%= Link.CategoryID.CellAttributes %>>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ListViewValue %></div>
</td>
		<td<%= Link.CompanyID.CellAttributes %>>
<div<%= Link.CompanyID.ViewAttributes %>><%= Link.CompanyID.ListViewValue %></div>
</td>
		<td<%= Link.SiteCategoryGroupID.CellAttributes %>>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= Link.zPageID.CellAttributes %>>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ListViewValue %></div>
</td>
		<td<%= Link.Views.CellAttributes %>>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Link.Ranks.CellAttributes %>>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ListViewValue %></div>
</td>
		<td<%= Link.UserID.CellAttributes %>>
<div<%= Link.UserID.ViewAttributes %>><%= Link.UserID.ListViewValue %></div>
</td>
		<td<%= Link.ASIN.CellAttributes %>>
<div<%= Link.ASIN.ViewAttributes %>><%= Link.ASIN.ListViewValue %></div>
</td>
		<td<%= Link.DateAdd.CellAttributes %>>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ListViewValue %></div>
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
