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
	''' Summary description for PageRoleinf_base
	''' </summary>

	Public MustInherit Class PageRoleinf_base
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

		Public Shared ReadOnly TableVar As String = "PageRole"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[PageRole]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = String.Empty

			' [PageRoleID] Field
			strFldName = "PageRoleID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[PageRoleID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_PageRoleID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageRoleID]"
			_table.Fields(strFldName).SortParm = "PageRoleID"
			_table.Fields(strFldName).AliasName = "PageRoleID"
			_table.Fields(strFldName).ParameterName = "PageRoleID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [RoleID] Field
			strFldName = "RoleID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[RoleID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_RoleID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[RoleID]"
			_table.Fields(strFldName).SortParm = "RoleID"
			_table.Fields(strFldName).AliasName = "RoleID"
			_table.Fields(strFldName).ParameterName = "RoleID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
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
		End Sub

		''' <summary>
		''' Gets User Filter 
		''' </summary>

		Public Shared Function GetUserFilter() As String
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, PageRoleinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As PageRolekey_base = TryCast(objProfile.MasterKey, PageRolekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[PageRoleID] = " & masterKey.PageRoleID & ""
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

		Public Shared Function LoadKey(ByVal key As PageRolekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field PageRoleID
			Dim k_PageRoleID As String = req.QueryString("PageRoleID")
			If (k_PageRoleID Is Nothing) Then
				messageList.Add("Missing key value: PageRoleID")
			ElseIf (Not DataFormat.CheckInt32(k_PageRoleID))
				messageList.Add("Invalid key value: PageRoleID")
			Else
				key.PageRoleID = Convert.ToInt32(k_PageRoleID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As PageRolekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("PageRoleID=" & key.PageRoleID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
