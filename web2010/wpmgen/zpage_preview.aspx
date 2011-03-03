<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="zpage_preview.aspx.vb" Inherits="zpage_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= zPage.TableCaption %>
<% If zPage_preview.lTotalRecs > 0 Then %>
(<%= zPage_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If zPage_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;"><%= zPage.ParentPageID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.zPageName.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.PageTitle.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.PageTypeID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.GroupID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.Active.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= zPage.PageOrder.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	zPage_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		zPage_preview.lRowCnt += 1
		zPage.CssClass = "ewTableRow"
		zPage.CssStyle = ""
		zPage.LoadListRowValues(Rs)

		' Render row
		zPage.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		zPage.RenderListRow()
%>
	<tr<%= zPage.RowAttributes %>>
		<!-- ParentPageID -->
		<td>
<div<%= zPage.ParentPageID.ViewAttributes %>><%= zPage.ParentPageID.ListViewValue %></div>
</td>
		<!-- PageName -->
		<td>
<div<%= zPage.zPageName.ViewAttributes %>><%= zPage.zPageName.ListViewValue %></div>
</td>
		<!-- PageTitle -->
		<td>
<div<%= zPage.PageTitle.ViewAttributes %>><%= zPage.PageTitle.ListViewValue %></div>
</td>
		<!-- PageTypeID -->
		<td>
<div<%= zPage.PageTypeID.ViewAttributes %>><%= zPage.PageTypeID.ListViewValue %></div>
</td>
		<!-- GroupID -->
		<td>
<div<%= zPage.GroupID.ViewAttributes %>><%= zPage.GroupID.ListViewValue %></div>
</td>
		<!-- Active -->
		<td>
<% If Convert.ToString(zPage.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= zPage.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<!-- PageOrder -->
		<td>
<div<%= zPage.PageOrder.ViewAttributes %>><%= zPage.PageOrder.ListViewValue %></div>
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
