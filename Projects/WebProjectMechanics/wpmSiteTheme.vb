Imports System.Web
Imports System.Text

Public Class wpmSiteTheme
    Public sbSiteTemplate As New StringBuilder

    Public Sub New(ByRef passedSiteMap As wpmActiveSite, ByVal UseDefault As Boolean)
        MyBase.New()
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.CompanyName, passedSiteMap.DefaultSitePrefix)
        Else
            If passedSiteMap.SiteTemplatePrefix = "" Then
                sbSiteTemplate = GetTemplateFile(passedSiteMap.CompanyName, passedSiteMap.SitePrefix)
            Else
                sbSiteTemplate = GetTemplateFile(passedSiteMap.CompanyName, passedSiteMap.SiteTemplatePrefix)
            End If
        End If
    End Sub
    Public Sub New(ByRef passedSiteMap As wpmActiveSite, ByVal UseDefault As Boolean, ByVal TemplatePrefix As String)
        MyBase.New()
        If TemplatePrefix = "" Then
            TemplatePrefix = passedSiteMap.SiteTemplatePrefix
        End If
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.CompanyName, passedSiteMap.DefaultSitePrefix)
        Else
            sbSiteTemplate = GetTemplateFile(passedSiteMap.CompanyName, TemplatePrefix)
        End If

    End Sub

    Public Shared Function GetTemplateFile(ByVal CompanyName As String, ByVal SiteTemplatePrefix As String) As StringBuilder
        Dim sPath As String
        Dim myStringBuilder As New StringBuilder
        Dim bTemplateFromDB As Boolean = True
        If SiteTemplatePrefix = "wpmADMIN" Or SiteTemplatePrefix Is Nothing Then
            sPath = HttpContext.Current.Server.MapPath("/wpm/admin/template/wpmADMIN-template.html")
            wpmFileProcessing.ReadFile(sPath, myStringBuilder)
        ElseIf SiteTemplatePrefix = "wpmRECIPE" Then
            sPath = HttpContext.Current.Server.MapPath("/recipe/template/wpmRECIPE-template.html")
            wpmFileProcessing.ReadFile(sPath, myStringBuilder)
        Else

            sPath = String.Format("{0}gen\{1}", wpmApp.Config.ConfigFolderPath, wpmUtil.FormatNameForURL(String.Format("{0}-Template-{1}.html", CompanyName, SiteTemplatePrefix)))
            If (wpmUser.IsAdmin) Or (wpmUser.IsEditor) Then
                bTemplateFromDB = True
            Else
                If wpmFileProcessing.FileExists(sPath) Then
                    bTemplateFromDB = False
                End If
            End If
            'If Not (wpmUser.IsAdmin Or wpmUser.IsEditor) Or (wpmFileProcessing.FileExists(sPath)) Then
            '    If wpmFileProcessing.ReadFile(sPath, myStringBuilder) Then
            '    End If
            'End If
            If bTemplateFromDB Then
                Dim strSQL As String = (String.Format("SELECT SiteTemplate.Top, SiteTemplate.Bottom FROM SiteTemplate  where SiteTemplate.TemplatePrefix='{0}'; ", SiteTemplatePrefix))
                For Each row As DataRow In wpmDB.GetDataTable(strSQL, "GetTemplateFile-" & SiteTemplatePrefix).Rows
                    myStringBuilder.Append(row.Item("Top"))
                    myStringBuilder.Append("~~MainContent~~")
                    myStringBuilder.Append(row.Item("Bottom"))
                    myStringBuilder.Append("</body></html>")
                Next
                SaveTemplateFile(myStringBuilder.ToString, sPath)
            Else
                wpmFileProcessing.ReadFile(sPath, myStringBuilder)
            End If
        End If
        Return myStringBuilder
    End Function
    Public Shared Function SaveTemplateFile(ByVal sTemplate As String, ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            If Not (wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "gen\")) Then
                wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "gen\")
            End If
            wpmFileProcessing.CreateFile(sPath, sTemplate)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
End Class
