<%@ Page Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false"
    CodeFile="ApplicationAdmin.aspx.vb" Inherits="wpm_admin_ApplicationAdmin" Title="Global Settings | Web Project Mechanics " %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <form id="form1" runat="server">
    <asp:CheckBox ID="cbCachingEnabled" runat="server" AutoPostBack="true" Text=" Application Caching " /><br />
    <asp:CheckBox ID="cbUse404Processing" runat="server" AutoPostBack="true" Text=" Use 404 Processing" /><br />
    <asp:CheckBox ID="cbFullLoggingOn" runat="server" AutoPostBack="true" Text=" Full Logging On" /><br />
    <asp:CheckBox ID="cbRemoveWWW" runat="server" AutoPostBack="true" Text=" Remove WWW" />
    <hr />
    <br />
    <asp:Label ID="SiteList" runat="server" />
    <hr />
    <asp:Label ID="debug" runat="server" />
    </form>
    </asp:Content>
 