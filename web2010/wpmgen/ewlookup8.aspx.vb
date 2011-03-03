Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Lookup)
'

Partial Class ewlookup8
	Inherits AspNetMaker8_wpmWebsite
	Dim lookup As clookup

	' Page class for lookup
	Class clookup
		Inherits AspNetMakerPage	

		' Constructor
		Public Sub New(ByRef APage As AspNetMaker8_wpmWebsite)
			m_ParentPage = APage
			m_Page = Me
			m_PageID = "lookup"
			m_PageObjName = "lookup"
			m_PageObjTypeName = "clookup"
		End Sub		

		' URL
		Public ReadOnly Property PageUrl() As String
			Get
				Return ew_CurrentPage() & "?"
			End Get
		End Property

		' Page Main
		Public Sub Page_Main()			
			If HttpContext.Current.Request.Querystring.Count > 0 Then
					Dim Sql As String = ew_Get("s")
					Sql = cTEA.Decrypt(Sql, EW_RANDOM_KEY)
					If Sql <> "" Then

						' Get the filter values (for "IN")
						Dim Value As String = ew_AdjustSql(ew_Get("f"))
						If Value <> "" Then
							Dim arValue() As String = Value.Split(New Char() {","c})
							Dim FldType As Integer = ew_ConvertToInt(ew_Get("lft")) ' Link field data type
							For ari As Integer = 0 To arValue.GetUpperBound(0)
								arValue(ari) = ew_QuotedValue(arValue(ari), FldType)
							Next
							Sql = Sql.Replace("{filter_value}", String.Join(",", arValue))
						End If

						' Get the query value (for "LIKE" or "=")
						Value = ew_AdjustSql(ew_Get("q"))
						If Value <> "" Then
							Sql = Sql.Replace("{query_value}", Value)
						End If

						' Get the lookup values
						If Not Sql.Contains("{filter_value}") AndAlso Not Sql.Contains("{query_value}") Then
							GetLookupValues(Sql)
						End If					
					End If
			End If
		End Sub

		' Get values from database
		Private Sub GetLookupValues(Sql As String)		
			Dim RsArr As ArrayList
			Dim str As String		

			' Connect to database
			Conn = New cConnection()
			Try
				RsArr = Conn.GetRows(Sql)
			Finally
				Conn.Dispose()
			End Try

			' Output		
			For Each Row As OrderedDictionary In RsArr
				For Each f As DictionaryEntry In Row
					str = Convert.ToString(f.Value)
					str = RemoveDelimiters(str)
					ew_Write(str & EW_FIELD_DELIMITER)
				Next
				ew_Write(EW_RECORD_DELIMITER)
			Next		
		End Sub

		' Process values
		Private Function RemoveDelimiters(str As String) As String
			Dim wrkstr As String = str
			If wrkstr.Length > 0 Then
				wrkstr = wrkstr.Replace(vbCr, " ")
				wrkstr = wrkstr.Replace(vbLf, " ")
				wrkstr = wrkstr.Replace(EW_RECORD_DELIMITER, "")
				wrkstr = wrkstr.Replace(EW_FIELD_DELIMITER, "")
			End If
			Return wrkstr
		End Function
	End Class

	' ASP.NET Page_Load event
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		lookup = New clookup(Me)
		lookup.Page_Main()	
	End Sub
End Class
