Imports WebProjectMechanics
Imports System

Public Class wpm_admin_skin_switch
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        If IsPostBack Then
            If Request.Form("cbTemplatePrefix") <> "" Then
                wpm_SiteTemplatePrefix = (Request.Form("cbTemplatePrefix"))
                wpm_DefaultSitePrefix = (Request.Form("cbTemplatePrefix"))
            End If
        End If
        mySB.Append(String.Format("{0}{1} &nbsp; &nbsp; <input type=""submit"" value=""Switch Skin""><br/><hr/>{1}", ApplicationDAL.GetDropDownList("SiteTemplate", "TemplatePrefix", "Name", "", wpm_SiteTemplatePrefix), vbCrLf))
        Literal1.Text = mySB.ToString
        Literal1.Text = mySB.ToString
    End Sub
End Class
