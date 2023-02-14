<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LocationStateEdit.ascx.vb" Inherits="MineralCollection_LocationStateEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>
<%@ Register Src="~/admin/UserControls/AlertBox.ascx" TagPrefix="uc1" TagName="AlertBox" %>


<uc1:AlertBox runat="server" ID="AlertBox" />

<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfLocationStateID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labeltbStateNM" runat="server" AssociatedControlID="tbStateNM">State:</asp:Label>
                    <asp:TextBox ID="tbStateNM" runat="server" CssClass="form-control" Width="100%" Wrap="False"></asp:TextBox>

                    <asp:Label ID="labeltbStateDS" runat="server" AssociatedControlID="tbStateDS">Description:</asp:Label>
                    <asp:TextBox ID="tbStateDS" runat="server" CssClass="form-control" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>

                    <asp:Label ID="labelddlCountry" runat="server" AssociatedControlID="ddlCountry">Country:</asp:Label>
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control"></asp:DropDownList>
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
