Imports System.Web
Imports System.Text

Public Class wpmForm
    Public Shared Function FormatTableCell(ByVal cell_value As String) As String
        Return String.Format("<td>{0}</td>", cell_value)
    End Function
    Public Shared Function FormatVariableLine(ByVal var_name As String, ByVal var_value As String) As String
        Dim tmpStr As New String(CType(String.Empty, Char()))
        If var_name <> "Submit" Then
            tmpStr = String.Format("{0}<b>{1}</b>:<br/>{2}", tmpStr, var_name.ToUpper(), vbCrLf)
            tmpStr = tmpStr & var_value & vbCrLf
            tmpStr = String.Format("{0}<br/>{1}", tmpStr, vbCrLf)
        End If
        Return tmpStr
    End Function
    Public Shared Function GetSessionPageHistory() As wpmPageHistoryList
        Dim myPageHistory As wpmPageHistoryList
        Try
            myPageHistory = CType(HttpContext.Current.Session("PageHistory"), wpmPageHistoryList)
            If myPageHistory Is Nothing Then
                myPageHistory = New wpmPageHistoryList
            End If
        Catch ex As Exception
            wpmLogging.AuditLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "wpmForm.New")
            myPageHistory = New wpmPageHistoryList
        End Try
        Return myPageHistory
    End Function

    Public Shared Function GetPageHistory() As String
        Dim mysb As New StringBuilder("<hr/><h2>Session History</h2><br/><table border=""1"">")
        mysb.Append("<thead><tr><td>Timestamp</td><td>Page Name</td><td>Previous Page</td></tr></thead>")
        For Each ph As wpmPageHistory In GetSessionPageHistory()
            mysb.Append(String.Format("<tr><td>{0}</td><td><a href=""{1}"">{2}</a></td><td>{3}</td></tr>", ph.ViewTime, ph.RequestURL, ph.PageName, ph.PageSource))
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function
    Public Shared Function SaveMailToFile(ByRef pageActiveSite As wpmActiveSite, ByRef sOutFile As String) As Boolean
        Dim filename As String = GetFormFilePath(Replace(Replace(Replace(String.Format("{0}-{1}", pageActiveSite.CompanyName, Format(System.DateTime.Now(), "yyyy:MM:dd:HH:mm:ss")), " ", "-"), ",", ""), ":", "-") & ".html")
        Return wpmFileProcessing.CreateFile(filename, String.Format("{0}<br/><br/><hr/>Sent to:{1}<br/>", sOutFile, pageActiveSite.FromEmail))
    End Function
    Public Shared Function GetFormFilePath(ByVal HTMLFileName As String) As String
        If Not wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "form") Then
            wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "form")
        End If
        If Not wpmFileProcessing.VerifyFolderExists(String.Format("{0}form\{1}", wpmApp.Config.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))) Then
            wpmFileProcessing.CreateFolder(String.Format("{0}form\{1}", wpmApp.Config.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")))
        End If
        Return String.Format("{0}form\{1}\{2}", wpmApp.Config.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), HTMLFileName)
    End Function
End Class
