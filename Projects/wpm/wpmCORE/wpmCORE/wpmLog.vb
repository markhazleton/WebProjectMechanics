Imports System.Web

Public Class wpmLog
    '********************************************************************************
    Private Shared Function WriteLog(ByVal MessageOne As String, ByVal MessageTwo As String, ByVal LogFileName As String) As Boolean
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")
        MessageOne = wpmUTIL.ApplyHTMLFormatting(wpmUTIL.GetStringValue(MessageOne))
        MessageTwo = wpmUTIL.ApplyHTMLFormatting(wpmUTIL.GetStringValue(MessageTwo))
        System.Web.HttpContext.Current.Application.Lock()
        Try

            Using sw As New StreamWriter(LogFileName, True)
                Try
                    sw.WriteLine( _
                           sQuote & _
                           HttpContext.Current.Request.Url.Host.ToString & sQuoteComma & _
                           DateTime.Now.ToShortDateString & sQuoteComma & _
                           DateTime.Now.ToLongTimeString & sQuoteComma & _
                           MessageOne & sQuoteComma & _
                           MessageTwo & sQuote)
                Catch
                    ' Do Nothing
                Finally
                    sw.Flush()
                    sw.Close()
                End Try
            End Using
            WriteLog = True
        Catch
            WriteLog = False
        End Try
        System.Web.HttpContext.Current.Application.UnLock()

    End Function
    Private Shared Function GetLogFilePath(ByVal LogFileName As String) As String
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "log") Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "log")
        End If

        If Not wpmFileIO.FileExists(App.Config.ConfigFolderPath & "log\" & LogFileName) Then
            Try
                Using sw As New StreamWriter(App.Config.ConfigFolderPath & "log\" & LogFileName, True)
                    Try
                        sw.WriteLine( _
                               sQuote & _
                               "HostName" & sQuoteComma & _
                               "Date" & sQuoteComma & _
                               "Time" & sQuoteComma & _
                               "MessageOne" & sQuoteComma & _
                               "MessageTwo" & sQuote)
                    Catch
                        ' Do Nothing
                    Finally
                        sw.Flush()
                        sw.Close()
                    End Try
                End Using
            Catch
                ' do nothing
            End Try
        End If
        Return App.Config.ConfigFolderPath & "log\" & LogFileName
    End Function

    ' ****************************************************************************
    Public Shared Function XMLLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("XMLLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function FileNotFoundLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("FileNotFoundLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function SiteReferLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("SiteReferLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function ErrorLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("ErrorLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function SearchLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("SearchLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function AccessLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If App.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("AccessLog.csv"))
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function DownloadLog(ByVal frmName As String, ByVal frmEmail As String, ByVal DownloadFile As String) As Boolean
        If App.Config.FullLoggingOn Then
            Return WriteLog(DownloadFile & """,""" & frmName, frmEmail, GetLogFilePath("DownloadLog.csv"))
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function AuditLog(ByVal AuditMessage As String, ByVal AuditProcess As String) As Boolean
        Return WriteLog(AuditProcess, AuditMessage, GetLogFilePath("AuditLog.csv"))
    End Function
    '********************************************************************************
    Public Shared Function SQLAudit(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("SQLLog.csv"))
    End Function

End Class
