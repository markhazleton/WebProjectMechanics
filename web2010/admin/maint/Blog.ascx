<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Blog.ascx.vb" Inherits="admin_maint_Blog" %>

<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>
<%@ Register Src="~/admin/UserControls/HTMLTextBox.ascx" TagPrefix="uc1" TagName="HTMLTextBox" %>

<asp:Panel ID="pnlLocationEdit" runat="server" CssClass="gadgetblock">
    <div class="form">
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Blog Location ID:
                    <asp:Literal ID="LocationIDLit" runat="server" Text="" />
                    </div>
                    <div class="panel-body" style="background: rgba(189, 182, 182, 0.17);">
                        <div class="row">
                            <div class="col-lg-9 form-group">
                                <label>Blog Name</label><asp:TextBox ID="tbLocationNM" runat="server" CssClass="form-control"></asp:TextBox>
                                <label>Parent Location</label><asp:DropDownList ID="ddlParentLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                                <label>Blog Title</label><asp:TextBox ID="tbLocationTitle" runat="server" Width="100%" Wrap="False"></asp:TextBox>
                                <label>Blog Description</label><asp:TextBox ID="tbLocationDescription" runat="server" Rows="2" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                <label>Blog Keywords</label><asp:TextBox ID="tbLocationKeywords" runat="server" Width="100%"></asp:TextBox>
                                </div>
                            <div class="col-lg-3 form-group">
                                <label>Active</label><asp:CheckBox ID="cbActive" runat="server" CssClass="form-control" />
                                <label>Display Order</label><asp:TextBox ID="tbLocationOrder" runat="server" CssClass="form-control"></asp:TextBox>
                                <label>Group (Security)</label><asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control">
                                <asp:ListItem Value="1">Admin</asp:ListItem>
                                <asp:ListItem Value="2">Editor</asp:ListItem>
                                <asp:ListItem Value="3">User</asp:ListItem>
                                <asp:ListItem Selected="True" Value="4">Guest</asp:ListItem>
                            </asp:DropDownList>
                                <label>Rows Per Page</label><asp:TextBox ID="tbRowsPerPage" runat="server" CssClass="form-control">3</asp:TextBox>
                                <label>Images Per Row</label><asp:TextBox ID="tbImagesPerRow" runat="server" CssClass="form-control">3</asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 form-group">
                                <asp:LinkButton ID="cmd_UpdateLocation" runat="server" CssClass="btn btn-primary" OnClick="cmd_UpdateLocation_Click"><span><span>Update Blog</span></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>


<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="gadgetblock">
    <asp:HiddenField ID="hfLocationID" runat="server" />
    <asp:HiddenField ID="hfArticleID" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Article ID:
                    <asp:Literal ID="ArticleIDLabel" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-6 form-group">
                            <label>Article</label><asp:TextBox ID="tbTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Publish Date</label><asp:TextBox ID="tbPubDate" runat="server" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                            <label>Author</label><asp:TextBox ID="tbAuthor" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label>Description</label><asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                            <label>Summary</label><asp:TextBox ID="tbSummary" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Body</label>
                            <uc1:HTMLTextBox runat="server" ID="HTMLTextBox" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary"><span><span>Insert</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default"><span><span>Cancel</span></span></asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning"><span><span>Delete</span></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Panel>
