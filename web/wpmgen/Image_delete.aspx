<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Image_delete.aspx.vb" Inherits="Image_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_delete = new ew_Page("Image_delete");
// page properties
Image_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Image_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Image_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Image_delete.LoadRecordset()
If Image_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Image_delete.Page_Terminate("Image_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site Image<br /><br />
<a href="<%= Image.ReturnUrl %>">Go Back</a></span></p>
<% Image_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Image" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Image_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Image_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Image.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Company</td>
		<td valign="top">Title</td>
		<td valign="top">Name</td>
	</tr>
	</thead>
	<tbody>
<%
Image_delete.lRecCnt = 0
Do While Rs.Read()
	Image_delete.lRecCnt = Image_delete.lRecCnt + 1

	' Set row properties
	Image.CssClass = ""
	Image.CssStyle = ""
	Image.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Image_delete.LoadRowValues(Rs)

	' Render row
	Image_delete.RenderRow()
%>
	<tr<%= Image.RowAttributes %>>
		<td<%= Image.CompanyID.CellAttributes %>>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
</td>
		<td<%= Image.title.CellAttributes %>>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
</td>
		<td<%= Image.ImageName.CellAttributes %>>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
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
