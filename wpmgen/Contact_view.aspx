<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Contact_view.aspx.vb" Inherits="Contact_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Contact.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Contact_view = new ew_Page("Contact_view");
// page properties
Contact_view.PageID = "view"; // page ID
var EW_PAGE_ID = Contact_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Contact_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Contact
<br /><br />
<% If Contact.Export = "" Then %>
<a href="Contact_list.aspx">Back to List</a>&nbsp;
<a href="<%= Contact.AddUrl %>">Add</a>&nbsp;
<a href="<%= Contact.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Contact.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= Contact.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% Contact_view.ShowMessage() %>
<p />
<% If Contact.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Contact_view.Pager Is Nothing Then Contact_view.Pager = New cPrevNextPager(Contact_view.lStartRec, Contact_view.lDisplayRecs, Contact_view.lTotalRecs) %>
<% If Contact_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Contact_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Contact_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Contact_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Contact_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Contact_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Contact_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Contact_view.sSrchWhere = "0=101" Then %>
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
<% If Contact.LogonName.Visible Then ' LogonName %>
	<tr<%= Contact.LogonName.RowAttributes %>>
		<td class="ewTableHeader">Logon Name</td>
		<td<%= Contact.LogonName.CellAttributes %>>
<div<%= Contact.LogonName.ViewAttributes %>><%= Contact.LogonName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.PrimaryContact.Visible Then ' PrimaryContact %>
	<tr<%= Contact.PrimaryContact.RowAttributes %>>
		<td class="ewTableHeader">Full Name</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.zEMail.Visible Then ' EMail %>
	<tr<%= Contact.zEMail.RowAttributes %>>
		<td class="ewTableHeader">EMail</td>
		<td<%= Contact.zEMail.CellAttributes %>>
<div<%= Contact.zEMail.ViewAttributes %>><%= Contact.zEMail.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.Active.Visible Then ' Active %>
	<tr<%= Contact.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Contact.Active.CellAttributes %>>
<% If Convert.ToString(Contact.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Contact.Active.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Contact.Active.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Contact.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Contact.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Contact.CompanyID.CellAttributes %>>
<div<%= Contact.CompanyID.ViewAttributes %>><%= Contact.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.GroupID.Visible Then ' GroupID %>
	<tr<%= Contact.GroupID.RowAttributes %>>
		<td class="ewTableHeader">Group</td>
		<td<%= Contact.GroupID.CellAttributes %>>
<div<%= Contact.GroupID.ViewAttributes %>><%= Contact.GroupID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.TemplatePrefix.Visible Then ' TemplatePrefix %>
	<tr<%= Contact.TemplatePrefix.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= Contact.TemplatePrefix.CellAttributes %>>
<div<%= Contact.TemplatePrefix.ViewAttributes %>><%= Contact.TemplatePrefix.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Contact.RoleID.Visible Then ' RoleID %>
	<tr<%= Contact.RoleID.RowAttributes %>>
		<td class="ewTableHeader">Role ID</td>
		<td<%= Contact.RoleID.CellAttributes %>>
<div<%= Contact.RoleID.ViewAttributes %>><%= Contact.RoleID.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Contact.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Contact_view.Pager Is Nothing Then Contact_view.Pager = New cPrevNextPager(Contact_view.lStartRec, Contact_view.lDisplayRecs, Contact_view.lTotalRecs) %>
<% If Contact_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Contact_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Contact_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Contact_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Contact_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Contact_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Contact_view.PageUrl %>start=<%= Contact_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Contact_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Contact_view.sSrchWhere = "0=101" Then %>
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
<% If Contact.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
