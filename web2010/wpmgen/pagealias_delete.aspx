<%@ Page ClassName="pagealias_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pagealias_delete.aspx.vb" Inherits="pagealias_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageAlias_delete = new ew_Page("PageAlias_delete");
// page properties
PageAlias_delete.PageID = "delete"; // page ID
PageAlias_delete.FormID = "fPageAliasdelete"; // form ID 
var EW_PAGE_ID = PageAlias_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
PageAlias_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageAlias_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageAlias_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = PageAlias_delete.LoadRecordset()
If PageAlias_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	PageAlias_delete.Page_Terminate("pagealias_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageAlias.TableCaption %><br /><br />
<a href="<%= PageAlias.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageAlias_delete.DebugMsg) %>
<% PageAlias_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="PageAlias" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to PageAlias_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(PageAlias_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= PageAlias.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= PageAlias.zPageURL.FldCaption %></td>
		<td valign="top"><%= PageAlias.TargetURL.FldCaption %></td>
		<td valign="top"><%= PageAlias.AliasType.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
PageAlias_delete.lRecCnt = 0
Do While Rs.Read()
	PageAlias_delete.lRecCnt = PageAlias_delete.lRecCnt + 1

	' Set row properties
	PageAlias.CssClass = ""
	PageAlias.CssStyle = ""
	PageAlias.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	PageAlias_delete.LoadRowValues(Rs)

	' Render row
	PageAlias_delete.RenderRow()
%>
	<tr<%= PageAlias.RowAttributes %>>
		<td<%= PageAlias.zPageURL.CellAttributes %>>
<div<%= PageAlias.zPageURL.ViewAttributes %>><%= PageAlias.zPageURL.ListViewValue %></div>
</td>
		<td<%= PageAlias.TargetURL.CellAttributes %>>
<div<%= PageAlias.TargetURL.ViewAttributes %>><%= PageAlias.TargetURL.ListViewValue %></div>
</td>
		<td<%= PageAlias.AliasType.CellAttributes %>>
<div<%= PageAlias.AliasType.ViewAttributes %>><%= PageAlias.AliasType.ListViewValue %></div>
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
