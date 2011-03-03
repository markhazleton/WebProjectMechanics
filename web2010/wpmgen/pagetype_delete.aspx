<%@ Page ClassName="pagetype_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pagetype_delete.aspx.vb" Inherits="pagetype_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageType_delete = new ew_Page("PageType_delete");
// page properties
PageType_delete.PageID = "delete"; // page ID
PageType_delete.FormID = "fPageTypedelete"; // form ID 
var EW_PAGE_ID = PageType_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
PageType_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageType_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = PageType_delete.LoadRecordset()
If PageType_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	PageType_delete.Page_Terminate("pagetype_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageType.TableCaption %><br /><br />
<a href="<%= PageType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageType_delete.DebugMsg) %>
<% PageType_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="PageType" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to PageType_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(PageType_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= PageType.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= PageType.PageTypeID.FldCaption %></td>
		<td valign="top"><%= PageType.PageTypeCD.FldCaption %></td>
		<td valign="top"><%= PageType.PageTypeDesc.FldCaption %></td>
		<td valign="top"><%= PageType.PageFileName.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
PageType_delete.lRecCnt = 0
Do While Rs.Read()
	PageType_delete.lRecCnt = PageType_delete.lRecCnt + 1

	' Set row properties
	PageType.CssClass = ""
	PageType.CssStyle = ""
	PageType.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	PageType_delete.LoadRowValues(Rs)

	' Render row
	PageType_delete.RenderRow()
%>
	<tr<%= PageType.RowAttributes %>>
		<td<%= PageType.PageTypeID.CellAttributes %>>
<div<%= PageType.PageTypeID.ViewAttributes %>><%= PageType.PageTypeID.ListViewValue %></div>
</td>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
<div<%= PageType.PageTypeCD.ViewAttributes %>><%= PageType.PageTypeCD.ListViewValue %></div>
</td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
<div<%= PageType.PageTypeDesc.ViewAttributes %>><%= PageType.PageTypeDesc.ListViewValue %></div>
</td>
		<td<%= PageType.PageFileName.CellAttributes %>>
<div<%= PageType.PageFileName.ViewAttributes %>><%= PageType.PageFileName.ListViewValue %></div>
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
