Imports System.Web

Public Class wpmHTML
    Public Shared Sub WriteTableCell(ByRef bCellIsTitle As Boolean, ByRef sContents As String)
        HttpContext.Current.Response.Write(vbTab & "<td valign=""top"">")
        If bCellIsTitle Then HttpContext.Current.Response.Write("<strong>")
        HttpContext.Current.Response.Write(sContents)
        If bCellIsTitle Then HttpContext.Current.Response.Write("</strong>")
        HttpContext.Current.Response.Write("</td>" & vbCrLf)
    End Sub
    Public Shared Sub WriteTableCellWidth(ByRef bCellIsTitle As Boolean, ByRef sContents As String, ByRef sWidth As String)
        HttpContext.Current.Response.Write(String.Format("{0}<td valign=""top"" width={1}>", vbTab, sWidth))
        If bCellIsTitle Then HttpContext.Current.Response.Write("<strong>")
        HttpContext.Current.Response.Write(sContents)
        If bCellIsTitle Then HttpContext.Current.Response.Write("</strong>")
        HttpContext.Current.Response.Write("</td>" & vbCrLf)
    End Sub
    Public Shared Function GetTableCell(ByRef bCellIsTitle As Boolean, ByRef sContents As String) As String
        Dim sReturn As String = (String.Format("{0}<td valign=""top"">", vbTab))
        If bCellIsTitle Then
            sReturn = String.Format("<strong>{0}{1}</strong>", sReturn, (sContents))
        Else
            sReturn = String.Format("{0}{1}", sReturn, (sContents))
        End If
        sReturn = String.Format("{0}</td>{1}", sReturn, vbCrLf)
        GetTableCell = sReturn
    End Function
    Public Shared Function GetTableCellWidth(ByRef bCellIsTitle As Boolean, ByRef sContents As String, ByRef sWidth As String) As String
        Dim sReturn As String = ""
        If bCellIsTitle Then
            sReturn = (String.Format("{0}<td valign=""top"" width={1}>", vbTab, sWidth))
        Else
            sReturn = (String.Format("{0}<td valign=""top"" width={1}>", vbTab, sWidth))
        End If
        If bCellIsTitle Then
            sReturn = sReturn & (sContents) & ("</strong>")
        Else
            sReturn = String.Format("{0}{1}", sReturn, (sContents))
        End If
        sReturn = sReturn & ("</td>" & vbCrLf)
        GetTableCellWidth = sReturn
    End Function
    Public Shared Function GetFormCheckbox(ByVal Value As Boolean, ByVal FieldName As String) As String
        Dim strReturn As String
        If Value Then
            strReturn = String.Format("<input type=""checkbox"" name=""{0}"" value=""True""  checked ", FieldName)
        Else
            strReturn = String.Format("<input type=""checkbox"" name=""{0}"" value=""True"" ", FieldName)
        End If
        strReturn = strReturn & ">"
        Return strReturn
    End Function
    Public Shared Function GetTextBox(ByVal Value As String, ByVal FieldName As String, ByVal BoxSize As String) As String
        Dim strReturn As String = String.Format("<input type=""text"" name=""{0}"" size=""{1}"" value=""{2}"">", FieldName, BoxSize, Value)
        GetTextBox = strReturn
    End Function
    Public Shared Function GetHiddenTextBox(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String = String.Format("<input type=""hidden"" name=""{0}"" size=""5"" value=""{1}"">{1}", FieldName, Value)
        GetHiddenTextBox = strReturn
    End Function
    Public Shared Function GetHiddenText(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String = String.Format("<input type=""hidden"" name=""{0}"" size=""5"" value=""{1}"">", FieldName, Value)
        GetHiddenText = strReturn
    End Function
    Public Shared Function GetPasswordTextBox(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String = String.Format("<input type=""password"" name=""{0}"" size=""5"" value=""{1}"">", FieldName, Value)
        GetPasswordTextBox = strReturn
    End Function
    Public Shared Function GetTextAreaBox(ByVal Value As String, ByVal FieldName As String, ByVal BoxWidth As String, ByVal BoxHeight As String) As String
        Dim strReturn As String = String.Format("<TEXTAREA NAME=""{0}"" COLS=""{1}"" ROWS=""{2}"">{3}</TEXTAREA>", FieldName, BoxWidth, BoxHeight, Value)
        GetTextAreaBox = strReturn
    End Function
End Class
