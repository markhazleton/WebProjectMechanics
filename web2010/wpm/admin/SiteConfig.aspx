<%@ Page Title="" Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false" CodeFile="SiteConfig.aspx.vb" Inherits="SiteConfiguration_Default" %>

<%@ Register src="SiteWebUserControl.ascx" tagname="SiteWebUserControl" tagprefix="ucsite" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" Runat="Server">
  <asp:Panel ID="PetSelect" runat="server" BorderColor="Silver" BackColor="Silver"
        BorderStyle="Solid" BorderWidth="1">
        <asp:DropDownList ID="ddPets" runat="server">
        </asp:DropDownList>
        &nbsp;&nbsp;
        <asp:Button ID="btnEdit" runat="server" Text="Edit" />&nbsp;&nbsp;
        <asp:Button ID="btnDelete" runat="server" Text="Delete" />&nbsp;&nbsp;
    </asp:Panel>
      <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="PetEdit" runat="server" BackColor="#FFFFCC" BorderColor="Black" 
          BorderStyle="Solid">
        <ucsite:SiteWebUserControl ID="SiteWebUserControl1" runat="server" />
    </asp:Panel>
    
    </asp:Content>

