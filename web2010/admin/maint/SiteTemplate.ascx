<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteTemplate.ascx.vb" Inherits="admin_maint_SiteTemplate" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>
<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server" CssClass="">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Template Code:<asp:Literal ID="litTemplateCD" runat="server"></asp:Literal>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>Template Code<asp:TextBox ID="tbTemplateCD" runat="server" Width="100%" Wrap="False"></asp:TextBox></label><br />
                            <label>Template Name<asp:TextBox ID="tbTemplateNM" runat="server" Width="100%"></asp:TextBox></label><br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                Top<br />
                                <asp:TextBox ID="tbTop" runat="server" TextMode="MultiLine" Width="800px" Rows="20"></asp:TextBox></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                Bottom<br />
                                <asp:TextBox ID="tbBottom" runat="server" TextMode="MultiLine" Width="800px" Rows="20"></asp:TextBox></label>
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
</asp:Panel>


