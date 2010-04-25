Imports System.Web

Public Class wpmSiteTheme
    Public sbSiteTemplate As New StringBuilder

    Public Sub New(ByRef passedSiteMap As wpmActiveSite, ByVal UseDefault As Boolean)
        MyBase.New()
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.SiteGallery, passedSiteMap.CompanyName, passedSiteMap.DefaultSitePrefix)
        Else
            If passedSiteMap.SiteTemplatePrefix = "" Then
                sbSiteTemplate = GetTemplateFile(passedSiteMap.SiteGallery, passedSiteMap.CompanyName, passedSiteMap.SitePrefix)
            Else
                sbSiteTemplate = GetTemplateFile(passedSiteMap.SiteGallery, passedSiteMap.CompanyName, passedSiteMap.SiteTemplatePrefix)
            End If
        End If
    End Sub
    Public Sub New(ByRef passedSiteMap As wpmActiveSite, ByVal UseDefault As Boolean, ByVal TemplatePrefix As String)
        MyBase.New()
        If TemplatePrefix = "" Then
            TemplatePrefix = passedSiteMap.SiteTemplatePrefix
        End If
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.SiteGallery, passedSiteMap.CompanyName, passedSiteMap.DefaultSitePrefix)
        Else
            sbSiteTemplate = GetTemplateFile(passedSiteMap.SiteGallery, passedSiteMap.CompanyName, TemplatePrefix)
        End If

    End Sub

    Public Shared Function GetTemplateFile(ByVal SiteGallery As String, ByVal CompanyName As String, ByVal SiteTemplatePrefix As String) As StringBuilder
        Dim sPath As String
        Dim myStringBuilder As New StringBuilder
        Dim bTemplateFromDB As Boolean = True
        If SiteTemplatePrefix = "wpmADMIN" Or SiteTemplatePrefix Is Nothing Then
            sPath = HttpContext.Current.Server.MapPath("/wpm/admin/template/wpmADMIN-template.html")
            wpmFileIO.ReadFile(sPath, myStringBuilder)
        ElseIf SiteTemplatePrefix = "wpmRECIPE" Then
            sPath = HttpContext.Current.Server.MapPath("/recipe/template/wpmRECIPE-template.html")
            wpmFileIO.ReadFile(sPath, myStringBuilder)
        Else
            sPath = App.Config.ConfigFolderPath & "gen\" & wpmUTIL.FormatNameForURL(CompanyName & "-Template-" & SiteTemplatePrefix & ".html")
            If Not (wpmUser.IsAdmin Or wpmUser.IsEditor) Or (wpmFileIO.FileExists(sPath)) Then
                If wpmFileIO.ReadFile(sPath, myStringBuilder) Then
                    bTemplateFromDB = False
                End If
            End If
            If bTemplateFromDB Then
                Dim strSQL As String = ("SELECT SiteTemplate.Top, SiteTemplate.Bottom FROM SiteTemplate " & _
                        " where SiteTemplate.TemplatePrefix='" & SiteTemplatePrefix & "'; ")
                For Each row As DataRow In wpmDB.GetDataTable(strSQL, "GetTemplateFile-" & SiteTemplatePrefix).Rows
                    myStringBuilder.Append(row.Item("Top"))
                    myStringBuilder.Append("~~MainContent~~")
                    myStringBuilder.Append(row.Item("Bottom"))
                    myStringBuilder.Append("</body></html>")
                Next
                SaveTemplateFile(myStringBuilder.ToString, sPath)
            End If
        End If
        Return myStringBuilder
    End Function
    Public Shared Function SaveTemplateFile(ByVal sTemplate As String, ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            If Not (wpmFileIO.VerifyFolderExists(App.Config.ConfigFolderPath & "gen\")) Then
                wpmFileIO.CreateFolder(App.Config.ConfigFolderPath & "gen\")
            End If
            wpmFileIO.CreateFile(sPath, sTemplate)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
End Class
