<%@ Page Title="" Language="VB" MasterPageFile="~/MineralCollection/MineralAdmin.master" AutoEventWireup="false" CodeFile="Admin.aspx.vb" Inherits="MineralCollection_Admin" %>

<%@ Register Src="controls/SpecimenFilter.ascx" TagName="SpecimenFilter" TagPrefix="uc1" %>
<%@ Register Src="controls/SpecimenList.ascx" TagName="SpecimenList" TagPrefix="uc2" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">





    <asp:Panel runat="server" ID="pnlSearchOptions" Visible="true">
        <span class="flip btn btn-default">Click here to Show/Hide Search Panel</span>
    </asp:Panel>

    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>



    <asp:Panel ID="pnlFilter" runat="server" CssClass="panel flippanel " Visible="true">
        <div class="row">
            <div class="col-lg-12 form-group">
                <asp:LinkButton ID="cmd_Search" runat="server" CssClass="btn btn-primary" OnClick="cmd_Search_Click"><span><span>Search</span></span></asp:LinkButton>
            </div>
        </div>
        <uc1:SpecimenFilter ID="SpecimenFilter1" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlMaintenance" runat="server"></asp:Panel>
    <uc2:SpecimenList ID="SpecimenList1" runat="server" Visible="false" />
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">
    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
            $(".flippanel").slideToggle("slow");
            $(".flip").click(function () {
                $(".flippanel").slideToggle("slow");
            });

            if ($("#uploads" + name).length == 0) {
                //it doesn't exist
            } else {
                var d = '<div id="dzFormDiv">';
                d += '  <form ';
                d += '      class="dropzone"';
                d += '      id="my-dropzone"><div class="dz-message">Drop Image Files at add to this Specimen</div>';
                d += '      <input type="hidden" id="dztoken" name="dztoken">  ';
                d += '      <input type="hidden" id="dzt2" name="dzt2"> ';
                d += '  </form>   ';
                d += '</div> ';
                $("#uploads").prepend(d);

                myAwesomeDropzone = new Dropzone("#my-dropzone", { url: "/MineralCollection/gallery/Upload.ashx" });
            }

        });

    </script>
</asp:Content>
