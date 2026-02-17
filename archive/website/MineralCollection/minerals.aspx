<%@ Page Title="" Language="VB" MasterPageFile="~/admin/runtime.master" AutoEventWireup="false" CodeFile="minerals.aspx.vb" Inherits="minerals" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <div class="row">
        <div class="large-12 columns">
            <div class="row">
                <div class="large-4 small-6 columns">
                    <asp:listbox id="ListBox1" runat="server" autopostback="True" datasourceid="LinqDataSource1" datatextfield="MineralNM" datavaluefield="MineralID" height="500px" width="250px"></asp:listbox>
                </div>
                <div class="large-8 small-6 columns">
                    <asp:literal id="litImage" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <asp:linqdatasource id="LinqDataSource1" runat="server" contexttypename="wpmMineralCollection.DataController" entitytypename="" tablename="Minerals" orderby="MineralNM">
    </asp:linqdatasource>
</asp:Content>

