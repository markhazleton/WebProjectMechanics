Imports System.Text


Public Class SiteTemplate
    Implements ISiteTemplate
    Sub New()
        TemplateBottom = String.Empty
        TemplateName = String.Empty
        TemplatePrefix = String.Empty
        TemplateTop = String.Empty
    End Sub

    Public Property TemplateBottom As String Implements ISiteTemplate.TemplateBottom
    Public Property TemplateName As String Implements ISiteTemplate.TemplateName
    Public Property TemplatePrefix As String Implements ISiteTemplate.TemplatePrefix
    Public Property TemplateTop As String Implements ISiteTemplate.TemplateTop

    Public sbSiteTemplate As New StringBuilder
    Public Sub New(ByRef passedSiteMap As ActiveCompany, ByVal UseDefault As Boolean)
        MyBase.New()
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(wpm_HostName, wpm_DefaultSitePrefix)
        Else
            If wpm_SiteTemplatePrefix = String.Empty Then
                sbSiteTemplate = GetTemplateFile(wpm_HostName, passedSiteMap.SitePrefix)
            Else
                sbSiteTemplate = GetTemplateFile(wpm_HostName, wpm_SiteTemplatePrefix)
            End If
        End If
    End Sub
    Public Sub New(ByVal UseDefault As Boolean, ByVal TemplatePrefix As String)
        MyBase.New()
        If TemplatePrefix = String.Empty Then
            TemplatePrefix = wpm_SiteTemplatePrefix
        End If
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(wpm_HostName, wpm_DefaultSitePrefix)
        Else
            sbSiteTemplate = GetTemplateFile(wpm_HostName, TemplatePrefix)
        End If

    End Sub
    Public Shared Function GetTemplateFile(ByVal Domain As String, ByVal SiteTemplatePrefix As String) As StringBuilder
        Dim sPath As String
        Dim myStringBuilder As New StringBuilder
        Dim bTemplateFromDB As Boolean = True
        If SiteTemplatePrefix = "wpmADMIN" Or SiteTemplatePrefix Is Nothing Then
            sPath = HttpContext.Current.Server.MapPath(String.Format("{0}admin/template/wpmADMIN-template.html", wpm_SiteConfig.AdminFolder))
            FileProcessing.ReadFile(sPath, myStringBuilder)
        ElseIf SiteTemplatePrefix = "wpmRECIPE" Then
            sPath = HttpContext.Current.Server.MapPath("/recipe/template/wpmRECIPE-template.html")
            FileProcessing.ReadFile(sPath, myStringBuilder)
        Else
            sPath = String.Format("{0}sites\{2}\gen\{1}", wpm_SiteConfig.ConfigFolderPath, wpm_FormatNameForURL(String.Format("{0}-Template-{1}.html", Domain, SiteTemplatePrefix)), Domain)
            If (wpm_IsAdmin) Or (wpm_IsEditor()) Then
                bTemplateFromDB = True
            Else
                If FileProcessing.FileExists(sPath) Then
                    bTemplateFromDB = False
                End If
            End If
            If bTemplateFromDB Then
                Dim strSQL As String = (String.Format("SELECT SiteTemplate.Top, SiteTemplate.Bottom FROM SiteTemplate  where SiteTemplate.TemplatePrefix='{0}'; ", SiteTemplatePrefix))
                For Each row As DataRow In wpm_GetDataTable(strSQL, "GetTemplateFile-" & SiteTemplatePrefix).Rows
                    myStringBuilder.Append(row.Item("Top"))
                    myStringBuilder.Append("~~MainContent~~")
                    myStringBuilder.Append(row.Item("Bottom"))
                    myStringBuilder.Append("</body></html>")
                Next
                SaveTemplateFile(myStringBuilder.ToString, sPath, Domain)
            Else
                FileProcessing.ReadFile(sPath, myStringBuilder)
            End If
        End If
        Return myStringBuilder
    End Function
    Public Shared Function SaveTemplateFile(ByVal sTemplate As String, ByVal sPath As String, ByVal Domain As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            If Not (FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "sites\")) Then
                FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "sites\")
            End If

            If Not (FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, Domain))) Then
                FileProcessing.CreateFolder(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, Domain))
            End If

            If Not (FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}\gen", wpm_SiteConfig.ConfigFolderPath, Domain))) Then
                FileProcessing.CreateFolder(String.Format("{0}sites\{1}\gen", wpm_SiteConfig.ConfigFolderPath, Domain))
            End If


            FileProcessing.CreateFile(sPath, sTemplate)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function

End Class
