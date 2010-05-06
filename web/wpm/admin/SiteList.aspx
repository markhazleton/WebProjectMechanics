<%@ Page Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false" CodeFile="SiteList.aspx.vb" Inherits="SiteList" title="Untitled Page" %>

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
    GridLines="None" AutoGenerateColumns="False" DataKeyNames="CompanyID" 
    DataSourceID="AccessDataSource">
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="CompanyID" HeaderText="CompanyID" 
            InsertVisible="False" ReadOnly="True" SortExpression="CompanyID" />
        <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" 
            SortExpression="CompanyName" />
        <asp:BoundField DataField="SiteURL" HeaderText="SiteURL" 
            SortExpression="SiteURL" />
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
<asp:AccessDataSource ID="AccessDataSource" runat="server" 
    DataFile="~/access_db/wpm-demo.mdb" 
    SelectCommand="SELECT [CompanyID], [CompanyName], [SiteURL] FROM [Company]">
</asp:AccessDataSource>
<hr />
<asp:Literal ID="MyHTML" runat="server" />


</form>


</asp:Content>

