<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageImage_view.aspx.vb" Inherits="PageImage_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If PageImage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var PageImage_view = new ew_Page("PageImage_view");
// page properties
PageImage_view.PageID = "view"; // page ID
var EW_PAGE_ID = PageImage_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
PageImage_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageImage_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Location Image
<br /><br />
<% If PageImage.Export = "" Then %>
<a href="PageImage_list.aspx">Back to List</a>&nbsp;
<a href="<%= PageImage.AddUrl %>">Add</a>&nbsp;
<a href="<%= PageImage.EditUrl %>">Edit</a>&nbsp;
<a href="<%= PageImage.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= PageImage.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% PageImage_view.ShowMessage() %>
<p />
<% If PageImage.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageImage_view.Pager Is Nothing Then PageImage_view.Pager = New cPrevNextPager(PageImage_view.lStartRec, PageImage_view.lDisplayRecs, PageImage_view.lTotalRecs) %>
<% If PageImage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageImage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageImage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageImage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageImage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageImage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageImage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageImage_view.sSrchWhere = "0=101" Then %>
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
<% If PageImage.zPageID.Visible Then ' PageID %>
	<tr<%= PageImage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">PageName</td>
		<td<%= PageImage.zPageID.CellAttributes %>>
<div<%= PageImage.zPageID.ViewAttributes %>><%= PageImage.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageImage.ImageID.Visible Then ' ImageID %>
	<tr<%= PageImage.ImageID.RowAttributes %>>
		<td class="ewTableHeader">ImageName</td>
		<td<%= PageImage.ImageID.CellAttributes %>>
<div<%= PageImage.ImageID.ViewAttributes %>><%= PageImage.ImageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If PageImage.PageImagePosition.Visible Then ' PageImagePosition %>
	<tr<%= PageImage.PageImagePosition.RowAttributes %>>
		<td class="ewTableHeader">Page Image Position</td>
		<td<%= PageImage.PageImagePosition.CellAttributes %>>
<div<%= PageImage.PageImagePosition.ViewAttributes %>><%= PageImage.PageImagePosition.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If PageImage.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If PageImage_view.Pager Is Nothing Then PageImage_view.Pager = New cPrevNextPager(PageImage_view.lStartRec, PageImage_view.lDisplayRecs, PageImage_view.lTotalRecs) %>
<% If PageImage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If PageImage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If PageImage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= PageImage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If PageImage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If PageImage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= PageImage_view.PageUrl %>start=<%= PageImage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= PageImage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If PageImage_view.sSrchWhere = "0=101" Then %>
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
<% If PageImage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
