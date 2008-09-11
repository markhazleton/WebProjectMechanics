Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Imagerow_base
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

		Public Function GetKey() As Imagekey
			Dim key As Imagekey = New Imagekey()
			key.ImageID = CType(Me.ImageID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("ImageID", GetType(Nullable(Of Int32))) ' ImageID Attribute
			Add("ImageName", GetType(String)) ' ImageName Attribute
			Add("Active", GetType(Nullable(Of Boolean))) ' Active Attribute
			Add("ImageDescription", GetType(String)) ' ImageDescription Attribute
			Add("ImageComment", GetType(String)) ' ImageComment Attribute
			Add("ImageFileName", GetType(String)) ' ImageFileName Attribute
			Add("ImageThumbFileName", GetType(String)) ' ImageThumbFileName Attribute
			Add("ImageDate", GetType(Nullable(Of DateTime))) ' ImageDate Attribute
			Add("ModifiedDT", GetType(Nullable(Of DateTime))) ' ModifiedDT Attribute
			Add("VersionNo", GetType(Nullable(Of Int32))) ' VersionNo Attribute
			Add("ContactID", GetType(Nullable(Of Int32))) ' ContactID Attribute
			Add("CompanyID", GetType(Nullable(Of Int32))) ' CompanyID Attribute
			Add("title", GetType(String)) ' title Attribute
			Add("medium", GetType(String)) ' medium Attribute
			Add("size", GetType(String)) ' size Attribute
			Add("price", GetType(String)) ' price Attribute
			Add("color", GetType(String)) ' color Attribute
			Add("subject", GetType(String)) ' subject Attribute
			Add("sold", GetType(Nullable(Of Boolean))) ' sold Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Imagerow = (DirectCast(obj, Imagerow))
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
			Dim info As Imageinf = New Imageinf()
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
		''' ImageID Attribute
		''' </summary>

		Public Overridable Property ImageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("ImageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("ImageID") = value
			End Set
		End Property

		''' <summary>
		''' ImageName Attribute
		''' </summary>

		Public Overridable Property ImageName As String
			Get 
				Return DirectCast(_Fields("ImageName"), String)
			End Get
			Set 
				_Fields("ImageName") = value
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
		''' ImageDescription Attribute
		''' </summary>

		Public Overridable Property ImageDescription As String
			Get 
				Return DirectCast(_Fields("ImageDescription"), String)
			End Get
			Set 
				_Fields("ImageDescription") = value
			End Set
		End Property

		''' <summary>
		''' ImageComment Attribute
		''' </summary>

		Public Overridable Property ImageComment As String
			Get 
				Return DirectCast(_Fields("ImageComment"), String)
			End Get
			Set 
				_Fields("ImageComment") = value
			End Set
		End Property

		''' <summary>
		''' ImageFileName Attribute
		''' </summary>

		Public Overridable Property ImageFileName As String
			Get 
				Return DirectCast(_Fields("ImageFileName"), String)
			End Get
			Set 
				_Fields("ImageFileName") = value
			End Set
		End Property

		''' <summary>
		''' ImageThumbFileName Attribute
		''' </summary>

		Public Overridable Property ImageThumbFileName As String
			Get 
				Return DirectCast(_Fields("ImageThumbFileName"), String)
			End Get
			Set 
				_Fields("ImageThumbFileName") = value
			End Set
		End Property

		''' <summary>
		''' ImageDate Attribute
		''' </summary>

		Public Overridable Property ImageDate As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("ImageDate"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("ImageDate") = value
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
		''' title Attribute
		''' </summary>

		Public Overridable Property title As String
			Get 
				Return DirectCast(_Fields("title"), String)
			End Get
			Set 
				_Fields("title") = value
			End Set
		End Property

		''' <summary>
		''' medium Attribute
		''' </summary>

		Public Overridable Property medium As String
			Get 
				Return DirectCast(_Fields("medium"), String)
			End Get
			Set 
				_Fields("medium") = value
			End Set
		End Property

		''' <summary>
		''' size Attribute
		''' </summary>

		Public Overridable Property size As String
			Get 
				Return DirectCast(_Fields("size"), String)
			End Get
			Set 
				_Fields("size") = value
			End Set
		End Property

		''' <summary>
		''' price Attribute
		''' </summary>

		Public Overridable Property price As String
			Get 
				Return DirectCast(_Fields("price"), String)
			End Get
			Set 
				_Fields("price") = value
			End Set
		End Property

		''' <summary>
		''' color Attribute
		''' </summary>

		Public Overridable Property color As String
			Get 
				Return DirectCast(_Fields("color"), String)
			End Get
			Set 
				_Fields("color") = value
			End Set
		End Property

		''' <summary>
		''' subject Attribute
		''' </summary>

		Public Overridable Property subject As String
			Get 
				Return DirectCast(_Fields("subject"), String)
			End Get
			Set 
				_Fields("subject") = value
			End Set
		End Property

		''' <summary>
		''' sold Attribute
		''' </summary>

		Public Overridable Property sold As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("sold"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("sold") = value
			End Set
		End Property
	End Class
End Namespace
