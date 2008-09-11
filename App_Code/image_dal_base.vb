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
	Public MustInherit class Imagedal_base

		' Default Order arguments
		Dim strDefaultOrder As String ' Default Order Expression
		Dim strDefaultOrderType As String ' Default Order Type

		' Sort arguments
		Dim strOrderBy As String ' Sort Expression

		' Basic Search arguments
		Dim strBasicSearchKeyword As String ' Basic Search Keyword
		Dim strBasicSearchType As Core.BasicSearchType ' Basic Search Type

		' Advanced Search arguments 
		Dim objAdvancedSearchParm1 As Imagerow ' Advanced Search Parm 1
		Dim objAdvancedSearchParm2 As Imagerow ' Advanced Search Parm 2

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
				objProfile = CustomProfile.GetTable(Share.ProjectName, Imageinf.TableVar)

				' Load Default Order Information
				strDefaultOrder = Convert.ToString(objProfile.DefaultOrder) 
				strDefaultOrderType = Convert.ToString(objProfile.DefaultOrderType)

				' Load Sort Information
				strOrderBy = Convert.ToString(objProfile.OrderBy) 

				' Load Basic Search Information
				strBasicSearchKeyword = Convert.ToString(objProfile.BasicSearchKeyword) 
				strBasicSearchType = objProfile.BasicSearchType 

				' Advanced Search arguments 
				Dim sSearchName As String = Share.ProjectName + "_" + Imageinf.TableVar + "_Search1" 
				objAdvancedSearchParm1 = LoadSearch(sSearchName) 
				sSearchName = Share.ProjectName + "_" + Imageinf.TableVar + "_Search2" 
				objAdvancedSearchParm2 = LoadSearch(sSearchName) 
			Else
				objAdvancedSearchParm1 = New Imagerow()
				objAdvancedSearchParm2 = New Imagerow()
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

		Public ReadOnly Property AdvancedSearchParm1 As Imagerow 
			Get 
				Return objAdvancedSearchParm1
			End Get
		End Property

		' ***********************************
		' *  Advanced Search Parm 2 Property
		' ***********************************

		Public Readonly Property AdvancedSearchParm2 As Imagerow 
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
				Dim sSearchName As String = String.Concat(Share.ProjectName, "_", Imageinf.TableVar, "_Search1") 
				SaveSearch(sSearchName, objAdvancedSearchParm1) 
				sSearchName = String.Concat(Share.ProjectName, "_", Imageinf.TableVar, "_Search2") 
				SaveSearch(sSearchName, objAdvancedSearchParm2) 
			End If
		End Sub

		' **************************
		' *  Load Search Row Object
		' **************************

		Private Function LoadSearch(ByVal objectName As String) As Imagerow 
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Imagerow()
				End If
				Return TryCast(HttpContext.Current.Session(objectName), Imagerow)
			Else
				Return Nothing
			End If
		End Function

		' **************************
		' *  Save Search Row Object
		' **************************

		Private Sub SaveSearch(ByVal objectName As String, ByVal row As Imagerow)
			If (bUseSession) Then
				If (HttpContext.Current.Session(objectName) Is Nothing) Then
					HttpContext.Current.Session(objectName) = New Imagerow()
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
			objAdvancedSearchParm1 = New Imagerow() 
			objAdvancedSearchParm2 = Nothing 
			objAdvancedSearchParm2 = New Imagerow() 
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

		Private Function GetAdvancedSearch(ByVal row1 As Imagerow, ByVal row2 As Imagerow, ByRef parameters As DbParameterCollection ) As String 
			Dim parm As Core.SearchParm 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim strParmWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing
			If (bUseSession) Then

				' ImageName Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("ImageName").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([ImageName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__ImageName"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__ImageName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.ImageName + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("ImageName").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("ImageName").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([ImageName] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__ImageName"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__ImageName", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.ImageName + parm.Suffix
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

				' title Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("title").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([title] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__title"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__title", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.title + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("title").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("title").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([title] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__title"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__title", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.title + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' medium Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("medium").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([medium] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__medium"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__medium", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.medium + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("medium").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("medium").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([medium] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__medium"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__medium", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.medium + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' size Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("size").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([size] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__size"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__size", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.size + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("size").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("size").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([size] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__size"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__size", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.size + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' price Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("price").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([price] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__price"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__price", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.price + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("price").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("price").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([price] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__price"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__price", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.price + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' color Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("color").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([color] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__color"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__color", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.color + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("color").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("color").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([color] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__color"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__color", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.color + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' subject Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("subject").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([subject] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__subject"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__subject", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row1.subject + parm.Suffix
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("subject").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("subject").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([subject] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__subject"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__subject", OleDbType.VarWChar)
					oParameter.Value = parm.Prefix + row2.subject + parm.Suffix
					parameters.Add(oParameter)
				End If
				If (strParmWhere.Length > 0) Then 
					If (strWhere.Length > 0) Then strWhere.Append(" AND ")
					strWhere.Append("(" & strParmWhere.ToString() & ")")
				End If

				' sold Field 
				parm = Core.SetAdvancedSearchParm(row1.Attribute("sold").SearchType)
				strParmWhere.Remove(0, strParmWhere.Length)
				If (parm.Operator.Length > 0) Then
					strParmWhere.Append(String.Concat("([sold] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch1__sold"), ")"))
					oParameter = New OleDbParameter("AdvSearch1__sold", OleDbType.Boolean)
					oParameter.Value = row1.sold
					parameters.Add(oParameter)
				End If
				parm = Core.SetAdvancedSearchParm(row2.Attribute("sold").SearchType)
				If (parm.Operator.Length > 0) Then
					If (strParmWhere.Length > 0) Then strParmWhere.Append(String.Concat(" ", row1.Attribute("sold").SearchCond.ToString(), " "))
					strParmWhere.Append(String.Concat("([sold] ", parm.Operator, " ",  Database.GetSqlParameterName(Db.DbType, "AdvSearch2__sold"), ")"))
					oParameter = New OleDbParameter("AdvSearch2__sold", OleDbType.Boolean)
					oParameter.Value = row2.sold
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

			' [title] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[title] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__title" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__title" & i.ToString(), OleDbType.VarWChar)
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

			' [medium] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[medium] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__medium" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__medium" & i.ToString(), OleDbType.VarWChar)
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

			' [size] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[size] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__size" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__size" & i.ToString(), OleDbType.VarWChar)
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

			' [price] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[price] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__price" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__price" & i.ToString(), OleDbType.VarWChar)
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

			' [color] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[color] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__color" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__color" & i.ToString(), OleDbType.VarWChar)
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

			' [subject] Field
			i = 0
			strKeywordWhere = String.Empty
			For Each strKeyWord As String In arrKeywords
				strKeywordWhere &= (String.Concat("[subject] LIKE ", Database.GetSqlParameterName(Db.DbType, "BasicSearch__subject" & i.ToString())))
				oParameter = New OleDbParameter("BasicSearch__subject" & i.ToString(), OleDbType.VarWChar)
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

		Public Overridable Function LoadRows(ByVal filter As String) As Imagerow()
			Dim rows As Imagerows = LoadList(filter)
			Dim array As Imagerow() = Nothing
			If (rows IsNot Nothing) Then
				array = New Imagerow(rows.Count - 1) {}
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
				Dim oInfo As Imageinf = New Imageinf() 
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
				Throw New ImageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************************
		' *  Get Rows Collection by filter string
		' ****************************************

		Public Overridable Function LoadList(ByVal filter As String) As Imagerows 
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
				Dim rows As Imagerows = New Imagerows()
				For Each drv As DataRowView In dv
					Dim row As Imagerow = GetRow(drv)
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
				Throw New ImageDataException(strDbErrorMessage)
			End Try
		End Function

		' *******************************************************
		' *  Get Rows Collection by filter string with page size
		' *******************************************************

		Public Overridable Function LoadList(ByVal filter As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As Imagerows 
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
			Dim oInfo As Imageinf = New Imageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Select, False, strWhere, strOrderBy) 
			Try
				Dim dv As DataView = Db.GetDataViewPage(sSql, parameters, PageSize, StartRow)
				If (dv Is Nothing) Then
					Return Nothing ' no records found
				End If
				Dim rows As Imagerows = New Imagerows()
				For Each drv As DataRowView In dv
					Dim row As Imagerow = GetRow(drv)
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
				Throw New ImageDataException(strDbErrorMessage)
			End Try
		End Function

		' ************************************
		' *  Get Row by key and filter string
		' ************************************

		Public Overridable Function LoadRow(ByVal key As Imagekey, ByVal filter As String) As Imagerow 
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
				Throw New ImageDataException(strDbErrorMessage)
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
			Dim oInfo As Imageinf = New Imageinf()
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
				Throw New ImageDataException(strDbErrorMessage)
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
				Throw New ImageDataException(strDbErrorMessage)
			End Try
		End Function

		' ****************************
		' *  Get Row from DataRowView
		' ****************************

		Private Function GetRow(ByVal drv As DataRowView) As Imagerow ' Get a row based on data reader
			Dim row As Imagerow = New Imagerow()
			Try

				' Field ImageID
				If (Not DBNull.Value.Equals(drv("ImageID"))) Then
					row.ImageID = Convert.ToInt32(drv("ImageID"))
				Else
					row.ImageID = Nothing
				End If

				' Field ImageName
				If (Not DBNull.Value.Equals(drv("ImageName"))) Then
					row.ImageName = Convert.ToString(drv("ImageName"))
				Else
					row.ImageName = Nothing
				End If

				' Field Active
				If (Not DBNull.Value.Equals(drv("Active"))) Then
					row.Active = Convert.ToBoolean(drv("Active"))
				Else
					row.Active = Nothing
				End If

				' Field ImageDescription
				If (Not DBNull.Value.Equals(drv("ImageDescription"))) Then
					row.ImageDescription = Convert.ToString(drv("ImageDescription"))
				Else
					row.ImageDescription = Nothing
				End If

				' Field ImageComment
				If (Not DBNull.Value.Equals(drv("ImageComment"))) Then
					row.ImageComment = Convert.ToString(drv("ImageComment"))
				Else
					row.ImageComment = Nothing
				End If

				' Field ImageFileName
				If (Not DBNull.Value.Equals(drv("ImageFileName"))) Then
					row.ImageFileName = Convert.ToString(drv("ImageFileName"))
				Else
					row.ImageFileName = Nothing
				End If

				' Field ImageThumbFileName
				If (Not DBNull.Value.Equals(drv("ImageThumbFileName"))) Then
					row.ImageThumbFileName = Convert.ToString(drv("ImageThumbFileName"))
				Else
					row.ImageThumbFileName = Nothing
				End If

				' Field ImageDate
				If (Not DBNull.Value.Equals(drv("ImageDate"))) Then
					row.ImageDate = Convert.ToDateTime(drv("ImageDate"))
				Else
					row.ImageDate = Nothing
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

				' Field ContactID
				If (Not DBNull.Value.Equals(drv("ContactID"))) Then
					row.ContactID = Convert.ToInt32(drv("ContactID"))
				Else
					row.ContactID = Nothing
				End If

				' Field CompanyID
				If (Not DBNull.Value.Equals(drv("CompanyID"))) Then
					row.CompanyID = Convert.ToInt32(drv("CompanyID"))
				Else
					row.CompanyID = Nothing
				End If

				' Field title
				If (Not DBNull.Value.Equals(drv("title"))) Then
					row.title = Convert.ToString(drv("title"))
				Else
					row.title = Nothing
				End If

				' Field medium
				If (Not DBNull.Value.Equals(drv("medium"))) Then
					row.medium = Convert.ToString(drv("medium"))
				Else
					row.medium = Nothing
				End If

				' Field size
				If (Not DBNull.Value.Equals(drv("size"))) Then
					row.size = Convert.ToString(drv("size"))
				Else
					row.size = Nothing
				End If

				' Field price
				If (Not DBNull.Value.Equals(drv("price"))) Then
					row.price = Convert.ToString(drv("price"))
				Else
					row.price = Nothing
				End If

				' Field color
				If (Not DBNull.Value.Equals(drv("color"))) Then
					row.color = Convert.ToString(drv("color"))
				Else
					row.color = Nothing
				End If

				' Field subject
				If (Not DBNull.Value.Equals(drv("subject"))) Then
					row.subject = Convert.ToString(drv("subject"))
				Else
					row.subject = Nothing
				End If

				' Field sold
				If (Not DBNull.Value.Equals(drv("sold"))) Then
					row.sold = Convert.ToBoolean(drv("sold"))
				Else
					row.sold = Nothing
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
				Case "ContactID"

					' linking sql & fields (Generated depends on Settings)
					If (value IsNot Nothing AndAlso value.Length > 0 AndAlso DataFormat.CheckInt32(value)) Then ' validate
						sSql = "SELECT [ContactID], [PrimaryContact] FROM [Contact] WHERE "
						sSql &= String.Concat("[ContactID]=", Db.AdjustSql(value), " ORDER BY [PrimaryContact] Asc")						
						sDispFld1 = "PrimaryContact"
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
				Case "sold"
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
				Case "ContactID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [ContactID], [PrimaryContact], [CompanyID] FROM [Contact]"
					sSql &= " ORDER BY [PrimaryContact] Asc"
					sLnkFld = "ContactID"
					sDispFld1 = "PrimaryContact"
					sDispFld2 = "" 
				Case "CompanyID"

				' linking sql & fields (Generated depends on Settings)
					sSql = "SELECT [CompanyID], [CompanyName] FROM [Company]"
					sSql &= " ORDER BY [CompanyName] Asc"
					sLnkFld = "CompanyID"
					sDispFld1 = "CompanyName"
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

		Public Overridable Sub Update(ByVal row As Imagerow)
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Imageinf = New Imageinf()
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
						If (row.Attribute("ImageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageID", OleDbType.Integer)
							If (row.ImageID.HasValue) Then
								oParameter.Value = DirectCast(row.ImageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageName)) Then
								oParameter.Value = DirectCast(row.ImageName, Object)
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
						If (row.Attribute("ImageDescription").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageDescription", OleDbType.LongVarWChar)
							If (Not String.IsNullOrEmpty(row.ImageDescription)) Then
								oParameter.Value = DirectCast(row.ImageDescription, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageComment").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageComment", OleDbType.LongVarWChar)
							If (row.ImageComment IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ImageComment, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageFileName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageFileName)) Then
								oParameter.Value = DirectCast(row.ImageFileName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageThumbFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageThumbFileName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageThumbFileName)) Then
								oParameter.Value = DirectCast(row.ImageThumbFileName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageDate").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageDate", OleDbType.DBTimeStamp)
							If (row.ImageDate.HasValue) Then
								oParameter.Value = DirectCast(row.ImageDate, Object)
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
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
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
						If (row.Attribute("title").AllowUpdate) Then
							oParameter = New OleDbParameter("title", OleDbType.VarWChar)
							If (row.title IsNot Nothing) Then
								oParameter.Value = DirectCast(row.title, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("medium").AllowUpdate) Then
							oParameter = New OleDbParameter("medium", OleDbType.VarWChar)
							If (row.medium IsNot Nothing) Then
								oParameter.Value = DirectCast(row.medium, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("size").AllowUpdate) Then
							oParameter = New OleDbParameter("size", OleDbType.VarWChar)
							If (row.size IsNot Nothing) Then
								oParameter.Value = DirectCast(row.size, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("price").AllowUpdate) Then
							oParameter = New OleDbParameter("price", OleDbType.VarWChar)
							If (row.price IsNot Nothing) Then
								oParameter.Value = DirectCast(row.price, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("color").AllowUpdate) Then
							oParameter = New OleDbParameter("color", OleDbType.VarWChar)
							If (row.color IsNot Nothing) Then
								oParameter.Value = DirectCast(row.color, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("subject").AllowUpdate) Then
							oParameter = New OleDbParameter("subject", OleDbType.VarWChar)
							If (row.subject IsNot Nothing) Then
								oParameter.Value = DirectCast(row.subject, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("sold").AllowUpdate) Then
							oParameter = New OleDbParameter("sold", OleDbType.Boolean)
							If (row.sold.HasValue) Then
								oParameter.Value = DirectCast(row.sold, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If

						' Key parameters
						oParameter = New OleDbParameter("ImageID", OleDbType.Integer)
						oParameter.Value = row.ImageID
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
				Throw New ImageDataException(strDbErrorMessage)
			End If
		End Sub

		' **************************
		' *  Insert Row to Database
		' **************************

		Public Overridable Function Insert(ByVal row As Imagerow) As Imagerow 
			Dim oTrans As DbTransaction = Nothing
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Construct SQL statement
			Dim oInfo As Imageinf = New Imageinf()
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
						If (row.Attribute("ImageID").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageID", OleDbType.Integer)
							If (row.ImageID.HasValue) Then
								oParameter.Value = DirectCast(row.ImageID, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageName)) Then
								oParameter.Value = DirectCast(row.ImageName, Object)
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
						If (row.Attribute("ImageDescription").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageDescription", OleDbType.LongVarWChar)
							If (Not String.IsNullOrEmpty(row.ImageDescription)) Then
								oParameter.Value = DirectCast(row.ImageDescription, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageComment").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageComment", OleDbType.LongVarWChar)
							If (row.ImageComment IsNot Nothing) Then
								oParameter.Value = DirectCast(row.ImageComment, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageFileName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageFileName)) Then
								oParameter.Value = DirectCast(row.ImageFileName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageThumbFileName").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageThumbFileName", OleDbType.VarWChar)
							If (Not String.IsNullOrEmpty(row.ImageThumbFileName)) Then
								oParameter.Value = DirectCast(row.ImageThumbFileName, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("ImageDate").AllowUpdate) Then
							oParameter = New OleDbParameter("ImageDate", OleDbType.DBTimeStamp)
							If (row.ImageDate.HasValue) Then
								oParameter.Value = DirectCast(row.ImageDate, Object)
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
						If (row.Attribute("ContactID").AllowUpdate) Then
							oParameter = New OleDbParameter("ContactID", OleDbType.Integer)
							If (row.ContactID.HasValue) Then
								oParameter.Value = DirectCast(row.ContactID, Object)
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
						If (row.Attribute("title").AllowUpdate) Then
							oParameter = New OleDbParameter("title", OleDbType.VarWChar)
							If (row.title IsNot Nothing) Then
								oParameter.Value = DirectCast(row.title, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("medium").AllowUpdate) Then
							oParameter = New OleDbParameter("medium", OleDbType.VarWChar)
							If (row.medium IsNot Nothing) Then
								oParameter.Value = DirectCast(row.medium, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("size").AllowUpdate) Then
							oParameter = New OleDbParameter("size", OleDbType.VarWChar)
							If (row.size IsNot Nothing) Then
								oParameter.Value = DirectCast(row.size, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("price").AllowUpdate) Then
							oParameter = New OleDbParameter("price", OleDbType.VarWChar)
							If (row.price IsNot Nothing) Then
								oParameter.Value = DirectCast(row.price, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("color").AllowUpdate) Then
							oParameter = New OleDbParameter("color", OleDbType.VarWChar)
							If (row.color IsNot Nothing) Then
								oParameter.Value = DirectCast(row.color, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("subject").AllowUpdate) Then
							oParameter = New OleDbParameter("subject", OleDbType.VarWChar)
							If (row.subject IsNot Nothing) Then
								oParameter.Value = DirectCast(row.subject, Object)
							Else
								oParameter.Value = DBNull.Value
							End If
							oCmd.Parameters.Add(oParameter) ' Update or Remove
						End If
						If (row.Attribute("sold").AllowUpdate) Then
							oParameter = New OleDbParameter("sold", OleDbType.Boolean)
							If (row.sold.HasValue) Then
								oParameter.Value = DirectCast(row.sold, Object)
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
						row.ImageID = Db.LastInsertedID(oConn) ' load auto-generated ID
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
				Throw New ImageDataException(strDbErrorMessage)
			End If
			Return Nothing
		End Function

		' **************************************
		' *  Delete to Database by key field(s)
		' **************************************

		Public Sub Delete(ByVal ImageID As Int32) 
			Dim oParameter As OleDbParameter = Nothing
			Dim bStatus As Boolean 

			' Get Connection String
			Dim sConnStr As String = Db.ConnStr

			' Construct SQL statement
			Dim oInfo As Imageinf = New Imageinf()
			Dim oSql As SqlBuilder = New SqlBuilder(Db.DbType, Db.QuoteS, Db.QuoteE)
			Dim sSql As String = oSql.GetSqlCommand(oInfo.TableInfo, Database.SqlType.Delete, True, String.Empty, String.Empty)
			Try
				Using oConn As DbConnection = Db.GetConnection()
					Using oCmd As DbCommand = Db.GetCommand(sSql, oConn)

						' Key parameters
						oParameter = New OleDbParameter("ImageID", OleDbType.Integer)
						oParameter.Value = ImageID
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
				Throw New ImageDataException(strDbErrorMessage)
			End If
		End Sub

		' *****************************
		' *  Delete to Database by row
		' *****************************

		Public Sub Delete(ByVal row As Imagerow )
			Me.Delete(CType(row.ImageID, Int32))
		End Sub

		' ***************************
		' *  Multiple Delete by keys
		' ***************************

		Public Sub Delete(ByVal keys As ArrayList)
			Dim bStatus As Boolean 
			Dim parameters As DbParameterCollection = Db.GetParameterCollection()

			' Construct SQL statement
			Dim strWhere As String = KeyFilter(keys, parameters)
			Dim oInfo As Imageinf = New Imageinf()
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
				Throw New ImageDataException(strDbErrorMessage)
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Imagekey)), ")"))
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
				strWhere.Append(String.Concat("(", KeyFilter(DirectCast(keys(i), Imagekey), parameters, i), ")"))
				i += 1
			Loop
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Imagekey) As String 
			Dim strWhere As StringBuilder = New StringBuilder()

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append(String.Concat("[ImageID] = ", key.ImageID)) ' [ImageID] field
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Imagekey, ByRef parameters As DbParameterCollection) As String 
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ImageID] = " & Database.GetSqlParameterName(Db.DbType, "key__ImageID"))
			oParameter = New OleDbParameter("key__ImageID", OleDbType.Integer)
			oParameter.Value = key.ImageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ********************************
		' *  Get key filter by single key
		' ********************************	

		Public Overridable Function KeyFilter(ByVal key As Imagekey, ByRef parameters As DbParameterCollection, ByVal index As Integer) As String
			Dim strWhere As StringBuilder = New StringBuilder()
			Dim oParameter As OleDbParameter = Nothing

			' Key parameters
			If (strWhere.Length > 0) Then strWhere.Append(" AND ")
			strWhere.Append("[ImageID] = " & Database.GetSqlParameterName(Db.DbType, "key__ImageID" & index.ToString())) ' [ImageID] field
			oParameter = New OleDbParameter("key__ImageID" + index.ToString(), OleDbType.Integer)
			oParameter.Value = key.ImageID
			parameters.Add(oParameter)
			Return strWhere.ToString()
		End Function

		' ************************
		' *  Data Exception Class
		' ************************

		Public Class ImageDataException 
			Inherits Exception
			Public Sub New (ByVal msg As String) 
				MyBase.New(msg)
			End Sub
		End Class
	End Class
End Namespace
