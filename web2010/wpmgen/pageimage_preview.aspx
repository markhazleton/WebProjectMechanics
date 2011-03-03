<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="pageimage_preview.aspx.vb" Inherits="pageimage_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= PageImage.TableCaption %>
<% If PageImage_preview.lTotalRecs > 0 Then %>
(<%= PageImage_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If PageImage_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;"><%= PageImage.zPageID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= PageImage.ImageID.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= PageImage.PageImagePosition.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	PageImage_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		PageImage_preview.lRowCnt += 1
		PageImage.CssClass = "ewTableRow"
		PageImage.CssStyle = ""
		PageImage.LoadListRowValues(Rs)

		' Render row
		PageImage.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		PageImage.RenderListRow()
%>
	<tr<%= PageImage.RowAttributes %>>
		<!-- PageID -->
		<td>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ListViewValue %></div>
</td>
		<!-- ImageID -->
		<td>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ListViewValue %></div>
</td>
		<!-- PageImagePosition -->
		<td>
<div<%= PageImage.PageImagePosition.ViewAttributes %>><%= PageImage.PageImagePosition.ListViewValue %></div>
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
