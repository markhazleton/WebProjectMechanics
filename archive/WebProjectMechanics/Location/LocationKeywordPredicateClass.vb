Public Class LocationKeywordPredicateClass
    Implements ILocationKeywordPredicateClass
    Private ReadOnly code As String
    Public Sub New(ByVal code As String)
        Me.code = LCase(Trim(code))
    End Sub
    Public Function PredicateFunction(ByVal bo As LocationKeyword) As Boolean Implements ILocationKeywordPredicateClass.PredicateFunction
        Return LCase(Trim(bo.Code)) = LCase(Trim(code))
    End Function
End Class
