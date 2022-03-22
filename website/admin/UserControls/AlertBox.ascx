<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AlertBox.ascx.vb" Inherits="controls_AlertBox" %>
<asp:Panel ID="NoApplications" runat="server" Visible="false">
    <div id="alertbox" runat="server" class="alert alert-dismissable alert-warning">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"></button>
        <button id="closebutton" runat="server" data-dismiss="alert" class="close" type="button"><span aria-hidden="true">&times;</span></button>
        <h4><%= boldnote%></h4>
        <p><%= message%></p>
    </div>
</asp:Panel>
