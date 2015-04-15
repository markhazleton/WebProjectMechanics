<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"  CodeFile="BrowseImages.aspx.vb" Inherits="admin_catalog_BrowseImages" Title="Untitled Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <br clear="all" />
    <table width="100%">
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlThumbs" runat="server" Height="700px" ScrollBars="vertical" Width="400px"
                    BorderStyle="Inset">
                </asp:Panel>
            </td>
            <td valign="top">
                <div id="ewTable">
                    <input type="hidden" id="x_ImageID" runat="server" />
                    <input type="hidden" id="x_SubFolder" runat="server" />
                    <input type="hidden" id="x_CompanyID" runat="server" />
                    <input type="hidden" id="x_ImageCount" runat="server" />
                    <input type="hidden" id="x_VersionNumber" runat="server" />
                    <input type="hidden" id="x_modifiedDT" runat="server" />
                    <table width="100%" border="1">
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Title
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageTitle" Visible="TRUE" runat="server" Width="400px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Image Name
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageName" Visible="TRUE" runat="server" Width="557px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Image Description
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageDescription" Visible="TRUE" runat="server" Height="75px"
                                    Width="554px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Image Comment
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageComment" Visible="TRUE" runat="server" Height="87px" Width="553px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Date
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageDate" Visible="TRUE" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ewTableHeader" style="width: 125px" valign="top">
                                Image File
                            </td>
                            <td class="ewTableRow" style="width: 571px">
                                <asp:TextBox ID="lblImageFileName" Visible="TRUE" runat="server" Width="550px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Image ID="imgMain" runat="server" BorderStyle="Inset" BorderWidth="2px" ImageUrl="~/admin/images/spacer.gif" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <input type="submit" value="Update Database" id="Submit1" runat="server">
                        <br />
                        <asp:Label runat="server" ID="Results" Text="Results"></asp:Label>
                    </center>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
