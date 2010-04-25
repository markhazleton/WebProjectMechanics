<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="PageType_srch.aspx.vb" Inherits="PageType_srch" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var PageType_search = new ew_Page("PageType_search");
// page properties
PageType_search.PageID = "search"; // page ID
var EW_PAGE_ID = PageType_search.PageID; // for backward compatibility
// extend page with validate function for search
PageType_search.ValidateSearch = function(fobj) {
	if (!this.ValidateRequired)
		return true; // ignore validation
	var infix = "";
	elm = fobj.elements["x" + infix + "_PageTypeID"];
	if (elm && !ew_CheckInteger(elm.value))
		return ew_OnError(this, elm, "Incorrect integer - Page Type ID");
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
<% If EW_CLIENT_VALIDATE Then %>
PageType_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
PageType_search.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker">Search TABLE: Location Type<br /><br />
<a href="<%= PageType.ReturnUrl %>">Back to List</a></span></p>
<% PageType_search.ShowMessage() %>
<form name="fPageTypesearch" id="fPageTypesearch" method="post" onsubmit="this.action=location.pathname;return PageType_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="PageType" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= PageType.PageTypeID.RowAttributes %>>
		<td class="ewTableHeader">Page Type ID</td>
		<td<%= PageType.PageTypeID.CellAttributes %>><span class="ewSearchOpr">=<input type="hidden" name="z_PageTypeID" id="z_PageTypeID" value="=" /></span></td>
		<td<%= PageType.PageTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeID" id="x_PageTypeID" value="<%= PageType.PageTypeID.EditValue %>"<%= PageType.PageTypeID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.PageTypeCD.RowAttributes %>>
		<td class="ewTableHeader">Page Type CD</td>
		<td<%= PageType.PageTypeCD.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageTypeCD" id="z_PageTypeCD" value="LIKE" /></span></td>
		<td<%= PageType.PageTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeCD" id="x_PageTypeCD" size="30" maxlength="50" value="<%= PageType.PageTypeCD.EditValue %>"<%= PageType.PageTypeCD.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.PageTypeDesc.RowAttributes %>>
		<td class="ewTableHeader">Page Type Desc</td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageTypeDesc" id="z_PageTypeDesc" value="LIKE" /></span></td>
		<td<%= PageType.PageTypeDesc.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageTypeDesc" id="x_PageTypeDesc" size="30" maxlength="50" value="<%= PageType.PageTypeDesc.EditValue %>"<%= PageType.PageTypeDesc.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= PageType.PageFileName.RowAttributes %>>
		<td class="ewTableHeader">Page File Name</td>
		<td<%= PageType.PageFileName.CellAttributes %>><span class="ewSearchOpr">contains<input type="hidden" name="z_PageFileName" id="z_PageFileName" value="LIKE" /></span></td>
		<td<%= PageType.PageFileName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_PageFileName" id="x_PageFileName" size="30" maxlength="50" value="<%= PageType.PageFileName.EditValue %>"<%= PageType.PageFileName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="  Search  " />
<input type="button" name="Reset" id="Reset" value="   Reset   " onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
