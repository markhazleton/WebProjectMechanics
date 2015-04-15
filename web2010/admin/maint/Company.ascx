<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Company.ascx.vb" Inherits="admin_maint_Company" %>
<%@ Register Src="../UserControls/DisplayTable.ascx" TagName="DisplayTable" TagPrefix="uc1" %>

<uc1:DisplayTable ID="dtList" runat="server" />

<asp:Panel ID="pnlEdit" runat="server">

    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Edit Company ID:
                    <asp:Literal ID="CompanyIDLabel" runat="server" Text="" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                CompanyName:
                            <asp:TextBox ID="CompanyNameTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                SiteURL:
                            <asp:TextBox ID="SiteURLTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                SiteTitle:
                            <asp:TextBox ID="SiteTitleTextBox" runat="server" Text="" Width="100%" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                City:
                            <asp:TextBox ID="CityTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                Address:
                            <asp:TextBox ID="AddressTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                PostalCode:
                            <asp:TextBox ID="PostalCodeTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                StateOrProvince:
                            <asp:TextBox ID="StateOrProvinceTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                Country:
                            <asp:TextBox ID="CountryTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                PhoneNumber:
                            <asp:TextBox ID="PhoneNumberTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                FaxNumber:
                            <asp:TextBox ID="FaxNumberTextBox" runat="server" Text="" Width="100%" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 form-group">
                            <label>
                                DefaultPaymentTerms:
                            <asp:TextBox ID="DefaultPaymentTermsTextBox" runat="server" Text="" Width="100%" Rows="5" TextMode="MultiLine" /></label>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label>
                                DefaultInvoiceDescription:
                            <asp:TextBox ID="DefaultInvoiceDescriptionTextBox" runat="server" Text="" Width="100%" Rows="5" TextMode="MultiLine" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                GalleryFolder:
                            <asp:TextBox ID="GalleryFolderTextBox" runat="server" Text="" Width="100%" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                SiteTemplate:
                            <asp:DropDownList ID="SiteTemplateDropDown" runat="server" Width="100%" /></label>
                            <label>
                                DefaultSiteTemplate:
                            <asp:DropDownList ID="DefaultSiteTemplateDropDown" runat="server" Width="100%" /></label>
                            <label>
                                DefaultArticleID:
                            <asp:DropDownList ID="DefaultArticleIDDropDown" runat="server" Width="100%" /></label>
                            <label>
                                HomePageID:
                            <asp:DropDownList ID="HomePageIDDropDown" runat="server" Width="100%" /></label>
                            <label>
                                SiteCategoryTypeID:
                            <asp:TextBox ID="SiteCategoryTypeIDTextBox" runat="server" Text="" Width="100%" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                Component:
                            <asp:TextBox ID="ComponentTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                FromEmail:
                            <asp:TextBox ID="FromEmailTextBox" runat="server" Text="" Width="100%" /></label>
                            <label>
                                SMTP:
                            <asp:TextBox ID="SMTPTextBox" runat="server" Text="" Width="100%" /></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <label>
                                ActiveFL:
                            <asp:CheckBox ID="ActiveFLCheckBox" runat="server" Text="Is Active?" /><br />
                            </label>
                            <label>
                                UseBreadCrumbURL:
                            <asp:CheckBox ID="UseBreadCrumbURLCheckBox" runat="server" Text="User Breadcrumb URL" /><br />
                            </label>
                            <label>
                                SingleSiteGallery:
                            <asp:CheckBox ID="SingleSiteGalleryCheckBox" runat="server" Text="Single Site Gallery?" /><br />
                            </label>
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
                    <!-- /.row (nested) -->
                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>

</asp:Panel>
