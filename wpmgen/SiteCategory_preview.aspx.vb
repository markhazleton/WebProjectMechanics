Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Preview)
'

Partial Class SiteCategory_preview
	Inherits AspNetMaker7_WPMGen

	Public nTotalRecs As Integer

	Public nRecCount As Integer

	' Page object
	Public SiteCategory_preview As cSiteCategory_preview

	' Page class for preview
	Class cSiteCategory_preview
		Inherits AspNetMakerPage
		Implements IDisposable		

		' Constructor
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)
			m_ParentPage = APage
			m_Page = Me			
		End Sub

		' SiteCategory
		Public Property SiteCategory() As cSiteCategory
			Get				
				Return ParentPage.SiteCategory
			End Get
			Set(ByVal v As cSiteCategory)
				ParentPage.SiteCategory = v	
			End Set	
		End Property

		'
		' Page main processing
		'
		Sub Page_Main()

			' Open connection to the database
			Conn = New cConnection()

			' Initialize table object
			SiteCategory = New cSiteCategory(Page)
		End Sub

		'
		'  Sub Page_Terminate
		'  - called when exit page
		'  - clean up connection and objects
		'  - if URL specified, redirect to URL
		'
		Sub Page_Terminate()	
			SiteCategory.Dispose()

			' Close connection
			Conn.Dispose()
		End Sub

		'
		'  Class terminate
		'  - clean up page object
		'
		Public Sub Dispose() Implements IDisposable.Dispose
			Page_Terminate()
		End Sub
	End Class	

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page main processing
		SiteCategory_preview = New cSiteCategory_preview(Me)
		SiteCategory_preview.Page_Main()

		' Load filter
		Dim filter As String = ew_Get("f")
		If filter.Trim() <> "" Then filter = cTEA.Decrypt(filter.Trim(), EW_RANDOM_KEY)
		If filter = "" Then filter = "0=1"

		' Load recordset			
		Rs = SiteCategory.LoadRs(filter)
		nTotalRecs = 0
		If Rs IsNot Nothing Then
			While Rs.Read()
				nTotalRecs = nTotalRecs + 1
			End While
			Rs.Close()
			Rs = SiteCategory.LoadRs(filter)
		End If		
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Close recordset
		If Rs IsNot Nothing Then
			Rs.Close()
			Rs.Dispose()
		End If

		' Dispose page object
		SiteCategory_preview.Dispose()
	End Sub
End Class