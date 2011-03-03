<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="pagealias_preview.aspx.vb" Inherits="pagealias_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= PageAlias.TableCaption %>
<% If PageAlias_preview.lTotalRecs > 0 Then %>
(<%= PageAlias_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If PageAlias_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;"><%= PageAlias.zPageURL.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= PageAlias.TargetURL.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= PageAlias.AliasType.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	PageAlias_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		PageAlias_preview.lRowCnt += 1
		PageAlias.CssClass = "ewTableRow"
		PageAlias.CssStyle = ""
		PageAlias.LoadListRowValues(Rs)

		' Render row
		PageAlias.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		PageAlias.RenderListRow()
%>
	<tr<%= PageAlias.RowAttributes %>>
		<!-- PageURL -->
		<td>
<div<%= PageAlias.zPageURL.ViewAttributes %>><%= PageAlias.zPageURL.ListViewValue %></div>
</td>
		<!-- TargetURL -->
		<td>
<div<%= PageAlias.TargetURL.ViewAttributes %>><%= PageAlias.TargetURL.ListViewValue %></div>
</td>
		<!-- AliasType -->
		<td>
<div<%= PageAlias.AliasType.ViewAttributes %>><%= PageAlias.AliasType.ListViewValue %></div>
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
