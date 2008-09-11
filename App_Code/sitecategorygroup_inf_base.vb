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
	''' Summary description for SiteCategoryGroupinf_base
	''' </summary>

	Public MustInherit Class SiteCategoryGroupinf_base
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

		Public Shared ReadOnly TableVar As String = "SiteCategoryGroup"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[SiteCategoryGroup]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[SiteCategoryGroupNM] ASC"

			' [SiteCategoryGroupID] Field
			strFldName = "SiteCategoryGroupID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[SiteCategoryGroupID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryGroupID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryGroupID]"
			_table.Fields(strFldName).SortParm = "SiteCategoryGroupID"
			_table.Fields(strFldName).AliasName = "SiteCategoryGroupID"
			_table.Fields(strFldName).ParameterName = "SiteCategoryGroupID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryGroupNM] Field
			strFldName = "SiteCategoryGroupNM"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryGroupNM]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryGroupNM"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryGroupNM]"
			_table.Fields(strFldName).SortParm = "SiteCategoryGroupNM"
			_table.Fields(strFldName).AliasName = "SiteCategoryGroupNM"
			_table.Fields(strFldName).ParameterName = "SiteCategoryGroupNM"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryGroupDS] Field
			strFldName = "SiteCategoryGroupDS"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryGroupDS]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryGroupDS"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryGroupDS]"
			_table.Fields(strFldName).SortParm = "SiteCategoryGroupDS"
			_table.Fields(strFldName).AliasName = "SiteCategoryGroupDS"
			_table.Fields(strFldName).ParameterName = "SiteCategoryGroupDS"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryGroupOrder] Field
			strFldName = "SiteCategoryGroupOrder"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryGroupOrder]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryGroupOrder"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryGroupOrder]"
			_table.Fields(strFldName).SortParm = "SiteCategoryGroupOrder"
			_table.Fields(strFldName).AliasName = "SiteCategoryGroupOrder"
			_table.Fields(strFldName).ParameterName = "SiteCategoryGroupOrder"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
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
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryGroupinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As SiteCategoryGroupkey_base = TryCast(objProfile.MasterKey, SiteCategoryGroupkey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[SiteCategoryGroupID] = " & masterKey.SiteCategoryGroupID & ""
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

		Public Shared Function LoadKey(ByVal key As SiteCategoryGroupkey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field SiteCategoryGroupID
			Dim k_SiteCategoryGroupID As String = req.QueryString("SiteCategoryGroupID")
			If (k_SiteCategoryGroupID Is Nothing) Then
				messageList.Add("Missing key value: SiteCategoryGroupID")
			ElseIf (Not DataFormat.CheckInt32(k_SiteCategoryGroupID))
				messageList.Add("Invalid key value: SiteCategoryGroupID")
			Else
				key.SiteCategoryGroupID = Convert.ToInt32(k_SiteCategoryGroupID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As SiteCategoryGroupkey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("SiteCategoryGroupID=" & key.SiteCategoryGroupID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
