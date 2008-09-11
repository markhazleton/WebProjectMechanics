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
	Public MustInherit class Articledal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As Articlerow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As Articlerow ' Advanced Search Parm 2

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
				objProfile = CustomProfile.GetTable(Share.ProjectName, Articleinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + Articleinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + Articleinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New Articlerow()
				objAdvancedSearchParm2 = New Articlerow()
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

		Public ReadOnly Property AdvancedSearchParm1 As Articlerow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As Articlerow 
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
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", Articleinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", Articleinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As Articlerow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Articlerow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), Articlerow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As Articlerow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Articlerow()
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
			objAdvancedSearchParm1 = New Articlerow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New Articlerow() 
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

		Private Function GetAdvancedSearch(ByVal row1 As Articlerow, ByVal row2 As Articlerow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

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

				' Description Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Description").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Description] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Description"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Description", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.Description + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Description").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Description").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Description] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Description"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Description", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.Description + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' ArticleSummary Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ArticleSummary").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ArticleSummary] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ArticleSummary"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ArticleSummary", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row1.ArticleSummary + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ArticleSummary").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ArticleSummary").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ArticleSummary] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ArticleSummary"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ArticleSummary", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row2.ArticleSummary + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' ArticleBody Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ArticleBody").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ArticleBody] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ArticleBody"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ArticleBody", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row1.ArticleBody + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ArticleBody").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ArticleBody").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ArticleBody] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ArticleBody"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ArticleBody", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row2.ArticleBody + parm.Suffix
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

				' PageID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("PageID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([PageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__PageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__PageID", OleDbType.Integer)
					oParameter.Value = row1.PageID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("PageID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("PageID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([PageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__PageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__PageID", OleDbType.Integer)
					oParameter.Value = row2.PageID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Active Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Active").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Active] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Active"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Active", OleDbType.Boolean)
					oParameter.Value = row1.Active
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Active").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Active").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Active] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Active"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Active", OleDbType.Boolean)
					oParameter.Value = row2.Active
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' ContactID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ContactID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ContactID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ContactID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ContactID", OleDbType.Integer)
					oParameter.Value = row1.ContactID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ContactID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ContactID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ContactID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ContactID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ContactID", OleDbType.Integer)
					oParameter.Value = row2.ContactID
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

			' [Title] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Title] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Title" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Title" & i.ToString(), OleDbType.VarWChar)
				oParameter.Value = "%" + strKeyword + "%"
				parameters.Add(oParameter)
				If ((strOpr.Length > 0) AndAlso (i < arrKeywords.Length - 1)) Then
					strKeywordWhere &= (String.Concat(" ", strOpr, " "))
				End If
				i += 1
			Next
			If (strKeywordWhere.Trim().Length > 0) Then
				strWhere.Append("(")
				strWhere.Append(strKeywordWhere)
				strWhere.Append(") OR ")
			End If

			' [Description] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Description] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Description" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Description" & i.ToString(), OleDbType.VarWChar)
				oParameter.Value = "%" + strKeyword + "%"
				parameters.Add(oParameter)
				If ((strOpr.Length > 0) AndAlso (i < arrKeywords.Length - 1)) Then
					strKeywordWhere &= (String.Concat(" ", strOpr, " "))
				End If
				i += 1
			Next
			If (strKeywordWhere.Trim().Length > 0) Then
				strWhere.Append("(")
				strWhere.Append(strKeywordWhere)
				strWhere.Append(") OR ")
			End If

			' [ArticleBody] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[ArticleBody] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__ArticleBody" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__ArticleBody" & i.ToString(), OleDbType.LongVarWChar)
				oParameter.Value = "%" + strKeyword + "%"
				parameters.Add(oParameter)
				If ((strOpr.Length > 0) AndAlso (i < arrKeywords.Length - 1)) Then
					strKeywordWhere &= (String.Concat(" ", strOpr, " "))
				End If
				i += 1
			Next
			If (strKeywordWhere.Trim().Length > 0) Then
				strWhere.Append("(")
				strWhere.Append(strKeywordWhere)
				strWhere.Append(") OR ")
			End If
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

		Public Overridable Function LoadRows(ByVal filter As String) As Articlerow()
			Dim rows As Articlerows = LoadList(filter)
			Dim array As Articlerow() = Nothing
			If (rows IsNot Nothing) Then
				array = New Articlerow(rows.Count - 1) {}
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
				Dim oInfo As Articleinf = New Articleinf() 
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
				Throw New ArticleDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As Articlerows 
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
				Dim rows As Articlerows = New Articlerows()
				For Each drv As DataRowView In dv
					Dim row As Articlerow = GetRow(drv)
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
				Throw New ArticleDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As Articlerows 
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
			Dim oInfo As Articleinf = New Articleinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As Articlerows = New Articlerows()
				For Each drv As DataRowView In dv
					Dim row As Articlerow = GetRow(drv)
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
				Throw New ArticleDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As Articlekey, ByVal filter As String) As Articlerow 
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
				Throw New ArticleDataException(strDbErrorMessage)
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
			Dim oInfo As Articleinf = New Articleinf()
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
				Throw New ArticleDataException(strDbErrorMessage)
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
				Throw New ArticleDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As Articlerow ' Get a row based on data reader
			Dim row As Articlerow = New Articlerow()
			Try

				' Field ArticleID
				If (Not DBNull.Value.Equals(drv("ArticleID"))) Then
					row.ArticleID = Convert.ToInt32(drv("ArticleID"))
				Else
					row.ArticleID = Nothing
				End If

				' Field Title
				If (Not DBNull.Value.Equals(drv("Title"))) Then
					row.Title = Convert.ToString(drv("Title"))
				Else
					row.Title = Nothing
				End If

				' Field Description
				If (Not DBNull.Value.Equals(drv("Description"))) Then
					row.Description = Convert.ToString(drv("Description"))
				Else
					row.Description = Nothing
				End If

				' Field ArticleSummary
				If (Not DBNull.Value.Equals(drv("ArticleSummary"))) Then
					row.ArticleSummary = Convert.ToString(drv("ArticleSummary"))
				Else
					row.ArticleSummary = Nothing
				End If

				' Field ArticleBody
				If (Not DBNull.Value.Equals(drv("ArticleBody"))) Then
					row.ArticleBody = Convert.ToString(drv("ArticleBody"))
				Else
					row.ArticleBody = Nothing
				End If

				' Field CompanyID
				If (Not DBNull.Value.Equals(drv("CompanyID"))) Then
					row.CompanyID = Convert.ToInt32(drv("CompanyID"))
				Else
					row.CompanyID = Nothing
				End If

				' Field PageID
				If (Not DBNull.Value.Equals(drv("PageID"))) Then
					row.PageID = Convert.ToInt32(drv("PageID"))
				Else
					row.PageID = Nothing
				End If

				' Field Active
				If (Not DBNull.Value.Equals(drv("Active"))) Then
					row.Active = Convert.ToBoolean(drv("Active"))
				Else
					row.Active = Nothing
				End If

				' Field StartDT
				If (Not DBNull.Value.Equals(drv("StartDT"))) Then
					row.StartDT = Convert.ToDateTime(drv("StartDT"))
				Else
					row.StartDT = Nothing
				End If

				' Field EndDT
				If (Not DBNull.Value.Equals(drv("EndDT"))) Then
					row.EndDT = Convert.ToDateTime(drv("EndDT"))
				Else
					row.EndDT = Nothing
				End If

				' Field ExpireDT
				If (Not DBNull.Value.Equals(drv("ExpireDT"))) Then
					row.ExpireDT = Convert.ToDateTime(drv("ExpireDT"))
				Else
					row.ExpireDT = Nothing
				End If

				' Field ContactID
				If (Not DBNull.Value.Equals(drv("ContactID"))) Then
					row.ContactID = Convert.ToInt32(drv("ContactID"))
				Else
					row.ContactID = Nothing
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

				' Field Counter
				If (Not DBNull.Value.Equals(drv("Counter"))) Then
					row.Counter = Convert.ToInt32(drv("Counter"))
				Else
					row.Counter = Nothing
				End If

				' Field Author
				If (Not DBNull.Value.Equals(drv("Author"))) Then
					row.Author = Convert.ToString(drv("Author"))
				Else
					row.Author = Nothing
				End If

				' Field userID
				If (Not DBNull.Value.Equals(drv("userID"))) Then
					row.userID = Convert.ToInt32(drv("userID"))
				Else
					row.userID = Nothing
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
				Case "PageID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [PageID], [PageName] FROM [Page] WHERE "
						sSql &= String.Concat("[PageID]=", Db.AdjustSql(value), " ORDER BY [PageName] Asc")						
						sDispFld1 = "PageName"
						sDispFld2 = ""
					End If
				Case "ContactID"

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
				Case "Active"
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
				Case "PageID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [PageID], [PageName], [CompanyID] FROM [Page]"
					sSql &= " ORDER BY [PageName] Asc"
					sLnkFld = "PageID"
					sDispFld1 = "PageName"
					sDispFld2 = "" 
				Case "ContactID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [ContactID], [PrimaryContact], [CompanyID] FROM [Contact]"
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

		Public Overridable Sub Update(ByVal row As Articlerow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Articleinf = New Articleinf()
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
						If (row.Attribute("ArticleID").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleID", OleDbType.Integer)
							If (row.ArticleID.HasValue) Then
								oParameter.Value = DirectCast(row.ArticleID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Title").AllowUpdate) Then
							oParameter = New OleDbParameter("Title", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Title)) Then
								oParameter.Value = DirectCast(row.Title, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Description").AllowUpdate) Then
							oParameter = New OleDbParameter("Description", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Description)) Then
								oParameter.Value = DirectCast(row.Description, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ArticleSummary").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleSummary", OleDbType.LongVarWChar)
							If (row.ArticleSummary IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ArticleSummary, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ArticleBody").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleBody", OleDbType.LongVarWChar)
							If (row.ArticleBody IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ArticleBody, Object)
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
						If (row.Attribute("PageID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageID", OleDbType.Integer)
							If (row.PageID.HasValue) Then
								oParameter.Value = DirectCast(row.PageID, Object)
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
						If (row.Attribute("StartDT").AllowUpdate) Then
							oParameter = New OleDbParameter("StartDT", OleDbType.DBTimeStamp)
							If (row.StartDT.HasValue) Then
								oParameter.Value = DirectCast(row.StartDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("EndDT").AllowUpdate) Then
							oParameter = New OleDbParameter("EndDT", OleDbType.DBTimeStamp)
							If (row.EndDT.HasValue) Then
								oParameter.Value = DirectCast(row.EndDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ExpireDT").AllowUpdate) Then
							oParameter = New OleDbParameter("ExpireDT", OleDbType.DBTimeStamp)
							If (row.ExpireDT.HasValue) Then
								oParameter.Value = DirectCast(row.ExpireDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
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
						If (row.Attribute("Counter").AllowUpdate) Then
							oParameter = New OleDbParameter("Counter", OleDbType.Integer)
							If (row.Counter.HasValue) Then
								oParameter.Value = DirectCast(row.Counter, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Author").AllowUpdate) Then
							oParameter = New OleDbParameter("Author", OleDbType.VarWChar)
							If (row.Author IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Author, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("userID").AllowUpdate) Then
							oParameter = New OleDbParameter("userID", OleDbType.Integer)
							If (row.userID.HasValue) Then
								oParameter.Value = DirectCast(row.userID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("ArticleID", OleDbType.Integer)
						oParameter.Value = row.ArticleID
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
				Throw New ArticleDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As Articlerow) As Articlerow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As Articleinf = New Articleinf()
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
						If (row.Attribute("ArticleID").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleID", OleDbType.Integer)
							If (row.ArticleID.HasValue) Then
								oParameter.Value = DirectCast(row.ArticleID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Title").AllowUpdate) Then
							oParameter = New OleDbParameter("Title", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Title)) Then
								oParameter.Value = DirectCast(row.Title, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Description").AllowUpdate) Then
							oParameter = New OleDbParameter("Description", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Description)) Then
								oParameter.Value = DirectCast(row.Description, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ArticleSummary").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleSummary", OleDbType.LongVarWChar)
							If (row.ArticleSummary IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ArticleSummary, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ArticleBody").AllowUpdate) Then
							oParameter = New OleDbParameter("ArticleBody", OleDbType.LongVarWChar)
							If (row.ArticleBody IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ArticleBody, Object)
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
						If (row.Attribute("PageID").AllowUpdate) Then
							oParameter = New OleDbParameter("PageID", OleDbType.Integer)
							If (row.PageID.HasValue) Then
								oParameter.Value = DirectCast(row.PageID, Object)
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
						If (row.Attribute("StartDT").AllowUpdate) Then
							oParameter = New OleDbParameter("StartDT", OleDbType.DBTimeStamp)
							If (row.StartDT.HasValue) Then
								oParameter.Value = DirectCast(row.StartDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("EndDT").AllowUpdate) Then
							oParameter = New OleDbParameter("EndDT", OleDbType.DBTimeStamp)
							If (row.EndDT.HasValue) Then
								oParameter.Value = DirectCast(row.EndDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ExpireDT").AllowUpdate) Then
							oParameter = New OleDbParameter("ExpireDT", OleDbType.DBTimeStamp)
							If (row.ExpireDT.HasValue) Then
								oParameter.Value = DirectCast(row.ExpireDT, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
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
						If (row.Attribute("Counter").AllowUpdate) Then
							oParameter = New OleDbParameter("Counter", OleDbType.Integer)
							If (row.Counter.HasValue) Then
								oParameter.Value = DirectCast(row.Counter, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Author").AllowUpdate) Then
							oParameter = New OleDbParameter("Author", OleDbType.VarWChar)
							If (row.Author IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Author, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("userID").AllowUpdate) Then
							oParameter = New OleDbParameter("userID", OleDbType.Integer)
							If (row.userID.HasValue) Then
								oParameter.Value = DirectCast(row.userID, Object)
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
						row.ArticleID = Db.LastInsertedID(oConn) ' load auto-generated ID
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
				Throw New ArticleDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal ArticleID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Articleinf = New Articleinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("ArticleID", OleDbType.Integer)
						oParameter.Value = ArticleID
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
				Throw New ArticleDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As Articlerow )
			Me.Delete(CType(row.ArticleID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As Articleinf = New Articleinf()
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
				Throw New ArticleDataException(strDbErrorMessage)
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Articlekey)), ")"))
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Articlekey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Articlekey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[ArticleID] = ", key.ArticleID)) ' [ArticleID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Articlekey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ArticleID] = " & Database.GetSqlParameterName(Db.DbType, "key__ArticleID"))
			oParameter = New OleDbParameter("key__ArticleID", OleDbType.Integer)
			oParameter.Value = key.ArticleID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Articlekey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ArticleID] = " & Database.GetSqlParameterName(Db.DbType, "key__ArticleID" & index.ToString())) ' [ArticleID] field
			oParameter = New OleDbParameter("key__ArticleID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.ArticleID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class ArticleDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
