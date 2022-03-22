Imports System.IO

Public Class ApplicationLogging
    Private Const STR_LogFilePathTemplate As String = "{0}\log\{1}"
    '********************************************************************************

    Public Shared Function WriteLog(ByVal MessageOne As String, ByVal MessageTwo As String, ByVal LogFileName As String) As Boolean
        wpm_AddGenericError(String.Format("ApplicationLogging.WriteLog - {0} {1}", MessageOne, MessageTwo))

        Dim bReturn As Boolean = False
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")
        MessageOne = wpm_ApplyHTMLFormatting(wpm_GetStringValue(MessageOne))
        MessageTwo = wpm_ApplyHTMLFormatting(wpm_GetStringValue(MessageTwo))
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New StreamWriter(LogFileName, True)
                Try
                    sw.WriteLine(String.Format("{0}{2}{1}{3}{1}{4}{1}{5}{1}{6}{1}{7}{1}{8}{0}", _
                                     sQuote, _
                                     sQuoteComma, _
                                     HttpContext.Current.Session("ContactName"), _
                                     HttpContext.Current.Request.Url.Host, _
                                     HttpContext.Current.Request.Url.AbsoluteUri, _
                                     DateTime.Now.ToShortDateString, _
                                     DateTime.Now.ToLongTimeString, _
                                     MessageOne, _
                                     MessageTwo))
                Catch
                    ' Do Nothing
                End Try
            End Using
            bReturn = True
        Catch
            ' do nothing
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function
    Public Shared Function GetLogFilePath(ByVal LogFileName As String) As String
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")
        Dim ConfigFolderPath As String = HttpContext.Current.Server.MapPath("/App_Data")

        If Not FileProcessing.VerifyFolderExists(ConfigFolderPath) Then
            FileProcessing.CreateFolder(ConfigFolderPath)
        End If

        If Not FileProcessing.VerifyFolderExists(String.Format("{0}\log", ConfigFolderPath)) Then
            FileProcessing.CreateFolder(String.Format("{0}\log", ConfigFolderPath))
        End If

        If Not FileProcessing.FileExists(String.Format(STR_LogFilePathTemplate, ConfigFolderPath, LogFileName)) Then
            Try
                Using sw As New StreamWriter(String.Format(STR_LogFilePathTemplate, ConfigFolderPath, LogFileName), True)
                    Try
                        sw.WriteLine( _
                               String.Format("{0}UserName{1}HostName{1}RequestURL{1}Date{1}Time{1}MessageOne{1}MessageTwo{0}", sQuote, sQuoteComma))
                    Catch
                        ' Do Nothing
                    End Try
                End Using
            Catch
                ' do nothing
            End Try
        End If
        Return String.Format(STR_LogFilePathTemplate, ConfigFolderPath, LogFileName)
    End Function

    ' ****************************************************************************
    Public Shared Function XMLLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        Return WriteLog(accessProcess, accessMessage, GetLogFilePath("wpmXML.csv"))
    End Function
    ' ****************************************************************************
    'Public Shared Function FileNotFoundLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
    '    Return WriteLog(accessProcess, accessMessage, GetLogFilePath("wpm404.csv"))
    'End Function
    ' ****************************************************************************
    Public Shared Function SiteReferLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        Return WriteLog(accessProcess, accessMessage, GetLogFilePath("wpmRefer.csv"))
    End Function
    ' ****************************************************************************
    Public Shared Function ErrorLog(ByVal logProcess As String, ByVal logMessage As String) As Boolean
        Return WriteLog(logProcess, logMessage, GetLogFilePath("wpmErrorLog.csv"))
    End Function
    ' ****************************************************************************
    Public Shared Function ConfigLog(ByVal logProcess As String, ByVal logMessage As String) As Boolean
        Return WriteLog(logProcess, logMessage, GetLogFilePath("wpmConfigError.csv"))
    End Function
    ' ****************************************************************************
    Public Shared Function SearchLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        Return WriteLog(accessProcess, accessMessage, GetLogFilePath("wpmSearch.csv"))
    End Function
    ' ****************************************************************************
    Public Shared Function AccessLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        Return WriteLog(accessMessage, accessProcess, GetLogFilePath("pmAccess.csv"))
    End Function
    ' ****************************************************************************
    'Public Shared Function GameLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
    '    Return WriteLog(accessMessage, accessProcess, GetLogFilePath("wpmGame.csv"))
    'End Function
    ''********************************************************************************
    'Public Shared Function DownloadLog(ByVal frmName As String, ByVal frmEmail As String, ByVal DownloadFile As String) As Boolean
    '    Return WriteLog(String.Format("{0}"",""{1}", DownloadFile, frmName), frmEmail, GetLogFilePath("wpmDownloadLog.csv"))
    'End Function
    '********************************************************************************
    Public Shared Function AuditLog(ByVal AuditMessage As String, ByVal AuditProcess As String) As Boolean
        Return WriteLog(AuditProcess, AuditMessage, GetLogFilePath("wpmAuditLog.csv"))
    End Function
    '********************************************************************************
    Public Shared Function SQLAudit(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("pmSQL-Audit.csv"))
    End Function
    Public Shared Function SQLSelectError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("pmSQL-Select-Error.csv"))
    End Function
    Public Shared Function SQLInsertError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("pmSQL-Insert-Error.csv"))
    End Function
    Public Shared Function SQLUpdateError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("pmSQL-Update-Error.csv"))
    End Function
    Public Shared Function MembershipProviderError(ByVal Error1 As String, ByVal Error2 As String) As Boolean
        Return WriteLog(Error1, Error2, GetLogFilePath("wpmMembershipProvider.csv"))
    End Function
    Public Shared Function SQLDeleteError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("pmSQL-Delete-Error.csv"))
    End Function
    Public Shared Function SQLExceptionLog(ByVal strCallingFunction As String, ByRef ex As Exception) As Boolean
        If ex.InnerException Is Nothing Then
            Return WriteLog(strCallingFunction, ex.ToString, GetLogFilePath("wpmSQLError.csv"))
        Else
            Return WriteLog(strCallingFunction, ex.InnerException.ToString, GetLogFilePath("wpmSQLError.csv"))
        End If
    End Function
End Class
