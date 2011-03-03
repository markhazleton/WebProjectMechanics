Imports WebProjectMechanics 

'
' ASP.NET code-behind class (Master Page)
'

Partial Class MasterPage
	Inherits wpmMasterPage

	Public ParentPage As Object

	'
	' Master page Page_Init event
	'

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		If TypeOf Me.Page Is AspNetMaker8_wpmWebsite Then
			ParentPage = CType(Me.Page, AspNetMaker8_wpmWebsite)
        else
            ParentPage = CType(Me.Page, wpmPage)
		End If
		ParentPage.StartTimer = Environment.TickCount
	End Sub

	'
	' Master page Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)  Handles Me.Load
        If TypeOf Me.Page Is AspNetMaker8_wpmWebsite Then
    		If (ParentPage.Language Is Nothing) Then
	    		ParentPage.Language = New AspNetMaker8_wpmWebsite.cLanguage(New AspNetMaker8_wpmWebsite.AspNetMakerPage())
		    End If
            If ParentPage.ew_Get("export") <> "" Then
                sExport = ParentPage.ew_Get("export")
            ElseIf ParentPage.ew_Post("exporttype") <> "" Then
                sExport = ParentPage.ew_Post("exporttype")
            End If
        End If
        Dim EmptyOrPrint As Boolean = (sExport = "" OrElse sExport = "print")
		ProjectCss.Visible = EmptyOrPrint
		ClientScript.Visible = EmptyOrPrint		
		StartupScript.Visible = EmptyOrPrint
		If sExport <> "" Then
			Links.Visible = False
			Header1.Visible = False
			MenuTop.Visible = False
			Menu.Visible = False
			MenuBottom.Visible = False
			Header2.Visible = False
			Footer.Visible = False
		End If		
	End Sub

    Public Shared Function ew_CalcElapsedTime(ByVal tm As Long) As String
        Dim endTimer As Long = Environment.TickCount
        Return "<div>page processing time: " & ((endTimer - tm) / 1000).ToString() & " seconds</div>"
    End Function
End Class
