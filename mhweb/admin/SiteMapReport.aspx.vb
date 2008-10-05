Imports system.xml
Imports System.Xml.Xsl

Partial Class mhweb_admin_SiteMapReport
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySiteMap As New mhSiteMap(Session)
        Dim myWebHomePath As New String(HttpContext.Current.Server.MapPath(mhConfig.mhWebHome))
        mySiteMap.mySession.ListPageURL = HttpContext.Current.Request.FilePath()
        mySiteMap.mySession.ListPageURL = Request.RawUrl
        Select Case Me.GetProperty("ReportID", "NavigationAdmin")
            Case "NavigationAdmin"
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_NavigationAdmin.xsl")
            Case "LinkAdmin"
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_LinkAdmin.xsl")
            Case "ParameterAdmin"
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_ParameterAdmin.xsl")
            Case "ImageAdmin"
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_ImageAdmin.xsl")
            Case "PageAlias"
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_PageAlias.xsl")
            Case Else
                myContent.Text = mySiteMap.getXMLTransform(mhConfig.mhWebConfigFolder & "\index\" & mySiteMap.mySiteFile.CompanyName & "-site-file.xml", myWebHomePath & "style/mhweb_" & Me.GetProperty("ReportID", "NavigationAdmin") & ".xsl")
        End Select
    End Sub
End Class
