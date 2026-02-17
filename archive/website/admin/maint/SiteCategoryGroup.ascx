<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteCategoryGroup.ascx.vb" Inherits="admin_maint_SiteCategoryGroup" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>
<uc1:DisplayTable ID="dtList" runat="server" />
<asp:Panel ID="pnlEdit" runat="server" CssClass="">
    <asp:HiddenField ID="hfRecordSource" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Parameter ID:
                    <asp:Literal ID="litSiteCategoryGroupID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbSortOrder" runat="server" CssClass="form-control" AssociatedControlID="tbSortOrder" Text="Order"></asp:Label>
                            <asp:TextBox ID="tbSortOrder" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbSiteCategoryGroupNM" runat="server" CssClass="form-control" AssociatedControlID="tbSiteCategoryGroupNM" Text="Name"></asp:Label>
                            <asp:TextBox ID="tbSiteCategoryGroupNM" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbSiteCategoryGroupDS" runat="server" CssClass="form-control" AssociatedControlID="tbSiteCategoryGroupDS" Text="Description"></asp:Label>
                            <asp:TextBox ID="tbSiteCategoryGroupDS" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" OnClick="cmd_Update_Click" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_SaveNew" runat="server" OnClick="cmd_SaveNew_Click" CssClass="btn btn-primary"><span><span>Save As New</span></span></asp:LinkButton>
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

    <uc1:DisplayTable ID="dtParameterUsage" runat="server" />

</asp:Panel>
