<%@ Page ClassName="sitetemplate_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitetemplate_delete.aspx.vb" Inherits="sitetemplate_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_delete = new ew_Page("SiteTemplate_delete");
// page properties
SiteTemplate_delete.PageID = "delete"; // page ID
SiteTemplate_delete.FormID = "fSiteTemplatedelete"; // form ID 
var EW_PAGE_ID = SiteTemplate_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
SiteTemplate_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = SiteTemplate_delete.LoadRecordset()
If SiteTemplate_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	SiteTemplate_delete.Page_Terminate("sitetemplate_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteTemplate.TableCaption %><br /><br />
<a href="<%= SiteTemplate.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteTemplate_delete.DebugMsg) %>
<% SiteTemplate_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="SiteTemplate" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to SiteTemplate_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(SiteTemplate_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= SiteTemplate.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= SiteTemplate.TemplatePrefix.FldCaption %></td>
		<td valign="top"><%= SiteTemplate.zName.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
SiteTemplate_delete.lRecCnt = 0
Do While Rs.Read()
	SiteTemplate_delete.lRecCnt = SiteTemplate_delete.lRecCnt + 1

	' Set row properties
	SiteTemplate.CssClass = ""
	SiteTemplate.CssStyle = ""
	SiteTemplate.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	SiteTemplate_delete.LoadRowValues(Rs)

	' Render row
	SiteTemplate_delete.RenderRow()
%>
	<tr<%= SiteTemplate.RowAttributes %>>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>>
<div<%= SiteTemplate.TemplatePrefix.ViewAttributes %>><%= SiteTemplate.TemplatePrefix.ListViewValue %></div>
</td>
		<td<%= SiteTemplate.zName.CellAttributes %>>
<div<%= SiteTemplate.zName.ViewAttributes %>><%= SiteTemplate.zName.ListViewValue %></div>
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
