<%@ Control ClassName="ewmenu" Language="VB" AutoEventWireup="false" CodeFile="ewmenu.ascx.vb" Inherits="ewmenu" %>
<!-- Begin Main Menu -->
<div class="aspnetmaker">
<%
If not IsNothing(RootMenu) Then 
  RootMenu.Render()
End If 
%>
</div>
<!-- End Main Menu -->
<script type="text/javascript">
<!--
// append space to menu item

function ew_SetupHorizontalMenuLink(a) {
	if (a.innerHTML != "") {
		var c = a.cloneNode(true);
		c.innerHTML += "&nbsp;";
		a.parentNode.replaceChild(c, a);
	}
}
// make room for the down arrow (horizontal menu only)
ewDom.getElementsByClassName("MenuBarItemSubmenu", "A", ewDom.get("RootMenu"), ew_SetupHorizontalMenuLink);
// init the menu
var RootMenu = new Spry.Widget.MenuBar("RootMenu", {imgDown:"images/SpryMenuBarDownHover.gif", imgRight:"images/SpryMenuBarRightHover.gif"});
//-->
</script>
