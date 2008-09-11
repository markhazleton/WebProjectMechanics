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
	''' Summary description for Contactinf_base
	''' </summary>

	Public MustInherit Class Contactinf_base
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

		Public Shared ReadOnly TableVar As String = "Contact"

		' ***********************
		' * Constructor
		' ***********************

		Public Sub New()
			Dim strFldName As String
			_table.TableName = "[Contact]"
            _table.DefaultFilter = "[CompanyID]=" & System.Web.HttpContext.Current.Session.Item("CompanyID") & " "
            _table.GroupBy = String.Empty
            _table.Having = String.Empty
            _table.DefaultOrderBy = "[PrimaryContact] ASC"

			' [ContactID] Field
			strFldName = "ContactID"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = True
			_table.Fields(strFldName).IsPrimaryKey = True
			_table.Fields(strFldName).FieldName = "[ContactID]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_ContactID"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[ContactID]"
			_table.Fields(strFldName).SortParm = "ContactID"
			_table.Fields(strFldName).AliasName = "ContactID"
			_table.Fields(strFldName).ParameterName = "ContactID"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PrimaryContact] Field
			strFldName = "PrimaryContact"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PrimaryContact]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PrimaryContact"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PrimaryContact]"
			_table.Fields(strFldName).SortParm = "PrimaryContact"
			_table.Fields(strFldName).AliasName = "PrimaryContact"
			_table.Fields(strFldName).ParameterName = "PrimaryContact"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [LogonName] Field
			strFldName = "LogonName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[LogonName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_LogonName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[LogonName]"
			_table.Fields(strFldName).SortParm = "LogonName"
			_table.Fields(strFldName).AliasName = "LogonName"
			_table.Fields(strFldName).ParameterName = "LogonName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = True

			' [LogonPassword] Field
			strFldName = "LogonPassword"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[LogonPassword]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_LogonPassword"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[LogonPassword]"
			_table.Fields(strFldName).SortParm = "LogonPassword"
			_table.Fields(strFldName).AliasName = "LogonPassword"
			_table.Fields(strFldName).ParameterName = "LogonPassword"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = True

			' [EMail] Field
			strFldName = "EMail"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[EMail]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_EMail"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[EMail]"
			_table.Fields(strFldName).SortParm = "EMail"
			_table.Fields(strFldName).AliasName = "EMail"
			_table.Fields(strFldName).ParameterName = "EMail"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
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

			' [CreateDT] Field
			strFldName = "CreateDT"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[CreateDT]"
			_table.Fields(strFldName).FieldType = 135
			_table.Fields(strFldName).FieldVar = "x_CreateDT"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[CreateDT]"
			_table.Fields(strFldName).SortParm = "CreateDT"
			_table.Fields(strFldName).AliasName = "CreateDT"
			_table.Fields(strFldName).ParameterName = "CreateDT"		
			_table.Fields(strFldName).QuoteStart = "#"			
			_table.Fields(strFldName).QuoteEnd = "#"
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [TemplatePrefix] Field
			strFldName = "TemplatePrefix"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
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
			_table.Fields(strFldName).IsList = True
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = True
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = True 
			_table.Fields(strFldName).IsRegister = False

			' [FirstName] Field
			strFldName = "FirstName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[FirstName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_FirstName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[FirstName]"
			_table.Fields(strFldName).SortParm = "FirstName"
			_table.Fields(strFldName).AliasName = "FirstName"
			_table.Fields(strFldName).ParameterName = "FirstName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [MiddleInitial] Field
			strFldName = "MiddleInitial"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[MiddleInitial]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_MiddleInitial"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[MiddleInitial]"
			_table.Fields(strFldName).SortParm = "MiddleInitial"
			_table.Fields(strFldName).AliasName = "MiddleInitial"
			_table.Fields(strFldName).ParameterName = "MiddleInitial"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [LastName] Field
			strFldName = "LastName"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[LastName]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_LastName"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[LastName]"
			_table.Fields(strFldName).SortParm = "LastName"
			_table.Fields(strFldName).AliasName = "LastName"
			_table.Fields(strFldName).ParameterName = "LastName"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [OfficePhone] Field
			strFldName = "OfficePhone"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[OfficePhone]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_OfficePhone"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[OfficePhone]"
			_table.Fields(strFldName).SortParm = "OfficePhone"
			_table.Fields(strFldName).AliasName = "OfficePhone"
			_table.Fields(strFldName).ParameterName = "OfficePhone"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [HomePhone] Field
			strFldName = "HomePhone"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[HomePhone]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_HomePhone"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[HomePhone]"
			_table.Fields(strFldName).SortParm = "HomePhone"
			_table.Fields(strFldName).AliasName = "HomePhone"
			_table.Fields(strFldName).ParameterName = "HomePhone"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [MobilPhone] Field
			strFldName = "MobilPhone"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[MobilPhone]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_MobilPhone"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[MobilPhone]"
			_table.Fields(strFldName).SortParm = "MobilPhone"
			_table.Fields(strFldName).AliasName = "MobilPhone"
			_table.Fields(strFldName).ParameterName = "MobilPhone"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Pager] Field
			strFldName = "Pager"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Pager]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Pager"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Pager]"
			_table.Fields(strFldName).SortParm = "Pager"
			_table.Fields(strFldName).AliasName = "Pager"
			_table.Fields(strFldName).ParameterName = "Pager"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Address1] Field
			strFldName = "Address1"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Address1]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Address1"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Address1]"
			_table.Fields(strFldName).SortParm = "Address1"
			_table.Fields(strFldName).AliasName = "Address1"
			_table.Fields(strFldName).ParameterName = "Address1"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Address2] Field
			strFldName = "Address2"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Address2]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Address2"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Address2]"
			_table.Fields(strFldName).SortParm = "Address2"
			_table.Fields(strFldName).AliasName = "Address2"
			_table.Fields(strFldName).ParameterName = "Address2"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [State] Field
			strFldName = "State"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[State]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_State"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[State]"
			_table.Fields(strFldName).SortParm = "State"
			_table.Fields(strFldName).AliasName = "State"
			_table.Fields(strFldName).ParameterName = "State"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Country] Field
			strFldName = "Country"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Country]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_Country"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Country]"
			_table.Fields(strFldName).SortParm = "Country"
			_table.Fields(strFldName).AliasName = "Country"
			_table.Fields(strFldName).ParameterName = "Country"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [PostalCode] Field
			strFldName = "PostalCode"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[PostalCode]"
			_table.Fields(strFldName).FieldType = 202
			_table.Fields(strFldName).FieldVar = "x_PostalCode"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[PostalCode]"
			_table.Fields(strFldName).SortParm = "PostalCode"
			_table.Fields(strFldName).AliasName = "PostalCode"
			_table.Fields(strFldName).ParameterName = "PostalCode"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Biography] Field
			strFldName = "Biography"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Biography]"
			_table.Fields(strFldName).FieldType = 203
			_table.Fields(strFldName).FieldVar = "x_Biography"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Biography]"
			_table.Fields(strFldName).SortParm = "Biography"
			_table.Fields(strFldName).AliasName = "Biography"
			_table.Fields(strFldName).ParameterName = "Biography"		
			_table.Fields(strFldName).QuoteStart = "'"			
			_table.Fields(strFldName).QuoteEnd = "'"
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [Paid] Field
			strFldName = "Paid"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[Paid]"
			_table.Fields(strFldName).FieldType = 3
			_table.Fields(strFldName).FieldVar = "x_Paid"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[Paid]"
			_table.Fields(strFldName).SortParm = "Paid"
			_table.Fields(strFldName).AliasName = "Paid"
			_table.Fields(strFldName).ParameterName = "Paid"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False

			' [email_subscribe] Field
			strFldName = "email_subscribe"
			_table.Add(strFldName)
			_table.Fields(strFldName).IsAutoIncrement = False
			_table.Fields(strFldName).IsPrimaryKey = False
			_table.Fields(strFldName).FieldName = "[email_subscribe]"
			_table.Fields(strFldName).FieldType = 11
			_table.Fields(strFldName).FieldVar = "x_email_subscribe"
			_table.Fields(strFldName).FieldUploadPath = ""
			_table.Fields(strFldName).SortName = "[email_subscribe]"
			_table.Fields(strFldName).SortParm = "email_subscribe"
			_table.Fields(strFldName).AliasName = "email_subscribe"
			_table.Fields(strFldName).ParameterName = "email_subscribe"		
			_table.Fields(strFldName).QuoteStart = ""			
			_table.Fields(strFldName).QuoteEnd = ""
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = False
			_table.Fields(strFldName).IsEdit = False
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = False 
			_table.Fields(strFldName).IsSearch = False 
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
			_table.Fields(strFldName).IsList = False
			_table.Fields(strFldName).IsView = True
			_table.Fields(strFldName).IsEdit = True
			_table.Fields(strFldName).IsDelete = False
			_table.Fields(strFldName).IsAdd = True 
			_table.Fields(strFldName).IsSearch = False 
			_table.Fields(strFldName).IsRegister = False
		End Sub

		''' <summary>
		''' Gets User Filter 
		''' </summary>

		Public Shared Function GetUserFilter() As String
			Dim objProfile As TableProfile = CustomProfile.GetTable(Share.ProjectName, Contactinf.TableVar)
			Dim strFilter As string = IIf(objProfile.SearchWhere.Length > 0, "(" & objProfile.SearchWhere & ")", String.Empty)
			Dim strTemp As String = String.Empty
			If (objProfile.isCollapsed AndAlso objProfile.MasterKey IsNot Nothing) Then
				strTemp = String.Empty
				Dim masterKey As Contactkey_base = TryCast(objProfile.MasterKey, Contactkey_base)
				If (strTemp.Length >0) Then strTemp &= " AND "
				strTemp &= "[ContactID] = " & masterKey.ContactID & ""
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

		Public Shared Function LoadKey(ByVal key As Contactkey_base) As ArrayList 
			Dim messageList As ArrayList = New ArrayList()
			Dim req As HttpRequest = HttpContext.Current.Request ' get current Http request object

			' Check Key field ContactID
			Dim k_ContactID As String = req.QueryString("ContactID")
			If (k_ContactID Is Nothing) Then
				messageList.Add("Missing key value: ContactID")
			ElseIf (Not DataFormat.CheckInt32(k_ContactID))
				messageList.Add("Invalid key value: ContactID")
			Else
				key.ContactID = Convert.ToInt32(k_ContactID)
			End If
			Return IIf((messageList.Count = 0), Nothing, messageList)
		End Function

		''' <summary>
		''' Builds a query string from key object
		''' </summary>

		Public Shared Function KeyUrl(ByVal key As Contactkey_base ) As String
			Dim strUrl As StringBuilder = New StringBuilder()

			' Add key field ID
				If (strUrl.Length > 0) Then strUrl.Append("&")
				strUrl.Append("ContactID=" & key.ContactID.ToString())
			Return strUrl.ToString()
		End Function
	End Class
End Namespace
