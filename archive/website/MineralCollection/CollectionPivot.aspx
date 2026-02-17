<%@ Page Title="" Language="VB" MasterPageFile="~/MineralCollection/MineralAdmin.master" AutoEventWireup="false" CodeFile="CollectionPivot.aspx.vb" Inherits="MineralCollection_CollectionPivot" %>

<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://bootswatch.com/yeti/bootstrap.css" />
    <style type="text/css">
        body {
            padding-top: 70px;
        }

        .container {
            width: 98%;
        }

        a {
            color: #0c0787;
            text-decoration: none;
        }

            a:hover,
            a:focus {
                color: #b200ff;
                text-decoration: underline;
            }

            a:focus {
                outline: thin dotted;
                outline: 5px auto -webkit-focus-ring-color;
                outline-offset: -2px;
            }

        * {
            font-family: Verdana;
        }
    </style>

    <!-- external libs from cdnjs -->
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.css">
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.min.css">

    <!-- PivotTable.js libs from ../dist -->
    <link rel="stylesheet" type="text/css" href="/pivottable/dist/pivot.css" />
    <script type="text/javascript" src="/pivottable/dist/pivot.js"></script>
    <script type="text/javascript" src="/pivottable/dist/export_renderers.js"></script>
    <script type="text/javascript" src="/pivottable/dist/d3_renderers.js"></script>
    <script type="text/javascript" src="/pivottable/dist/c3_renderers.js"></script>


    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.5/d3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui-touch-punch/0.2.3/jquery.ui.touch-punch.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-csv/0.71/jquery.csv-0.71.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.jquery.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div id="output" style="margin: 10px;"></div>
    <br />
    <uc1:DisplayTable runat="server" ID="dtList" EnableViewState="false" />

    <asp:TextBox ID="tbJSON" runat="server" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
    <script type="text/javascript" src="/Scripts/pivottable/examples/ext/jquery-ui-1.9.2.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/pivottable/dist/pivot.js"></script>
    <script type="text/javascript">
        $(function () {

            var derivers = $.pivotUtilities.derivers;

            var renderers = $.extend(
                $.pivotUtilities.renderers,
                $.pivotUtilities.c3_renderers,
                $.pivotUtilities.d3_renderers,
                $.pivotUtilities.export_renderers
                );

            var input = $("#dataTables-admin")
            $("#output").pivotUI(input,
            {
                renderers: renderers,
                rows: ["Collection"],
                cols: [""],
                vals: ["Purchase Price"],
                exclusions: [],
                unusedAttrsVertical: "auto",
                autoSortUnusedAttrs: false,
                aggregatorName: "Sum",
                rendererName: "Table",
                onRefresh: function (config) {
                    var config_copy = JSON.parse(JSON.stringify(config));
                    //delete some values which are functions
                    delete config_copy["aggregators"];
                    delete config_copy["renderers"];
                    delete config_copy["derivedAttributes"];
                    //delete some bulky default values
                    delete config_copy["rendererOptions"];
                    delete config_copy["localeStrings"];
                    //$("#config_json").text(JSON.stringify(config_copy, undefined, 2));
                    $("#tbJSON").text(JSON.stringify(config_copy, undefined, 2));
                }
            });
        });
    </script>
</asp:Content>

