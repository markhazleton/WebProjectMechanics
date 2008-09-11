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
	''' Summary description for PageTypeinf_base
	''' </summary>

	Public MustInherit Class PageTypeinf_base
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

		Public Shared ReadOnly TableVar As String = "PageType"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[PageType]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[PageTypeCD] ASC"

			' [PageTypeID] Field
			strFldName = "PageTypeID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[PageTypeID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_PageTypeID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageTypeID]"
			_table.Fields(strFldName).SortParm = "PageTypeID"
			_table.Fields(strFldName).AliasName = "PageTypeID"
			_table.Fields(strFldName).ParameterName = "PageTypeID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [PageTypeCD] Field
			strFldName = "PageTypeCD"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageTypeCD]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageTypeCD"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageTypeCD]"
			_table.Fields(strFldName).SortParm = "PageTypeCD"
			_table.Fields(strFldName).AliasName = "PageTypeCD"
			_table.Fields(strFldName).ParameterName = "PageTypeCD"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [PageTypeDesc] Field
			strFldName = "PageTypeDesc"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageTypeDesc]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageTypeDesc"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageTypeDesc]"
			_table.Fields(strFldName).SortParm = "PageTypeDesc"
			_table.Fields(strFldName).AliasName = "PageTypeDesc"
			_table.Fields(strFldName).ParameterName = "PageTypeDesc"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [PageFileName] Field
			strFldName = "PageFileName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageFileName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageFileName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageFileName]"
			_table.Fields(strFldName).SortParm = "PageFileName"
			_table.Fields(strFldName).AliasName = "PageFileName"
			_table.Fields(strFldName).ParameterName = "PageFileName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False
		End Sub

		''' <summary>
		''' Gets User Filter 
		''' </summary>

		Public Shared Function GetUserFilter() As String
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, PageTypeinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As PageTypekey_base = TryCast(objProfile.MasterKey, PageTypekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[PageTypeID] = " & masterKey.PageTypeID & ""
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

		Public Shared Function LoadKey(ByVal key As PageTypekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field PageTypeID
			Dim k_PageTypeID As String = req.QueryString("PageTypeID")
			If (k_PageTypeID Is Nothing) Then
				messageList.Add("Missing key value: PageTypeID")
			ElseIf (Not DataFormat.CheckInt32(k_PageTypeID))
				messageList.Add("Invalid key value: PageTypeID")
			Else
				key.PageTypeID = Convert.ToInt32(k_PageTypeID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As PageTypekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("PageTypeID=" & key.PageTypeID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
