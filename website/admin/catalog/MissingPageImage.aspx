<%@ Page Language="VB" MasterPageFile="~/admin/admin2.master" ValidateRequest="false"
    AutoEventWireup="false" CodeFile="MissingPageImage.aspx.vb" Inherits="MissingPageImage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function SetAllCheckBoxes(FormName, FieldName, CheckValue) {
            if (!document.forms[FormName])
                return;
            var objCheckBoxes = document.forms[FormName].elements[FieldName];
            if (!objCheckBoxes)
                return;
            var countCheckBoxes = objCheckBoxes.length;
            if (!countCheckBoxes)
                objCheckBoxes.checked = CheckValue;
            else
                // set the check value for all check boxes
                for (var i = 0; i < countCheckBoxes; i++)
                    objCheckBoxes[i].checked = CheckValue;
        }

    </script>
    <style type="text/css">
        #updated {
            BORDER-RIGHT: #333 1px dashed;
            PADDING-RIGHT: 5px;
            BORDER-TOP: #333 1px dashed;
            PADDING-LEFT: 5px;
            PADDING-BOTTOM: 5px;
            MARGIN: 10px auto;
            BORDER-LEFT: #333 1px dashed;
            PADDING-TOP: 5px;
            BORDER-BOTTOM: #333 1px dashed;
            TEXT-ALIGN: center;
        }

        #info {
            BORDER-RIGHT: white 1px dashed;
            PADDING-RIGHT: 10px;
            BORDER-TOP: white 1px dashed;
            MARGIN-TOP: 10px;
            PADDING-LEFT: 10px;
            BACKGROUND: white;
            FLOAT: right;
            PADDING-BOTTOM: 10px;
            BORDER-LEFT: white 1px dashed;
            WIDTH: 350px;
            PADDING-TOP: 10px;
            BORDER-BOTTOM: white 1px dashed;
            TEXT-ALIGN: center;
        }

        #page {
            WIDTH: 800px;
            TEXT-ALIGN: center;
        }

        #container {
            BORDER-RIGHT: white 1px dashed;
            PADDING-RIGHT: 10px;
            BORDER-TOP: white 1px dashed;
            PADDING-LEFT: 10px;
            BACKGROUND: white;
            FLOAT: left;
            PADDING-BOTTOM: 10px;
            MARGIN: 10px auto;
            BORDER-LEFT: white 1px dashed;
            WIDTH: 372px;
            PADDING-TOP: 10px;
            BORDER-BOTTOM: white 1px dashed;
            TEXT-ALIGN: center;
        }

            #container SPAN {
                FONT-SIZE: 9px;
            }

        #images {
            BORDER-RIGHT: #666 1px solid;
            PADDING-RIGHT: 10px;
            BORDER-TOP: #666 1px solid;
            PADDING-LEFT: 10px;
            PADDING-BOTTOM: 10px;
            MARGIN: 0px auto;
            BORDER-LEFT: #666 1px solid;
            WIDTH: 330px;
            PADDING-TOP: 10px;
            BORDER-BOTTOM: #666 1px solid;
            TEXT-ALIGN: center;
        }

        #imageCaption {
            BORDER-RIGHT: #666 1px dashed;
            PADDING-RIGHT: 10px;
            BORDER-TOP: #666 1px dashed;
            PADDING-LEFT: 10px;
            PADDING-BOTTOM: 10px;
            MARGIN: 5px auto 0px;
            BORDER-LEFT: #666 1px dashed;
            WIDTH: 330px;
            PADDING-TOP: 10px;
            BORDER-BOTTOM: #666 1px dashed;
            TEXT-ALIGN: center;
        }

        .thumb {
            BORDER-RIGHT: #aaa 1px dotted;
            PADDING-RIGHT: 3px;
            BORDER-TOP: #aaa 1px dotted;
            PADDING-LEFT: 3px;
            FLOAT: left;
            PADDING-BOTTOM: 3px;
            MARGIN: 5px;
            BORDER-LEFT: #aaa 1px dotted;
            WIDTH: 220px;
            HEIGHT: 220px;
            PADDING-TOP: 3px;
            BORDER-BOTTOM: #aaa 1px dotted;
            TEXT-ALIGN: center;
            FONT-SIZE: 9px;
        }

        .hidden {
            DISPLAY: none;
        }
    </style>



</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="Server">

   <div style="clear: both;">
        <asp:Literal ID="litOne" runat="server"></asp:Literal>
        <br style="clear: both;" />
        <asp:Literal ID="litTwo" runat="server"></asp:Literal>
    </div>
    <div style="clear: both;">
        <br style="clear: both;" />
    </div>
</asp:Content>
