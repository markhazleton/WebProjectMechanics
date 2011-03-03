<%@ Page ClassName="sitelink_srch" Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="sitelink_srch.aspx.vb" Inherits="sitelink_srch" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteLink_search = new ew_Page("SiteLink_search");
// page properties
SiteLink_search.PageID = "search"; // page ID
SiteLink_search.FormID = "fSiteLinksearch"; // form ID 
var EW_PAGE_ID = SiteLink_search.PageID; // for backward compatibility
// extend page with validate function for search
SiteLink_search.ValidateSearch = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (this.ValidateRequired) { 
		var infix = "";
		elm = fobj.elements["x" + infix + "_ID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.ID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_Ranks"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.Ranks.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_UserID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.UserID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.SiteCategoryID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryTypeID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.SiteCategoryTypeID.FldErrMsg) %>");
		elm = fobj.elements["x" + infix + "_SiteCategoryGroupID"];
		if (elm && elm.type != "hidden" && !ew_CheckInteger(elm.value)) // skip hidden field
			return ew_OnError(this, elm, "<%= ew_JsEncode2(SiteLink.SiteCategoryGroupID.FldErrMsg) %>");
		// Form Custom Validate event
		if (!this.Form_CustomValidate(fobj)) return false;
	} 
	for (var i=0;i<fobj.elements.length;i++) {
		var elem = fobj.elements[i];
		if (elem.name.substring(0,2) == "s_" || elem.name.substring(0,3) == "sv_")
			elem.value = "";
	}
	return true;
}
// extend page with Form_CustomValidate function
SiteLink_search.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_search.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_search.ValidateRequired = false; // no JavaScript validation
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
// To include another .js script, you can use, e.g.
// ew_ClientScriptInclude("my_javascript.js"); or
// ew_ClientScriptInclude("my_javascript.js", {onSuccess: function(o) { // your code here }});
//-->
</script>
<p><span class="aspnetmaker"><%= Language.Phrase("Search") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteLink.TableCaption %><br /><br />
<a href="<%= SiteLink.ReturnUrl %>"><%= Language.Phrase("BackToList") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteLink_search.DebugMsg) %>
<% SiteLink_search.ShowMessage() %>
<form name="fSiteLinksearch" id="fSiteLinksearch" method="post" onsubmit="this.action=location.pathname;return SiteLink_search.ValidateSearch(this);">
<p />
<input type="hidden" name="t" id="t" value="SiteLink" />
<input type="hidden" name="a_search" id="a_search" value="S" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.ID.FldCaption %></td>
		<td<%= SiteLink.ID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_ID" id="z_ID" value="=" /></span></td>
		<td<%= SiteLink.ID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ID" id="x_ID" title="<%= SiteLink.ID.FldTitle %>" value="<%= SiteLink.ID.EditValue %>"<%= SiteLink.ID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.CompanyID.FldCaption %></td>
		<td<%= SiteLink.CompanyID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CompanyID" id="z_CompanyID" value="=" /></span></td>
		<td<%= SiteLink.CompanyID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CompanyID" name="x_CompanyID" title="<%= SiteLink.CompanyID.FldTitle %>"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.AdvancedSearch.SearchValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.LinkTypeCD.FldCaption %></td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_LinkTypeCD" id="z_LinkTypeCD" value="LIKE" /></span></td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_LinkTypeCD" name="x_LinkTypeCD" title="<%= SiteLink.LinkTypeCD.FldTitle %>"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.AdvancedSearch.SearchValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Title.FldCaption %></td>
		<td<%= SiteLink.Title.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Title" id="z_Title" value="LIKE" /></span></td>
		<td<%= SiteLink.Title.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Title" id="x_Title" title="<%= SiteLink.Title.FldTitle %>" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Description.FldCaption %></td>
		<td<%= SiteLink.Description.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_Description" id="z_Description" value="LIKE" /></span></td>
		<td<%= SiteLink.Description.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_Description" id="x_Description" title="<%= SiteLink.Description.FldTitle %>" cols="35" rows="4"<%= SiteLink.Description.EditAttributes %>><%= SiteLink.Description.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.DateAdd.FldCaption %></td>
		<td<%= SiteLink.DateAdd.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_DateAdd" id="z_DateAdd" value="=" /></span></td>
		<td<%= SiteLink.DateAdd.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_DateAdd" id="x_DateAdd" title="<%= SiteLink.DateAdd.FldTitle %>" value="<%= SiteLink.DateAdd.EditValue %>"<%= SiteLink.DateAdd.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Ranks.FldCaption %></td>
		<td<%= SiteLink.Ranks.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Ranks" id="z_Ranks" value="=" /></span></td>
		<td<%= SiteLink.Ranks.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_Ranks" id="x_Ranks" title="<%= SiteLink.Ranks.FldTitle %>" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Views.FldCaption %></td>
		<td<%= SiteLink.Views.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_Views" id="z_Views" value="=" /></span></td>
		<td<%= SiteLink.Views.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<%
If ew_SameStr(SiteLink.Views.AdvancedSearch.SearchValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" title="<%= SiteLink.Views.FldTitle %>" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.UserName.FldCaption %></td>
		<td<%= SiteLink.UserName.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_UserName" id="z_UserName" value="LIKE" /></span></td>
		<td<%= SiteLink.UserName.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_UserName" id="x_UserName" title="<%= SiteLink.UserName.FldTitle %>" size="30" maxlength="50" value="<%= SiteLink.UserName.EditValue %>"<%= SiteLink.UserName.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.UserID.FldCaption %></td>
		<td<%= SiteLink.UserID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_UserID" id="z_UserID" value="=" /></span></td>
		<td<%= SiteLink.UserID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_UserID" id="x_UserID" title="<%= SiteLink.UserID.FldTitle %>" size="30" value="<%= SiteLink.UserID.EditValue %>"<%= SiteLink.UserID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.ASIN.FldCaption %></td>
		<td<%= SiteLink.ASIN.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_ASIN" id="z_ASIN" value="LIKE" /></span></td>
		<td<%= SiteLink.ASIN.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_ASIN" id="x_ASIN" title="<%= SiteLink.ASIN.FldTitle %>" size="30" maxlength="50" value="<%= SiteLink.ASIN.EditValue %>"<%= SiteLink.ASIN.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.URL.FldCaption %></td>
		<td<%= SiteLink.URL.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("LIKE") %><input type="hidden" name="z_URL" id="z_URL" value="LIKE" /></span></td>
		<td<%= SiteLink.URL.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<textarea name="x_URL" id="x_URL" title="<%= SiteLink.URL.FldTitle %>" cols="35" rows="4"<%= SiteLink.URL.EditAttributes %>><%= SiteLink.URL.EditValue %></textarea>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.CategoryID.FldCaption %></td>
		<td<%= SiteLink.CategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_CategoryID" id="z_CategoryID" value="=" /></span></td>
		<td<%= SiteLink.CategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<select id="x_CategoryID" name="x_CategoryID" title="<%= SiteLink.CategoryID.FldTitle %>"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.AdvancedSearch.SearchValue) Then
			selwrk = " selected=""selected"""
			emptywrk = False
		Else
			selwrk = ""
		End If
%>
<option value="<%= ew_HtmlEncode(arwrk(rowcntwrk)(0)) %>"<%= selwrk %>>
<%= arwrk(rowcntwrk)(1) %>
</option>
<%
	Next
End If
%>
</select>
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryID" id="z_SiteCategoryID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryID" id="x_SiteCategoryID" title="<%= SiteLink.SiteCategoryID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryID.EditValue %>"<%= SiteLink.SiteCategoryID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryTypeID" id="z_SiteCategoryTypeID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" title="<%= SiteLink.SiteCategoryTypeID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryTypeID.EditValue %>"<%= SiteLink.SiteCategoryTypeID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>><span class="ewSearchOpr"><%= Language.Phrase("=") %><input type="hidden" name="z_SiteCategoryGroupID" id="z_SiteCategoryGroupID" value="=" /></span></td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>>
			<table cellspacing="0" class="ewItemTable"><tr>
				<td><span class="aspnetmaker">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= SiteLink.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryGroupID.EditValue %>"<%= SiteLink.SiteCategoryGroupID.EditAttributes %> />
</span></td>
			</tr></table>
		</td>
	</tr>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="Action" id="Action" value="<%= ew_BtnCaption(Language.Phrase("Search")) %>" />
<input type="button" name="Reset" id="Reset" value="<%= ew_BtnCaption(Language.Phrase("Reset")) %>" onclick="ew_ClearForm(this.form);" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
