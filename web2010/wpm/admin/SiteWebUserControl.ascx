<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SiteWebUserControl.ascx.vb"
    Inherits="wpmSiteWebUserControl" %>
<style type="text/css">
    .style1
    {
        width: 166px;
    }
</style>
<table style="width: 700px">
    <tr>
        <td class="style1">
            <asp:Label ID="lblDomain" runat="server" Text="Domain"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbDomain" runat="server" Width="252px"></asp:TextBox><br />
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="lblCompanyID" runat="server" Text="CompanyID"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbCompanyID" runat="server"></asp:TextBox><br />
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="lblAccessDatabasePath" runat="server" Text="Access Data Path"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbAccessDatabasePath" runat="server" Width="512px"></asp:TextBox><br />
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="lblSQLDBConnString" runat="server" Text="SQL Connection String"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="tbSQLDBConnString" runat="server" Width="516px"></asp:TextBox><br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <center>
                <asp:Button ID="btnSave" runat="server" Text="Save" /> &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </center>
        </td>
    </tr>
</table>
