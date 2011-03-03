Imports System.Data
Imports System.Data.Common
Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 Project Class (Table)
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

	Public Contact As cContact

	' Define table class
	Class cContact
		Inherits AspNetMakerBase
		Implements IDisposable

		' Used by system generated functions
		Private RsWrk As Object, sSqlWrk As String, sWhereWrk As String

		Private sFilterWrk As String

		Private arwrk As Object

		Private armultiwrk() As String

		Public TableFilter As String = ""

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
				Return "Contact"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Contact"
			End Get
		End Property

		' Table type
		Public ReadOnly Property TableType() As String
			Get
				Return "TABLE"
			End Get
		End Property

		' Table caption
		Public ReadOnly Property TableCaption() As String
			Get
				Return Language.TablePhrase(TableVar, "TblCaption")
			End Get
		End Property

		' Page caption
		Public ReadOnly Property PageCaption(Page As Integer) As String
			Get
				Dim sPageCaption As String
				sPageCaption = Language.TablePhrase(TableVar, "TblPageCaption" & Convert.ToString(Page))
				If sPageCaption = "" Then sPageCaption = "Page " & Convert.ToString(Page)
				Return sPageCaption
			End Get
		End Property

		' Export Return Page
		Public Property ExportReturnUrl() As String
			Get
				If ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_EXPORT_RETURN_URL) <> "" Then
					Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_EXPORT_RETURN_URL)
				Else
					Return ew_CurrentPage()
				End If
			End Get
			Set (ByVal Value As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_EXPORT_RETURN_URL) = Value
			End Set
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
				Return "Contact_Highlight"
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

		Public BasicSearchKeyword As String

		Public BasicSearchType As String

		' Basic search keyword		
		Public Property SessionBasicSearchKeyword() As String
			Get				
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH)
			End Get
			Set(ByVal Value As String)				
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_BASIC_SEARCH) = Value
			End Set
		End Property

		' Basic Search Type		
		Public Property SessionBasicSearchType() As String
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
				Return "SELECT * FROM [Contact]"
			End Get
		End Property

		Public ReadOnly Property SqlWhere() As String
			Get ' Where
				Dim sWhere As String = ""
        TableFilter = "[CompanyID]=" & HttpContext.Current.Session("CompanyID") & ""
        If TableFilter <> "" Then
			If sWhere <> "" Then sWhere = "(" & sWhere & ") AND "
			sWhere &= "(" & TableFilter & ")"
        End If
        Return sWhere	
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
				Dim sFilter As String = CurrentFilter
				Dim sSort As String = SessionOrderBy			
				Return ew_BuildSelectSql(SqlSelect, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, sFilter, sSort)
			End Get
		End Property

		' Return table SQL with list page filter
		Public ReadOnly Property ListSQL() As String
			Get
				Dim sFilter As String = SessionWhere
				Dim sSort As String
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
				Return "SELECT COUNT(*) FROM (" & ListSQL & ")"
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
					values = values & SqlParameter(fld) & ","
				End If
			Next f
			If names.EndsWith(",") Then names = names.Remove(names.Length - 1)
			If values.EndsWith(",") Then values = values.Remove(values.Length - 1)
			If names = "" Then Return -1
			Sql = "INSERT INTO [Contact] (" & names & ") VALUES (" & values & ")"
			Dim command As OleDbCommand = Conn.GetCommand(Sql)
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					command.Parameters.Add(fld.FldVar, fld.FldDbType).Value = ParameterValue(fld, f.Value)	
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
					values = values & fld.FldExpression & "=" & SqlParameter(fld) & ","
				End If
			Next f
			If values.EndsWith(",") Then values = values.Remove(values.Length - 1)
			If values = "" Then Return -1
			Sql = "UPDATE [Contact] SET " & values
			If CurrentFilter <> "" Then Sql = Sql & " WHERE " & CurrentFilter
			Dim command As OleDbCommand = Conn.GetCommand(Sql)
			For Each f As DictionaryEntry In Rs
				fld = FieldByName(f.Key)
				If fld IsNot Nothing Then
					command.Parameters.Add(fld.FldVar, fld.FldDbType).Value = ParameterValue(fld, f.Value)
				End If
			Next
			Return command.ExecuteNonQuery()
		End Function

		' Convert to parameter name for use in SQL
		Public Function SqlParameter(ByRef fld As cField) As String
			Dim sValue As String = EW_DB_SQLPARAM_SYMBOL
			If EW_DB_SQLPARAM_SYMBOL <> "?" Then sValue = sValue & fld.FldVar
			Return sValue
		End Function

		' Convert value to object for parameter
		Public Function ParameterValue(ByRef fld As cField, value As Object) As Object
			Return value	
		End Function		

		' Delete
		Public Function Delete(ByRef Rs As OrderedDictionary) As Integer
			Dim Sql As String
			Dim fld As cField			
			Sql = "DELETE FROM [Contact] WHERE "
			fld = FieldByName("ContactID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("ContactID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[ContactID] = @ContactID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(ContactID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@ContactID@", ew_AdjustSql(ContactID.CurrentValue)) ' Replace key value
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
					Return "contact_list.aspx"
				End If
			End Get
		End Property

		' List url
		Public Function ListUrl() As String
			Return "contact_list.aspx"
		End Function

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("contact_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "contact_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("contact_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("contact_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("contact_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(ContactID.CurrentValue) Then
				sUrl = sUrl & "ContactID=" & ContactID.CurrentValue
			Else
				Return "javascript:alert(ewLanguage.Phrase(""InvalidRecord""));"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=Contact"
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
			If CurrentAction <> "" OrElse Export <> "" OrElse (fld.FldType = 201 OrElse fld.FldType = 203 OrElse fld.FldType = 205 OrElse fld.FldType = 141) Then
				OutStr = ""
			ElseIf fld.Sortable Then
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

		' Function LoadRecordCount
		' - Load record count based on filter
		Public Function LoadRecordCount(sFilter As String) As Integer

			' Set up filter (SQL WHERE clause)
			CurrentFilter = sFilter

			' Recordset Selecting event
			Recordset_Selecting(CurrentFilter)
			Dim sSql As String = SQL()
			Dim sSqlCnt As String = "SELECT COUNT(*) FROM (" & sSql & ")"

			' Try count sql first
			Try
				Return Conn.ExecuteScalar(sSqlCnt)
			Catch
			End Try

			' Loop datareader to get count
			Try
				Dim Rs As OleDbDataReader = Conn.GetTempDataReader(sSql)
				Dim Cnt As Integer = 0
				While Rs.Read()
					Cnt = Cnt + 1
				End While
				Rs.Close()		
				Return Cnt
			Catch
				Return 0
			End Try
		End Function

		' Load row values from recordset
		Public Sub LoadListRowValues(ByRef RsRow As OleDbDataReader)			
			ContactID.DbValue = RsRow("ContactID")
			LogonName.DbValue = RsRow("LogonName")
			LogonPassword.DbValue = RsRow("LogonPassword")
			GroupID.DbValue = RsRow("GroupID")
			CompanyID.DbValue = RsRow("CompanyID")
			TemplatePrefix.DbValue = RsRow("TemplatePrefix")
			Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
			zEMail.DbValue = RsRow("EMail")
			PrimaryContact.DbValue = RsRow("PrimaryContact")
			FirstName.DbValue = RsRow("FirstName")
			MiddleInitial.DbValue = RsRow("MiddleInitial")
			LastName.DbValue = RsRow("LastName")
			MobilPhone.DbValue = RsRow("MobilPhone")
			OfficePhone.DbValue = RsRow("OfficePhone")
			HomePhone.DbValue = RsRow("HomePhone")
			Pager.DbValue = RsRow("Pager")
			URL.DbValue = RsRow("URL")
			Address1.DbValue = RsRow("Address1")
			Address2.DbValue = RsRow("Address2")
			City.DbValue = RsRow("City")
			State.DbValue = RsRow("State")
			Country.DbValue = RsRow("Country")
			PostalCode.DbValue = RsRow("PostalCode")
			Biography.DbValue = RsRow("Biography")
			CreateDT.DbValue = RsRow("CreateDT")
			Paid.DbValue = RsRow("Paid")
			email_subscribe.DbValue = IIf(ew_ConvertToBool(RsRow("email_subscribe")), "1", "0")
			RoleID.DbValue = RsRow("RoleID")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

		'
		'  Common render codes
		'
			' LogonName

			LogonName.CellCssStyle = "white-space: nowrap;"
			LogonName.CellCssClass = ""
			LogonName.CellAttrs.Clear(): LogonName.ViewAttrs.Clear(): LogonName.EditAttrs.Clear()

			' CompanyID
			CompanyID.CellCssStyle = "white-space: nowrap;"
			CompanyID.CellCssClass = ""
			CompanyID.CellAttrs.Clear(): CompanyID.ViewAttrs.Clear(): CompanyID.EditAttrs.Clear()

			' EMail
			zEMail.CellCssStyle = "white-space: nowrap;"
			zEMail.CellCssClass = ""
			zEMail.CellAttrs.Clear(): zEMail.ViewAttrs.Clear(): zEMail.EditAttrs.Clear()

			' PrimaryContact
			PrimaryContact.CellCssStyle = "white-space: nowrap;"
			PrimaryContact.CellCssClass = ""
			PrimaryContact.CellAttrs.Clear(): PrimaryContact.ViewAttrs.Clear(): PrimaryContact.EditAttrs.Clear()

			' Row Rendering event
			Row_Rendering()

		'
		'  Render for View
		'
			' LogonName

			LogonName.ViewValue = LogonName.CurrentValue
			LogonName.CssStyle = ""
			LogonName.CssClass = ""
			LogonName.ViewCustomAttributes = ""

			' CompanyID
			If ew_NotEmpty(CompanyID.CurrentValue) Then
				sFilterWrk = "[CompanyID] = " & ew_AdjustSql(CompanyID.CurrentValue) & ""
			sSqlWrk = "SELECT [CompanyName] FROM [Company]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [CompanyName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CompanyID.ViewValue = RsWrk("CompanyName")
				Else
					CompanyID.ViewValue = CompanyID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CompanyID.ViewValue = System.DBNull.Value
			End If
			CompanyID.CssStyle = ""
			CompanyID.CssClass = ""
			CompanyID.ViewCustomAttributes = ""

			' EMail
			zEMail.ViewValue = zEMail.CurrentValue
			zEMail.CssStyle = ""
			zEMail.CssClass = ""
			zEMail.ViewCustomAttributes = ""

			' PrimaryContact
			PrimaryContact.ViewValue = PrimaryContact.CurrentValue
			PrimaryContact.CssStyle = ""
			PrimaryContact.CssClass = ""
			PrimaryContact.ViewCustomAttributes = ""

		'
		'  Render for View Refer
		'
			' LogonName

			LogonName.HrefValue = ""
			LogonName.TooltipValue = ""

			' CompanyID
			CompanyID.HrefValue = ""
			CompanyID.TooltipValue = ""

			' EMail
			zEMail.HrefValue = ""
			zEMail.TooltipValue = ""

			' PrimaryContact
			PrimaryContact.HrefValue = ""
			PrimaryContact.TooltipValue = ""

			' Row Rendered event
			Row_Rendered()
		End Sub

		' Aggregate list row values
		Public Sub AggregateListRowValues()
		End Sub

		' Aggregate list row (for rendering)
		Sub AggregateListRow()
		End Sub

		Public CurrentAction As String ' Current action

		Public UpdateConflict As String ' Update conflict 

		Public EventCancelled As Boolean ' Event cancelled

		Public CancelMessage As String ' Cancel message

		' Row Type
		Public RowType As Integer	

		Public CssClass As String = "" ' CSS class

		Public CssStyle As String = "" ' CSS style

		Public RowAttrs As New Hashtable

		' Row Styles
		Public ReadOnly Property RowStyles() As String
			Get
				Dim sAtt As String = ""
				Dim sStyle As String, sClass As String
				sStyle = CssStyle
				If RowAttrs("style") <> "" Then
					sStyle &= " " & RowAttrs("style")
				End If
				sClass = CssClass
				If RowAttrs("class") <> "" Then
					sClass &= " " & RowAttrs("class")
				End If
				If sStyle.Trim() <> "" Then
					sAtt &= " style=""" & sStyle.Trim() & """"
				End If
				If sClass.Trim() <> "" Then
					sAtt &= " class=""" & sClass.Trim() & """"
				End If
				Return sAtt
			End Get
		End Property

		' Row Attribute
		Public ReadOnly Property RowAttributes() As String
			Get
				Dim sAtt As String = RowStyles
				If m_Export = "" Then
					For Each Attr As DictionaryEntry In RowAttrs
						If (Attr.Key <> "style" AndAlso Attr.Key <> "class" AndAlso ew_NotEmpty(Attr.Value)) Then
							sAtt &= " " & Attr.Key & "=""" & Trim(Attr.Value) & """"
						End If
					Next
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
		Public ExportAll As Boolean = True

		' Send Email
		Public SendEmail As Boolean

		' Custom Inner Html
		Public TableCustomInnerHtml As Object

		'
		'  Field objects
		'		
		Public Function FieldByName(Name As String) As Object
			If Name = "ContactID" Then Return ContactID
			If Name = "LogonName" Then Return LogonName
			If Name = "LogonPassword" Then Return LogonPassword
			If Name = "GroupID" Then Return GroupID
			If Name = "CompanyID" Then Return CompanyID
			If Name = "TemplatePrefix" Then Return TemplatePrefix
			If Name = "Active" Then Return Active
			If Name = "EMail" Then Return zEMail
			If Name = "PrimaryContact" Then Return PrimaryContact
			If Name = "FirstName" Then Return FirstName
			If Name = "MiddleInitial" Then Return MiddleInitial
			If Name = "LastName" Then Return LastName
			If Name = "MobilPhone" Then Return MobilPhone
			If Name = "OfficePhone" Then Return OfficePhone
			If Name = "HomePhone" Then Return HomePhone
			If Name = "Pager" Then Return Pager
			If Name = "URL" Then Return URL
			If Name = "Address1" Then Return Address1
			If Name = "Address2" Then Return Address2
			If Name = "City" Then Return City
			If Name = "State" Then Return State
			If Name = "Country" Then Return Country
			If Name = "PostalCode" Then Return PostalCode
			If Name = "Biography" Then Return Biography
			If Name = "CreateDT" Then Return CreateDT
			If Name = "Paid" Then Return Paid
			If Name = "email_subscribe" Then Return email_subscribe
			If Name = "RoleID" Then Return RoleID
			Return Nothing		
		End Function

		' ContactID
		Private m_ContactID As cField

		Public ReadOnly Property ContactID() As cField
			Get
				If m_ContactID Is Nothing Then
					m_ContactID = New cField(m_Page, "Contact", "Contact", "x_ContactID", "ContactID", "[ContactID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_ContactID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_ContactID
			End Get
		End Property

		' LogonName
		Private m_LogonName As cField

		Public ReadOnly Property LogonName() As cField
			Get
				If m_LogonName Is Nothing Then
					m_LogonName = New cField(m_Page, "Contact", "Contact", "x_LogonName", "LogonName", "[LogonName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_LogonName
			End Get
		End Property

		' LogonPassword
		Private m_LogonPassword As cField

		Public ReadOnly Property LogonPassword() As cField
			Get
				If m_LogonPassword Is Nothing Then
					m_LogonPassword = New cField(m_Page, "Contact", "Contact", "x_LogonPassword", "LogonPassword", "[LogonPassword]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_LogonPassword
			End Get
		End Property

		' GroupID
		Private m_GroupID As cField

		Public ReadOnly Property GroupID() As cField
			Get
				If m_GroupID Is Nothing Then
					m_GroupID = New cField(m_Page, "Contact", "Contact", "x_GroupID", "GroupID", "[GroupID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_GroupID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_GroupID
			End Get
		End Property

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then
					m_CompanyID = New cField(m_Page, "Contact", "Contact", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_CompanyID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_CompanyID
			End Get
		End Property

		' TemplatePrefix
		Private m_TemplatePrefix As cField

		Public ReadOnly Property TemplatePrefix() As cField
			Get
				If m_TemplatePrefix Is Nothing Then
					m_TemplatePrefix = New cField(m_Page, "Contact", "Contact", "x_TemplatePrefix", "TemplatePrefix", "[TemplatePrefix]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_TemplatePrefix
			End Get
		End Property

		' Active
		Private m_Active As cField

		Public ReadOnly Property Active() As cField
			Get
				If m_Active Is Nothing Then
					m_Active = New cField(m_Page, "Contact", "Contact", "x_Active", "Active", "[Active]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				End If
				Return m_Active
			End Get
		End Property

		' EMail
		Private m_zEMail As cField

		Public ReadOnly Property zEMail() As cField
			Get
				If m_zEMail Is Nothing Then
					m_zEMail = New cField(m_Page, "Contact", "Contact", "x_zEMail", "EMail", "[EMail]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_zEMail
			End Get
		End Property

		' PrimaryContact
		Private m_PrimaryContact As cField

		Public ReadOnly Property PrimaryContact() As cField
			Get
				If m_PrimaryContact Is Nothing Then
					m_PrimaryContact = New cField(m_Page, "Contact", "Contact", "x_PrimaryContact", "PrimaryContact", "[PrimaryContact]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PrimaryContact
			End Get
		End Property

		' FirstName
		Private m_FirstName As cField

		Public ReadOnly Property FirstName() As cField
			Get
				If m_FirstName Is Nothing Then
					m_FirstName = New cField(m_Page, "Contact", "Contact", "x_FirstName", "FirstName", "[FirstName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_FirstName
			End Get
		End Property

		' MiddleInitial
		Private m_MiddleInitial As cField

		Public ReadOnly Property MiddleInitial() As cField
			Get
				If m_MiddleInitial Is Nothing Then
					m_MiddleInitial = New cField(m_Page, "Contact", "Contact", "x_MiddleInitial", "MiddleInitial", "[MiddleInitial]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_MiddleInitial
			End Get
		End Property

		' LastName
		Private m_LastName As cField

		Public ReadOnly Property LastName() As cField
			Get
				If m_LastName Is Nothing Then
					m_LastName = New cField(m_Page, "Contact", "Contact", "x_LastName", "LastName", "[LastName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_LastName
			End Get
		End Property

		' MobilPhone
		Private m_MobilPhone As cField

		Public ReadOnly Property MobilPhone() As cField
			Get
				If m_MobilPhone Is Nothing Then
					m_MobilPhone = New cField(m_Page, "Contact", "Contact", "x_MobilPhone", "MobilPhone", "[MobilPhone]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_MobilPhone
			End Get
		End Property

		' OfficePhone
		Private m_OfficePhone As cField

		Public ReadOnly Property OfficePhone() As cField
			Get
				If m_OfficePhone Is Nothing Then
					m_OfficePhone = New cField(m_Page, "Contact", "Contact", "x_OfficePhone", "OfficePhone", "[OfficePhone]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_OfficePhone
			End Get
		End Property

		' HomePhone
		Private m_HomePhone As cField

		Public ReadOnly Property HomePhone() As cField
			Get
				If m_HomePhone Is Nothing Then
					m_HomePhone = New cField(m_Page, "Contact", "Contact", "x_HomePhone", "HomePhone", "[HomePhone]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_HomePhone
			End Get
		End Property

		' Pager
		Private m_Pager As cField

		Public ReadOnly Property Pager() As cField
			Get
				If m_Pager Is Nothing Then
					m_Pager = New cField(m_Page, "Contact", "Contact", "x_Pager", "Pager", "[Pager]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Pager
			End Get
		End Property

		' URL
		Private m_URL As cField

		Public ReadOnly Property URL() As cField
			Get
				If m_URL Is Nothing Then
					m_URL = New cField(m_Page, "Contact", "Contact", "x_URL", "URL", "[URL]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_URL
			End Get
		End Property

		' Address1
		Private m_Address1 As cField

		Public ReadOnly Property Address1() As cField
			Get
				If m_Address1 Is Nothing Then
					m_Address1 = New cField(m_Page, "Contact", "Contact", "x_Address1", "Address1", "[Address1]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Address1
			End Get
		End Property

		' Address2
		Private m_Address2 As cField

		Public ReadOnly Property Address2() As cField
			Get
				If m_Address2 Is Nothing Then
					m_Address2 = New cField(m_Page, "Contact", "Contact", "x_Address2", "Address2", "[Address2]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Address2
			End Get
		End Property

		' City
		Private m_City As cField

		Public ReadOnly Property City() As cField
			Get
				If m_City Is Nothing Then
					m_City = New cField(m_Page, "Contact", "Contact", "x_City", "City", "[City]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_City
			End Get
		End Property

		' State
		Private m_State As cField

		Public ReadOnly Property State() As cField
			Get
				If m_State Is Nothing Then
					m_State = New cField(m_Page, "Contact", "Contact", "x_State", "State", "[State]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_State
			End Get
		End Property

		' Country
		Private m_Country As cField

		Public ReadOnly Property Country() As cField
			Get
				If m_Country Is Nothing Then
					m_Country = New cField(m_Page, "Contact", "Contact", "x_Country", "Country", "[Country]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Country
			End Get
		End Property

		' PostalCode
		Private m_PostalCode As cField

		Public ReadOnly Property PostalCode() As cField
			Get
				If m_PostalCode Is Nothing Then
					m_PostalCode = New cField(m_Page, "Contact", "Contact", "x_PostalCode", "PostalCode", "[PostalCode]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PostalCode
			End Get
		End Property

		' Biography
		Private m_Biography As cField

		Public ReadOnly Property Biography() As cField
			Get
				If m_Biography Is Nothing Then
					m_Biography = New cField(m_Page, "Contact", "Contact", "x_Biography", "Biography", "[Biography]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				End If
				Return m_Biography
			End Get
		End Property

		' CreateDT
		Private m_CreateDT As cField

		Public ReadOnly Property CreateDT() As cField
			Get
				If m_CreateDT Is Nothing Then
					m_CreateDT = New cField(m_Page, "Contact", "Contact", "x_CreateDT", "CreateDT", "[CreateDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  0)
				End If
				Return m_CreateDT
			End Get
		End Property

		' Paid
		Private m_Paid As cField

		Public ReadOnly Property Paid() As cField
			Get
				If m_Paid Is Nothing Then
					m_Paid = New cField(m_Page, "Contact", "Contact", "x_Paid", "Paid", "[Paid]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_Paid.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_Paid
			End Get
		End Property

		' email_subscribe
		Private m_email_subscribe As cField

		Public ReadOnly Property email_subscribe() As cField
			Get
				If m_email_subscribe Is Nothing Then
					m_email_subscribe = New cField(m_Page, "Contact", "Contact", "x_email_subscribe", "email_subscribe", "[email_subscribe]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				End If
				Return m_email_subscribe
			End Get
		End Property

		' RoleID
		Private m_RoleID As cField

		Public ReadOnly Property RoleID() As cField
			Get
				If m_RoleID Is Nothing Then
					m_RoleID = New cField(m_Page, "Contact", "Contact", "x_RoleID", "RoleID", "[RoleID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_RoleID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_RoleID
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

		' Recordset Search Validated event
		Public Sub Recordset_SearchValidated()

			' Enter your code here
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

		' Row Update Conflict event
		Public Function Row_UpdateConflict(rsold As OrderedDictionary, ByRef rsnew As OrderedDictionary) As Boolean

			' Enter your code here
			' To ignore conflict, set return value to False

			Return True
		End Function

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
			If m_ContactID IsNot Nothing Then m_ContactID.Dispose()
			If m_LogonName IsNot Nothing Then m_LogonName.Dispose()
			If m_LogonPassword IsNot Nothing Then m_LogonPassword.Dispose()
			If m_GroupID IsNot Nothing Then m_GroupID.Dispose()
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_TemplatePrefix IsNot Nothing Then m_TemplatePrefix.Dispose()
			If m_Active IsNot Nothing Then m_Active.Dispose()
			If m_zEMail IsNot Nothing Then m_zEMail.Dispose()
			If m_PrimaryContact IsNot Nothing Then m_PrimaryContact.Dispose()
			If m_FirstName IsNot Nothing Then m_FirstName.Dispose()
			If m_MiddleInitial IsNot Nothing Then m_MiddleInitial.Dispose()
			If m_LastName IsNot Nothing Then m_LastName.Dispose()
			If m_MobilPhone IsNot Nothing Then m_MobilPhone.Dispose()
			If m_OfficePhone IsNot Nothing Then m_OfficePhone.Dispose()
			If m_HomePhone IsNot Nothing Then m_HomePhone.Dispose()
			If m_Pager IsNot Nothing Then m_Pager.Dispose()
			If m_URL IsNot Nothing Then m_URL.Dispose()
			If m_Address1 IsNot Nothing Then m_Address1.Dispose()
			If m_Address2 IsNot Nothing Then m_Address2.Dispose()
			If m_City IsNot Nothing Then m_City.Dispose()
			If m_State IsNot Nothing Then m_State.Dispose()
			If m_Country IsNot Nothing Then m_Country.Dispose()
			If m_PostalCode IsNot Nothing Then m_PostalCode.Dispose()
			If m_Biography IsNot Nothing Then m_Biography.Dispose()
			If m_CreateDT IsNot Nothing Then m_CreateDT.Dispose()
			If m_Paid IsNot Nothing Then m_Paid.Dispose()
			If m_email_subscribe IsNot Nothing Then m_email_subscribe.Dispose()
			If m_RoleID IsNot Nothing Then m_RoleID.Dispose()
		End Sub
	End Class
End Class
