<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HTMLTextBox.ascx.vb" Inherits="admin_UserControls_HTMLTextBox" %>

<asp:Literal ID="litHTMLControl" runat="server"></asp:Literal>

<!-- Load the CKEditor 5 Classic build -->
<script src="https://cdn.ckeditor.com/ckeditor5/39.0.0/classic/ckeditor.js"></script>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        ClassicEditor
            .create(document.querySelector('#editor1'))
            .catch(error => {
                console.error(error);
            });
    });
</script>
