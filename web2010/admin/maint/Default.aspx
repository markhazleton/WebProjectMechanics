<%@ Page Title="" Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="Default.aspx.vb" Inherits="Admin_Maint_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:Repeater ID="rptNavigation" runat="server">
        <HeaderTemplate>
            <div id="navcontainer" class="row">
                <ul>
        </HeaderTemplate>
        <ItemTemplate>
               <li><a href="<%# Eval("Value") %>" class="<%# GetClass(Eval("Name")) %>"><%# Eval("Name") %></a></li>
        </ItemTemplate>
        <FooterTemplate>
              </ul>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Panel ID="pnlMaintenance" runat="server" CssClass="row">
    </asp:Panel>
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">
    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
        });
    </script>
</asp:Content>


