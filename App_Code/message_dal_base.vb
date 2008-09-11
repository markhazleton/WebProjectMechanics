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
	Public MustInherit class Messagedal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As Messagerow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As Messagerow ' Advanced Search Parm 2

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
				objProfile = CustomProfile.GetTable(Share.ProjectName, Messageinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + Messageinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + Messageinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New Messagerow()
				objAdvancedSearchParm2 = New Messagerow()
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

		Public ReadOnly Property AdvancedSearchParm1 As Messagerow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As Messagerow 
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
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", Messageinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", Messageinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As Messagerow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Messagerow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), Messagerow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As Messagerow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Messagerow()
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
			objAdvancedSearchParm1 = New Messagerow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New Messagerow() 
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

		Private Function GetAdvancedSearch(ByVal row1 As Messagerow, ByVal row2 As Messagerow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

				' MessageID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("MessageID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([MessageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__MessageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__MessageID", OleDbType.Integer)
					oParameter.Value = row1.MessageID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("MessageID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("MessageID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([MessageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__MessageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__MessageID", OleDbType.Integer)
					oParameter.Value = row2.MessageID
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

				' ParentMessageID Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ParentMessageID").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ParentMessageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ParentMessageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ParentMessageID", OleDbType.Integer)
					oParameter.Value = row1.ParentMessageID
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ParentMessageID").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ParentMessageID").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ParentMessageID] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ParentMessageID"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ParentMessageID", OleDbType.Integer)
					oParameter.Value = row2.ParentMessageID
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Subject Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Subject").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Subject] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Subject"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Subject", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.Subject + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Subject").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Subject").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Subject] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Subject"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Subject", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.Subject + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Author Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Author").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Author] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Author"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Author", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.Author + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Author").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Author").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Author] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Author"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Author", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.Author + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Email Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Email").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Email] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Email"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Email", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.Email + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Email").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Email").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Email] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Email"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Email", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.Email + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' City Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("City").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([City] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__City"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__City", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.City + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("City").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("City").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([City] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__City"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__City", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.City + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' URL Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("URL").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([URL] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__URL"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__URL", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.URL + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("URL").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("URL").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([URL] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__URL"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__URL", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.URL + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' MessageDate Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("MessageDate").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([MessageDate] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__MessageDate"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__MessageDate", OleDbType.DBTimeStamp)
					oParameter.Value = row1.MessageDate
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("MessageDate").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("MessageDate").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([MessageDate] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__MessageDate"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__MessageDate", OleDbType.DBTimeStamp)
					oParameter.Value = row2.MessageDate
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' Body Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("Body").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([Body] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__Body"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__Body", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row1.Body + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("Body").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("Body").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([Body] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__Body"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__Body", OleDbType.LongVarWChar)
					oParameter.Value = parm.Prefix + row2.Body + parm.Suffix
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

			' [Subject] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Subject] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Subject" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Subject" & i.ToString(), OleDbType.VarWChar)
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

			' [Author] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Author] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Author" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Author" & i.ToString(), OleDbType.VarWChar)
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

			' [Email] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Email] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Email" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Email" & i.ToString(), OleDbType.VarWChar)
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

			' [City] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[City] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__City" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__City" & i.ToString(), OleDbType.VarWChar)
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

			' [URL] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[URL] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__URL" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__URL" & i.ToString(), OleDbType.VarWChar)
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

			' [Body] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[Body] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__Body" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__Body" & i.ToString(), OleDbType.LongVarWChar)
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

		Public Overridable Function LoadRows(ByVal filter As String) As Messagerow()
			Dim rows As Messagerows = LoadList(filter)
			Dim array As Messagerow() = Nothing
			If (rows IsNot Nothing) Then
				array = New Messagerow(rows.Count - 1) {}
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
				Dim oInfo As Messageinf = New Messageinf() 
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
				Throw New MessageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As Messagerows 
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
				Dim rows As Messagerows = New Messagerows()
				For Each drv As DataRowView In dv
					Dim row As Messagerow = GetRow(drv)
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
				Throw New MessageDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As Messagerows 
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
			Dim oInfo As Messageinf = New Messageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As Messagerows = New Messagerows()
				For Each drv As DataRowView In dv
					Dim row As Messagerow = GetRow(drv)
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
				Throw New MessageDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As Messagekey, ByVal filter As String) As Messagerow 
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
				Throw New MessageDataException(strDbErrorMessage)
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
			Dim oInfo As Messageinf = New Messageinf()
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
				Throw New MessageDataException(strDbErrorMessage)
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
				Throw New MessageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As Messagerow ' Get a row based on data reader
			Dim row As Messagerow = New Messagerow()
			Try

				' Field MessageID
				If (Not DBNull.Value.Equals(drv("MessageID"))) Then
					row.MessageID = Convert.ToInt32(drv("MessageID"))
				Else
					row.MessageID = Nothing
				End If

				' Field PageID
				If (Not DBNull.Value.Equals(drv("PageID"))) Then
					row.PageID = Convert.ToInt32(drv("PageID"))
				Else
					row.PageID = Nothing
				End If

				' Field ParentMessageID
				If (Not DBNull.Value.Equals(drv("ParentMessageID"))) Then
					row.ParentMessageID = Convert.ToInt32(drv("ParentMessageID"))
				Else
					row.ParentMessageID = Nothing
				End If

				' Field Subject
				If (Not DBNull.Value.Equals(drv("Subject"))) Then
					row.Subject = Convert.ToString(drv("Subject"))
				Else
					row.Subject = Nothing
				End If

				' Field Author
				If (Not DBNull.Value.Equals(drv("Author"))) Then
					row.Author = Convert.ToString(drv("Author"))
				Else
					row.Author = Nothing
				End If

				' Field Email
				If (Not DBNull.Value.Equals(drv("Email"))) Then
					row.Email = Convert.ToString(drv("Email"))
				Else
					row.Email = Nothing
				End If

				' Field City
				If (Not DBNull.Value.Equals(drv("City"))) Then
					row.City = Convert.ToString(drv("City"))
				Else
					row.City = Nothing
				End If

				' Field URL
				If (Not DBNull.Value.Equals(drv("URL"))) Then
					row.URL = Convert.ToString(drv("URL"))
				Else
					row.URL = Nothing
				End If

				' Field MessageDate
				If (Not DBNull.Value.Equals(drv("MessageDate"))) Then
					row.MessageDate = Convert.ToDateTime(drv("MessageDate"))
				Else
					row.MessageDate = Nothing
				End If

				' Field Body
				If (Not DBNull.Value.Equals(drv("Body"))) Then
					row.Body = Convert.ToString(drv("Body"))
				Else
					row.Body = Nothing
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

		Public Overridable Sub Update(ByVal row As Messagerow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Messageinf = New Messageinf()
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
						If (row.Attribute("MessageID").AllowUpdate) Then
							oParameter = New OleDbParameter("MessageID", OleDbType.Integer)
							If (row.MessageID.HasValue) Then
								oParameter.Value = DirectCast(row.MessageID, Object)
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
						If (row.Attribute("ParentMessageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ParentMessageID", OleDbType.Integer)
							If (row.ParentMessageID.HasValue) Then
								oParameter.Value = DirectCast(row.ParentMessageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Subject").AllowUpdate) Then
							oParameter = New OleDbParameter("Subject", OleDbType.VarWChar)
							If (row.Subject IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Subject, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Author").AllowUpdate) Then
							oParameter = New OleDbParameter("Author", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Author)) Then
								oParameter.Value = DirectCast(row.Author, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Email").AllowUpdate) Then
							oParameter = New OleDbParameter("Email", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Email)) Then
								oParameter.Value = DirectCast(row.Email, Object)
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
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.URL)) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MessageDate").AllowUpdate) Then
							oParameter = New OleDbParameter("MessageDate", OleDbType.DBTimeStamp)
							If (row.MessageDate.HasValue) Then
								oParameter.Value = DirectCast(row.MessageDate, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Body").AllowUpdate) Then
							oParameter = New OleDbParameter("Body", OleDbType.LongVarWChar)
							If (row.Body IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Body, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("MessageID", OleDbType.Integer)
						oParameter.Value = row.MessageID
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
				Throw New MessageDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As Messagerow) As Messagerow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As Messageinf = New Messageinf()
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
						If (row.Attribute("MessageID").AllowUpdate) Then
							oParameter = New OleDbParameter("MessageID", OleDbType.Integer)
							If (row.MessageID.HasValue) Then
								oParameter.Value = DirectCast(row.MessageID, Object)
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
						If (row.Attribute("ParentMessageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ParentMessageID", OleDbType.Integer)
							If (row.ParentMessageID.HasValue) Then
								oParameter.Value = DirectCast(row.ParentMessageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Subject").AllowUpdate) Then
							oParameter = New OleDbParameter("Subject", OleDbType.VarWChar)
							If (row.Subject IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Subject, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Author").AllowUpdate) Then
							oParameter = New OleDbParameter("Author", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Author)) Then
								oParameter.Value = DirectCast(row.Author, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Email").AllowUpdate) Then
							oParameter = New OleDbParameter("Email", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.Email)) Then
								oParameter.Value = DirectCast(row.Email, Object)
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
						If (row.Attribute("URL").AllowUpdate) Then
							oParameter = New OleDbParameter("URL", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.URL)) Then
								oParameter.Value = DirectCast(row.URL, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("MessageDate").AllowUpdate) Then
							oParameter = New OleDbParameter("MessageDate", OleDbType.DBTimeStamp)
							If (row.MessageDate.HasValue) Then
								oParameter.Value = DirectCast(row.MessageDate, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("Body").AllowUpdate) Then
							oParameter = New OleDbParameter("Body", OleDbType.LongVarWChar)
							If (row.Body IsNot Nothing) Then
								oParameter.Value = DirectCast(row.Body, Object)
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
						row.MessageID = Db.LastInsertedID(oConn) ' load auto-generated ID
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
				Throw New MessageDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal MessageID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Messageinf = New Messageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("MessageID", OleDbType.Integer)
						oParameter.Value = MessageID
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
				Throw New MessageDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As Messagerow )
			Me.Delete(CType(row.MessageID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As Messageinf = New Messageinf()
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
				Throw New MessageDataException(strDbErrorMessage)
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Messagekey)), ")"))
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Messagekey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Messagekey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[MessageID] = ", key.MessageID)) ' [MessageID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Messagekey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[MessageID] = " & Database.GetSqlParameterName(Db.DbType, "key__MessageID"))
			oParameter = New OleDbParameter("key__MessageID", OleDbType.Integer)
			oParameter.Value = key.MessageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Messagekey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[MessageID] = " & Database.GetSqlParameterName(Db.DbType, "key__MessageID" & index.ToString())) ' [MessageID] field
			oParameter = New OleDbParameter("key__MessageID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.MessageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class MessageDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
