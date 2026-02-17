<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="ReadLogFile.aspx.vb" Inherits="wpm_admin_ReadLogFile" Title="Untitled Page" %>

<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <asp:DropDownList ID="myFileListBox" runat="server">
    </asp:DropDownList>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
    <asp:Literal ID="MyHTML" runat="server" />
    
    <uc1:DisplayTable runat="server" ID="dtList" />
</asp:Content>


<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">

    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
        });
    </script>

</asp:Content>
