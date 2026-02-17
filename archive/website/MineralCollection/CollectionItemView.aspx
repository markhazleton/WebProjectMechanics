<%@ Page Title="" Language="VB" MasterPageFile="~/admin/runtime.master" AutoEventWireup="false" CodeFile="CollectionItemView.aspx.vb" Inherits="MineralCollection_CollectionItemView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <!--portfolio start-->
    <section id="collection">
        <div class="collection-header"></div>
        <!--work header-->
        <div class="container">

    <asp:panel id="pnlFilter" runat="server" cssclass="panel panel-default flippanel ">
        <div class="panel-heading"></div>
        <div class="panel-body">
            <div class="form">
                <div class="row ">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="form-group">
                            <asp:linkbutton id="cmd_Search" runat="server" text="Search" cssclass="btn btn-primary"></asp:linkbutton>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="LabelCollection" runat="server" Text="Collection">
                                <asp:DropDownList ID="ddlCollection" runat="server" CssClass="form-control"></asp:DropDownList>
                            </asp:Label>
                        <div class="form-group">
                            <asp:CheckBox id="cbShowFeatured" runat="server" Checked="False" Text="Show Featured" CssClass="form-control"></asp:CheckBox>
                        </div>
                        <div class="form-group">
                            <asp:CheckBox id="cbShowSold" runat="server" Checked="False" Text="Show Sold" CssClass="form-control"></asp:CheckBox>
                        </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>SpecimenNumber:</label><asp:TextBox ID="tbSpecimenNumber" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>Mineral:</label><asp:DropDownList ID="ddPrimaryMineral" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>Description:</label><asp:TextBox ID="tbDescription" runat="server" TextMode="SingleLine" CssClass="form-control"/>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>Mine:</label><asp:DropDownList ID="ddMineNM" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>City:</label><asp:DropDownList ID="ddCity" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>State:</label><asp:DropDownList ID="ddState" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                        <div class="form-group">
                            <label>Country:</label><asp:DropDownList ID="ddCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer"></div>
    </asp:panel>

    <asp:formview id="FormView1" runat="server" datakeynames="CollectionItemID" enablemodelvalidation="False" enabletheming="False" enableviewstate="False" renderoutertable="False" viewstatemode="Disabled">
        <ItemTemplate>
            <div class="panel panel-default ">
                <div class="panel-heading">
                    <%# wpmMineralCollection.Display.DisplayWithSold("Specimen #", Eval("SpecimenNumber"), Eval("IsSold"), Eval("IsFeatured")) %>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm=4 col-xs-4">
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Nickname", Eval("Nickname")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayLinkField("Primary Mineral", Eval("PrimaryMineralNM"),"MineralID",Eval("PrimaryMineralID") ) %>
                            </div>
                            <div class="form-group">
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Variety", Eval("MineralVariety")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Description", Eval("Description")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayLinkField("Mine", Eval("MineNM"),"MineNM",Eval("MineNM")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayLinkField("City", Eval("City"),"LocationCityID",Eval("LocationCityID")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayLinkField("State", Eval("StateNM"),"LocationStateID",Eval("LocationStateID")) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayLinkField("Country", Eval("CountryNM"),"LocationCountryID",Eval("LocationCountryID")) %>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm=6 col-xs-6">
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Height (CM)", Eval("HeightCm")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Width (CM)", Eval("WidthCm")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Depth (CM)", Eval("ThicknessCm")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Weight (KG)", Eval("WeightKg")) %>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm=6 col-xs-6">
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Height (IN)", Eval("HeightIn")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Width (IN)", Eval("WidthIn")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Depth (IN)", Eval("ThicknessIn")) %>
                                    <%# wpmMineralCollection.Display.DisplayNumericField("Weight (GR)", Eval("WeightGr")) %>
                                </div>
                            </div>
                            
                            <%# AdminSectionStart(isAdmin) %>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayCollectionItemID(Eval("CollectionItemID"), isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Date of Purchase", wpmMineralCollection.Display.DisplayDate( Eval("PurchaseDate")),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Purchased From", Eval("PurchasedFromCompanyNM"),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Show where purchased", Eval("ShowWherePurchased"),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Storage Location", Eval("StorageLocation"),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Former Collection", Eval("ExCollection"),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayField("Sale Date", wpmMineralCollection.Display.DisplayDate(Eval("SaleDT")),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayNumericField("Estimated Value", Eval("Value"),isAdmin) %>
                            </div>
                            <div class="form-group">
                                <%# wpmMineralCollection.Display.DisplayNumericField("Sale Price", Eval("SalePrice"),isAdmin) %>
                            </div>
                            <%# AdminSectionEnd(isAdmin) %>

                        </div>
                        <div class="col-lg-9 col-md-9 col-sm=9 col-xs-9" >


<asp:Repeater runat="server" id="rptItemImagesThumb" datasource=<%# Eval("Images") %> >
    <HeaderTemplate>
  	<!-- thumb navigation carousel -->
    <div class="row hidden-xs" id="slider-thumbs">
            <!-- thumb navigation carousel items -->
          <ul class="list-inline">
    </HeaderTemplate>
    <ItemTemplate>
          <li> <a id='carousel-selector-00<%# Eval("DisplayOrder") %>'  class='<%# GetThumbnailClassByOrder(Eval("DisplayOrder")) %>' >
            <img src='<%# GetThumbnailURL( Eval("ImageFileNM")) %>' class="img-responsive" style="max-height:60px;max-width:80px;">
          </a></li>
    </ItemTemplate>
    <FooterTemplate>
            </ul>
    </div>
    </FooterTemplate>
</asp:Repeater>


<asp:Repeater runat="server" id="rptItemImagesCarousel" datasource=<%# Eval("Images") %> >
    <HeaderTemplate>
    <!-- main slider carousel -->
        <div class="row" id="slider">
                <div class="" id="carousel-bounding-box">
                    <div id="myCarousel" class="carousel slide">
                        <!-- main slider carousel items -->
                        <div class="carousel-inner">

    </HeaderTemplate>
    <ItemTemplate>
        <div class='<%# GetCarouselImageClassByOrder(Eval("DisplayOrder")) %>' data-slide-number='00<%# Eval("DisplayOrder") %>'>
            <img src='<%# GetImageURL( Eval("ImageFileNM")) %>' class="img-responsive img_zoom" style="max-height:500px;max-width:1200px;">
        </div>
    </ItemTemplate>
    <FooterTemplate>
                        </div>
                        <!-- main slider carousel nav controls --> <a class="carousel-control left" href="#myCarousel" data-slide="prev">‹</a>
                        <a class="carousel-control right" href="#myCarousel" data-slide="next">›</a>
                    </div>
                </div>
        </div>
    <!--/main slider carousel-->
    </FooterTemplate>
</asp:Repeater>


                            
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <br />
                </div>
            </div>
        </ItemTemplate>
    </asp:formview>

    <asp:literal runat="server" id="litResults" />

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <asp:repeater id="rptCollectionItems" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-md-3 col-sm-3 col-xs-6 specimen " style="text-align:center;">
                            <a class="thumbnail" href='/MineralCollection/CollectionItemView.aspx?CollectionItemID=<%# Eval("CollectionItemID") %>'>
                                <img src='<%# GetThumbnailURL(Eval("ImageFileNM")) %>' class="img-responsive" style="max-height:120px;max-width:160px;">
                                <br />
                                Specimen # <%# Eval("SpecimenNumber") %><%# GetSold(Eval("IsSold"), Eval("IsFeatured")) %>
                                <br />
                                <%# Eval("PrimaryMineralNM")  %>
                                <br />
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:repeater>
            </div>
        </div>
    </div>
</div>        
</section>
</asp:Content>
