<%@ Page ClassName="company_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="company_delete.aspx.vb" Inherits="company_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Company_delete = new ew_Page("Company_delete");
// page properties
Company_delete.PageID = "delete"; // page ID
Company_delete.FormID = "fCompanydelete"; // form ID 
var EW_PAGE_ID = Company_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Company_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<%

' Load records for display
Rs = Company_delete.LoadRecordset()
If Company_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Company_delete.Page_Terminate("company_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Company.TableCaption %><br /><br />
<a href="<%= Company.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Company_delete.DebugMsg) %>
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
		<td valign="top"><%= Company.CompanyName.FldCaption %></td>
		<td valign="top"><%= Company.SiteTitle.FldCaption %></td>
		<td valign="top"><%= Company.GalleryFolder.FldCaption %></td>
		<td valign="top"><%= Company.SiteTemplate.FldCaption %></td>
		<td valign="top"><%= Company.DefaultSiteTemplate.FldCaption %></td>
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
		<td<%= Company.GalleryFolder.CellAttributes %>>
<div<%= Company.GalleryFolder.ViewAttributes %>><%= Company.GalleryFolder.ListViewValue %></div>
</td>
		<td<%= Company.SiteTemplate.CellAttributes %>>
<div<%= Company.SiteTemplate.ViewAttributes %>><%= Company.SiteTemplate.ListViewValue %></div>
</td>
		<td<%= Company.DefaultSiteTemplate.CellAttributes %>>
<div<%= Company.DefaultSiteTemplate.ViewAttributes %>><%= Company.DefaultSiteTemplate.ListViewValue %></div>
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
