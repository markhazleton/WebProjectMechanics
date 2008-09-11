Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Messagerow_base
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

		Public Function GetKey() As Messagekey
			Dim key As Messagekey = New Messagekey()
			key.MessageID = CType(Me.MessageID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("MessageID", GetType(Nullable(Of Int32))) ' MessageID Attribute
			Add("PageID", GetType(Nullable(Of Int32))) ' PageID Attribute
			Add("ParentMessageID", GetType(Nullable(Of Int32))) ' ParentMessageID Attribute
			Add("Subject", GetType(String)) ' Subject Attribute
			Add("Author", GetType(String)) ' Author Attribute
			Add("Email", GetType(String)) ' Email Attribute
			Add("City", GetType(String)) ' City Attribute
			Add("URL", GetType(String)) ' URL Attribute
			Add("MessageDate", GetType(Nullable(Of DateTime))) ' MessageDate Attribute
			Add("Body", GetType(String)) ' Body Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Messagerow = (DirectCast(obj, Messagerow))
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
			Dim info As Messageinf = New Messageinf()
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
		''' MessageID Attribute
		''' </summary>

		Public Overridable Property MessageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("MessageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("MessageID") = value
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
		''' ParentMessageID Attribute
		''' </summary>

		Public Overridable Property ParentMessageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ParentMessageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ParentMessageID") = value
			End Set
		End Property

		''' <summary>
		''' Subject Attribute
		''' </summary>

		Public Overridable Property Subject As String
			Get 
				Return DirectCast(_Fields("Subject"), String)
			End Get
			Set 
				_Fields("Subject") = value
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
		''' Email Attribute
		''' </summary>

		Public Overridable Property Email As String
			Get 
				Return DirectCast(_Fields("Email"), String)
			End Get
			Set 
				_Fields("Email") = value
			End Set
		End Property

		''' <summary>
		''' City Attribute
		''' </summary>

		Public Overridable Property City As String
			Get 
				Return DirectCast(_Fields("City"), String)
			End Get
			Set 
				_Fields("City") = value
			End Set
		End Property

		''' <summary>
		''' URL Attribute
		''' </summary>

		Public Overridable Property URL As String
			Get 
				Return DirectCast(_Fields("URL"), String)
			End Get
			Set 
				_Fields("URL") = value
			End Set
		End Property

		''' <summary>
		''' MessageDate Attribute
		''' </summary>

		Public Overridable Property MessageDate As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("MessageDate"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("MessageDate") = value
			End Set
		End Property

		''' <summary>
		''' Body Attribute
		''' </summary>

		Public Overridable Property Body As String
			Get 
				Return DirectCast(_Fields("Body"), String)
			End Get
			Set 
				_Fields("Body") = value
			End Set
		End Property
	End Class
End Namespace
