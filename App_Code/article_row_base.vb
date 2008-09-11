Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Articlerow_base
		Private _Att As Dictionary(Of String, RowAtt)= New Dictionary(Of String, RowAtt)()
		Private _Att_index As Dictionary(Of Integer, RowAtt)= New Dictionary(Of Integer, RowAtt)()
		Private _Fields As Dictionary(Of String, Object)  = New Dictionary(Of String, Object)()

		''' <summary>
		''' Gets the number of fields contained
		''' </summary>

		Public ReadOnly Property Count As Integer
			Get
				Return _Att.Count
			End Get
		End Property

		''' <summary>
		''' Gets Collection of Fields
		''' </summary>

		Public Function GetFields() As Dictionary(Of String, Object)
			Return _Fields
		End Function

		''' <summary>
		''' Gets Attributes of a field
		''' </summary>

		Public Overloads Function Attribute(ByVal key As String) As RowAtt
			If _Att.ContainsKey(key) Then
				Return _Att(key)
			Else
				Return Nothing
			End If
		End Function

		''' <summary>
		''' Gets Attributes of a field
		''' </summary>

		Public Overloads Function Attribute(ByVal index As Int32) As RowAtt
			If ((index >= 1) AndAlso (index <= _Att_index.Count)) Then
				Return _Att_index(index)
			Else
				Return Nothing
			End If
		End Function

		''' <summary>
		''' Adds a field
		''' </summary>

		Public Sub Add(ByVal key As String, ByVal type As Type)
			Dim att As RowAtt = New RowAtt
			att.FieldType = type
			_Att.Add(key, att)
			_Att_index.Add(_Att.Count, _Att(key))
			_Fields.Add(key, Nothing)
		End Sub

		''' <summary>
		''' Gets a key object
		''' </summary>

		Public Function GetKey() As Articlekey
			Dim key As Articlekey = New Articlekey()
			key.ArticleID = CType(Me.ArticleID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("ArticleID", GetType(Nullable(Of Int32))) ' ArticleID Attribute
			Add("Title", GetType(String)) ' Title Attribute
			Add("Description", GetType(String)) ' Description Attribute
			Add("ArticleSummary", GetType(String)) ' ArticleSummary Attribute
			Add("ArticleBody", GetType(String)) ' ArticleBody Attribute
			Add("CompanyID", GetType(Nullable(Of Int32))) ' CompanyID Attribute
			Add("PageID", GetType(Nullable(Of Int32))) ' PageID Attribute
			Add("Active", GetType(Nullable(Of Boolean))) ' Active Attribute
			Add("StartDT", GetType(Nullable(Of DateTime))) ' StartDT Attribute
			Add("EndDT", GetType(Nullable(Of DateTime))) ' EndDT Attribute
			Add("ExpireDT", GetType(Nullable(Of DateTime))) ' ExpireDT Attribute
			Add("ContactID", GetType(Nullable(Of Int32))) ' ContactID Attribute
			Add("ModifiedDT", GetType(Nullable(Of DateTime))) ' ModifiedDT Attribute
			Add("VersionNo", GetType(Nullable(Of Int32))) ' VersionNo Attribute
			Add("Counter", GetType(Nullable(Of Int32))) ' Counter Attribute
			Add("Author", GetType(String)) ' Author Attribute
			Add("userID", GetType(Nullable(Of Int32))) ' userID Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Articlerow = (DirectCast(obj, Articlerow))
			Dim y As Dictionary(Of String, object) = row2.GetFields()
			Dim bEqual As Boolean = True
			For Each key As String In _Att.Keys
				If (row2.Attribute(key).AllowUpdate OrElse Attribute(key).AllowUpdate)
					Dim fieldType As Type = _Att(key).FieldType
					If _Fields(key) Is Nothing Then 
						If y(key) IsNot Nothing Then
							bEqual = False
							Exit For
						End If
					ElseIf y(key) Is Nothing Then
						If _Fields(key) IsNot Nothing Then
							bEqual = False
							Exit For
						End If
					ElseIf (Not Core.ChangeType(_Fields(key), fieldType).Equals(Core.ChangeType(y(key), fieldType))) Then
						bEqual = False
						Exit For
					End If
				End If
			Next
			Return bEqual
		End Function

		''' <summary>
		''' Returns the hash code of this object.
		''' </summary>

		Public Overrides Function GetHashCode() As Integer
			Return MyBase.GetHashCode()
		End Function

		''' <summary>
		''' Set up default values based on paged type
		''' </summary>

		Public Sub SetPageType(ByVal pt As Core.PageType)
			Dim info As Articleinf = New Articleinf()
			Dim tbl As Table = info.TableInfo
			Dim bUpdate As Boolean = false
			Dim i As Integer = 1
			Do While (i <= tbl.Count)
				Select Case (pt)
					Case Core.PageType.List
						bUpdate = (tbl.Fields(i).IsList AndAlso Not tbl.Fields(i).IsPrimaryKey)
					Case Core.PageType.View
						bUpdate = tbl.Fields(i).IsView
					Case Core.PageType.Edit
						bUpdate = tbl.Fields(i).IsEdit
					Case Core.PageType.Delete
						bUpdate = tbl.Fields(i).IsDelete
					Case Core.PageType.Add
						bUpdate = tbl.Fields(i).IsAdd
					Case Core.PageType.Search
						bUpdate = tbl.Fields(i).IsSearch
					Case Core.PageType.Register
						bUpdate = tbl.Fields(i).IsRegister
				End Select
				If tbl.Fields(i).IsAutoIncrement Then
					bUpdate = false
				End If

				' skip for auto-increment fields
				_Att(tbl.Fields(i).ParameterName).AllowUpdate = bUpdate

				' initialize update attribute
				i += 1
			Loop
		End Sub

		''' <summary>
		''' ArticleID Attribute
		''' </summary>

		Public Overridable Property ArticleID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ArticleID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ArticleID") = value
			End Set
		End Property

		''' <summary>
		''' Title Attribute
		''' </summary>

		Public Overridable Property Title As String
			Get 
				Return DirectCast(_Fields("Title"), String)
			End Get
			Set 
				_Fields("Title") = value
			End Set
		End Property

		''' <summary>
		''' Description Attribute
		''' </summary>

		Public Overridable Property Description As String
			Get 
				Return DirectCast(_Fields("Description"), String)
			End Get
			Set 
				_Fields("Description") = value
			End Set
		End Property

		''' <summary>
		''' ArticleSummary Attribute
		''' </summary>

		Public Overridable Property ArticleSummary As String
			Get 
				Return DirectCast(_Fields("ArticleSummary"), String)
			End Get
			Set 
				_Fields("ArticleSummary") = value
			End Set
		End Property

		''' <summary>
		''' ArticleBody Attribute
		''' </summary>

		Public Overridable Property ArticleBody As String
			Get 
				Return DirectCast(_Fields("ArticleBody"), String)
			End Get
			Set 
				_Fields("ArticleBody") = value
			End Set
		End Property

		''' <summary>
		''' CompanyID Attribute
		''' </summary>

		Public Overridable Property CompanyID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("CompanyID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("CompanyID") = value
			End Set
		End Property

		''' <summary>
		''' PageID Attribute
		''' </summary>

		Public Overridable Property PageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("PageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("PageID") = value
			End Set
		End Property

		''' <summary>
		''' Active Attribute
		''' </summary>

		Public Overridable Property Active As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("Active"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("Active") = value
			End Set
		End Property

		''' <summary>
		''' StartDT Attribute
		''' </summary>

		Public Overridable Property StartDT As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("StartDT"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("StartDT") = value
			End Set
		End Property

		''' <summary>
		''' EndDT Attribute
		''' </summary>

		Public Overridable Property EndDT As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("EndDT"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("EndDT") = value
			End Set
		End Property

		''' <summary>
		''' ExpireDT Attribute
		''' </summary>

		Public Overridable Property ExpireDT As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("ExpireDT"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("ExpireDT") = value
			End Set
		End Property

		''' <summary>
		''' ContactID Attribute
		''' </summary>

		Public Overridable Property ContactID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ContactID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ContactID") = value
			End Set
		End Property

		''' <summary>
		''' ModifiedDT Attribute
		''' </summary>

		Public Overridable Property ModifiedDT As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("ModifiedDT"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("ModifiedDT") = value
			End Set
		End Property

		''' <summary>
		''' VersionNo Attribute
		''' </summary>

		Public Overridable Property VersionNo As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("VersionNo"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("VersionNo") = value
			End Set
		End Property

		''' <summary>
		''' Counter Attribute
		''' </summary>

		Public Overridable Property Counter As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("Counter"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("Counter") = value
			End Set
		End Property

		''' <summary>
		''' Author Attribute
		''' </summary>

		Public Overridable Property Author As String
			Get 
				Return DirectCast(_Fields("Author"), String)
			End Get
			Set 
				_Fields("Author") = value
			End Set
		End Property

		''' <summary>
		''' userID Attribute
		''' </summary>

		Public Overridable Property userID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("userID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("userID") = value
			End Set
		End Property
	End Class
End Namespace
