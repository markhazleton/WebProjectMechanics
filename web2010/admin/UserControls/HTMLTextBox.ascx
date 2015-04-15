<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HTMLTextBox.ascx.vb" Inherits="admin_UserControls_HTMLTextBox" %>

<asp:Literal ID="litHTMLControl" runat="server"></asp:Literal>

<script src="//cdn.ckeditor.com/4.4.6/full-all/ckeditor.js"></script>
<script>
    CKEDITOR.replace('editor1');
</script>

