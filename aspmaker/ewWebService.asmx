<%@ WebService Language="VB" Class="PMGEN.WebService" %>
Imports System
Imports System.Web
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.Common
Imports PMGEN
Imports EW.Data
Imports EW.Web.UI.Controls
Namespace PMGEN

	''' <summary>
	''' Summary description for Lookup
	''' </summary>

	<WebService(Namespace:="http://tempuri.org/"),  _
	 WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1), _
	 System.Web.Script.Services.ScriptService()> _
	Public Class WebService
		Inherits System.Web.Services.WebService
		<WebMethod(), System.Web.Script.Services.ScriptMethod()>  _
		Public Function GetSuggest(ByVal prefixText As String, ByVal sqlCommand As String, ByVal count As Integer) As String()
			Dim filteredList As ArrayList = New ArrayList
			If ((sqlCommand IsNot Nothing)  _
						AndAlso (sqlCommand.Length > 0)) Then
				sqlCommand = AutoSuggestExtender.DecryptSqlCommand(sqlCommand, Share.RandomKey)
				sqlCommand = sqlCommand.Replace("@FILTER_VALUE", prefixText)
				Dim sProviderName As String = Db.ProviderName
				Dim sConnStr As String = Db.ConnStr
				Try 
					Dim oDr As DbDataReader = Database.GetDataReader(sProviderName, sConnStr, sqlCommand)
					If (oDr IsNot Nothing) Then
						Dim iCnt As Integer = 0
						While oDr.Read
							iCnt = (iCnt + 1)
							If (oDr.FieldCount > 1) Then
								filteredList.Add(Convert.ToString(oDr(0) & Share.ValueSeparator(iCnt) & Convert.ToString(oDr(1))))
							Else
								filteredList.Add(Convert.ToString(oDr(0)))
							End If
						End While
						oDr.Close
					End If
				Catch e As Exception
				End Try
			End If
			Return CType(filteredList.ToArray(GetType(System.String)),String())
		End Function
		<WebMethod(), System.Web.Script.Services.ScriptMethod()>  _
		Public Function GetDropDownList(ByVal knownCategoryValues As String, ByVal category As String, ByVal parentCategory As String, ByVal sqlCommand As String, ByVal displayFieldNames As String) As CascadingDropDownNameValue()
			Dim sProviderName As String = Db.ProviderName
			Dim sConnStr As String = Db.ConnStr
			sqlCommand = CascadingDropDown.DecryptSqlCommand(sqlCommand, Share.RandomKey)
			If ((parentCategory IsNot Nothing)  _
						AndAlso (parentCategory.Length > 0)) Then
				Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
				Dim key As String = Nothing
				If Not kv.ContainsKey(parentCategory) Then
					Return Nothing
				End If
				key = kv(parentCategory)
				sqlCommand = sqlCommand.Replace("@FILTER_VALUE", key)
			End If
			Dim values As List(Of CascadingDropDownNameValue) = New List(Of CascadingDropDownNameValue)
			Try 
				Dim dr As DbDataReader = Database.GetDataReader(sProviderName, sConnStr, sqlCommand)
				While dr.Read
					Dim dispValue As String = String.Empty
					Dim id As String = dr(0).ToString
					Dim arrFieldName() As String = displayFieldNames.Split(Microsoft.VisualBasic.ChrW(44))

					'if (dr.FieldCount > 2)
					'{
					'    dispValue = Convert.ToString(dr[1]) + ", " + Convert.ToString(dr[2]);
					'}
					'else if (dr.FieldCount > 1)
					'{
					'    dispValue = Convert.ToString(dr[1]);
					'}
					'else
					'{
					'    dispValue = Convert.ToString(dr[0]);
					'}

					dispValue = Convert.ToString(dr(arrFieldName(0)))
					Dim i As Integer = 1
					Do While (i < arrFieldName.Length)
						Dim strName As String = arrFieldName(i)
						dispValue = (dispValue + String.Concat(", ", Convert.ToString(dr(strName))))
						i = (i + 1)
					Loop
					values.Add(New CascadingDropDownNameValue(dispValue, id))
				End While
				dr.Close
			Catch e As Exception
			End Try
			Return values.ToArray
		End Function
		<WebMethod(), System.Web.Script.Services.ScriptMethod()>  _
		Public Function AddOption(ByVal lookupTableName As String, ByVal linkFieldName As String, ByVal displayFieldName As String, ByVal displayField2Name As String, ByVal parentFieldName As String, ByVal linkFieldValue As String, ByVal displayFieldValue As String, ByVal displayField2Value As String, ByVal parentFieldValue As String, ByVal linkFieldQuote As String, ByVal displayFieldQuote As String, ByVal displayField2Quote As String, ByVal parentFieldQuote As String) As String
			Dim sLeftQuote As String = Db.QuoteS
			Dim sRightQuote As String = Db.QuoteE
			If (displayFieldName = linkFieldName) Then
				displayFieldValue = linkFieldValue
			End If
			If (displayField2Name = linkFieldName) Then
				displayField2Value = linkFieldValue
			ElseIf (displayField2Name = displayFieldName) Then
				displayField2Value = displayFieldValue
			End If
			Dim bUseLinkField As Boolean = ((linkFieldName.Length > 0) AndAlso (linkFieldValue.Length > 0))
			Dim bUseDisplayField As Boolean = ((displayFieldName.Length > 0) AndAlso (displayFieldName <> linkFieldName)  _
						AndAlso (displayFieldValue.Length > 0))
			Dim bUseDisplayField2 As Boolean = ((displayField2Name.Length > 0)	AndAlso (displayField2Name <> linkFieldName)  _
						AndAlso (displayField2Name <> displayFieldName)  _
						AndAlso (displayField2Value.Length > 0))
			Dim bUseParentField As Boolean = ((parentFieldName.Length > 0) AndAlso (parentFieldValue.Length > 0) _
						AndAlso (parentFieldName <> linkFieldName)  _
						AndAlso (parentFieldName <> displayFieldName)  _
						AndAlso (parentFieldName <> displayField2Name))
			Dim sSql As String = string.Empty
			If bUseLinkField Then
				sSql &= (sLeftQuote & linkFieldName & sRightQuote)
			End If
			If bUseDisplayField Then
				If (sSql.Length > 0) Then
					sSql &= ","
				End If
				sSql &= (sLeftQuote & displayFieldName & sRightQuote)
			End If
			If bUseDisplayField2 Then
				If (sSql.Length > 0) Then
					sSql &= ","
				End If
				sSql &= (sLeftQuote & displayField2Name & sRightQuote)
			End If
			If bUseParentField Then
				If (sSql.Length > 0) Then
					sSql &= ","
				End If
				sSql &= (sLeftQuote & parentFieldName & sRightQuote)
			End If
			sSql = "SELECT DISTINCT " & sSql & " FROM " & sLeftQuote & lookupTableName & sRightQuote
			Dim sWhere As String = string.Empty
			If bUseLinkField Then
				If (sWhere.Length > 0) Then
					sWhere &= " AND "
				End If
				sWhere &= String.Concat(sLeftQuote, linkFieldName, sRightQuote, "=", linkFieldQuote, Db.AdjustSql(linkFieldValue), linkFieldQuote)
			End If
			If bUseDisplayField Then
				If (sWhere.Length > 0) Then
					sWhere &= " AND "
				End If
				sWhere &= String.Concat(sLeftQuote, displayFieldName, sRightQuote, "=", displayFieldQuote, Db.AdjustSql(displayFieldValue), displayFieldQuote)
			End If
			If bUseDisplayField2 Then
				If (sWhere.Length > 0) Then
					sWhere &= " AND "
				End If
				sWhere &= String.Concat(sLeftQuote, displayField2Name, sRightQuote, "=", displayField2Quote, Db.AdjustSql(displayField2Value), displayField2Quote)
			End If
			If bUseParentField Then
				If (sWhere.Length > 0) Then
					sWhere &= " AND "
				End If
				sWhere &= String.Concat(sLeftQuote, parentFieldName, sRightQuote, "=", parentFieldQuote, Db.AdjustSql(parentFieldValue), parentFieldQuote)
			End If
			sSql &= " WHERE " & sWhere
			Dim sProviderName As String = Db.ProviderName
			Dim sConnStr As String = Db.ConnStr
			Dim sErrorMsg As String = string.Empty
			Dim bError As Boolean = False
			Dim bOptionExist As Boolean = False
			Try 
				Using oDr As DbDataReader = Database.GetDataReader(sProviderName, sConnStr, sSql)
					bOptionExist = oDr.Read
				End Using
			Catch e As Exception
				sErrorMsg = e.Message
				bError = True
			End Try
			If Not bError Then
				If Not bOptionExist Then
					Dim sFieldList As String = string.Empty
					Dim sValueList As String = string.Empty
					If bUseLinkField Then
						sFieldList = String.Concat(sFieldList, sLeftQuote, linkFieldName, sRightQuote)
						sValueList = String.Concat(sValueList, linkFieldQuote, Db.AdjustSql(linkFieldValue), linkFieldQuote)
					End If
					If bUseDisplayField Then
						If (sFieldList.Length > 0) Then
							sFieldList &= ","
						End If
						sFieldList &= String.Concat(sLeftQuote, displayFieldName, sRightQuote)
						If (sValueList.Length > 0) Then
							sValueList &= ","
						End If
						sValueList &= String.Concat(displayFieldQuote, Db.AdjustSql(displayFieldValue), displayFieldQuote)
					End If
					If bUseDisplayField2 Then
						If (sFieldList.Length > 0) Then
							sFieldList &= ","
						End If
						sFieldList &= String.Concat(sLeftQuote, displayField2Name, sRightQuote)
						If (sValueList.Length > 0) Then
							sValueList &= ","
						End If
						sValueList &= String.Concat(displayField2Quote, Db.AdjustSql(displayField2Value), displayField2Quote)
					End If
					If bUseParentField Then
						If (sFieldList.Length > 0) Then
							sFieldList &= ","
						End If
						sFieldList &= String.Concat(sLeftQuote, parentFieldName, sRightQuote)
						If (sValueList.Length > 0) Then
							sValueList &= ","
						End If
						sValueList &= String.Concat(parentFieldQuote, Db.AdjustSql(parentFieldValue), parentFieldQuote)
					End If
					sSql = String.Concat("INSERT INTO ",sLeftQuote, lookupTableName, sRightQuote , " (", sFieldList, ") VALUES (", sValueList + ")")
					sErrorMsg = Db.ExecuteNonQuery(sSql)
					bError = (sErrorMsg <> string.Empty)
				Else
					sErrorMsg = "Option already exists"
					bError = True
				End If
			End If
			If Not bError Then
				If (linkFieldValue.Length = 0) Then ' Get new link field value
					sSql = String.Concat("SELECT ", sLeftQuote, linkFieldName, sRightQuote, " FROM ", sLeftQuote, lookupTableName, sRightQuote, " WHERE ", sWhere)
					Dim oDr As DbDataReader = Database.GetDataReader(sProviderName, sConnStr, sSql)
					Try 
						If oDr.Read Then
							linkFieldValue = Convert.ToString(oDr(0))
							If (displayFieldName = linkFieldName) Then
								displayFieldValue = linkFieldValue
							End If
							If (displayField2Name = linkFieldName) Then
								displayField2Value = linkFieldValue
							End If
						End If
					Catch e As Exception
						sErrorMsg = e.Message
						bError = True
					Finally
						If (oDr IsNot Nothing) Then
							oDr.Close
						End If
						oDr = Nothing
					End Try
				End If
			End If
			If Not bError Then
				Return String.Concat("OK" & vbCr, Convert.ToString(linkFieldValue), vbCr, Convert.ToString(displayFieldValue), vbCr, Convert.ToString(displayField2Value))
			Else
				Return sErrorMsg
			End If
		End Function
	End Class
End Namespace
