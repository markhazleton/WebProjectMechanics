<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SpecimenImageList.ascx.vb" Inherits="MineralCollection_controls_SpecimenImageList" %>
<%@ Register Src="~/admin/UserControls/DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>
<%@ Register Src="~/MineralCollection/gallery/SpecimenImageForm.ascx" TagPrefix="uc1" TagName="SpecimenImageForm" %>


<div class="row">
    <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
        <div class="form-group">
            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:LinkButton ID="cmd_FileUpload" runat="server" CssClass="btn btn-primary" OnClick="cmd_FileUpload_Click" Text="Upload File"></asp:LinkButton>
        </div>
        <asp:Literal ID="litUpload" runat="server"></asp:Literal>
    </div>
</div>

<uc1:DisplayTable ID="dtList" runat="server" />

<uc1:SpecimenImageForm runat="server" ID="SpecimenImageForm" />
