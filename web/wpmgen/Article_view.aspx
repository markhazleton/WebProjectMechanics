<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Article_view.aspx.vb" Inherits="Article_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Article.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Article_view = new ew_Page("Article_view");
// page properties
Article_view.PageID = "view"; // page ID
var EW_PAGE_ID = Article_view.PageID; // for backward compatibility
Article_view.SelectAllKey = function(elem) {
	ew_SelectAll(elem);
}
<% If EW_CLIENT_VALIDATE Then %>
Article_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Article_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Article
<br /><br />
<% If Article.Export = "" Then %>
<a href="Article_list.aspx">Back to List</a>&nbsp;
<a href="<%= Article.AddUrl %>">Add</a>&nbsp;
<a href="<%= Article.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Article.CopyUrl %>">Copy</a>&nbsp;
<a onclick="return ew_Confirm('Do you want to delete this record?');" href="<%= Article.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% Article_view.ShowMessage() %>
<p />
<% If Article.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Article_view.Pager Is Nothing Then Article_view.Pager = New cPrevNextPager(Article_view.lStartRec, Article_view.lDisplayRecs, Article_view.lTotalRecs) %>
<% If Article_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Article_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Article_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Article_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Article_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Article_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Article_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Article_view.sSrchWhere = "0=101" Then %>
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
<% If Article.Active.Visible Then ' Active %>
	<tr<%= Article.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Article.Active.CellAttributes %>>
<% If Convert.ToString(Article.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Article.Active.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Article.Active.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Article.StartDT.Visible Then ' StartDT %>
	<tr<%= Article.StartDT.RowAttributes %>>
		<td class="ewTableHeader">Start DT</td>
		<td<%= Article.StartDT.CellAttributes %>>
<div<%= Article.StartDT.ViewAttributes %>><%= Article.StartDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.Title.Visible Then ' Title %>
	<tr<%= Article.Title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Article.Title.CellAttributes %>>
<div<%= Article.Title.ViewAttributes %>><%= Article.Title.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Article.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Site</td>
		<td<%= Article.CompanyID.CellAttributes %>>
<div<%= Article.CompanyID.ViewAttributes %>><%= Article.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.zPageID.Visible Then ' PageID %>
	<tr<%= Article.zPageID.RowAttributes %>>
		<td class="ewTableHeader">Location</td>
		<td<%= Article.zPageID.CellAttributes %>>
<div<%= Article.zPageID.ViewAttributes %>><%= Article.zPageID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.ContactID.Visible Then ' ContactID %>
	<tr<%= Article.ContactID.RowAttributes %>>
		<td class="ewTableHeader">Contact</td>
		<td<%= Article.ContactID.CellAttributes %>>
<div<%= Article.ContactID.ViewAttributes %>><%= Article.ContactID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.Description.Visible Then ' Description %>
	<tr<%= Article.Description.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Article.Description.CellAttributes %>>
<div<%= Article.Description.ViewAttributes %>><%= Article.Description.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.ArticleBody.Visible Then ' ArticleBody %>
	<tr<%= Article.ArticleBody.RowAttributes %>>
		<td class="ewTableHeader">Article Body</td>
		<td<%= Article.ArticleBody.CellAttributes %>>
<div<%= Article.ArticleBody.ViewAttributes %>><%= Article.ArticleBody.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= Article.ModifiedDT.RowAttributes %>>
		<td class="ewTableHeader">Modified DT</td>
		<td<%= Article.ModifiedDT.CellAttributes %>>
<div<%= Article.ModifiedDT.ViewAttributes %>><%= Article.ModifiedDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.EndDT.Visible Then ' EndDT %>
	<tr<%= Article.EndDT.RowAttributes %>>
		<td class="ewTableHeader">End DT</td>
		<td<%= Article.EndDT.CellAttributes %>>
<div<%= Article.EndDT.ViewAttributes %>><%= Article.EndDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Article.ExpireDT.Visible Then ' ExpireDT %>
	<tr<%= Article.ExpireDT.RowAttributes %>>
		<td class="ewTableHeader">Expire DT</td>
		<td<%= Article.ExpireDT.CellAttributes %>>
<div<%= Article.ExpireDT.ViewAttributes %>><%= Article.ExpireDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Article.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Article_view.Pager Is Nothing Then Article_view.Pager = New cPrevNextPager(Article_view.lStartRec, Article_view.lDisplayRecs, Article_view.lTotalRecs) %>
<% If Article_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Article_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Article_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Article_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Article_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Article_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Article_view.PageUrl %>start=<%= Article_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Article_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Article_view.sSrchWhere = "0=101" Then %>
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
<% If Article.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
