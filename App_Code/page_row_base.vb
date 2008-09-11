Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Pagerow_base
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

		Public Function GetKey() As Pagekey
			Dim key As Pagekey = New Pagekey()
			key.PageID = CType(Me.PageID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("PageID", GetType(Nullable(Of Int32))) ' PageID Attribute
			Add("PageOrder", GetType(Nullable(Of Int32))) ' PageOrder Attribute
			Add("PageName", GetType(String)) ' PageName Attribute
			Add("PageTitle", GetType(String)) ' PageTitle Attribute
			Add("PageDescription", GetType(String)) ' PageDescription Attribute
			Add("PageKeywords", GetType(String)) ' PageKeywords Attribute
			Add("PageTypeID", GetType(Nullable(Of Int32))) ' PageTypeID Attribute
			Add("ImagesPerRow", GetType(Nullable(Of Int32))) ' ImagesPerRow Attribute
			Add("RowsPerPage", GetType(Nullable(Of Int32))) ' RowsPerPage Attribute
			Add("ParentPageID", GetType(Nullable(Of Int32))) ' ParentPageID Attribute
			Add("Active", GetType(Nullable(Of Boolean))) ' Active Attribute
			Add("CompanyID", GetType(Nullable(Of Int32))) ' CompanyID Attribute
			Add("PageFileName", GetType(String)) ' PageFileName Attribute
			Add("GroupID", GetType(Nullable(Of Int32))) ' GroupID Attribute
			Add("ModifiedDT", GetType(Nullable(Of DateTime))) ' ModifiedDT Attribute
			Add("VersionNo", GetType(Nullable(Of Int32))) ' VersionNo Attribute
			Add("AllowMessage", GetType(Nullable(Of Boolean))) ' AllowMessage Attribute
			Add("SiteCategoryID", GetType(Nullable(Of Int32))) ' SiteCategoryID Attribute
			Add("SiteCategoryGroupID", GetType(Nullable(Of Int32))) ' SiteCategoryGroupID Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Pagerow = (DirectCast(obj, Pagerow))
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
			Dim info As Pageinf = New Pageinf()
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
		''' PageOrder Attribute
		''' </summary>

		Public Overridable Property PageOrder As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("PageOrder"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("PageOrder") = value
			End Set
		End Property

		''' <summary>
		''' PageName Attribute
		''' </summary>

		Public Overridable Property PageName As String
			Get 
				Return DirectCast(_Fields("PageName"), String)
			End Get
			Set 
				_Fields("PageName") = value
			End Set
		End Property

		''' <summary>
		''' PageTitle Attribute
		''' </summary>

		Public Overridable Property PageTitle As String
			Get 
				Return DirectCast(_Fields("PageTitle"), String)
			End Get
			Set 
				_Fields("PageTitle") = value
			End Set
		End Property

		''' <summary>
		''' PageDescription Attribute
		''' </summary>

		Public Overridable Property PageDescription As String
			Get 
				Return DirectCast(_Fields("PageDescription"), String)
			End Get
			Set 
				_Fields("PageDescription") = value
			End Set
		End Property

		''' <summary>
		''' PageKeywords Attribute
		''' </summary>

		Public Overridable Property PageKeywords As String
			Get 
				Return DirectCast(_Fields("PageKeywords"), String)
			End Get
			Set 
				_Fields("PageKeywords") = value
			End Set
		End Property

		''' <summary>
		''' PageTypeID Attribute
		''' </summary>

		Public Overridable Property PageTypeID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("PageTypeID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("PageTypeID") = value
			End Set
		End Property

		''' <summary>
		''' ImagesPerRow Attribute
		''' </summary>

		Public Overridable Property ImagesPerRow As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ImagesPerRow"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ImagesPerRow") = value
			End Set
		End Property

		''' <summary>
		''' RowsPerPage Attribute
		''' </summary>

		Public Overridable Property RowsPerPage As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("RowsPerPage"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("RowsPerPage") = value
			End Set
		End Property

		''' <summary>
		''' ParentPageID Attribute
		''' </summary>

		Public Overridable Property ParentPageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ParentPageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ParentPageID") = value
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
		''' PageFileName Attribute
		''' </summary>

		Public Overridable Property PageFileName As String
			Get 
				Return DirectCast(_Fields("PageFileName"), String)
			End Get
			Set 
				_Fields("PageFileName") = value
			End Set
		End Property

		''' <summary>
		''' GroupID Attribute
		''' </summary>

		Public Overridable Property GroupID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("GroupID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("GroupID") = value
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
		''' AllowMessage Attribute
		''' </summary>

		Public Overridable Property AllowMessage As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("AllowMessage"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("AllowMessage") = value
			End Set
		End Property

		''' <summary>
		''' SiteCategoryID Attribute
		''' </summary>

		Public Overridable Property SiteCategoryID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("SiteCategoryID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("SiteCategoryID") = value
			End Set
		End Property

		''' <summary>
		''' SiteCategoryGroupID Attribute
		''' </summary>

		Public Overridable Property SiteCategoryGroupID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("SiteCategoryGroupID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("SiteCategoryGroupID") = value
			End Set
		End Property
	End Class
End Namespace
