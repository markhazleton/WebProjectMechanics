<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CKEditorTest.aspx.vb" Inherits="admin_CKEditorTest" ValidateRequest="false" EnableEventValidation="false"  %>

<%@ Register Src="~/admin/UserControls/HTMLTextBox.ascx" TagPrefix="uc1" TagName="HTMLTextBox" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
        <uc1:HTMLTextBox runat="server" ID="HTMLTextBox" />
        <asp:LinkButton ID="cmd_SaveHTML" runat="server" Text="SaveHTML" OnClick="cmd_SaveHTML_Click"></asp:LinkButton>


        <hr /><br />
        <asp:Literal ID="litHTML" runat="server" ValidateRequestMode="Disabled"></asp:Literal>

    </form>
</body>
</html>
