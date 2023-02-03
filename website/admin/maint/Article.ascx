<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Article.ascx.vb" Inherits="admin_maint_Article" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>
<%@ Register Src="~/admin/UserControls/HTMLTextBox.ascx" TagPrefix="uc1" TagName="HTMLTextBox" %>

<uc1:DisplayTable ID="dtList" runat="server" />
<asp:Panel ID="pnlEdit" runat="server" CssClass="gadgetblock">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Edit Article ID:
                    <asp:Literal ID="ArticleIDLabel" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-6 form-group">
                            <label>Title</label><asp:TextBox ID="tbTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Publish Date</label><asp:TextBox ID="tbPubDate" runat="server" Textmode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                            <label>Page</label><asp:DropDownList ID="ddlPage" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label>Author</label><asp:TextBox ID="tbAuthor" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Description</label><asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Summary</label>
                            <asp:TextBox ID="tbSummary" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                            <label>Body</label>
                            <uc1:HTMLTextBox runat="server" ID="HTMLTextBox" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary" OnClick="cmd_Update_Click"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary" OnClick="cmd_Insert_Click"><span><span>Insert</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default" OnClick="cmd_Cancel_Click"><span><span>Cancel</span></span></asp:LinkButton>
                            <br /><br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning" OnClick="cmd_Delete_Click"><span><span>Delete</span></span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Panel>
