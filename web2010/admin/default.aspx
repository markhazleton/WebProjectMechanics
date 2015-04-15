<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="wpm_admin_default" Title="Untitled Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <asp:Repeater ID="rptNavigation" runat="server">
        <HeaderTemplate>
            <div id="navcontainer" class="row">
                <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href="<%# Eval("Value") %>" class="<%# GetClass(CStr(Eval("Name"))) %>"><%# Eval("Name") %></a></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Literal runat="server" Text="" ID="myContent" />
</asp:Content>
