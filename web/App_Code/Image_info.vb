Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Table)
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	Public Image As cImage

	' Define table class
	Class cImage
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
				Return "Image"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Image"
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
				Return "Image_Highlight"
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
				Return "SELECT * FROM [Image]"
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
			Sql = "INSERT INTO [Image] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [Image] SET " & values
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
			Sql = "DELETE FROM [Image] WHERE "
			fld = FieldByName("ImageID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("ImageID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[ImageID] = @ImageID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(ImageID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@ImageID@", ew_AdjustSql(ImageID.CurrentValue)) ' Replace key value
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
					Return "Image_list.aspx"
				End If
			End Get
		End Property

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("Image_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "Image_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("Image_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("Image_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("Image_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(ImageID.CurrentValue) Then
				sUrl = sUrl & "ImageID=" & ImageID.CurrentValue
			Else
				Return "javascript:alert('Invalid Record! Key is null');"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=Image"
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
			ImageID.DbValue = RsRow("ImageID")
			CompanyID.DbValue = RsRow("CompanyID")
			title.DbValue = RsRow("title")
			ImageName.DbValue = RsRow("ImageName")
			ImageDescription.DbValue = RsRow("ImageDescription")
			ImageComment.DbValue = RsRow("ImageComment")
			ImageFileName.DbValue = RsRow("ImageFileName")
			ImageThumbFileName.DbValue = RsRow("ImageThumbFileName")
			ImageDate.DbValue = RsRow("ImageDate")
			Active.DbValue = IIf(ew_ConvertToBool(RsRow("Active")), "1", "0")
			ModifiedDT.DbValue = RsRow("ModifiedDT")
			VersionNo.DbValue = RsRow("VersionNo")
			ContactID.DbValue = RsRow("ContactID")
			medium.DbValue = RsRow("medium")
			size.DbValue = RsRow("size")
			price.DbValue = RsRow("price")
			color.DbValue = RsRow("color")
			subject.DbValue = RsRow("subject")
			sold.DbValue = IIf(ew_ConvertToBool(RsRow("sold")), "1", "0")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

			' Row Rendering event
			Row_Rendering()

			' CompanyID
			If ew_NotEmpty(CompanyID.CurrentValue) Then
				sSqlWrk = "SELECT [CompanyName] FROM [Company] WHERE [CompanyID] = " & ew_AdjustSql(CompanyID.CurrentValue) & ""
				sSqlWrk = sSqlWrk & " ORDER BY [CompanyName] "
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

			' title
			title.ViewValue = title.CurrentValue
			title.CssStyle = ""
			title.CssClass = ""
			title.ViewCustomAttributes = ""

			' ImageName
			ImageName.ViewValue = ImageName.CurrentValue
			ImageName.CssStyle = ""
			ImageName.CssClass = ""
			ImageName.ViewCustomAttributes = ""

			' CompanyID
			CompanyID.HrefValue = ""

			' title
			title.HrefValue = ""

			' ImageName
			ImageName.HrefValue = ""

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
			If Name = "ImageID" Then Return ImageID
			If Name = "CompanyID" Then Return CompanyID
			If Name = "title" Then Return title
			If Name = "ImageName" Then Return ImageName
			If Name = "ImageDescription" Then Return ImageDescription
			If Name = "ImageComment" Then Return ImageComment
			If Name = "ImageFileName" Then Return ImageFileName
			If Name = "ImageThumbFileName" Then Return ImageThumbFileName
			If Name = "ImageDate" Then Return ImageDate
			If Name = "Active" Then Return Active
			If Name = "ModifiedDT" Then Return ModifiedDT
			If Name = "VersionNo" Then Return VersionNo
			If Name = "ContactID" Then Return ContactID
			If Name = "medium" Then Return medium
			If Name = "size" Then Return size
			If Name = "price" Then Return price
			If Name = "color" Then Return color
			If Name = "subject" Then Return subject
			If Name = "sold" Then Return sold
			Return Nothing		
		End Function

		' ImageID
		Private m_ImageID As cField

		Public ReadOnly Property ImageID() As cField
			Get
				If m_ImageID Is Nothing Then m_ImageID = New cField("Image", "x_ImageID", "ImageID", "[ImageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_ImageID
			End Get
		End Property

		' CompanyID
		Private m_CompanyID As cField

		Public ReadOnly Property CompanyID() As cField
			Get
				If m_CompanyID Is Nothing Then m_CompanyID = New cField("Image", "x_CompanyID", "CompanyID", "[CompanyID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_CompanyID
			End Get
		End Property

		' title
		Private m_title As cField

		Public ReadOnly Property title() As cField
			Get
				If m_title Is Nothing Then m_title = New cField("Image", "x_title", "title", "[title]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_title
			End Get
		End Property

		' ImageName
		Private m_ImageName As cField

		Public ReadOnly Property ImageName() As cField
			Get
				If m_ImageName Is Nothing Then m_ImageName = New cField("Image", "x_ImageName", "ImageName", "[ImageName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_ImageName
			End Get
		End Property

		' ImageDescription
		Private m_ImageDescription As cField

		Public ReadOnly Property ImageDescription() As cField
			Get
				If m_ImageDescription Is Nothing Then m_ImageDescription = New cField("Image", "x_ImageDescription", "ImageDescription", "[ImageDescription]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_ImageDescription
			End Get
		End Property

		' ImageComment
		Private m_ImageComment As cField

		Public ReadOnly Property ImageComment() As cField
			Get
				If m_ImageComment Is Nothing Then m_ImageComment = New cField("Image", "x_ImageComment", "ImageComment", "[ImageComment]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_ImageComment
			End Get
		End Property

		' ImageFileName
		Private m_ImageFileName As cField

		Public ReadOnly Property ImageFileName() As cField
			Get
				If m_ImageFileName Is Nothing Then m_ImageFileName = New cField("Image", "x_ImageFileName", "ImageFileName", "[ImageFileName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_ImageFileName
			End Get
		End Property

		' ImageThumbFileName
		Private m_ImageThumbFileName As cField

		Public ReadOnly Property ImageThumbFileName() As cField
			Get
				If m_ImageThumbFileName Is Nothing Then m_ImageThumbFileName = New cField("Image", "x_ImageThumbFileName", "ImageThumbFileName", "[ImageThumbFileName]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_ImageThumbFileName
			End Get
		End Property

		' ImageDate
		Private m_ImageDate As cField

		Public ReadOnly Property ImageDate() As cField
			Get
				If m_ImageDate Is Nothing Then m_ImageDate = New cField("Image", "x_ImageDate", "ImageDate", "[ImageDate]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_ImageDate
			End Get
		End Property

		' Active
		Private m_Active As cField

		Public ReadOnly Property Active() As cField
			Get
				If m_Active Is Nothing Then m_Active = New cField("Image", "x_Active", "Active", "[Active]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_Active
			End Get
		End Property

		' ModifiedDT
		Private m_ModifiedDT As cField

		Public ReadOnly Property ModifiedDT() As cField
			Get
				If m_ModifiedDT Is Nothing Then m_ModifiedDT = New cField("Image", "x_ModifiedDT", "ModifiedDT", "[ModifiedDT]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_ModifiedDT
			End Get
		End Property

		' VersionNo
		Private m_VersionNo As cField

		Public ReadOnly Property VersionNo() As cField
			Get
				If m_VersionNo Is Nothing Then m_VersionNo = New cField("Image", "x_VersionNo", "VersionNo", "[VersionNo]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_VersionNo
			End Get
		End Property

		' ContactID
		Private m_ContactID As cField

		Public ReadOnly Property ContactID() As cField
			Get
				If m_ContactID Is Nothing Then m_ContactID = New cField("Image", "x_ContactID", "ContactID", "[ContactID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_ContactID
			End Get
		End Property

		' medium
		Private m_medium As cField

		Public ReadOnly Property medium() As cField
			Get
				If m_medium Is Nothing Then m_medium = New cField("Image", "x_medium", "medium", "[medium]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_medium
			End Get
		End Property

		' size
		Private m_size As cField

		Public ReadOnly Property size() As cField
			Get
				If m_size Is Nothing Then m_size = New cField("Image", "x_size", "size", "[size]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_size
			End Get
		End Property

		' price
		Private m_price As cField

		Public ReadOnly Property price() As cField
			Get
				If m_price Is Nothing Then m_price = New cField("Image", "x_price", "price", "[price]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_price
			End Get
		End Property

		' color
		Private m_color As cField

		Public ReadOnly Property color() As cField
			Get
				If m_color Is Nothing Then m_color = New cField("Image", "x_color", "color", "[color]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_color
			End Get
		End Property

		' subject
		Private m_subject As cField

		Public ReadOnly Property subject() As cField
			Get
				If m_subject Is Nothing Then m_subject = New cField("Image", "x_subject", "subject", "[subject]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_subject
			End Get
		End Property

		' sold
		Private m_sold As cField

		Public ReadOnly Property sold() As cField
			Get
				If m_sold Is Nothing Then m_sold = New cField("Image", "x_sold", "sold", "[sold]", 11, OleDbType.Boolean, EW_DATATYPE_BOOLEAN,  0)
				Return m_sold
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
			If m_ImageID IsNot Nothing Then m_ImageID.Dispose()
			If m_CompanyID IsNot Nothing Then m_CompanyID.Dispose()
			If m_title IsNot Nothing Then m_title.Dispose()
			If m_ImageName IsNot Nothing Then m_ImageName.Dispose()
			If m_ImageDescription IsNot Nothing Then m_ImageDescription.Dispose()
			If m_ImageComment IsNot Nothing Then m_ImageComment.Dispose()
			If m_ImageFileName IsNot Nothing Then m_ImageFileName.Dispose()
			If m_ImageThumbFileName IsNot Nothing Then m_ImageThumbFileName.Dispose()
			If m_ImageDate IsNot Nothing Then m_ImageDate.Dispose()
			If m_Active IsNot Nothing Then m_Active.Dispose()
			If m_ModifiedDT IsNot Nothing Then m_ModifiedDT.Dispose()
			If m_VersionNo IsNot Nothing Then m_VersionNo.Dispose()
			If m_ContactID IsNot Nothing Then m_ContactID.Dispose()
			If m_medium IsNot Nothing Then m_medium.Dispose()
			If m_size IsNot Nothing Then m_size.Dispose()
			If m_price IsNot Nothing Then m_price.Dispose()
			If m_color IsNot Nothing Then m_color.Dispose()
			If m_subject IsNot Nothing Then m_subject.Dispose()
			If m_sold IsNot Nothing Then m_sold.Dispose()
		End Sub
	End Class
End Class
