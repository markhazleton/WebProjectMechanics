<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Role.ascx.vb" Inherits="admin_maint_Role" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="row" Visible="false">
    <asp:HiddenField ID="hfRecordSource" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-Primary">
                <div class="panel-heading">
                    Edit Role Type ID:
                    <asp:Literal ID="litRoleID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbRoleName" runat="server" CssClass="form-control" AssociatedControlID="tbRoleName" Text="Role"></asp:Label>
                            <asp:TextBox ID="tbRoleName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbRoleNM" runat="server" CssClass="form-control" AssociatedControlID="tbRoleNM" Text="Role Name"></asp:Label>
                            <asp:TextBox ID="tbRoleNM" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbRoleTitle" runat="server" CssClass="form-control" AssociatedControlID="tbRoleTitle" Text="Role Title"></asp:Label>
                            <asp:TextBox ID="tbRoleTitle" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
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
