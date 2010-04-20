Imports System.Web
Imports System.Xml.Serialization

Public Enum wpmSortDirection
    ASC
    DESC
End Enum

Public Class wpmHTML
    Public Shared Sub WriteTableCell(ByRef bCellIsTitle As Boolean, ByRef sContents As Object)
        HttpContext.Current.Response.Write(vbTab & "<td valign=""top"">")
        If bCellIsTitle Then HttpContext.Current.Response.Write("<strong>")
        HttpContext.Current.Response.Write(sContents)
        If bCellIsTitle Then HttpContext.Current.Response.Write("</strong>")
        HttpContext.Current.Response.Write("</td>" & vbCrLf)
    End Sub
    Public Shared Sub WriteTableCellWidth(ByRef bCellIsTitle As Boolean, ByRef sContents As Object, ByRef sWidth As String)
        HttpContext.Current.Response.Write(vbTab & "<td valign=""top"" width=" & sWidth & ">")
        If bCellIsTitle Then HttpContext.Current.Response.Write("<strong>")
        HttpContext.Current.Response.Write(sContents)
        If bCellIsTitle Then HttpContext.Current.Response.Write("</strong>")
        HttpContext.Current.Response.Write("</td>" & vbCrLf)
    End Sub
    Public Shared Function GetTableCell(ByRef bCellIsTitle As Boolean, ByRef sContents As String) As String
        Dim sReturn As String = ""
        sReturn = (vbTab & "<td valign=""top"">")
        If bCellIsTitle Then sReturn = sReturn & ("<strong>")
        sReturn = sReturn & (sContents)
        If bCellIsTitle Then sReturn = sReturn & ("</strong>")
        sReturn = sReturn & ("</td>" & vbCrLf)
        GetTableCell = sReturn
    End Function
    Public Shared Function GetTableCellWidth(ByRef bCellIsTitle As Boolean, ByRef sContents As String, ByRef sWidth As String) As String
        Dim sReturn As String = ""
        sReturn = (vbTab & "<td valign=""top"" width=" & sWidth & ">")
        If bCellIsTitle Then sReturn = sReturn & ("<strong>")
        sReturn = sReturn & (sContents)
        If bCellIsTitle Then sReturn = sReturn & ("</strong>")
        sReturn = sReturn & ("</td>" & vbCrLf)
        GetTableCellWidth = sReturn
    End Function
    Public Shared Function GetFormCheckbox(ByVal Value As Boolean, ByVal FieldName As String) As String
        Dim strReturn As String
        strReturn = "<input type=""checkbox"" name=""" & FieldName & """ value=""True"" "
        If (Value) Then
            strReturn = strReturn & " checked "
        End If
        strReturn = strReturn & ">"
        Return strReturn
    End Function
    Public Shared Function GetTextBox(ByVal Value As String, ByVal FieldName As String, ByVal BoxSize As String) As String
        Dim strReturn As String
        strReturn = "<input type=""text"" name=""" & FieldName & """ size=""" & BoxSize & """ value=""" & Value & """>"
        GetTextBox = strReturn
    End Function
    Public Shared Function GetHiddenTextBox(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String
        strReturn = "<input type=""hidden"" name=""" & FieldName & """ size=""5"" value=""" & Value & """>" & Value
        GetHiddenTextBox = strReturn
    End Function
    Public Shared Function GetHiddenText(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String
        strReturn = "<input type=""hidden"" name=""" & FieldName & """ size=""5"" value=""" & Value & """>"
        GetHiddenText = strReturn
    End Function
    Public Shared Function GetPasswordTextBox(ByVal Value As String, ByVal FieldName As String) As String
        Dim strReturn As String
        strReturn = "<input type=""password"" name=""" & FieldName & """ size=""5"" value=""" & Value & """>"
        GetPasswordTextBox = strReturn
    End Function
    Public Shared Function GetTextAreaBox(ByVal Value As String, ByVal FieldName As String, ByVal BoxWidth As String, ByVal BoxHeight As String) As String
        Dim strReturn As String
        strReturn = "<TEXTAREA NAME=""" & FieldName & """ COLS=""" & BoxWidth & """ ROWS=""" & BoxHeight & """>" & Value & "</TEXTAREA>"
        GetTextAreaBox = strReturn
    End Function
End Class

<Serializable()> Public Class wpmSiteSettings
    Public wpmSite As wpmSiteClass

    Public Sub New()
        wpmSite = New wpmSiteClass
    End Sub

    Shared Function Load(ByVal fname As String) As wpmSiteSettings
        If (fname.Trim) = String.Empty Then
            Return New wpmSiteSettings
        Else
            If wpmFileIO.IsValidPath(fname) Then
                Dim sr As New StreamReader(fname)
                Try
                    Dim xs As New XmlSerializer(GetType(wpmSiteSettings))
                    Return DirectCast(xs.Deserialize(sr), wpmSiteSettings)
                Finally
                    sr.Close()
                End Try
            Else
                Return New wpmSiteSettings
            End If
        End If
    End Function

    Shared Sub Save(ByVal fname As String, ByVal obj As wpmSiteSettings)
        Dim sw As New StreamWriter(fname)
        Try
            Dim xs As New XmlSerializer(GetType(wpmSiteSettings))
            xs.Serialize(sw, obj)
        Finally
            sw.Close()
        End Try
    End Sub
End Class

<Serializable()> _
Public Class wpmSiteClass
    Public CompanyID As Integer
    Public SQLDBConnString As String = String.Empty
End Class
