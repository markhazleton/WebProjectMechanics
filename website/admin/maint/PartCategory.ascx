<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PartCategory.ascx.vb" Inherits="admin_maint_LinkCategory" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="row" Visible="false">
    <asp:HiddenField ID="hfRecordSource" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-Primary">
                <div class="panel-heading">
                    Edit Location Type ID:
                    <asp:Literal ID="litLinkCategoryID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbLinkCategoryNM" runat="server" CssClass="form-control" AssociatedControlID="tbLinkCategoryNM" Text="Page File Name"></asp:Label>
                            <asp:TextBox ID="tbLinkCategoryNM" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="LabeltbLinkCategoryDS" runat="server" CssClass="form-control" AssociatedControlID="tbLinkCategoryDS" Text="Description"></asp:Label>
                            <asp:TextBox ID="tbLinkCategoryDS" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="LabelddlParentID" runat="server" AssociatedControlID="ddlParentID" Text="Parent"></asp:Label>
                            <asp:DropDownList ID="ddlParentID" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:Label ID="LabelddlPageID" runat="server" AssociatedControlID="ddlPageID" Text="Page"></asp:Label>
                            <asp:DropDownList ID="ddlPageID" runat="server" CssClass="form-control"></asp:DropDownList>
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
