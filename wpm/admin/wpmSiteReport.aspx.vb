Imports system.xml
Imports System.Xml.Xsl

Partial Class wpm_admin_SiteMapReport
    Inherits AspNetMaker7_WPMGen


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myWebHomePath As New String(HttpContext.Current.Server.MapPath(App.Config.wpmWebHome))
        Dim myXML As New wpmXML(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_NavigationAdmin.xsl")
        HttpContext.Current.Session("ListPageURL") = Request.RawUrl
        Select Case Me.GetProperty("ReportID", "NavigationAdmin")
            Case "NavigationAdmin"
                myContent.Text = myXML.getXMLTransform()
            Case "LinkAdmin"
                myContent.Text = myXML.getXMLTransform(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_LinkAdmin.xsl")
            Case "ParameterAdmin"
                myContent.Text = myXML.getXMLTransform(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_ParameterAdmin.xsl")
            Case "ImageAdmin"
                myContent.Text = myXML.getXMLTransform(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_ImageAdmin.xsl")
            Case "PageAlias"
                myContent.Text = myXML.getXMLTransform(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_PageAlias.xsl")
            Case Else
                myContent.Text = myXML.getXMLTransform(App.Config.ConfigFolderPath & "\index\" & pageActiveSite.CompanyName & "-site-file.xml", myWebHomePath & "style/wpm_" & Me.GetProperty("ReportID", "NavigationAdmin") & ".xsl")
        End Select
    End Sub
End Class
