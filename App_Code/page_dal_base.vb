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
	Public MustInherit class Pagedal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As Pagerow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As Pagerow ' Advanced Search Parm 2

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
				objProfile = CustomProfile.GetTable(Share.ProjectName, Pageinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + Pageinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + Pageinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New Pagerow()
				objAdvancedSearchParm2 = New Pagerow()
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

		Public ReadOnly Property AdvancedSearchParm1 As Pagerow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As Pagerow 
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
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", Pageinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", Pageinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As Pagerow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Pagerow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), Pagerow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As Pagerow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Pagerow()
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
			objAdvancedSearchParm1 = New Pagerow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New Pagerow() 
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

		Private Function GetAdvancedSearch(ByVal row1 As Pagerow, ByVal row2 As Pagerow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

				' PageName Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("PageName").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([PageName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__PageName"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__PageName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.PageName + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("PageName").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("PageName").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([PageName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__PageName"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__PageName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.PageName + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' PageTypeID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("PageTypeID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([PageTypeID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__PageTypeID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__PageTypeID", OleDbType.Integer)
					oParameter.Value = row1.PageTypeID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("PageTypeID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("PageTypeID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([PageTypeID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__PageTypeID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__PageTypeID", OleDbType.Integer)
					oParameter.Value = row2.PageTypeID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' ParentPageID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ParentPageID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ParentPageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ParentPageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ParentPageID", OleDbType.Integer)
					oParameter.Value = row1.ParentPageID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ParentPageID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ParentPageID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ParentPageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ParentPageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ParentPageID", OleDbType.Integer)
					oParameter.Value = row2.ParentPageID
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

				' SiteCategoryID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("SiteCategoryID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([SiteCategoryID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__SiteCategoryID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__SiteCategoryID", OleDbType.Integer)
					oParameter.Value = row1.SiteCategoryID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("SiteCategoryID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("SiteCategoryID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([SiteCategoryID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__SiteCategoryID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__SiteCategoryID", OleDbType.Integer)
					oParameter.Value = row2.SiteCategoryID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' SiteCategoryGroupID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("SiteCategoryGroupID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([SiteCategoryGroupID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__SiteCategoryGroupID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__SiteCategoryGroupID", OleDbType.Integer)
					oParameter.Value = row1.SiteCategoryGroupID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("SiteCategoryGroupID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("SiteCategoryGroupID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([SiteCategoryGroupID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__SiteCategoryGroupID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__SiteCategoryGroupID", OleDbType.Integer)
					oParameter.Value = row2.SiteCategoryGroupID
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

		Public Overridable Function LoadRows(ByVal filter As String) As Pagerow()
			Dim rows As Pagerows = LoadList(filter)
			Dim array As Pagerow() = Nothing
			If (rows IsNot Nothing) Then
				array = New Pagerow(rows.Count - 1) {}
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
				Dim oInfo As Pageinf = New Pageinf() 
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
				Throw New PageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As Pagerows 
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
				Dim rows As Pagerows = New Pagerows()
				For Each drv As DataRowView In dv
					Dim row As Pagerow = GetRow(drv)
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
				Throw New PageDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As Pagerows 
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
			Dim oInfo As Pageinf = New Pageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As Pagerows = New Pagerows()
				For Each drv As DataRowView In dv
					Dim row As Pagerow = GetRow(drv)
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
				Throw New PageDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As Pagekey, ByVal filter As String) As Pagerow 
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
				Throw New PageDataException(strDbErrorMessage)
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
			Dim oInfo As Pageinf = New Pageinf()
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
				Throw New PageDataException(strDbErrorMessage)
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
				Throw New PageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As Pagerow ' Get a row based on data reader
			Dim row As Pagerow = New Pagerow()
			Try

				' Field PageID
				If (Not DBNull.Value.Equals(drv("PageID"))) Then
					row.PageID = Convert.ToInt32(drv("PageID"))
				Else
					row.PageID = Nothing
				End If

				' Field PageOrder
				If (Not DBNull.Value.Equals(drv("PageOrder"))) Then
					row.PageOrder = Convert.ToInt32(drv("PageOrder"))
				Else
					row.PageOrder = Nothing
				End If

				' Field PageName
				If (Not DBNull.Value.Equals(drv("PageName"))) Then
					row.PageName = Convert.ToString(drv("PageName"))
				Else
					row.PageName = Nothing
				End If

				' Field PageTitle
				If (Not DBNull.Value.Equals(drv("PageTitle"))) Then
					row.PageTitle = Convert.ToString(drv("PageTitle"))
				Else
					row.PageTitle = Nothing
				End If

				' Field PageDescription
				If (Not DBNull.Value.Equals(drv("PageDescription"))) Then
					row.PageDescription = Convert.ToString(drv("PageDescription"))
				Else
					row.PageDescription = Nothing
				End If

				' Field PageKeywords
				If (Not DBNull.Value.Equals(drv("PageKeywords"))) Then
					row.PageKeywords = Convert.ToString(drv("PageKeywords"))
				Else
					row.PageKeywords = Nothing
				End If

				' Field PageTypeID
				If (Not DBNull.Value.Equals(drv("PageTypeID"))) Then
					row.PageTypeID = Convert.ToInt32(drv("PageTypeID"))
				Else
					row.PageTypeID = Nothing
				End If

				' Field ImagesPerRow
				If (Not DBNull.Value.Equals(drv("ImagesPerRow"))) Then
					row.ImagesPerRow = Convert.ToInt32(drv("ImagesPerRow"))
				Else
					row.ImagesPerRow = Nothing
				End If

				' Field RowsPerPage
				If (Not DBNull.Value.Equals(drv("RowsPerPage"))) Then
					row.RowsPerPage = Convert.ToInt32(drv("RowsPerPage"))
				Else
					row.RowsPerPage = Nothing
				End If

				' Field ParentPageID
				If (Not DBNull.Value.Equals(drv("ParentPageID"))) Then
					row.ParentPageID = Convert.ToInt32(drv("ParentPageID"))
				Else
					row.ParentPageID = Nothing
				End If

				' Field Active
				If (Not DBNull.Value.Equals(drv("Active"))) Then
					row.Active = Convert.ToBoolean(drv("Active"))
				Else
					row.Active = Nothing
				End If

				' Field CompanyID
				If (Not DBNull.Value.Equals(drv("CompanyID"))) Then
					row.CompanyID = Convert.ToInt32(drv("CompanyID"))
				Else
					row.CompanyID = Nothing
				End If

				' Field PageFileName
				If (Not DBNull.Value.Equals(drv("PageFileName"))) Then
					row.PageFileName = Convert.ToString(drv("PageFileName"))
				Else
					row.PageFileName = Nothing
				End If

				' Field GroupID
				If (Not DBNull.Value.Equals(drv("GroupID"))) Then
					row.GroupID = Convert.ToInt32(drv("GroupID"))
				Else
					row.GroupID = Nothing
				End If

				' Field ModifiedDT
				If (Not DBNull.Value.Equals(drv("ModifiedDT"))) Then
					row.ModifiedDT = Convert.ToDateTime(drv("ModifiedDT"))
				Else
					row.ModifiedDT = Nothing
				End If

				' Field VersionNo
				If (Not DBNull.Value.Equals(drv("VersionNo"))) Then
					row.VersionNo = Convert.ToInt32(drv("VersionNo"))
				Else
					row.VersionNo = Nothing
				End If

				' Field AllowMessage
				If (Not DBNull.Value.Equals(drv("AllowMessage"))) Then
					row.AllowMessage = Convert.ToBoolean(drv("AllowMessage"))
				Else
					row.AllowMessage = Nothing
				End If

				' Field SiteCategoryID
				If (Not DBNull.Value.Equals(drv("SiteCategoryID"))) Then
					row.SiteCategoryID = Convert.ToInt32(drv("SiteCategoryID"))
				Else
					row.SiteCategoryID = Nothing
				End If

				' Field SiteCategoryGroupID
				If (Not DBNull.Value.Equals(drv("SiteCategoryGroupID"))) Then
					row.SiteCategoryGroupID = Convert.ToInt32(drv("SiteCategoryGroupID"))
				Else
					row.SiteCategoryGroupID = Nothing
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
				Case "PageTypeID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [PageTypeID], [PageTypeCD] FROM [PageType] WHERE "
						sSql &= String.Concat("[PageTypeID]=", Db.AdjustSql(value), " ORDER BY [PageTypeCD] Asc")						
						sDispFld1 = "PageTypeCD"
						sDispFld2 = ""
					End If
				Case "ParentPageID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [PageID], [PageName] FROM [Page] WHERE "
						sSql &= String.Concat("[PageID]=", Db.AdjustSql(value), " ORDER BY [PageName] Asc")						
						sDispFld1 = "PageName"
						sDispFld2 = ""
					End If
				Case "CompanyID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [CompanyID], [CompanyName] FROM [Company] WHERE "
						sSql &= String.Concat("[CompanyID]=", Db.AdjustSql(value), " ORDER BY [CompanyName] ")						
						sDispFld1 = "CompanyName"
						sDispFld2 = ""
					End If
				Case "GroupID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [GroupID], [GroupName] FROM [Group] WHERE "
						sSql &= String.Concat("[GroupID]=", Db.AdjustSql(value), " ORDER BY [GroupID] Asc")						
						sDispFld1 = "GroupName"
						sDispFld2 = ""
					End If
				Case "SiteCategoryID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE "
						sSql &= "[SiteCategoryTypeID]=" & System.Web.HttpContext.Current.Session.Item("SiteCategoryTypeID") & " " & " AND "
						sSql &= String.Concat("[SiteCategoryID]=", Db.AdjustSql(value), " ORDER BY [CategoryName] Asc")						
						sDispFld1 = "CategoryName"
						sDispFld2 = ""
					End If
				Case "SiteCategoryGroupID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT DISTINCT [SiteCategoryGroupID], [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE "
						sSql &= String.Concat("[SiteCategoryGroupID]=", Db.AdjustSql(value), "")						
						sDispFld1 = "SiteCategoryGroupNM"
						sDispFld2 = ""
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
				Case "AllowMessage"
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
				Case "PageTypeID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [PageTypeID], [PageTypeCD] FROM [PageType]"
					sSql &= " ORDER BY [PageTypeCD] Asc"
					sLnkFld = "PageTypeID"
					sDispFld1 = "PageTypeCD"
					sDispFld2 = "" 
				Case "ParentPageID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [PageID], [PageName], [CompanyID] FROM [Page]"
					sSql &= " ORDER BY [PageName] Asc"
					sLnkFld = "PageID"
					sDispFld1 = "PageName"
					sDispFld2 = "" 
				Case "CompanyID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [CompanyID], [CompanyName] FROM [Company]"
					sSql &= " ORDER BY [CompanyName] "
					sLnkFld = "CompanyID"
					sDispFld1 = "CompanyName"
					sDispFld2 = "" 
				Case "GroupID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [GroupID], [GroupName] FROM [Group]"
					sSql &= " ORDER BY [GroupID] Asc"
					sLnkFld = "GroupID"
					sDispFld1 = "GroupName"
					sDispFld2 = "" 
				Case "SiteCategoryID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory]"
					sSql &= " WHERE " & "[SiteCategoryTypeID]=" & System.Web.HttpContext.Current.Session.Item("SiteCategoryTypeID") & " "
					sSql &= " ORDER BY [CategoryName] Asc"
					sLnkFld = "SiteCategoryID"
					sDispFld1 = "CategoryName"
					sDispFld2 = "" 
				Case "SiteCategoryGroupID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT DISTINCT [SiteCategoryGroupID], [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
					sLnkFld = "SiteCategoryGroupID"
					sDispFld1 = "SiteCategoryGroupNM"
					sDispFld2 = "" 
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

		Public Overridable Sub Update(ByVal row As Pagerow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Pageinf = New Pageinf()
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
						If (row.Attribute("PageID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageID", OleDbType.Integer)
							If (row.PageID.HasValue) Then
								oParameter.Value = DirectCast(row.PageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageOrder").AllowUpdate) Then
							oParameter = New OleDbParameter("PageOrder", OleDbType.SmallInt)
							If (row.PageOrder.HasValue) Then
								oParameter.Value = DirectCast(row.PageOrder, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageName").AllowUpdate) Then
							oParameter = New OleDbParameter("PageName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageName)) Then
								oParameter.Value = DirectCast(row.PageName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageTitle").AllowUpdate) Then
							oParameter = New OleDbParameter("PageTitle", OleDbType.VarWChar)
							If (row.PageTitle IsNot Nothing) Then
								oParameter.Value = DirectCast(row.PageTitle, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageDescription").AllowUpdate) Then
							oParameter = New OleDbParameter("PageDescription", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageDescription)) Then
								oParameter.Value = DirectCast(row.PageDescription, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageKeywords").AllowUpdate) Then
							oParameter = New OleDbParameter("PageKeywords", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageKeywords)) Then
								oParameter.Value = DirectCast(row.PageKeywords, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageTypeID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageTypeID", OleDbType.Integer)
							If (row.PageTypeID.HasValue) Then
								oParameter.Value = DirectCast(row.PageTypeID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImagesPerRow").AllowUpdate) Then
							oParameter = New OleDbParameter("ImagesPerRow", OleDbType.Integer)
							If (row.ImagesPerRow.HasValue) Then
								oParameter.Value = DirectCast(row.ImagesPerRow, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("RowsPerPage").AllowUpdate) Then
							oParameter = New OleDbParameter("RowsPerPage", OleDbType.Integer)
							If (row.RowsPerPage.HasValue) Then
								oParameter.Value = DirectCast(row.RowsPerPage, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ParentPageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ParentPageID", OleDbType.Integer)
							If (row.ParentPageID.HasValue) Then
								oParameter.Value = DirectCast(row.ParentPageID, Object)
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
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("PageFileName", OleDbType.VarWChar)
							If (row.PageFileName IsNot Nothing) Then
								oParameter.Value = DirectCast(row.PageFileName, Object)
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
						If (row.Attribute("ModifiedDT").AllowUpdate) Then
							oParameter = New OleDbParameter("ModifiedDT", OleDbType.DBTimeStamp)
							If (row.ModifiedDT.HasValue) Then
								oParameter.Value = DirectCast(row.ModifiedDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("VersionNo").AllowUpdate) Then
							oParameter = New OleDbParameter("VersionNo", OleDbType.Integer)
							If (row.VersionNo.HasValue) Then
								oParameter.Value = DirectCast(row.VersionNo, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("AllowMessage").AllowUpdate) Then
							oParameter = New OleDbParameter("AllowMessage", OleDbType.Boolean)
							If (row.AllowMessage.HasValue) Then
								oParameter.Value = DirectCast(row.AllowMessage, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryID", OleDbType.Integer)
							If (row.SiteCategoryID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryGroupID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryGroupID", OleDbType.Integer)
							If (row.SiteCategoryGroupID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryGroupID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("PageID", OleDbType.Integer)
						oParameter.Value = row.PageID
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
				Throw New PageDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As Pagerow) As Pagerow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As Pageinf = New Pageinf()
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
						If (row.Attribute("PageID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageID", OleDbType.Integer)
							If (row.PageID.HasValue) Then
								oParameter.Value = DirectCast(row.PageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageOrder").AllowUpdate) Then
							oParameter = New OleDbParameter("PageOrder", OleDbType.SmallInt)
							If (row.PageOrder.HasValue) Then
								oParameter.Value = DirectCast(row.PageOrder, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageName").AllowUpdate) Then
							oParameter = New OleDbParameter("PageName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageName)) Then
								oParameter.Value = DirectCast(row.PageName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageTitle").AllowUpdate) Then
							oParameter = New OleDbParameter("PageTitle", OleDbType.VarWChar)
							If (row.PageTitle IsNot Nothing) Then
								oParameter.Value = DirectCast(row.PageTitle, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageDescription").AllowUpdate) Then
							oParameter = New OleDbParameter("PageDescription", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageDescription)) Then
								oParameter.Value = DirectCast(row.PageDescription, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageKeywords").AllowUpdate) Then
							oParameter = New OleDbParameter("PageKeywords", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.PageKeywords)) Then
								oParameter.Value = DirectCast(row.PageKeywords, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageTypeID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageTypeID", OleDbType.Integer)
							If (row.PageTypeID.HasValue) Then
								oParameter.Value = DirectCast(row.PageTypeID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImagesPerRow").AllowUpdate) Then
							oParameter = New OleDbParameter("ImagesPerRow", OleDbType.Integer)
							If (row.ImagesPerRow.HasValue) Then
								oParameter.Value = DirectCast(row.ImagesPerRow, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("RowsPerPage").AllowUpdate) Then
							oParameter = New OleDbParameter("RowsPerPage", OleDbType.Integer)
							If (row.RowsPerPage.HasValue) Then
								oParameter.Value = DirectCast(row.RowsPerPage, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ParentPageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ParentPageID", OleDbType.Integer)
							If (row.ParentPageID.HasValue) Then
								oParameter.Value = DirectCast(row.ParentPageID, Object)
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
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("PageFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("PageFileName", OleDbType.VarWChar)
							If (row.PageFileName IsNot Nothing) Then
								oParameter.Value = DirectCast(row.PageFileName, Object)
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
						If (row.Attribute("ModifiedDT").AllowUpdate) Then
							oParameter = New OleDbParameter("ModifiedDT", OleDbType.DBTimeStamp)
							If (row.ModifiedDT.HasValue) Then
								oParameter.Value = DirectCast(row.ModifiedDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("VersionNo").AllowUpdate) Then
							oParameter = New OleDbParameter("VersionNo", OleDbType.Integer)
							If (row.VersionNo.HasValue) Then
								oParameter.Value = DirectCast(row.VersionNo, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("AllowMessage").AllowUpdate) Then
							oParameter = New OleDbParameter("AllowMessage", OleDbType.Boolean)
							If (row.AllowMessage.HasValue) Then
								oParameter.Value = DirectCast(row.AllowMessage, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryID", OleDbType.Integer)
							If (row.SiteCategoryID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryGroupID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryGroupID", OleDbType.Integer)
							If (row.SiteCategoryGroupID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryGroupID, Object)
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
						row.PageID = Db.LastInsertedID(oConn) ' load auto-generated ID
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
				Throw New PageDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal PageID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Pageinf = New Pageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("PageID", OleDbType.Integer)
						oParameter.Value = PageID
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
				Throw New PageDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As Pagerow )
			Me.Delete(CType(row.PageID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As Pageinf = New Pageinf()
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
				Throw New PageDataException(strDbErrorMessage)
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Pagekey)), ")"))
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Pagekey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Pagekey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[PageID] = ", key.PageID)) ' [PageID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Pagekey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[PageID] = " & Database.GetSqlParameterName(Db.DbType, "key__PageID"))
			oParameter = New OleDbParameter("key__PageID", OleDbType.Integer)
			oParameter.Value = key.PageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Pagekey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[PageID] = " & Database.GetSqlParameterName(Db.DbType, "key__PageID" & index.ToString())) ' [PageID] field
			oParameter = New OleDbParameter("key__PageID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.PageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class PageDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
