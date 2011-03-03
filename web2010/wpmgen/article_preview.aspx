<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="article_preview.aspx.vb" Inherits="article_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Article.TableCaption %>
<% If Article_preview.lTotalRecs > 0 Then %>
(<%= Article_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If Article_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top" style="white-space: nowrap;"><%= Article.Active.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Article.Title.FldCaption %></td>
			<td valign="top" style="white-space: nowrap;"><%= Article.zPageID.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	Article_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		Article_preview.lRowCnt += 1
		Article.CssClass = "ewTableRow"
		Article.CssStyle = ""
		Article.LoadListRowValues(Rs)

		' Render row
		Article.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		Article.RenderListRow()
%>
	<tr<%= Article.RowAttributes %>>
		<!-- Active -->
		<td>
<% If Convert.ToString(Article.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Article.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<!-- Title -->
		<td>
<div<%= Article.Title.ViewAttributes %>><%= Article.Title.ListViewValue %></div>
</td>
		<!-- PageID -->
		<td>
<div<%= Article.zPageID.ViewAttributes %>><%= Article.zPageID.ListViewValue %></div>
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
