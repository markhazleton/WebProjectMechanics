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
	''' Summary description for CompanySiteParameterinf_base
	''' </summary>

	Public MustInherit Class CompanySiteParameterinf_base
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

		Public Shared ReadOnly TableVar As String = "CompanySiteParameter"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[CompanySiteParameter]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = String.Empty

			' [CompanySiteParameterID] Field
			strFldName = "CompanySiteParameterID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[CompanySiteParameterID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_CompanySiteParameterID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[CompanySiteParameterID]"
			_table.Fields(strFldName).SortParm = "CompanySiteParameterID"
			_table.Fields(strFldName).AliasName = "CompanySiteParameterID"
			_table.Fields(strFldName).ParameterName = "CompanySiteParameterID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [CompanyID] Field
			strFldName = "CompanyID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[CompanyID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_CompanyID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[CompanyID]"
			_table.Fields(strFldName).SortParm = "CompanyID"
			_table.Fields(strFldName).AliasName = "CompanyID"
			_table.Fields(strFldName).ParameterName = "CompanyID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteParameterTypeID] Field
			strFldName = "SiteParameterTypeID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteParameterTypeID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SiteParameterTypeID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteParameterTypeID]"
			_table.Fields(strFldName).SortParm = "SiteParameterTypeID"
			_table.Fields(strFldName).AliasName = "SiteParameterTypeID"
			_table.Fields(strFldName).ParameterName = "SiteParameterTypeID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SortOrder] Field
			strFldName = "SortOrder"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SortOrder]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SortOrder"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SortOrder]"
			_table.Fields(strFldName).SortParm = "SortOrder"
			_table.Fields(strFldName).AliasName = "SortOrder"
			_table.Fields(strFldName).ParameterName = "SortOrder"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [ParameterValue] Field
			strFldName = "ParameterValue"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[ParameterValue]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_ParameterValue"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ParameterValue]"
			_table.Fields(strFldName).SortParm = "ParameterValue"
			_table.Fields(strFldName).AliasName = "ParameterValue"
			_table.Fields(strFldName).ParameterName = "ParameterValue"		
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
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, CompanySiteParameterinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As CompanySiteParameterkey_base = TryCast(objProfile.MasterKey, CompanySiteParameterkey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[CompanySiteParameterID] = " & masterKey.CompanySiteParameterID & ""
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

		Public Shared Function LoadKey(ByVal key As CompanySiteParameterkey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field CompanySiteParameterID
			Dim k_CompanySiteParameterID As String = req.QueryString("CompanySiteParameterID")
			If (k_CompanySiteParameterID Is Nothing) Then
				messageList.Add("Missing key value: CompanySiteParameterID")
			ElseIf (Not DataFormat.CheckInt32(k_CompanySiteParameterID))
				messageList.Add("Invalid key value: CompanySiteParameterID")
			Else
				key.CompanySiteParameterID = Convert.ToInt32(k_CompanySiteParameterID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As CompanySiteParameterkey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("CompanySiteParameterID=" & key.CompanySiteParameterID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
