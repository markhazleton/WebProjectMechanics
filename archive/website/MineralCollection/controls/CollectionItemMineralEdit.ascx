<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CollectionItemMineralEdit.ascx.vb" Inherits="MineralCollection_CollectionItemMineralEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>



<asp:Panel ID="pnlEdit" runat="server">

    <div class="form">
        <asp:HiddenField runat="server" ID="hfCollectionItemID" />
        <asp:HiddenField ID="hfCollectionItemMineralID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labelddlMineral" runat="server" AssociatedControlID="ddlMineral">Mineral:</asp:Label>
                    <asp:DropDownList ID="ddlMineral" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="labeltbPosition" runat="server" AssociatedControlID="tbPosition">Order:</asp:Label>
                    <asp:TextBox ID="tbPosition" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" CssClass="btn btn-primary" />
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" CssClass="btn btn-warning" />
                </div>

            </div>
        </div>
    </div>


</asp:Panel>

<uc1:DisplayTable runat="server" ID="dtList" />