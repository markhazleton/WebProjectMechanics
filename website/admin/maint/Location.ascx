<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Location.ascx.vb" Inherits="admin_maint_Location" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>
<%@ Register Src="~/admin/UserControls/HTMLTextBox.ascx" TagPrefix="uc1" TagName="HTMLTextBox" %>

<uc1:DisplayTable ID="dtLocationList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="gadgetblock">
    <div class="panel panel-info">
        <div class="panel-heading">
            Edit Location ID:
                    <asp:Literal ID="LocationIDLit" runat="server" Text="" />
            <asp:CheckBox ID="cbActive" runat="server" Text=" Is Active" />
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="form-group">
                    <div class="col-lg-6">
                        <asp:Label ID="labeltbLocationNM" runat="server" AssociatedControlID="tbLocationNM">Name (used for URL):</asp:Label>
                        <asp:TextBox ID="tbLocationNM" runat="server" CssClass="form-control" Wrap="False"></asp:TextBox>

                        <asp:Label ID="labeltbTitle" runat="server" AssociatedControlID="tbTitle">Title:</asp:Label>
                        <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control" Wrap="False"></asp:TextBox>

                        <asp:Label ID="labeltbKeywords" runat="server" AssociatedControlID="tbKeywords">Keywords:</asp:Label>
                        <asp:TextBox ID="tbKeywords" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="labeltbLocationDS" runat="server" AssociatedControlID="tbLocationDS">Description:</asp:Label>
                    <asp:TextBox ID="tbLocationDS" runat="server" Rows="2" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                    </div>
                    <div class="col-lg-3">
                        <asp:Label ID="labelddlCompany" runat="server" AssociatedControlID="ddlCompany">Site:</asp:Label>
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:Label ID="labelddlParentLocation" runat="server" AssociatedControlID="ddlParentLocation">Parent Location:</asp:Label>
                        <asp:DropDownList ID="ddlParentLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:Label ID="labelddlGroup" runat="server" AssociatedControlID="ddlGroup">Group (Security):</asp:Label>
                        <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">Admin</asp:ListItem>
                            <asp:ListItem Value="2">Editor</asp:ListItem>
                            <asp:ListItem Value="3">User</asp:ListItem>
                            <asp:ListItem Selected="True" Value="4">Guest</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-3">
                        <asp:Label ID="labelddlLocationType" runat="server" AssociatedControlID="ddlLocationType">Location Type:</asp:Label>
                        <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:Label ID="labeltbFileName" runat="server" AssociatedControlID="tbFileName">File Name (Only For Type NO File Name):</asp:Label>
                        <asp:TextBox ID="tbFileName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-lg-3">
                    </div>
                    <div class="col-lg-3">
                        <asp:Label ID="labelddlLocationGroup" runat="server" AssociatedControlID="ddlLocationGroup">Location Group:</asp:Label>
                        <asp:DropDownList ID="ddlLocationGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:Label ID="labeltbLocationOrder" runat="server" AssociatedControlID="tbLocationOrder">Location Order:</asp:Label>
                        <asp:TextBox ID="tbLocationOrder" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <asp:panel runat="server" id="pnlArticle" cssclass="col-lg-12 form-group">
                    <asp:HiddenField ID="hfArticleID" runat="server" />
                    <label>Summary</label>
                    <asp:TextBox ID="tbSummary" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                    <label>Body</label>
                    <uc1:HTMLTextBox runat="server" ID="HTMLTextBox" />
                </asp:panel>
            </div>
        </div>
        <div class="panel-footer">
            <div class="form-group">
                <asp:LinkButton ID="cmd_Update" runat="server" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                <asp:LinkButton ID="cmd_Insert" runat="server" CssClass="btn btn-primary"><span><span>Insert</span></span></asp:LinkButton>
                <asp:LinkButton ID="cmd_Cancel" runat="server" CssClass="btn btn-default"><span><span>Cancel</span></span></asp:LinkButton>
                <br />
                <br />
                <asp:LinkButton ID="cmd_Delete" runat="server" CssClass="btn btn-warning"><span><span>Delete</span></span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlThumbnails" runat="server" CssClass="row" Visible="false">
    <div class="col-lg-12 col-md-12 col-xs-12">
        <div class="panel panel-primary">
            <div class="panel-heading">Gallery Images</div>
            <div class="panel-body">
                <asp:Panel ID="pnlPageImages" runat="server" CssClass="row">
                </asp:Panel>
                <div class="form-group" id="uploads"></div>
            </div>
            <div class="panel-footer">
            </div>
        </div>
    </div>
</asp:Panel>
