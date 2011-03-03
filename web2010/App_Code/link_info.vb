Imports System.Data
Imports System.Data.Common
Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 Project Class (Table)
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

	Public Link As cLink

	' Define table class
	Class cLink
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
				Return "Link"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Link"
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
				Return "Link_Highlight"
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

		' Current master table name
		Public Property CurrentMasterTable() As String
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_MASTER_TABLE)
			End Get
			Set(ByVal v As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_MASTER_TABLE) = v
			End Set
		End Property

		' Session master where clause
		Public Property MasterFilter() As String
			Get	
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_MASTER_FILTER)
			End Get
			Set(ByVal v As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_MASTER_FILTER) = v
			End Set
		End Property

		' Session detail where clause
		Public Property DetailFilter() As String
			Get
				Return ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_DETAIL_FILTER)
			End Get
			Set(ByVal v As String)
				ew_Session(EW_PROJECT_NAME & "_" & TableVar & "_" & EW_TABLE_DETAIL_FILTER) = v
			End Set
		End Property

		' Company
		Public ReadOnly Property Company() As cCompany
			Get
				Return ParentPage.Company
			End Get
		End Property		

		' Master filter
		Public ReadOnly Property SqlMasterFilter_Company() As String
			Get
				Return "[CompanyID]=@CompanyID@"
			End Get
		End Property

		' Detail filter
		Public ReadOnly Property SqlDetailFilter_Company() As String
			Get
				Return "[CompanyID]=@CompanyID@"
			End Get
		End Property

		' LinkType
		Public ReadOnly Property LinkType() As cLinkType
			Get
				Return ParentPage.LinkType
			End Get
		End Property		

		' Master filter
		Public ReadOnly Property SqlMasterFilter_LinkType() As String
			Get
				Return "[LinkTypeCD]='@LinkTypeCD@'"
			End Get
		End Property

		' Detail filter
		Public ReadOnly Property SqlDetailFilter_LinkType() As String
			Get
				Return "[LinkTypeCD]='@LinkTypeCD@'"
			End Get
		End Property

		' LinkCategory
		Public ReadOnly Property LinkCategory() As cLinkCategory
			Get
				Return ParentPage.LinkCategory
			End Get
		End Property		

		' Master filter
		Public ReadOnly Property SqlMasterFilter_LinkCategory() As String
			Get
				Return "[ID]=@ID@"
			End Get
		End Property

		' Detail filter
		Public ReadOnly Property SqlDetailFilter_LinkCategory() As String
			Get
				Return "[CategoryID]=@CategoryID@"
			End Get
		End Property

		' Table level SQL
		Public ReadOnly Property SqlSelect() As String
			Get ' Select
				Return "SELECT * FROM [Link]"
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
			Sql = "INSERT INTO [Link] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [Link] SET " & values
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
			Sql = "DELETE FROM [Link] WHERE "
			fld = FieldByName("ID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("ID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[ID] = @ID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(ID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@ID@", ew_AdjustSql(ID.CurrentValue)) ' Replace key value
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
					Return "link_list.aspx"
				End If
			End Get
		End Property

		' List url
		Public Function ListUrl() As String
			Return "link_list.aspx"
		End Function

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("link_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "link_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("link_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("link_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("link_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(ID.CurrentValue) Then
				sUrl = sUrl & "ID=" & ID.CurrentValue
			Else
				Return "javascript:alert(ewLanguage.Phrase(""InvalidRecord""));"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=Link"
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
			ID.DbValue = RsRow("ID")
			Title.DbValue = RsRow("Title")
			LinkTypeCD.DbValue = RsRow("LinkTypeCD")
			CategoryID.DbValue = RsRow("CategoryID")
			CompanyID.DbValue = RsRow("CompanyID")
			SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
			zPageID.DbValue = RsRow("PageID")
			Views.DbValue = IIf(ew_ConvertToBool(RsRow("Views")), "1", "0")
			Description.DbValue = RsRow("Description")
			URL.DbValue = RsRow("URL")
			Ranks.DbValue = RsRow("Ranks")
			UserID.DbValue = RsRow("UserID")
			ASIN.DbValue = RsRow("ASIN")
			DateAdd.DbValue = RsRow("DateAdd")
			UserName.DbValue = RsRow("UserName")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

		'
		'  Common render codes
		'
			' Title

			Title.CellCssStyle = "white-space: nowrap;"
			Title.CellCssClass = ""
			Title.CellAttrs.Clear(): Title.ViewAttrs.Clear(): Title.EditAttrs.Clear()

			' LinkTypeCD
			LinkTypeCD.CellCssStyle = "white-space: nowrap;"
			LinkTypeCD.CellCssClass = ""
			LinkTypeCD.CellAttrs.Clear(): LinkTypeCD.ViewAttrs.Clear(): LinkTypeCD.EditAttrs.Clear()

			' CategoryID
			CategoryID.CellCssStyle = "white-space: nowrap;"
			CategoryID.CellCssClass = ""
			CategoryID.CellAttrs.Clear(): CategoryID.ViewAttrs.Clear(): CategoryID.EditAttrs.Clear()

			' CompanyID
			CompanyID.CellCssStyle = "white-space: nowrap;"
			CompanyID.CellCssClass = ""
			CompanyID.CellAttrs.Clear(): CompanyID.ViewAttrs.Clear(): CompanyID.EditAttrs.Clear()

			' SiteCategoryGroupID
			SiteCategoryGroupID.CellCssStyle = "white-space: nowrap;"
			SiteCategoryGroupID.CellCssClass = ""
			SiteCategoryGroupID.CellAttrs.Clear(): SiteCategoryGroupID.ViewAttrs.Clear(): SiteCategoryGroupID.EditAttrs.Clear()

			' PageID
			zPageID.CellCssStyle = "white-space: nowrap;"
			zPageID.CellCssClass = ""
			zPageID.CellAttrs.Clear(): zPageID.ViewAttrs.Clear(): zPageID.EditAttrs.Clear()

			' Views
			Views.CellCssStyle = "white-space: nowrap;"
			Views.CellCssClass = ""
			Views.CellAttrs.Clear(): Views.ViewAttrs.Clear(): Views.EditAttrs.Clear()

			' Ranks
			Ranks.CellCssStyle = "white-space: nowrap;"
			Ranks.CellCssClass = ""
			Ranks.CellAttrs.Clear(): Ranks.ViewAttrs.Clear(): Ranks.EditAttrs.Clear()

			' UserID
			UserID.CellCssStyle = "white-space: nowrap;"
			UserID.CellCssClass = ""
			UserID.CellAttrs.Clear(): UserID.ViewAttrs.Clear(): UserID.EditAttrs.Clear()

			' ASIN
			ASIN.CellCssStyle = "white-space: nowrap;"
			ASIN.CellCssClass = ""
			ASIN.CellAttrs.Clear(): ASIN.ViewAttrs.Clear(): ASIN.EditAttrs.Clear()

			' DateAdd
			DateAdd.CellCssStyle = "white-space: nowrap;"
			DateAdd.CellCssClass = ""
			DateAdd.CellAttrs.Clear(): DateAdd.ViewAttrs.Clear(): DateAdd.EditAttrs.Clear()

			' Row Rendering event
			Row_Rendering()

		'
		'  Render for View
		'
			' Title

			Title.ViewValue = Title.CurrentValue
			Title.CssStyle = ""
			Title.CssClass = ""
			Title.ViewCustomAttributes = ""

			' LinkTypeCD
			If ew_NotEmpty(LinkTypeCD.CurrentValue) Then
				sFilterWrk = "[LinkTypeCD] = '" & ew_AdjustSql(LinkTypeCD.CurrentValue) & "'"
			sSqlWrk = "SELECT [LinkTypeDesc] FROM [LinkType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [LinkTypeDesc] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					LinkTypeCD.ViewValue = RsWrk("LinkTypeDesc")
				Else
					LinkTypeCD.ViewValue = LinkTypeCD.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				LinkTypeCD.ViewValue = System.DBNull.Value
			End If
			LinkTypeCD.CssStyle = ""
			LinkTypeCD.CssClass = ""
			LinkTypeCD.ViewCustomAttributes = ""

			' CategoryID
			If ew_NotEmpty(CategoryID.CurrentValue) Then
				sFilterWrk = "[ID] = " & ew_AdjustSql(CategoryID.CurrentValue) & ""
			sSqlWrk = "SELECT [Title] FROM [LinkCategory]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [Title] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					CategoryID.ViewValue = RsWrk("Title")
				Else
					CategoryID.ViewValue = CategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				CategoryID.ViewValue = System.DBNull.Value
			End If
			CategoryID.CssStyle = ""
			CategoryID.CssClass = ""
			CategoryID.ViewCustomAttributes = ""

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

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteCategoryGroupID.CurrentValue) Then
				sFilterWrk = "[SiteCategoryGroupID] = " & ew_AdjustSql(SiteCategoryGroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [SiteCategoryGroupNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryGroupID.ViewValue = RsWrk("SiteCategoryGroupNM")
				Else
					SiteCategoryGroupID.ViewValue = SiteCategoryGroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryGroupID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryGroupID.CssStyle = ""
			SiteCategoryGroupID.CssClass = ""
			SiteCategoryGroupID.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(zPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(zPageID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageName] FROM [Page]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					zPageID.ViewValue = RsWrk("PageName")
				Else
					zPageID.ViewValue = zPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				zPageID.ViewValue = System.DBNull.Value
			End If
			zPageID.CssStyle = ""
			zPageID.CssClass = ""
			zPageID.ViewCustomAttributes = ""

			' Views
			If Convert.ToString(Views.CurrentValue) = "1" Then
				Views.ViewValue = "Yes"
			Else
				Views.ViewValue = "No"
			End If
			Views.CssStyle = ""
			Views.CssClass = ""
			Views.ViewCustomAttributes = ""

			' Ranks
			Ranks.ViewValue = Ranks.CurrentValue
			Ranks.CssStyle = ""
			Ranks.CssClass = ""
			Ranks.ViewCustomAttributes = ""

			' UserID
			If ew_NotEmpty(UserID.CurrentValue) Then
				sFilterWrk = "[ContactID] = " & ew_AdjustSql(UserID.CurrentValue) & ""
			sSqlWrk = "SELECT [PrimaryContact] FROM [Contact]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PrimaryContact] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					UserID.ViewValue = RsWrk("PrimaryContact")
				Else
					UserID.ViewValue = UserID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				UserID.ViewValue = System.DBNull.Value
			End If
			UserID.CssStyle = ""
			UserID.CssClass = ""
			UserID.ViewCustomAttributes = ""

			' ASIN
			ASIN.ViewValue = ASIN.CurrentValue
			ASIN.CssStyle = ""
			ASIN.CssClass = ""
			ASIN.ViewCustomAttributes = ""

			' DateAdd
			DateAdd.ViewValue = DateAdd.CurrentValue
			DateAdd.CssStyle = ""
			DateAdd.CssClass = ""
			DateAdd.ViewCustomAttributes = ""

		'
		'  Render for View Refer
		'
			' Title

			Title.HrefValue = ""
			Title.TooltipValue = ""

			' LinkTypeCD
			LinkTypeCD.HrefValue = ""
			LinkTypeCD.TooltipValue = ""

			' CategoryID
			CategoryID.HrefValue = ""
			CategoryID.TooltipValue = ""

			' CompanyID
			CompanyID.HrefValue = ""
			CompanyID.TooltipValue = ""

			' SiteCategoryGroupID
			SiteCategoryGroupID.HrefValue = ""
			SiteCategoryGroupID.TooltipValue = ""

			' PageID
			zPageID.HrefValue = ""
			zPageID.TooltipValue = ""

			' Views
			Views.HrefValue = ""
			Views.TooltipValue = ""

			' Ranks
			Ranks.HrefValue = ""
			Ranks.TooltipValue = ""

			' UserID
			UserID.HrefValue = ""
			UserID.TooltipValue = ""

			' ASIN
			ASIN.HrefValue = ""
			ASIN.TooltipValue = ""

			' DateAdd
			DateAdd.HrefValue = ""
			DateAdd.TooltipValue = ""

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
			If Name = "ID" Then Return ID
			If Name = "Title" Then Return Title
			If Name = "LinkTypeCD" Then Return LinkTypeCD
			If Name = "CategoryID" Then Return CategoryID
			If Name = "CompanyID" Then Return CompanyID
			If Name = "SiteCategoryGroupID" Then Return SiteCategoryGroupID
			If Name = "PageID" Then Return zPageID
			If Name = "Views" Then Return Views
			If Name = "Description" Then Return Description
			If Name = "URL" Then Return URL
			If Name = "Ranks" Then Return Ranks
			If Name = "UserID" Then Return UserID
			If Name = "ASIN" Then Return ASIN
			If Name = "DateAdd" Then Return DateAdd
			If Name = "UserName" Then Return UserName
			Return Nothing		
		End Function

		' ID
		Private m_ID As cField

		Public ReadOnly Property ID() As cField
			Get
				If m_ID Is Nothing Then
					m_ID = New cField(m_Page, "Link", "Link", "x_ID", "ID", "[ID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_ID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_ID
			End Get
		End Property

		' Title
		Private m_Title As cField

		Public ReadOnly Property Title() As cField
			Get
				If m_Title Is Nothing Then
					m_Title = New cField(m_Page, "Link", "Link", "x_Title", "Title", "[Title]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Title
			End Get
		End Property

		' LinkTypeCD
		Private m_LinkTypeCD As cField

		Public ReadOnly Property LinkTypeCD() As cField
			Get
				If m_LinkTypeCD Is Nothing Then
					m_LinkTypeCD = New cField(m_Page, "Link", "Link", "x_LinkTypeCD", "LinkTypeCD", "[LinkTypeCD]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_LinkTypeCD
			End Get
		End Property

		' CategoryID
		Private m_CategoryID As cField

		Public ReadOnly Property CategoryID() As cField
			Get
				If m_CategoryID Is Nothing Then
					m_CategoryID = New cField(m_Page, "Link", "Link", "x_CategoryID", "CategoryID", "[CategoryID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_CategoryID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_CategoryID
			End Get
		End Property

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then
					m_CompanyID = New cField(m_Page, "Link", "Link", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_CompanyID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_CompanyID
			End Get
		End Property

		' SiteCategoryGroupID
		Private m_SiteCategoryGroupID As cField

		Public ReadOnly Property SiteCategoryGroupID() As cField
			Get
				If m_SiteCategoryGroupID Is Nothing Then
					m_SiteCategoryGroupID = New cField(m_Page, "Link", "Link", "x_SiteCategoryGroupID", "SiteCategoryGroupID", "[SiteCategoryGroupID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_SiteCategoryGroupID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_SiteCategoryGroupID
			End Get
		End Property

		' PageID
		Private m_zPageID As cField

		Public ReadOnly Property zPageID() As cField
			Get
				If m_zPageID Is Nothing Then
					m_zPageID = New cField(m_Page, "Link", "Link", "x_zPageID", "PageID", "[PageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_zPageID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_zPageID
			End Get
		End Property

		' Views
		Private m_Views As cField

		Public ReadOnly Property Views() As cField
			Get
				If m_Views Is Nothing Then
					m_Views = New cField(m_Page, "Link", "Link", "x_Views", "Views", "[Views]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				End If
				Return m_Views
			End Get
		End Property

		' Description
		Private m_Description As cField

		Public ReadOnly Property Description() As cField
			Get
				If m_Description Is Nothing Then
					m_Description = New cField(m_Page, "Link", "Link", "x_Description", "Description", "[Description]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				End If
				Return m_Description
			End Get
		End Property

		' URL
		Private m_URL As cField

		Public ReadOnly Property URL() As cField
			Get
				If m_URL Is Nothing Then
					m_URL = New cField(m_Page, "Link", "Link", "x_URL", "URL", "[URL]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				End If
				Return m_URL
			End Get
		End Property

		' Ranks
		Private m_Ranks As cField

		Public ReadOnly Property Ranks() As cField
			Get
				If m_Ranks Is Nothing Then
					m_Ranks = New cField(m_Page, "Link", "Link", "x_Ranks", "Ranks", "[Ranks]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_Ranks.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_Ranks
			End Get
		End Property

		' UserID
		Private m_UserID As cField

		Public ReadOnly Property UserID() As cField
			Get
				If m_UserID Is Nothing Then
					m_UserID = New cField(m_Page, "Link", "Link", "x_UserID", "UserID", "[UserID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_UserID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_UserID
			End Get
		End Property

		' ASIN
		Private m_ASIN As cField

		Public ReadOnly Property ASIN() As cField
			Get
				If m_ASIN Is Nothing Then
					m_ASIN = New cField(m_Page, "Link", "Link", "x_ASIN", "ASIN", "[ASIN]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_ASIN
			End Get
		End Property

		' DateAdd
		Private m_DateAdd As cField

		Public ReadOnly Property DateAdd() As cField
			Get
				If m_DateAdd Is Nothing Then
					m_DateAdd = New cField(m_Page, "Link", "Link", "x_DateAdd", "DateAdd", "[DateAdd]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  0)
				End If
				Return m_DateAdd
			End Get
		End Property

		' UserName
		Private m_UserName As cField

		Public ReadOnly Property UserName() As cField
			Get
				If m_UserName Is Nothing Then
					m_UserName = New cField(m_Page, "Link", "Link", "x_UserName", "UserName", "[UserName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_UserName
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
			If m_ID IsNot Nothing Then m_ID.Dispose()
			If m_Title IsNot Nothing Then m_Title.Dispose()
			If m_LinkTypeCD IsNot Nothing Then m_LinkTypeCD.Dispose()
			If m_CategoryID IsNot Nothing Then m_CategoryID.Dispose()
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_SiteCategoryGroupID IsNot Nothing Then m_SiteCategoryGroupID.Dispose()
			If m_zPageID IsNot Nothing Then m_zPageID.Dispose()
			If m_Views IsNot Nothing Then m_Views.Dispose()
			If m_Description IsNot Nothing Then m_Description.Dispose()
			If m_URL IsNot Nothing Then m_URL.Dispose()
			If m_Ranks IsNot Nothing Then m_Ranks.Dispose()
			If m_UserID IsNot Nothing Then m_UserID.Dispose()
			If m_ASIN IsNot Nothing Then m_ASIN.Dispose()
			If m_DateAdd IsNot Nothing Then m_DateAdd.Dispose()
			If m_UserName IsNot Nothing Then m_UserName.Dispose()
		End Sub
	End Class
End Class
