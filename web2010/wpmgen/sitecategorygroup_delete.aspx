<%@ Page ClassName="sitecategorygroup_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitecategorygroup_delete.aspx.vb" Inherits="sitecategorygroup_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryGroup_delete = new ew_Page("SiteCategoryGroup_delete");
// page properties
SiteCategoryGroup_delete.PageID = "delete"; // page ID
SiteCategoryGroup_delete.FormID = "fSiteCategoryGroupdelete"; // form ID 
var EW_PAGE_ID = SiteCategoryGroup_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteCategoryGroup_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryGroup_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryGroup_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteCategoryGroup_delete.LoadRecordset()
If SiteCategoryGroup_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteCategoryGroup_delete.Page_Terminate("sitecategorygroup_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteCategoryGroup.TableCaption %><br /><br />
<a href="<%= SiteCategoryGroup.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteCategoryGroup_delete.DebugMsg) %>
<% SiteCategoryGroup_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryGroup" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteCategoryGroup_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteCategoryGroup_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteCategoryGroup.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= SiteCategoryGroup.SiteCategoryGroupID.FldCaption %></td>
		<td valign="top"><%= SiteCategoryGroup.SiteCategoryGroupNM.FldCaption %></td>
		<td valign="top"><%= SiteCategoryGroup.SiteCategoryGroupDS.FldCaption %></td>
		<td valign="top"><%= SiteCategoryGroup.SiteCategoryGroupOrder.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
SiteCategoryGroup_delete.lRecCnt = 0
Do While Rs.Read()
	SiteCategoryGroup_delete.lRecCnt = SiteCategoryGroup_delete.lRecCnt + 1

	' Set row properties
	SiteCategoryGroup.CssClass = ""
	SiteCategoryGroup.CssStyle = ""
	SiteCategoryGroup.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteCategoryGroup_delete.LoadRowValues(Rs)

	' Render row
	SiteCategoryGroup_delete.RenderRow()
%>
	<tr<%= SiteCategoryGroup.RowAttributes %>>
		<td<%= SiteCategoryGroup.SiteCategoryGroupID.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupID.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupID.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupNM.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupNM.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupNM.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupDS.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupDS.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupDS.ListViewValue %></div>
</td>
		<td<%= SiteCategoryGroup.SiteCategoryGroupOrder.CellAttributes %>>
<div<%= SiteCategoryGroup.SiteCategoryGroupOrder.ViewAttributes %>><%= SiteCategoryGroup.SiteCategoryGroupOrder.ListViewValue %></div>
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
