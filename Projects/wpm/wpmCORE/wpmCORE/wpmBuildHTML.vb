Imports System.Web

Public Class wpmBuildHTML
    Private Shared Function GetHTMLFilePath(ByVal HTMLFileName As String) As String
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "html") Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "html")
        End If
        If Not wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "html\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "")) Then
            wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "html\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
        End If
        Return App.Config.ConfigFolderPath & "html\" & Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", "") & "\" & HTMLFileName
    End Function
    Public Shared Function SaveHTML(ByVal sPageName As String, ByVal sContent As String) As Boolean
        Dim sPath As String = GetHTMLFilePath(Replace(sPageName, "/", ""))
        Dim bReturn As Boolean = False
        If Trim(sPageName) <> "" Then
            If App.Config.DefaultExtension = String.Empty Then
                sPath = sPath & ".html"
            Else
                sPath = sPath & App.Config.DefaultExtension
            End If
            bReturn = wpmFileIO.CreateFile(sPath, sContent)
        End If
        Return bReturn
    End Function
End Class
