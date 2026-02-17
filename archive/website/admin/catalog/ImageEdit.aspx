<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" AutoEventWireup="false"
    CodeFile="ImageEdit.aspx.vb" Inherits="Admin_LocationImageEdit" Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">
    <fieldset style="width: 1024px;">
        <asp:Label ID="lblAction" Visible="TRUE" runat="server" />
        <legend>Image Edit</legend>
        <table width="1024px" style="border: none;">
            <tr>
                <td style="width: 500px">
                    <asp:Image ID="imgThumbnail" runat="server" class="thumbnail" ImageAlign="Left"></asp:Image>
                    <br style="clear: both" />
                    <br style="clear: both" />
                    <label>Title</label><br />
                    <asp:TextBox ID="tbImageTitle" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>Name</label><br />
                    <asp:TextBox ID="tbImageName" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>Description</label><br />
                    <asp:TextBox ID="tbImageDescription" Visible="TRUE" runat="server"
                            Width="450px" TextMode="MultiLine" ToolTip="Long description of the image to appear on image detail page"
                            Rows="3" /><br />
                    <label>Comment</label><br />
                    <asp:TextBox ID="tbImageComment" Visible="TRUE" runat="server" Width="450px"
                            Rows="3" TextMode="MultiLine" /><br />
                </td>
                <td style="width: 591px">
                    <label>
                        Subject</label><br />
                    <asp:TextBox ID="tbImageSubject" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>
                        Size</label><br />
                    <asp:TextBox ID="tbImageSize" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>
                        Medium</label><br />
                    <asp:TextBox ID="tbImageMedium" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>
                        Price</label><br />
                    <asp:TextBox ID="tbImagePrice" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>
                        Image Date</label><br />
                    <asp:TextBox ID="tbImageDate" Visible="TRUE" runat="server" Width="450px" /><br />
                    <label>
                        Color</label><br />
                    <asp:TextBox ID="tbImageColor" Visible="TRUE" runat="server" Width="450px" /><br />
                    <asp:CheckBox ID="cbImageActive" Visible="TRUE" runat="server" Text="Active" />
                    <br style="clear: both" />
                    <asp:CheckBox ID="cbImageSold" Visible="TRUE" runat="server" name="checkbox1" Text="Sold" />
                    <br style="clear: both" />
                    <label>
                        Image File Name</label><br />
                    <asp:TextBox ID="tbImageFileName" Visible="TRUE" runat="server"  Width="450px" /><br />
                    <label>Thumbnail File Name</label><br />
                    <asp:TextBox ID="tbImageThumbFileName" Visible="TRUE" runat="server" Width="450px" /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br class="clearAll" />
                    <input type="hidden" id="x_CompanyID" runat="server" />
                    <input type="hidden" id="x_ImageID" runat="server" />
                    <input type="hidden" id="x_ContactID" runat="server" />
                    <input type="hidden" id="x_VersionNumber" runat="server" />
                    <p class="submit">
                        <input type="submit" value="Save Changes" id="Submit1" runat="server" />
                        <input type="submit" value="Cancel" id="Cancel" runat="server" />
                        <input type="submit" value="Delete" id="Delete" runat="server" />
                    </p>
                    <br style="clear: both" />
                    <br style="clear: both" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
