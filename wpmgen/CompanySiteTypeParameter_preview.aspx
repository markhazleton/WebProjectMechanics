<%@ Page Language="VB" CodePage="65001" LCID="1078" AutoEventWireup="false" CodeFile="CompanySiteTypeParameter_preview.aspx.vb" Inherits="CompanySiteTypeParameter_preview" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<link href="WPMGen.css" rel="stylesheet" type="text/css" />
<p><span class="aspnetmaker" style="white-space: nowrap;">TABLE: Site Type Parameter
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
			<td valign="top">Parameter</td>
			<td valign="top">Site</td>
			<td valign="top">Site Type</td>
			<td valign="top">Site Group</td>
			<td valign="top">Site Category</td>
			<td valign="top">Process Order</td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	nRecCount = 0
	Do While Rs.Read()

		' Init row class and style
		nRecCount = nRecCount + 1
		CompanySiteTypeParameter.CssClass = "ewTableRow"
		CompanySiteTypeParameter.CssStyle = ""
		CompanySiteTypeParameter.LoadListRowValues(Rs)

		' Render row
		CompanySiteTypeParameter.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		CompanySiteTypeParameter.RenderListRow()
%>
	<tr<%= CompanySiteTypeParameter.RowAttributes %>>
		<!-- SiteParameterTypeID -->
		<td>
<div<%= CompanySiteTypeParameter.SiteParameterTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteParameterTypeID.ListViewValue %></div>
</td>
		<!-- CompanyID -->
		<td>
<div<%= CompanySiteTypeParameter.CompanyID.ViewAttributes %>><%= CompanySiteTypeParameter.CompanyID.ListViewValue %></div>
</td>
		<!-- SiteCategoryTypeID -->
		<td>
<div<%= CompanySiteTypeParameter.SiteCategoryTypeID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<!-- SiteCategoryGroupID -->
		<td>
<div<%= CompanySiteTypeParameter.SiteCategoryGroupID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<!-- SiteCategoryID -->
		<td>
<div<%= CompanySiteTypeParameter.SiteCategoryID.ViewAttributes %>><%= CompanySiteTypeParameter.SiteCategoryID.ListViewValue %></div>
</td>
		<!-- SortOrder -->
		<td>
<div<%= CompanySiteTypeParameter.SortOrder.ViewAttributes %>><%= CompanySiteTypeParameter.SortOrder.ListViewValue %></div>
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
