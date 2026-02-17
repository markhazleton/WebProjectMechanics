Imports WebProjectMechanics
Imports System

Partial Class wpm_admin_SiteMapReport
    Inherits ApplicationPage
    Private Const sIndexFileNameTemplate As String = "{0}\sites\{1}\{1}-site-file.xml"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim myWebHomePath As String = HttpContext.Current.Server.MapPath(wpm_SiteConfig.AdminFolder)
        Dim myXML As UtilityXMLDocument
        wpm_ListPageURL = Request.RawUrl
        Select Case wpm_GetProperty("ReportID", "NavigationAdmin")
            Case "NavigationAdmin"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/NavigationAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "LinkAdmin"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/LinkAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "ParameterAdmin"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/ParameterAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "ImageAdmin"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/ImageAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "PageAlias"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/PageAlias.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "SiteInventory"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/SiteInventoryReport.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "SiteProfile"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/SiteProfileReport.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case Else
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/NavigationAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
        End Select
    End Sub
End Class
