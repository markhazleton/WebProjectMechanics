<%@ Page Language="VB" MasterPageFile="~/aspmaker/masterpage.master" AutoEventWireup="false" CodeFile="mhImageEdit.aspx.vb" Inherits="mhweb_mhImage" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<fieldset style="width:1024px;">
<asp:Label id="lblAction" visible="TRUE" runat="server" />
<legend>Image Edit</legend>
<table width="1024px" style="border:none;"><tr><td style="width: 500px">
<asp:image id="imgThumbnail" runat="server" class="thumbnail" ImageAlign="Left" ></asp:image>
<br style="clear:both"/>
<label>Artist</label><asp:textbox id="tbImageArtist" visible="TRUE" runat="server" Width="450px" />
<label>Title</label><asp:textbox id="tbImageTitle" visible="TRUE" runat="server" Width="450px" />
<label>Name</label><asp:textbox id="tbImageName" visible="TRUE" runat="server" Width="450px" />
<label>Description</label><asp:textbox id="tbImageDescription" visible="TRUE" runat="server" Width="450px" TextMode="MultiLine" ToolTip="Long description of the image to appear on image detail page" Rows="3" />
<label>Comment</label><asp:textbox id="tbImageComment" visible="TRUE" runat="server" Width="450px" Rows="3" TextMode="MultiLine" />
</td><td style="width: 591px">
<label>Subject</label><asp:textbox id="tbImageSubject" visible="TRUE" runat="server" Width="450px" />
<label>Size</label><asp:textbox id="tbImageSize" visible="TRUE" runat="server" Width="450px" />
<label>Medium</label><asp:textbox id="tbImageMedium" visible="TRUE" runat="server" Width="450px" />
<label>Price</label><asp:textbox id="tbImagePrice" visible="TRUE" runat="server" Width="450px" />
<label>Image Date</label><asp:textbox id="tbImageDate" visible="TRUE" runat="server" Width="450px" />
<label>Color</label><asp:textbox id="tbImageColor" visible="TRUE" runat="server" Width="450px" />
<br style="clear:both"/>
<br style="clear:both"/>
<br style="clear:both"/>
<asp:checkbox id="cbImageActive" visible="TRUE" runat="server" text="Active" />
<br style="clear:both"/>
<asp:checkbox id="cbImageSold" visible="TRUE" runat="server" name="checkbox1" text="Sold" />
<br style="clear:both"/>
<br style="clear:both"/>
<label>Image File Name</label><asp:textbox id="tbImageFileName" visible="TRUE" runat="server" Width="450px" />
<label>Thumbnail File Name</label><asp:textbox id="tbImageThumbFileName" visible="TRUE" runat="server" Width="450px" />
</td></tr><tr><td colspan="2">
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
<br style="clear:both"/>
<br style="clear:both"/>
</td></tr></table>
</fieldset>
</asp:Content>
