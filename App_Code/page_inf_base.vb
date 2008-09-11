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
	''' Summary description for Pageinf_base
	''' </summary>

	Public MustInherit Class Pageinf_base
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

		Public Shared ReadOnly TableVar As String = "Page"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[Page]"
            _table.DefaultFilter = "[CompanyID]=" & System.Web.HttpContext.Current.Session.Item("CompanyID") & " "
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[CompanyID] ASC,[PageName] ASC"

			' [PageID] Field
			strFldName = "PageID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PageOrder] Field
			strFldName = "PageOrder"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageOrder]"
			_table.Fields(strFldName).FieldType = 2
			_table.Fields(strFldName).FieldVar = "x_PageOrder"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageOrder]"
			_table.Fields(strFldName).SortParm = "PageOrder"
			_table.Fields(strFldName).AliasName = "PageOrder"
			_table.Fields(strFldName).ParameterName = "PageOrder"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PageName] Field
			strFldName = "PageName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageName]"
			_table.Fields(strFldName).SortParm = "PageName"
			_table.Fields(strFldName).AliasName = "PageName"
			_table.Fields(strFldName).ParameterName = "PageName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [PageTitle] Field
			strFldName = "PageTitle"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageTitle]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageTitle"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageTitle]"
			_table.Fields(strFldName).SortParm = "PageTitle"
			_table.Fields(strFldName).AliasName = "PageTitle"
			_table.Fields(strFldName).ParameterName = "PageTitle"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PageDescription] Field
			strFldName = "PageDescription"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageDescription]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageDescription"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageDescription]"
			_table.Fields(strFldName).SortParm = "PageDescription"
			_table.Fields(strFldName).AliasName = "PageDescription"
			_table.Fields(strFldName).ParameterName = "PageDescription"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PageKeywords] Field
			strFldName = "PageKeywords"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PageKeywords]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PageKeywords"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PageKeywords]"
			_table.Fields(strFldName).SortParm = "PageKeywords"
			_table.Fields(strFldName).AliasName = "PageKeywords"
			_table.Fields(strFldName).ParameterName = "PageKeywords"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PageTypeID] Field
			strFldName = "PageTypeID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
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
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [ImagesPerRow] Field
			strFldName = "ImagesPerRow"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[ImagesPerRow]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_ImagesPerRow"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ImagesPerRow]"
			_table.Fields(strFldName).SortParm = "ImagesPerRow"
			_table.Fields(strFldName).AliasName = "ImagesPerRow"
			_table.Fields(strFldName).ParameterName = "ImagesPerRow"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [RowsPerPage] Field
			strFldName = "RowsPerPage"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[RowsPerPage]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_RowsPerPage"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[RowsPerPage]"
			_table.Fields(strFldName).SortParm = "RowsPerPage"
			_table.Fields(strFldName).AliasName = "RowsPerPage"
			_table.Fields(strFldName).ParameterName = "RowsPerPage"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [ParentPageID] Field
			strFldName = "ParentPageID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[ParentPageID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_ParentPageID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ParentPageID]"
			_table.Fields(strFldName).SortParm = "ParentPageID"
			_table.Fields(strFldName).AliasName = "ParentPageID"
			_table.Fields(strFldName).ParameterName = "ParentPageID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [Active] Field
			strFldName = "Active"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Active]"
			_table.Fields(strFldName).FieldType = 11
			_table.Fields(strFldName).FieldVar = "x_Active"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Active]"
			_table.Fields(strFldName).SortParm = "Active"
			_table.Fields(strFldName).AliasName = "Active"
			_table.Fields(strFldName).ParameterName = "Active"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [GroupID] Field
			strFldName = "GroupID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[GroupID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_GroupID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[GroupID]"
			_table.Fields(strFldName).SortParm = "GroupID"
			_table.Fields(strFldName).AliasName = "GroupID"
			_table.Fields(strFldName).ParameterName = "GroupID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [ModifiedDT] Field
			strFldName = "ModifiedDT"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[ModifiedDT]"
			_table.Fields(strFldName).FieldType = 135
			_table.Fields(strFldName).FieldVar = "x_ModifiedDT"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ModifiedDT]"
			_table.Fields(strFldName).SortParm = "ModifiedDT"
			_table.Fields(strFldName).AliasName = "ModifiedDT"
			_table.Fields(strFldName).ParameterName = "ModifiedDT"		
			_table.Fields(strFldName).QuoteStart = "#"			
			_table.Fields(strFldName).QuoteEnd = "#"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [VersionNo] Field
			strFldName = "VersionNo"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[VersionNo]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_VersionNo"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[VersionNo]"
			_table.Fields(strFldName).SortParm = "VersionNo"
			_table.Fields(strFldName).AliasName = "VersionNo"
			_table.Fields(strFldName).ParameterName = "VersionNo"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [AllowMessage] Field
			strFldName = "AllowMessage"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[AllowMessage]"
			_table.Fields(strFldName).FieldType = 11
			_table.Fields(strFldName).FieldVar = "x_AllowMessage"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[AllowMessage]"
			_table.Fields(strFldName).SortParm = "AllowMessage"
			_table.Fields(strFldName).AliasName = "AllowMessage"
			_table.Fields(strFldName).ParameterName = "AllowMessage"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryID] Field
			strFldName = "SiteCategoryID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[SiteCategoryID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_SiteCategoryID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[SiteCategoryID]"
			_table.Fields(strFldName).SortParm = "SiteCategoryID"
			_table.Fields(strFldName).AliasName = "SiteCategoryID"
			_table.Fields(strFldName).ParameterName = "SiteCategoryID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [SiteCategoryGroupID] Field
			strFldName = "SiteCategoryGroupID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
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
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, Pageinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As Pagekey_base = TryCast(objProfile.MasterKey, Pagekey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[PageID] = " & masterKey.PageID & ""
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

		Public Shared Function LoadKey(ByVal key As Pagekey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field PageID
			Dim k_PageID As String = req.QueryString("PageID")
			If (k_PageID Is Nothing) Then
				messageList.Add("Missing key value: PageID")
			ElseIf (Not DataFormat.CheckInt32(k_PageID))
				messageList.Add("Invalid key value: PageID")
			Else
				key.PageID = Convert.ToInt32(k_PageID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As Pagekey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("PageID=" & key.PageID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
