<%@ Page ClassName="sitecategorytype_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategorytype_delete.aspx.vb" Inherits="sitecategorytype_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_delete = new ew_Page("SiteCategoryType_delete");
// page properties
SiteCategoryType_delete.PageID = "delete"; // page ID
SiteCategoryType_delete.FormID = "fSiteCategoryTypedelete"; // form ID 
var EW_PAGE_ID = SiteCategoryType_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteCategoryType_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategoryType_delete.LoadRecordset()
If SiteCategoryType_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategoryType_delete.Page_Terminate("sitecategorytype_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryType.TableCaption %><br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryType_delete.DebugMsg) %>
<% SiteCategoryType_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategoryType_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategoryType_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategoryType.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= SiteCategoryType.SiteCategoryTypeID.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.SiteCategoryTypeNM.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.SiteCategoryTypeDS.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.SiteCategoryComment.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.SiteCategoryFileName.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.SiteCategoryTransferURL.FldCaption %></td>
		<td valign="top"><%= SiteCategoryType.DefaultSiteCategoryID.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategoryType_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategoryType_delete.lRecCnt = SiteCategoryType_delete.lRecCnt + 1

	' Set row properties
	SiteCategoryType.CssClass = ""
	SiteCategoryType.CssStyle = ""
	SiteCategoryType.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategoryType_delete.LoadRowValues(Rs)

	' Render row
	SiteCategoryType_delete.RenderRow()
%>
	<tr<%= SiteCategoryType.RowAttributes %>>
		<td<%= SiteCategoryType.SiteCategoryTypeID.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeID.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeID.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeNM.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeNM.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTypeDS.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTypeDS.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryComment.ViewAttributes %>><%= SiteCategoryType.SiteCategoryComment.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryFileName.ViewAttributes %>><%= SiteCategoryType.SiteCategoryFileName.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>>
<div<%= SiteCategoryType.SiteCategoryTransferURL.ViewAttributes %>><%= SiteCategoryType.SiteCategoryTransferURL.ListViewValue %></div>
</td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>>
<div<%= SiteCategoryType.DefaultSiteCategoryID.ViewAttributes %>><%= SiteCategoryType.DefaultSiteCategoryID.ListViewValue %></div>
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
