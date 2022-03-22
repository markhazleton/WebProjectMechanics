<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Image.ascx.vb" Inherits="admin_maint_Image" %>

<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="gadgetblock">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Image ID:
                    <asp:Literal ID="ImageIDLiteral" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class=" row form-group">
                        <div class="col-lg-4">
                            <asp:Image ID="imgThumb" runat="server" CssClass="img-responsive" Width="100%" ImageAlign="Left" />
                            <br style="clear: both;" />
                        </div>

                        <div class="col-lg-8">
                            <asp:Label ID="labelImageNameTextBox" runat="server" AssociatedControlID="ImageNameTextBox">Image Name (URL):</asp:Label>
                            <asp:TextBox ID="ImageNameTextBox" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                            <asp:Label ID="labeltitleTextBox" runat="server" AssociatedControlID="titleTextBox">Title:</asp:Label>
                            <asp:TextBox ID="titleTextBox" runat="server" Text="" CssClass="form-control" />
                            <asp:CheckBox ID="activeCheckBox" runat="server" Text="Is Active ?" CssClass="form-control" />
                            <asp:Label ID="labelImageDescriptionTextBox" runat="server" AssociatedControlID="ImageDescriptionTextBox">Description:</asp:Label>
                            <asp:TextBox ID="ImageDescriptionTextBox" runat="server" Text="" CssClass="form-control" Rows="10" TextMode="MultiLine" />
                        </div>

                    </div>
                    <div class=" row form-group">
                        <div class="col-lg-4">
                            <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default"><span><span>Cancel</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary"><span><span>Insert</span></span></asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning"><span><span>Delete</span></span></asp:LinkButton>
                        </div>
                        <div class="col-lg-8">
                            <asp:Label ID="labelImageCommentTextBox" runat="server" AssociatedControlID="ImageCommentTextBox">Body:</asp:Label>
                            <asp:TextBox ID="ImageCommentTextBox" runat="server" Text="" CssClass="form-control" Rows="15" TextMode="MultiLine" />

                            <asp:Label ID="labelImageDateTextBox" runat="server" AssociatedControlID="ImageDateTextBox">ImageDate:</asp:Label>
                            <asp:TextBox ID="ImageDateTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelModifiedDTTextBox" runat="server" AssociatedControlID="ModifiedDTTextBox">ModifiedDT:</asp:Label>
                            <asp:TextBox ID="ModifiedDTTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelVersionNoTextBox" runat="server" AssociatedControlID="VersionNoTextBox">VersionNo:</asp:Label>
                            <asp:TextBox ID="VersionNoTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelContactIDTextBox" runat="server" AssociatedControlID="ContactIDTextBox">ContactID:</asp:Label>
                            <asp:TextBox ID="ContactIDTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelmediumTextBox" runat="server" AssociatedControlID="mediumTextBox">medium:</asp:Label>
                            <asp:TextBox ID="mediumTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelsizeTextBox" runat="server" AssociatedControlID="sizeTextBox">size:</asp:Label>
                            <asp:TextBox ID="sizeTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelpriceTextBox" runat="server" AssociatedControlID="priceTextBox">price:</asp:Label>
                            <asp:TextBox ID="priceTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelcolorTextBox" runat="server" AssociatedControlID="colorTextBox">color:</asp:Label>
                            <asp:TextBox ID="colorTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:Label ID="labelsubjectTextBox" runat="server" AssociatedControlID="subjectTextBox">subject:</asp:Label>
                            <asp:TextBox ID="subjectTextBox" runat="server" Text="" CssClass="form-control" />

                            <asp:CheckBox ID="soldCheckBox" runat="server" Text="Is Sold?" CssClass="form-control" />
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
