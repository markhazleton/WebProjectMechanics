Option Strict On

Option Explicit On


Namespace System.Linq.Dynamic
    Public Class DynamicProperty
        Private _name As String
        Private _type As Type

        Public Sub New(ByVal name As String, ByVal type As Type)
            If (name Is Nothing) Then Throw New ArgumentNullException("name")
            If (type Is Nothing) Then Throw New ArgumentNullException("type")
            Me._name = name
            Me._type = type
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property Type() As Type
            Get
                Return _type
            End Get
        End Property
    End Class
End Namespace
