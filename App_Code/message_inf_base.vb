Imports System
Imports System.Collections 
Imports System.Collections.Generic 
Imports System.Text
Imports System.Web 
Imports EW.Data
Imports EW.Data.Utilities
Imports EW.Web
Namespace PMGEN

	''' <summary>
	''' Summary description for Messageinf_base
	''' </summary>

	Public MustInherit Class Messageinf_base
		Private _table As Table = New Table()

		''' <summary>
		''' Gets the table info
		''' </summary>

		Public Overridable ReadOnly Property TableInfo As Ew.Data.Table
			Get
				Return _table
			End Get
		End Property

		''' <summary>
		''' Gets the table variable name
		''' </summary>

		Public Shared ReadOnly TableVar As String = "Message"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[Message]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = String.Empty

			' [MessageID] Field
			strFldName = "MessageID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[MessageID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_MessageID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[MessageID]"
			_table.Fields(strFldName).SortParm = "MessageID"
			_table.Fields(strFldName).AliasName = "MessageID"
			_table.Fields(strFldName).ParameterName = "MessageID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [PageID] Field
			strFldName = "PageID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_PageID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageID]"
			_table.Fields(strFldName).SortParm = "PageID"
			_table.Fields(strFldName).AliasName = "PageID"
			_table.Fields(strFldName).ParameterName = "PageID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [ParentMessageID] Field
			strFldName = "ParentMessageID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[ParentMessageID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_ParentMessageID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ParentMessageID]"
			_table.Fields(strFldName).SortParm = "ParentMessageID"
			_table.Fields(strFldName).AliasName = "ParentMessageID"
			_table.Fields(strFldName).ParameterName = "ParentMessageID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [Subject] Field
			strFldName = "Subject"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Subject]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Subject"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Subject]"
			_table.Fields(strFldName).SortParm = "Subject"
			_table.Fields(strFldName).AliasName = "Subject"
			_table.Fields(strFldName).ParameterName = "Subject"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [Author] Field
			strFldName = "Author"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Author]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Author"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Author]"
			_table.Fields(strFldName).SortParm = "Author"
			_table.Fields(strFldName).AliasName = "Author"
			_table.Fields(strFldName).ParameterName = "Author"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [Email] Field
			strFldName = "Email"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Email]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Email"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Email]"
			_table.Fields(strFldName).SortParm = "Email"
			_table.Fields(strFldName).AliasName = "Email"
			_table.Fields(strFldName).ParameterName = "Email"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [City] Field
			strFldName = "City"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[City]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_City"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[City]"
			_table.Fields(strFldName).SortParm = "City"
			_table.Fields(strFldName).AliasName = "City"
			_table.Fields(strFldName).ParameterName = "City"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [URL] Field
			strFldName = "URL"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[URL]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_URL"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[URL]"
			_table.Fields(strFldName).SortParm = "URL"
			_table.Fields(strFldName).AliasName = "URL"
			_table.Fields(strFldName).ParameterName = "URL"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [MessageDate] Field
			strFldName = "MessageDate"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[MessageDate]"
			_table.Fields(strFldName).FieldType = 135
			_table.Fields(strFldName).FieldVar = "x_MessageDate"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[MessageDate]"
			_table.Fields(strFldName).SortParm = "MessageDate"
			_table.Fields(strFldName).AliasName = "MessageDate"
			_table.Fields(strFldName).ParameterName = "MessageDate"		
			_table.Fields(strFldName).QuoteStart = "#"			
			_table.Fields(strFldName).QuoteEnd = "#"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [Body] Field
			strFldName = "Body"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Body]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_Body"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Body]"
			_table.Fields(strFldName).SortParm = "Body"
			_table.Fields(strFldName).AliasName = "Body"
			_table.Fields(strFldName).ParameterName = "Body"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False
		End Sub

		''' <summary>
		''' Gets User Filter 
		''' </summary>

		Public Shared Function GetUserFilter() As String
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, Messageinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As Messagekey_base = TryCast(objProfile.MasterKey, Messagekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[MessageID] = " & masterKey.MessageID & ""
				If (strTemp.Length > 0) Then
					If (strFilter.Length > 0) Then strFilter &= " AND "
					strFilter &= "(" & strTemp & ")"
				End If
			End If
			Return strFilter
		End Function

		''' <summary>
		''' Constructs a key object from querystring
		''' </summary>

		Public Shared Function LoadKey(ByVal key As Messagekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field MessageID
			Dim k_MessageID As String = req.QueryString("MessageID")
			If (k_MessageID Is Nothing) Then
				messageList.Add("Missing key value: MessageID")
			ElseIf (Not DataFormat.CheckInt32(k_MessageID))
				messageList.Add("Invalid key value: MessageID")
			Else
				key.MessageID = Convert.ToInt32(k_MessageID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As Messagekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("MessageID=" & key.MessageID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
