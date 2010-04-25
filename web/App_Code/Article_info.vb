Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Table)
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	Public Article As cArticle

	' Define table class
	Class cArticle
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
				Return "Article"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Article"
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
				Return "Article_Highlight"
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
				Return "SELECT * FROM [Article]"
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
				Return "[Title] ASC"
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
			Sql = "INSERT INTO [Article] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [Article] SET " & values
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
			Sql = "DELETE FROM [Article] WHERE "
			fld = FieldByName("ArticleID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("ArticleID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[ArticleID] = @ArticleID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(ArticleID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@ArticleID@", ew_AdjustSql(ArticleID.CurrentValue)) ' Replace key value
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
					Return "Article_list.aspx"
				End If
			End Get
		End Property

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("Article_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "Article_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("Article_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("Article_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("Article_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(ArticleID.CurrentValue) Then
				sUrl = sUrl & "ArticleID=" & ArticleID.CurrentValue
			Else
				Return "javascript:alert('Invalid Record! Key is null');"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=Article"
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
			ArticleID.DbValue = RsRow("ArticleID")
			Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
			StartDT.DbValue = RsRow("StartDT")
			Title.DbValue = RsRow("Title")
			CompanyID.DbValue = RsRow("CompanyID")
			zPageID.DbValue = RsRow("PageID")
			ContactID.DbValue = RsRow("ContactID")
			Description.DbValue = RsRow("Description")
			ArticleBody.DbValue = RsRow("ArticleBody")
			ModifiedDT.DbValue = RsRow("ModifiedDT")
			Counter.DbValue = RsRow("Counter")
			VersionNo.DbValue = RsRow("VersionNo")
			userID.DbValue = RsRow("userID")
			EndDT.DbValue = RsRow("EndDT")
			ExpireDT.DbValue = RsRow("ExpireDT")
			Author.DbValue = RsRow("Author")
			ArticleSummary.DbValue = RsRow("ArticleSummary")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

			' Row Rendering event
			Row_Rendering()

			' Active
			If Convert.ToString(Active.CurrentValue) = "1" Then
				Active.ViewValue = "Yes"
			Else
				Active.ViewValue = "No"
			End If
			Active.CssStyle = ""
			Active.CssClass = ""
			Active.ViewCustomAttributes = ""

			' StartDT
			StartDT.ViewValue = StartDT.CurrentValue
			StartDT.ViewValue = ew_FormatDateTime(StartDT.ViewValue, 6)
			StartDT.CssStyle = ""
			StartDT.CssClass = ""
			StartDT.ViewCustomAttributes = ""

			' Title
			Title.ViewValue = Title.CurrentValue
			Title.CssStyle = ""
			Title.CssClass = ""
			Title.ViewCustomAttributes = ""

			' PageID
			If ew_NotEmpty(zPageID.CurrentValue) Then
				sSqlWrk = "SELECT [PageName] FROM [Page] WHERE [PageID] = " & ew_AdjustSql(zPageID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [PageName] Asc"
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

			' Active
			Active.HrefValue = ""

			' StartDT
			StartDT.HrefValue = ""

			' Title
			Title.HrefValue = ""

			' PageID
			zPageID.HrefValue = ""

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
			If Name = "ArticleID" Then Return ArticleID
			If Name = "Active" Then Return Active
			If Name = "StartDT" Then Return StartDT
			If Name = "Title" Then Return Title
			If Name = "CompanyID" Then Return CompanyID
			If Name = "PageID" Then Return zPageID
			If Name = "ContactID" Then Return ContactID
			If Name = "Description" Then Return Description
			If Name = "ArticleBody" Then Return ArticleBody
			If Name = "ModifiedDT" Then Return ModifiedDT
			If Name = "Counter" Then Return Counter
			If Name = "VersionNo" Then Return VersionNo
			If Name = "userID" Then Return userID
			If Name = "EndDT" Then Return EndDT
			If Name = "ExpireDT" Then Return ExpireDT
			If Name = "Author" Then Return Author
			If Name = "ArticleSummary" Then Return ArticleSummary
			Return Nothing		
		End Function

		' ArticleID
		Private m_ArticleID As cField

		Public ReadOnly Property ArticleID() As cField
			Get
				If m_ArticleID Is Nothing Then m_ArticleID = New cField("Article", "x_ArticleID", "ArticleID", "[ArticleID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_ArticleID
			End Get
		End Property

		' Active
		Private m_Active As cField

		Public ReadOnly Property Active() As cField
			Get
				If m_Active Is Nothing Then m_Active = New cField("Article", "x_Active", "Active", "[Active]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_Active
			End Get
		End Property

		' StartDT
		Private m_StartDT As cField

		Public ReadOnly Property StartDT() As cField
			Get
				If m_StartDT Is Nothing Then m_StartDT = New cField("Article", "x_StartDT", "StartDT", "[StartDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_StartDT
			End Get
		End Property

		' Title
		Private m_Title As cField

		Public ReadOnly Property Title() As cField
			Get
				If m_Title Is Nothing Then m_Title = New cField("Article", "x_Title", "Title", "[Title]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Title
			End Get
		End Property

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then m_CompanyID = New cField("Article", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_CompanyID
			End Get
		End Property

		' PageID
		Private m_zPageID As cField

		Public ReadOnly Property zPageID() As cField
			Get
				If m_zPageID Is Nothing Then m_zPageID = New cField("Article", "x_zPageID", "PageID", "[PageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_zPageID
			End Get
		End Property

		' ContactID
		Private m_ContactID As cField

		Public ReadOnly Property ContactID() As cField
			Get
				If m_ContactID Is Nothing Then m_ContactID = New cField("Article", "x_ContactID", "ContactID", "[ContactID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_ContactID
			End Get
		End Property

		' Description
		Private m_Description As cField

		Public ReadOnly Property Description() As cField
			Get
				If m_Description Is Nothing Then m_Description = New cField("Article", "x_Description", "Description", "[Description]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Description
			End Get
		End Property

		' ArticleBody
		Private m_ArticleBody As cField

		Public ReadOnly Property ArticleBody() As cField
			Get
				If m_ArticleBody Is Nothing Then m_ArticleBody = New cField("Article", "x_ArticleBody", "ArticleBody", "[ArticleBody]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_ArticleBody
			End Get
		End Property

		' ModifiedDT
		Private m_ModifiedDT As cField

		Public ReadOnly Property ModifiedDT() As cField
			Get
				If m_ModifiedDT Is Nothing Then m_ModifiedDT = New cField("Article", "x_ModifiedDT", "ModifiedDT", "[ModifiedDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_ModifiedDT
			End Get
		End Property

		' Counter
		Private m_Counter As cField

		Public ReadOnly Property Counter() As cField
			Get
				If m_Counter Is Nothing Then m_Counter = New cField("Article", "x_Counter", "Counter", "[Counter]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_Counter
			End Get
		End Property

		' VersionNo
		Private m_VersionNo As cField

		Public ReadOnly Property VersionNo() As cField
			Get
				If m_VersionNo Is Nothing Then m_VersionNo = New cField("Article", "x_VersionNo", "VersionNo", "[VersionNo]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_VersionNo
			End Get
		End Property

		' userID
		Private m_userID As cField

		Public ReadOnly Property userID() As cField
			Get
				If m_userID Is Nothing Then m_userID = New cField("Article", "x_userID", "userID", "[userID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_userID
			End Get
		End Property

		' EndDT
		Private m_EndDT As cField

		Public ReadOnly Property EndDT() As cField
			Get
				If m_EndDT Is Nothing Then m_EndDT = New cField("Article", "x_EndDT", "EndDT", "[EndDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_EndDT
			End Get
		End Property

		' ExpireDT
		Private m_ExpireDT As cField

		Public ReadOnly Property ExpireDT() As cField
			Get
				If m_ExpireDT Is Nothing Then m_ExpireDT = New cField("Article", "x_ExpireDT", "ExpireDT", "[ExpireDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_ExpireDT
			End Get
		End Property

		' Author
		Private m_Author As cField

		Public ReadOnly Property Author() As cField
			Get
				If m_Author Is Nothing Then m_Author = New cField("Article", "x_Author", "Author", "[Author]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Author
			End Get
		End Property

		' ArticleSummary
		Private m_ArticleSummary As cField

		Public ReadOnly Property ArticleSummary() As cField
			Get
				If m_ArticleSummary Is Nothing Then m_ArticleSummary = New cField("Article", "x_ArticleSummary", "ArticleSummary", "[ArticleSummary]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_ArticleSummary
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
			If m_ArticleID IsNot Nothing Then m_ArticleID.Dispose()
			If m_Active IsNot Nothing Then m_Active.Dispose()
			If m_StartDT IsNot Nothing Then m_StartDT.Dispose()
			If m_Title IsNot Nothing Then m_Title.Dispose()
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_zPageID IsNot Nothing Then m_zPageID.Dispose()
			If m_ContactID IsNot Nothing Then m_ContactID.Dispose()
			If m_Description IsNot Nothing Then m_Description.Dispose()
			If m_ArticleBody IsNot Nothing Then m_ArticleBody.Dispose()
			If m_ModifiedDT IsNot Nothing Then m_ModifiedDT.Dispose()
			If m_Counter IsNot Nothing Then m_Counter.Dispose()
			If m_VersionNo IsNot Nothing Then m_VersionNo.Dispose()
			If m_userID IsNot Nothing Then m_userID.Dispose()
			If m_EndDT IsNot Nothing Then m_EndDT.Dispose()
			If m_ExpireDT IsNot Nothing Then m_ExpireDT.Dispose()
			If m_Author IsNot Nothing Then m_Author.Dispose()
			If m_ArticleSummary IsNot Nothing Then m_ArticleSummary.Dispose()
		End Sub
	End Class
End Class
