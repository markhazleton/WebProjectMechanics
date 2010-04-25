<%@ Page Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="SiteCategoryType_add.aspx.vb" Inherits="SiteCategoryType_add" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteCategoryType_add = new ew_Page("SiteCategoryType_add");
// page properties
SiteCategoryType_add.PageID = "add"; // page ID
var EW_PAGE_ID = SiteCategoryType_add.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteCategoryType_add.ValidateForm = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
SiteCategoryType_add.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteCategoryType_add.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<script type="text/javascript">
<!--
var ew_DHTMLEditors = [];
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your client script here, no need to add script tags.
// To include another .js script, use:
// ew_ClientScriptInclude("my_javascript.js"); 
//-->
</script>
<p><span class="aspnetmaker">Add to TABLE: Site Type<br /><br />
<a href="<%= SiteCategoryType.ReturnUrl %>">Go Back</a></span></p>
<% SiteCategoryType_add.ShowMessage() %>
<form name="fSiteCategoryTypeadd" id="fSiteCategoryTypeadd" method="post" onsubmit="this.action=location.pathname;return SiteCategoryType_add.ValidateForm(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteCategoryType" />
<input type="hidden" name="a_add" id="a_add" value="A" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteCategoryType.SiteCategoryTypeNM.Visible Then ' SiteCategoryTypeNM %>
	<tr<%= SiteCategoryType.SiteCategoryTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Site Type</td>
		<td<%= SiteCategoryType.SiteCategoryTypeNM.CellAttributes %>><span id="el_SiteCategoryTypeNM">
<input type="text" name="x_SiteCategoryTypeNM" id="x_SiteCategoryTypeNM" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeNM.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeNM.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTypeNM.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTypeDS.Visible Then ' SiteCategoryTypeDS %>
	<tr<%= SiteCategoryType.SiteCategoryTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Description</td>
		<td<%= SiteCategoryType.SiteCategoryTypeDS.CellAttributes %>><span id="el_SiteCategoryTypeDS">
<input type="text" name="x_SiteCategoryTypeDS" id="x_SiteCategoryTypeDS" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTypeDS.EditValue %>"<%= SiteCategoryType.SiteCategoryTypeDS.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTypeDS.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryComment.Visible Then ' SiteCategoryComment %>
	<tr<%= SiteCategoryType.SiteCategoryComment.RowAttributes %>>
		<td class="ewTableHeader">Comment</td>
		<td<%= SiteCategoryType.SiteCategoryComment.CellAttributes %>><span id="el_SiteCategoryComment">
<input type="text" name="x_SiteCategoryComment" id="x_SiteCategoryComment" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryComment.EditValue %>"<%= SiteCategoryType.SiteCategoryComment.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryComment.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryFileName.Visible Then ' SiteCategoryFileName %>
	<tr<%= SiteCategoryType.SiteCategoryFileName.RowAttributes %>>
		<td class="ewTableHeader">File Name</td>
		<td<%= SiteCategoryType.SiteCategoryFileName.CellAttributes %>><span id="el_SiteCategoryFileName">
<input type="text" name="x_SiteCategoryFileName" id="x_SiteCategoryFileName" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryFileName.EditValue %>"<%= SiteCategoryType.SiteCategoryFileName.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryFileName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.SiteCategoryTransferURL.Visible Then ' SiteCategoryTransferURL %>
	<tr<%= SiteCategoryType.SiteCategoryTransferURL.RowAttributes %>>
		<td class="ewTableHeader">Transfer URL</td>
		<td<%= SiteCategoryType.SiteCategoryTransferURL.CellAttributes %>><span id="el_SiteCategoryTransferURL">
<input type="text" name="x_SiteCategoryTransferURL" id="x_SiteCategoryTransferURL" size="30" maxlength="255" value="<%= SiteCategoryType.SiteCategoryTransferURL.EditValue %>"<%= SiteCategoryType.SiteCategoryTransferURL.EditAttributes %> />
</span><%= SiteCategoryType.SiteCategoryTransferURL.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteCategoryType.DefaultSiteCategoryID.Visible Then ' DefaultSiteCategoryID %>
	<tr<%= SiteCategoryType.DefaultSiteCategoryID.RowAttributes %>>
		<td class="ewTableHeader">Default Category</td>
		<td<%= SiteCategoryType.DefaultSiteCategoryID.CellAttributes %>><span id="el_DefaultSiteCategoryID">
<select id="x_DefaultSiteCategoryID" name="x_DefaultSiteCategoryID"<%= SiteCategoryType.DefaultSiteCategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteCategoryType.DefaultSiteCategoryID.EditValue) Then
	arwrk = SiteCategoryType.DefaultSiteCategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteCategoryType.DefaultSiteCategoryID.CurrentValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
<% If ew_NotEmpty(arwrk(rowcntwrk)(2)) Then %>
<%= ew_ValueSeparator(rowcntwrk) %><%= arwrk(rowcntwrk)(2) %>
<% End If %>
</option>
<%
	Next
End If
%>
</select>
<%
sSqlWrk = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory]"
sWhereWrk = ""
If sWhereWrk <> "" Then sSqlWrk = sSqlWrk & " WHERE " & sWhereWrk
sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeID] Asc"
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
%>
<input type="hidden" name="s_x_DefaultSiteCategoryID" id="s_x_DefaultSiteCategoryID" value="<%= sSqlWrk %>">
<input type="hidden" name="lft_x_DefaultSiteCategoryID" id="lft_x_DefaultSiteCategoryID" value="">
</span><%= SiteCategoryType.DefaultSiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value=" Save New " />
</form>
<script language="JavaScript">
<!--
ew_UpdateOpts([['x_DefaultSiteCategoryID','x_DefaultSiteCategoryID',false]]);
//-->
</script>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
