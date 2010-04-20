<%@ Page Language="VB" MasterPageFile="masterpage.master" AutoEventWireup="false" CodeFile="SiteParameterType_view.aspx.vb" Inherits="SiteParameterType_view" CodeFileBaseClass="AspNetMaker7_WPMGen" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
<% If SiteParameterType.Export = "" Then %>
<script type="text/javascript">
<!--
// Create page object
var SiteParameterType_view = new ew_Page("SiteParameterType_view");
// page properties
SiteParameterType_view.PageID = "view"; // page ID
var EW_PAGE_ID = SiteParameterType_view.PageID; // for backward compatibility
<% If EW_CLIENT_VALIDATE Then %>
SiteParameterType_view.ValidateRequired = true; // uses JavaScript validation
<% Else %>
SiteParameterType_view.ValidateRequired = false; // no JavaScript validation
<% End If %>
//-->
</script>
<style>
	/* styles for details panel */
	.yui-overlay { position:absolute;background:#fff;border:2px solid orange;padding:4px;margin:10px; }
	.yui-overlay .hd { border:1px solid red;padding:5px; }
	.yui-overlay .bd { border:0px solid green;padding:5px; }
	.yui-overlay .ft { border:1px solid blue;padding:5px; }
</style>
<div id="ewDetailsDiv" name="ewDetailsDivDiv" style="visibility:hidden"></div>
<script language="JavaScript" type="text/javascript">
<!--
// YUI container
var ewDetailsDiv;
var ew_AjaxDetailsTimer = null;
// init details div

function ew_InitDetailsDiv() {
	ewDetailsDiv = new YAHOO.widget.Overlay("ewDetailsDiv", { context:null, visible:false} );
	ewDetailsDiv.beforeMoveEvent.subscribe(ew_EnforceConstraints, ewDetailsDiv, true);
	ewDetailsDiv.render();
}
// init details div on window.load
YAHOO.util.Event.addListener(window, "load", ew_InitDetailsDiv);
// show results in details div
var ew_AjaxHandleSuccess = function(o) {
	if (ewDetailsDiv && o.responseText !== undefined) {
		ewDetailsDiv.cfg.applyConfig({context:[o.argument.id,o.argument.elcorner,o.argument.ctxcorner], visible:false}, true);
		ewDetailsDiv.setBody(o.responseText);
		ewDetailsDiv.render();
		ew_SetupTable(document.getElementById("ewDetailsPreviewTable"));
		ewDetailsDiv.show();
	}
}
// show error in details div
var ew_AjaxHandleFailure = function(o) {
	if (ewDetailsDiv && o.responseText != "") {
		ewDetailsDiv.cfg.applyConfig({context:[o.argument.id,o.argument.elcorner,o.argument.ctxcorner], visible:false, constraintoviewport:true}, true);
		ewDetailsDiv.setBody(o.responseText);
		ewDetailsDiv.render();
		ewDetailsDiv.show();
	}
}
// show details div

function ew_AjaxShowDetails(obj, url) {
	if (ew_AjaxDetailsTimer)
		clearTimeout(ew_AjaxDetailsTimer);
	ew_AjaxDetailsTimer = setTimeout(function() { YAHOO.util.Connect.asyncRequest('GET', url, {success: ew_AjaxHandleSuccess , failure: ew_AjaxHandleFailure, argument:{id: obj.id, elcorner: "tl", ctxcorner: "tr"}}) }, 200);
}
// hide details div

function ew_AjaxHideDetails(obj) {
	if (ew_AjaxDetailsTimer)
		clearTimeout(ew_AjaxDetailsTimer);
	if (ewDetailsDiv)
		ewDetailsDiv.hide();
}
// move details div
ew_EnforceConstraints = function(type, args, obj) {
	var pos = args[0];
	var x = pos[0];
	var y = pos[1];
	var offsetHeight = this.element.offsetHeight;
	var offsetWidth = this.element.offsetWidth;
	var viewPortWidth = YAHOO.util.Dom.getViewportWidth();
	var viewPortHeight = YAHOO.util.Dom.getViewportHeight();
	var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
	var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
	var topConstraint = scrollY + 10;
	var leftConstraint = scrollX + 10;
	var bottomConstraint = scrollY + viewPortHeight - offsetHeight - 10;
	var rightConstraint = scrollX + viewPortWidth - offsetWidth - 10;
// if (x < leftConstraint) {
// x = leftConstraint;
// } else if (x > rightConstraint) {
// x = rightConstraint;
// }
	if (y < topConstraint) {
		y = topConstraint;
	} else if (y > bottomConstraint) {
		y = (bottomConstraint < topConstraint) ? topConstraint : bottomConstraint;
	}
// this.cfg.setProperty("x", x, true);
	this.cfg.setProperty("y", y, true);
	this.cfg.setProperty("xy", [x,y], true);
};
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
<p><span class="aspnetmaker">View TABLE: Parameter Type
<br /><br />
<% If SiteParameterType.Export = "" Then %>
<a href="SiteParameterType_list.aspx">Back to List</a>&nbsp;
<a href="<%= SiteParameterType.AddUrl %>">Add</a>&nbsp;
<a href="<%= SiteParameterType.EditUrl %>">Edit</a>&nbsp;
<a href="<%= SiteParameterType.CopyUrl %>">Copy</a>&nbsp;
<a href="<%= SiteParameterType.DeleteUrl %>">Delete</a>&nbsp;
<%
sSqlWrk = "[SiteParameterTypeID]=" & ew_AdjustSql(SiteParameterType.SiteParameterTypeID.CurrentValue) & ""
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
sSqlWrk = sSqlWrk.Replace("'", "\'")
%>
<a name="ew_SiteParameterType_CompanySiteParameter_DetailLink" id="ew_SiteParameterType_CompanySiteParameter_DetailLink" href="CompanySiteParameter_list.aspx?<%= EW_TABLE_SHOW_MASTER %>=SiteParameterType&SiteParameterTypeID=<%= Server.UrlEncode(Convert.ToString(SiteParameterType.SiteParameterTypeID.CurrentValue)) %>" onmouseover="ew_AjaxShowDetails(this, 'CompanySiteParameter_preview.aspx?f=<%= sSqlWrk %>')" onmouseout="ew_AjaxHideDetails(this);">Site Parameter...</a>
&nbsp;
<%
sSqlWrk = "[SiteCategoryTypeID]=" & ew_AdjustSql(SiteParameterType.SiteParameterTypeID.CurrentValue) & ""
sSqlWrk = cTEA.Encrypt(sSqlWrk, EW_RANDOM_KEY)
sSqlWrk = sSqlWrk.Replace("'", "\'")
%>
<a name="ew_SiteParameterType_CompanySiteTypeParameter_DetailLink" id="ew_SiteParameterType_CompanySiteTypeParameter_DetailLink" href="CompanySiteTypeParameter_list.aspx?<%= EW_TABLE_SHOW_MASTER %>=SiteParameterType&SiteParameterTypeID=<%= Server.UrlEncode(Convert.ToString(SiteParameterType.SiteParameterTypeID.CurrentValue)) %>" onmouseover="ew_AjaxShowDetails(this, 'CompanySiteTypeParameter_preview.aspx?f=<%= sSqlWrk %>')" onmouseout="ew_AjaxHideDetails(this);">Site Type Parameter...</a>
&nbsp;
<% End If %>
</span></p>
<% SiteParameterType_view.ShowMessage() %>
<p />
<% If SiteParameterType.Export = "" Then %>
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteParameterType_view.Pager Is Nothing Then SiteParameterType_view.Pager = New cPrevNextPager(SiteParameterType_view.lStartRec, SiteParameterType_view.lDisplayRecs, SiteParameterType_view.lTotalRecs) %>
<% If SiteParameterType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteParameterType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteParameterType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteParameterType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteParameterType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteParameterType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteParameterType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteParameterType_view.sSrchWhere = "0=101" Then %>
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
<% If SiteParameterType.SiteParameterTypeNM.Visible Then ' SiteParameterTypeNM %>
	<tr<%= SiteParameterType.SiteParameterTypeNM.RowAttributes %>>
		<td class="ewTableHeader">Parameter Name</td>
		<td<%= SiteParameterType.SiteParameterTypeNM.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeNM.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeNM.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeDS.Visible Then ' SiteParameterTypeDS %>
	<tr<%= SiteParameterType.SiteParameterTypeDS.RowAttributes %>>
		<td class="ewTableHeader">Parameter Description</td>
		<td<%= SiteParameterType.SiteParameterTypeDS.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeDS.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeDS.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTypeOrder.Visible Then ' SiteParameterTypeOrder %>
	<tr<%= SiteParameterType.SiteParameterTypeOrder.RowAttributes %>>
		<td class="ewTableHeader">Parameter Order</td>
		<td<%= SiteParameterType.SiteParameterTypeOrder.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTypeOrder.ViewAttributes %>><%= SiteParameterType.SiteParameterTypeOrder.ViewValue %></div>
</td>
	</tr>
<% End If %>
<% If SiteParameterType.SiteParameterTemplate.Visible Then ' SiteParameterTemplate %>
	<tr<%= SiteParameterType.SiteParameterTemplate.RowAttributes %>>
		<td class="ewTableHeader">Parameter Template</td>
		<td<%= SiteParameterType.SiteParameterTemplate.CellAttributes %>>
<div<%= SiteParameterType.SiteParameterTemplate.ViewAttributes %>><%= SiteParameterType.SiteParameterTemplate.ViewValue %></div>
</td>
	</tr>
<% End If %>
</table>
</div>
</td></tr></table>
<% If SiteParameterType.Export = "" Then %>
<br />
<form name="ewpagerform" id="ewpagerform" class="ewForm">
<table border="0" cellspacing="0" cellpadding="0" class="ewPager">
	<tr>
		<td>
<% If SiteParameterType_view.Pager Is Nothing Then SiteParameterType_view.Pager = New cPrevNextPager(SiteParameterType_view.lStartRec, SiteParameterType_view.lDisplayRecs, SiteParameterType_view.lTotalRecs) %>
<% If SiteParameterType_view.Pager.RecordCount > 0 Then %>
	<table border="0" cellspacing="0" cellpadding="0"><tr><td><span class="aspnetmaker">Page&nbsp;</span></td>
<!--first page button-->
	<% If SiteParameterType_view.Pager.FirstButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.FirstButton.Start %>"><img src="images/first.gif" alt="First" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/firstdisab.gif" alt="First" width="16" height="16" border="0"></td>
	<% End If %>
<!--previous page button-->
	<% If SiteParameterType_view.Pager.PrevButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.PrevButton.Start %>"><img src="images/prev.gif" alt="Previous" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/prevdisab.gif" alt="Previous" width="16" height="16" border="0"></td>
	<% End If %>
<!--current page number-->
	<td><input type="text" name="<%= EW_TABLE_PAGE_NO %>" id="<%= EW_TABLE_PAGE_NO %>" value="<%= SiteParameterType_view.Pager.CurrentPage %>" size="4" /></td>
<!--next page button-->
	<% If SiteParameterType_view.Pager.NextButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.NextButton.Start %>"><img src="images/next.gif" alt="Next" width="16" height="16" border="0"></a></td>
	<% Else %>
	<td><img src="images/nextdisab.gif" alt="Next" width="16" height="16" border="0"></td>
	<% End If %>
<!--last page button-->
	<% If SiteParameterType_view.Pager.LastButton.Enabled Then %>
	<td><a href="<%= SiteParameterType_view.PageUrl %>start=<%= SiteParameterType_view.Pager.LastButton.Start %>"><img src="images/last.gif" alt="Last" width="16" height="16" border="0"></a></td>	
	<% Else %>
	<td><img src="images/lastdisab.gif" alt="Last" width="16" height="16" border="0"></td>
	<% End If %>
	<td><span class="aspnetmaker">&nbsp;of <%= SiteParameterType_view.Pager.PageCount %></span></td>
	</tr></table>
<% Else %>
	<% If SiteParameterType_view.sSrchWhere = "0=101" Then %>
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
<% If SiteParameterType.Export = "" Then %>
<script language="JavaScript" type="text/javascript">
<!--
// Write your table-specific startup script here
// document.write("page loaded");
//-->
</script>
<% End If %>
</asp:Content>
