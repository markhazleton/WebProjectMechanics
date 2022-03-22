Imports WebProjectMechanics
Imports System

Public Class wpm_admin_LinkAdmin
    Inherits ApplicationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim myXML As New UtilityXMLDocument(String.Format("{0}\sites\{1}\{1}-site-file.xml", wpm_SiteConfig.ConfigFolderPath, wpm_HostName), Server.MapPath("\admin\xsl\SiteInventoryReport.xsl"))
        wpm_ListPageURL = HttpContext.Current.Request.FilePath()
        myContent.Text = myXML.getXMLTransform()
    End Sub
End Class
