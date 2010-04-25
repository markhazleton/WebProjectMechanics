<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteTemplate_view.aspx.vb" Inherits="SiteTemplate_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteTemplate.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteTemplate_view = new ew_Page("SiteTemplate_view");
// page properties
SiteTemplate_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteTemplate_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteTemplate_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteTemplate_view.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<div id="ewDetailsDiv" name="ewDetailsDivDiv" style="visibility:hidden"></div>
<script language="JavaScript" type="text/javascript">
<!--
// YUI container
var ewDetailsDiv;
var ew_AjaxDetailsTimer = null;
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<% End If %>
<p><span class="aspnetmaker">View TABLE: Presentation Template (skin)
<br /><br />
<% If SiteTemplate.Export = "" Then %>
<a href="SiteTemplate_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteTemplate.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteTemplate.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteTemplate.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= SiteTemplate.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% SiteTemplate_view.ShowMessage() %>
<p />
<% If SiteTemplate.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteTemplate_view.Pager Is Nothing Then SiteTemplate_view.Pager = New cPrevNextPager(SiteTemplate_view.lStartRec, SiteTemplate_view.lDisplayRecs, SiteTemplate_view.lTotalRecs) %>
<% If SiteTemplate_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteTemplate_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteTemplate_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteTemplate_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteTemplate_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteTemplate_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteTemplate_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteTemplate_view.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
	</tr>
</table>
</form>
<br />
<% End If %>
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteTemplate.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= SiteTemplate.TemplatePrefix.RowAttributes %>>
		<td class="ewTableHeader">Template Prefix</td>
		<td<%= SiteTemplate.TemplatePrefix.CellAttributes %>>
<div<%= SiteTemplate.TemplatePrefix.ViewAttributes %>><%= SiteTemplate.TemplatePrefix.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteTemplate.zName.Visible Then ' Name %>
	<tr<%= SiteTemplate.zName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= SiteTemplate.zName.CellAttributes %>>
<div<%= SiteTemplate.zName.ViewAttributes %>><%= SiteTemplate.zName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteTemplate.Top.Visible Then ' Top %>
	<tr<%= SiteTemplate.Top.RowAttributes %>>
		<td class="ewTableHeader">Top</td>
		<td<%= SiteTemplate.Top.CellAttributes %>>
<div<%= SiteTemplate.Top.ViewAttributes %>><%= SiteTemplate.Top.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteTemplate.Bottom.Visible Then ' Bottom %>
	<tr<%= SiteTemplate.Bottom.RowAttributes %>>
		<td class="ewTableHeader">Bottom</td>
		<td<%= SiteTemplate.Bottom.CellAttributes %>>
<div<%= SiteTemplate.Bottom.ViewAttributes %>><%= SiteTemplate.Bottom.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteTemplate.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteTemplate_view.Pager Is Nothing Then SiteTemplate_view.Pager = New cPrevNextPager(SiteTemplate_view.lStartRec, SiteTemplate_view.lDisplayRecs, SiteTemplate_view.lTotalRecs) %>
<% If SiteTemplate_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteTemplate_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteTemplate_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteTemplate_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteTemplate_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteTemplate_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteTemplate_view.PageUrl %>start=<%= SiteTemplate_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteTemplate_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteTemplate_view.sSrchWhere = "0=101" Then %>
	<span class="aspnetmaker">Please enter search criteria</span>
	<% Else %>
	<span class="aspnetmaker">No records found</span>
	<% End If %>
<% End If %>
		</td>
	</tr>
</table>
</form>
<% End If %>
<p />
<% If SiteTemplate.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
