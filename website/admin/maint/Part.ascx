﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Part.ascx.vb" Inherits="admin_maint_Part" %>
<%@ Register Src="~/admin/UserControls/HTMLTextBox.ascx" TagPrefix="uc1" TagName="HTMLTextBox" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtPartList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Part ID:
                    <asp:Literal ID="litPartID" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Part</label>
                            <asp:TextBox ID="tbTitle" runat="server" Width="100%"></asp:TextBox>
                            <label>Description</label>
                            <asp:TextBox ID="tbDescription" runat="server" Width="100%"></asp:TextBox>
                            <label>Amazon Index</label>
                            <asp:TextBox ID="tbAmazonIndex" runat="server" Width="100%"></asp:TextBox>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Site<asp:DropDownList ID="ddlSite" runat="server" Width="100%"></asp:DropDownList></label>
                            <label>Category<asp:DropDownList ID="ddlCategory" runat="server" Width="100%"></asp:DropDownList></label>
                            <label>Type<asp:DropDownList ID="ddlType" runat="server" Width="100%"></asp:DropDownList></label>
                            <label>Location<asp:DropDownList ID="ddlLocation" runat="server" Width="100%"></asp:DropDownList></label>
                            <label>Order<asp:TextBox ID="tbDisplayOrder" runat="server" Width="100%"></asp:TextBox></label>
                            <label>Contact<asp:DropDownList ID="ddlContact" runat="server" Width="100%"></asp:DropDownList></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>URL/HTML</label><br />
                            <asp:TextBox runat="server" ID="tbURL" TextMode="MultiLine" Rows="10" Width="100%" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary" OnClick="cmd_Update_Click"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_SaveNew" runat="server" CssClass="btn btn-primary" OnClick ="cmd_SaveNew_Click"><span><span>Save As New</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary" OnClick="cmd_Insert_Click"><span><span>Insert</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default" OnClick="cmd_Cancel_Click"><span><span>Cancel</span></span></asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning" OnClick="cmd_Delete_Click"><span><span>Delete</span></span></asp:LinkButton>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
