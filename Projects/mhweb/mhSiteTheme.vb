Public Class mhSiteTheme
    Public sbSiteTemplate As New StringBuilder

    Public Sub New(ByRef passedSiteMap As mhSiteMap, ByVal UseDefault As Boolean)
        MyBase.New()
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.mySiteFile.SiteGallery, passedSiteMap.mySiteFile.CompanyName, passedSiteMap.mySiteFile.DefaultSitePrefix)
        Else
            If passedSiteMap.mySession.SiteTemplatePrefix = "" Then
                sbSiteTemplate = GetTemplateFile(passedSiteMap.mySiteFile.SiteGallery, passedSiteMap.mySiteFile.CompanyName, passedSiteMap.mySiteFile.SitePrefix)
            Else
                sbSiteTemplate = GetTemplateFile(passedSiteMap.mySiteFile.SiteGallery, passedSiteMap.mySiteFile.CompanyName, passedSiteMap.mySession.SiteTemplatePrefix)
            End If
        End If
    End Sub
    Public Sub New(ByRef passedSiteMap As mhSiteMap, ByVal UseDefault As Boolean, ByVal TemplatePrefix As String)
        MyBase.New()
        If TemplatePrefix = "" Then
            TemplatePrefix = passedSiteMap.mySession.SiteTemplatePrefix
        End If
        If UseDefault Then
            sbSiteTemplate = GetTemplateFile(passedSiteMap.mySiteFile.SiteGallery, passedSiteMap.mySiteFile.CompanyName, passedSiteMap.mySiteFile.DefaultSitePrefix)
        Else
            sbSiteTemplate = GetTemplateFile(passedSiteMap.mySiteFile.SiteGallery, passedSiteMap.mySiteFile.CompanyName, TemplatePrefix)
        End If

    End Sub

    Public Shared Function GetTemplateFile(ByVal SiteGallery As String, ByVal CompanyName As String, ByVal SiteTemplatePrefix As String) As StringBuilder
        Dim sPath As String
        Dim myStringBuilder As New StringBuilder
        Dim bTemplateFromDB As Boolean = True
        If SiteTemplatePrefix = "MHADMIN" Then
            sPath = HttpContext.Current.Server.MapPath("/util/skins/MHADMIN/mhadmin-template.html")
            mhfio.ReadFile(sPath, myStringBuilder)
        Else
            sPath = mhConfig.mhWebConfigFolder & "gen\" & mhUTIL.FormatNameForURL(CompanyName & "-Template-" & SiteTemplatePrefix & ".html")
            If mhUser.IsAdmin Or mhUser.IsEditor Then
                bTemplateFromDB = True
            ElseIf (mhfio.ReadFile(sPath, myStringBuilder)) Then
                bTemplateFromDB = False
            End If
            If bTemplateFromDB Then
                Dim strSQL As String = ("SELECT SiteTemplate.Top, SiteTemplate.Bottom FROM SiteTemplate " & _
                        " where SiteTemplate.TemplatePrefix='" & SiteTemplatePrefix & "'; ")
                For Each row As DataRow In mhDB.GetDataTable(strSQL, "GetTemplateFile-" & SiteTemplatePrefix).Rows
                    myStringBuilder.Append(row.Item("Top"))
                    myStringBuilder.Append("~~MainContent~~")
                    myStringBuilder.Append(row.Item("Bottom"))
                    myStringBuilder.Append("</body></html>")
                Next
                mhfio.SaveTemplateFile(myStringBuilder.ToString, sPath)
            End If
        End If
        Return myStringBuilder
    End Function
End Class
