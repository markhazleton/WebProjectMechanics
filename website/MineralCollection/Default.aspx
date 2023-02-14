<%@ Page Title="Mineral Collection - Default" Language="VB" MasterPageFile="~/MineralCollection/MineralAdmin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="MineralCollection_Default" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $.get("http://api.bootswatch.com/3/", function (data) {
                var themes = data.themes;
                var select = $("select");
                select.show();
                $(".alert").toggleClass("alert-info alert-success");
                $(".alert h4").text("Success!");

                themes.forEach(function (value, index) {
                    select.append($("<option />")
                          .val(index)
                          .text(value.name));
                });

                select.change(function () {
                    var theme = themes[$(this).val()];
                    $("link").attr("href", theme.css);
                    $(".alert h4").text(theme.name);
                }).change();

            }, "json").fail(function () {
                $(".alert").toggleClass("alert-info alert-danger");
                $(".alert h4").text("Failure!");
            });

        });
    </script>
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
<h1>Mineral Collection Admin</h1>
</asp:Content>

