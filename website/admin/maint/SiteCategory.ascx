﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteCategory.ascx.vb" Inherits="admin_maint_SiteCategory" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtLocationList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="gadgetblock">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Site Category ID:
                    <asp:Literal ID="LocationIDLit" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-lg-6">
                            <asp:Label ID="labeltbLocationNM" runat="server" AssociatedControlID="tbLocationNM">Name:</asp:Label>
                            <asp:TextBox ID="tbLocationNM" runat="server" CssClass="form-control" Wrap="False"></asp:TextBox>
                            <asp:Label ID="labeltbTitle" runat="server" AssociatedControlID="tbTitle">Title:</asp:Label>
                            <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control" Wrap="False"></asp:TextBox>
                            <asp:Label ID="labeltbLocationDS" runat="server" AssociatedControlID="tbLocationDS">Description:</asp:Label>
                            <asp:TextBox ID="tbLocationDS" runat="server" Rows="2" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                            <asp:Label ID="labeltbKeywords" runat="server" AssociatedControlID="tbKeywords">Keywords:</asp:Label>
                            <asp:TextBox ID="tbKeywords" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6">
                            <asp:Label ID="labelddlLocationType" runat="server" AssociatedControlID="ddlLocationType">Category Type:</asp:Label>
                            <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:Label ID="labelddlParentLocation" runat="server" AssociatedControlID="ddlParentLocation">Parent Category:</asp:Label>
                            <asp:DropDownList ID="ddlParentLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:Label ID="labeltbLocationOrder" runat="server" AssociatedControlID="tbLocationOrder">Order:</asp:Label>
                            <asp:TextBox ID="tbLocationOrder" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6">
                            <asp:Label ID="labeltbFileName" runat="server" AssociatedControlID="tbFileName">File Name:</asp:Label>
                            <asp:TextBox ID="tbFileName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6">
                            <asp:Label ID="labelddlLocationGroup" runat="server" AssociatedControlID="ddlLocationGroup">Location Group:</asp:Label>
                            <asp:DropDownList ID="ddlLocationGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                            <br />
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="form-group">
                        <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                        <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary"><span><span>Insert</span></span></asp:LinkButton>
                        <asp:LinkButton ID="cmd_SaveNew" runat="server" CssClass="btn btn-primary"><span><span>Save As New</span></span></asp:LinkButton>
                        <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default"><span><span>Cancel</span></span></asp:LinkButton>
                        <br />
                        <br />
                        <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning"><span><span>Delete</span></span></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
</asp:Panel>
