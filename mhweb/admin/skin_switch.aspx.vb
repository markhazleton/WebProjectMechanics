
Partial Class mhweb_skin_switch
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        If IsPostBack Then
            If Request.Form("cbTemplatePrefix") <> "" Then
                mySession.SiteTemplatePrefix = (Request.Form("cbTemplatePrefix"))
                'Response.Redirect("/")
            End If
        End If
        mySB.Append(mhDataCon.GetDropDownList("SiteTemplate", "TemplatePrefix", "Name", "", mySession.SiteTemplatePrefix) & vbCrLf & " &nbsp; &nbsp; <input type=""submit"" value=""Switch Skin""><br/><hr/>" & vbCrLf)
        Label1.Text = mySB.ToString
        '        mhwcm.AddSessionDebug(mySB)
        Label1.Text = mySB.ToString
    End Sub
End Class
