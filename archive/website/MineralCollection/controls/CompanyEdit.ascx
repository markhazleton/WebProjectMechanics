<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CompanyEdit.ascx.vb" Inherits="MineralCollection_CompanyEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfCompanyID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="labeltbCompanyNM" runat="server" AssociatedControlID="tbCompanyNM">Company:</asp:Label>
                    <asp:TextBox ID="tbCompanyNM" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="labeltbCompanyDS" runat="server" AssociatedControlID="tbCompanyDS">Description:</asp:Label>
                    <asp:TextBox ID="tbCompanyDS" runat="server" Height="100px" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="cmd_Update_Click" />
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" CssClass="btn btn-primary" OnClick="cmd_Insert_Click" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="cmd_Cancel_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" CssClass="btn btn-warning" OnClick="cmd_Delete_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<uc1:DisplayTable runat="server" ID="dtList" />
