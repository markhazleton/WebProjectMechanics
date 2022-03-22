<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PageAlias.ascx.vb" Inherits="admin_maint_PageAlias" %>

<asp:Panel ID="pnlList" runat="server">
    <asp:Literal runat="server" Text="" ID="myContent" />
</asp:Panel>

<asp:Panel ID="pnlEdit" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Page Alias ID:<asp:Literal ID="PageAliasIDLabel1" runat="server" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>PageURL:</label><asp:TextBox ID="PageURLTextBox" runat="server" Width="100%" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>TargetURL:</label><asp:TextBox ID="TargetURLTextBox" runat="server" Width="100%"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>AliasType:</label><asp:TextBox ID="AliasTypeTextBox" runat="server" Width="100%"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Site<asp:DropDownList ID="ddlCompany" runat="server" Width="100%"></asp:DropDownList></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary" OnClick="cmd_Update_Click"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary" OnClick="cmd_Insert_Click"><span><span>Insert</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default" OnClick="cmd_Cancel_Click"><span><span>Cancel</span></span></asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning" OnClick="cmd_Delete_Click"><span><span>Delete</span></span></asp:LinkButton>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Panel>







