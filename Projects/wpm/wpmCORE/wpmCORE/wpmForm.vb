Imports System.Web

Public Class wpmForm
    Public Function FormatTableCell(ByVal cell_value As String) As String
        Return "<td>" & cell_value & "</td>"
    End Function
    Public Function FormatVariableLine(ByVal var_name As String, ByVal var_value As String) As String
        Dim tmpStr As New String(CType(String.Empty, Char()))
        If var_name <> "Submit" Then
            tmpStr = tmpStr & "<b>" & var_name.ToUpper() & "</b>:<br/>" & vbCrLf
            tmpStr = tmpStr & var_value & vbCrLf
            tmpStr = tmpStr & "<br/>" & vbCrLf
        End If
        Return tmpStr
    End Function
    Public Function GetSessionPageHistory() As wpmPageHistoryList
        Dim myPageHistory As wpmPageHistoryList
        Try
            myPageHistory = CType(HttpContext.Current.Session("PageHistory"), wpmPageHistoryList)
            If myPageHistory Is Nothing Then
                myPageHistory = New wpmPageHistoryList
            End If
        Catch ex As Exception
            wpmLog.AuditLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "wpmForm.New")
            myPageHistory = New wpmPageHistoryList
        End Try
        Return myPageHistory
    End Function

    Public Function GetPageHistory() As String
        Dim mysb As New StringBuilder("<hr/><h2>Session History</h2><br/><table border=""1"">")
        mysb.Append("<thead><tr><td>Timestamp</td><td>Page Name</td><td>Previous Page</td></tr></thead>")
        For Each ph As wpmPageHistory In GetSessionPageHistory()
            mysb.Append("<tr><td>" & ph.ViewTime.ToString() & "</td><td><a href=""" & ph.RequestURL & """>" & ph.PageName & "</a></td><td>" & ph.PageSource & "</td></tr>")
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function
    Public Function SaveMailToFile(ByRef pageActiveSite As wpmActiveSite, ByRef sOutFile As String) As Boolean
        Dim filename As String = GetFormFilePath(Replace(Replace(Replace(pageActiveSite.CompanyName & "-" & Format(System.DateTime.Now(), "yyyy:MM:dd:HH:mm:ss"), " ", "-"), ",", ""), ":", "-") & ".html")
        wpmFileIO.CreateFile(filename, sOutFile & "<br/><br/><hr/>Sent to:" & pageActiveSite.FromEmail & "<br/>")

    End Function
    Public Function GetFormFilePath(ByVal HTMLFileName As String) As String
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "form") Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "form")
        End If
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "form\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")) Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "form\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
        End If
        Return App.Config.ConfigFolderPath & "form\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "\" & HTMLFileName
    End Function
End Class
