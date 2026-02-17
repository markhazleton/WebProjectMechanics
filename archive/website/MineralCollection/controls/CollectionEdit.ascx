<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CollectionEdit.ascx.vb" Inherits="MineralCollection_CollectionEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>
<%@ Register Src="~/admin/UserControls/AlertBox.ascx" TagPrefix="uc1" TagName="AlertBox" %>

<uc1:AlertBox runat="server" ID="AlertBox" />
<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfCollectionID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="htModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labeltbCollectionNM" runat="server" AssociatedControlID="tbCollectionNM">Collection:</asp:Label>
                    <asp:TextBox ID="tbCollectionNM" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="labeltbCollectionDS" runat="server" AssociatedControlID="tbCollectionDS">Description:</asp:Label>
                    <asp:TextBox ID="tbCollectionDS" runat="server" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" CssClass="btn btn-primary" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" CssClass="btn btn-warning" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<uc1:DisplayTable runat="server" ID="dtList" />
