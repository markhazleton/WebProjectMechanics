<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayTable.ascx.vb" Inherits="controls_DisplayTable" %>
<h2>
    <asp:Literal ID="tblTitle" runat="server"></asp:Literal></h2>
<div class="table-responsive">
    <table class="table table-striped table-bordered table-hover" id="dataTables-admin">
        <thead>
            <tr>
                <asp:Repeater ID="rptHeaderRow" runat="server">
                    <ItemTemplate>
                        <%# Container.DataItem%>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDataRows" runat="server">
                <ItemTemplate>
                    <tr><%# Container.DataItem%></tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
<asp:HiddenField ID="hfCSV" runat="server" />
