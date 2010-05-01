<%@ Page Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false" CodeFile="wpmSiteList.aspx.vb" Inherits="wpm_admin_ReadLogFile" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">

<form runat="server">

<asp:DropDownList ID="myFileListBox" runat="server">
</asp:DropDownList>
<asp:Button ID="btnSubmit" runat="server" Text="Submit"  />
<br />
<br />
<asp:Literal ID="SiteList" runat="server" />
<br />
<br />
<asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
    GridLines="None">
    <RowStyle BackColor="#E3EAEB" />
    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#7C6F57" />
    <AlternatingRowStyle BackColor="White" />
</asp:GridView>
</form>
<hr />
<asp:Literal ID="MyHTML" runat="server" />


</asp:Content>

