Imports System.Data
Imports System.Data.Common
Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 Project Class (Table)
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

	Public LinkRank As cLinkRank

	' Define table class
	Class cLinkRank
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
				Return "LinkRank"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "LinkRank"
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
				Return "LinkRank_Highlight"
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
				Return "SELECT * FROM [LinkRank]"
			End Get
		End Property

		Public ReadOnly Property SqlWhere() As String
			Get ' Where
				Dim sWhere As String = ""
        TableFilter = ""
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
			Sql = "INSERT INTO [LinkRank] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [LinkRank] SET " & values
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
			Sql = "DELETE FROM [LinkRank] WHERE "
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
					Return "linkrank_list.aspx"
				End If
			End Get
		End Property

		' List url
		Public Function ListUrl() As String
			Return "linkrank_list.aspx"
		End Function

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("linkrank_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "linkrank_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("linkrank_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("linkrank_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("linkrank_delete.aspx", UrlParm(""))
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
				OutStr = "t=LinkRank"
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
			LinkID.DbValue = RsRow("LinkID")
			UserID.DbValue = RsRow("UserID")
			RankNum.DbValue = RsRow("RankNum")
			CateID.DbValue = RsRow("CateID")
			Comment.DbValue = RsRow("Comment")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

		'
		'  Common render codes
		'
			' ID

			ID.CellCssStyle = ""
			ID.CellCssClass = ""
			ID.CellAttrs.Clear(): ID.ViewAttrs.Clear(): ID.EditAttrs.Clear()

			' LinkID
			LinkID.CellCssStyle = ""
			LinkID.CellCssClass = ""
			LinkID.CellAttrs.Clear(): LinkID.ViewAttrs.Clear(): LinkID.EditAttrs.Clear()

			' UserID
			UserID.CellCssStyle = ""
			UserID.CellCssClass = ""
			UserID.CellAttrs.Clear(): UserID.ViewAttrs.Clear(): UserID.EditAttrs.Clear()

			' RankNum
			RankNum.CellCssStyle = ""
			RankNum.CellCssClass = ""
			RankNum.CellAttrs.Clear(): RankNum.ViewAttrs.Clear(): RankNum.EditAttrs.Clear()

			' CateID
			CateID.CellCssStyle = ""
			CateID.CellCssClass = ""
			CateID.CellAttrs.Clear(): CateID.ViewAttrs.Clear(): CateID.EditAttrs.Clear()

			' Comment
			Comment.CellCssStyle = ""
			Comment.CellCssClass = ""
			Comment.CellAttrs.Clear(): Comment.ViewAttrs.Clear(): Comment.EditAttrs.Clear()

			' Row Rendering event
			Row_Rendering()

		'
		'  Render for View
		'
			' ID

			ID.ViewValue = ID.CurrentValue
			ID.CssStyle = ""
			ID.CssClass = ""
			ID.ViewCustomAttributes = ""

			' LinkID
			LinkID.ViewValue = LinkID.CurrentValue
			LinkID.CssStyle = ""
			LinkID.CssClass = ""
			LinkID.ViewCustomAttributes = ""

			' UserID
			UserID.ViewValue = UserID.CurrentValue
			UserID.CssStyle = ""
			UserID.CssClass = ""
			UserID.ViewCustomAttributes = ""

			' RankNum
			RankNum.ViewValue = RankNum.CurrentValue
			RankNum.CssStyle = ""
			RankNum.CssClass = ""
			RankNum.ViewCustomAttributes = ""

			' CateID
			CateID.ViewValue = CateID.CurrentValue
			CateID.CssStyle = ""
			CateID.CssClass = ""
			CateID.ViewCustomAttributes = ""

			' Comment
			Comment.ViewValue = Comment.CurrentValue
			Comment.CssStyle = ""
			Comment.CssClass = ""
			Comment.ViewCustomAttributes = ""

		'
		'  Render for View Refer
		'
			' ID

			ID.HrefValue = ""
			ID.TooltipValue = ""

			' LinkID
			LinkID.HrefValue = ""
			LinkID.TooltipValue = ""

			' UserID
			UserID.HrefValue = ""
			UserID.TooltipValue = ""

			' RankNum
			RankNum.HrefValue = ""
			RankNum.TooltipValue = ""

			' CateID
			CateID.HrefValue = ""
			CateID.TooltipValue = ""

			' Comment
			Comment.HrefValue = ""
			Comment.TooltipValue = ""

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
			If Name = "LinkID" Then Return LinkID
			If Name = "UserID" Then Return UserID
			If Name = "RankNum" Then Return RankNum
			If Name = "CateID" Then Return CateID
			If Name = "Comment" Then Return Comment
			Return Nothing		
		End Function

		' ID
		Private m_ID As cField

		Public ReadOnly Property ID() As cField
			Get
				If m_ID Is Nothing Then
					m_ID = New cField(m_Page, "LinkRank", "LinkRank", "x_ID", "ID", "[ID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_ID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_ID
			End Get
		End Property

		' LinkID
		Private m_LinkID As cField

		Public ReadOnly Property LinkID() As cField
			Get
				If m_LinkID Is Nothing Then
					m_LinkID = New cField(m_Page, "LinkRank", "LinkRank", "x_LinkID", "LinkID", "[LinkID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_LinkID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_LinkID
			End Get
		End Property

		' UserID
		Private m_UserID As cField

		Public ReadOnly Property UserID() As cField
			Get
				If m_UserID Is Nothing Then
					m_UserID = New cField(m_Page, "LinkRank", "LinkRank", "x_UserID", "UserID", "[UserID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_UserID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_UserID
			End Get
		End Property

		' RankNum
		Private m_RankNum As cField

		Public ReadOnly Property RankNum() As cField
			Get
				If m_RankNum Is Nothing Then
					m_RankNum = New cField(m_Page, "LinkRank", "LinkRank", "x_RankNum", "RankNum", "[RankNum]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_RankNum.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_RankNum
			End Get
		End Property

		' CateID
		Private m_CateID As cField

		Public ReadOnly Property CateID() As cField
			Get
				If m_CateID Is Nothing Then
					m_CateID = New cField(m_Page, "LinkRank", "LinkRank", "x_CateID", "CateID", "[CateID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
					m_CateID.FldDefaultErrMsg = Language.Phrase("IncorrectInteger")
				End If
				Return m_CateID
			End Get
		End Property

		' Comment
		Private m_Comment As cField

		Public ReadOnly Property Comment() As cField
			Get
				If m_Comment Is Nothing Then
					m_Comment = New cField(m_Page, "LinkRank", "LinkRank", "x_Comment", "Comment", "[Comment]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				End If
				Return m_Comment
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
			If m_LinkID IsNot Nothing Then m_LinkID.Dispose()
			If m_UserID IsNot Nothing Then m_UserID.Dispose()
			If m_RankNum IsNot Nothing Then m_RankNum.Dispose()
			If m_CateID IsNot Nothing Then m_CateID.Dispose()
			If m_Comment IsNot Nothing Then m_Comment.Dispose()
		End Sub
	End Class
End Class
