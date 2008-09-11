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
	''' Summary description for SiteTemplateinf_base
	''' </summary>

	Public MustInherit Class SiteTemplateinf_base
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

		Public Shared ReadOnly TableVar As String = "SiteTemplate"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[SiteTemplate]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[Name] ASC"

			' [TemplatePrefix] Field
			strFldName = "TemplatePrefix"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[TemplatePrefix]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_TemplatePrefix"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[TemplatePrefix]"
			_table.Fields(strFldName).SortParm = "TemplatePrefix"
			_table.Fields(strFldName).AliasName = "TemplatePrefix"
			_table.Fields(strFldName).ParameterName = "TemplatePrefix"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Name] Field
			strFldName = "Name"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Name]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Name"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Name]"
			_table.Fields(strFldName).SortParm = "Name"
			_table.Fields(strFldName).AliasName = "Name"
			_table.Fields(strFldName).ParameterName = "Name"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Top] Field
			strFldName = "Top"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Top]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_Top"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Top]"
			_table.Fields(strFldName).SortParm = "Top"
			_table.Fields(strFldName).AliasName = "Top"
			_table.Fields(strFldName).ParameterName = "Top"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Bottom] Field
			strFldName = "Bottom"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Bottom]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_Bottom"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Bottom]"
			_table.Fields(strFldName).SortParm = "Bottom"
			_table.Fields(strFldName).AliasName = "Bottom"
			_table.Fields(strFldName).ParameterName = "Bottom"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [CSS] Field
			strFldName = "CSS"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[CSS]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_CSS"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[CSS]"
			_table.Fields(strFldName).SortParm = "CSS"
			_table.Fields(strFldName).AliasName = "CSS"
			_table.Fields(strFldName).ParameterName = "CSS"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False
		End Sub

		''' <summary>
		''' Gets User Filter 
		''' </summary>

		Public Shared Function GetUserFilter() As String
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, SiteTemplateinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As SiteTemplatekey_base = TryCast(objProfile.MasterKey, SiteTemplatekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[TemplatePrefix] = '" & masterKey.TemplatePrefix & "'"
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

		Public Shared Function LoadKey(ByVal key As SiteTemplatekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field TemplatePrefix
			Dim k_TemplatePrefix As String = req.QueryString("TemplatePrefix")
			If (k_TemplatePrefix Is Nothing) Then
				messageList.Add("Missing key value: TemplatePrefix")
			ElseIf (Not DataFormat.CheckString(k_TemplatePrefix))
				messageList.Add("Invalid key value: TemplatePrefix")
			Else
				key.TemplatePrefix = Convert.ToString(k_TemplatePrefix)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As SiteTemplatekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("TemplatePrefix=" & key.TemplatePrefix.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
