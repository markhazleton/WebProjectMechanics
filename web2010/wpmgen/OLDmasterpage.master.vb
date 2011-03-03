Imports WebProjectMechanics

Partial Class wpmgen_sample
    Inherits wpmMasterPage
    Public ParentPage As Object

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If TypeOf Me.Page Is AspNetMaker8_wpmWebsite Then
            ParentPage = CType(Me.Page, AspNetMaker8_wpmWebsite)
        Else
            ParentPage = CType(Me.Page, wpmPage)
        End If
        ParentPage.StartTimer = Environment.TickCount

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If TypeOf Me.Page Is AspNetMaker8_wpmWebsite Then
            If (ParentPage.Language Is Nothing) Then
                ParentPage.Language = New AspNetMaker8_wpmWebsite.cLanguage(New AspNetMaker8_wpmWebsite.AspNetMakerPage())
            End If
        End If
    End Sub

    Public Shared Function ew_CalcElapsedTime(ByVal tm As Long) As String
        Dim endTimer As Long = Environment.TickCount
        Return String.Format("<div>page processing time: {0} seconds</div>", (endTimer - tm) / 1000)
    End Function
End Class

