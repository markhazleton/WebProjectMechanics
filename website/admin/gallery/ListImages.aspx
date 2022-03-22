<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" CodeFile="ListImages.aspx.vb" Inherits="Admin_ListImages" %>

<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>


<asp:Content ID="ContentMain" ContentPlaceHolderID="Content" runat="Server">

    <div class="row">
        <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
            <div class="form-group">
                <asp:LinkButton ID="cmd_FileUpload" runat="server" CssClass="btn btn-primary" PostBackUrl="~/admin/gallery/Default.aspx" Text="Upload Image Files"></asp:LinkButton>
            </div>
            <asp:Literal ID="litUpload" runat="server"></asp:Literal>
        </div>
    </div>
    <uc1:DisplayTable runat="server" ID="dtList" />
</asp:Content>


<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">
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
