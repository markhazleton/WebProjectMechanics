<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteParameterType_delete.aspx.vb" Inherits="SiteParameterType_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_delete = new ew_Page("SiteParameterType_delete");
// page properties
SiteParameterType_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = SiteParameterType_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_delete.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<%

' Load records for display
Rs = SiteParameterType_delete.LoadRecordset()
If SiteParameterType_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteParameterType_delete.Page_Terminate("SiteParameterType_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Parameter Type<br /><br />
<a href="<%= SiteParameterType.ReturnUrl %>">Go Back</a></span></p>
<% SiteParameterType_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteParameterType" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteParameterType_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteParameterType_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteParameterType.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Parameter Name</td>
		<td valign="top">Parameter Description</td>
		<td valign="top">Parameter Order</td>
	</tr>
	</thead>
	<tbody>
<%
SiteParameterType_delete.lRecCnt = 0
Do While Rs.Read()
	SiteParameterType_delete.lRecCnt = SiteParameterType_delete.lRecCnt + 1

	' Set row properties
	SiteParameterType.CssClass = ""
	SiteParameterType.CssStyle = ""
	SiteParameterType.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteParameterType_delete.LoadRowValues(Rs)

	' Render row
	SiteParameterType_delete.RenderRow()
%>
	<tr<%= SiteParameterType.RowAttributes %>>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeNM.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeNM.ListViewValue %></div>
</td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeDS.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeDS.ListViewValue %></div>
</td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeOrder.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeOrder.ListViewValue %></div>
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
<input type="submit" name="Action" id="Action" value="Confirm Delete" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
