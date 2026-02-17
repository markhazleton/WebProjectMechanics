Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection

Public Class OnMethodInvokeArgs
    Inherits CancelEventArgs

    Protected Friend Sub New(method__1 As MethodInfo)
        Method = method__1
    End Sub

    Public Property Method() As MethodInfo
        Get
            Return m_Method
        End Get
        Private Set
            m_Method = Value
        End Set
    End Property
    Private m_Method As MethodInfo

End Class
