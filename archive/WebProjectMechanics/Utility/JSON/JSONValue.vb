

Public Class JSONValue
    Private _type As JSONType,
            _value As Object

    Public Sub New(ByVal type As JSONType, ByVal value As Object)
        _type = type
        _value = value
    End Sub

    Public ReadOnly Property Type() As JSONType
        Get
            Return _type
        End Get
    End Property

    Public ReadOnly Property Value() As Object
        Get
            Return _value
        End Get
    End Property
End Class
