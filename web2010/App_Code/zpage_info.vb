Imports System.Data
Imports System.Data.Common
Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 Project Class (Table)
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

	Public zPage As czPage

	' Define table class
	Class czPage
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
				Return "zPage"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Page"
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
				Return "zPage_Highlight"
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

		' Table level SQL
		Public ReadOnly Property SqlSelect() As String
			Get ' Select
				Return "SELECT * FROM [Page]"
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
			Sql = "INSERT INTO [Page] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [Page] SET " & values
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
			Sql = "DELETE FROM [Page] WHERE "
			fld = FieldByName("PageID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("PageID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[PageID] = @zPageID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(zPageID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@zPageID@", ew_AdjustSql(zPageID.CurrentValue)) ' Replace key value
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
					Return "zpage_list.aspx"
				End If
			End Get
		End Property

		' List url
		Public Function ListUrl() As String
			Return "zpage_list.aspx"
		End Function

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("zpage_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "zpage_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("zpage_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("zpage_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("zpage_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(zPageID.CurrentValue) Then
				sUrl = sUrl & "zPageID=" & zPageID.CurrentValue
			Else
				Return "javascript:alert(ewLanguage.Phrase(""InvalidRecord""));"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=zPage"
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
			zPageID.DbValue = RsRow("PageID")
			ParentPageID.DbValue = RsRow("ParentPageID")
			zPageName.DbValue = RsRow("PageName")
			PageTitle.DbValue = RsRow("PageTitle")
			PageTypeID.DbValue = RsRow("PageTypeID")
			GroupID.DbValue = RsRow("GroupID")
			Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
			PageOrder.DbValue = RsRow("PageOrder")
			CompanyID.DbValue = RsRow("CompanyID")
			PageDescription.DbValue = RsRow("PageDescription")
			PageKeywords.DbValue = RsRow("PageKeywords")
			ImagesPerRow.DbValue = RsRow("ImagesPerRow")
			RowsPerPage.DbValue = RsRow("RowsPerPage")
			PageFileName.DbValue = RsRow("PageFileName")
			AllowMessage.DbValue = IIf(ew_ConvertToBool(RsRow("AllowMessage")), "1", "0")
			SiteCategoryID.DbValue = RsRow("SiteCategoryID")
			SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
			VersionNo.DbValue = RsRow("VersionNo")
			ModifiedDT.DbValue = RsRow("ModifiedDT")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

		'
		'  Common render codes
		'
			' ParentPageID

			ParentPageID.CellCssStyle = "white-space: nowrap;"
			ParentPageID.CellCssClass = ""
			ParentPageID.CellAttrs.Clear(): ParentPageID.ViewAttrs.Clear(): ParentPageID.EditAttrs.Clear()

			' PageName
			zPageName.CellCssStyle = "white-space: nowrap;"
			zPageName.CellCssClass = ""
			zPageName.CellAttrs.Clear(): zPageName.ViewAttrs.Clear(): zPageName.EditAttrs.Clear()

			' PageTitle
			PageTitle.CellCssStyle = "white-space: nowrap;"
			PageTitle.CellCssClass = ""
			PageTitle.CellAttrs.Clear(): PageTitle.ViewAttrs.Clear(): PageTitle.EditAttrs.Clear()

			' PageTypeID
			PageTypeID.CellCssStyle = "white-space: nowrap;"
			PageTypeID.CellCssClass = ""
			PageTypeID.CellAttrs.Clear(): PageTypeID.ViewAttrs.Clear(): PageTypeID.EditAttrs.Clear()

			' GroupID
			GroupID.CellCssStyle = "white-space: nowrap;"
			GroupID.CellCssClass = ""
			GroupID.CellAttrs.Clear(): GroupID.ViewAttrs.Clear(): GroupID.EditAttrs.Clear()

			' Active
			Active.CellCssStyle = "white-space: nowrap;"
			Active.CellCssClass = ""
			Active.CellAttrs.Clear(): Active.ViewAttrs.Clear(): Active.EditAttrs.Clear()

			' PageOrder
			PageOrder.CellCssStyle = "white-space: nowrap;"
			PageOrder.CellCssClass = ""
			PageOrder.CellAttrs.Clear(): PageOrder.ViewAttrs.Clear(): PageOrder.EditAttrs.Clear()

			' Row Rendering event
			Row_Rendering()

		'
		'  Render for View
		'
			' ParentPageID

			If ew_NotEmpty(ParentPageID.CurrentValue) Then
				sFilterWrk = "[PageID] = " & ew_AdjustSql(ParentPageID.CurrentValue) & ""
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
					ParentPageID.ViewValue = RsWrk("PageName")
				Else
					ParentPageID.ViewValue = ParentPageID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				ParentPageID.ViewValue = System.DBNull.Value
			End If
			ParentPageID.CssStyle = ""
			ParentPageID.CssClass = ""
			ParentPageID.ViewCustomAttributes = ""

			' PageName
			zPageName.ViewValue = zPageName.CurrentValue
			zPageName.CssStyle = ""
			zPageName.CssClass = ""
			zPageName.ViewCustomAttributes = ""

			' PageTitle
			PageTitle.ViewValue = PageTitle.CurrentValue
			PageTitle.CssStyle = ""
			PageTitle.CssClass = ""
			PageTitle.ViewCustomAttributes = ""

			' PageTypeID
			If ew_NotEmpty(PageTypeID.CurrentValue) Then
				sFilterWrk = "[PageTypeID] = " & ew_AdjustSql(PageTypeID.CurrentValue) & ""
			sSqlWrk = "SELECT [PageTypeCD] FROM [PageType]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [PageTypeCD] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					PageTypeID.ViewValue = RsWrk("PageTypeCD")
				Else
					PageTypeID.ViewValue = PageTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				PageTypeID.ViewValue = System.DBNull.Value
			End If
			PageTypeID.CssStyle = ""
			PageTypeID.CssClass = ""
			PageTypeID.ViewCustomAttributes = ""

			' GroupID
			If ew_NotEmpty(GroupID.CurrentValue) Then
				sFilterWrk = "[GroupID] = " & ew_AdjustSql(GroupID.CurrentValue) & ""
			sSqlWrk = "SELECT [GroupName] FROM [Group]"
			sWhereWrk = ""
			If sFilterWrk <> "" Then
				If sWhereWrk <> "" Then sWhereWrk &= " AND "
				sWhereWrk &= "(" & sFilterWrk & ")"
			End If
			If sWhereWrk <> "" Then sSqlWrk &= " WHERE " & sWhereWrk
			sSqlWrk &= " ORDER BY [GroupName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					GroupID.ViewValue = RsWrk("GroupName")
				Else
					GroupID.ViewValue = GroupID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				GroupID.ViewValue = System.DBNull.Value
			End If
			GroupID.CssStyle = ""
			GroupID.CssClass = ""
			GroupID.ViewCustomAttributes = ""

			' Active
			If Convert.ToString(Active.CurrentValue) = "1" Then
				Active.ViewValue = "Yes"
			Else
				Active.ViewValue = "No"
			End If
			Active.CssStyle = ""
			Active.CssClass = ""
			Active.ViewCustomAttributes = ""

			' PageOrder
			PageOrder.ViewValue = PageOrder.CurrentValue
			PageOrder.CssStyle = ""
			PageOrder.CssClass = ""
			PageOrder.ViewCustomAttributes = ""

		'
		'  Render for View Refer
		'
			' ParentPageID

			ParentPageID.HrefValue = ""
			ParentPageID.TooltipValue = ""

			' PageName
			zPageName.HrefValue = ""
			zPageName.TooltipValue = ""

			' PageTitle
			PageTitle.HrefValue = ""
			PageTitle.TooltipValue = ""

			' PageTypeID
			PageTypeID.HrefValue = ""
			PageTypeID.TooltipValue = ""

			' GroupID
			GroupID.HrefValue = ""
			GroupID.TooltipValue = ""

			' Active
			Active.HrefValue = ""
			Active.TooltipValue = ""

			' PageOrder
			PageOrder.HrefValue = ""
			PageOrder.TooltipValue = ""

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
			If Name = "PageID" Then Return zPageID
			If Name = "ParentPageID" Then Return ParentPageID
			If Name = "PageName" Then Return zPageName
			If Name = "PageTitle" Then Return PageTitle
			If Name = "PageTypeID" Then Return PageTypeID
			If Name = "GroupID" Then Return GroupID
			If Name = "Active" Then Return Active
			If Name = "PageOrder" Then Return PageOrder
			If Name = "CompanyID" Then Return CompanyID
			If Name = "PageDescription" Then Return PageDescription
			If Name = "PageKeywords" Then Return PageKeywords
			If Name = "ImagesPerRow" Then Return ImagesPerRow
			If Name = "RowsPerPage" Then Return RowsPerPage
			If Name = "PageFileName" Then Return PageFileName
			If Name = "AllowMessage" Then Return AllowMessage
			If Name = "SiteCategoryID" Then Return SiteCategoryID
			If Name = "SiteCategoryGroupID" Then Return SiteCategoryGroupID
			If Name = "VersionNo" Then Return VersionNo
			If Name = "ModifiedDT" Then Return ModifiedDT
			Return Nothing		
		End Function

		' PageID
		Private m_zPageID As cField

		Public ReadOnly Property zPageID() As cField
			Get
				If m_zPageID Is Nothing Then
					m_zPageID = New cField(m_Page, "zPage", "Page", "x_zPageID", "PageID", "[PageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_zPageID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_zPageID
			End Get
		End Property

		' ParentPageID
		Private m_ParentPageID As cField

		Public ReadOnly Property ParentPageID() As cField
			Get
				If m_ParentPageID Is Nothing Then
					m_ParentPageID = New cField(m_Page, "zPage", "Page", "x_ParentPageID", "ParentPageID", "[ParentPageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_ParentPageID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_ParentPageID
			End Get
		End Property

		' PageName
		Private m_zPageName As cField

		Public ReadOnly Property zPageName() As cField
			Get
				If m_zPageName Is Nothing Then
					m_zPageName = New cField(m_Page, "zPage", "Page", "x_zPageName", "PageName", "[PageName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_zPageName
			End Get
		End Property

		' PageTitle
		Private m_PageTitle As cField

		Public ReadOnly Property PageTitle() As cField
			Get
				If m_PageTitle Is Nothing Then
					m_PageTitle = New cField(m_Page, "zPage", "Page", "x_PageTitle", "PageTitle", "[PageTitle]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PageTitle
			End Get
		End Property

		' PageTypeID
		Private m_PageTypeID As cField

		Public ReadOnly Property PageTypeID() As cField
			Get
				If m_PageTypeID Is Nothing Then
					m_PageTypeID = New cField(m_Page, "zPage", "Page", "x_PageTypeID", "PageTypeID", "[PageTypeID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_PageTypeID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_PageTypeID
			End Get
		End Property

		' GroupID
		Private m_GroupID As cField

		Public ReadOnly Property GroupID() As cField
			Get
				If m_GroupID Is Nothing Then
					m_GroupID = New cField(m_Page, "zPage", "Page", "x_GroupID", "GroupID", "[GroupID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_GroupID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_GroupID
			End Get
		End Property

		' Active
		Private m_Active As cField

		Public ReadOnly Property Active() As cField
			Get
				If m_Active Is Nothing Then
					m_Active = New cField(m_Page, "zPage", "Page", "x_Active", "Active", "[Active]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				End If
				Return m_Active
			End Get
		End Property

		' PageOrder
		Private m_PageOrder As cField

		Public ReadOnly Property PageOrder() As cField
			Get
				If m_PageOrder Is Nothing Then
					m_PageOrder = New cField(m_Page, "zPage", "Page", "x_PageOrder", "PageOrder", "[PageOrder]", 2, OleDbType.SmallInt, EW_DATATYPE_NUMBER,  0)
					m_PageOrder.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_PageOrder
			End Get
		End Property

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then
					m_CompanyID = New cField(m_Page, "zPage", "Page", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_CompanyID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_CompanyID
			End Get
		End Property

		' PageDescription
		Private m_PageDescription As cField

		Public ReadOnly Property PageDescription() As cField
			Get
				If m_PageDescription Is Nothing Then
					m_PageDescription = New cField(m_Page, "zPage", "Page", "x_PageDescription", "PageDescription", "[PageDescription]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PageDescription
			End Get
		End Property

		' PageKeywords
		Private m_PageKeywords As cField

		Public ReadOnly Property PageKeywords() As cField
			Get
				If m_PageKeywords Is Nothing Then
					m_PageKeywords = New cField(m_Page, "zPage", "Page", "x_PageKeywords", "PageKeywords", "[PageKeywords]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PageKeywords
			End Get
		End Property

		' ImagesPerRow
		Private m_ImagesPerRow As cField

		Public ReadOnly Property ImagesPerRow() As cField
			Get
				If m_ImagesPerRow Is Nothing Then
					m_ImagesPerRow = New cField(m_Page, "zPage", "Page", "x_ImagesPerRow", "ImagesPerRow", "[ImagesPerRow]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_ImagesPerRow.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_ImagesPerRow
			End Get
		End Property

		' RowsPerPage
		Private m_RowsPerPage As cField

		Public ReadOnly Property RowsPerPage() As cField
			Get
				If m_RowsPerPage Is Nothing Then
					m_RowsPerPage = New cField(m_Page, "zPage", "Page", "x_RowsPerPage", "RowsPerPage", "[RowsPerPage]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_RowsPerPage.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_RowsPerPage
			End Get
		End Property

		' PageFileName
		Private m_PageFileName As cField

		Public ReadOnly Property PageFileName() As cField
			Get
				If m_PageFileName Is Nothing Then
					m_PageFileName = New cField(m_Page, "zPage", "Page", "x_PageFileName", "PageFileName", "[PageFileName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_PageFileName
			End Get
		End Property

		' AllowMessage
		Private m_AllowMessage As cField

		Public ReadOnly Property AllowMessage() As cField
			Get
				If m_AllowMessage Is Nothing Then
					m_AllowMessage = New cField(m_Page, "zPage", "Page", "x_AllowMessage", "AllowMessage", "[AllowMessage]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				End If
				Return m_AllowMessage
			End Get
		End Property

		' SiteCategoryID
		Private m_SiteCategoryID As cField

		Public ReadOnly Property SiteCategoryID() As cField
			Get
				If m_SiteCategoryID Is Nothing Then
					m_SiteCategoryID = New cField(m_Page, "zPage", "Page", "x_SiteCategoryID", "SiteCategoryID", "[SiteCategoryID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_SiteCategoryID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_SiteCategoryID
			End Get
		End Property

		' SiteCategoryGroupID
		Private m_SiteCategoryGroupID As cField

		Public ReadOnly Property SiteCategoryGroupID() As cField
			Get
				If m_SiteCategoryGroupID Is Nothing Then
					m_SiteCategoryGroupID = New cField(m_Page, "zPage", "Page", "x_SiteCategoryGroupID", "SiteCategoryGroupID", "[SiteCategoryGroupID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_SiteCategoryGroupID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_SiteCategoryGroupID
			End Get
		End Property

		' VersionNo
		Private m_VersionNo As cField

		Public ReadOnly Property VersionNo() As cField
			Get
				If m_VersionNo Is Nothing Then
					m_VersionNo = New cField(m_Page, "zPage", "Page", "x_VersionNo", "VersionNo", "[VersionNo]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_VersionNo.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_VersionNo
			End Get
		End Property

		' ModifiedDT
		Private m_ModifiedDT As cField

		Public ReadOnly Property ModifiedDT() As cField
			Get
				If m_ModifiedDT Is Nothing Then
					m_ModifiedDT = New cField(m_Page, "zPage", "Page", "x_ModifiedDT", "ModifiedDT", "[ModifiedDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  0)
				End If
				Return m_ModifiedDT
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
			If m_zPageID IsNot Nothing Then m_zPageID.Dispose()
			If m_ParentPageID IsNot Nothing Then m_ParentPageID.Dispose()
			If m_zPageName IsNot Nothing Then m_zPageName.Dispose()
			If m_PageTitle IsNot Nothing Then m_PageTitle.Dispose()
			If m_PageTypeID IsNot Nothing Then m_PageTypeID.Dispose()
			If m_GroupID IsNot Nothing Then m_GroupID.Dispose()
			If m_Active IsNot Nothing Then m_Active.Dispose()
			If m_PageOrder IsNot Nothing Then m_PageOrder.Dispose()
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_PageDescription IsNot Nothing Then m_PageDescription.Dispose()
			If m_PageKeywords IsNot Nothing Then m_PageKeywords.Dispose()
			If m_ImagesPerRow IsNot Nothing Then m_ImagesPerRow.Dispose()
			If m_RowsPerPage IsNot Nothing Then m_RowsPerPage.Dispose()
			If m_PageFileName IsNot Nothing Then m_PageFileName.Dispose()
			If m_AllowMessage IsNot Nothing Then m_AllowMessage.Dispose()
			If m_SiteCategoryID IsNot Nothing Then m_SiteCategoryID.Dispose()
			If m_SiteCategoryGroupID IsNot Nothing Then m_SiteCategoryGroupID.Dispose()
			If m_VersionNo IsNot Nothing Then m_VersionNo.Dispose()
			If m_ModifiedDT IsNot Nothing Then m_ModifiedDT.Dispose()
		End Sub
	End Class
End Class
