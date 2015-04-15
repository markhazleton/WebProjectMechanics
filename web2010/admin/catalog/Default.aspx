<%@ Page Title="" Language="VB" MasterPageFile="~/admin/WPMAdmin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_catalog_Default" %>

<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="row">
        <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
            <div class="form-group">
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:LinkButton ID="cmd_FileUpload" runat="server" CssClass="btn btn-primary" OnClick="cmd_FileUpload_Click" Text="Upload File"></asp:LinkButton>
            </div>
            <asp:Literal ID="litUpload" runat="server"></asp:Literal>
        </div>
    </div>
    <uc1:DisplayTable runat="server" ID="dtList" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
            $(".flippanel").slideToggle("slow");
            $(".flip").click(function () {
                $(".flippanel").slideToggle("slow");
            });
        });
    </script>

</asp:Content>

