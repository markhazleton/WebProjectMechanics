Imports System.Linq

Public Class HttpVerbNotAllowedException
    Inherits Exception
    Public Sub New()
        MyBase.New("The operation does not support the request HTTP verb.")
    End Sub
End Class
