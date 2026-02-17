Imports System.IO

Public Module ExceptionUtility
    Private Const STR_App_DatalogErrorLogtxt As String = "/App_Data/log/ErrorLog.txt"
    ' Log an Exception
    Public Sub LogException(ByVal exc As Exception, ByVal source As String)
        ApplicationLogging.ErrorLog(exc.ToString, source)

        ' Open the log file for append and write the log
        Dim sw = New StreamWriter(HttpContext.Current.Server.MapPath(STR_App_DatalogErrorLogtxt), True)
        sw.WriteLine(String.Format("**** {0} ****", DateTime.Now))
        sw.WriteLine(String.Format("**** {0} ****", HttpContext.Current.Request.Url.AbsoluteUri))
        If exc IsNot Nothing AndAlso exc.InnerException IsNot Nothing Then
            sw.Write("Inner Exception Type: ")
            sw.WriteLine(exc.InnerException.GetType.ToString)
            sw.Write("Inner Exception: ")
            sw.WriteLine(exc.InnerException.Message)
            sw.Write("Inner Source: ")
            sw.WriteLine(exc.InnerException.Source)
            If exc.InnerException.StackTrace IsNot Nothing Then
                sw.WriteLine("Inner Stack Trace: ")
                sw.WriteLine(exc.InnerException.StackTrace)
            End If
        End If
        If exc IsNot Nothing Then
            sw.Write("Exception Type: ")
            sw.WriteLine(exc.GetType.ToString)
            sw.WriteLine("Exception: " & exc.Message)
            sw.WriteLine("Source: " & source)
            If exc.StackTrace IsNot Nothing Then
                sw.WriteLine("Stack Trace: ")
                sw.WriteLine(exc.StackTrace)
            End If
        End If
        sw.WriteLine()
        sw.Close()
    End Sub
End Module
