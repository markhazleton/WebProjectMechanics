<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CollectionItemImageEdit.ascx.vb" Inherits="MineralCollection_CollectionItemImageEdit" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>
<%@ Register Src="~/admin/UserControls/AlertBox.ascx" TagPrefix="uc1" TagName="AlertBox" %>

<uc1:AlertBox runat="server" ID="AlertBox" />

<asp:Panel ID="pnlEdit" runat="server" BackColor="#DEBA84">
    <div class="form ">
        <asp:HiddenField runat="server" ID="hfCollectionItemID" />
        <asp:HiddenField runat="server" ID="hfCollectionItemImageID" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm=12 col-xs-12">
                <div class="form-group">
                    <asp:UpdatePanel ID="pnlImageThumb" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlImage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="labelddlImage" runat="server" AssociatedControlID="ddlImage" CssClass="form-control">Image:</asp:Label>
                            <asp:DropDownList runat="server" ID="ddlImage" AutoPostBack="true"></asp:DropDownList>
                            <asp:Literal ID="litImage" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Label ID="labeltbImageName" runat="server" AssociatedControlID="tbImageName">Image Name:</asp:Label>
                    <asp:TextBox ID="tbImageName" runat="server" CssClass="form-control"></asp:TextBox>

                    <asp:Label ID="labeltbImageDescription" runat="server" AssociatedControlID="tbImageDescription">Image Description:</asp:Label>
                    <asp:TextBox ID="tbImageDescription" runat="server" CssClass="form-control"></asp:TextBox>

                    <asp:Label ID="labelddlImageType" runat="server" AssociatedControlID="ddlImageType">Image Type:</asp:Label>
                    <asp:DropDownList ID="ddlImageType" runat="server" CssClass="form-control">
                        <asp:ListItem>Photo</asp:ListItem>
                        <asp:ListItem>Label</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label ID="labeltbPosition" runat="server" AssociatedControlID="tbPosition">Display Order:</asp:Label>
                    <asp:TextBox ID="tbPosition" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" CssClass="btn btn-primary" />
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" CssClass="btn btn-warning" ToolTip="This will remove the image from the specimen, but will NOT delte the file from the server" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>


<uc1:DisplayTable runat="server" ID="dtList" />


