Imports System.Web

Public Class wpmBuildHTML
    Private Shared Function GetHTMLFilePath(ByVal HTMLFileName As String) As String
        If Not wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "html") Then
            wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "html")
        End If
        If Not wpmFileProcessing.VerifyFolderExists(String.Format("{0}html\{1}", wpmApp.Config.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))) Then
            wpmFileProcessing.CreateFolder(String.Format("{0}html\{1}", wpmApp.Config.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")))
        End If
        Return String.Format("{0}html\{1}\{2}", _
                   wpmApp.Config.ConfigFolderPath, _
                   Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""), _
                   HTMLFileName)
    End Function
    Public Shared Function SaveHTML(ByVal sPageName As String, ByVal sContent As String) As Boolean
        Dim sPath As String = GetHTMLFilePath(Replace(sPageName, "/", ""))
        Dim bReturn As Boolean = False
        If Trim(sPageName) <> "" Then
            If wpmApp.Config.DefaultExtension = String.Empty Then
                sPath = sPath & ".html"
            Else
                sPath = sPath & wpmApp.Config.DefaultExtension
            End If
            bReturn = wpmFileProcessing.CreateFile(sPath, sContent)
        End If
        Return bReturn
    End Function
End Class
