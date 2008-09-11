<%@ Page Language="VB" MasterPageFile="~/aspmaker/masterpage.master" AutoEventWireup="false" CodeFile="Browse_Images.aspx.vb" Inherits="mhweb_catalog_Browse_Images" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <table>
      <tr>
        <td valign="top" width="230">
          <asp:Panel id="pnlThumbs" runat="server" height="700px" scrollbars="vertical" width="220px" borderstyle="Inset"  ></asp:Panel>
        </td>
          <td valign="top">
<div id="ewTable">

<input type="hidden" id="x_ImageID" runat="server" />
<input type="hidden" id="x_CompanyID" runat="server" />
<input type="hidden" id="x_ImageCount" runat="server" />
<input type="hidden" id="x_VersionNumber" runat="server" />
<input type="hidden" id="x_modifiedDT" runat="server" />
<table width="100%" border="1">
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Title</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageTitle" visible="TRUE" runat="server" Width="560px" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Image Name</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageName" visible="TRUE" runat="server" Width="557px" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Image Description</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageDescription" visible="TRUE" runat="server" Height="75px" Width="554px"  /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Image Comment</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageComment" visible="TRUE" runat="server" Height="87px" Width="553px" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Size</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageSize" visible="TRUE" runat="server" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Date</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageDate" visible="TRUE" runat="server" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Medium</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageMedium" visible="TRUE" runat="server" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Price</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImagePrice" visible="TRUE" runat="server" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Color</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageColor" visible="TRUE" runat="server" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Subject</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageSubject" visible="TRUE" runat="server" Width="557px" /></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Sold</td>
  <td class="ewTableRow" style="width: 571px"><asp:checkbox id="cbImageSold" visible="TRUE" runat="server" /></td>
</tr>

<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top"></td>
  <td class="ewTableRow" style="width: 571px"></td>
</tr>
<tr>
  <td class="ewTableHeader" style="width: 125px" valign="top">Image File</td>
  <td class="ewTableRow" style="width: 571px"><asp:textbox id="lblImageFileName" visible="TRUE" runat="server" Width="550px" /></td>
</tr>


</table>
<br /><br /><center>
<input type="submit" value="Update Database" id="Submit1" runat="server">
<br />
    <asp:label runat="server" id="Results" text="sdf"></asp:label>
</center>
</div>
</td>
<td valign="top" style="width: 451px">
<asp:Image ID="imgMain" runat="server" BorderStyle="Inset" BorderWidth="2px" ImageUrl="~/images/spacer.gif"   />
</td>
</tr>
</table>
</asp:Content>

