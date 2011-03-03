<%@ Page ClassName="sitecategory_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategory_delete.aspx.vb" Inherits="sitecategory_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategory_delete = new ew_Page("SiteCategory_delete");
// page properties
SiteCategory_delete.PageID = "delete"; // page ID
SiteCategory_delete.FormID = "fSiteCategorydelete"; // form ID 
var EW_PAGE_ID = SiteCategory_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteCategory_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategory_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategory_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategory_delete.LoadRecordset()
If SiteCategory_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategory_delete.Page_Terminate("sitecategory_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategory.TableCaption %><br /><br />
<a href="<%= SiteCategory.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategory_delete.DebugMsg) %>
<% SiteCategory_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategory" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategory_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategory_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategory.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= SiteCategory.SiteCategoryID.FldCaption %></td>
		<td valign="top"><%= SiteCategory.CategoryKeywords.FldCaption %></td>
		<td valign="top"><%= SiteCategory.CategoryName.FldCaption %></td>
		<td valign="top"><%= SiteCategory.CategoryTitle.FldCaption %></td>
		<td valign="top"><%= SiteCategory.CategoryDescription.FldCaption %></td>
		<td valign="top"><%= SiteCategory.GroupOrder.FldCaption %></td>
		<td valign="top"><%= SiteCategory.ParentCategoryID.FldCaption %></td>
		<td valign="top"><%= SiteCategory.CategoryFileName.FldCaption %></td>
		<td valign="top"><%= SiteCategory.SiteCategoryTypeID.FldCaption %></td>
		<td valign="top"><%= SiteCategory.SiteCategoryGroupID.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategory_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategory_delete.lRecCnt = SiteCategory_delete.lRecCnt + 1

	' Set row properties
	SiteCategory.CssClass = ""
	SiteCategory.CssStyle = ""
	SiteCategory.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategory_delete.LoadRowValues(Rs)

	' Render row
	SiteCategory_delete.RenderRow()
%>
	<tr<%= SiteCategory.RowAttributes %>>
		<td<%= SiteCategory.SiteCategoryID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryID.ViewAttributes %>><%= SiteCategory.SiteCategoryID.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryKeywords.CellAttributes %>>
<div<%= SiteCategory.CategoryKeywords.ViewAttributes %>><%= SiteCategory.CategoryKeywords.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryName.CellAttributes %>>
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryTitle.CellAttributes %>>
<div<%= SiteCategory.CategoryTitle.ViewAttributes %>><%= SiteCategory.CategoryTitle.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryDescription.CellAttributes %>>
<div<%= SiteCategory.CategoryDescription.ViewAttributes %>><%= SiteCategory.CategoryDescription.ListViewValue %></div>
</td>
		<td<%= SiteCategory.GroupOrder.CellAttributes %>>
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ListViewValue %></div>
</td>
		<td<%= SiteCategory.ParentCategoryID.CellAttributes %>>
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ListViewValue %></div>
</td>
		<td<%= SiteCategory.CategoryFileName.CellAttributes %>>
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ListViewValue %></div>
</td>
		<td<%= SiteCategory.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<td<%= SiteCategory.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ListViewValue %></div>
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
