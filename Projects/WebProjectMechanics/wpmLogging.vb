Imports System.IO
Imports System.Data.SqlClient

Public Class wpmLogging
    '********************************************************************************
    Public Shared Function WriteLog(ByVal MessageOne As String, ByVal MessageTwo As String, ByVal LogFileName As String) As Boolean
        Dim bReturn As Boolean = False
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")
        MessageOne = wpmUtil.ApplyHTMLFormatting(wpmUtil.GetStringValue(MessageOne))
        MessageTwo = wpmUtil.ApplyHTMLFormatting(wpmUtil.GetStringValue(MessageTwo))
        System.Web.HttpContext.Current.Application.Lock()
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
        System.Web.HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function
    Public Shared Function GetLogFilePath(ByVal LogFileName As String) As String
        Dim sQuoteComma As String = (""",""")
        Dim sQuote As String = ("""")

        If Not wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "log") Then
            wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "log")
        End If

        If Not wpmFileProcessing.FileExists(String.Format("{0}log\{1}", wpmApp.Config.ConfigFolderPath, LogFileName)) Then
            Try
                Using sw As New StreamWriter(String.Format("{0}log\{1}", wpmApp.Config.ConfigFolderPath, LogFileName), True)
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
        Return String.Format("{0}log\{1}", wpmApp.Config.ConfigFolderPath, LogFileName)
    End Function

    ' ****************************************************************************
    Public Shared Function XMLLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("XMLLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function FileNotFoundLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("FileNotFoundLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function SiteReferLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("SiteReferLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function ErrorLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("ErrorLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function SearchLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessProcess, accessMessage, GetLogFilePath("SearchLog.csv"))
        Else
            Return False
        End If
    End Function
    ' ****************************************************************************
    Public Shared Function AccessLog(ByVal accessMessage As String, ByVal accessProcess As String) As Boolean
        If wpmApp.Config.FullLoggingOn() Then
            Return WriteLog(accessMessage, accessProcess, GetLogFilePath("AccessLog.csv"))
        Else
            Return False
        End If
    End Function
    '********************************************************************************
    Public Shared Function DownloadLog(ByVal frmName As String, ByVal frmEmail As String, ByVal DownloadFile As String) As Boolean
        If wpmApp.Config.FullLoggingOn Then
            Return WriteLog(String.Format("{0}"",""{1}", DownloadFile, frmName), frmEmail, GetLogFilePath("DownloadLog.csv"))
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
        Return WriteLog(strSQL, pageID, GetLogFilePath("SQL-Audit.csv"))
    End Function
    Public Shared Function SQLSelectError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("SQL-Select-Error.csv"))
    End Function
    Public Shared Function SQLInsertError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("SQL-Insert-Error.csv"))
    End Function
    Public Shared Function MembershipProviderError(ByVal Error1 As String, ByVal Error2 As String) As Boolean
        Return WriteLog(Error1, Error2, GetLogFilePath("MembershipProvider.csv"))
    End Function
    Public Shared Function SQLDeleteError(ByVal strSQL As String, ByVal pageID As String) As Boolean
        Return WriteLog(strSQL, pageID, GetLogFilePath("SQL-Delete-Error.csv"))
    End Function
    Public Shared Function SQLExceptionLog(ByVal strCallingFunction As String, ByRef ex As SqlException) As Boolean
        If ex.InnerException Is Nothing Then
            Return WriteLog(strCallingFunction, ex.ToString, GetLogFilePath("SQLError.csv"))
        Else
            Return WriteLog(strCallingFunction, ex.InnerException.ToString, GetLogFilePath("SQLError.csv"))
        End If
    End Function
End Class
