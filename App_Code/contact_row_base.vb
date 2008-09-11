Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Contactrow_base
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

		Public Function GetKey() As Contactkey
			Dim key As Contactkey = New Contactkey()
			key.ContactID = CType(Me.ContactID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("ContactID", GetType(Nullable(Of Int32))) ' ContactID Attribute
			Add("PrimaryContact", GetType(String)) ' PrimaryContact Attribute
			Add("LogonName", GetType(String)) ' LogonName Attribute
			Add("LogonPassword", GetType(String)) ' LogonPassword Attribute
			Add("EMail", GetType(String)) ' EMail Attribute
			Add("CompanyID", GetType(Nullable(Of Int32))) ' CompanyID Attribute
			Add("Active", GetType(Nullable(Of Boolean))) ' Active Attribute
			Add("GroupID", GetType(Nullable(Of Int32))) ' GroupID Attribute
			Add("CreateDT", GetType(Nullable(Of DateTime))) ' CreateDT Attribute
			Add("TemplatePrefix", GetType(String)) ' TemplatePrefix Attribute
			Add("FirstName", GetType(String)) ' FirstName Attribute
			Add("MiddleInitial", GetType(String)) ' MiddleInitial Attribute
			Add("LastName", GetType(String)) ' LastName Attribute
			Add("OfficePhone", GetType(String)) ' OfficePhone Attribute
			Add("HomePhone", GetType(String)) ' HomePhone Attribute
			Add("MobilPhone", GetType(String)) ' MobilPhone Attribute
			Add("Pager", GetType(String)) ' Pager Attribute
			Add("URL", GetType(String)) ' URL Attribute
			Add("Address1", GetType(String)) ' Address1 Attribute
			Add("Address2", GetType(String)) ' Address2 Attribute
			Add("City", GetType(String)) ' City Attribute
			Add("State", GetType(String)) ' State Attribute
			Add("Country", GetType(String)) ' Country Attribute
			Add("PostalCode", GetType(String)) ' PostalCode Attribute
			Add("Biography", GetType(String)) ' Biography Attribute
			Add("Paid", GetType(Nullable(Of Int32))) ' Paid Attribute
			Add("email_subscribe", GetType(Nullable(Of Boolean))) ' email_subscribe Attribute
			Add("RoleID", GetType(Nullable(Of Int32))) ' RoleID Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Contactrow = (DirectCast(obj, Contactrow))
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
			Dim info As Contactinf = New Contactinf()
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
		''' PrimaryContact Attribute
		''' </summary>

		Public Overridable Property PrimaryContact As String
			Get 
				Return DirectCast(_Fields("PrimaryContact"), String)
			End Get
			Set 
				_Fields("PrimaryContact") = value
			End Set
		End Property

		''' <summary>
		''' LogonName Attribute
		''' </summary>

		Public Overridable Property LogonName As String
			Get 
				Return DirectCast(_Fields("LogonName"), String)
			End Get
			Set 
				_Fields("LogonName") = value
			End Set
		End Property

		''' <summary>
		''' LogonPassword Attribute
		''' </summary>

		Public Overridable Property LogonPassword As String
			Get 
				Return DirectCast(_Fields("LogonPassword"), String)
			End Get
			Set 
				_Fields("LogonPassword") = value
			End Set
		End Property

		''' <summary>
		''' EMail Attribute
		''' </summary>

		Public Overridable Property EMail As String
			Get 
				Return DirectCast(_Fields("EMail"), String)
			End Get
			Set 
				_Fields("EMail") = value
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
		''' CreateDT Attribute
		''' </summary>

		Public Overridable Property CreateDT As Nullable(Of DateTime)
			Get 
				Return DirectCast(_Fields("CreateDT"), Nullable(Of DateTime))
			End Get
			Set 
				_Fields("CreateDT") = value
			End Set
		End Property

		''' <summary>
		''' TemplatePrefix Attribute
		''' </summary>

		Public Overridable Property TemplatePrefix As String
			Get 
				Return DirectCast(_Fields("TemplatePrefix"), String)
			End Get
			Set 
				_Fields("TemplatePrefix") = value
			End Set
		End Property

		''' <summary>
		''' FirstName Attribute
		''' </summary>

		Public Overridable Property FirstName As String
			Get 
				Return DirectCast(_Fields("FirstName"), String)
			End Get
			Set 
				_Fields("FirstName") = value
			End Set
		End Property

		''' <summary>
		''' MiddleInitial Attribute
		''' </summary>

		Public Overridable Property MiddleInitial As String
			Get 
				Return DirectCast(_Fields("MiddleInitial"), String)
			End Get
			Set 
				_Fields("MiddleInitial") = value
			End Set
		End Property

		''' <summary>
		''' LastName Attribute
		''' </summary>

		Public Overridable Property LastName As String
			Get 
				Return DirectCast(_Fields("LastName"), String)
			End Get
			Set 
				_Fields("LastName") = value
			End Set
		End Property

		''' <summary>
		''' OfficePhone Attribute
		''' </summary>

		Public Overridable Property OfficePhone As String
			Get 
				Return DirectCast(_Fields("OfficePhone"), String)
			End Get
			Set 
				_Fields("OfficePhone") = value
			End Set
		End Property

		''' <summary>
		''' HomePhone Attribute
		''' </summary>

		Public Overridable Property HomePhone As String
			Get 
				Return DirectCast(_Fields("HomePhone"), String)
			End Get
			Set 
				_Fields("HomePhone") = value
			End Set
		End Property

		''' <summary>
		''' MobilPhone Attribute
		''' </summary>

		Public Overridable Property MobilPhone As String
			Get 
				Return DirectCast(_Fields("MobilPhone"), String)
			End Get
			Set 
				_Fields("MobilPhone") = value
			End Set
		End Property

		''' <summary>
		''' Pager Attribute
		''' </summary>

		Public Overridable Property Pager As String
			Get 
				Return DirectCast(_Fields("Pager"), String)
			End Get
			Set 
				_Fields("Pager") = value
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
		''' Address1 Attribute
		''' </summary>

		Public Overridable Property Address1 As String
			Get 
				Return DirectCast(_Fields("Address1"), String)
			End Get
			Set 
				_Fields("Address1") = value
			End Set
		End Property

		''' <summary>
		''' Address2 Attribute
		''' </summary>

		Public Overridable Property Address2 As String
			Get 
				Return DirectCast(_Fields("Address2"), String)
			End Get
			Set 
				_Fields("Address2") = value
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
		''' State Attribute
		''' </summary>

		Public Overridable Property State As String
			Get 
				Return DirectCast(_Fields("State"), String)
			End Get
			Set 
				_Fields("State") = value
			End Set
		End Property

		''' <summary>
		''' Country Attribute
		''' </summary>

		Public Overridable Property Country As String
			Get 
				Return DirectCast(_Fields("Country"), String)
			End Get
			Set 
				_Fields("Country") = value
			End Set
		End Property

		''' <summary>
		''' PostalCode Attribute
		''' </summary>

		Public Overridable Property PostalCode As String
			Get 
				Return DirectCast(_Fields("PostalCode"), String)
			End Get
			Set 
				_Fields("PostalCode") = value
			End Set
		End Property

		''' <summary>
		''' Biography Attribute
		''' </summary>

		Public Overridable Property Biography As String
			Get 
				Return DirectCast(_Fields("Biography"), String)
			End Get
			Set 
				_Fields("Biography") = value
			End Set
		End Property

		''' <summary>
		''' Paid Attribute
		''' </summary>

		Public Overridable Property Paid As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("Paid"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("Paid") = value
			End Set
		End Property

		''' <summary>
		''' email_subscribe Attribute
		''' </summary>

		Public Overridable Property email_subscribe As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("email_subscribe"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("email_subscribe") = value
			End Set
		End Property

		''' <summary>
		''' RoleID Attribute
		''' </summary>

		Public Overridable Property RoleID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("RoleID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("RoleID") = value
			End Set
		End Property
	End Class
End Namespace
