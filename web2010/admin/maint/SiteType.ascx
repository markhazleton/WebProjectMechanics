<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteType.ascx.vb" Inherits="admin_maint_SiteType" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />


<asp:Panel ID="pnlEdit" runat="server" CssClass="row" Visible="false">
    <asp:HiddenField ID="hfRecordSource" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-Primary">
                <div class="panel-heading">
                    Edit Location Type ID:
                    <asp:Literal ID="litSiteTypeID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbSiteTypeNM" runat="server" CssClass="form-control" AssociatedControlID="tbSiteTypeNM" Text="Name"></asp:Label>
                            <asp:TextBox ID="tbSiteTypeNM" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbSiteTypeDS" runat="server" CssClass="form-control" AssociatedControlID="tbSiteTypeDS" Text="Description"></asp:Label>
                            <asp:TextBox ID="tbSiteTypeDS" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbSiteTypeComment" runat="server" CssClass="form-control" AssociatedControlID="tbSiteTypeComment" Text="Comment"></asp:Label>
                            <asp:TextBox ID="tbSiteTypeComment" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabeltbSiteTypeFileName" runat="server" CssClass="form-control" AssociatedControlID="tbSiteTypeFileName" Text="File Name"></asp:Label>
                            <asp:TextBox ID="tbSiteTypeFileName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbSiteTypeTransferURL" runat="server" CssClass="form-control" AssociatedControlID="tbSiteTypeTransferURL" Text="Transfer URL"></asp:Label>
                            <asp:TextBox ID="tbSiteTypeTransferURL" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" OnClick="cmd_Update_Click" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" OnClick="cmd_Insert_Click" CssClass="btn btn-primary"><span><span>Insert</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" OnClick="cmd_Cancel_Click" CssClass="btn btn-default"><span><span>Cancel</span></span></asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" OnClick="cmd_Delete_Click" CssClass="btn btn-warning"><span><span>Delete</span></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <uc1:DisplayTable ID="dtSiteTypeUsage" runat="server" />

</asp:Panel>
