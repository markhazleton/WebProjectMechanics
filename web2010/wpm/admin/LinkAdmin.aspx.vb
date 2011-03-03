Imports system.xml
Imports System.Xml.Xsl
Imports WebProjectMechanics

Partial Class wpm_admin_SiteInventoryReport
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myXML As New wpmXML(wpmApp.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", wpmApp.Config.ConfigFolderPath & "\style\LinkAdmin.xsl")
        Session("ListPageURL") = HttpContext.Current.Request.FilePath()
        myContent.Text = myXML.getXMLTransform()
    End Sub
End Class
