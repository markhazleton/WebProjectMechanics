Imports WebProjectMechanics

Partial Class wpm_skin_switch
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        If IsPostBack Then
            If Request.Form("cbTemplatePrefix") <> "" Then
                pageActiveSite.Session.SiteTemplatePrefix = (Request.Form("cbTemplatePrefix"))
            End If
        End If
        mySB.Append(wpmDataCon.GetDropDownList("SiteTemplate", "TemplatePrefix", "Name", "", pageActiveSite.SiteTemplatePrefix) & vbCrLf & " &nbsp; &nbsp; <input type=""submit"" value=""Switch Skin""><br/><hr/>" & vbCrLf)
        Literal1.Text = mySB.ToString
        Literal1.Text = mySB.ToString
    End Sub
End Class
