Imports system.xml
Imports System.Xml.Xsl

Partial Class mhweb_admin_SiteInventoryReport
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySiteMap As New mhSiteMap(Session)
        mySiteMap.mySession.ListPageURL = HttpContext.Current.Request.FilePath()
        myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", mhConfig.mhWebConfigFolder & "\style\LinkAdmin.xsl")
    End Sub
End Class
