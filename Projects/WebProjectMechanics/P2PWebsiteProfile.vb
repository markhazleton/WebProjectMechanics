Imports System.Text
Imports System.Web
Imports System.Xml.Serialization

<XmlRootAttribute("WebsiteProfile", _
 Namespace:="http://www.p2padvisor.com", IsNullable:=False)> _
Public Class P2PWebsiteProfile
#Region "Class Members"
    Public Property CompanyName() As String
    Public Property SiteURL() As String
    Public Property SiteTitle() As String
    Public Property SiteKeywords() As String
    Public Property SiteDescription() As String
    Public Property SiteHomePageID() As String
    Public Property DefaultArticleID() As String
    Public Property SitePrefix() As String
    Public Property DefaultSitePrefix() As String
    Public Property SiteGallery() As String
    Public Property SiteConfig() As String
    Public Property SiteState() As String
    Public Property SiteCountry() As String
    Public Property SiteCity() As String
    Public Property SiteCategoryTypeID() As String
    Public Property DefaultSiteCategoryID() As String
    Public Property UseBreadCrumbURL() As Boolean
    Public Property FromEmail() As String
    Public Property SMTP() As String
    Public Property Component() As String
#End Region
#Region "Public Methods"
    Public Sub GetSiteFileFromDB(ByVal CompanyID As String, ByVal OrderBy As String)
        If Trim(OrderBy) = "" Then
            OrderBy = "ORDER"
        End If
        GetCompanyValues(CompanyID)
        If wpmApp.Config.FullLoggingOn() Or wpmUser.IsAdmin Then
            SaveSiteMapFile()
        End If

    End Sub

    Public Function ReplaceTags(ByVal sValue As String) As String
        If sValue <> String.Empty And sValue.Contains("~~") Then
            Dim mySB As New StringBuilder(sValue)
            ReplaceTags(mySB)
            sValue = mySB.ToString
        End If
        Return sValue
    End Function
    Public Function ReplaceTags(ByRef sbContent As StringBuilder) As Boolean
        sbContent.Replace("~~SiteURL~~", SiteURL)
        sbContent.Replace("~~SiteCompanyName~~", CompanyName)
        sbContent.Replace("~~SiteDescription~~", SiteDescription)
        sbContent.Replace("~~SiteKeywords~~", SiteKeywords)
        sbContent.Replace("~~SiteCity~~", SiteCity)
        sbContent.Replace("~~SiteCityDash~~", wpmUtil.FormatPageNameForURL(SiteCity))
        sbContent.Replace("~~SiteCityNoSpace~~", Replace(SiteCity, " ", ""))
        sbContent.Replace("~~SiteState~~", SiteState)
        sbContent.Replace("~~SiteCountry~~", SiteCountry)
        Return True
    End Function

#End Region
#Region "Private Methods"

    Private Shared Function GetCompanyValues(ByVal ReqCompanyID As String) As Boolean
        If CInt(ReqCompanyID) > 0 Then
            'For Each myrow As DataRow In DataCon.GetCompanyData(ReqCompanyID).Rows
            '    Me.CompanyName = Utility.GetDBString(myrow("CompanyName"))
            '    Me.SiteGallery = Utility.GetDBString(myrow("GalleryFolder"))
            '    Me.SiteURL = Utility.GetDBString(myrow("SiteURL"))
            '    Me.SiteTitle = Utility.GetDBString(myrow("SiteTitle"))
            '    Me.SiteKeywords = Utility.GetDBString(myrow("DefaultPaymentTerms"))
            '    Me.SiteDescription = Utility.GetDBString(myrow("DefaultInvoiceDescription"))
            '    Me.SitePrefix = Utility.GetDBString(myrow("SiteTemplate"))
            '    Me.DefaultArticleID = Utility.GetDBString(myrow("DefaultArticleID"))
            '    Me.SiteHomePageID = Utility.GetDBString(myrow("HomePageID"))
            '    Me.DefaultSitePrefix = Utility.GetDBString(myrow("DefaultSiteTemplate"))
            '    Me.UseBreadCrumbURL = Utility.GetDBBoolean(myrow("UseBreadCrumbURL"))
            '    Me.SiteCity = Utility.GetDBString(myrow("City"))
            '    Me.SiteState = Utility.GetDBString(myrow("StateOrProvince"))
            '    Me.SiteCountry = Utility.GetDBString(myrow("Country"))
            '    Me.FromEmail = Utility.GetDBString(myrow("FromEmail"))
            '    Me.SMTP = Utility.GetDBString(myrow("SMTP"))
            '    Me.Component = Utility.GetDBString(myrow("Component"))
            '    Me.SiteCategoryTypeID = Utility.GetDBString(myrow("SiteCategoryTypeID"))
            '    Me.DefaultSiteCategoryID = Utility.GetDBString(myrow("DefaultSiteCategoryID"))
            '    If Me.SiteHomePageID = String.Empty And Me.DefaultSiteCategoryID <> String.Empty Then
            '        Me.SiteHomePageID = "CAT-" & Me.DefaultSiteCategoryID
            '    End If

            '    Exit For
            'Next
            Return True
        Else
            Return False
        End If
    End Function
    Private Function GetIndexFilePath() As String
        If Not wpmFileProcessing.VerifyFolderExists(wpmApp.Config.ConfigFolderPath & "index") Then
            wpmFileProcessing.CreateFolder(wpmApp.Config.ConfigFolderPath & "index")
        End If
        Return Replace(String.Format("{0}\index\{1}-site-file.xml", wpmApp.Config.ConfigFolderPath, CompanyName), "\\", "\")
    End Function

    Private Function SaveSiteMapFile() As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New System.IO.StreamWriter(GetIndexFilePath(), False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(WebsiteProfile))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    wpmLogging.AuditLog("Error Saving File - " & ex.ToString, "SiteProfile.SaveSiteMapFile")
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            wpmLogging.AuditLog("Error Before Saving File  - " & ex.ToString, "SiteProfile.SaveSiteMapFile")
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function
#End Region

End Class
