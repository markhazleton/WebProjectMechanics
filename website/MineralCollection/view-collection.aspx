<%@ Page Title="" Language="VB" MasterPageFile="~/admin/runtime.master" AutoEventWireup="false" CodeFile="view-collection.aspx.vb" Inherits="MineralCollection_view_collection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">

    <!--collection start-->
    <section id="collection">
        <div class="collection-header">
        </div>
        <!--collection header-->
        <!--filter start-->
        <div class="container">
            <asp:repeater runat="server" id="rptItem">
                    <HeaderTemplate>
                        <div id="js-grid-awesome-work" class="cbp cbp-l-grid-work">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="cbp-item identity">
                            <a href="/MineralCollection/view-item.ashx?Specimen=<%# Eval("CollectionItemID") %>" class="cbp-caption cbp-singlePage" rel="nofollow">
                                <div class="cbp-caption-defaultWrap">
                                    <img src="<%# GetThumbnailURL(Eval("ImageFileNM")) %>" alt="">
                                </div>
                                <div class="cbp-caption-activeWrap"></div>
                            </a>
                            <a href="/MineralCollection/view-item.ashx?Specimen=<%# Eval("CollectionItemID") %>" class="cbp-l-grid-work-title cbp-singlePage" rel="nofollow">
                                <%# wpmMineralCollection.Display.DisplayWithSold("Specimen #", Eval("SpecimenNumber"), Eval("IsSold")) %>
                            </a>
                            <div class="cbp-l-grid-work-desc">
                                <%# wpmMineralCollection.Display.DisplayLinkField("Primary Mineral", Eval("PrimaryMineralNM"), "MineralID", Eval("PrimaryMineralID")) %>
                            </div>
                        </div><!--work item-->
                    </ItemTemplate>
                    <FooterTemplate>
        </div>
        </FooterTemplate>
                </asp:Repeater>
        </div><!--container end-->
    </section>
    <!--portfolio end-->


</asp:Content>

