
Partial Class mhweb_catalog_mhCatalog
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySiteMap As New mhSiteMap(Session)
        Dim myCat As New mhcatalog(mySiteMap)
        Dim mysb As New StringBuilder(myCat.ProcessPageRequest(GetProperty("Page", String.Empty), GetProperty("i", String.Empty), GetProperty("Template", String.Empty)))
        mySiteMap.BuildTemplate(mysb)
        content.Text = mysb.ToString
    End Sub
End Class
