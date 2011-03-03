Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Preview)
'

Partial Class zpage_preview
	Inherits AspNetMaker8_wpmWebsite

	' Page object
	Public zPage_preview As czPage_preview

	' Page class for preview
	Class czPage_preview
		Inherits AspNetMakerPage
		Implements IDisposable

		Public lTotalRecs As Integer

		Public lRowCnt As Integer				

		' Constructor
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)
			m_ParentPage = APage
			m_Page = Me

			' Initialize language object
			Language = New cLanguage(Me)
		End Sub

		' zPage
		Public Property zPage() As czPage
			Get				
				Return ParentPage.zPage
			End Get
			Set(ByVal v As czPage)
				ParentPage.zPage = v	
			End Set	
		End Property

		' Company
		Public Property Company() As cCompany
			Get				
				Return ParentPage.Company
			End Get
			Set(ByVal v As cCompany)
				ParentPage.Company = v	
			End Set	
		End Property

		'
		' Page main processing
		'
		Sub Page_Main()

			' Open connection to the database
			Conn = New cConnection()

			' Initialize table object
			zPage = New czPage(Page)
			Company = New cCompany(Page)
		End Sub

		'
		'  Sub Page_Terminate
		'  - called when exit page
		'  - clean up connection and objects
		'  - if URL specified, redirect to URL
		'
		Sub Page_Terminate()	
			zPage.Dispose()
			Company.Dispose()

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
		zPage_preview = New czPage_preview(Me)
		zPage_preview.Page_Main()

		' Load filter
		Dim filter As String = ew_Get("f")
		If filter.Trim() <> "" Then filter = cTEA.Decrypt(filter.Trim(), EW_RANDOM_KEY)
		If filter = "" Then filter = "0=1"

		' Load recordset
		' Call Recordset Selecting event

		zPage.Recordset_Selecting(filter)
		Rs = zPage.LoadRs(filter)
		zPage_preview.lTotalRecs = 0
		If Rs IsNot Nothing Then
			While Rs.Read()
				zPage_preview.lTotalRecs += 1
			End While
			Rs.Close()
			Rs = zPage.LoadRs(filter)
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
		zPage_preview.Dispose()
	End Sub
End Class