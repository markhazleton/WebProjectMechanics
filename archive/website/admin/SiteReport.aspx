<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="SiteReport.aspx.vb" Inherits="wpm_admin_SiteMapReport" Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <style>
        #navcontainer ul {
            margin: 0;
            padding: 0;
            list-style-type: none;
            text-align: center;
        }

            #navcontainer ul li {
                display: inline;
            }

                #navcontainer ul li a {
                    text-decoration: none;
                    padding: .2em 1em;
                    color: #fff;
                    background-color: #036;
                }

                    #navcontainer ul li a:hover {
                        color: #fff;
                        background-color: #369;
                    }
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <div id="navcontainer">
        <ul>
            <li><a href="/admin/SiteReport.aspx?ReportID=SiteReport">Site Report</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=NavigationAdmin">NavigationAdmin</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=LinkAdmin">Link Admin</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=ParameterAdmin">Parameter Admin</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=ImageAdmin">Image Report</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=PageAlias">PageAlias</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=SiteInventory">SiteInventory</a></li>
            <li><a href="/admin/SiteReport.aspx?ReportID=SiteProfile">SiteProfile</a></li>
        </ul>
    </div>
    <asp:Literal runat="server" Text="" ID="myContent" />
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">

    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
        });
    </script>

</asp:Content>
