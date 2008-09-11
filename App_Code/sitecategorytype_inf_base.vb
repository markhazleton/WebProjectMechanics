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
	''' Summary description for SiteCategoryTypeinf_base
	''' </summary>

	Public MustInherit Class SiteCategoryTypeinf_base
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

		Public Shared ReadOnly TableVar As String = "SiteCategoryType"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[SiteCategoryType]"
            _table.DefaultFilter = String.Empty
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[SiteCategoryTypeNM] ASC"

			' [SiteCategoryTypeID] Field
			strFldName = "SiteCategoryTypeID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[SiteCategoryTypeID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryTypeID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryTypeID]"
			_table.Fields(strFldName).SortParm = "SiteCategoryTypeID"
			_table.Fields(strFldName).AliasName = "SiteCategoryTypeID"
			_table.Fields(strFldName).ParameterName = "SiteCategoryTypeID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryTypeNM] Field
			strFldName = "SiteCategoryTypeNM"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryTypeNM]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryTypeNM"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryTypeNM]"
			_table.Fields(strFldName).SortParm = "SiteCategoryTypeNM"
			_table.Fields(strFldName).AliasName = "SiteCategoryTypeNM"
			_table.Fields(strFldName).ParameterName = "SiteCategoryTypeNM"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryTypeDS] Field
			strFldName = "SiteCategoryTypeDS"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryTypeDS]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryTypeDS"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryTypeDS]"
			_table.Fields(strFldName).SortParm = "SiteCategoryTypeDS"
			_table.Fields(strFldName).AliasName = "SiteCategoryTypeDS"
			_table.Fields(strFldName).ParameterName = "SiteCategoryTypeDS"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryComment] Field
			strFldName = "SiteCategoryComment"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryComment]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryComment"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryComment]"
			_table.Fields(strFldName).SortParm = "SiteCategoryComment"
			_table.Fields(strFldName).AliasName = "SiteCategoryComment"
			_table.Fields(strFldName).ParameterName = "SiteCategoryComment"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryFileName] Field
			strFldName = "SiteCategoryFileName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryFileName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryFileName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryFileName]"
			_table.Fields(strFldName).SortParm = "SiteCategoryFileName"
			_table.Fields(strFldName).AliasName = "SiteCategoryFileName"
			_table.Fields(strFldName).ParameterName = "SiteCategoryFileName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryTransferURL] Field
			strFldName = "SiteCategoryTransferURL"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryTransferURL]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryTransferURL"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryTransferURL]"
			_table.Fields(strFldName).SortParm = "SiteCategoryTransferURL"
			_table.Fields(strFldName).AliasName = "SiteCategoryTransferURL"
			_table.Fields(strFldName).ParameterName = "SiteCategoryTransferURL"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [DefaultSiteCategoryID] Field
			strFldName = "DefaultSiteCategoryID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[DefaultSiteCategoryID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_DefaultSiteCategoryID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[DefaultSiteCategoryID]"
			_table.Fields(strFldName).SortParm = "DefaultSiteCategoryID"
			_table.Fields(strFldName).AliasName = "DefaultSiteCategoryID"
			_table.Fields(strFldName).ParameterName = "DefaultSiteCategoryID"		
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
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, SiteCategoryTypeinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As SiteCategoryTypekey_base = TryCast(objProfile.MasterKey, SiteCategoryTypekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[SiteCategoryTypeID] = " & masterKey.SiteCategoryTypeID & ""
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

		Public Shared Function LoadKey(ByVal key As SiteCategoryTypekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field SiteCategoryTypeID
			Dim k_SiteCategoryTypeID As String = req.QueryString("SiteCategoryTypeID")
			If (k_SiteCategoryTypeID Is Nothing) Then
				messageList.Add("Missing key value: SiteCategoryTypeID")
			ElseIf (Not DataFormat.CheckInt32(k_SiteCategoryTypeID))
				messageList.Add("Invalid key value: SiteCategoryTypeID")
			Else
				key.SiteCategoryTypeID = Convert.ToInt32(k_SiteCategoryTypeID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As SiteCategoryTypekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("SiteCategoryTypeID=" & key.SiteCategoryTypeID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
