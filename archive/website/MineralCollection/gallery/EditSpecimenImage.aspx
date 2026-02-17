<%@ Page Language="VB" MasterPageFile="~/MineralCollection/MineralAdmin.master" AutoEventWireup="false" CodeFile="EditSpecimenImage.aspx.vb" Inherits="Gallery_EditSpecimenImage" Title="Gallery_EditSpecimenImage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false"></asp:Panel>


    <div class="form">
        <asp:HiddenField ID="hfCollectionItemImageID" runat="server" />
        <asp:HiddenField ID="hfModifiedID" runat="server" />
        <asp:HiddenField ID="hfModifiedDT" runat="server" />
        <div class="row">
            <div class="col-lg-6 col-md-6 col-xs-6">
                <div class="form-group">
                    <label for="ddlCollection">Collection:</label>
                    <asp:DropDownList ID="ddlCollection" runat="server" CssClass="form-control"></asp:DropDownList>
                    <br />
                    <label for="">Specimen #</label>
                    <asp:DropDownList ID="ddlCollectionItemID" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlImageType">Image Type</label>
                    <asp:DropDownList ID="ddlImageType" runat="server" CssClass="form-control">
                        <asp:ListItem>Photo</asp:ListItem>
                        <asp:ListItem>Label</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="tbDisplayOrder">Display Order</label>
                    <asp:DropDownList ID="ddDisplayOrder" runat="server" CssClass="form-control">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="tbImageNM">Image Name</label>
                    <asp:TextBox ID="tbImageNM" runat="server" CssClass="form-control" Width="100%" TextMode="SingleLine"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="tbImageDS">Image Description</label>
                    <asp:TextBox ID="tbImageDescription" Visible="true" runat="server" Height="75px" Width="100%" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="tbImageFileNM">Image File Name</label>
                    <asp:TextBox ID="tbImageFileNM" runat="server" CssClass="form-control" Width="100%" TextMode="SingleLine"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="cmd_SaveChanges" Text="Save Changes" OnClick="cmd_SaveChanges_Click" runat="server" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="cmd_DeleteFile" Text="Delete Image" onclick="cmd_DeleteFile_Click" runat="server" CssClass="btn btn-warning" ToolTip="This will delete the image file and remove all references to the specimen if necessary"></asp:LinkButton>
                    <asp:LinkButton ID="cmd_Cancel" Text="Cancel Changes"  OnClick="cmd_Cancel_Click" runat="server" CssClass="btn btn-default"></asp:LinkButton>
                   
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-xs-6">
                <div class="form-group">
                    <asp:Image runat="server" ID="imgSpecimen" CssClass="img-responsive" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>
