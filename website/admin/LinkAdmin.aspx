<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="LinkAdmin.aspx.vb" Inherits="wpm_admin_LinkAdmin" Title="Link Admin" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <asp:Literal runat="server" Text="" ID="myContent" />
</asp:Content>
<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">

    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
        });
    </script>

</asp:Content>
