Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class SiteCategoryGrouprow_base
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

		Public Function GetKey() As SiteCategoryGroupkey
			Dim key As SiteCategoryGroupkey = New SiteCategoryGroupkey()
			key.SiteCategoryGroupID = CType(Me.SiteCategoryGroupID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("SiteCategoryGroupID", GetType(Nullable(Of Int32))) ' SiteCategoryGroupID Attribute
			Add("SiteCategoryGroupNM", GetType(String)) ' SiteCategoryGroupNM Attribute
			Add("SiteCategoryGroupDS", GetType(String)) ' SiteCategoryGroupDS Attribute
			Add("SiteCategoryGroupOrder", GetType(Nullable(Of Int32))) ' SiteCategoryGroupOrder Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As SiteCategoryGrouprow = (DirectCast(obj, SiteCategoryGrouprow))
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
			Dim info As SiteCategoryGroupinf = New SiteCategoryGroupinf()
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

		''' <summary>
		''' SiteCategoryGroupNM Attribute
		''' </summary>

		Public Overridable Property SiteCategoryGroupNM As String
			Get 
				Return DirectCast(_Fields("SiteCategoryGroupNM"), String)
			End Get
			Set 
				_Fields("SiteCategoryGroupNM") = value
			End Set
		End Property

		''' <summary>
		''' SiteCategoryGroupDS Attribute
		''' </summary>

		Public Overridable Property SiteCategoryGroupDS As String
			Get 
				Return DirectCast(_Fields("SiteCategoryGroupDS"), String)
			End Get
			Set 
				_Fields("SiteCategoryGroupDS") = value
			End Set
		End Property

		''' <summary>
		''' SiteCategoryGroupOrder Attribute
		''' </summary>

		Public Overridable Property SiteCategoryGroupOrder As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("SiteCategoryGroupOrder"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("SiteCategoryGroupOrder") = value
			End Set
		End Property
	End Class
End Namespace
