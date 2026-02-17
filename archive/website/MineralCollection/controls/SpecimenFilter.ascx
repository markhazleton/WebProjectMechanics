<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SpecimenFilter.ascx.vb" Inherits="MineralCollection_SpecimenFilter" %>

<div class="row">
    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-heading">
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-6 form-group">
                        <asp:Label ID="LabelCollection" runat="server" Text="Collection" AssociatedControlID="ddlCollection"></asp:Label>
                        <asp:DropDownList ID="ddlCollection" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabeltbSpecimenNumber" AssociatedControlID="tbSpecimenNumber" runat="server" Text="Specimen #"></asp:Label>
                        <asp:TextBox ID="tbSpecimenNumber" runat="server" Width="100%" />

                        <asp:Label ID="LabelddPrimaryMineral" AssociatedControlID="ddPrimaryMineral" runat="server" Text="Primary Mineral"></asp:Label>
                        <asp:DropDownList ID="ddPrimaryMineral" runat="server" Width="100%"></asp:DropDownList>
                        
                        <asp:Label ID="LabeltbMineralVariety" AssociatedControlID="tbMineralVariety" runat="server" Text="Mineral Variety"></asp:Label>
                        <asp:TextBox ID="tbMineralVariety" runat="server" TextMode="SingleLine" Width="100%" />

                        <asp:Label ID="LabeltbDescription" AssociatedControlID="tbDescription" runat="server" Text="Description"></asp:Label>
                        <asp:TextBox ID="tbDescription" runat="server" TextMode="SingleLine" Width="100%" />

                        <asp:Label ID="lblStorageLocation" runat="server" Text="Storage Location:" AssociatedControlID="ddStorageLocation"></asp:Label>
                        <asp:DropDownList ID="ddStorageLocation" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabelddShowWherePurchased" runat="server" Text="Show Where Purchased:" AssociatedControlID="ddShowWherePurchased"></asp:Label>
                        <asp:DropDownList ID="ddShowWherePurchased" runat="server" Width="100%"></asp:DropDownList>

                    </div>
                    <div class="col-lg-6 form-group">
                        <asp:Label ID="LabelddPurchasedFromCompany" runat="server" Text="Purchased From:" AssociatedControlID="ddPurchasedFromCompany"></asp:Label>
                        <asp:DropDownList ID="ddPurchasedFromCompany" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabelddMineNM" runat="server" Text="Mine:" AssociatedControlID="ddMineNM"></asp:Label>
                        <asp:DropDownList ID="ddMineNM" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabelddCity" runat="server" Text="City:" AssociatedControlID="ddCity"></asp:Label>
                        <asp:DropDownList ID="ddCity" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabelddState" runat="server" Text="State:" AssociatedControlID="ddState"></asp:Label>
                        <asp:DropDownList ID="ddState" runat="server" Width="100%"></asp:DropDownList>

                        <asp:Label ID="LabelddCountry" runat="server" Text="Country:" AssociatedControlID="ddCountry"></asp:Label>
                        <asp:DropDownList ID="ddCountry" runat="server" Width="100%"></asp:DropDownList>
                    </div>
                    <!-- /.row (nested) -->
                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>
        <!-- /.row -->
</div>
