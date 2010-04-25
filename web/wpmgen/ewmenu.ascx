<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ewmenu.ascx.vb" Inherits="ewmenu" %>
<!-- Begin Main Menu -->
<div class="aspnetmaker">
<%
RootMenu.Render()
%>
</div>
<!-- End Main Menu -->
<script type="text/javascript">
<!--
var RootMenu = new Spry.Widget.MenuBar("RootMenu", {imgRight: "/wpmgen/images/SpryMenuBarRightHover.gif"}); // Main menu 
//-->
</script>
