<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PageOrder.aspx.vb" Inherits="VB" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="Button1" runat="server" Text="Move UP" />
        <asp:Button ID="Button2" runat="server" Text="Move DOWN" />
        <br />
        <asp:ListBox ID="lstLeft" runat="server" SelectionMode="Multiple" Width="300" Rows="20"></asp:ListBox>
        <br />
    </form>
</body>
</html>
