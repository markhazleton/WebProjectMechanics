<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" AutoEventWireup="false" CodeFile="mhImageEdit.aspx.vb" Inherits="mhweb_mhImage" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<asp:Label id="lblAction" visible="TRUE" runat="server" />
<fieldset>
<legend>Image Edit</legend>
<asp:image id="imgThumbnail" runat="server" class="thumbnail" CssClass="thumbnail" ImageAlign="Right" ></asp:image>
<br class="clearAll" />
<label>Artist</label><asp:textbox id="tbImageArtist" visible="TRUE" runat="server" Width="200px" />
<br class="clearAll" />
<label>Title</label><asp:textbox id="tbImageTitle" visible="TRUE" runat="server" Width="450px" />
<br class="clearAll" />
<label>Name</label><asp:textbox id="tbImageName" visible="TRUE" runat="server" Width="450px" />
<br class="clearAll" />
<label>Description</label><asp:textbox id="tbImageDescription" visible="TRUE" runat="server" Width="450px" TextMode="MultiLine" ToolTip="Long description of the image to appear on image detail page" Rows="3" />
<br class="clearAll" /><br />
<label>Comment</label><asp:textbox id="tbImageComment" visible="TRUE" runat="server" Width="450px" Rows="3" TextMode="MultiLine" />
<br class="clearAll" /><br />
<label>Subject</label><asp:textbox id="tbImageSubject" visible="TRUE" runat="server" Width="450px" />
<br class="clearAll" /><br />
<label>Size</label><asp:textbox id="tbImageSize" visible="TRUE" runat="server" Width="275px" />
<br class="clearAll" />
<label>Medium</label><asp:textbox id="tbImageMedium" visible="TRUE" runat="server" Width="275px" />
<br class="clearAll" />
<label>Price</label><asp:textbox id="tbImagePrice" visible="TRUE" runat="server" Width="275px" />
<br class="clearAll" />
<label>Image Date</label><asp:textbox id="tbImageDate" visible="TRUE" runat="server" Width="275px" />
<br class="clearAll" />
<label>Color</label><asp:textbox id="tbImageColor" visible="TRUE" runat="server" Width="275px" />
<br class="clearAll" />
<strong>Image Status</strong><asp:checkbox id="cbImageActive" visible="TRUE" runat="server" /><label>Active</label>
<asp:checkbox id="cbImageSold" visible="TRUE" runat="server" name="checkbox1" /><label>Sold</label>
<br class="clearAll" /><br />
<label>Image File Name</label><asp:textbox id="tbImageFileName" visible="TRUE" runat="server" Width="350px" />
<br class="clearAll" />
<label>Thumbnail File Name</label><asp:textbox id="tbImageThumbFileName" visible="TRUE" runat="server" Width="350px" />
<br class="clearAll" />
  <input type="hidden" id="x_CompanyID" runat="server" />
  <input type="hidden" id="x_ImageID" runat="server" />
  <input type="hidden" id="x_ContactID" runat="server" />
  <input type="hidden" id="x_VersionNumber" runat="server" />
<p class="submit">
  <input type="submit" value="Save Changes" id="Submit1" runat="server" />
  <input type="submit" value="Cancel" id="Cancel" runat="server"/>
  <input type="submit" value="Delete" id="Delete" runat="server" />
</p>

</fieldset>
</asp:Content>
