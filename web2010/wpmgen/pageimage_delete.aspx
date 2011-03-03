<%@ Page ClassName="pageimage_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="pageimage_delete.aspx.vb" Inherits="pageimage_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageImage_delete = new ew_Page("PageImage_delete");
// page properties
PageImage_delete.PageID = "delete"; // page ID
PageImage_delete.FormID = "fPageImagedelete"; // form ID 
var EW_PAGE_ID = PageImage_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
PageImage_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
PageImage_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = PageImage_delete.LoadRecordset()
If PageImage_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	PageImage_delete.Page_Terminate("pageimage_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= PageImage.TableCaption %><br /><br />
<a href="<%= PageImage.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(PageImage_delete.DebugMsg) %>
<% PageImage_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="PageImage" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to PageImage_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(PageImage_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= PageImage.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= PageImage.zPageID.FldCaption %></td>
		<td valign="top"><%= PageImage.ImageID.FldCaption %></td>
		<td valign="top"><%= PageImage.PageImagePosition.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
PageImage_delete.lRecCnt = 0
Do While Rs.Read()
	PageImage_delete.lRecCnt = PageImage_delete.lRecCnt + 1

	' Set row properties
	PageImage.CssClass = ""
	PageImage.CssStyle = ""
	PageImage.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	PageImage_delete.LoadRowValues(Rs)

	' Render row
	PageImage_delete.RenderRow()
%>
	<tr<%= PageImage.RowAttributes %>>
		<td<%= PageImage.zPageID.CellAttributes %>>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ListViewValue %></div>
</td>
		<td<%= PageImage.ImageID.CellAttributes %>>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ListViewValue %></div>
</td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>>
<div<%= PageImage.PageImagePosition.ViewAttributes %>><%= PageImage.PageImagePosition.ListViewValue %></div>
</td>
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
