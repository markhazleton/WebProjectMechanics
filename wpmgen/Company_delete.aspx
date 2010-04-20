<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Company_delete.aspx.vb" Inherits="Company_delete" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_delete = new ew_Page("Company_delete");
// page properties
Company_delete.PageID = "delete"; // page ID
var EW_PAGE_ID = Company_delete.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Company_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Company_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Company_delete.LoadRecordset()
If Company_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Company_delete.Page_Terminate("Company_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker">Delete From TABLE: Site<br /><br />
<a href="<%= Company.ReturnUrl %>">Go Back</a></span></p>
<% Company_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Company" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Company_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Company_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Company.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top">Company Name</td>
		<td valign="top">Site Title</td>
		<td valign="top">Site URL</td>
		<td valign="top">Template</td>
		<td valign="top">Default Template</td>
		<td valign="top">Active FL</td>
	</tr>
	</thead>
	<tbody>
<%
Company_delete.lRecCnt = 0
Do While Rs.Read()
	Company_delete.lRecCnt = Company_delete.lRecCnt + 1

	' Set row properties
	Company.CssClass = ""
	Company.CssStyle = ""
	Company.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Company_delete.LoadRowValues(Rs)

	' Render row
	Company_delete.RenderRow()
%>
	<tr<%= Company.RowAttributes %>>
		<td<%= Company.CompanyName.CellAttributes %>>
<div<%= Company.CompanyName.ViewAttributes %>><%= Company.CompanyName.ListViewValue %></div>
</td>
		<td<%= Company.SiteTitle.CellAttributes %>>
<div<%= Company.SiteTitle.ViewAttributes %>><%= Company.SiteTitle.ListViewValue %></div>
</td>
		<td<%= Company.SiteURL.CellAttributes %>>
<div<%= Company.SiteURL.ViewAttributes %>><%= Company.SiteURL.ListViewValue %></div>
</td>
		<td<%= Company.SiteTemplate.CellAttributes %>>
<div<%= Company.SiteTemplate.ViewAttributes %>><%= Company.SiteTemplate.ListViewValue %></div>
</td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
<div<%= Company.DefaultSiteTemplate.ViewAttributes %>><%= Company.DefaultSiteTemplate.ListViewValue %></div>
</td>
		<td<%= Company.ActiveFL.CellAttributes %>>
<% If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Company.ActiveFL.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Company.ActiveFL.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
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
