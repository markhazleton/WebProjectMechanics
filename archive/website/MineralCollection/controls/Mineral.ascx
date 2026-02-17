<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Mineral.ascx.vb" Inherits="MineralCollection_controls_Mineral" %>
<asp:FormView ID="FormView1" runat="server" AllowPaging="True" DataKeyNames="MineralID" DataSourceID="LinqDataSource1" >
    <EditItemTemplate>
    </EditItemTemplate>
    <InsertItemTemplate>
    </InsertItemTemplate>
    <ItemTemplate>
        MineralID:<%# Eval("MineralID") %><br />
        <br />
        MineralNM:<%# Eval("MineralNM") %><br />
        <br />
        MineralDS:<p><%# Eval("MineralDS") %></p>
        Primary:
        <asp:Label ID="CollectionItemsLabel" runat="server" Text='<%# Bind("CollectionItems") %>' />

        Secondary:
        <asp:Label ID="CollectionItemMineralsLabel" runat="server" Text='<%# Bind("CollectionItemMinerals") %>' />
    </ItemTemplate>
</asp:FormView>
<asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="wpmMineralCollection.DataController" EntityTypeName="" TableName="Minerals" Where="MineralID == @MineralID">
    <WhereParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="MineralID" QueryStringField="MineralID" Type="Int32" />
    </WhereParameters>
</asp:LinqDataSource>

