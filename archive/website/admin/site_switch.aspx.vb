Imports WebProjectMechanics

Partial Class admin_site_switch
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        Try
            If IsPostBack Then
                If Request.Form("cbCompanyID") <> String.Empty Then
                    Session("message") = String.Format("Old Company={0}", wpm_CurrentSiteID)
                    wpm_CurrentSiteID = Request.Form("cbCompanyID")
                    masterPage.myCompany.LoadSiteProfile("ORDER")
                    wpm_SiteTemplatePrefix = masterPage.myCompany.SitePrefix
                    wpm_DefaultSitePrefix = masterPage.myCompany.SitePrefix
                End If
            End If
            mySB.Append(ApplicationDAL.GetDropDownList("Company", "CompanyID", "CompanyName", "", Session("CompanyID")) & vbCrLf _
                       & " &nbsp; &nbsp; <input type=""submit"" value=""Switch Site""><br/><hr/>" & vbCrLf)
            Label1.Text = mySB.ToString
        Catch ex As Exception
            Label1.Text = ex.ToString()
        End Try
    End Sub
End Class
