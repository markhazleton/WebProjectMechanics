Public Class DisplayTableHeaderItem
    Public Property Name As String
    Public Property Value As String
    Public Property KeyField As Boolean = False
    Public Property LinkPath As String
    Public Property LinkKeyName As String
    Public Property LinkTextName As String
    Public Property ViewOnPhone As Boolean = True
    Public Property GridIndex As Integer
    Public Property ThumbnailPath As String
    Public Property DisplayFormat As DisplayFormat

    Public Function GetFormatTableCell(ByVal PropValue As String, ByVal LinkURL As String) As String
        Dim myReturn As String = String.Empty
        If IsNumeric(PropValue) Then
            Select Case DisplayFormat
                Case DisplayFormat.Currency
                    PropValue = Math.Round(CDec(PropValue), 2).ToString("c")
                    myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                Case DisplayFormat.Text
                    PropValue = Math.Round(CDec(PropValue), 2).ToString()
                    myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                Case DisplayFormat.Number
                    PropValue = Math.Round(CDec(PropValue), 2).ToString()
                    myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                Case DisplayFormat.Fraction
                    PropValue = Math.Round(CDbl(PropValue), 4).ToString()
                    myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                Case DisplayFormat.Thumbnail
                    myReturn = (String.Format("<td {2} style='text-align: right; ' ><img width='50px' src='{1}' alt='{0}' />{0}</td>", PropValue, LinkURL, GetTDCssClass()))
                Case DisplayFormat.YesNo
                    If Math.Round(CDec(PropValue), 2) = 0 Then
                        PropValue = "No"
                    Else
                        PropValue = "Yes"
                    End If
                    myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case Else
                    PropValue = Math.Round(CDec(PropValue), 2).ToString()
                    myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
            End Select
        Else
            myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
        End If
        Return myReturn
    End Function
    Public Function GetFormatTableCell(ByVal PropValue As String) As String
        Dim myReturn As String = String.Empty
        If IsNumeric(PropValue) Then
            Select Case DisplayFormat
                Case DisplayFormat.Currency
                    PropValue = Math.Round(CDec(PropValue), 2).ToString("c")
                    myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case DisplayFormat.Text
                    PropValue = Math.Round(CDec(PropValue), 2).ToString()
                    myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case DisplayFormat.Number
                    PropValue = Math.Round(CDec(PropValue), 2).ToString("N0")
                    myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case DisplayFormat.Fraction
                    PropValue = Math.Round(CDbl(PropValue), 4).ToString("P")
                    myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case DisplayFormat.YesNo
                    If Math.Round(CDec(PropValue), 2) = 0 Then
                        PropValue = "No"
                    Else
                        PropValue = "Yes"
                    End If
                    myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                Case Else
                    PropValue = Math.Round(CDec(PropValue), 2).ToString()
                    myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
            End Select
        Else
            myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
        End If
        Return myReturn
    End Function
    Private Function GetTDCssClass() As String
        If ViewOnPhone Then
            Return String.Empty
        Else
            Return " class='hidden-xs' "
        End If
    End Function


End Class

Public Enum DisplayFormat
    Number
    Currency
    Text
    Fraction
    Thumbnail
    YesNo
End Enum