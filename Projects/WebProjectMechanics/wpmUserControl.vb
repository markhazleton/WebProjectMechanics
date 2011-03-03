Imports System.Web.UI.WebControls

Public Class wpmUserControl
    Inherits System.Web.UI.UserControl
    Public Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function
    Protected Function GetIntegerProperty(ByVal myProperty As String, ByVal curValue As Integer) As Integer
        Dim myValue As Integer
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = CInt(Request.QueryString(myProperty))
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = CInt(Request.Form.Item(myProperty))
        Else
            myValue = curValue
        End If
        Return myValue
    End Function

    Public Shared Function GetContentCell(ByVal CSSClass As String, ByVal Content As String) As TableCell
        Dim mycell As New TableCell() With {.CssClass = CSSClass, .Width = New System.Web.UI.WebControls.Unit(300)}
        mycell.Controls.Add(New Label With {.Text = Content})
        Return mycell
    End Function
    Public Shared Function GetControlCell(ByVal CSSClass As String, ByVal Content As Control) As TableCell
        Dim mycell As New TableCell() With {.CssClass = CSSClass, .Width = New System.Web.UI.WebControls.Unit(500)}
        mycell.Controls.Add(Content)
        Return mycell
    End Function

End Class
