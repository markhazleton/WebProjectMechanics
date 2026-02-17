<%@ Page Title="" Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_gallery_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="/admin/gallery/dropzone.js"></script>
    <link rel="stylesheet" href="/admin/gallery/dropzone.css" />

    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <p>
        Add Images to Site    </p>
       

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <!-- Change /upload-target to your upload address -->
    <h1>Image Upload</h1>
    <form action="/admin/gallery/Upload.ashx" class="dropzone">
        <Br /><br />
    </form>
</asp:Content>

