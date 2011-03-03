Imports WebProjectMechanics

Partial Class wpm_site_switch
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        If IsPostBack Then
            If Request.Form("cbCompanyID") <> String.Empty Then
                pageActiveSite.Session.CompanyID = (Request.Form("cbCompanyID"))
                Dim newActiveSite As New wpmActiveSite(Session)
                pageActiveSite.Session.SiteTemplatePrefix = newActiveSite.SitePrefix
            End If
        End If
        mySB.Append(wpmDataCon.GetDropDownList("Company", "CompanyID", "CompanyName", "", pageActiveSite.Session.CompanyID) & vbCrLf _
                    & " &nbsp; &nbsp; <input type=""submit"" value=""Switch Site""><br/><hr/>" & vbCrLf)
        Label1.Text = mySB.ToString
    End Sub
End Class
