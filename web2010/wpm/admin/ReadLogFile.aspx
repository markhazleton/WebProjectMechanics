<%@ Page Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false"  CodeFile="ReadLogFile.aspx.vb" Inherits="wpm_admin_ReadLogFile" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:DropDownList ID="myFileListBox" runat="server"></asp:DropDownList>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
    <asp:Literal ID="MyHTML" runat="server" />
</asp:Content>
