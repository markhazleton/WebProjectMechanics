Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.Common
Imports EW.Data
Namespace PMGEN
	<Serializable()> _
	Public MustInherit Class Companyrow_base
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

		Public Function GetKey() As Companykey
			Dim key As Companykey = New Companykey()
			key.CompanyID = CType(Me.CompanyID,Int32) 
			Return key
		End Function

		' ***************
		' * Constructor
		' ***************

		Public Sub New()
			Add("CompanyID", GetType(Nullable(Of Int32))) ' CompanyID Attribute
			Add("CompanyName", GetType(String)) ' CompanyName Attribute
			Add("SiteTitle", GetType(String)) ' SiteTitle Attribute
			Add("SiteTemplate", GetType(String)) ' SiteTemplate Attribute
			Add("DefaultSiteTemplate", GetType(String)) ' DefaultSiteTemplate Attribute
			Add("GalleryFolder", GetType(String)) ' GalleryFolder Attribute
			Add("SiteURL", GetType(String)) ' SiteURL Attribute
			Add("City", GetType(String)) ' City Attribute
			Add("StateOrProvince", GetType(String)) ' StateOrProvince Attribute
			Add("Country", GetType(String)) ' Country Attribute
			Add("DefaultPaymentTerms", GetType(String)) ' DefaultPaymentTerms Attribute
			Add("DefaultInvoiceDescription", GetType(String)) ' DefaultInvoiceDescription Attribute
			Add("DefaultArticleID", GetType(Nullable(Of Int32))) ' DefaultArticleID Attribute
			Add("HomePageID", GetType(Nullable(Of Int32))) ' HomePageID Attribute
			Add("UseBreadCrumbURL", GetType(Nullable(Of Boolean))) ' UseBreadCrumbURL Attribute
			Add("SingleSiteGallery", GetType(Nullable(Of Boolean))) ' SingleSiteGallery Attribute
			Add("SiteCategoryTypeID", GetType(Nullable(Of Int32))) ' SiteCategoryTypeID Attribute
			Add("ActiveFL", GetType(Nullable(Of Boolean))) ' ActiveFL Attribute
			Add("Component", GetType(String)) ' Component Attribute
			Add("FromEmail", GetType(String)) ' FromEmail Attribute
			Add("SMTP", GetType(String)) ' SMTP Attribute
			Add("Address", GetType(String)) ' Address Attribute
			Add("PostalCode", GetType(String)) ' PostalCode Attribute
			Add("FaxNumber", GetType(String)) ' FaxNumber Attribute
			Add("PhoneNumber", GetType(String)) ' PhoneNumber Attribute
		End Sub

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overrides Function Equals(ByVal obj As Object) As Boolean
			If (obj Is Nothing) Then
				Return False
			End If
			Dim row2 As Companyrow = (DirectCast(obj, Companyrow))
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
			Dim info As Companyinf = New Companyinf()
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
		''' CompanyName Attribute
		''' </summary>

		Public Overridable Property CompanyName As String
			Get 
				Return DirectCast(_Fields("CompanyName"), String)
			End Get
			Set 
				_Fields("CompanyName") = value
			End Set
		End Property

		''' <summary>
		''' SiteTitle Attribute
		''' </summary>

		Public Overridable Property SiteTitle As String
			Get 
				Return DirectCast(_Fields("SiteTitle"), String)
			End Get
			Set 
				_Fields("SiteTitle") = value
			End Set
		End Property

		''' <summary>
		''' SiteTemplate Attribute
		''' </summary>

		Public Overridable Property SiteTemplate As String
			Get 
				Return DirectCast(_Fields("SiteTemplate"), String)
			End Get
			Set 
				_Fields("SiteTemplate") = value
			End Set
		End Property

		''' <summary>
		''' DefaultSiteTemplate Attribute
		''' </summary>

		Public Overridable Property DefaultSiteTemplate As String
			Get 
				Return DirectCast(_Fields("DefaultSiteTemplate"), String)
			End Get
			Set 
				_Fields("DefaultSiteTemplate") = value
			End Set
		End Property

		''' <summary>
		''' GalleryFolder Attribute
		''' </summary>

		Public Overridable Property GalleryFolder As String
			Get 
				Return DirectCast(_Fields("GalleryFolder"), String)
			End Get
			Set 
				_Fields("GalleryFolder") = value
			End Set
		End Property

		''' <summary>
		''' SiteURL Attribute
		''' </summary>

		Public Overridable Property SiteURL As String
			Get 
				Return DirectCast(_Fields("SiteURL"), String)
			End Get
			Set 
				_Fields("SiteURL") = value
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
		''' StateOrProvince Attribute
		''' </summary>

		Public Overridable Property StateOrProvince As String
			Get 
				Return DirectCast(_Fields("StateOrProvince"), String)
			End Get
			Set 
				_Fields("StateOrProvince") = value
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
		''' DefaultPaymentTerms Attribute
		''' </summary>

		Public Overridable Property DefaultPaymentTerms As String
			Get 
				Return DirectCast(_Fields("DefaultPaymentTerms"), String)
			End Get
			Set 
				_Fields("DefaultPaymentTerms") = value
			End Set
		End Property

		''' <summary>
		''' DefaultInvoiceDescription Attribute
		''' </summary>

		Public Overridable Property DefaultInvoiceDescription As String
			Get 
				Return DirectCast(_Fields("DefaultInvoiceDescription"), String)
			End Get
			Set 
				_Fields("DefaultInvoiceDescription") = value
			End Set
		End Property

		''' <summary>
		''' DefaultArticleID Attribute
		''' </summary>

		Public Overridable Property DefaultArticleID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("DefaultArticleID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("DefaultArticleID") = value
			End Set
		End Property

		''' <summary>
		''' HomePageID Attribute
		''' </summary>

		Public Overridable Property HomePageID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("HomePageID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("HomePageID") = value
			End Set
		End Property

		''' <summary>
		''' UseBreadCrumbURL Attribute
		''' </summary>

		Public Overridable Property UseBreadCrumbURL As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("UseBreadCrumbURL"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("UseBreadCrumbURL") = value
			End Set
		End Property

		''' <summary>
		''' SingleSiteGallery Attribute
		''' </summary>

		Public Overridable Property SingleSiteGallery As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("SingleSiteGallery"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("SingleSiteGallery") = value
			End Set
		End Property

		''' <summary>
		''' SiteCategoryTypeID Attribute
		''' </summary>

		Public Overridable Property SiteCategoryTypeID As Nullable(Of Int32)
			Get 
				Return DirectCast(_Fields("SiteCategoryTypeID"), Nullable(Of Int32))
			End Get
			Set 
				_Fields("SiteCategoryTypeID") = value
			End Set
		End Property

		''' <summary>
		''' ActiveFL Attribute
		''' </summary>

		Public Overridable Property ActiveFL As Nullable(Of Boolean)
			Get 
				Return DirectCast(_Fields("ActiveFL"), Nullable(Of Boolean))
			End Get
			Set 
				_Fields("ActiveFL") = value
			End Set
		End Property

		''' <summary>
		''' Component Attribute
		''' </summary>

		Public Overridable Property Component As String
			Get 
				Return DirectCast(_Fields("Component"), String)
			End Get
			Set 
				_Fields("Component") = value
			End Set
		End Property

		''' <summary>
		''' FromEmail Attribute
		''' </summary>

		Public Overridable Property FromEmail As String
			Get 
				Return DirectCast(_Fields("FromEmail"), String)
			End Get
			Set 
				_Fields("FromEmail") = value
			End Set
		End Property

		''' <summary>
		''' SMTP Attribute
		''' </summary>

		Public Overridable Property SMTP As String
			Get 
				Return DirectCast(_Fields("SMTP"), String)
			End Get
			Set 
				_Fields("SMTP") = value
			End Set
		End Property

		''' <summary>
		''' Address Attribute
		''' </summary>

		Public Overridable Property Address As String
			Get 
				Return DirectCast(_Fields("Address"), String)
			End Get
			Set 
				_Fields("Address") = value
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
		''' FaxNumber Attribute
		''' </summary>

		Public Overridable Property FaxNumber As String
			Get 
				Return DirectCast(_Fields("FaxNumber"), String)
			End Get
			Set 
				_Fields("FaxNumber") = value
			End Set
		End Property

		''' <summary>
		''' PhoneNumber Attribute
		''' </summary>

		Public Overridable Property PhoneNumber As String
			Get 
				Return DirectCast(_Fields("PhoneNumber"), String)
			End Get
			Set 
				_Fields("PhoneNumber") = value
			End Set
		End Property
	End Class
End Namespace
