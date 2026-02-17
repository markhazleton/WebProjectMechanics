<%@ Page Title="" Language="VB" MasterPageFile="~/MineralCollection/MineralAdmin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="MineralCollection_gallery_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <p>
        Add Images to Website
    </p>
       

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <!-- Change /upload-target to your upload address -->
    <form action="/admin/gallery/Upload.ashx" class="dropzone">
    </form>
</asp:Content>

