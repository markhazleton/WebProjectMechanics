Imports WebProjectMechanics

Partial Class ProjectMechanicsModule
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With masterPage.myCompany
            .CurLocation.HideGlobalContent = True
            .GetHTML(.CurLocation.LocationBody, False, wpm_SiteTemplatePrefix)
        End With
    End Sub

End Class


