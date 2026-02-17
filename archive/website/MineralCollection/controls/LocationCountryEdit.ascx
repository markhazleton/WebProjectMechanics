<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LocationCountryEdit.ascx.vb" Inherits="MineralCollection_controls_LocationCountryEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfLocationCountryID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labeltbCountry" runat="server" AssociatedControlID="tbCountry">Country:</asp:Label>
                    <asp:TextBox ID="tbCountry" runat="server" CssClass="form-control" Width="100%" Wrap="False"></asp:TextBox>
                    <asp:Label ID="labeltbCountryDS" runat="server" AssociatedControlID="tbCountryDS">Description:</asp:Label>
                    <asp:TextBox ID="tbCountryDS" runat="server" CssClass="form-control" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" OnClick="cmd_Update_Click" runat="server" Text="Update" CssClass="btn btn-primary" />
                    <asp:LinkButton ID="cmd_Insert" OnClick="cmd_Insert_Click" runat="server" Text="Insert" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" OnClick="cmd_Cancel_Click" runat="server" Text="Cancel" CssClass="btn btn-default" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" OnClick="cmd_Delete_Click" runat="server" Text="Delete" CssClass="btn btn-warning" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<uc1:DisplayTable runat="server" ID="dtList" />
