<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" AutoEventWireup="false" CodeFile="mhArticleEdit.aspx.vb" Inherits="aspmaker_fckeditor_mhImageEdit" title="Edit Article" EnableViewState="true" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server" >
<b>Title: </b><br />
<asp:textbox id="tbArticleTitle" runat="server" width="600" ></asp:textbox>
<FCKeditorV2:FCKeditor ID="FCKeditor1" BasePath="~/aspmaker/fckeditor/" runat="server" Height="600" ></FCKeditorV2:FCKeditor>

<asp:textbox id="tbArticleID" runat="server" ></asp:textbox> <br />
<asp:dropdownlist id="DropDownListContact" runat="server" ></asp:dropdownlist>
</asp:Content>

