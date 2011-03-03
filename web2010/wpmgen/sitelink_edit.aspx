<%@ Page ClassName="sitelink_edit" Language="VB" MasterPageFile="masterpage.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="sitelink_edit.aspx.vb" Inherits="sitelink_edit" CodeFileBaseClass="AspNetMaker8_wpmWebsite" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<script type="text/javascript">
<!--
// Create page object
var SiteLink_edit = new ew_Page("SiteLink_edit");
// page properties
SiteLink_edit.PageID = "edit"; // page ID
SiteLink_edit.FormID = "fSiteLinkedit"; // form ID 
var EW_PAGE_ID = SiteLink_edit.PageID; // for backward compatibility
// extend page with ValidateForm function
SiteLink_edit.ValidateForm = function(fobj) {
	ew_PostAutoSuggest(fobj); 
	if (!this.ValidateRequired)
		return true; // ignore validation
	if (fobj.a_confirm && fobj.a_confirm.value == "F")
		return true;
	var i, elm, aelm, infix;
	var rowcnt = (fobj.key_count) ? Number(fobj.key_count.value) : 1;
	for (i=0; i<rowcnt; i++) {
		infix = (fobj.key_count) ? String(i+1) : "";
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
	return true;
}
// extend page with Form_CustomValidate function
SiteLink_edit.Form_CustomValidate =  
 function(fobj) { // DO NOT CHANGE THIS LINE!
 	// Your custom validation code here, return false if invalid. 
 	return true;
 }
<% If EW_CLIENT_VALIDATE Then %>
SiteLink_edit.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteLink_edit.ValidateRequired = false; // no JavaScript validation
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
<p><span class="aspnetmaker"><%= Language.Phrase("Edit") %>&nbsp;<%= Language.Phrase("TblTypeTABLE") %><%= SiteLink.TableCaption %><br /><br />
<a href="<%= SiteLink.ReturnUrl %>"><%= Language.Phrase("GoBack") %></a></span></p>
<% If EW_DEBUG_ENABLED Then HttpContext.Current.Response.Write(SiteLink_edit.DebugMsg) %>
<% SiteLink_edit.ShowMessage() %>
<form name="fSiteLinkedit" id="fSiteLinkedit" method="post" onsubmit="this.action=location.pathname;return SiteLink_edit.ValidateForm(this);">
<p />
<input type="hidden" name="a_table" id="a_table" value="SiteLink" />
<input type="hidden" name="a_edit" id="a_edit" value="U" />
<table cellspacing="0" class="ewGrid"><tr><td class="ewGridContent">
<div class="ewGridMiddlePanel">
<table cellspacing="0" class="ewTable">
<% If SiteLink.ID.Visible Then ' ID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.ID.FldCaption %></td>
		<td<%= SiteLink.ID.CellAttributes %>><span id="el_ID">
<div<%= SiteLink.ID.ViewAttributes %>><%= SiteLink.ID.EditValue %></div>
<input type="hidden" name="x_ID" id="x_ID" value="<%= ew_HTMLEncode(SiteLink.ID.CurrentValue) %>" />
</span><%= SiteLink.ID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.CompanyID.Visible Then ' CompanyID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.CompanyID.FldCaption %></td>
		<td<%= SiteLink.CompanyID.CellAttributes %>><span id="el_CompanyID">
<select id="x_CompanyID" name="x_CompanyID" title="<%= SiteLink.CompanyID.FldTitle %>"<%= SiteLink.CompanyID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CompanyID.EditValue) Then
	arwrk = SiteLink.CompanyID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CompanyID.CurrentValue) Then
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
</span><%= SiteLink.CompanyID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.LinkTypeCD.Visible Then ' LinkTypeCD %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.LinkTypeCD.FldCaption %></td>
		<td<%= SiteLink.LinkTypeCD.CellAttributes %>><span id="el_LinkTypeCD">
<select id="x_LinkTypeCD" name="x_LinkTypeCD" title="<%= SiteLink.LinkTypeCD.FldTitle %>"<%= SiteLink.LinkTypeCD.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.LinkTypeCD.EditValue) Then
	arwrk = SiteLink.LinkTypeCD.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.LinkTypeCD.CurrentValue) Then
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
</span><%= SiteLink.LinkTypeCD.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Title.Visible Then ' Title %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Title.FldCaption %></td>
		<td<%= SiteLink.Title.CellAttributes %>><span id="el_Title">
<input type="text" name="x_Title" id="x_Title" title="<%= SiteLink.Title.FldTitle %>" size="30" maxlength="255" value="<%= SiteLink.Title.EditValue %>"<%= SiteLink.Title.EditAttributes %> />
</span><%= SiteLink.Title.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Description.Visible Then ' Description %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Description.FldCaption %></td>
		<td<%= SiteLink.Description.CellAttributes %>><span id="el_Description">
<textarea name="x_Description" id="x_Description" title="<%= SiteLink.Description.FldTitle %>" cols="35" rows="4"<%= SiteLink.Description.EditAttributes %>><%= SiteLink.Description.EditValue %></textarea>
</span><%= SiteLink.Description.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.DateAdd.Visible Then ' DateAdd %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.DateAdd.FldCaption %></td>
		<td<%= SiteLink.DateAdd.CellAttributes %>><span id="el_DateAdd">
<input type="text" name="x_DateAdd" id="x_DateAdd" title="<%= SiteLink.DateAdd.FldTitle %>" value="<%= SiteLink.DateAdd.EditValue %>"<%= SiteLink.DateAdd.EditAttributes %> />
</span><%= SiteLink.DateAdd.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Ranks.Visible Then ' Ranks %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Ranks.FldCaption %></td>
		<td<%= SiteLink.Ranks.CellAttributes %>><span id="el_Ranks">
<input type="text" name="x_Ranks" id="x_Ranks" title="<%= SiteLink.Ranks.FldTitle %>" size="30" value="<%= SiteLink.Ranks.EditValue %>"<%= SiteLink.Ranks.EditAttributes %> />
</span><%= SiteLink.Ranks.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.Views.Visible Then ' Views %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.Views.FldCaption %><%= Language.Phrase("FieldRequiredIndicator") %></td>
		<td<%= SiteLink.Views.CellAttributes %>><span id="el_Views">
<%
If ew_SameStr(SiteLink.Views.CurrentValue, "1") Then
	selwrk = " checked=""checked"""
Else
	selwrk = ""
End If
%>
<input type="checkbox" name="x_Views" id="x_Views" title="<%= SiteLink.Views.FldTitle %>" value="1"<%= selwrk %><%= SiteLink.Views.EditAttributes %> />
</span><%= SiteLink.Views.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.UserName.Visible Then ' UserName %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.UserName.FldCaption %></td>
		<td<%= SiteLink.UserName.CellAttributes %>><span id="el_UserName">
<input type="text" name="x_UserName" id="x_UserName" title="<%= SiteLink.UserName.FldTitle %>" size="30" maxlength="50" value="<%= SiteLink.UserName.EditValue %>"<%= SiteLink.UserName.EditAttributes %> />
</span><%= SiteLink.UserName.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.UserID.Visible Then ' UserID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.UserID.FldCaption %></td>
		<td<%= SiteLink.UserID.CellAttributes %>><span id="el_UserID">
<input type="text" name="x_UserID" id="x_UserID" title="<%= SiteLink.UserID.FldTitle %>" size="30" value="<%= SiteLink.UserID.EditValue %>"<%= SiteLink.UserID.EditAttributes %> />
</span><%= SiteLink.UserID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.ASIN.Visible Then ' ASIN %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.ASIN.FldCaption %></td>
		<td<%= SiteLink.ASIN.CellAttributes %>><span id="el_ASIN">
<input type="text" name="x_ASIN" id="x_ASIN" title="<%= SiteLink.ASIN.FldTitle %>" size="30" maxlength="50" value="<%= SiteLink.ASIN.EditValue %>"<%= SiteLink.ASIN.EditAttributes %> />
</span><%= SiteLink.ASIN.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.URL.Visible Then ' URL %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.URL.FldCaption %></td>
		<td<%= SiteLink.URL.CellAttributes %>><span id="el_URL">
<textarea name="x_URL" id="x_URL" title="<%= SiteLink.URL.FldTitle %>" cols="35" rows="4"<%= SiteLink.URL.EditAttributes %>><%= SiteLink.URL.EditValue %></textarea>
</span><%= SiteLink.URL.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.CategoryID.Visible Then ' CategoryID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.CategoryID.FldCaption %></td>
		<td<%= SiteLink.CategoryID.CellAttributes %>><span id="el_CategoryID">
<select id="x_CategoryID" name="x_CategoryID" title="<%= SiteLink.CategoryID.FldTitle %>"<%= SiteLink.CategoryID.EditAttributes %>>
<%
emptywrk = True
If ew_IsArrayList(SiteLink.CategoryID.EditValue) Then
	arwrk = SiteLink.CategoryID.EditValue
	For rowcntwrk As Integer = 0 To arwrk.Count - 1
		If ew_SameStr(arwrk(rowcntwrk)(0), SiteLink.CategoryID.CurrentValue) Then
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
</span><%= SiteLink.CategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryID.Visible Then ' SiteCategoryID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryID.CellAttributes %>><span id="el_SiteCategoryID">
<input type="text" name="x_SiteCategoryID" id="x_SiteCategoryID" title="<%= SiteLink.SiteCategoryID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryID.EditValue %>"<%= SiteLink.SiteCategoryID.EditAttributes %> />
</span><%= SiteLink.SiteCategoryID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryTypeID.Visible Then ' SiteCategoryTypeID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryTypeID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryTypeID.CellAttributes %>><span id="el_SiteCategoryTypeID">
<input type="text" name="x_SiteCategoryTypeID" id="x_SiteCategoryTypeID" title="<%= SiteLink.SiteCategoryTypeID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryTypeID.EditValue %>"<%= SiteLink.SiteCategoryTypeID.EditAttributes %> />
</span><%= SiteLink.SiteCategoryTypeID.CustomMsg %></td>
	</tr>
<% End If %>
<% If SiteLink.SiteCategoryGroupID.Visible Then ' SiteCategoryGroupID %>
	<tr<%= SiteLink.RowAttributes %>>
		<td class="ewTableHeader"><%= SiteLink.SiteCategoryGroupID.FldCaption %></td>
		<td<%= SiteLink.SiteCategoryGroupID.CellAttributes %>><span id="el_SiteCategoryGroupID">
<input type="text" name="x_SiteCategoryGroupID" id="x_SiteCategoryGroupID" title="<%= SiteLink.SiteCategoryGroupID.FldTitle %>" size="30" value="<%= SiteLink.SiteCategoryGroupID.EditValue %>"<%= SiteLink.SiteCategoryGroupID.EditAttributes %> />
</span><%= SiteLink.SiteCategoryGroupID.CustomMsg %></td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<p />
<input type="submit" name="btnAction" id="btnAction" value="<%= ew_BtnCaption(Language.Phrase("EditBtn")) %>" />
</form>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
</asp:Content>
