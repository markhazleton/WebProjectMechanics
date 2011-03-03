<%@ Page ClassName="contact_delete" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="contact_delete.aspx.vb" Inherits="contact_delete" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var Contact_delete = new ew_Page("Contact_delete");
// page properties
Contact_delete.PageID = "delete"; // page ID
Contact_delete.FormID = "fContactdelete"; // form ID 
var EW_PAGE_ID = Contact_delete.PageID; // for backward compatibility
// extend page with Form_CustomValidate function
Contact_delete.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
Contact_delete.ValidateRequired = true; // uses JavaScript validation
<% Else %>
Contact_delete.ValidateRequired = false; // no JavaScript validation
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
Rs = Contact_delete.LoadRecordset()
If Contact_delete.lTotalRecs <= 0 Then ' No record found, exit
	Rs.Close()
	Rs.Dispose()
	Contact_delete.Page_Terminate("contact_list.aspx") ' Return to list
End If
%>
<p><span class="aspnetmaker"><%= Language.Phrase("Delete") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= Contact.TableCaption %><br /><br />
<a href="<%= Contact.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(Contact_delete.DebugMsg) %>
<% Contact_delete.ShowMessage() %>
<form method="post">
<p />
<input type="hidden" name="t" id="t" value="Contact" />
<input type="hidden" name="a_delete" id="a_delete" value="D" />
<% For i As Integer = 0 to Contact_delete.arRecKeys.GetUpperBound(0) %>
<input type="hidden" name="key_m" id="key_m" value="<%= Server.HtmlEncode(Convert.ToString(Contact_delete.arRecKeys(i))) %>" />
<% Next %>
<table class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable ewTableSeparate">
<%= Contact.TableCustomInnerHTML %>
	<thead>
	<tr class="ewTableHeader">
		<td valign="top"><%= Contact.LogonName.FldCaption %></td>
		<td valign="top"><%= Contact.CompanyID.FldCaption %></td>
		<td valign="top"><%= Contact.zEMail.FldCaption %></td>
		<td valign="top"><%= Contact.PrimaryContact.FldCaption %></td>
	</tr>
	</thead>
	<tbody>
<%
Contact_delete.lRecCnt = 0
Do While Rs.Read()
	Contact_delete.lRecCnt = Contact_delete.lRecCnt + 1

	' Set row properties
	Contact.CssClass = ""
	Contact.CssStyle = ""
	Contact.RowType = EW_ROWTYPE_VIEW ' view

	' Get the field contents
	Contact_delete.LoadRowValues(Rs)

	' Render row
	Contact_delete.RenderRow()
%>
	<tr<%= Contact.RowAttributes %>>
		<td<%= Contact.LogonName.CellAttributes %>>
<div<%= Contact.LogonName.ViewAttributes %>><%= Contact.LogonName.ListViewValue %></div>
</td>
		<td<%= Contact.CompanyID.CellAttributes %>>
<div<%= Contact.CompanyID.ViewAttributes %>><%= Contact.CompanyID.ListViewValue %></div>
</td>
		<td<%= Contact.zEMail.CellAttributes %>>
<div<%= Contact.zEMail.ViewAttributes %>><%= Contact.zEMail.ListViewValue %></div>
</td>
		<td<%= Contact.PrimaryContact.CellAttributes %>>
<div<%= Contact.PrimaryContact.ViewAttributes %>><%= Contact.PrimaryContact.ListViewValue %></div>
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
