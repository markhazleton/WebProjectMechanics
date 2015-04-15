<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Contact.ascx.vb" Inherits="admin_maint_Contact" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server">
    <asp:HiddenField ID="hfCompanyID" runat="server" />
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Contact ID:<asp:Label ID="ContactIDLabel" runat="server" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Primary Contact:</label>
                            <asp:TextBox ID="PrimaryContactTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                First Name:
                            <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="form-control"></asp:TextBox></label>
                            <label>
                                Last Name:
                            <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="form-control"></asp:TextBox></label>
                            <label>
                                Active:
                            <asp:CheckBox ID="ActiveCheckBox" runat="server" CssClass="form-control" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>EMail:</label>
                            <asp:TextBox ID="EMailTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>LogonName:</label>
                            <asp:TextBox ID="LogonNameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>LogonPassword:</label>
                            <asp:TextBox ID="LogonPasswordTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Group (Security)</label>
                            <asp:DropDownList ID="ddlGroup" runat="server" Width="100%">
                                <asp:ListItem Value="1">Admin</asp:ListItem>
                                <asp:ListItem Value="2">Editor</asp:ListItem>
                                <asp:ListItem Value="3">User</asp:ListItem>
                                <asp:ListItem Selected="True" Value="4">Guest</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
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



