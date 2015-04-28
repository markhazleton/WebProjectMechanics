<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Parameter.ascx.vb" Inherits="admin_maint_Parameter" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" EnableViewState="false" ViewStateMode="Disabled" />


<asp:Panel ID="pnlEdit" runat="server" CssClass="">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Parameter ID:
                    <asp:Literal ID="litParameterID" runat="server" Text="" />
                    <asp:HiddenField ID="hfParameterID" runat="server" />
                    <asp:HiddenField ID="hfRecordSource" runat="server" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabelddlParameterTypeID" runat="server" AssociatedControlID="ddlParameterTypeID" Text="Parameter Type"></asp:Label>
                            <asp:DropDownList ID="ddlParameterTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:TextBox ID="tbParameterTypeDS" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbSortOrder" runat="server" AssociatedControlID="tbSortOrder" Text="Order"></asp:Label>
                            <asp:TextBox ID="tbSortOrder" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="background-color: aliceblue; border: 1px solid black;">
                        <p><strong>Targeting</strong></p>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="labelddlSiteCategoryTypeID" runat="server" AssociatedControlID="ddlSiteCategoryTypeID">Site Type:</asp:Label>
                            <asp:DropDownList ID="ddlSiteCategoryTypeID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="labelddlLocation" runat="server" AssociatedControlID="ddlLocation">Location:</asp:Label>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="labelddlLocationGroupID" runat="server" AssociatedControlID="ddlLocationGroupID">Location Group:</asp:Label>
                            <asp:DropDownList ID="ddlLocationGroupID" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="labelddlCompany" runat="server" AssociatedControlID="ddlCompany">Site:</asp:Label>
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:Label ID="LabeltbParameterValue" runat="server" CssClass="form-control" AssociatedControlID="tbParameterValue" Text="Value"></asp:Label>
                            <asp:TextBox ID="tbParameterValue" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <asp:LinkButton ID="cmd_Update" runat="server" OnClick="cmd_Update_Click" CssClass="btn btn-primary"><span><span>Update</span></span></asp:LinkButton>
                            <asp:LinkButton ID="cmd_SaveNew" runat="server" OnClick="cmd_SaveNew_Click" CssClass="btn btn-primary"><span><span>Save As New</span></span></asp:LinkButton>
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
</asp:Panel>
