Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Page)
'

Partial Class Company_delete
	Inherits AspNetMaker7_WPMGen

	' Page object
	Public Company_delete As cCompany_delete

	'
	' Page Class
	'
	Class cCompany_delete
		Inherits AspNetMakerPage
		Implements IDisposable		

		' Used by system generated functions
		Private RsWrk As Object, sSqlWrk As String, sWhereWrk As String

		Private arwrk As Object

		Private armultiwrk() As String		

		' Page URL
		Public ReadOnly Property PageUrl() As String
			Get
				Dim Url As String = ew_CurrentPage() & "?"
				If Company.UseTokenInUrl Then Url = Url & "t=" & Company.TableVar & "&" ' Add page token
				Return Url	
			End Get
		End Property

		' Validate page request
		Public Function IsPageRequest() As Boolean
			Dim Result As Boolean
			If Company.UseTokenInUrl Then
				Result = False
				If ObjForm IsNot Nothing Then
					Result = (Company.TableVar = ObjForm.GetValue("t"))
				End If
				If ew_Get("t") <> "" Then
					Result = (Company.TableVar = ew_Get("t"))
				End If
				Return Result
			End If
			Return True			
		End Function	

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
		'  Constructor
		'  - init objects
		'  - open connection
		'
		Public Sub New(ByRef APage As AspNetMaker7_WPMGen)				
			m_ParentPage = APage
			m_Page = Me	
			m_PageID = "delete"
			m_PageObjName = "Company_delete"
			m_PageObjTypeName = "cCompany_delete"

			' Table Name
			m_TableName = "Company"

			' Initialize table object
			Company = New cCompany(Me)

			' Connect to database
			Conn = New cConnection()
		End Sub

		'
		'  Subroutine Page_Init
		'  - called before page main
		'  - check Security
		'  - set up response header
		'  - call page load events
		'
		Public Sub Page_Init()

			' Global page loading event (in ewglobal*.vb)
			ParentPage.Page_Loading()

			' Page load event, used in current page
			Page_Load()
		End Sub

		'
		'  Class terminate
		'  - clean up page object
		'
		Public Sub Dispose() Implements IDisposable.Dispose
			Page_Terminate("")
		End Sub

		'
		'  Sub Page_Terminate
		'  - called when exit page
		'  - clean up connection and objects
		'  - if URL specified, redirect to URL
		'
		Sub Page_Terminate(url As String)

			' Page unload event, used in current page
			Page_Unload()

			' Global page unloaded event (in ewglobal*.vb)
			ParentPage.Page_Unloaded()

			' Close connection
			Conn.Dispose()
			Company.Dispose()

			' Go to URL if specified
			If url <> "" Then
				HttpContext.Current.Response.Clear()
				HttpContext.Current.Response.Redirect(url)
			End If
		End Sub

	Public lTotalRecs As Integer

	Public lRecCnt As Integer

	Public arRecKeys() As String

	'
	' Page main processing
	'
	Sub Page_Main()
		Dim sKey As String = ""
		Dim bSingleDelete As Boolean = True ' Initialize as single delete
		Dim nKeySelected As Integer = 0 ' Initialize selected key count
		Dim sKeyFld As String, arKeyFlds() As String
		Dim sFilter As String		

		' Load Key Parameters
		If ew_Get("CompanyID") <> "" Then
			Company.CompanyID.QueryStringValue = ew_Get("CompanyID")
			If Not IsNumeric(Company.CompanyID.QueryStringValue) Then
			Page_Terminate("Company_list.aspx") ' Prevent SQL injection, return to list
			End If
			sKey = sKey & Company.CompanyID.QueryStringValue
		Else
			bSingleDelete = False
		End If
		If bSingleDelete Then
			nKeySelected = 1 ' Set up key selected count
			Array.Resize(arRecKeys, 1) ' Set up key
			arRecKeys(0) = sKey
		Else
			If HttpContext.Current.Request.Form("key_m") IsNot Nothing Then ' Key in form
				arRecKeys = HttpContext.Current.Request.Form.GetValues("key_m")
				nKeySelected = arRecKeys.Length				
			End If
		End If
		If nKeySelected <= 0 Then Page_Terminate("Company_list.aspx") ' No key specified, return to list

		' Build filter
		For i As Integer = 0 to arRecKeys.GetUpperBound(0)
			sKey = arRecKeys(i).Trim()
			sFilter = sFilter & "("

			' Set up key field
			sKeyFld = sKey
			If Not IsNumeric(sKeyFld) Then Page_Terminate("Company_list.aspx") ' Prevent SQL injection, return to list
			sFilter = sFilter & "[CompanyID]=" & ew_AdjustSql(sKeyFld) & " AND "
			If sFilter.EndsWith(" AND ") Then sFilter = sFilter.Substring(0, sFilter.Length-5) & ") OR "
		Next
		If sFilter.EndsWith(" OR ") Then sFilter = sFilter.Substring(0, sFilter.Length-4)

		' Set up filter (SQL WHERE clause)
		' SQL constructor in Company class, Companyinfo.aspx

		Company.CurrentFilter = sFilter

		' Get action
		If ew_Post("a_delete") <> "" Then
			Company.CurrentAction = ew_Post("a_delete")
		Else
			Company.CurrentAction = "I"	' Display record
		End If
		Select Case Company.CurrentAction
			Case "D" ' Delete
				Company.SendEmail = True ' Send email on delete success
				If DeleteRows() Then ' delete rows
					Message = "Delete succeeded" ' Set up success message
				Page_Terminate(Company.ReturnUrl) ' Return to caller
				End If
		End Select
	End Sub

	'
	'  Function DeleteRows
	'  - Delete Records based on current filter
	'
	Function DeleteRows() As Boolean
		Dim sKey As String, sThisKey As String, sKeyFld As String
		Dim arKeyFlds() As String
		Dim RsDelete As OleDbDataReader = Nothing
		Dim sSql As String, sWrkFilter As String 
		Dim RsOld As ArrayList
		DeleteRows = True
		sWrkFilter = Company.CurrentFilter

		' Set up filter (SQL WHERE Clause)
		' SQL constructor in Company class, Companyinfo.aspx

		Company.CurrentFilter = sWrkFilter
		sSql = Company.SQL
		Conn.BeginTrans() ' Begin transaction
		Try
			RsDelete = Conn.GetDataReader(sSql)
			If Not RsDelete.HasRows Then
				Message = "No records found" ' No record found
				RsDelete.Close()
				RsDelete.Dispose()					
				Return False
			End If
		Catch e As Exception
			If EW_DEBUG_ENABLED Then Throw
			Message = e.Message
			Return False
		End Try

		' Clone old rows
		RsOld = Conn.GetRows(RsDelete)
		RsDelete.Close()
		RsDelete.Dispose()

		' Call Row_Deleting event
		If DeleteRows Then
			For Each Row As OrderedDictionary in RsOld
				DeleteRows = Company.Row_Deleting(Row)
				If Not DeleteRows Then Exit For
			Next Row
		End If
		If DeleteRows Then
			sKey = ""
			For Each Row As OrderedDictionary in RsOld
				sThisKey = ""
				If sThisKey <> "" Then sThisKey = sThisKey & EW_COMPOSITE_KEY_SEPARATOR
				sThisKey = sThisKey & Convert.ToString(Row("CompanyID"))
				Try
					Company.Delete(Row)
				Catch e As Exception
					If EW_DEBUG_ENABLED Then Throw			
					Message = e.Message ' Set up error message
					DeleteRows = False
					Exit For
				End Try
				If sKey <> "" Then sKey = sKey & ", "
				sKey = sKey & sThisKey
			Next Row
		Else

			' Set up error message
			If Company.CancelMessage <> "" Then
				Message = Company.CancelMessage
				Company.CancelMessage = ""
			Else
				Message = "Delete cancelled"
			End If
		End If
		If DeleteRows Then

			' Commit the changes
			Conn.CommitTrans()				

			' Write audit trail	
			For Each Row As OrderedDictionary in RsOld
				WriteAuditTrailOnDelete(Row)
			Next

			' Row deleted event
			For Each Row As OrderedDictionary in RsOld
				Company.Row_Deleted(Row)
			Next
		Else
			Conn.RollbackTrans() ' Rollback transaction			
		End If
	End Function

	'
	' Load default values
	'
	Sub LoadDefaultValues()
	End Sub

	'
	' Load recordset
	'
	Function LoadRecordset() As OleDbDataReader

		' Recordset Selecting event
		Company.Recordset_Selecting(Company.CurrentFilter)

		' Load list page SQL
		Dim sSql As String = Company.ListSQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)

		' Count
		lTotalRecs = -1		
		Try			
			If sSql.StartsWith("SELECT * FROM ", StringComparison.InvariantCultureIgnoreCase) AndAlso _				
				ew_Empty(Company.SqlGroupBy) AndAlso _
				ew_Empty(Company.SqlHaving) Then
				Dim sCntSql As String = Company.SelectCountSQL

				' Write SQL for debug
				If EW_DEBUG_ENABLED Then ew_Write("<br>" & sCntSql)
				lTotalRecs = Conn.ExecuteScalar(sCntSql)
			End If			
		Catch
		End Try

		' Load recordset
		Dim Rs As OleDbDataReader = Conn.GetDataReader(sSql)
		If lTotalRecs < 0 AndAlso Rs.HasRows Then
			lTotalRecs = 0
			While Rs.Read()
				lTotalRecs = lTotalRecs + 1
			End While
			Rs.Close()		
			Rs = Conn.GetDataReader(sSql)
		End If

		' Recordset Selected event
		Company.Recordset_Selected(Rs)
		Return Rs
	End Function

	'
	' Load row based on key values
	'
	Function LoadRow() As Boolean
		Dim RsRow As OleDbDataReader
		Dim sFilter As String = Company.KeyFilter

		' Row Selecting event
		Company.Row_Selecting(sFilter)

		' Load SQL based on filter
		Company.CurrentFilter = sFilter
		Dim sSql As String = Company.SQL

		' Write SQL for debug
		If EW_DEBUG_ENABLED Then ew_Write(sSql)
		Try
			RsRow = Conn.GetTempDataReader(sSql)	
			If Not RsRow.Read() Then
				Return False
			Else				
				LoadRowValues(RsRow) ' Load row values

				' Row Selected event
				Company.Row_Selected(RsRow)
				Return True	
			End If
		Catch
			If EW_DEBUG_ENABLED Then Throw
			Return False
		Finally
			Conn.CloseTempDataReader()
		End Try
	End Function

	'
	' Load row values from recordset
	'
	Sub LoadRowValues(ByRef RsRow As OleDbDataReader)
		Company.CompanyID.DbValue = RsRow("CompanyID")
		Company.CompanyName.DbValue = RsRow("CompanyName")
		Company.SiteTitle.DbValue = RsRow("SiteTitle")
		Company.SiteURL.DbValue = RsRow("SiteURL")
		Company.GalleryFolder.DbValue = RsRow("GalleryFolder")
		Company.SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
		Company.HomePageID.DbValue = RsRow("HomePageID")
		Company.DefaultArticleID.DbValue = RsRow("DefaultArticleID")
		Company.SiteTemplate.DbValue = RsRow("SiteTemplate")
		Company.DefaultSiteTemplate.DbValue = RsRow("DefaultSiteTemplate")
		Company.UseBreadCrumbURL.DbValue = IIf(ew_ConvertToBool(RsRow("UseBreadCrumbURL")), "1", "0")
		Company.SingleSiteGallery.DbValue = IIf(ew_ConvertToBool(RsRow("SingleSiteGallery")), "1", "0")
		Company.ActiveFL.DbValue = IIf(ew_ConvertToBool(RsRow("ActiveFL")), "1", "0")
		Company.Address.DbValue = RsRow("Address")
		Company.City.DbValue = RsRow("City")
		Company.StateOrProvince.DbValue = RsRow("StateOrProvince")
		Company.PostalCode.DbValue = RsRow("PostalCode")
		Company.Country.DbValue = RsRow("Country")
		Company.PhoneNumber.DbValue = RsRow("PhoneNumber")
		Company.FaxNumber.DbValue = RsRow("FaxNumber")
		Company.DefaultPaymentTerms.DbValue = RsRow("DefaultPaymentTerms")
		Company.DefaultInvoiceDescription.DbValue = RsRow("DefaultInvoiceDescription")
		Company.Component.DbValue = RsRow("Component")
		Company.FromEmail.DbValue = RsRow("FromEmail")
		Company.SMTP.DbValue = RsRow("SMTP")
	End Sub

	'
	' Render row values based on field settings
	'
	Sub RenderRow()

		' Row Rendering event
		Company.Row_Rendering()

		'
		'  Common render codes for all row types
		'
		' CompanyName

		Company.CompanyName.CellCssStyle = ""
		Company.CompanyName.CellCssClass = ""

		' SiteTitle
		Company.SiteTitle.CellCssStyle = ""
		Company.SiteTitle.CellCssClass = ""

		' SiteURL
		Company.SiteURL.CellCssStyle = ""
		Company.SiteURL.CellCssClass = ""

		' SiteTemplate
		Company.SiteTemplate.CellCssStyle = ""
		Company.SiteTemplate.CellCssClass = ""

		' DefaultSiteTemplate
		Company.DefaultSiteTemplate.CellCssStyle = ""
		Company.DefaultSiteTemplate.CellCssClass = ""

		' ActiveFL
		Company.ActiveFL.CellCssStyle = ""
		Company.ActiveFL.CellCssClass = ""

		'
		'  View  Row
		'

		If Company.RowType = EW_ROWTYPE_VIEW Then ' View row

			' CompanyID
			Company.CompanyID.ViewValue = Company.CompanyID.CurrentValue
			Company.CompanyID.CssStyle = ""
			Company.CompanyID.CssClass = ""
			Company.CompanyID.ViewCustomAttributes = ""

			' CompanyName
			Company.CompanyName.ViewValue = Company.CompanyName.CurrentValue
			Company.CompanyName.CssStyle = ""
			Company.CompanyName.CssClass = ""
			Company.CompanyName.ViewCustomAttributes = ""

			' SiteTitle
			Company.SiteTitle.ViewValue = Company.SiteTitle.CurrentValue
			Company.SiteTitle.CssStyle = ""
			Company.SiteTitle.CssClass = ""
			Company.SiteTitle.ViewCustomAttributes = ""

			' SiteURL
			Company.SiteURL.ViewValue = Company.SiteURL.CurrentValue
			Company.SiteURL.CssStyle = ""
			Company.SiteURL.CssClass = ""
			Company.SiteURL.ViewCustomAttributes = ""

			' GalleryFolder
			Company.GalleryFolder.ViewValue = Company.GalleryFolder.CurrentValue
			Company.GalleryFolder.CssStyle = ""
			Company.GalleryFolder.CssClass = ""
			Company.GalleryFolder.ViewCustomAttributes = ""

			' SiteCategoryTypeID
			If ew_NotEmpty(Company.SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(Company.SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					Company.SiteCategoryTypeID.ViewValue = Company.SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			Company.SiteCategoryTypeID.CssStyle = ""
			Company.SiteCategoryTypeID.CssClass = ""
			Company.SiteCategoryTypeID.ViewCustomAttributes = ""

			' HomePageID
			If ew_NotEmpty(Company.HomePageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(Company.HomePageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.HomePageID.ViewValue = RsWrk("PageName")
				Else
					Company.HomePageID.ViewValue = Company.HomePageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.HomePageID.ViewValue = System.DBNull.Value
			End If
			Company.HomePageID.CssStyle = ""
			Company.HomePageID.CssClass = ""
			Company.HomePageID.ViewCustomAttributes = ""

			' DefaultArticleID
			If ew_NotEmpty(Company.DefaultArticleID.CurrentValue) Then
				sSqlWrk = "SELECT [Title] FROM [Article] WHERE [ArticleID] = " & ew_AdjustSql(Company.DefaultArticleID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.DefaultArticleID.ViewValue = RsWrk("Title")
				Else
					Company.DefaultArticleID.ViewValue = Company.DefaultArticleID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultArticleID.ViewValue = System.DBNull.Value
			End If
			Company.DefaultArticleID.CssStyle = ""
			Company.DefaultArticleID.CssClass = ""
			Company.DefaultArticleID.ViewCustomAttributes = ""

			' SiteTemplate
			If ew_NotEmpty(Company.SiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.SiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.SiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.SiteTemplate.ViewValue = Company.SiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.SiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.SiteTemplate.CssStyle = ""
			Company.SiteTemplate.CssClass = ""
			Company.SiteTemplate.ViewCustomAttributes = ""

			' DefaultSiteTemplate
			If ew_NotEmpty(Company.DefaultSiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(Company.DefaultSiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					Company.DefaultSiteTemplate.ViewValue = RsWrk("Name")
				Else
					Company.DefaultSiteTemplate.ViewValue = Company.DefaultSiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				Company.DefaultSiteTemplate.ViewValue = System.DBNull.Value
			End If
			Company.DefaultSiteTemplate.CssStyle = ""
			Company.DefaultSiteTemplate.CssClass = ""
			Company.DefaultSiteTemplate.ViewCustomAttributes = ""

			' UseBreadCrumbURL
			If Convert.ToString(Company.UseBreadCrumbURL.CurrentValue) = "1" Then
				Company.UseBreadCrumbURL.ViewValue = "Yes"
			Else
				Company.UseBreadCrumbURL.ViewValue = "No"
			End If
			Company.UseBreadCrumbURL.CssStyle = ""
			Company.UseBreadCrumbURL.CssClass = ""
			Company.UseBreadCrumbURL.ViewCustomAttributes = ""

			' SingleSiteGallery
			If Convert.ToString(Company.SingleSiteGallery.CurrentValue) = "1" Then
				Company.SingleSiteGallery.ViewValue = "Yes"
			Else
				Company.SingleSiteGallery.ViewValue = "No"
			End If
			Company.SingleSiteGallery.CssStyle = ""
			Company.SingleSiteGallery.CssClass = ""
			Company.SingleSiteGallery.ViewCustomAttributes = ""

			' ActiveFL
			If Convert.ToString(Company.ActiveFL.CurrentValue) = "1" Then
				Company.ActiveFL.ViewValue = "Yes"
			Else
				Company.ActiveFL.ViewValue = "No"
			End If
			Company.ActiveFL.CssStyle = ""
			Company.ActiveFL.CssClass = ""
			Company.ActiveFL.ViewCustomAttributes = ""

			' Address
			Company.Address.ViewValue = Company.Address.CurrentValue
			Company.Address.CssStyle = ""
			Company.Address.CssClass = ""
			Company.Address.ViewCustomAttributes = ""

			' City
			Company.City.ViewValue = Company.City.CurrentValue
			Company.City.CssStyle = ""
			Company.City.CssClass = ""
			Company.City.ViewCustomAttributes = ""

			' StateOrProvince
			Company.StateOrProvince.ViewValue = Company.StateOrProvince.CurrentValue
			Company.StateOrProvince.CssStyle = ""
			Company.StateOrProvince.CssClass = ""
			Company.StateOrProvince.ViewCustomAttributes = ""

			' PostalCode
			Company.PostalCode.ViewValue = Company.PostalCode.CurrentValue
			Company.PostalCode.CssStyle = ""
			Company.PostalCode.CssClass = ""
			Company.PostalCode.ViewCustomAttributes = ""

			' Country
			Company.Country.ViewValue = Company.Country.CurrentValue
			Company.Country.CssStyle = ""
			Company.Country.CssClass = ""
			Company.Country.ViewCustomAttributes = ""

			' PhoneNumber
			Company.PhoneNumber.ViewValue = Company.PhoneNumber.CurrentValue
			Company.PhoneNumber.CssStyle = ""
			Company.PhoneNumber.CssClass = ""
			Company.PhoneNumber.ViewCustomAttributes = ""

			' FaxNumber
			Company.FaxNumber.ViewValue = Company.FaxNumber.CurrentValue
			Company.FaxNumber.CssStyle = ""
			Company.FaxNumber.CssClass = ""
			Company.FaxNumber.ViewCustomAttributes = ""

			' DefaultPaymentTerms
			Company.DefaultPaymentTerms.ViewValue = Company.DefaultPaymentTerms.CurrentValue
			Company.DefaultPaymentTerms.CssStyle = ""
			Company.DefaultPaymentTerms.CssClass = ""
			Company.DefaultPaymentTerms.ViewCustomAttributes = ""

			' DefaultInvoiceDescription
			Company.DefaultInvoiceDescription.ViewValue = Company.DefaultInvoiceDescription.CurrentValue
			Company.DefaultInvoiceDescription.CssStyle = ""
			Company.DefaultInvoiceDescription.CssClass = ""
			Company.DefaultInvoiceDescription.ViewCustomAttributes = ""

			' Component
			Company.Component.ViewValue = Company.Component.CurrentValue
			Company.Component.CssStyle = ""
			Company.Component.CssClass = ""
			Company.Component.ViewCustomAttributes = ""

			' FromEmail
			Company.FromEmail.ViewValue = Company.FromEmail.CurrentValue
			Company.FromEmail.CssStyle = ""
			Company.FromEmail.CssClass = ""
			Company.FromEmail.ViewCustomAttributes = ""

			' SMTP
			Company.SMTP.ViewValue = Company.SMTP.CurrentValue
			Company.SMTP.CssStyle = ""
			Company.SMTP.CssClass = ""
			Company.SMTP.ViewCustomAttributes = ""

			' View refer script
			' CompanyName

			Company.CompanyName.HrefValue = ""

			' SiteTitle
			Company.SiteTitle.HrefValue = ""

			' SiteURL
			Company.SiteURL.HrefValue = ""

			' SiteTemplate
			Company.SiteTemplate.HrefValue = ""

			' DefaultSiteTemplate
			Company.DefaultSiteTemplate.HrefValue = ""

			' ActiveFL
			Company.ActiveFL.HrefValue = ""
		End If

		' Row Rendered event
		Company.Row_Rendered()
	End Sub

	' Write Audit Trail start/end for grid update
	Sub WriteAuditTrailDummy(typ As String)
		Dim table As String = "Company"
		Dim filePfx As String = "log"
		Dim curDate As String, curTime As String, id As String, user As String, action As String
		Dim i As Integer
		Dim dt As DateTime = Now()		
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()
		action = typ
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "", "", "", "")
	End Sub

	' Write Audit Trail (delete page)
	Sub WriteAuditTrailOnDelete(ByRef RsSrc As OrderedDictionary)
		Dim table As String = "Company"
		Dim filePfx As String = "log"
		Dim action As String = "D"
		Dim newvalue As Object = ""
		Dim curDate As String, curTime As String, id As String, user As String, field As String
		Dim keyvalue As Object, oldvalue As Object
		Dim dt As DateTime = Now()
		curDate = dt.ToString("yyyy/MM/dd")
		curTime = dt.ToString("HH:mm:ss")
		id = HttpContext.Current.Request.ServerVariables("SCRIPT_NAME")
    user = CurrentUserName()

		' Get key value
		Dim sKey As String = ""
		If sKey <> "" Then sKey = sKey & EW_COMPOSITE_KEY_SEPARATOR
		sKey = sKey & RsSrc("CompanyID")
		keyvalue = sKey

		' CompanyID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyID", keyvalue, RsSrc("CompanyID"), newvalue)

		' CompanyName Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "CompanyName", keyvalue, RsSrc("CompanyName"), newvalue)

		' SiteTitle Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteTitle", keyvalue, RsSrc("SiteTitle"), newvalue)

		' SiteURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteURL", keyvalue, RsSrc("SiteURL"), newvalue)

		' GalleryFolder Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "GalleryFolder", keyvalue, RsSrc("GalleryFolder"), newvalue)

		' SiteCategoryTypeID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteCategoryTypeID", keyvalue, RsSrc("SiteCategoryTypeID"), newvalue)

		' HomePageID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "HomePageID", keyvalue, RsSrc("HomePageID"), newvalue)

		' DefaultArticleID Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultArticleID", keyvalue, RsSrc("DefaultArticleID"), newvalue)

		' SiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SiteTemplate", keyvalue, RsSrc("SiteTemplate"), newvalue)

		' DefaultSiteTemplate Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultSiteTemplate", keyvalue, RsSrc("DefaultSiteTemplate"), newvalue)

		' UseBreadCrumbURL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "UseBreadCrumbURL", keyvalue, RsSrc("UseBreadCrumbURL"), newvalue)

		' SingleSiteGallery Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SingleSiteGallery", keyvalue, RsSrc("SingleSiteGallery"), newvalue)

		' ActiveFL Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "ActiveFL", keyvalue, RsSrc("ActiveFL"), newvalue)

		' Address Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Address", keyvalue, RsSrc("Address"), newvalue)

		' City Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "City", keyvalue, RsSrc("City"), newvalue)

		' StateOrProvince Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "StateOrProvince", keyvalue, RsSrc("StateOrProvince"), newvalue)

		' PostalCode Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PostalCode", keyvalue, RsSrc("PostalCode"), newvalue)

		' Country Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Country", keyvalue, RsSrc("Country"), newvalue)

		' PhoneNumber Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "PhoneNumber", keyvalue, RsSrc("PhoneNumber"), newvalue)

		' FaxNumber Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "FaxNumber", keyvalue, RsSrc("FaxNumber"), newvalue)

		' DefaultPaymentTerms Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultPaymentTerms", keyvalue, RsSrc("DefaultPaymentTerms"), newvalue)

		' DefaultInvoiceDescription Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "DefaultInvoiceDescription", keyvalue, "<MEMO>", newvalue)

		' Component Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "Component", keyvalue, RsSrc("Component"), newvalue)

		' FromEmail Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "FromEmail", keyvalue, RsSrc("FromEmail"), newvalue)

		' SMTP Field
		ew_WriteAuditTrail(filePfx, curDate, curTime, id, user, action, table, "SMTP", keyvalue, RsSrc("SMTP"), newvalue)
	End Sub

		' Page Load event
		Public Sub Page_Load()

			'HttpContext.Current.Response.Write("Page Load")
		End Sub

		' Page Unload event
		Public Sub Page_Unload()

			'HttpContext.Current.Response.Write("Page Unload")
		End Sub
	End Class

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Buffer = EW_RESPONSE_BUFFER
		Response.Cache.SetCacheability(HttpCacheability.NoCache)

		' Page init
		Company_delete = New cCompany_delete(Me)		
		Company_delete.Page_Init()

		' Page main processing
		Company_delete.Page_Main()
	End Sub

	'
	' ASP.NET Page_Unload event
	'

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

		' Dispose page object
		If Company_delete IsNot Nothing Then Company_delete.Dispose()
	End Sub
End Class
