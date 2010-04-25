<%@ Page Language="VB" CodePage="65001" LCID="1078" AutoEventWireup="false" CodeFile="SiteCategory_preview.aspx.vb" Inherits="SiteCategory_preview" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<link href="WPMGen.css" rel="stylesheet" type="text/css" />
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type Location
<% If nTotalRecs > 0 Then %>
(<%= nTotalRecs %> Records)
<% Else %>
(No records found)
<% End If %>
</span></p>
<%
If nTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;">Site Type</td>
			<td valign="top" style="white-space: nowrap;">Order</td>
			<td valign="top" style="white-space: nowrap;">Name</td>
			<td valign="top" style="white-space: nowrap;">Parent Category</td>
			<td valign="top" style="white-space: nowrap;">Category File Name</td>
			<td valign="top" style="white-space: nowrap;">Site Group</td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	nRecCount = 0
	Do While Rs.Read()

		' Init row class and style
		nRecCount = nRecCount + 1
		SiteCategory.CssClass = "ewTableRow"
		SiteCategory.CssStyle = ""
		SiteCategory.LoadListRowValues(Rs)

		' Render row
		SiteCategory.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		SiteCategory.RenderListRow()
%>
	<tr<%= SiteCategory.RowAttributes %>>
		<!-- SiteCategoryTypeID -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategory.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<!-- GroupOrder -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.GroupOrder.ViewAttributes %>><%= SiteCategory.GroupOrder.ListViewValue %></div>
</td>
		<!-- CategoryName -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.CategoryName.ViewAttributes %>><%= SiteCategory.CategoryName.ListViewValue %></div>
</td>
		<!-- ParentCategoryID -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.ParentCategoryID.ViewAttributes %>><%= SiteCategory.ParentCategoryID.ListViewValue %></div>
</td>
		<!-- CategoryFileName -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.CategoryFileName.ViewAttributes %>><%= SiteCategory.CategoryFileName.ListViewValue %></div>
</td>
		<!-- SiteCategoryGroupID -->
		<td style="white-space: nowrap;">
<div<%= SiteCategory.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategory.SiteCategoryGroupID.ListViewValue %></div>
</td>
	</tr>
<%
	Loop
%>
	</tbody>
</table>
</div>
</td></tr></table>
<%
End If
%>
