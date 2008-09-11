Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Web
Imports EW.Data
Imports EW.Data.Utilities
Imports EW.Web
Namespace PMGEN
	Public MustInherit class Contactdal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As Contactrow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As Contactrow ' Advanced Search Parm 2

		' Use Session state
		Dim bUseSession As Boolean 

		' Connection object
		Dim Shared conn As DbConnection = Nothing

		' Table Profile
		Dim objProfile As TableProfile = Nothing

		' ****************
		' *  Constructor
		' ****************

		Public Sub New () 
			Me.New(True)
		End Sub

		' ****************
		' *  Constructor
		' ****************

		Public Sub New (ByVal isUseSession As Boolean)
			bUseSession = isUseSession
			If (bUseSession) Then
				objProfile = CustomProfile.GetTable(Share.ProjectName, Contactinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + Contactinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + Contactinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New Contactrow()
				objAdvancedSearchParm2 = New Contactrow()
			End If
		End Sub

		' **************************
		' *  Default Order Property
		' **************************

		Public Property DefaultOrder As String
			Get 
				Return strDefaultOrder
			End Get 
			Set
				strDefaultOrder = value
				SaveProperties()
			End Set
		End Property

		' *********************************
		' *  Basic Search Keyword Property
		' *********************************

		Public Property BasicSearchKeyword As String
			Get 
				Return strBasicSearchKeyword
			End Get
			Set 
				strBasicSearchKeyword = value
			End Set
		End Property

		' ******************************
		' *  Basic Search Type Property 
		' ******************************

		Public Property BasicSearchType As Core.BasicSearchType 
			Get 
				Return strBasicSearchType
			End Get
			Set 
				strBasicSearchType = value
			End Set
		End Property

		' ***********************************
		' *  Advanced Search Parm 1 Property
		' ***********************************

		Public ReadOnly Property AdvancedSearchParm1 As Contactrow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As Contactrow 
			Get 
				Return objAdvancedSearchParm2
			End Get
		End Property

		' ***********************************
		' *  Save Properties to TableProfile
		' ***********************************

		Private Sub SaveProperties()
			If (bUseSession) Then 

				' Save Default Order Information
				objProfile.DefaultOrder = strDefaultOrder 
				objProfile.DefaultOrderType = strDefaultOrderType 

				' Save Sort Information
				objProfile.OrderBy = strOrderBy 

				' Save Basic Search Information
				objProfile.BasicSearchKeyword = strBasicSearchKeyword 
				objProfile.BasicSearchType = strBasicSearchType 

				' Save Advanced Search Information 
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", Contactinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", Contactinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As Contactrow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Contactrow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), Contactrow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As Contactrow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Contactrow()
				End If
				HttpContext.Current.Session(objectName) = row
			End If
		End Sub

		' **********************
		' *  Handler for Search
		' **********************

		Public Overridable Sub Search() ' override
			SaveProperties()
		End Sub

		' ****************************
		' *  Handler for Clear Search
		' ****************************

		Public Overridable Sub ClearSearch() ' override 

			' Clear Basic Search
			strBasicSearchKeyword = String.Empty 
			strBasicSearchType = Core.BasicSearchType.None

			' Clear Advanced Search 
			objAdvancedSearchParm1 = Nothing 
			objAdvancedSearchParm1 = New Contactrow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New Contactrow() 
			SaveProperties()
		End Sub

		' *********************
		' *  Order By Property
		' *********************

		Public Property OrderBy As String 
			Get	
				Return strOrderBy
			End Get
			Set
				strOrderBy = value
				SaveProperties()
			End Set
		End Property

		' ****************************
		' *  Get Search Filter String
		' ****************************

		Public Overridable Function SearchFilter(ByRef parameters As DbParameterCollection) As String 

			' Construct Where
			Dim strWhere As StringBuilder = New StringBuilder()

			'Basic Search 
			Dim strWhereBasicSearch As String = GetBasicSearch(strBasicSearchKeyword, strBasicSearchType, parameters) 
			If (Not String.IsNullOrEmpty(strWhereBasicSearch)) Then
			   If (strWhere.Length > 0) Then strWhere.Append(" AND ") 
			   strWhere.Append(strWhereBasicSearch) 
			End If

			'Advanced Search 
			Dim strWhereAdvancedSearch As String = GetAdvancedSearch(objAdvancedSearchParm1, objAdvancedSearchParm2, parameters) 
			If (Not String.IsNullOrEmpty(strWhereAdvancedSearch)) Then
				If (strWhere.Length > 0) Then strWhere.Append(" AND ")
				strWhere.Append(strWhereAdvancedSearch) 
			End If
			Return IIf(strWhere.Length > 0, "(" & strWhere.ToString() & ")", String.Empty)
		End Function

		' *******************
		' *  Get Sort String
		' *******************

		Public Overridable Function SortString() As String 
			If (bUseSession) Then
				Return objProfile.OrderBy
			Else
				Return String.Empty
			End If
		End Function

		' *************************************
		' *  Get Master Detail Filter String
		' *************************************

		Public Overridable Function MasterDetailFilter(ByRef parameters As DbParameterCollection) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Return IIf(strWhere.Length > 0, String.Concat("(", strWhere.ToString(), ")"), String.Empty)
		End Function

		' *************************************
		' *  Get Advanced Search Filter String
		' *************************************

		Private Function GetAdvancedSearch(ByVal row1 As Contactrow, ByVal row2 As Contactrow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

				' PrimaryContact Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("PrimaryContact").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([PrimaryContact] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__PrimaryContact"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__PrimaryContact", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.PrimaryContact + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("PrimaryContact").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("PrimaryContact").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([PrimaryContact] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__PrimaryContact"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__PrimaryContact", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.PrimaryContact + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' LogonName Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("LogonName").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([LogonName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__LogonName"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__LogonName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.LogonName + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("LogonName").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("LogonName").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([LogonName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__LogonName"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__LogonName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.LogonName + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' CompanyID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("CompanyID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([CompanyID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__CompanyID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__CompanyID", OleDbType.Integer)
					oParameter.Value = row1.CompanyID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("CompanyID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("CompanyID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([CompanyID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__CompanyID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__CompanyID", OleDbType.Integer)
					oParameter.Value = row2.CompanyID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' GroupID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("GroupID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([GroupID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__GroupID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__GroupID", OleDbType.Integer)
					oParameter.Value = row1.GroupID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("GroupID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("GroupID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([GroupID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__GroupID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__GroupID", OleDbType.Integer)
					oParameter.Value = row2.GroupID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' CreateDT Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("CreateDT").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([CreateDT] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__CreateDT"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__CreateDT", OleDbType.DBTimeStamp)
					oParameter.Value = row1.CreateDT
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("CreateDT").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("CreateDT").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([CreateDT] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__CreateDT"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__CreateDT", OleDbType.DBTimeStamp)
					oParameter.Value = row2.CreateDT
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' TemplatePrefix Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("TemplatePrefix").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([TemplatePrefix] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__TemplatePrefix"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__TemplatePrefix", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.TemplatePrefix + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("TemplatePrefix").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("TemplatePrefix").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([TemplatePrefix] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__TemplatePrefix"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__TemplatePrefix", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.TemplatePrefix + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If
			End If
			Return strWhere.ToString()
		End Function

		' **********************************
		' *  Get Basic Search Filter String
		' **********************************

		Private Function GetBasicSearch(ByVal keyword As String, ByVal searchType As Core.BasicSearchType, ByRef parameters As DbParameterCollection) As String 
			Dim arrKeywords As String() = Nothing
			Dim strOpr As String = String.Empty
			Dim strKeyword As String = keyword
			If (String.IsNullOrEmpty(strKeyword)) Then Return String.Empty
			Select Case searchType
				Case Core.BasicSearchType.None

					' Ignore
					Return String.Empty
				Case Core.BasicSearchType.ExactPhrase
					arrKeywords = New String(1 - 1) {}
					arrKeywords(0) = strKeyword
					strOpr = String.Empty
				Case Core.BasicSearchType.AllWords
					Do While (strKeyword.IndexOf("  ") <> -1)
						strKeyword = strKeyword.Replace("  ", " ")
					Loop
					arrKeywords = strKeyword.Split(" "c)
					strOpr = "AND"
				Case Core.BasicSearchType.AnyWords
					Do While (strKeyword.IndexOf("  ") <> -1)
						strKeyword = strKeyword.Replace("  ", " ")
					Loop
					arrKeywords = strKeyword.Split(" "c)
					strOpr = "OR"
			End Select
			Return BasicSearchSql(arrKeywords, strOpr, parameters)
		End Function

		' ******************************
		' *  Get Keywords Filter String
		' ******************************

		Private Function BasicSearchSql(ByVal arrKeywords As String(), ByVal strOpr As String, ByRef parameters As DbParameterCollection) As String
			Dim strWhere As StringBuilder = New StringBuilder() 
			Dim strKeywordWhere As string = String.Empty
			Dim oParameter As OleDbParameter = Nothing
			Dim i As Integer = 0
			Dim strSearchSql As String = strWhere.ToString()
			If (strSearchSql.EndsWith(" OR ")) Then strSearchSql = strSearchSql.SubString(0, strSearchSql.Length - 4)
			Return IIf(strSearchSql.Length > 0, "(" + strSearchSql + ")", "(1=0)")
		End Function
		Dim strDbErrorMessage As String 

		' ******************************
		' *  DB Error Message Property
		' ******************************

		Public ReadOnly Property DbErrorMessage As String 
			Get 
				Return strDbErrorMessage
			End Get
		End Property

		' **********************************
		' *  Get Data View by filter string
		' **********************************

		Public Function LoadDataView(ByVal filter As String) As DataView 
			Return LoadDataView(filter, Nothing) 
		End Function

		' **********************************
		' *  Get Data View by filter string
		' **********************************

		Public Function LoadDataView(ByVal filter As String, ByRef parameters As DbParameterCollection) As DataView 
			Dim strOrderBy As String = String.Empty 
			Return LoadDataViewFromSql(filter, strOrderBy, parameters) 
		End Function

		' **********************************
		' *  Get Data View by filter string
		' **********************************

		Public Overridable Function LoadRows(ByVal filter As String) As Contactrow()
			Dim rows As Contactrows = LoadList(filter)
			Dim array As Contactrow() = Nothing
			If (rows IsNot Nothing) Then
				array = New Contactrow(rows.Count - 1) {}
				rows.CopyTo(array, 0)
			End If
			Return array
		End Function

		' ************************************************
		' *  Get Data View by filter string and order by 
		' ************************************************

		Private Function LoadDataViewFromSql(ByVal filter As String, ByVal orderBy As String) As DataView 
			Try
				Dim parameters As DbParameterCollection = Db.GetParameterCollection()
				Return LoadDataViewFromSql(filter, orderBy, parameters)
			Catch e As Exception
				Throw
			End Try
		End Function

		' ************************************************
		' *  Get Data View by filter string and order by 
		' ************************************************

		Private Function LoadDataViewFromSql(ByVal filter As String, ByVal orderBy As String, ByRef parameters As DbParameterCollection) As DataView 
			Try
				Dim oInfo As Contactinf = New Contactinf() 
				Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE) 
				Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, filter, orderBy) 
				Return Db.GetDataView(sSql, parameters)
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As Contactrows 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()
			Dim strWhere As String = SearchFilter(parameters) 
			Dim strMasterDetailWhere As String = MasterDetailFilter(parameters)
			If (Not String.IsNullOrEmpty(strMasterDetailWhere)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= strMasterDetailWhere
			End If
			If (Not String.IsNullOrEmpty(filter)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= filter
			End If
			Dim strOrderBy As String = SortString() 
			Try
				Dim dv As DataView = LoadDataViewFromSql(strWhere, strOrderBy, parameters)
				If (dv Is Nothing) Then
					Return Nothing
				End If
				Dim rows As Contactrows = New Contactrows()
				For Each drv As DataRowView In dv
					Dim row As Contactrow = GetRow(drv)
					rows.Add(row)
				Next
				Return IIf(rows.Count > 0, rows, Nothing)
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As Contactrows 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()
			Dim strWhere As String = SearchFilter(parameters) 
			Dim strMasterDetailWhere As String = MasterDetailFilter(parameters)
			If (Not String.IsNullOrEmpty(strMasterDetailWhere)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= strMasterDetailWhere
			End If
			If (Not String.IsNullOrEmpty(filter)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= filter
			End If
			Dim strOrderBy As String = SortString() 
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As Contactrows = New Contactrows()
				For Each drv As DataRowView In dv
					Dim row As Contactrow = GetRow(drv)
					rows.Add(row)
				Next
				Return IIf(rows.Count > 0, rows, Nothing)
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As Contactkey, ByVal filter As String) As Contactrow 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()
			Dim strWhere As String = KeyFilter(key, parameters) 
			If (Not String.IsNullOrEmpty(filter)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= filter
			End If
			Try
				Dim dv As DataView = LoadDataViewFromSql(strWhere, strOrderBy, parameters)
				If (dv Is Nothing OrElse dv.Count = 0) Then
					Return Nothing
				End If
				Return GetRow(dv(0)) ' Return 1st row
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' ***********************************
		' *  Get Rows Count by filter string
		' ***********************************

		Public Function GetRowsCount(ByVal filter As String) As Integer
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()
			Dim strWhere As String = SearchFilter(parameters) 
			Dim strMasterDetailWhere As String = MasterDetailFilter(parameters)
			Dim strOrderBy As String = SortString() 
			If (Not String.IsNullOrEmpty(strMasterDetailWhere)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= strMasterDetailWhere
			End If
			If (Not String.IsNullOrEmpty(filter)) Then
				If (strWhere.Length > 0) Then strWhere &= " AND "
				strWhere &= filter
			End If
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim strSql As String = oSql.GetCountSqlCommand(oInfo.TableInfo, strWhere) 
			Try
				Return Db.GetDataViewCount(strSql, parameters)
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' *************************************************
		' *  Get Page Count by filter string and page size
		' *************************************************

		Public Function GetPageCount(ByVal filter As String, ByVal pageSize As Integer) As Integer
			Try
				Dim intRecordCount As Integer = GetRowsCount(filter)
				If (intRecordCount = 0) Then Return 0
				Dim intPageCount As Integer = intRecordCount/pageSize
				If (intRecordCount Mod pageSize > 0) Then intPageCount += 1
				Return intPageCount
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Load record failed"
				End If
				Throw New ContactDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As Contactrow ' Get a row based on data reader
			Dim row As Contactrow = New Contactrow()
			Try

				' Field ContactID
				If (Not DBNull.Value.Equals(drv("ContactID"))) Then
					row.ContactID = Convert.ToInt32(drv("ContactID"))
				Else
					row.ContactID = Nothing
				End If

				' Field PrimaryContact
				If (Not DBNull.Value.Equals(drv("PrimaryContact"))) Then
					row.PrimaryContact = Convert.ToString(drv("PrimaryContact"))
				Else
					row.PrimaryContact = Nothing
				End If

				' Field LogonName
				If (Not DBNull.Value.Equals(drv("LogonName"))) Then
					row.LogonName = Convert.ToString(drv("LogonName"))
				Else
					row.LogonName = Nothing
				End If

				' Field LogonPassword
				If (Not DBNull.Value.Equals(drv("LogonPassword"))) Then
					row.LogonPassword = Convert.ToString(drv("LogonPassword"))
				Else
					row.LogonPassword = Nothing
				End If

				' Field EMail
				If (Not DBNull.Value.Equals(drv("EMail"))) Then
					row.EMail = Convert.ToString(drv("EMail"))
				Else
					row.EMail = Nothing
				End If

				' Field CompanyID
				If (Not DBNull.Value.Equals(drv("CompanyID"))) Then
					row.CompanyID = Convert.ToInt32(drv("CompanyID"))
				Else
					row.CompanyID = Nothing
				End If

				' Field Active
				If (Not DBNull.Value.Equals(drv("Active"))) Then
					row.Active = Convert.ToBoolean(drv("Active"))
				Else
					row.Active = Nothing
				End If

				' Field GroupID
				If (Not DBNull.Value.Equals(drv("GroupID"))) Then
					row.GroupID = Convert.ToInt32(drv("GroupID"))
				Else
					row.GroupID = Nothing
				End If

				' Field CreateDT
				If (Not DBNull.Value.Equals(drv("CreateDT"))) Then
					row.CreateDT = Convert.ToDateTime(drv("CreateDT"))
				Else
					row.CreateDT = Nothing
				End If

				' Field TemplatePrefix
				If (Not DBNull.Value.Equals(drv("TemplatePrefix"))) Then
					row.TemplatePrefix = Convert.ToString(drv("TemplatePrefix"))
				Else
					row.TemplatePrefix = Nothing
				End If

				' Field FirstName
				If (Not DBNull.Value.Equals(drv("FirstName"))) Then
					row.FirstName = Convert.ToString(drv("FirstName"))
				Else
					row.FirstName = Nothing
				End If

				' Field MiddleInitial
				If (Not DBNull.Value.Equals(drv("MiddleInitial"))) Then
					row.MiddleInitial = Convert.ToString(drv("MiddleInitial"))
				Else
					row.MiddleInitial = Nothing
				End If

				' Field LastName
				If (Not DBNull.Value.Equals(drv("LastName"))) Then
					row.LastName = Convert.ToString(drv("LastName"))
				Else
					row.LastName = Nothing
				End If

				' Field OfficePhone
				If (Not DBNull.Value.Equals(drv("OfficePhone"))) Then
					row.OfficePhone = Convert.ToString(drv("OfficePhone"))
				Else
					row.OfficePhone = Nothing
				End If

				' Field HomePhone
				If (Not DBNull.Value.Equals(drv("HomePhone"))) Then
					row.HomePhone = Convert.ToString(drv("HomePhone"))
				Else
					row.HomePhone = Nothing
				End If

				' Field MobilPhone
				If (Not DBNull.Value.Equals(drv("MobilPhone"))) Then
					row.MobilPhone = Convert.ToString(drv("MobilPhone"))
				Else
					row.MobilPhone = Nothing
				End If

				' Field Pager
				If (Not DBNull.Value.Equals(drv("Pager"))) Then
					row.Pager = Convert.ToString(drv("Pager"))
				Else
					row.Pager = Nothing
				End If

				' Field URL
				If (Not DBNull.Value.Equals(drv("URL"))) Then
					row.URL = Convert.ToString(drv("URL"))
				Else
					row.URL = Nothing
				End If

				' Field Address1
				If (Not DBNull.Value.Equals(drv("Address1"))) Then
					row.Address1 = Convert.ToString(drv("Address1"))
				Else
					row.Address1 = Nothing
				End If

				' Field Address2
				If (Not DBNull.Value.Equals(drv("Address2"))) Then
					row.Address2 = Convert.ToString(drv("Address2"))
				Else
					row.Address2 = Nothing
				End If

				' Field City
				If (Not DBNull.Value.Equals(drv("City"))) Then
					row.City = Convert.ToString(drv("City"))
				Else
					row.City = Nothing
				End If

				' Field State
				If (Not DBNull.Value.Equals(drv("State"))) Then
					row.State = Convert.ToString(drv("State"))
				Else
					row.State = Nothing
				End If

				' Field Country
				If (Not DBNull.Value.Equals(drv("Country"))) Then
					row.Country = Convert.ToString(drv("Country"))
				Else
					row.Country = Nothing
				End If

				' Field PostalCode
				If (Not DBNull.Value.Equals(drv("PostalCode"))) Then
					row.PostalCode = Convert.ToString(drv("PostalCode"))
				Else
					row.PostalCode = Nothing
				End If

				' Field Biography
				If (Not DBNull.Value.Equals(drv("Biography"))) Then
					row.Biography = Convert.ToString(drv("Biography"))
				Else
					row.Biography = Nothing
				End If

				' Field Paid
				If (Not DBNull.Value.Equals(drv("Paid"))) Then
					row.Paid = Convert.ToInt32(drv("Paid"))
				Else
					row.Paid = Nothing
				End If

				' Field email_subscribe
				If (Not DBNull.Value.Equals(drv("email_subscribe"))) Then
					row.email_subscribe = Convert.ToBoolean(drv("email_subscribe"))
				Else
					row.email_subscribe = Nothing
				End If

				' Field RoleID
				If (Not DBNull.Value.Equals(drv("RoleID"))) Then
					row.RoleID = Convert.ToInt32(drv("RoleID"))
				Else
					row.RoleID = Nothing
				End If
			Catch e As Exception
				Throw
			End Try
			Return row
		End Function
		Public Shared Sub OpenConnection()
			If (conn Is Nothing) Then conn = Db.GetConnection()
			If (conn IsNot Nothing AndAlso conn.State = ConnectionState.Closed) Then
				conn.Open()
			End If
		End Sub
		Public Shared Sub CloseAndDisposeConnection()
			If (conn Is Nothing) Then Return
			If (conn.State <> ConnectionState.Closed) Then
				conn.Close()
				conn.Dispose()
				conn = Nothing
			End If
		End Sub
		Public Shared Function IsConnectionOpen() As Boolean
			Return (conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed)
		End Function

		' *************************************
		' *  Loop Up Value from Table for view
		' *************************************

		Public Shared Function LookUpTable(ByVal fieldName As String, ByVal value As String) As String
			Dim sSql As String = String.Empty 
			Dim sDispFld1 As String = String.Empty
			Dim sDispFld2 As String = String.Empty
			Select Case fieldName
				Case "CompanyID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [CompanyID], [CompanyName] FROM [Company] WHERE "
						sSql &= String.Concat("[CompanyID]=", Db.AdjustSql(value), " ORDER BY [CompanyName] Asc")						
						sDispFld1 = "CompanyName"
						sDispFld2 = ""
					End If
				Case "GroupID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [GroupID], [GroupName] FROM [Group] WHERE "
						sSql &= String.Concat("[GroupID]=", Db.AdjustSql(value), " ORDER BY [GroupID] Desc")						
						sDispFld1 = "GroupName"
						sDispFld2 = ""
					End If
				Case "TemplatePrefix"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckString(value)) Then ' validate
						sSql = "SELECT [TemplatePrefix], [Name] FROM [SiteTemplate] WHERE "
						sSql &= String.Concat("[TemplatePrefix]='", Db.AdjustSql(value), "' ORDER BY [Name] Asc")						
						sDispFld1 = "Name"
						sDispFld2 = ""
					End If
				Case "RoleID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [RoleID], [RoleName], [FilterMenu] FROM [role] WHERE "
						sSql &= String.Concat("[RoleID]=", Db.AdjustSql(value), " ORDER BY [RoleName] Asc")						
						sDispFld1 = "RoleName"
						sDispFld2 = "FilterMenu"
					End If
			End Select
			Dim sVal As String = Nothing 
			If (sSql.Length > 0) Then
				sVal = LookUpTableString(sSql, sDispFld1, sDispFld2) 
				Return IIf(sVal Is Nothing, value, sVal)
			Else
				Return value
			End If
		End Function

		' **************************
		' *  Loop Up Value for view
		' **************************

		Public Shared Function LookUpValue(ByVal fieldName As String, ByVal value As String) As String 
			Dim sSql As String = String.Empty 
			Dim sDispFld1 As String = String.Empty 
			Dim sDispFld2 As String = String.Empty 
			Dim ar_FldTagValue As ArrayList = New ArrayList()
			Select Case fieldName
				Case "Active"
					ar_FldTagValue.Add(New String(){"Yes","Yes"})
					ar_FldTagValue.Add(New String(){"No","No"})
				Case "email_subscribe"
					ar_FldTagValue.Add(New String(){"Yes","Yes"})
					ar_FldTagValue.Add(New String(){"No","No"})
			End Select
			Dim i As Integer = 0
			Do While (i < ar_FldTagValue.Count)
				If (CType(ar_FldTagValue(i), String())(0) = value)
					Return CType(ar_FldTagValue(i),String())(1)
				End if
				i += 1
			Loop
			Return value
		End Function

		' ***********************************
		' *  Return string for Loop Up Table
		' ***********************************

		Private Shared Function LookUpTableString(ByVal sql As String, ByVal dispField1 As String, ByVal dispField2 As String) As String 
			Dim sVal As String = Nothing
			If (String.IsNullOrEmpty(sql)) Then Return Nothing
			If (String.IsNullOrEmpty(dispField1)) Then Return Nothing
			Dim isInitialClosed As Boolean = False
			Try
				If (Not IsConnectionOpen()) Then
					isInitialClosed = True
					OpenConnection()
				End If
				Using oDr As DbDataReader = Db.GetDataReader(sql, conn)
					If (oDr Is Nothing) Then Return Nothing
					Do while (oDr.Read())
						If (sVal IsNot Nothing) Then sVal &= ", "
						sVal &= Convert.ToString(oDr(dispField1))
						If (Not String.IsNullOrEmpty(dispField2)) Then
							sVal &= ", " & Convert.ToString(oDr(dispField2))
						End If
					Loop
					oDr.Close()
				End Using
			Catch e As Exception
			Finally
				If (isInitialClosed AndAlso IsConnectionOpen())
					CloseAndDisposeConnection()
				End If
			End Try
			Return sVal
		End Function

		' **********************************************
		' *  Return DataView for Loop Up Table for Edit
		' **********************************************

		Public Shared Function LookUpTable(ByVal fieldName As String) As DataView 
			Dim sSql As String = String.Empty 
			Dim sLnkFld As String = String.Empty 
			Dim sDispFld1 As String = String.Empty 
			Dim sDispFld2 As String = String.Empty 
			Select Case fieldName
				Case "CompanyID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [CompanyID], [CompanyName] FROM [Company]"
					sSql &= " ORDER BY [CompanyName] Asc"
					sLnkFld = "CompanyID"
					sDispFld1 = "CompanyName"
					sDispFld2 = "" 
				Case "GroupID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [GroupID], [GroupName] FROM [Group]"
					sSql &= " ORDER BY [GroupID] Desc"
					sLnkFld = "GroupID"
					sDispFld1 = "GroupName"
					sDispFld2 = "" 
				Case "TemplatePrefix"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [TemplatePrefix], [Name] FROM [SiteTemplate]"
					sSql &= " ORDER BY [Name] Asc"
					sLnkFld = "TemplatePrefix"
					sDispFld1 = "Name"
					sDispFld2 = "" 
				Case "RoleID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [RoleID], [RoleName], [FilterMenu] AS [FilterMenu] FROM [role]"
					sSql &= " ORDER BY [RoleName] Asc"
					sLnkFld = "RoleID"
					sDispFld1 = "RoleName"
					sDispFld2 = "FilterMenu" 
			End Select
			Return IIf(sSql.Length > 0, LookUpTableDataView(sSql, sLnkFld, sDispFld1, sDispFld2), Nothing)
		End Function

		' **********************************************
		' *  Return DataView for Loop Up Table for Edit
		' **********************************************

		Private Shared Function LookUpTableDataView(ByVal sql As String, ByVal linkField As String, ByVal dispField1 As String, ByVal dispField2 As String) As DataView 
			If (String.IsNullOrEmpty(sql)) Then Return Nothing 
			If (String.IsNullOrEmpty(linkField)) Then Return Nothing 
			If (String.IsNullOrEmpty(dispField1)) Then Return Nothing 
			Dim strConnStr As String = Db.ConnStr ' Get Connection String
			Dim oDs As DataSet = New DataSet()
			Dim isInitialClosed As Boolean = False
			Try
				If (Not IsConnectionOpen()) Then
					isInitialClosed = True
					OpenConnection()
				End If
				Using oDa As DbDataAdapter = Db.GetDataAdapter(sql, conn)

					' Fill the DataSet with Data from the DataAdapter object
					oDa.Fill(oDs, "ewDataSet")

					' Create the TextField and ValueField Columns
					oDs.Tables(0).Columns.Add("ewValueField", Type.GetType("System.String"), "[" + linkField + "]")
					If (String.IsNullOrEmpty(dispField2)) Then
						oDs.Tables(0).Columns.Add("ewTextField", Type.GetType("System.String"), "[" + dispField1 + "]")
					Else
						oDs.Tables(0).Columns.Add("ewTextField", Type.GetType("System.String"), "[" + dispField1 + "] + ', ' + [" + dispField2 + "]")
					End If
				End Using
			Catch e As Exception
			Finally
				If (isInitialClosed AndAlso IsConnectionOpen()) Then
					CloseAndDisposeConnection()
				End If
			End Try
			Return IIf(oDs.Tables.Count > 0, oDs.Tables(0).DefaultView, Nothing)
		End Function

		' **************************
		' *  Update Row to Database
		' **************************

		Public Overridable Sub Update(ByVal row As Contactrow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim oTbl As Table = oInfo.TableInfo 
			Dim i As Integer = 1
			Do While (i <= oTbl.Count)
				oTbl.Fields(i).IsGenerateToSql = row.Attribute(oTbl.Fields(i).ParameterName).AllowUpdate 
				i += 1
			Loop
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Update, False, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PrimaryContact").AllowUpdate) Then
							oParameter = New OleDbParameter("PrimaryContact", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PrimaryContact)) Then
								oParameter.Value = DirectCast(row.PrimaryContact, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LogonName").AllowUpdate) Then
							oParameter = New OleDbParameter("LogonName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LogonName)) Then
								oParameter.Value = DirectCast(row.LogonName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LogonPassword").AllowUpdate) Then
							oParameter = New OleDbParameter("LogonPassword", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LogonPassword)) Then
								oParameter.Value = DirectCast(row.LogonPassword, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("EMail").AllowUpdate) Then
							oParameter = New OleDbParameter("EMail", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.EMail)) Then
								oParameter.Value = DirectCast(row.EMail, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Active").AllowUpdate) Then
							oParameter = New OleDbParameter("Active", OleDbType.Boolean)
							If (row.Active.HasValue) Then
								oParameter.Value = DirectCast(row.Active, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("GroupID").AllowUpdate) Then
							oParameter = New OleDbParameter("GroupID", OleDbType.Integer)
							If (row.GroupID.HasValue) Then
								oParameter.Value = DirectCast(row.GroupID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CreateDT").AllowUpdate) Then
							oParameter = New OleDbParameter("CreateDT", OleDbType.DBTimeStamp)
							If (row.CreateDT.HasValue) Then
								oParameter.Value = DirectCast(row.CreateDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("TemplatePrefix").AllowUpdate) Then
							oParameter = New OleDbParameter("TemplatePrefix", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.TemplatePrefix)) Then
								oParameter.Value = DirectCast(row.TemplatePrefix, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("FirstName").AllowUpdate) Then
							oParameter = New OleDbParameter("FirstName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.FirstName)) Then
								oParameter.Value = DirectCast(row.FirstName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MiddleInitial").AllowUpdate) Then
							oParameter = New OleDbParameter("MiddleInitial", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.MiddleInitial)) Then
								oParameter.Value = DirectCast(row.MiddleInitial, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LastName").AllowUpdate) Then
							oParameter = New OleDbParameter("LastName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LastName)) Then
								oParameter.Value = DirectCast(row.LastName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("OfficePhone").AllowUpdate) Then
							oParameter = New OleDbParameter("OfficePhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.OfficePhone)) Then
								oParameter.Value = DirectCast(row.OfficePhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("HomePhone").AllowUpdate) Then
							oParameter = New OleDbParameter("HomePhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.HomePhone)) Then
								oParameter.Value = DirectCast(row.HomePhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MobilPhone").AllowUpdate) Then
							oParameter = New OleDbParameter("MobilPhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.MobilPhone)) Then
								oParameter.Value = DirectCast(row.MobilPhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Pager").AllowUpdate) Then
							oParameter = New OleDbParameter("Pager", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Pager)) Then
								oParameter.Value = DirectCast(row.Pager, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.URL)) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Address1").AllowUpdate) Then
							oParameter = New OleDbParameter("Address1", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Address1)) Then
								oParameter.Value = DirectCast(row.Address1, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Address2").AllowUpdate) Then
							oParameter = New OleDbParameter("Address2", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Address2)) Then
								oParameter.Value = DirectCast(row.Address2, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("City").AllowUpdate) Then
							oParameter = New OleDbParameter("City", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.City)) Then
								oParameter.Value = DirectCast(row.City, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("State").AllowUpdate) Then
							oParameter = New OleDbParameter("State", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.State)) Then
								oParameter.Value = DirectCast(row.State, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Country").AllowUpdate) Then
							oParameter = New OleDbParameter("Country", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Country)) Then
								oParameter.Value = DirectCast(row.Country, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PostalCode").AllowUpdate) Then
							oParameter = New OleDbParameter("PostalCode", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PostalCode)) Then
								oParameter.Value = DirectCast(row.PostalCode, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Biography").AllowUpdate) Then
							oParameter = New OleDbParameter("Biography", OleDbType.LongVarWChar)
							If (Not String.IsNullOrEmpty(row.Biography)) Then
								oParameter.Value = DirectCast(row.Biography, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Paid").AllowUpdate) Then
							oParameter = New OleDbParameter("Paid", OleDbType.Integer)
							If (row.Paid.HasValue) Then
								oParameter.Value = DirectCast(row.Paid, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("email_subscribe").AllowUpdate) Then
							oParameter = New OleDbParameter("email_subscribe", OleDbType.Boolean)
							If (row.email_subscribe.HasValue) Then
								oParameter.Value = DirectCast(row.email_subscribe, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("RoleID").AllowUpdate) Then
							oParameter = New OleDbParameter("RoleID", OleDbType.Integer)
							If (row.RoleID.HasValue) Then
								oParameter.Value = DirectCast(row.RoleID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
						oParameter.Value = row.ContactID
						oCmd.Parameters.Add(oParameter)

						' Open connection
						oConn.Open()

						' Perform the Update
						oCmd.ExecuteNonQuery()

						' Update Successful
						strDbErrorMessage = String.Empty
						bStatus = True ' Successful
					End Using
				End Using
			Catch e As Exception
				If (TypeOf e Is OleDbException) Then
					strDbErrorMessage = Db.DataErrorMessage(CType(e,OleDbException))
				Else
					strDbErrorMessage = e.Message
				End If
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Update record failed"
				End If
				bStatus = false ' Failed
			Finally
			End Try
			If (Not bStatus) Then
				Throw New ContactDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As Contactrow) As Contactrow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)

			' Construct SQL statement
			Dim oTbl As Table = oInfo.TableInfo 
			Dim i As Integer = 1
			Do While (i <= oTbl.Count)
				oTbl.Fields(i).IsGenerateToSql = row.Attribute(oTbl.Fields(i).ParameterName).AllowUpdate
				i += 1
			Loop
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Insert, false, String.Empty, String.Empty)

			' Create a New Connection object using the Connection String
			Using oConn As DbConnection = Db.GetConnection()

				' Create a New Command object
				Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)
					Try
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PrimaryContact").AllowUpdate) Then
							oParameter = New OleDbParameter("PrimaryContact", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PrimaryContact)) Then
								oParameter.Value = DirectCast(row.PrimaryContact, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LogonName").AllowUpdate) Then
							oParameter = New OleDbParameter("LogonName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LogonName)) Then
								oParameter.Value = DirectCast(row.LogonName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LogonPassword").AllowUpdate) Then
							oParameter = New OleDbParameter("LogonPassword", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LogonPassword)) Then
								oParameter.Value = DirectCast(row.LogonPassword, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("EMail").AllowUpdate) Then
							oParameter = New OleDbParameter("EMail", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.EMail)) Then
								oParameter.Value = DirectCast(row.EMail, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Active").AllowUpdate) Then
							oParameter = New OleDbParameter("Active", OleDbType.Boolean)
							If (row.Active.HasValue) Then
								oParameter.Value = DirectCast(row.Active, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("GroupID").AllowUpdate) Then
							oParameter = New OleDbParameter("GroupID", OleDbType.Integer)
							If (row.GroupID.HasValue) Then
								oParameter.Value = DirectCast(row.GroupID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CreateDT").AllowUpdate) Then
							oParameter = New OleDbParameter("CreateDT", OleDbType.DBTimeStamp)
							If (row.CreateDT.HasValue) Then
								oParameter.Value = DirectCast(row.CreateDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("TemplatePrefix").AllowUpdate) Then
							oParameter = New OleDbParameter("TemplatePrefix", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.TemplatePrefix)) Then
								oParameter.Value = DirectCast(row.TemplatePrefix, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("FirstName").AllowUpdate) Then
							oParameter = New OleDbParameter("FirstName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.FirstName)) Then
								oParameter.Value = DirectCast(row.FirstName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MiddleInitial").AllowUpdate) Then
							oParameter = New OleDbParameter("MiddleInitial", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.MiddleInitial)) Then
								oParameter.Value = DirectCast(row.MiddleInitial, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LastName").AllowUpdate) Then
							oParameter = New OleDbParameter("LastName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.LastName)) Then
								oParameter.Value = DirectCast(row.LastName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("OfficePhone").AllowUpdate) Then
							oParameter = New OleDbParameter("OfficePhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.OfficePhone)) Then
								oParameter.Value = DirectCast(row.OfficePhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("HomePhone").AllowUpdate) Then
							oParameter = New OleDbParameter("HomePhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.HomePhone)) Then
								oParameter.Value = DirectCast(row.HomePhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MobilPhone").AllowUpdate) Then
							oParameter = New OleDbParameter("MobilPhone", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.MobilPhone)) Then
								oParameter.Value = DirectCast(row.MobilPhone, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Pager").AllowUpdate) Then
							oParameter = New OleDbParameter("Pager", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Pager)) Then
								oParameter.Value = DirectCast(row.Pager, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.URL)) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Address1").AllowUpdate) Then
							oParameter = New OleDbParameter("Address1", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Address1)) Then
								oParameter.Value = DirectCast(row.Address1, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Address2").AllowUpdate) Then
							oParameter = New OleDbParameter("Address2", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Address2)) Then
								oParameter.Value = DirectCast(row.Address2, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("City").AllowUpdate) Then
							oParameter = New OleDbParameter("City", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.City)) Then
								oParameter.Value = DirectCast(row.City, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("State").AllowUpdate) Then
							oParameter = New OleDbParameter("State", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.State)) Then
								oParameter.Value = DirectCast(row.State, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Country").AllowUpdate) Then
							oParameter = New OleDbParameter("Country", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Country)) Then
								oParameter.Value = DirectCast(row.Country, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PostalCode").AllowUpdate) Then
							oParameter = New OleDbParameter("PostalCode", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PostalCode)) Then
								oParameter.Value = DirectCast(row.PostalCode, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Biography").AllowUpdate) Then
							oParameter = New OleDbParameter("Biography", OleDbType.LongVarWChar)
							If (Not String.IsNullOrEmpty(row.Biography)) Then
								oParameter.Value = DirectCast(row.Biography, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Paid").AllowUpdate) Then
							oParameter = New OleDbParameter("Paid", OleDbType.Integer)
							If (row.Paid.HasValue) Then
								oParameter.Value = DirectCast(row.Paid, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("email_subscribe").AllowUpdate) Then
							oParameter = New OleDbParameter("email_subscribe", OleDbType.Boolean)
							If (row.email_subscribe.HasValue) Then
								oParameter.Value = DirectCast(row.email_subscribe, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("RoleID").AllowUpdate) Then
							oParameter = New OleDbParameter("RoleID", OleDbType.Integer)
							If (row.RoleID.HasValue) Then
								oParameter.Value = DirectCast(row.RoleID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Open Connection
						oConn.Open()

						' Begin Transaction
						oTrans = oConn.BeginTransaction()

						' Perform the Insert
						oCmd.Transaction = oTrans
						oCmd.ExecuteNonQuery()

						' Commit Transaction
						oTrans.Commit()

						' Add Successful
						strDbErrorMessage = String.Empty
						bStatus = True ' Successful
						row.ContactID = Db.LastInsertedID(oConn) ' load auto-generated ID
						Return row ' Return row object
					Catch e As OleDbException
						strDbErrorMessage = Db.DataErrorMessage(e)
						If (String.IsNullOrEmpty(strDbErrorMessage)) Then
							strDbErrorMessage = "Add record failed"
						End If
						If (oTrans IsNot Nothing) Then oTrans.Rollback()
						bStatus = false ' Failed
					Catch e As Exception
						strDbErrorMessage = e.Message
						If (String.IsNullOrEmpty(strDbErrorMessage)) Then
							strDbErrorMessage = "Add record failed"
						End If
						oTrans.Rollback()
						bStatus = false ' Failed
					Finally

						' Close Connection
						If (oConn IsNot Nothing AndAlso oConn.State <> ConnectionState.Closed) Then oConn.Close()
					End Try
				End Using
			End Using
			If (Not bStatus) Then
				Throw New ContactDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal ContactID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
						oParameter.Value = ContactID
						oCmd.Parameters.Add(oParameter)
						oConn.Open()
						oCmd.ExecuteNonQuery()

						' Delete Successful
						strDbErrorMessage = String.Empty
						bStatus = True ' Successful
					End USing
				End Using
			Catch e As OleDbException
				strDbErrorMessage = Db.DataErrorMessage(e)
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then 
					strDbErrorMessage = "Delete record failed"
				End If
				bStatus = false ' Failed
			Finally
			End Try
			If (Not bStatus) Then
				Throw New ContactDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As Contactrow )
			Me.Delete(CType(row.ContactID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As Contactinf = New Contactinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, False, strWhere, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn, parameters)
						oConn.Open()
						oCmd.ExecuteNonQuery()

						' Delete Successful
						strDbErrorMessage = String.Empty
						bStatus = True ' Successful
					End Using
				End Using
			Catch e As OleDbException
				strDbErrorMessage = Db.DataErrorMessage(e)
				If (String.IsNullOrEmpty(strDbErrorMessage)) Then
					strDbErrorMessage = "Delete record failed"
				End If
				bStatus = false ' Failed
			Finally
			End Try
			If (Not bStatus) Then
				Throw New ContactDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Get key filter by keys
		' **************************

		Public Overridable Function KeyFilter(ByVal keys As ArrayList) As String 
			If (keys Is Nothing) Then Return String.Empty
			Dim strWhere As StringBuilder = New StringBuilder() 
			Dim i As Integer = 0
			Do While (i < keys.Count)
				If (strWhere.Length > 0) Then strWhere.Append(" OR ") 
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Contactkey)), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' **************************
		' *  Get key filter by keys
		' **************************

		Public Overridable Function KeyFilter(ByVal keys As ArrayList, ByRef parameters As DbParameterCollection) As String 
			If (keys Is Nothing) Then Return String.Empty
			Dim strWhere As StringBuilder = New StringBuilder() 
			Dim i As Integer = 0
			Do While (i < keys.Count)
				If (strWhere.Length > 0) Then strWhere.Append(" OR ") 
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Contactkey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Contactkey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[ContactID] = ", key.ContactID)) ' [ContactID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Contactkey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ContactID] = " & Database.GetSqlParameterName(Db.DbType, "key__ContactID"))
			oParameter = New OleDbParameter("key__ContactID", OleDbType.Integer)
			oParameter.Value = key.ContactID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Contactkey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ContactID] = " & Database.GetSqlParameterName(Db.DbType, "key__ContactID" & index.ToString())) ' [ContactID] field
			oParameter = New OleDbParameter("key__ContactID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.ContactID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class ContactDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
