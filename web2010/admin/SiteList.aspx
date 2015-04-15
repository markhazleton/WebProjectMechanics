<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="SiteList.aspx.vb" Inherits="SiteList" Title="Untitled Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <asp:dropdownlist id="myFileListBox" runat="server"></asp:dropdownlist>

    <asp:button id="btnSubmit" runat="server" text="Submit" />
    <br />
    <br />
    <asp:literal id="SiteList" runat="server" />
    <br />
    <br />
    <asp:gridview id="GridView1" runat="server" cellpadding="4" forecolor="#333333" gridlines="None"
        autogeneratecolumns="False" datakeynames="CompanyID" datasourceid="AccessDataSource"
        enablemodelvalidation="True">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="CompanyID" HeaderText="CompanyID" InsertVisible="False"
                        ReadOnly="True" SortExpression="CompanyID" />
                    <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" SortExpression="CompanyName" />
                    <asp:BoundField DataField="SiteURL" HeaderText="SiteURL" SortExpression="SiteURL" />
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:gridview>
    <asp:accessdatasource id="AccessDataSource" runat="server" datafile="~/App_Data/wpm-demo.mdb"
        selectcommand="SELECT [CompanyID], [CompanyName], [SiteURL] FROM [Company] order by [CompanyName] asc">
            </asp:accessdatasource>

    <hr />
    <asp:literal id="MyHTML" runat="server" />
</asp:Content>
