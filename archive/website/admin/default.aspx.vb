Imports WebProjectMechanics
Imports System

Partial Class wpm_admin_default
    Inherits AdminPage
    Private Const sIndexFileNameTemplate As String = "{0}\sites\{1}\{1}-site-file.xml"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim myWebHomePath As String = HttpContext.Current.Server.MapPath(wpm_SiteConfig.AdminFolder)
        Dim myXML As UtilityXMLDocument

        rptReports.DataSource = ReportMenu()
        rptReports.DataBind()

        rptNavigation.DataSource = LoadMenu()
        rptNavigation.DataBind()

        wpm_ListPageURL = Request.RawUrl
        Select Case wpm_GetProperty("Report", String.Empty)
            Case "Location"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/NavigationAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "Part"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/LinkAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "Parameter"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/ParameterAdmin.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "Alias"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/PageAlias.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "Inventory"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/SiteInventoryReport.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case "SiteProfile"
                myXML = New UtilityXMLDocument(String.Format(sIndexFileNameTemplate, wpm_SiteConfig.ConfigFolderPath, wpm_HostName), myWebHomePath & "xsl/SiteProfileReport.xsl")
                myContent.Text = myXML.getXMLTransform()
            Case Else
                myContent.Text = curCompany.LocationList.BuildPageTree(String.Empty, 0, String.Empty)
        End Select




    End Sub
    Private Function ReportMenu() As List(Of LookupItem)
        Dim myListItems As New List(Of LookupItem)
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=Location", .Name = "Location"})
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=Part", .Name = "Part"})
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=Parameter", .Name = "Parameter"})
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=Alias", .Name = "Alias"})
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=Inventory", .Name = "Inventory"})
        myListItems.Add(New LookupItem With {.Value = "/admin/default.aspx?Report=SiteProfile", .Name = "SiteProfile"})
        Return myListItems
    End Function

End Class
