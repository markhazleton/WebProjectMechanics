<%@ Page ClassName="linktype_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="linktype_delete.aspx.vb" Inherits="linktype_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var LinkType_delete = new ew_Page("LinkType_delete");
// page properties
LinkType_delete.PageID = "delete"; // page ID
LinkType_delete.FormID = "fLinkTypedelete"; // form ID 
var EW_PAGE_ID = LinkType_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
LinkType_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
LinkType_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
LinkType_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = LinkType_delete.LoadRecordset()
If LinkType_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	LinkType_delete.Page_Terminate("linktype_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= LinkType.TableCaption %><br /><br />
<a href="<%= LinkType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(LinkType_delete.DebugMsg) %>
<% LinkType_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="LinkType" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to LinkType_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(LinkType_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= LinkType.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= LinkType.LinkTypeCD.FldCaption %></td>
		<td valign="top"><%= LinkType.LinkTypeDesc.FldCaption %></td>
		<td valign="top"><%= LinkType.LinkTypeTarget.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
LinkType_delete.lRecCnt = 0
Do While Rs.Read()
	LinkType_delete.lRecCnt = LinkType_delete.lRecCnt + 1

	' Set row properties
	LinkType.CssClass = ""
	LinkType.CssStyle = ""
	LinkType.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	LinkType_delete.LoadRowValues(Rs)

	' Render row
	LinkType_delete.RenderRow()
%>
	<tr<%= LinkType.RowAttributes %>>
		<td<%= LinkType.LinkTypeCD.CellAttributes %>>
<div<%= LinkType.LinkTypeCD.ViewAttributes %>><%= LinkType.LinkTypeCD.ListViewValue %></div>
</td>
		<td<%= LinkType.LinkTypeDesc.CellAttributes %>>
<div<%= LinkType.LinkTypeDesc.ViewAttributes %>><%= LinkType.LinkTypeDesc.ListViewValue %></div>
</td>
		<td<%= LinkType.LinkTypeTarget.CellAttributes %>>
<div<%= LinkType.LinkTypeTarget.ViewAttributes %>><%= LinkType.LinkTypeTarget.ListViewValue %></div>
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
