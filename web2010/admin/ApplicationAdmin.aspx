<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="ApplicationAdmin.aspx.vb" Inherits="Admin_ApplicationAdmin" Title="Global Settings | Web Project Mechanics " %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <div class="panel panel-default row">
        <div class="panel-heading">
            <i class="fa fa-bell fa-fw"></i>Application Configuration
        </div>
        <!-- /.panel-heading -->
        <div class="panel-body">
            <asp:LinkButton ID="lbResetCache" runat="server" Text="Reset Cache" CssClass="btn btn-primary" OnClick="lbResetCache_Click"></asp:LinkButton>
            <div class="list-group">
                <asp:CheckBox ID="cbCachingEnabled" runat="server" AutoPostBack="true" Text=" Application Caching " CssClass="list-group-item" />
                <asp:CheckBox ID="cbUse404Processing" runat="server" AutoPostBack="true" Text=" Use 404 Processing" CssClass="list-group-item" />
                <asp:CheckBox ID="cbFullLoggingOn" runat="server" AutoPostBack="true" Text=" Full Logging On" CssClass="list-group-item" />
                <asp:CheckBox ID="cbRemoveWWW" runat="server" AutoPostBack="true" Text=" Remove WWW" CssClass="list-group-item" />
            </div>
        </div>
        <!-- /.panel-body -->
    </div>
    <div class="panel panel-default row">
        <div class="panel-heading">
            <i class="fa fa-bell fa-fw"></i>Site Information
        </div>
        <!-- /.panel-heading -->
        <div class="panel-body">
            <asp:Literal ID="SiteInformation" runat="server" Text="Application Admin" />
            <asp:Literal ID="siteList" runat="server" />
        </div>
    </div>
    <div class="panel panel-default row">
        <div class="panel-heading">
            <i class="fa fa-bell fa-fw"></i>Session Information
        </div>
        <!-- /.panel-heading -->
        <div class="panel-body">
            <asp:Label ID="debug" runat="server" />
        </div>
    </div>
</asp:Content>
