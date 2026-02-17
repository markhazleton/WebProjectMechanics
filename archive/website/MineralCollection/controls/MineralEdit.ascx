<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MineralEdit.ascx.vb" Inherits="MineralCollection_MineralEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfMineralID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labeltbMineralNM" runat="server" AssociatedControlID="tbMineralNM">Mineral:</asp:Label>
                    <asp:TextBox ID="tbMineralNM" runat="server" Width="100%" Wrap="False" CssClass="form-control"></asp:TextBox>

                    <asp:Label ID="labeltbWikipediaURL" runat="server" AssociatedControlID="tbWikipediaURL">Wikipedia URL:</asp:Label>
                    <asp:TextBox ID="tbWikipediaURL" runat="server" Width="100%" Wrap="False" CssClass="form-control"></asp:TextBox>

                    <asp:Label ID="labeltbMineralDS" runat="server" AssociatedControlID="tbMineralDS">Description:</asp:Label>
                    <asp:TextBox ID="tbMineralDS" runat="server" CssClass="form-control" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" OnClick="cmd_Update_Click" CssClass="btn btn-primary" />
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" OnClick="cmd_Insert_Click" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" OnClick="cmd_Cancel_Click" CssClass="btn btn-default" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" OnClick="cmd_Delete_Click" CssClass="btn btn-warning" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<uc1:DisplayTable runat="server" ID="dtList" />

