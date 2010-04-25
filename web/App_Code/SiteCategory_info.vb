Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Table)
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	Public SiteCategory As cSiteCategory

	' Define table class
	Class cSiteCategory
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
				Return "SiteCategory"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "SiteCategory"
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
				Return "SiteCategory_Highlight"
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
				Return "SELECT * FROM [SiteCategory]"
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
			Sql = "INSERT INTO [SiteCategory] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [SiteCategory] SET " & values
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
			Sql = "DELETE FROM [SiteCategory] WHERE "
			fld = FieldByName("SiteCategoryID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("SiteCategoryID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[SiteCategoryID] = @SiteCategoryID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(SiteCategoryID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@SiteCategoryID@", ew_AdjustSql(SiteCategoryID.CurrentValue)) ' Replace key value
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
					Return "SiteCategory_list.aspx"
				End If
			End Get
		End Property

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("SiteCategory_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "SiteCategory_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("SiteCategory_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("SiteCategory_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("SiteCategory_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(SiteCategoryID.CurrentValue) Then
				sUrl = sUrl & "SiteCategoryID=" & SiteCategoryID.CurrentValue
			Else
				Return "javascript:alert('Invalid Record! Key is null');"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=SiteCategory"
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
			SiteCategoryID.DbValue = RsRow("SiteCategoryID")
			SiteCategoryTypeID.DbValue = RsRow("SiteCategoryTypeID")
			GroupOrder.DbValue = RsRow("GroupOrder")
			CategoryName.DbValue = RsRow("CategoryName")
			CategoryTitle.DbValue = RsRow("CategoryTitle")
			CategoryDescription.DbValue = RsRow("CategoryDescription")
			CategoryKeywords.DbValue = RsRow("CategoryKeywords")
			ParentCategoryID.DbValue = RsRow("ParentCategoryID")
			CategoryFileName.DbValue = RsRow("CategoryFileName")
			SiteCategoryGroupID.DbValue = RsRow("SiteCategoryGroupID")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

			' Row Rendering event
			Row_Rendering()

			' SiteCategoryTypeID
			If ew_NotEmpty(SiteCategoryTypeID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE [SiteCategoryTypeID] = " & ew_AdjustSql(SiteCategoryTypeID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryTypeNM] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					SiteCategoryTypeID.ViewValue = RsWrk("SiteCategoryTypeNM")
				Else
					SiteCategoryTypeID.ViewValue = SiteCategoryTypeID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				SiteCategoryTypeID.ViewValue = System.DBNull.Value
			End If
			SiteCategoryTypeID.CssStyle = ""
			SiteCategoryTypeID.CssClass = ""
			SiteCategoryTypeID.ViewCustomAttributes = ""

			' GroupOrder
			GroupOrder.ViewValue = GroupOrder.CurrentValue
			GroupOrder.CssStyle = ""
			GroupOrder.CssClass = ""
			GroupOrder.ViewCustomAttributes = ""

			' CategoryName
			CategoryName.ViewValue = CategoryName.CurrentValue
			CategoryName.CssStyle = ""
			CategoryName.CssClass = ""
			CategoryName.ViewCustomAttributes = ""

			' ParentCategoryID
			If ew_NotEmpty(ParentCategoryID.CurrentValue) Then
				sSqlWrk = "SELECT [CategoryName] FROM [SiteCategory] WHERE [SiteCategoryID] = " & ew_AdjustSql(ParentCategoryID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CategoryName] Asc"
				RsWrk = Conn.GetTempDataReader(sSqlWrk)
				If RsWrk.Read() Then
					ParentCategoryID.ViewValue = RsWrk("CategoryName")
				Else
					ParentCategoryID.ViewValue = ParentCategoryID.CurrentValue
				End If
				Conn.CloseTempDataReader()
			Else
				ParentCategoryID.ViewValue = System.DBNull.Value
			End If
			ParentCategoryID.CssStyle = ""
			ParentCategoryID.CssClass = ""
			ParentCategoryID.ViewCustomAttributes = ""

			' CategoryFileName
			CategoryFileName.ViewValue = CategoryFileName.CurrentValue
			CategoryFileName.CssStyle = ""
			CategoryFileName.CssClass = ""
			CategoryFileName.ViewCustomAttributes = ""

			' SiteCategoryGroupID
			If ew_NotEmpty(SiteCategoryGroupID.CurrentValue) Then
				sSqlWrk = "SELECT [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE [SiteCategoryGroupID] = " & ew_AdjustSql(SiteCategoryGroupID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [SiteCategoryGroupNM] Asc"
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

			' SiteCategoryTypeID
			SiteCategoryTypeID.HrefValue = ""

			' GroupOrder
			GroupOrder.HrefValue = ""

			' CategoryName
			CategoryName.HrefValue = ""

			' ParentCategoryID
			ParentCategoryID.HrefValue = ""

			' CategoryFileName
			CategoryFileName.HrefValue = ""

			' SiteCategoryGroupID
			SiteCategoryGroupID.HrefValue = ""

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
			If Name = "SiteCategoryID" Then Return SiteCategoryID
			If Name = "SiteCategoryTypeID" Then Return SiteCategoryTypeID
			If Name = "GroupOrder" Then Return GroupOrder
			If Name = "CategoryName" Then Return CategoryName
			If Name = "CategoryTitle" Then Return CategoryTitle
			If Name = "CategoryDescription" Then Return CategoryDescription
			If Name = "CategoryKeywords" Then Return CategoryKeywords
			If Name = "ParentCategoryID" Then Return ParentCategoryID
			If Name = "CategoryFileName" Then Return CategoryFileName
			If Name = "SiteCategoryGroupID" Then Return SiteCategoryGroupID
			Return Nothing		
		End Function

		' SiteCategoryID
		Private m_SiteCategoryID As cField

		Public ReadOnly Property SiteCategoryID() As cField
			Get
				If m_SiteCategoryID Is Nothing Then m_SiteCategoryID = New cField("SiteCategory", "x_SiteCategoryID", "SiteCategoryID", "[SiteCategoryID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_SiteCategoryID
			End Get
		End Property

		' SiteCategoryTypeID
		Private m_SiteCategoryTypeID As cField

		Public ReadOnly Property SiteCategoryTypeID() As cField
			Get
				If m_SiteCategoryTypeID Is Nothing Then m_SiteCategoryTypeID = New cField("SiteCategory", "x_SiteCategoryTypeID", "SiteCategoryTypeID", "[SiteCategoryTypeID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_SiteCategoryTypeID
			End Get
		End Property

		' GroupOrder
		Private m_GroupOrder As cField

		Public ReadOnly Property GroupOrder() As cField
			Get
				If m_GroupOrder Is Nothing Then m_GroupOrder = New cField("SiteCategory", "x_GroupOrder", "GroupOrder", "[GroupOrder]", 5, OleDbType.Double, EW_DATATYPE_NUMBER,  0)
				Return m_GroupOrder
			End Get
		End Property

		' CategoryName
		Private m_CategoryName As cField

		Public ReadOnly Property CategoryName() As cField
			Get
				If m_CategoryName Is Nothing Then m_CategoryName = New cField("SiteCategory", "x_CategoryName", "CategoryName", "[CategoryName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CategoryName
			End Get
		End Property

		' CategoryTitle
		Private m_CategoryTitle As cField

		Public ReadOnly Property CategoryTitle() As cField
			Get
				If m_CategoryTitle Is Nothing Then m_CategoryTitle = New cField("SiteCategory", "x_CategoryTitle", "CategoryTitle", "[CategoryTitle]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CategoryTitle
			End Get
		End Property

		' CategoryDescription
		Private m_CategoryDescription As cField

		Public ReadOnly Property CategoryDescription() As cField
			Get
				If m_CategoryDescription Is Nothing Then m_CategoryDescription = New cField("SiteCategory", "x_CategoryDescription", "CategoryDescription", "[CategoryDescription]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CategoryDescription
			End Get
		End Property

		' CategoryKeywords
		Private m_CategoryKeywords As cField

		Public ReadOnly Property CategoryKeywords() As cField
			Get
				If m_CategoryKeywords Is Nothing Then m_CategoryKeywords = New cField("SiteCategory", "x_CategoryKeywords", "CategoryKeywords", "[CategoryKeywords]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CategoryKeywords
			End Get
		End Property

		' ParentCategoryID
		Private m_ParentCategoryID As cField

		Public ReadOnly Property ParentCategoryID() As cField
			Get
				If m_ParentCategoryID Is Nothing Then m_ParentCategoryID = New cField("SiteCategory", "x_ParentCategoryID", "ParentCategoryID", "[ParentCategoryID]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_ParentCategoryID
			End Get
		End Property

		' CategoryFileName
		Private m_CategoryFileName As cField

		Public ReadOnly Property CategoryFileName() As cField
			Get
				If m_CategoryFileName Is Nothing Then m_CategoryFileName = New cField("SiteCategory", "x_CategoryFileName", "CategoryFileName", "[CategoryFileName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_CategoryFileName
			End Get
		End Property

		' SiteCategoryGroupID
		Private m_SiteCategoryGroupID As cField

		Public ReadOnly Property SiteCategoryGroupID() As cField
			Get
				If m_SiteCategoryGroupID Is Nothing Then m_SiteCategoryGroupID = New cField("SiteCategory", "x_SiteCategoryGroupID", "SiteCategoryGroupID", "[SiteCategoryGroupID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_SiteCategoryGroupID
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
			If m_SiteCategoryID IsNot Nothing Then m_SiteCategoryID.Dispose()
			If m_SiteCategoryTypeID IsNot Nothing Then m_SiteCategoryTypeID.Dispose()
			If m_GroupOrder IsNot Nothing Then m_GroupOrder.Dispose()
			If m_CategoryName IsNot Nothing Then m_CategoryName.Dispose()
			If m_CategoryTitle IsNot Nothing Then m_CategoryTitle.Dispose()
			If m_CategoryDescription IsNot Nothing Then m_CategoryDescription.Dispose()
			If m_CategoryKeywords IsNot Nothing Then m_CategoryKeywords.Dispose()
			If m_ParentCategoryID IsNot Nothing Then m_ParentCategoryID.Dispose()
			If m_CategoryFileName IsNot Nothing Then m_CategoryFileName.Dispose()
			If m_SiteCategoryGroupID IsNot Nothing Then m_SiteCategoryGroupID.Dispose()
		End Sub
	End Class
End Class
