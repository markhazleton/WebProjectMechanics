<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="Image_view.aspx.vb" Inherits="Image_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If Image.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var Image_view = new ew_Page("Image_view");
// page properties
Image_view.PageID = "view"; // page ID
var EW_PAGE_ID = Image_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
Image_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Image_view.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">View TABLE: Site Image
<br /><br />
<% If Image.Export = "" Then %>
<a href="Image_list.aspx">Back to List</a>&nbsp;
<a href="<%= Image.AddUrl %>">Add</a>&nbsp;
<a href="<%= Image.EditUrl %>">Edit</a>&nbsp;
<a href="<%= Image.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= Image.DeleteUrl %>">Delete</a>&nbsp;
<% End If %>
</span></p>
<% Image_view.ShowMessage() %>
<p />
<% If Image.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Image_view.Pager Is Nothing Then Image_view.Pager = New cPrevNextPager(Image_view.lStartRec, Image_view.lDisplayRecs, Image_view.lTotalRecs) %>
<% If Image_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Image_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Image_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Image_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Image_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Image_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Image_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Image_view.sSrchWhere = "0=101" Then %>
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
<% If Image.CompanyID.Visible Then ' CompanyID %>
	<tr<%= Image.CompanyID.RowAttributes %>>
		<td class="ewTableHeader">Company</td>
		<td<%= Image.CompanyID.CellAttributes %>>
<div<%= Image.CompanyID.ViewAttributes %>><%= Image.CompanyID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.title.Visible Then ' title %>
	<tr<%= Image.title.RowAttributes %>>
		<td class="ewTableHeader">Title</td>
		<td<%= Image.title.CellAttributes %>>
<div<%= Image.title.ViewAttributes %>><%= Image.title.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageName.Visible Then ' ImageName %>
	<tr<%= Image.ImageName.RowAttributes %>>
		<td class="ewTableHeader">Name</td>
		<td<%= Image.ImageName.CellAttributes %>>
<div<%= Image.ImageName.ViewAttributes %>><%= Image.ImageName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageDescription.Visible Then ' ImageDescription %>
	<tr<%= Image.ImageDescription.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= Image.ImageDescription.CellAttributes %>>
<div<%= Image.ImageDescription.ViewAttributes %>><%= Image.ImageDescription.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageComment.Visible Then ' ImageComment %>
	<tr<%= Image.ImageComment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= Image.ImageComment.CellAttributes %>>
<div<%= Image.ImageComment.ViewAttributes %>><%= Image.ImageComment.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageFileName.Visible Then ' ImageFileName %>
	<tr<%= Image.ImageFileName.RowAttributes %>>
		<td class="ewTableHeader">File Name</td>
		<td<%= Image.ImageFileName.CellAttributes %>>
<div<%= Image.ImageFileName.ViewAttributes %>><%= Image.ImageFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageThumbFileName.Visible Then ' ImageThumbFileName %>
	<tr<%= Image.ImageThumbFileName.RowAttributes %>>
		<td class="ewTableHeader">Thumb File Name</td>
		<td<%= Image.ImageThumbFileName.CellAttributes %>>
<div<%= Image.ImageThumbFileName.ViewAttributes %>><%= Image.ImageThumbFileName.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ImageDate.Visible Then ' ImageDate %>
	<tr<%= Image.ImageDate.RowAttributes %>>
		<td class="ewTableHeader">Created</td>
		<td<%= Image.ImageDate.CellAttributes %>>
<div<%= Image.ImageDate.ViewAttributes %>><%= Image.ImageDate.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.Active.Visible Then ' Active %>
	<tr<%= Image.Active.RowAttributes %>>
		<td class="ewTableHeader">Active</td>
		<td<%= Image.Active.CellAttributes %>>
<% If Convert.ToString(Image.Active.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.Active.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.Active.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
<% If Image.ModifiedDT.Visible Then ' ModifiedDT %>
	<tr<%= Image.ModifiedDT.RowAttributes %>>
		<td class="ewTableHeader">Modified</td>
		<td<%= Image.ModifiedDT.CellAttributes %>>
<div<%= Image.ModifiedDT.ViewAttributes %>><%= Image.ModifiedDT.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.VersionNo.Visible Then ' VersionNo %>
	<tr<%= Image.VersionNo.RowAttributes %>>
		<td class="ewTableHeader">Version No</td>
		<td<%= Image.VersionNo.CellAttributes %>>
<div<%= Image.VersionNo.ViewAttributes %>><%= Image.VersionNo.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.ContactID.Visible Then ' ContactID %>
	<tr<%= Image.ContactID.RowAttributes %>>
		<td class="ewTableHeader">UserName</td>
		<td<%= Image.ContactID.CellAttributes %>>
<div<%= Image.ContactID.ViewAttributes %>><%= Image.ContactID.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.medium.Visible Then ' medium %>
	<tr<%= Image.medium.RowAttributes %>>
		<td class="ewTableHeader">Medium</td>
		<td<%= Image.medium.CellAttributes %>>
<div<%= Image.medium.ViewAttributes %>><%= Image.medium.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.size.Visible Then ' size %>
	<tr<%= Image.size.RowAttributes %>>
		<td class="ewTableHeader">Size</td>
		<td<%= Image.size.CellAttributes %>>
<div<%= Image.size.ViewAttributes %>><%= Image.size.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.price.Visible Then ' price %>
	<tr<%= Image.price.RowAttributes %>>
		<td class="ewTableHeader">Price</td>
		<td<%= Image.price.CellAttributes %>>
<div<%= Image.price.ViewAttributes %>><%= Image.price.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.color.Visible Then ' color %>
	<tr<%= Image.color.RowAttributes %>>
		<td class="ewTableHeader">Color</td>
		<td<%= Image.color.CellAttributes %>>
<div<%= Image.color.ViewAttributes %>><%= Image.color.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.subject.Visible Then ' subject %>
	<tr<%= Image.subject.RowAttributes %>>
		<td class="ewTableHeader">Subject</td>
		<td<%= Image.subject.CellAttributes %>>
<div<%= Image.subject.ViewAttributes %>><%= Image.subject.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If Image.sold.Visible Then ' sold %>
	<tr<%= Image.sold.RowAttributes %>>
		<td class="ewTableHeader">Sold</td>
		<td<%= Image.sold.CellAttributes %>>
<% If Convert.ToString(Image.sold.CurrentValue) = "1" Then %>
<input type="checkbox" value="<%= Image.sold.ViewValue %>" checked onclick="this.form.reset();" disabled="disabled" />
<% Else %>
<input type="checkbox" value="<%= Image.sold.ViewValue %>" onclick="this.form.reset();" disabled="disabled" />
<% End If %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If Image.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If Image_view.Pager Is Nothing Then Image_view.Pager = New cPrevNextPager(Image_view.lStartRec, Image_view.lDisplayRecs, Image_view.lTotalRecs) %>
<% If Image_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If Image_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If Image_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= Image_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If Image_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If Image_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= Image_view.PageUrl %>start=<%= Image_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= Image_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If Image_view.sSrchWhere = "0=101" Then %>
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
<% If Image.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
