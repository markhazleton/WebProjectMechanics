<%@ Page ClassName="zpage_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zpage_delete.aspx.vb" Inherits="zpage_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var zPage_delete = new ew_Page("zPage_delete");
// page properties
zPage_delete.PageID = "delete"; // page ID
zPage_delete.FormID = "fzPagedelete"; // form ID 
var EW_PAGE_ID = zPage_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
zPage_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
zPage_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zPage_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = zPage_delete.LoadRecordset()
If zPage_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	zPage_delete.Page_Terminate("zpage_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= zPage.TableCaption %><br /><br />
<a href="<%= zPage.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(zPage_delete.DebugMsg) %>
<% zPage_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="zPage" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to zPage_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(zPage_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= zPage.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= zPage.ParentPageID.FldCaption %></td>
		<td valign="top"><%= zPage.zPageName.FldCaption %></td>
		<td valign="top"><%= zPage.PageTitle.FldCaption %></td>
		<td valign="top"><%= zPage.PageTypeID.FldCaption %></td>
		<td valign="top"><%= zPage.GroupID.FldCaption %></td>
		<td valign="top"><%= zPage.Active.FldCaption %></td>
		<td valign="top"><%= zPage.PageOrder.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
zPage_delete.lRecCnt = 0
Do While Rs.Read()
	zPage_delete.lRecCnt = zPage_delete.lRecCnt + 1

	' Set row properties
	zPage.CssClass = ""
	zPage.CssStyle = ""
	zPage.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	zPage_delete.LoadRowValues(Rs)

	' Render row
	zPage_delete.RenderRow()
%>
	<tr<%= zPage.RowAttributes %>>
		<td<%= zPage.ParentPageID.CellAttributes %>>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ListViewValue %></div>
</td>
		<td<%= zPage.zPageName.CellAttributes %>>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ListViewValue %></div>
</td>
		<td<%= zPage.PageTitle.CellAttributes %>>
<div<%= zPage.PageTitle.ViewAttributes %>><%= zPage.PageTitle.ListViewValue %></div>
</td>
		<td<%= zPage.PageTypeID.CellAttributes %>>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ListViewValue %></div>
</td>
		<td<%= zPage.GroupID.CellAttributes %>>
<div<%= zPage.GroupID.ViewAttributes %>><%= zPage.GroupID.ListViewValue %></div>
</td>
		<td<%= zPage.Active.CellAttributes %>>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= zPage.PageOrder.CellAttributes %>>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ListViewValue %></div>
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
