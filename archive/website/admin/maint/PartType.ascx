<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PartType.ascx.vb" Inherits="admin_maint_LinkType" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="row" Visible="false">
    <asp:HiddenField ID="hfRecordSource" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-Primary">
                <div class="panel-heading">
                    Edit Part Type ID:
                    <asp:Literal ID="litLinkTypeID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbLinkTypeCD" runat="server" AssociatedControlID="tbLinkTypeCD" Text="Code"></asp:Label>
                            <asp:TextBox ID="tbLinkTypeCD" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbLinkTypeDS" runat="server" AssociatedControlID="tbLinkTypeDS" Text="Description"></asp:Label>
                            <asp:TextBox ID="tbLinkTypeDS" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabeltbLinkTypeComment" runat="server" AssociatedControlID="tbLinkTypeComment" Text="Comment"></asp:Label>
                            <asp:TextBox ID="tbLinkTypeComment" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>

                            <asp:Label ID="LabelddlLinkTypeTarget" runat="server" AssociatedControlID="ddlLinkTypeTarget" Text="Target"></asp:Label>
                            <asp:DropDownList ID="ddlLinkTypeTarget" runat="server" CssClass="form-control">
                                <asp:ListItem>_blank</asp:ListItem>
                                <asp:ListItem>_self</asp:ListItem>
                                <asp:ListItem>_parent</asp:ListItem>
                                <asp:ListItem>_top</asp:ListItem>
                            </asp:DropDownList>
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

    <uc1:DisplayTable ID="dtPageUsage" runat="server" />

</asp:Panel>
