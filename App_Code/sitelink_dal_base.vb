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
	Public MustInherit class SiteLinkdal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As SiteLinkrow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As SiteLinkrow ' Advanced Search Parm 2

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
				objProfile = CustomProfile.GetTable(Share.ProjectName, SiteLinkinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + SiteLinkinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + SiteLinkinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New SiteLinkrow()
				objAdvancedSearchParm2 = New SiteLinkrow()
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

		Public ReadOnly Property AdvancedSearchParm1 As SiteLinkrow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As SiteLinkrow 
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
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", SiteLinkinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", SiteLinkinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As SiteLinkrow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New SiteLinkrow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), SiteLinkrow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As SiteLinkrow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New SiteLinkrow()
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
			objAdvancedSearchParm1 = New SiteLinkrow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New SiteLinkrow() 
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

		Private Function GetAdvancedSearch(ByVal row1 As SiteLinkrow, ByVal row2 As SiteLinkrow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

				' SiteCategoryTypeID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("SiteCategoryTypeID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([SiteCategoryTypeID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__SiteCategoryTypeID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__SiteCategoryTypeID", OleDbType.Integer)
					oParameter.Value = row1.SiteCategoryTypeID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("SiteCategoryTypeID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("SiteCategoryTypeID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([SiteCategoryTypeID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__SiteCategoryTypeID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__SiteCategoryTypeID", OleDbType.Integer)
					oParameter.Value = row2.SiteCategoryTypeID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' CategoryID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("CategoryID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([CategoryID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__CategoryID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__CategoryID", OleDbType.Integer)
					oParameter.Value = row1.CategoryID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("CategoryID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("CategoryID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([CategoryID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__CategoryID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__CategoryID", OleDbType.Integer)
					oParameter.Value = row2.CategoryID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' LinkTypeCD Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("LinkTypeCD").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([LinkTypeCD] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__LinkTypeCD"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__LinkTypeCD", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.LinkTypeCD + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("LinkTypeCD").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("LinkTypeCD").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([LinkTypeCD] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__LinkTypeCD"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__LinkTypeCD", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.LinkTypeCD + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Title Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Title").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Title] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Title"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Title", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.Title + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Title").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Title").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Title] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Title"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Title", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.Title + parm.Suffix
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

		Public Overridable Function LoadRows(ByVal filter As String) As SiteLinkrow()
			Dim rows As SiteLinkrows = LoadList(filter)
			Dim array As SiteLinkrow() = Nothing
			If (rows IsNot Nothing) Then
				array = New SiteLinkrow(rows.Count - 1) {}
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
				Dim oInfo As SiteLinkinf = New SiteLinkinf() 
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As SiteLinkrows 
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
				Dim rows As SiteLinkrows = New SiteLinkrows()
				For Each drv As DataRowView In dv
					Dim row As SiteLinkrow = GetRow(drv)
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As SiteLinkrows 
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
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As SiteLinkrows = New SiteLinkrows()
				For Each drv As DataRowView In dv
					Dim row As SiteLinkrow = GetRow(drv)
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As SiteLinkkey, ByVal filter As String) As SiteLinkrow 
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
				Throw New SiteLinkDataException(strDbErrorMessage)
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
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
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
				Throw New SiteLinkDataException(strDbErrorMessage)
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As SiteLinkrow ' Get a row based on data reader
			Dim row As SiteLinkrow = New SiteLinkrow()
			Try

				' Field ID
				If (Not DBNull.Value.Equals(drv("ID"))) Then
					row.ID = Convert.ToInt32(drv("ID"))
				Else
					row.ID = Nothing
				End If

				' Field SiteCategoryTypeID
				If (Not DBNull.Value.Equals(drv("SiteCategoryTypeID"))) Then
					row.SiteCategoryTypeID = Convert.ToInt32(drv("SiteCategoryTypeID"))
				Else
					row.SiteCategoryTypeID = Nothing
				End If

				' Field CategoryID
				If (Not DBNull.Value.Equals(drv("CategoryID"))) Then
					row.CategoryID = Convert.ToInt32(drv("CategoryID"))
				Else
					row.CategoryID = Nothing
				End If

				' Field LinkTypeCD
				If (Not DBNull.Value.Equals(drv("LinkTypeCD"))) Then
					row.LinkTypeCD = Convert.ToString(drv("LinkTypeCD"))
				Else
					row.LinkTypeCD = Nothing
				End If

				' Field Title
				If (Not DBNull.Value.Equals(drv("Title"))) Then
					row.Title = Convert.ToString(drv("Title"))
				Else
					row.Title = Nothing
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

				' Field CompanyID
				If (Not DBNull.Value.Equals(drv("CompanyID"))) Then
					row.CompanyID = Convert.ToInt32(drv("CompanyID"))
				Else
					row.CompanyID = Nothing
				End If

				' Field Description
				If (Not DBNull.Value.Equals(drv("Description"))) Then
					row.Description = Convert.ToString(drv("Description"))
				Else
					row.Description = Nothing
				End If

				' Field URL
				If (Not DBNull.Value.Equals(drv("URL"))) Then
					row.URL = Convert.ToString(drv("URL"))
				Else
					row.URL = Nothing
				End If

				' Field DateAdd
				If (Not DBNull.Value.Equals(drv("DateAdd"))) Then
					row.DateAdd = Convert.ToDateTime(drv("DateAdd"))
				Else
					row.DateAdd = Nothing
				End If

				' Field Ranks
				If (Not DBNull.Value.Equals(drv("Ranks"))) Then
					row.Ranks = Convert.ToInt32(drv("Ranks"))
				Else
					row.Ranks = Nothing
				End If

				' Field Views
				If (Not DBNull.Value.Equals(drv("Views"))) Then
					row.Views = Convert.ToBoolean(drv("Views"))
				Else
					row.Views = Nothing
				End If

				' Field UserName
				If (Not DBNull.Value.Equals(drv("UserName"))) Then
					row.UserName = Convert.ToString(drv("UserName"))
				Else
					row.UserName = Nothing
				End If

				' Field UserID
				If (Not DBNull.Value.Equals(drv("UserID"))) Then
					row.UserID = Convert.ToInt32(drv("UserID"))
				Else
					row.UserID = Nothing
				End If

				' Field ASIN
				If (Not DBNull.Value.Equals(drv("ASIN"))) Then
					row.ASIN = Convert.ToString(drv("ASIN"))
				Else
					row.ASIN = Nothing
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
				Case "SiteCategoryTypeID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM] FROM [SiteCategoryType] WHERE "
						sSql &= String.Concat("[SiteCategoryTypeID]=", Db.AdjustSql(value), " ORDER BY [SiteCategoryTypeNM] Asc")						
						sDispFld1 = "SiteCategoryTypeNM"
						sDispFld2 = ""
					End If
				Case "CategoryID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [ID], [Title] FROM [LinkCategory] WHERE "
						sSql &= String.Concat("[ID]=", Db.AdjustSql(value), " ORDER BY [Title] Asc")						
						sDispFld1 = "Title"
						sDispFld2 = ""
					End If
				Case "LinkTypeCD"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckString(value)) Then ' validate
						sSql = "SELECT [LinkTypeCD], [LinkTypeDesc] FROM [LinkType] WHERE "
						sSql &= String.Concat("[LinkTypeCD]='", Db.AdjustSql(value), "' ORDER BY [LinkTypeDesc] Asc")						
						sDispFld1 = "LinkTypeDesc"
						sDispFld2 = ""
					End If
				Case "SiteCategoryID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [SiteCategoryID], [CategoryName] FROM [SiteCategory] WHERE "
						sSql &= String.Concat("[SiteCategoryID]=", Db.AdjustSql(value), " ORDER BY [CategoryName] Asc")						
						sDispFld1 = "CategoryName"
						sDispFld2 = ""
					End If
				Case "SiteCategoryGroupID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM] FROM [SiteCategoryGroup] WHERE "
						sSql &= String.Concat("[SiteCategoryGroupID]=", Db.AdjustSql(value), " ORDER BY [SiteCategoryGroupOrder] Asc")						
						sDispFld1 = "SiteCategoryGroupNM"
						sDispFld2 = ""
					End If
				Case "CompanyID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [CompanyID], [CompanyName] FROM [Company] WHERE "
						sSql &= String.Concat("[CompanyID]=", Db.AdjustSql(value), " ORDER BY [CompanyName] Asc")						
						sDispFld1 = "CompanyName"
						sDispFld2 = ""
					End If
				Case "UserID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE "
						sSql &= String.Concat("[ContactID]=", Db.AdjustSql(value), " ORDER BY [PrimaryContact] Asc")						
						sDispFld1 = "PrimaryContact"
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
				Case "Views"
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
				Case "SiteCategoryTypeID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [SiteCategoryTypeID], [SiteCategoryTypeNM] FROM [SiteCategoryType]"
					sSql &= " ORDER BY [SiteCategoryTypeNM] Asc"
					sLnkFld = "SiteCategoryTypeID"
					sDispFld1 = "SiteCategoryTypeNM"
					sDispFld2 = "" 
				Case "CategoryID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [ID], [Title] FROM [LinkCategory]"
					sSql &= " ORDER BY [Title] Asc"
					sLnkFld = "ID"
					sDispFld1 = "Title"
					sDispFld2 = "" 
				Case "LinkTypeCD"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [LinkTypeCD], [LinkTypeDesc] FROM [LinkType]"
					sSql &= " ORDER BY [LinkTypeDesc] Asc"
					sLnkFld = "LinkTypeCD"
					sDispFld1 = "LinkTypeDesc"
					sDispFld2 = "" 
				Case "SiteCategoryID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [SiteCategoryID], [CategoryName], [SiteCategoryTypeID] FROM [SiteCategory]"
					sSql &= " ORDER BY [CategoryName] Asc"
					sLnkFld = "SiteCategoryID"
					sDispFld1 = "CategoryName"
					sDispFld2 = "" 
				Case "SiteCategoryGroupID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [SiteCategoryGroupID], [SiteCategoryGroupNM] FROM [SiteCategoryGroup]"
					sSql &= " ORDER BY [SiteCategoryGroupOrder] Asc"
					sLnkFld = "SiteCategoryGroupID"
					sDispFld1 = "SiteCategoryGroupNM"
					sDispFld2 = "" 
				Case "CompanyID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [CompanyID], [CompanyName], [SiteCategoryTypeID] FROM [Company]"
					sSql &= " ORDER BY [CompanyName] Asc"
					sLnkFld = "CompanyID"
					sDispFld1 = "CompanyName"
					sDispFld2 = "" 
				Case "UserID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [ContactID], [PrimaryContact] FROM [Contact]"
					sSql &= " ORDER BY [PrimaryContact] Asc"
					sLnkFld = "ContactID"
					sDispFld1 = "PrimaryContact"
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

		Public Overridable Sub Update(ByVal row As SiteLinkrow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
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
						If (row.Attribute("ID").AllowUpdate) Then
							oParameter = New OleDbParameter("ID", OleDbType.Integer)
							If (row.ID.HasValue) Then
								oParameter.Value = DirectCast(row.ID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryTypeID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryTypeID", OleDbType.Integer)
							If (row.SiteCategoryTypeID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryTypeID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CategoryID").AllowUpdate) Then
							oParameter = New OleDbParameter("CategoryID", OleDbType.Integer)
							If (row.CategoryID.HasValue) Then
								oParameter.Value = DirectCast(row.CategoryID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LinkTypeCD").AllowUpdate) Then
							oParameter = New OleDbParameter("LinkTypeCD", OleDbType.VarWChar)
							If (row.LinkTypeCD IsNot Nothing) Then
								oParameter.Value = DirectCast(row.LinkTypeCD, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Title").AllowUpdate) Then
							oParameter = New OleDbParameter("Title", OleDbType.VarWChar)
							If (row.Title IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Title, Object)
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
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Description").AllowUpdate) Then
							oParameter = New OleDbParameter("Description", OleDbType.LongVarWChar)
							If (row.Description IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Description, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.LongVarWChar)
							If (row.URL IsNot Nothing) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("DateAdd").AllowUpdate) Then
							oParameter = New OleDbParameter("DateAdd", OleDbType.DBTimeStamp)
							If (row.DateAdd.HasValue) Then
								oParameter.Value = DirectCast(row.DateAdd, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Ranks").AllowUpdate) Then
							oParameter = New OleDbParameter("Ranks", OleDbType.Integer)
							If (row.Ranks.HasValue) Then
								oParameter.Value = DirectCast(row.Ranks, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Views").AllowUpdate) Then
							oParameter = New OleDbParameter("Views", OleDbType.Boolean)
							If (row.Views.HasValue) Then
								oParameter.Value = DirectCast(row.Views, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("UserName").AllowUpdate) Then
							oParameter = New OleDbParameter("UserName", OleDbType.VarWChar)
							If (row.UserName IsNot Nothing) Then
								oParameter.Value = DirectCast(row.UserName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("UserID").AllowUpdate) Then
							oParameter = New OleDbParameter("UserID", OleDbType.Integer)
							If (row.UserID.HasValue) Then
								oParameter.Value = DirectCast(row.UserID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ASIN").AllowUpdate) Then
							oParameter = New OleDbParameter("ASIN", OleDbType.VarWChar)
							If (row.ASIN IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ASIN, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("ID", OleDbType.Integer)
						oParameter.Value = row.ID
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As SiteLinkrow) As SiteLinkrow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
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
						If (row.Attribute("ID").AllowUpdate) Then
							oParameter = New OleDbParameter("ID", OleDbType.Integer)
							If (row.ID.HasValue) Then
								oParameter.Value = DirectCast(row.ID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("SiteCategoryTypeID").AllowUpdate) Then
							oParameter = New OleDbParameter("SiteCategoryTypeID", OleDbType.Integer)
							If (row.SiteCategoryTypeID.HasValue) Then
								oParameter.Value = DirectCast(row.SiteCategoryTypeID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("CategoryID").AllowUpdate) Then
							oParameter = New OleDbParameter("CategoryID", OleDbType.Integer)
							If (row.CategoryID.HasValue) Then
								oParameter.Value = DirectCast(row.CategoryID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("LinkTypeCD").AllowUpdate) Then
							oParameter = New OleDbParameter("LinkTypeCD", OleDbType.VarWChar)
							If (row.LinkTypeCD IsNot Nothing) Then
								oParameter.Value = DirectCast(row.LinkTypeCD, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Title").AllowUpdate) Then
							oParameter = New OleDbParameter("Title", OleDbType.VarWChar)
							If (row.Title IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Title, Object)
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
						If (row.Attribute("CompanyID").AllowUpdate) Then
							oParameter = New OleDbParameter("CompanyID", OleDbType.Integer)
							If (row.CompanyID.HasValue) Then
								oParameter.Value = DirectCast(row.CompanyID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Description").AllowUpdate) Then
							oParameter = New OleDbParameter("Description", OleDbType.LongVarWChar)
							If (row.Description IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Description, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.LongVarWChar)
							If (row.URL IsNot Nothing) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("DateAdd").AllowUpdate) Then
							oParameter = New OleDbParameter("DateAdd", OleDbType.DBTimeStamp)
							If (row.DateAdd.HasValue) Then
								oParameter.Value = DirectCast(row.DateAdd, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Ranks").AllowUpdate) Then
							oParameter = New OleDbParameter("Ranks", OleDbType.Integer)
							If (row.Ranks.HasValue) Then
								oParameter.Value = DirectCast(row.Ranks, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Views").AllowUpdate) Then
							oParameter = New OleDbParameter("Views", OleDbType.Boolean)
							If (row.Views.HasValue) Then
								oParameter.Value = DirectCast(row.Views, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("UserName").AllowUpdate) Then
							oParameter = New OleDbParameter("UserName", OleDbType.VarWChar)
							If (row.UserName IsNot Nothing) Then
								oParameter.Value = DirectCast(row.UserName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("UserID").AllowUpdate) Then
							oParameter = New OleDbParameter("UserID", OleDbType.Integer)
							If (row.UserID.HasValue) Then
								oParameter.Value = DirectCast(row.UserID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ASIN").AllowUpdate) Then
							oParameter = New OleDbParameter("ASIN", OleDbType.VarWChar)
							If (row.ASIN IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ASIN, Object)
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
						row.ID = Db.LastInsertedID(oConn) ' load auto-generated ID
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal ID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("ID", OleDbType.Integer)
						oParameter.Value = ID
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
				Throw New SiteLinkDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As SiteLinkrow )
			Me.Delete(CType(row.ID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As SiteLinkinf = New SiteLinkinf()
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
				Throw New SiteLinkDataException(strDbErrorMessage)
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), SiteLinkkey)), ")"))
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), SiteLinkkey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As SiteLinkkey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[ID] = ", key.ID)) ' [ID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As SiteLinkkey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ID] = " & Database.GetSqlParameterName(Db.DbType, "key__ID"))
			oParameter = New OleDbParameter("key__ID", OleDbType.Integer)
			oParameter.Value = key.ID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As SiteLinkkey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ID] = " & Database.GetSqlParameterName(Db.DbType, "key__ID" & index.ToString())) ' [ID] field
			oParameter = New OleDbParameter("key__ID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.ID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class SiteLinkDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
