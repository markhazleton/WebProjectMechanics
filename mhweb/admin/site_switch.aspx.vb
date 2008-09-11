
Partial Class mhweb_site_switch
    Inherits mhPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        If IsPostBack Then
            If Request.Form("cbCompanyID") <> "" Then
                mySession.CompanyID = (Request.Form("cbCompanyID"))
                Dim mySiteFile As New mhSiteFile
                mySiteFile.GetCompanyValues(mySession.CompanyID, mySession.SiteDB)
                mySession.SiteTemplatePrefix = mySiteFile.SitePrefix
                'Response.Redirect("/")
            End If
        End If
        mySB.Append(mhDataCon.GetDropDownList("Company", "CompanyID", "CompanyName", "", mySession.CompanyID) & vbCrLf & " &nbsp; &nbsp; <input type=""submit"" value=""Switch Site""><br/><hr/>" & vbCrLf)
        Label1.Text = mySB.ToString
        '        mhwcm.AddSessionDebug(mySB)
        Label1.Text = mySB.ToString
    End Sub
End Class
