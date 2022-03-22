<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="wpm_admin_default" Title="Untitled Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <div class="panel panel-default" id="navs">
        <div class="panel-body" style="background-color: aliceblue;">
            <asp:Repeater ID="rptNavigation" runat="server">
                <HeaderTemplate>
                    <strong>Site Configuration</strong>
                    <ol class="breadcrumb">
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a class="<%# GetClass(Eval("Name")) %>" href="<%# Eval("Value") %>"><%# Eval("Name") %></a></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="panel panel-default" id="reports">
        <div class="panel-body" style="background-color: aliceblue;">
            <asp:Repeater ID="rptReports" runat="server">
                <HeaderTemplate>
                    <strong>Site Reports</strong>
                    <ol class="breadcrumb">
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a class=" <%# GetClass(Eval("Name")) %>" href="<%# Eval("Value") %>"><%# Eval("Name") %></a></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentMain" runat="server">
    <div class="row" id="MainContent">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-heading">Main Content</div>
                <div class="panel-body">
                    <asp:Literal runat="server" Text="" ID="myContent" />
                </div>
                <div class="panel-footer">
                </div>
            </div>
        </div>
    </div>
</asp:Content>


<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">
    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('table.dataTable').dataTable();
        });
    </script>
</asp:Content>

