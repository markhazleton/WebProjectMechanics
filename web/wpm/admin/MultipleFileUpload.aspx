<%@ Page Language="VB" MasterPageFile="~/wpmgen/masterpage.master" AutoEventWireup="false" CodeFile="MultipleFileUpload.aspx.vb" Inherits="wpm_admin_MultipleFileUpload" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">


<ASP:PANEL id="MultipleFileUploadForm"     runat="server"  visible="true">
   <FORM id="Form1" method="post" encType="multipart/form-data" runat="server">
    <H1>ASP.NET Multiple File Upload Example</H1>
    <P>Select the Files to Upload to the Server:
       <BR>
      <INPUT id="File1" type="file" name="File1" runat="server">
    </P>
    <P>
    <INPUT id="File2"  type="file" name="File2" runat="server"></P>
        <P>
       <INPUT id="Submit1" type="submit" value="Upload Files" name="Submit1" runat="server" OnServerClick="UploadMultipleFiles_Clicked">
    </P>
   </FORM>
</ASP:PANEL>
<ASP:LABEL id="ResultMsg" runat="server" Visible="False" ForeColor="#ff0033"></ASP:LABEL>





</asp:Content>

