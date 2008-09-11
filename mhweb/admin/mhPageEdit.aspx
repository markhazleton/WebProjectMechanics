<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" AutoEventWireup="false" CodeFile="mhPageEdit.aspx.vb" Inherits="mhweb_admin_mhPageEdit" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
            <asp:FormView ID="FormView1" runat="server" AllowPaging="True" DataKeyNames="PageID"
                DataSourceID="CustomersDataSource" OnDataBound="FormView1_DataBound" OnItemUpdating="FormView1_ItemUpdating" CellPadding="4" ForeColor="#333333" Width="432px" FooterText="<br/>Record Navigation<br/><br/>" HeaderText="Edit Page">
<PagerSettings FirstPageText="&amp;lt;&amp;lt; First &amp;nbsp;" LastPageText="&amp;nbsp; Last &amp;gt;&amp;gt;" Mode="NextPreviousFirstLast" NextPageText="Next &amp;gt; &amp;nbsp;" PreviousPageText="&amp;lt; Prev &amp;nbsp;"></PagerSettings>

<FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White"></FooterStyle>

<RowStyle BackColor="#FFFBD6" ForeColor="#333333"></RowStyle>
<EditItemTemplate>

                    PageID:
                    <asp:Label ID="PageIDLabel1" runat="server" Text='<%# Eval("PageID") %>' />
                    <br />
                    Name:
                    <asp:TextBox ID="PageNameTextBox" runat="server" Text='<%# Bind("PageName") %>' />
                    <br />
                    
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Update">
                    </asp:LinkButton>

                    <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel">
                    </asp:LinkButton>
                
</EditItemTemplate>
<ItemTemplate>
PageID: <asp:Label id="CustomerIDLabel" runat="server" Text='<%# Eval("PageID") %>' __designer:wfdid="w9"></asp:Label> 
<BR />PageName: <asp:Label id="NameLabel" runat="server" Text='<%# Eval("PageName") %>' __designer:wfdid="w10"></asp:Label> <BR /><BR /><asp:LinkButton id="Edit" runat="server" CommandName="Edit" __designer:wfdid="w11">Edit</asp:LinkButton> 
</ItemTemplate>

<PagerStyle HorizontalAlign="Center" BackColor="#FFCC66" ForeColor="#333333"></PagerStyle>

<HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White"></HeaderStyle>
</asp:FormView>


<asp:AccessDataSource 
     ID="CustomersDataSource" 
     runat="server" 
     DataFile="~/access_db/frogsfolly.mdb"
     SelectCommand="SELECT [PageID], [PageName], [PageTitle], 
        [PageDescription] FROM [Page]"
     UpdateCommand="UPDATE [Page] SET [PageName] = ? WHERE [PageID] = ?">
     <UpdateParameters>
      <asp:Parameter Name="PageName" Type="String" />
      <asp:Parameter Name="PageID" Type="Int32" />
     </UpdateParameters>
</asp:AccessDataSource>



</asp:Content>

