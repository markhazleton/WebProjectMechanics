
Public Class FlashGame
    Private _name As String
    Private _gameurl As String
    Private _zipurl As String
    Private _thumbnail_large_url As String
    Private _description As String
    Private _height As Integer
    Private _width As Integer
    Private _category As String
    Private _author As String
    Private _tags As List(Of String)
    Private _categories As List(Of String)
    Private _JSON As String

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            If _name = value Then
                Return
            End If
            _name = value
        End Set
    End Property
    Public Property GameURL As String
        Get
            Return _gameurl
        End Get
        Set(value As String)
            If _gameurl = value Then
                Return
            End If
            _gameurl = value
        End Set
    End Property
    Public Property Zipurl As String
        Get
            Return _zipurl
        End Get
        Set(value As String)
            If _zipurl = Value Then
                Return
            End If
            _zipurl = Value
        End Set
    End Property
    Public Property thumbnail_large_url As String
        Get
            Return _thumbnail_large_url
        End Get
        Set(value As String)
            _thumbnail_large_url = value
        End Set
    End Property
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            If _description = value Then
                Return
            End If
            _description = value
        End Set
    End Property
    Public Property Height As Integer
        Get
            Return _height
        End Get
        Set(value As Integer)
            If _height = value Then
                Return
            End If
            _height = value
        End Set
    End Property
    Public Property Width As Integer
        Get
            Return _width
        End Get
        Set(value As Integer)
            If _width = value Then
                Return
            End If
            _width = value
        End Set
    End Property
    Public Property Category As String
        Get
            Return _category
        End Get
        Set(value As String)
            If _category = value Then
                Return
            End If
            _category = value
        End Set
    End Property
    Public Property Author As String
        Get
            Return _author
        End Get
        Set(value As String)
            If _author = value Then
                Return
            End If
            _author = value
        End Set
    End Property
    Public Property Tags As List(Of String)
        Get
            Return _tags
        End Get
        Set(value As List(Of String))
            _tags = value
        End Set
    End Property
    Public Property Categories As List(Of String)
        Get
            Return _categories
        End Get
        Set(value As List(Of String))
            _categories = value
        End Set
    End Property
    Public Property JSON As String
        Get
            Return _JSON
        End Get
        Set(value As String)
            If _JSON = value Then
                Return
            End If
            _JSON = value
        End Set
    End Property


End Class
