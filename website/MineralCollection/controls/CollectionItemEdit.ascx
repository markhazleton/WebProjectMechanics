<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CollectionItemEdit.ascx.vb" Inherits="MineralCollection_CollectionItemEdit" %>
<%@ Register Src="CollectionItemMineralEdit.ascx" TagName="CollectionItemMineralEdit" TagPrefix="uc2" %>
<%@ Register Src="CollectionItemImageEdit.ascx" TagName="CollectionItemImageEdit" TagPrefix="uc2" %>
<%@ Register Src="~/admin/UserControls/AlertBox.ascx" TagPrefix="uc2" TagName="AlertBox" %>

<uc2:AlertBox runat="server" ID="AlertBox" />
<asp:Panel ID="pnlEdit" runat="server">
    <div class="form">
        <asp:HiddenField ID="hfCollectionItemID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />

        <asp:Panel ID="pnlCollectionItem" runat="server" CssClass="row">
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <asp:Label ID="lblddlCollection" runat="server" AssociatedControlID="ddlCollection">Collection:</asp:Label>
                    <asp:DropDownList ID="ddlCollection" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="LabeltbSpecimenNumber" runat="server" AssociatedControlID="tbSpecimenNumber">Specimen #:</asp:Label>
                    <asp:TextBox ID="tbSpecimenNumber" runat="server" CssClass="form-control" />
                    <asp:Label ID="LabeltbNickname" runat="server" AssociatedControlID="tbNickname">Nickname:</asp:Label>
                    <asp:TextBox ID="tbNickname" runat="server" CssClass="form-control" />
                    <asp:Label ID="LabelcbIsFeatured" runat="server" AssociatedControlID="cbIsFeatured">Featured:</asp:Label>
                    <asp:CheckBox ID="cbIsFeatured" runat="server" CssClass="form-control" />
                    <asp:Label ID="LabelcbIsSold" runat="server" AssociatedControlID="cbIsSold">Sold:</asp:Label>
                    <asp:CheckBox ID="cbIsSold" runat="server" CssClass="form-control" />

                </div>

                <div class="form-group">
                    <asp:Label ID="labelddPrimaryMineral" runat="server" AssociatedControlID="ddPrimaryMineral">Primary Mineral:</asp:Label>
                    <asp:DropDownList ID="ddPrimaryMineral" runat="server" CssClass="form-control"></asp:DropDownList>

                    <asp:Label ID="labeltbVariety" runat="server" AssociatedControlID="tbVariety">Variety:</asp:Label>
                    <asp:TextBox ID="tbVariety" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <asp:Label ID="LabeltbDescription" runat="server" AssociatedControlID="tbDescription">Description:</asp:Label>
                    <asp:TextBox ID="tbDescription" runat="server" Rows="3" TextMode="MultiLine" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <asp:Label ID="LabeltbSpecimenNotes" runat="server" AssociatedControlID="tbSpecimenNotes">Specimen Notes:</asp:Label>
                    <asp:TextBox ID="tbSpecimenNotes" runat="server" Rows="3" TextMode="MultiLine" CssClass="form-control" />
                </div>
            </div>
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <p class="help-block">Specimen Source Location Information.</p>
                    <asp:Label ID="labeltbMine_Name" runat="server" AssociatedControlID="tbMine_Name">Mine Name:</asp:Label>
                    <asp:TextBox ID="tbMine_Name" runat="server" CssClass="form-control" />
                    <asp:Label ID="labelddlLocationCity" runat="server" AssociatedControlID="ddlLocationCity">City:</asp:Label>
                    <asp:DropDownList ID="ddlLocationCity" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="labelddlLocationState" runat="server" AssociatedControlID="ddlLocationState">State or Province:</asp:Label>
                    <asp:DropDownList ID="ddlLocationState" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:Label ID="labelddlLocationCountry" runat="server" AssociatedControlID="ddlLocationCountry">Country:</asp:Label>
                    <asp:DropDownList ID="ddlLocationCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <p class="help-block">Specimen Aquisition Information.</p>
                    <asp:Label ID="labeltbDate_of_Purchase" runat="server" AssociatedControlID="tbDate_of_Purchase">Date of Purchase:</asp:Label>
                    <asp:TextBox ID="tbDate_of_Purchase" runat="server" CausesValidation="False" CssClass="form-control" />
                    <asp:Label ID="labeltbPurchase_Price" runat="server" AssociatedControlID="tbPurchase_Price">Purchase Price:</asp:Label>
                    <asp:TextBox ID="tbPurchase_Price" runat="server" Font-Overline="False" CssClass="form-control" />
                    <asp:Label ID="labeltbValue" runat="server" AssociatedControlID="tbValue">Estimated Value:</asp:Label>
                    <asp:TextBox ID="tbValue" runat="server" CssClass="form-control" />
                    <asp:Label ID="labeltbShow_Purchase" runat="server" AssociatedControlID="tbShow_Purchase">Show where Purchased:</asp:Label>
                    <asp:TextBox ID="tbShow_Purchase" runat="server" CssClass="form-control" />
                    <asp:Label ID="labelddPurchasedFromCompany" runat="server" AssociatedControlID="ddPurchasedFromCompany">Purchased From:</asp:Label>
                    <asp:DropDownList ID="ddPurchasedFromCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                </div>
                <div class="form-group">
                    <asp:Label ID="labeltbStorage_Location" runat="server" AssociatedControlID="tbStorage_Location">Storage Location:</asp:Label>
                    <asp:TextBox ID="tbStorage_Location" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label ID="labeltbEx_Collection" runat="server" AssociatedControlID="tbEx_Collection">Former Collection:</asp:Label>
                    <asp:TextBox ID="tbEx_Collection" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label ID="labeltbSaleDT" runat="server" AssociatedControlID="tbSaleDT">Sale Date:</asp:Label>
                    <asp:TextBox ID="tbSaleDT" runat="server" CausesValidation="False" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label ID="labeltbSalePrice" runat="server" AssociatedControlID="tbSalePrice">Sale Price:</asp:Label>
                    <asp:TextBox ID="tbSalePrice" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                <div class="form-group">
                    <p class="help-block">Specimen Dimensions.</p>
                    <div class="col-lg-6 col-md-6 col-xs-12">
                        <asp:Label ID="labeltbHeightCM" runat="server" AssociatedControlID="tbHeightCm">Height (CM):</asp:Label>
                        <asp:TextBox ID="tbHeightCm" runat="server" CssClass="form-control" />
                        <asp:Label ID="LabeltbWidthCm" runat="server" AssociatedControlID="tbWidthCm">Width (CM):</asp:Label>
                        <asp:TextBox ID="tbWidthCm" runat="server" CssClass="form-control" />
                        <asp:Label ID="LabeltbThicknessCm" runat="server" AssociatedControlID="tbThicknessCm">Depth (CM):</asp:Label>
                        <asp:TextBox ID="tbThicknessCm" runat="server" CssClass="form-control" />
                        <asp:Label ID="LabeltbWeightKg" runat="server" AssociatedControlID="tbWeightKg">Weight (KG):</asp:Label>
                        <asp:TextBox ID="tbWeightKg" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-lg-6 col-md-6 col-xs-12">
                        <asp:Label ID="labeltbHeightIn" runat="server" AssociatedControlID="tbHeightIn">Height (IN):</asp:Label>
                        <asp:TextBox ID="tbHeightIn" runat="server" CssClass="form-control" />

                        <asp:Label ID="labeltbWidthIn" runat="server" AssociatedControlID="tbWidthIn">Width (IN):</asp:Label>
                        <asp:TextBox ID="tbWidthIn" runat="server" CssClass="form-control" />

                        <asp:Label ID="labeltbThicknessIn" runat="server" AssociatedControlID="tbThicknessIn">Depth (IN):</asp:Label>
                        <asp:TextBox ID="tbThicknessIn" runat="server" CssClass="form-control" />

                        <asp:Label ID="labeltbWeightGr" runat="server" AssociatedControlID="tbWeightGr">Weight (GR):</asp:Label>
                        <asp:TextBox ID="tbWeightGr" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-xs-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">Specimen Images</div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlThumbnails" runat="server" CssClass="row">
                        </asp:Panel>
                        <div class="form-group" id="uploads"></div>
                    </div>
                    <div class="panel-footer">
                        <asp:LinkButton ID="cmd_RefreshImages" runat="server" Text="Refresh Images" CssClass="btn btn-primary" OnClick="cmd_RefreshImages_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="form-group">
                    <asp:LinkButton ID="cmd_Update" runat="server" Text="Update" CssClass="btn btn-primary" />
                    <asp:LinkButton ID="cmd_Insert" runat="server" Text="Insert" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:LinkButton ID="cmd_Cancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete" CssClass="btn btn-warning" />
                </div>
            </div>
        </asp:Panel>
        <uc2:CollectionItemMineralEdit ID="CollectionItemMineralEdit1" runat="server" />
        <uc2:CollectionItemImageEdit ID="CollectionItemImageEdit1" runat="server" />
        <br />
        <br />
</asp:Panel>
<br />
<br />
<br />
<br />
<br />
