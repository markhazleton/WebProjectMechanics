<%@ Page Language="VB" CodePage="65001" LCID="1078" AutoEventWireup="false" CodeFile="CompanySiteParameter_preview.aspx.vb" Inherits="CompanySiteParameter_preview" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<link href="WPMGen.css" rel="stylesheet" type="text/css" />
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Parameter
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
			<td valign="top">Site</td>
			<td valign="top">Page</td>
			<td valign="top">Category Group</td>
			<td valign="top"> Parameter</td>
			<td valign="top">Process Order</td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	nRecCount = 0
	Do While Rs.Read()

		' Init row class and style
		nRecCount = nRecCount + 1
		CompanySiteParameter.CssClass = "ewTableRow"
		CompanySiteParameter.CssStyle = ""
		CompanySiteParameter.LoadListRowValues(Rs)

		' Render row
		CompanySiteParameter.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		CompanySiteParameter.RenderListRow()
%>
	<tr<%= CompanySiteParameter.RowAttributes %>>
		<!-- CompanyID -->
		<td>
<div<%= CompanySiteParameter.CompanyID.ViewAttributes %>><%= CompanySiteParameter.CompanyID.ListViewValue %></div>
</td>
		<!-- PageID -->
		<td>
<div<%= CompanySiteParameter.zPageID.ViewAttributes %>><%= CompanySiteParameter.zPageID.ListViewValue %></div>
</td>
		<!-- SiteCategoryGroupID -->
		<td>
<div<%= CompanySiteParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteParameter.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<!-- SiteParameterTypeID -->
		<td>
<div<%= CompanySiteParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteParameter.SiteParameterTypeID.ListViewValue %></div>
</td>
		<!-- SortOrder -->
		<td>
<div<%= CompanySiteParameter.SortOrder.ViewAttributes %>><%= CompanySiteParameter.SortOrder.ListViewValue %></div>
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
