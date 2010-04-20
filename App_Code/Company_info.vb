Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Table)
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	Public Company As cCompany

	' Define table class
	Class cCompany
		Inherits AspNetMakerBase
		Implements IDisposable

		' Used by system generated functions
		Private RsWrk As Object, sSqlWrk As String, sWhereWrk As String

		Private arwrk As Object

		Private armultiwrk() As String

		' Constructor
		Public Sub New(ByRef APage As AspNetMakerPage)
			m_Page = APage
			m_ParentPage = APage.ParentPage			
		End Sub

		' Define table level constants	
		Public UseTokenInUrl As Boolean = EW_USE_TOKEN_IN_URL

		' Table variable
		Public ReadOnly Property TableVar() As String
			Get
				Return "Company"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Company"
			End Get
		End Property

		' Records per page		
		Public Property RecordsPerPage() As Integer
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_REC_PER_PAGE)
			End Get
			Set(ByVal Value As Integer)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_REC_PER_PAGE) = Value
			End Set
		End Property

		' Start record number		
		Public Property StartRecordNumber() As Integer
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_START_REC)
			End Get
			Set(ByVal Value As Integer)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_START_REC) = Value
			End Set
		End Property

		' Search Highlight Name
		Public ReadOnly Property HighlightName() As String
			Get
				Return "Company_Highlight"
			End Get
		End Property

		' Advanced search
		Public Function GetAdvancedSearch(ByRef fld As Object) As String			
			Return Convert.ToString(ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_ADVANCED_SEARCH & "_" & fld))
		End Function

		Public Sub SetAdvancedSearch(ByRef fld As Object, v As Object)			
			If ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_ADVANCED_SEARCH & "_" & fld) <> v Then
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_ADVANCED_SEARCH & "_" & fld) = v
			End If
		End Sub

		' Basic search keyword		
		Public Property BasicSearchKeyword() As String
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH)
			End Get
			Set(ByVal Value As String)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH) = Value
			End Set
		End Property

		' Basic Search Type		
		Public Property BasicSearchType() As String
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH_TYPE)
			End Get
			Set(ByVal Value As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH_TYPE) = Value
			End Set
		End Property

		' Search WHERE clause		
		Public Property SearchWhere() As String
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_SEARCH_WHERE)
			End Get
			Set(ByVal Value As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_SEARCH_WHERE) = Value
			End Set
		End Property

		' Single column sort
		Public Sub UpdateSort(ByRef ofld As Object)
			Dim sLastSort As String, sSortField As String, sThisSort As Object
			If CurrentOrder = ofld.FldName Then
				sSortField = ofld.FldExpression
				sLastSort = ofld.Sort
				If CurrentOrderType = "ASC" OrElse CurrentOrderType = "DESC" Then
					sThisSort = CurrentOrderType
				Else
					If sLastSort = "ASC" Then sThisSort = "DESC" Else sThisSort = "ASC"
				End If
				ofld.Sort = sThisSort
				SessionOrderBy = sSortField & " " & sThisSort ' Save to Session
			Else
				ofld.Sort = ""
			End If
		End Sub

		' Session WHERE Clause		
		Public Property SessionWhere() As String
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_WHERE)
			End Get
			Set(ByVal Value As String)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_WHERE) = Value
			End Set
		End Property

		' Session ORDER BY		
		Public Property SessionOrderBy() As String
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_ORDER_BY)
			End Get
			Set(ByVal Value As String)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_ORDER_BY) = Value
			End Set
		End Property

		' Session key
		Public Function GetKey(ByRef fld As Object) As Object
			Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_KEY & "_" & fld)
		End Function

		Public Sub SetKey(ByRef fld As Object, ByRef v As Object)
			ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_KEY & "_" & fld) = v
		End Sub

		' Table level SQL
		Public ReadOnly Property SqlSelect() As String
			Get ' Select
				Return "SELECT * FROM [Company]"
			End Get
		End Property

		Public ReadOnly Property SqlWhere() As String
			Get ' Where
				Return ""
			End Get
		End Property

		Public ReadOnly Property SqlGroupBy() As String
			Get ' Group By
				Return ""
			End Get
		End Property

		Public ReadOnly Property SqlHaving() As String
			Get ' Having
				Return ""
			End Get
		End Property

		Public ReadOnly Property SqlOrderBy() As String
			Get ' Order By
				Return ""
			End Get
		End Property

		' SQL variables
		Public CurrentFilter As String ' Current filter

		Public CurrentOrder As String ' Current order

		Public CurrentOrderType As String ' Current order type

		' Get SQL
		Public Function GetSQL(where As String, orderby As String) As String
			Return ew_BuildSelectSql(SqlSelect, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, where, orderby)
		End Function

		' Table SQL
		Public ReadOnly Property SQL() As String
			Get
				Dim sFilter As String, sSort As String
				sFilter = CurrentFilter
				sSort = SessionOrderBy
				Return ew_BuildSelectSql(SqlSelect, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, sFilter, sSort)
			End Get
		End Property

		' Return table SQL with list page filter
		Public ReadOnly Property ListSQL() As String
			Get
				Dim sFilter As String, sSort As String
				sFilter = SessionWhere
				If CurrentFilter <> "" Then
					If sFilter <> "" Then
						sFilter = "(" & sFilter & ") AND (" & CurrentFilter & ")"
					Else
						sFilter = CurrentFilter
					End If
				End If
				sSort = SessionOrderBy
				Return ew_BuildSelectSql(SqlSelect, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, sFilter, sSort)
			End Get
		End Property

		' Return SQL for record count
		Public ReadOnly Property SelectCountSQL() As String
			Get
				Dim sFilter As String = SessionWhere
				If CurrentFilter <> "" Then
					If sFilter <> "" Then
						sFilter = "(" & sFilter & ") AND (" & CurrentFilter & ")"
					Else
						sFilter = CurrentFilter
					End If
				End If
				Return ew_BuildSelectSql("SELECT COUNT(*) FROM " & SqlSelect.Substring(14), SqlWhere, "", "", "", sFilter, "")
			End Get
		End Property		

		' Insert
		Public Function Insert(ByRef Rs As OrderedDictionary) As Integer
			Dim Sql As String = ""			
			Dim names As String = ""
			Dim values As String = ""			
			Dim value As Object
			Dim fld As cField		
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					names = names & fld.FldExpression & ","
					values = values & EW_DB_SQLPARAM_SYMBOL
					If EW_DB_SQLPARAM_SYMBOL <> "?" Then values = values & fld.FldVar
					values = values & ","
				End If
			Next f
			If names.EndsWith(",") Then names = names.Remove(names.Length - 1)
			If values.EndsWith(",") Then values = values.Remove(values.Length - 1)
			If names = "" Then Return -1
			Sql = "INSERT INTO [Company] (" & names & ") VALUES (" & values & ")"
			Dim command As OleDbCommand = Conn.GetCommand(Sql)
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					value = f.Value	
					command.Parameters.Add(fld.FldVar, fld.FldDbType).Value = value	
				End If
			Next
			Return command.ExecuteNonQuery()			
		End Function

		' Update
		Public Function Update(ByRef Rs As OrderedDictionary) As Integer
			Dim Sql As String = ""			
			Dim values As String = ""			
			Dim value As Object
			Dim fld As cField
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					values = values & fld.FldExpression & "=" & EW_DB_SQLPARAM_SYMBOL
					If EW_DB_SQLPARAM_SYMBOL <> "?" Then values = values & fld.FldVar
					values = values & ","
				End If
			Next f
			If values.EndsWith(",") Then values = values.Remove(values.Length - 1)
			If values = "" Then Return -1
			Sql = "UPDATE [Company] SET " & values
			If CurrentFilter <> "" Then Sql = Sql & " WHERE " & CurrentFilter
			Dim command As OleDbCommand = Conn.GetCommand(Sql)
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					value = f.Value	
					command.Parameters.Add(fld.FldVar, fld.FldDbType).Value = value
				End If
			Next
			Return command.ExecuteNonQuery()
		End Function

		' Delete
		Public Function Delete(ByRef Rs As OrderedDictionary) As Integer
			Dim Sql As String
			Dim fld As cField			
			Sql = "DELETE FROM [Company] WHERE "
			fld = FieldByName("CompanyID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("CompanyID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[CompanyID] = @CompanyID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(CompanyID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@CompanyID@", ew_AdjustSql(CompanyID.CurrentValue)) ' Replace key value
				Return sKeyFilter
			End Get
		End Property

		' Return URL
		Public ReadOnly Property ReturnUrl() As String
			Get ' Get referer URL automatically
				If HttpContext.Current.Request.ServerVariables("HTTP_REFERER") IsNot Nothing Then
					If ew_ReferPage() <> ew_CurrentPage() AndAlso ew_ReferPage() <> "login.aspx" Then ' Referer not same page or login page
						Dim url As String = HttpContext.Current.Request.ServerVariables("HTTP_REFERER")
						If url.Contains("?a=") Then ' Remove action
							Dim p1 As Integer = url.LastIndexOf("?a=")							
							Dim p2 As Integer = url.IndexOf("&", p1)							
							If p2 > -1 Then
								url = url.Substring(0, p1 + 1) & url.Substring(p2 + 1)
							Else
								url = url.Substring(0, p1) 								
							End If
						End If
						ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_RETURN_URL) = url ' Save to Session
					End If
				End If
				If ew_NotEmpty(ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_RETURN_URL)) Then
					Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_RETURN_URL)
				Else
					Return "Company_list.aspx"
				End If
			End Get
		End Property

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("Company_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "Company_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("Company_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("Company_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("Company_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(CompanyID.CurrentValue) Then
				sUrl = sUrl & "CompanyID=" & CompanyID.CurrentValue
			Else
				Return "javascript:alert('Invalid Record! Key is null');"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=Company"
			End If
			If parm <> "" Then
				If OutStr <> "" Then OutStr = OutStr & "&"
				OutStr = OutStr & parm
			End If
			Return OutStr
		End Function

		' Sort URL
		Public Function SortUrl(ByRef fld As cField) As String
			Dim OutStr As String = ""
			If CurrentAction <> "" OrElse Export <> "" OrElse (fld.FldType = 201 OrElse fld.FldType = 203 OrElse fld.FldType = 205) Then
				OutStr = ""
			Else
				OutStr = ew_CurrentPage()
				Dim sUrlParm As String
				sUrlParm = UrlParm("order=" & HttpContext.Current.Server.UrlEncode(fld.FldName) & "&amp;ordertype=" & fld.ReverseSort)
				OutStr = OutStr & "?" & sUrlParm
			End If
			Return OutStr
		End Function		

		' Load rows based on filter
		Public Function LoadRs(sFilter As String) As OleDbDataReader
			Dim RsRows As OleDbDataReader

			' Set up filter (SQL WHERE clause)
			CurrentFilter = sFilter
			Dim sSql As String = SQL()
			Try
				RsRows = Conn.GetDataReader(sSql)
				If RsRows.HasRows Then
					Return RsRows
				Else
					RsRows.Close()
					RsRows.Dispose()
				End If
			Catch
			End Try
			Return Nothing
		End Function

		' Load row values from recordset
		Public Sub LoadListRowValues(ByRef RsRow As OleDbDataReader)			
			CompanyID.DbValue = RsRow("CompanyID")
			CompanyName.DbValue = RsRow("CompanyName")
			SiteTitle.DbValue = RsRow("SiteTitle")
			SiteURL.DbValue = RsRow("SiteURL")
			GalleryFolder.DbValue = RsRow("GalleryFolder")
			SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
			HomePageID.DbValue = RsRow("HomePageID")
			DefaultArticleID.DbValue = RsRow("DefaultArticleID")
			SiteTemplate.DbValue = RsRow("SiteTemplate")
			DefaultSiteTemplate.DbValue = RsRow("DefaultSiteTemplate")
			UseBreadCrumbURL.DbValue = IIf(ew_ConvertToBool(RsRow("UseBreadCrumbURL")), "1", "0")
			SingleSiteGallery.DbValue = IIf(ew_ConvertToBool(RsRow("SingleSiteGallery")), "1", "0")
			ActiveFL.DbValue = IIf(ew_ConvertToBool(RsRow("ActiveFL")), "1", "0")
			Address.DbValue = RsRow("Address")
			City.DbValue = RsRow("City")
			StateOrProvince.DbValue = RsRow("StateOrProvince")
			PostalCode.DbValue = RsRow("PostalCode")
			Country.DbValue = RsRow("Country")
			PhoneNumber.DbValue = RsRow("PhoneNumber")
			FaxNumber.DbValue = RsRow("FaxNumber")
			DefaultPaymentTerms.DbValue = RsRow("DefaultPaymentTerms")
			DefaultInvoiceDescription.DbValue = RsRow("DefaultInvoiceDescription")
			Component.DbValue = RsRow("Component")
			FromEmail.DbValue = RsRow("FromEmail")
			SMTP.DbValue = RsRow("SMTP")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

			' Row Rendering event
			Row_Rendering()

			' CompanyName
			CompanyName.ViewValue = CompanyName.CurrentValue
			CompanyName.CssStyle = ""
			CompanyName.CssClass = ""
			CompanyName.ViewCustomAttributes = ""

			' SiteTitle
			SiteTitle.ViewValue = SiteTitle.CurrentValue
			SiteTitle.CssStyle = ""
			SiteTitle.CssClass = ""
			SiteTitle.ViewCustomAttributes = ""

			' SiteURL
			SiteURL.ViewValue = SiteURL.CurrentValue
			SiteURL.CssStyle = ""
			SiteURL.CssClass = ""
			SiteURL.ViewCustomAttributes = ""

			' SiteTemplate
			If ew_NotEmpty(SiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(SiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteTemplate.ViewValue = RsWrk("Name")
				Else
					SiteTemplate.ViewValue = SiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteTemplate.ViewValue = System.DBNull.Value
			End If
			SiteTemplate.CssStyle = ""
			SiteTemplate.CssClass = ""
			SiteTemplate.ViewCustomAttributes = ""

			' DefaultSiteTemplate
			If ew_NotEmpty(DefaultSiteTemplate.CurrentValue) Then
				sSqlWrk = "SELECT [Name] FROM [SiteTemplate] WHERE [TemplatePrefix] = '" & ew_AdjustSql(DefaultSiteTemplate.CurrentValue) & "'"
				sSqlWrk = sSqlWrk & " ORDER BY [Name] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					DefaultSiteTemplate.ViewValue = RsWrk("Name")
				Else
					DefaultSiteTemplate.ViewValue = DefaultSiteTemplate.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				DefaultSiteTemplate.ViewValue = System.DBNull.Value
			End If
			DefaultSiteTemplate.CssStyle = ""
			DefaultSiteTemplate.CssClass = ""
			DefaultSiteTemplate.ViewCustomAttributes = ""

			' ActiveFL
			If Convert.ToString(ActiveFL.CurrentValue) = "1" Then
				ActiveFL.ViewValue = "Yes"
			Else
				ActiveFL.ViewValue = "No"
			End If
			ActiveFL.CssStyle = ""
			ActiveFL.CssClass = ""
			ActiveFL.ViewCustomAttributes = ""

			' CompanyName
			CompanyName.HrefValue = ""

			' SiteTitle
			SiteTitle.HrefValue = ""

			' SiteURL
			SiteURL.HrefValue = ""

			' SiteTemplate
			SiteTemplate.HrefValue = ""

			' DefaultSiteTemplate
			DefaultSiteTemplate.HrefValue = ""

			' ActiveFL
			ActiveFL.HrefValue = ""

			' Row Rendered event
			Row_Rendered()
		End Sub

		Public CurrentAction As String ' Current action

		Public EventCancelled As Boolean ' Event cancelled

		Public CancelMessage As String ' Cancel message

		' Row Type
		Public RowType As Integer	

		Public CssClass As String = "" ' CSS class

		Public CssStyle As String = "" ' CSS style

		Public RowClientEvents As String = "" ' Row client events

		' Row Attribute
		Public ReadOnly Property RowAttributes() As String
			Get
				Dim sAtt As String = ""
				If ew_NotEmpty(CssStyle) Then
					sAtt = sAtt & " style=""" & CssStyle.Trim() & """"
				End If
				If ew_NotEmpty(CssClass) Then
					sAtt = sAtt & " class=""" & CssClass.Trim() & """"
				End If
				If m_Export = "" Then
					If ew_NotEmpty(RowClientEvents) Then
						sAtt = sAtt & " " & RowClientEvents.Trim()
					End If
				End If
				Return sAtt
			End Get
		End Property

		' Export
		Private m_Export As String

		Public Property Export() As String
			Get
				Return m_Export
			End Get
			Set(ByVal Value As String)
				m_Export = Value
			End Set
		End Property

		' Export Original Value
		Public ExportOriginalValue As Boolean = EW_EXPORT_ORIGINAL_VALUE

		' Export All
		Public ExportAll As Boolean = EW_EXPORT_ALL

		' Send Email
		Public SendEmail As Boolean

		' Custom Inner Html
		Public TableCustomInnerHtml As Object

		'
		'  Field objects
		'		
		Public Function FieldByName(Name As String) As Object
			If Name = "CompanyID" Then Return CompanyID
			If Name = "CompanyName" Then Return CompanyName
			If Name = "SiteTitle" Then Return SiteTitle
			If Name = "SiteURL" Then Return SiteURL
			If Name = "GalleryFolder" Then Return GalleryFolder
			If Name = "SiteCategoryTypeID" Then Return SiteCategoryTypeID
			If Name = "HomePageID" Then Return HomePageID
			If Name = "DefaultArticleID" Then Return DefaultArticleID
			If Name = "SiteTemplate" Then Return SiteTemplate
			If Name = "DefaultSiteTemplate" Then Return DefaultSiteTemplate
			If Name = "UseBreadCrumbURL" Then Return UseBreadCrumbURL
			If Name = "SingleSiteGallery" Then Return SingleSiteGallery
			If Name = "ActiveFL" Then Return ActiveFL
			If Name = "Address" Then Return Address
			If Name = "City" Then Return City
			If Name = "StateOrProvince" Then Return StateOrProvince
			If Name = "PostalCode" Then Return PostalCode
			If Name = "Country" Then Return Country
			If Name = "PhoneNumber" Then Return PhoneNumber
			If Name = "FaxNumber" Then Return FaxNumber
			If Name = "DefaultPaymentTerms" Then Return DefaultPaymentTerms
			If Name = "DefaultInvoiceDescription" Then Return DefaultInvoiceDescription
			If Name = "Component" Then Return Component
			If Name = "FromEmail" Then Return FromEmail
			If Name = "SMTP" Then Return SMTP
			Return Nothing		
		End Function

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then m_CompanyID = New cField("Company", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_CompanyID
			End Get
		End Property

		' CompanyName
		Private m_CompanyName As cField

		Public ReadOnly Property CompanyName() As cField
			Get
				If m_CompanyName Is Nothing Then m_CompanyName = New cField("Company", "x_CompanyName", "CompanyName", "[CompanyName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CompanyName
			End Get
		End Property

		' SiteTitle
		Private m_SiteTitle As cField

		Public ReadOnly Property SiteTitle() As cField
			Get
				If m_SiteTitle Is Nothing Then m_SiteTitle = New cField("Company", "x_SiteTitle", "SiteTitle", "[SiteTitle]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_SiteTitle
			End Get
		End Property

		' SiteURL
		Private m_SiteURL As cField

		Public ReadOnly Property SiteURL() As cField
			Get
				If m_SiteURL Is Nothing Then m_SiteURL = New cField("Company", "x_SiteURL", "SiteURL", "[SiteURL]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_SiteURL
			End Get
		End Property

		' GalleryFolder
		Private m_GalleryFolder As cField

		Public ReadOnly Property GalleryFolder() As cField
			Get
				If m_GalleryFolder Is Nothing Then m_GalleryFolder = New cField("Company", "x_GalleryFolder", "GalleryFolder", "[GalleryFolder]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_GalleryFolder
			End Get
		End Property

		' SiteCategoryTypeID
		Private m_SiteCategoryTypeID As cField

		Public ReadOnly Property SiteCategoryTypeID() As cField
			Get
				If m_SiteCategoryTypeID Is Nothing Then m_SiteCategoryTypeID = New cField("Company", "x_SiteCategoryTypeID", "SiteCategoryTypeID", "[SiteCategoryTypeID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_SiteCategoryTypeID
			End Get
		End Property

		' HomePageID
		Private m_HomePageID As cField

		Public ReadOnly Property HomePageID() As cField
			Get
				If m_HomePageID Is Nothing Then m_HomePageID = New cField("Company", "x_HomePageID", "HomePageID", "[HomePageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_HomePageID
			End Get
		End Property

		' DefaultArticleID
		Private m_DefaultArticleID As cField

		Public ReadOnly Property DefaultArticleID() As cField
			Get
				If m_DefaultArticleID Is Nothing Then m_DefaultArticleID = New cField("Company", "x_DefaultArticleID", "DefaultArticleID", "[DefaultArticleID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_DefaultArticleID
			End Get
		End Property

		' SiteTemplate
		Private m_SiteTemplate As cField

		Public ReadOnly Property SiteTemplate() As cField
			Get
				If m_SiteTemplate Is Nothing Then m_SiteTemplate = New cField("Company", "x_SiteTemplate", "SiteTemplate", "[SiteTemplate]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_SiteTemplate
			End Get
		End Property

		' DefaultSiteTemplate
		Private m_DefaultSiteTemplate As cField

		Public ReadOnly Property DefaultSiteTemplate() As cField
			Get
				If m_DefaultSiteTemplate Is Nothing Then m_DefaultSiteTemplate = New cField("Company", "x_DefaultSiteTemplate", "DefaultSiteTemplate", "[DefaultSiteTemplate]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_DefaultSiteTemplate
			End Get
		End Property

		' UseBreadCrumbURL
		Private m_UseBreadCrumbURL As cField

		Public ReadOnly Property UseBreadCrumbURL() As cField
			Get
				If m_UseBreadCrumbURL Is Nothing Then m_UseBreadCrumbURL = New cField("Company", "x_UseBreadCrumbURL", "UseBreadCrumbURL", "[UseBreadCrumbURL]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_UseBreadCrumbURL
			End Get
		End Property

		' SingleSiteGallery
		Private m_SingleSiteGallery As cField

		Public ReadOnly Property SingleSiteGallery() As cField
			Get
				If m_SingleSiteGallery Is Nothing Then m_SingleSiteGallery = New cField("Company", "x_SingleSiteGallery", "SingleSiteGallery", "[SingleSiteGallery]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_SingleSiteGallery
			End Get
		End Property

		' ActiveFL
		Private m_ActiveFL As cField

		Public ReadOnly Property ActiveFL() As cField
			Get
				If m_ActiveFL Is Nothing Then m_ActiveFL = New cField("Company", "x_ActiveFL", "ActiveFL", "[ActiveFL]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_ActiveFL
			End Get
		End Property

		' Address
		Private m_Address As cField

		Public ReadOnly Property Address() As cField
			Get
				If m_Address Is Nothing Then m_Address = New cField("Company", "x_Address", "Address", "[Address]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Address
			End Get
		End Property

		' City
		Private m_City As cField

		Public ReadOnly Property City() As cField
			Get
				If m_City Is Nothing Then m_City = New cField("Company", "x_City", "City", "[City]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_City
			End Get
		End Property

		' StateOrProvince
		Private m_StateOrProvince As cField

		Public ReadOnly Property StateOrProvince() As cField
			Get
				If m_StateOrProvince Is Nothing Then m_StateOrProvince = New cField("Company", "x_StateOrProvince", "StateOrProvince", "[StateOrProvince]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_StateOrProvince
			End Get
		End Property

		' PostalCode
		Private m_PostalCode As cField

		Public ReadOnly Property PostalCode() As cField
			Get
				If m_PostalCode Is Nothing Then m_PostalCode = New cField("Company", "x_PostalCode", "PostalCode", "[PostalCode]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_PostalCode
			End Get
		End Property

		' Country
		Private m_Country As cField

		Public ReadOnly Property Country() As cField
			Get
				If m_Country Is Nothing Then m_Country = New cField("Company", "x_Country", "Country", "[Country]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Country
			End Get
		End Property

		' PhoneNumber
		Private m_PhoneNumber As cField

		Public ReadOnly Property PhoneNumber() As cField
			Get
				If m_PhoneNumber Is Nothing Then m_PhoneNumber = New cField("Company", "x_PhoneNumber", "PhoneNumber", "[PhoneNumber]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_PhoneNumber
			End Get
		End Property

		' FaxNumber
		Private m_FaxNumber As cField

		Public ReadOnly Property FaxNumber() As cField
			Get
				If m_FaxNumber Is Nothing Then m_FaxNumber = New cField("Company", "x_FaxNumber", "FaxNumber", "[FaxNumber]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_FaxNumber
			End Get
		End Property

		' DefaultPaymentTerms
		Private m_DefaultPaymentTerms As cField

		Public ReadOnly Property DefaultPaymentTerms() As cField
			Get
				If m_DefaultPaymentTerms Is Nothing Then m_DefaultPaymentTerms = New cField("Company", "x_DefaultPaymentTerms", "DefaultPaymentTerms", "[DefaultPaymentTerms]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_DefaultPaymentTerms
			End Get
		End Property

		' DefaultInvoiceDescription
		Private m_DefaultInvoiceDescription As cField

		Public ReadOnly Property DefaultInvoiceDescription() As cField
			Get
				If m_DefaultInvoiceDescription Is Nothing Then m_DefaultInvoiceDescription = New cField("Company", "x_DefaultInvoiceDescription", "DefaultInvoiceDescription", "[DefaultInvoiceDescription]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_DefaultInvoiceDescription
			End Get
		End Property

		' Component
		Private m_Component As cField

		Public ReadOnly Property Component() As cField
			Get
				If m_Component Is Nothing Then m_Component = New cField("Company", "x_Component", "Component", "[Component]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Component
			End Get
		End Property

		' FromEmail
		Private m_FromEmail As cField

		Public ReadOnly Property FromEmail() As cField
			Get
				If m_FromEmail Is Nothing Then m_FromEmail = New cField("Company", "x_FromEmail", "FromEmail", "[FromEmail]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_FromEmail
			End Get
		End Property

		' SMTP
		Private m_SMTP As cField

		Public ReadOnly Property SMTP() As cField
			Get
				If m_SMTP Is Nothing Then m_SMTP = New cField("Company", "x_SMTP", "SMTP", "[SMTP]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_SMTP
			End Get
		End Property

		' Table level events
		' Recordset Selecting event
		Public Sub Recordset_Selecting(ByRef filter As String) 

			' Enter your code here	
		End Sub

		' Recordset Selected event
		Public Sub Recordset_Selected(rs As DbDataReader)

			'HttpContext.Current.Response.Write("Recordset Selected")
		End Sub

		' Recordset Searching event
		Public Sub Recordset_Searching(ByRef filter As String)

			' Enter your code here
		End Sub

		' Row_Selecting event
		Public Sub Row_Selecting(ByRef filter As String)

			' Enter your code here	
		End Sub

		' Row Selected event
		Public Sub Row_Selected(rs As DbDataReader)

			'HttpContext.Current.Response.Write("Row Selected")
		End Sub

		' Row Rendering event
		Public Sub Row_Rendering()

			' Enter your code here	
		End Sub

		' Row Rendered event
		Public Sub Row_Rendered()

			' To view properties of field class, use:
			' HttpContext.Current.Response.Write(<FieldName>.AsString())

		End Sub

		' Row Inserting event
		Public Function Row_Inserting(ByRef rs As OrderedDictionary) As Boolean

			' Enter your code here
			' To cancel, set return value to False and error message to CancelMessage

			Return True
		End Function

		' Row Inserted event
		Public Sub Row_Inserted(rs As OrderedDictionary)

			'HttpContext.Current.Response.Write("Row Inserted")
		End Sub

		' Row Updating event
		Public Function Row_Updating(rsold As OrderedDictionary, ByRef rsnew As OrderedDictionary) As Boolean

			' Enter your code here
			' To cancel, set return value to False and error message to CancelMessage

			Return True
		End Function

		' Row Updated event
		Public Sub Row_Updated(rsold As OrderedDictionary, rsnew As OrderedDictionary)

			'HttpContext.Current.Response.Write("Row Updated")
		End Sub

		' Recordset Deleting event
		Public Function Row_Deleting(rs As OrderedDictionary) As Boolean

			' Enter your code here
			' To cancel, set return value to False and error message to CancelMessage

			Return True
		End Function

		' Recordset Deleted event
		Public Sub Row_Deleted(rs As OrderedDictionary)

			'HttpContext.Current.Response.Write("Row Deleted")
		End Sub

		' Email Sending event
		Public Function Email_Sending(ByRef Email As cEmail, Args As Hashtable) As Boolean

			'HttpContext.Current.Response.Write(Email.AsString())
			'HttpContext.Current.Response.End()

			Return True
		End Function

		' Class terminate
		Public Sub Dispose() Implements IDisposable.Dispose
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_CompanyName IsNot Nothing Then m_CompanyName.Dispose()
			If m_SiteTitle IsNot Nothing Then m_SiteTitle.Dispose()
			If m_SiteURL IsNot Nothing Then m_SiteURL.Dispose()
			If m_GalleryFolder IsNot Nothing Then m_GalleryFolder.Dispose()
			If m_SiteCategoryTypeID IsNot Nothing Then m_SiteCategoryTypeID.Dispose()
			If m_HomePageID IsNot Nothing Then m_HomePageID.Dispose()
			If m_DefaultArticleID IsNot Nothing Then m_DefaultArticleID.Dispose()
			If m_SiteTemplate IsNot Nothing Then m_SiteTemplate.Dispose()
			If m_DefaultSiteTemplate IsNot Nothing Then m_DefaultSiteTemplate.Dispose()
			If m_UseBreadCrumbURL IsNot Nothing Then m_UseBreadCrumbURL.Dispose()
			If m_SingleSiteGallery IsNot Nothing Then m_SingleSiteGallery.Dispose()
			If m_ActiveFL IsNot Nothing Then m_ActiveFL.Dispose()
			If m_Address IsNot Nothing Then m_Address.Dispose()
			If m_City IsNot Nothing Then m_City.Dispose()
			If m_StateOrProvince IsNot Nothing Then m_StateOrProvince.Dispose()
			If m_PostalCode IsNot Nothing Then m_PostalCode.Dispose()
			If m_Country IsNot Nothing Then m_Country.Dispose()
			If m_PhoneNumber IsNot Nothing Then m_PhoneNumber.Dispose()
			If m_FaxNumber IsNot Nothing Then m_FaxNumber.Dispose()
			If m_DefaultPaymentTerms IsNot Nothing Then m_DefaultPaymentTerms.Dispose()
			If m_DefaultInvoiceDescription IsNot Nothing Then m_DefaultInvoiceDescription.Dispose()
			If m_Component IsNot Nothing Then m_Component.Dispose()
			If m_FromEmail IsNot Nothing Then m_FromEmail.Dispose()
			If m_SMTP IsNot Nothing Then m_SMTP.Dispose()
		End Sub
	End Class
End Class