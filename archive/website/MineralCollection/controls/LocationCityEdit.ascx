<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LocationCityEdit.ascx.vb" Inherits="MineralCollection_LocationCityEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfLocationCityID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">

                <div class="form-group">
                    <asp:Label ID="lbltbCity" runat="server" AssociatedControlID="tbCity">City:</asp:Label>
                    <asp:TextBox ID="tbCity" runat="server" Width="100%" Wrap="False" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lblddlState" runat="server" AssociatedControlID="ddlState">State:</asp:Label>
                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="LabelddlCountry" runat="server" AssociatedControlID="ddlCountry">Country:</asp:Label>
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="LabeltbCityDS" runat="server" AssociatedControlID="tbCityDS">Description:</asp:Label>
                    <asp:TextBox ID="tbCityDS" runat="server" Height="100px" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
