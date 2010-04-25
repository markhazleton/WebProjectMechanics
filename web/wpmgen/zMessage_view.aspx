<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="zMessage_view.aspx.vb" Inherits="zMessage_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If zMessage.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var zMessage_view = new ew_Page("zMessage_view");
// page properties
zMessage_view.PageID = "view"; // page ID
var EW_PAGE_ID = zMessage_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
zMessage_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
zMessage_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Message
<br /><br />
<% If zMessage.Export = "" Then %>
<a href="zMessage_list.aspx">Back to List</a>&nbsp;
<a href="<%= zMessage.AddUrl %>">Add</a>&nbsp;
<a href="<%= zMessage.EditUrl %>">Edit</a>&nbsp;
<a href="<%= zMessage.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= zMessage.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% zMessage_view.ShowMessage() %>
<p />
<% If zMessage.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zMessage_view.Pager Is Nothing Then zMessage_view.Pager = New cPrevNextPager(zMessage_view.lStartRec, zMessage_view.lDisplayRecs, zMessage_view.lTotalRecs) %>
<% If zMessage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zMessage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zMessage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zMessage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zMessage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zMessage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zMessage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If zMessage_view.sSrchWhere = "0=101" Then %>
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
<% If zMessage.MessageID.Visible Then ' MessageID %>
	<tr<%= zMessage.MessageID.RowAttributes %>>
		<td class="ewTableHeader">Message ID</td>
		<td<%= zMessage.MessageID.CellAttributes %>>
<div<%= zMessage.MessageID.ViewAttributes %>><%= zMessage.MessageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.zPageID.Visible Then ' PageID %>
	<tr<%= zMessage.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Page ID</td>
		<td<%= zMessage.zPageID.CellAttributes %>>
<div<%= zMessage.zPageID.ViewAttributes %>><%= zMessage.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.ParentMessageID.Visible Then ' ParentMessageID %>
	<tr<%= zMessage.ParentMessageID.RowAttributes %>>
		<td class="ewTableHeader">Parent Message ID</td>
		<td<%= zMessage.ParentMessageID.CellAttributes %>>
<div<%= zMessage.ParentMessageID.ViewAttributes %>><%= zMessage.ParentMessageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.Subject.Visible Then ' Subject %>
	<tr<%= zMessage.Subject.RowAttributes %>>
		<td class="ewTableHeader">Subject</td>
		<td<%= zMessage.Subject.CellAttributes %>>
<div<%= zMessage.Subject.ViewAttributes %>><%= zMessage.Subject.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.Author.Visible Then ' Author %>
	<tr<%= zMessage.Author.RowAttributes %>>
		<td class="ewTableHeader">Author</td>
		<td<%= zMessage.Author.CellAttributes %>>
<div<%= zMessage.Author.ViewAttributes %>><%= zMessage.Author.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.zEmail.Visible Then ' Email %>
	<tr<%= zMessage.zEmail.RowAttributes %>>
		<td class="ewTableHeader">Email</td>
		<td<%= zMessage.zEmail.CellAttributes %>>
<div<%= zMessage.zEmail.ViewAttributes %>><%= zMessage.zEmail.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.City.Visible Then ' City %>
	<tr<%= zMessage.City.RowAttributes %>>
		<td class="ewTableHeader">City</td>
		<td<%= zMessage.City.CellAttributes %>>
<div<%= zMessage.City.ViewAttributes %>><%= zMessage.City.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.URL.Visible Then ' URL %>
	<tr<%= zMessage.URL.RowAttributes %>>
		<td class="ewTableHeader">URL</td>
		<td<%= zMessage.URL.CellAttributes %>>
<div<%= zMessage.URL.ViewAttributes %>><%= zMessage.URL.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.MessageDate.Visible Then ' MessageDate %>
	<tr<%= zMessage.MessageDate.RowAttributes %>>
		<td class="ewTableHeader">Message Date</td>
		<td<%= zMessage.MessageDate.CellAttributes %>>
<div<%= zMessage.MessageDate.ViewAttributes %>><%= zMessage.MessageDate.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If zMessage.Body.Visible Then ' Body %>
	<tr<%= zMessage.Body.RowAttributes %>>
		<td class="ewTableHeader">Body</td>
		<td<%= zMessage.Body.CellAttributes %>>
<div<%= zMessage.Body.ViewAttributes %>><%= zMessage.Body.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If zMessage.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If zMessage_view.Pager Is Nothing Then zMessage_view.Pager = New cPrevNextPager(zMessage_view.lStartRec, zMessage_view.lDisplayRecs, zMessage_view.lTotalRecs) %>
<% If zMessage_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If zMessage_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If zMessage_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= zMessage_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If zMessage_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If zMessage_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= zMessage_view.PageUrl %>start=<%= zMessage_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= zMessage_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If zMessage_view.sSrchWhere = "0=101" Then %>
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
<% If zMessage.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
