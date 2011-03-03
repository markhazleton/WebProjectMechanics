<%@ Page ClassName="image_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="image_delete.aspx.vb" Inherits="image_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Image_delete = new ew_Page("Image_delete");
// page properties
Image_delete.PageID = "delete"; // page ID
Image_delete.FormID = "fImagedelete"; // form ID 
var EW_PAGE_ID = Image_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Image_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Image_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Image_delete.LoadRecordset()
If Image_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Image_delete.Page_Terminate("image_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Image.TableCaption %><br /><br />
<a href="<%= Image.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Image_delete.DebugMsg) %>
<% Image_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Image" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Image_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Image_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Image.TableCustomInnerHTML %>
	<thead>
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
	<tbody>
<%
Image_delete.lRecCnt = 0
Do While Rs.Read()
	Image_delete.lRecCnt = Image_delete.lRecCnt + 1

	' Set row properties
	Image.CssClass = ""
	Image.CssStyle = ""
	Image.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Image_delete.LoadRowValues(Rs)

	' Render row
	Image_delete.RenderRow()
%>
	<tr<%= Image.RowAttributes %>>
		<td<%= Image.ImageID.CellAttributes %>>
<div<%= Image.ImageID.ViewAttributes %>><%= Image.ImageID.ListViewValue %></div>
</td>
		<td<%= Image.ImageName.CellAttributes %>>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ListViewValue %></div>
</td>
		<td<%= Image.ImageFileName.CellAttributes %>>
<div<%= Image.ImageFileName.ViewAttributes %>><%= Image.ImageFileName.ListViewValue %></div>
</td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>>
<div<%= Image.ImageThumbFileName.ViewAttributes %>><%= Image.ImageThumbFileName.ListViewValue %></div>
</td>
		<td<%= Image.ImageDate.CellAttributes %>>
<div<%= Image.ImageDate.ViewAttributes %>><%= Image.ImageDate.ListViewValue %></div>
</td>
		<td<%= Image.Active.CellAttributes %>>
<% If Convert.ToString(Image.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.Active.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
		<td<%= Image.ModifiedDT.CellAttributes %>>
<div<%= Image.ModifiedDT.ViewAttributes %>><%= Image.ModifiedDT.ListViewValue %></div>
</td>
		<td<%= Image.VersionNo.CellAttributes %>>
<div<%= Image.VersionNo.ViewAttributes %>><%= Image.VersionNo.ListViewValue %></div>
</td>
		<td<%= Image.ContactID.CellAttributes %>>
<div<%= Image.ContactID.ViewAttributes %>><%= Image.ContactID.ListViewValue %></div>
</td>
		<td<%= Image.CompanyID.CellAttributes %>>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ListViewValue %></div>
</td>
		<td<%= Image.title.CellAttributes %>>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ListViewValue %></div>
</td>
		<td<%= Image.medium.CellAttributes %>>
<div<%= Image.medium.ViewAttributes %>><%= Image.medium.ListViewValue %></div>
</td>
		<td<%= Image.size.CellAttributes %>>
<div<%= Image.size.ViewAttributes %>><%= Image.size.ListViewValue %></div>
</td>
		<td<%= Image.price.CellAttributes %>>
<div<%= Image.price.ViewAttributes %>><%= Image.price.ListViewValue %></div>
</td>
		<td<%= Image.color.CellAttributes %>>
<div<%= Image.color.ViewAttributes %>><%= Image.color.ListViewValue %></div>
</td>
		<td<%= Image.subject.CellAttributes %>>
<div<%= Image.subject.ViewAttributes %>><%= Image.subject.ListViewValue %></div>
</td>
		<td<%= Image.sold.CellAttributes %>>
<% If Convert.ToString(Image.sold.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.sold.ListViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
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
