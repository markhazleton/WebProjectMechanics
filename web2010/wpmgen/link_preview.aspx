<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="link_preview.aspx.vb" Inherits="link_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Link.TableCaption %>
<% If Link_preview.lTotalRecs > 0 Then %>
(<%= Link_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If Link_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;"><%= Link.Title.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.LinkTypeCD.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.CategoryID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.CompanyID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.SiteCategoryGroupID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.zPageID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.Views.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.Ranks.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.UserID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.ASIN.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Link.DateAdd.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	Link_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		Link_preview.lRowCnt += 1
		Link.CssClass = "ewTableRow"
		Link.CssStyle = ""
		Link.LoadListRowValues(Rs)

		' Render row
		Link.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		Link.RenderListRow()
%>
	<tr<%= Link.RowAttributes %>>
		<!-- Title -->
		<td>
<div<%= Link.Title.ViewAttributes %>><%= Link.Title.ListViewValue %></div>
</td>
		<!-- LinkTypeCD -->
		<td>
<div<%= Link.LinkTypeCD.ViewAttributes %>><%= Link.LinkTypeCD.ListViewValue %></div>
</td>
		<!-- CategoryID -->
		<td>
<div<%= Link.CategoryID.ViewAttributes %>><%= Link.CategoryID.ListViewValue %></div>
</td>
		<!-- CompanyID -->
		<td>
<div<%= Link.CompanyID.ViewAttributes %>><%= Link.CompanyID.ListViewValue %></div>
</td>
		<!-- SiteCategoryGroupID -->
		<td>
<div<%= Link.SiteCategoryGroupID.ViewAttributes %>><%= Link.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<!-- PageID -->
		<td>
<div<%= Link.zPageID.ViewAttributes %>><%= Link.zPageID.ListViewValue %></div>
</td>
		<!-- Views -->
		<td>
<% If Convert.ToString(Link.Views.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Link.Views.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<!-- Ranks -->
		<td>
<div<%= Link.Ranks.ViewAttributes %>><%= Link.Ranks.ListViewValue %></div>
</td>
		<!-- UserID -->
		<td>
<div<%= Link.UserID.ViewAttributes %>><%= Link.UserID.ListViewValue %></div>
</td>
		<!-- ASIN -->
		<td>
<div<%= Link.ASIN.ViewAttributes %>><%= Link.ASIN.ListViewValue %></div>
</td>
		<!-- DateAdd -->
		<td>
<div<%= Link.DateAdd.ViewAttributes %>><%= Link.DateAdd.ListViewValue %></div>
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
