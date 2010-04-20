Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 Project Class (Table)
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	Public zMessage As czMessage

	' Define table class
	Class czMessage
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
				Return "zMessage"
			End Get
		End Property

		' Table name
		Public ReadOnly Property TableName() As String
			Get
				Return "Message"
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
				Return "zMessage_Highlight"
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
				Return "SELECT * FROM [Message]"
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
			Sql = "INSERT INTO [Message] (" & names & ") VALUES (" & values & ")"
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
			Sql = "UPDATE [Message] SET " & values
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
			Sql = "DELETE FROM [Message] WHERE "
			fld = FieldByName("MessageID")
			Sql = Sql & fld.FldExpression & "=" & ew_QuotedValue(Rs("MessageID"), EW_DATATYPE_NUMBER) & " AND "
			If Sql.EndsWith(" AND ") Then Sql = Sql.Remove(Sql.Length - 5)
			If CurrentFilter <> "" Then	Sql = Sql & " AND " & CurrentFilter
			Return Conn.Execute(Sql)
		End Function

		' Key filter for table
		Private ReadOnly Property SqlKeyFilter() As String
			Get
				Return "[MessageID] = @MessageID@"
			End Get
		End Property

		' Return Key filter for table
		Public ReadOnly Property KeyFilter() As String
			Get
				Dim sKeyFilter As String
				sKeyFilter = SqlKeyFilter
				If Not IsNumeric(MessageID.CurrentValue) Then
					sKeyFilter = "0=1" ' Invalid key
				End If
				sKeyFilter = sKeyFilter.Replace("@MessageID@", ew_AdjustSql(MessageID.CurrentValue)) ' Replace key value
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
					Return "zMessage_list.aspx"
				End If
			End Get
		End Property

		' View URL
		Public Function ViewUrl() As String
			Return KeyUrl("zMessage_view.aspx", UrlParm(""))
		End Function

		' Add URL
		Public Function AddUrl() As String
			AddUrl = "zMessage_add.aspx"
			Dim sUrlParm As String
			sUrlParm = UrlParm("")
			If sUrlParm <> "" Then AddUrl = AddUrl & "?" & sUrlParm
		End Function

		' Edit URL
		Public Function EditUrl() As String
			Return KeyUrl("zMessage_edit.aspx", UrlParm(""))
		End Function

		' Inline edit URL
		Public Function InlineEditUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=edit"))
		End Function

		' Copy URL
		Public Function CopyUrl() As String
			Return KeyUrl("zMessage_add.aspx", UrlParm(""))
		End Function

		' Inline copy URL
		Public Function InlineCopyUrl() As String
			Return KeyUrl(ew_CurrentPage(), UrlParm("a=copy"))
		End Function

		' Delete URL
		Public Function DeleteUrl() As String
			Return KeyUrl("zMessage_delete.aspx", UrlParm(""))
		End Function

		' Key URL
		Public Function KeyUrl(url As String, parm As String) As String
			Dim sUrl As String
			sUrl = url & "?"
			If parm <> "" Then sUrl = sUrl & parm & "&"
			If Not IsDbNull(MessageID.CurrentValue) Then
				sUrl = sUrl & "MessageID=" & MessageID.CurrentValue
			Else
				Return "javascript:alert('Invalid Record! Key is null');"
			End If
			Return sUrl
		End Function

		' URL parm
		Function UrlParm(parm As String) As String
			Dim OutStr As String = ""
			If UseTokenInUrl Then
				OutStr = "t=zMessage"
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
			MessageID.DbValue = RsRow("MessageID")
			zPageID.DbValue = RsRow("PageID")
			ParentMessageID.DbValue = RsRow("ParentMessageID")
			Subject.DbValue = RsRow("Subject")
			Author.DbValue = RsRow("Author")
			zEmail.DbValue = RsRow("Email")
			City.DbValue = RsRow("City")
			URL.DbValue = RsRow("URL")
			MessageDate.DbValue = RsRow("MessageDate")
			Body.DbValue = RsRow("Body")
		End Sub

		' Render list row values
		Public Sub RenderListRow()

			' Row Rendering event
			Row_Rendering()

			' MessageID
			MessageID.ViewValue = MessageID.CurrentValue
			MessageID.CssStyle = ""
			MessageID.CssClass = ""
			MessageID.ViewCustomAttributes = ""

			' PageID
			zPageID.ViewValue = zPageID.CurrentValue
			zPageID.CssStyle = ""
			zPageID.CssClass = ""
			zPageID.ViewCustomAttributes = ""

			' ParentMessageID
			ParentMessageID.ViewValue = ParentMessageID.CurrentValue
			ParentMessageID.CssStyle = ""
			ParentMessageID.CssClass = ""
			ParentMessageID.ViewCustomAttributes = ""

			' Subject
			Subject.ViewValue = Subject.CurrentValue
			Subject.CssStyle = ""
			Subject.CssClass = ""
			Subject.ViewCustomAttributes = ""

			' Author
			Author.ViewValue = Author.CurrentValue
			Author.CssStyle = ""
			Author.CssClass = ""
			Author.ViewCustomAttributes = ""

			' Email
			zEmail.ViewValue = zEmail.CurrentValue
			zEmail.CssStyle = ""
			zEmail.CssClass = ""
			zEmail.ViewCustomAttributes = ""

			' City
			City.ViewValue = City.CurrentValue
			City.CssStyle = ""
			City.CssClass = ""
			City.ViewCustomAttributes = ""

			' URL
			URL.ViewValue = URL.CurrentValue
			URL.CssStyle = ""
			URL.CssClass = ""
			URL.ViewCustomAttributes = ""

			' MessageDate
			MessageDate.ViewValue = MessageDate.CurrentValue
			MessageDate.ViewValue = ew_FormatDateTime(MessageDate.ViewValue, 6)
			MessageDate.CssStyle = ""
			MessageDate.CssClass = ""
			MessageDate.ViewCustomAttributes = ""

			' MessageID
			MessageID.HrefValue = ""

			' PageID
			zPageID.HrefValue = ""

			' ParentMessageID
			ParentMessageID.HrefValue = ""

			' Subject
			Subject.HrefValue = ""

			' Author
			Author.HrefValue = ""

			' Email
			zEmail.HrefValue = ""

			' City
			City.HrefValue = ""

			' URL
			URL.HrefValue = ""

			' MessageDate
			MessageDate.HrefValue = ""

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
			If Name = "MessageID" Then Return MessageID
			If Name = "PageID" Then Return zPageID
			If Name = "ParentMessageID" Then Return ParentMessageID
			If Name = "Subject" Then Return Subject
			If Name = "Author" Then Return Author
			If Name = "Email" Then Return zEmail
			If Name = "City" Then Return City
			If Name = "URL" Then Return URL
			If Name = "MessageDate" Then Return MessageDate
			If Name = "Body" Then Return Body
			Return Nothing		
		End Function

		' MessageID
		Private m_MessageID As cField

		Public ReadOnly Property MessageID() As cField
			Get
				If m_MessageID Is Nothing Then m_MessageID = New cField("zMessage", "x_MessageID", "MessageID", "[MessageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_MessageID
			End Get
		End Property

		' PageID
		Private m_zPageID As cField

		Public ReadOnly Property zPageID() As cField
			Get
				If m_zPageID Is Nothing Then m_zPageID = New cField("zMessage", "x_zPageID", "PageID", "[PageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_zPageID
			End Get
		End Property

		' ParentMessageID
		Private m_ParentMessageID As cField

		Public ReadOnly Property ParentMessageID() As cField
			Get
				If m_ParentMessageID Is Nothing Then m_ParentMessageID = New cField("zMessage", "x_ParentMessageID", "ParentMessageID", "[ParentMessageID]", 3, OleDbType.Integer, EW_DATATYPE_NUMBER,  0)
				Return m_ParentMessageID
			End Get
		End Property

		' Subject
		Private m_Subject As cField

		Public ReadOnly Property Subject() As cField
			Get
				If m_Subject Is Nothing Then m_Subject = New cField("zMessage", "x_Subject", "Subject", "[Subject]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Subject
			End Get
		End Property

		' Author
		Private m_Author As cField

		Public ReadOnly Property Author() As cField
			Get
				If m_Author Is Nothing Then m_Author = New cField("zMessage", "x_Author", "Author", "[Author]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_Author
			End Get
		End Property

		' Email
		Private m_zEmail As cField

		Public ReadOnly Property zEmail() As cField
			Get
				If m_zEmail Is Nothing Then m_zEmail = New cField("zMessage", "x_zEmail", "Email", "[Email]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_zEmail
			End Get
		End Property

		' City
		Private m_City As cField

		Public ReadOnly Property City() As cField
			Get
				If m_City Is Nothing Then m_City = New cField("zMessage", "x_City", "City", "[City]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_City
			End Get
		End Property

		' URL
		Private m_URL As cField

		Public ReadOnly Property URL() As cField
			Get
				If m_URL Is Nothing Then m_URL = New cField("zMessage", "x_URL", "URL", "[URL]", 202, OleDbType.VarWChar, EW_DATATYPE_STRING,  0)
				Return m_URL
			End Get
		End Property

		' MessageDate
		Private m_MessageDate As cField

		Public ReadOnly Property MessageDate() As cField
			Get
				If m_MessageDate Is Nothing Then m_MessageDate = New cField("zMessage", "x_MessageDate", "MessageDate", "[MessageDate]", 135, OleDbType.DBTimeStamp, EW_DATATYPE_DATE,  6)
				Return m_MessageDate
			End Get
		End Property

		' Body
		Private m_Body As cField

		Public ReadOnly Property Body() As cField
			Get
				If m_Body Is Nothing Then m_Body = New cField("zMessage", "x_Body", "Body", "[Body]", 203, OleDbType.LongVarWChar, EW_DATATYPE_MEMO,  0)
				Return m_Body
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
			If m_MessageID IsNot Nothing Then m_MessageID.Dispose()
			If m_zPageID IsNot Nothing Then m_zPageID.Dispose()
			If m_ParentMessageID IsNot Nothing Then m_ParentMessageID.Dispose()
			If m_Subject IsNot Nothing Then m_Subject.Dispose()
			If m_Author IsNot Nothing Then m_Author.Dispose()
			If m_zEmail IsNot Nothing Then m_zEmail.Dispose()
			If m_City IsNot Nothing Then m_City.Dispose()
			If m_URL IsNot Nothing Then m_URL.Dispose()
			If m_MessageDate IsNot Nothing Then m_MessageDate.Dispose()
			If m_Body IsNot Nothing Then m_Body.Dispose()
		End Sub
	End Class
End Class