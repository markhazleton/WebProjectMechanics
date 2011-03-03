<%@ Page Language="VB" CodePage="65001" AutoEventWireup="false" CodeFile="image_preview.aspx.vb" Inherits="image_preview" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<link href="wpmwebsite.css" rel="stylesheet" type="text/css" />
<div class="aspnetmaker" style="white-space: nowrap;"><%= Language.Phrase("TblTypeTABLE") %><%= Image.TableCaption %>
<% If Image_preview.lTotalRecs > 0 Then %>
(<%= Image_preview.lTotalRecs %>&nbsp;<%= Language.Phrase("Record") %>)
<% Else %>
(<%= Language.Phrase("NoRecord") %>)
<% End If %>
</div>
<%
If Image_preview.lTotalRecs > 0 Then
%>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table id="ewDetailsPreviewTable" name="ewDetailsPreviewTable" cellspacing="0" class="ewTable ewTableSeparate">
	<thead><!-- Table header -->
		<tr class="ewTableHeader">
			<td valign="top"><%= Image.ImageID.FldCaption %></td>
			<td valign="top"><%= Image.ImageName.FldCaption %></td>
			<td valign="top"><%= Image.ImageFileName.FldCaption %></td>
			<td valign="top"><%= Image.ImageThumbFileName.FldCaption %></td>
			<td valign="top"><%= Image.ImageDate.FldCaption %></td>
			<td valign="top"><%= Image.Active.FldCaption %></td>
			<td valign="top"><%= Image.ModifiedDT.FldCaption %></td>
			<td valign="top"><%= Image.VersionNo.FldCaption %></td>
			<td valign="top"><%= Image.ContactID.FldCaption %></td>
			<td valign="top"><%= Image.CompanyID.FldCaption %></td>
			<td valign="top"><%= Image.title.FldCaption %></td>
			<td valign="top"><%= Image.medium.FldCaption %></td>
			<td valign="top"><%= Image.size.FldCaption %></td>
			<td valign="top"><%= Image.price.FldCaption %></td>
			<td valign="top"><%= Image.color.FldCaption %></td>
			<td valign="top"><%= Image.subject.FldCaption %></td>
			<td valign="top"><%= Image.sold.FldCaption %></td>
		</tr>
	</thead>
	<tbody><!-- Table body -->
<%
	Image_preview.lRowCnt = 0
	Do While Rs.Read()

		' Init row class and style
		Image_preview.lRowCnt += 1
		Image.CssClass = "ewTableRow"
		Image.CssStyle = ""
		Image.LoadListRowValues(Rs)

		' Render row
		Image.RowType = EW_ROWTYPE_PREVIEW ' Preview record
		Image.RenderListRow()
%>
	<tr<%= Image.RowAttributes %>>
		<!-- ImageID -->
		<td>
<div<%= Image.ImageID.ViewAttributes %>><%= Image.ImageID.ListViewValue %></div>
</td>
		<!-- ImageName -->
		<td>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
</td>
		<!-- ImageFileName -->
		<td>
<div<%= Image.ImageFileName.ViewAttributes %>><%= Image.ImageFileName.ListViewValue %></div>
</td>
		<!-- ImageThumbFileName -->
		<td>
<div<%= Image.ImageThumbFileName.ViewAttributes %>><%= Image.ImageThumbFileName.ListViewValue %></div>
</td>
		<!-- ImageDate -->
		<td>
<div<%= Image.ImageDate.ViewAttributes %>><%= Image.ImageDate.ListViewValue %></div>
</td>
		<!-- Active -->
		<td>
<% If Convert.ToString(Image.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<!-- ModifiedDT -->
		<td>
<div<%= Image.ModifiedDT.ViewAttributes %>><%= Image.ModifiedDT.ListViewValue %></div>
</td>
		<!-- VersionNo -->
		<td>
<div<%= Image.VersionNo.ViewAttributes %>><%= Image.VersionNo.ListViewValue %></div>
</td>
		<!-- ContactID -->
		<td>
<div<%= Image.ContactID.ViewAttributes %>><%= Image.ContactID.ListViewValue %></div>
</td>
		<!-- CompanyID -->
		<td>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
</td>
		<!-- title -->
		<td>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
</td>
		<!-- medium -->
		<td>
<div<%= Image.medium.ViewAttributes %>><%= Image.medium.ListViewValue %></div>
</td>
		<!-- size -->
		<td>
<div<%= Image.size.ViewAttributes %>><%= Image.size.ListViewValue %></div>
</td>
		<!-- price -->
		<td>
<div<%= Image.price.ViewAttributes %>><%= Image.price.ListViewValue %></div>
</td>
		<!-- color -->
		<td>
<div<%= Image.color.ViewAttributes %>><%= Image.color.ListViewValue %></div>
</td>
		<!-- subject -->
		<td>
<div<%= Image.subject.ViewAttributes %>><%= Image.subject.ListViewValue %></div>
</td>
		<!-- sold -->
		<td>
<% If Convert.ToString(Image.sold.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
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
