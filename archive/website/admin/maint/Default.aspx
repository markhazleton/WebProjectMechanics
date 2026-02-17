<%@ Page Title="" Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="Default.aspx.vb" Inherits="Admin_Maint_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:Panel ID="pnlMaintenance" runat="server" >
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
