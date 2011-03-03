Public Interface IKeywordPredicateClass
    Function PredicateFunction(ByVal bo As wpmKeyword) As Boolean
End Interface
Public Class KeywordPredicateClass
    Implements IKeywordPredicateClass
    Private code As String
    Public Sub New(ByVal code As String)
        Me.code = LCase(Trim(code))
    End Sub
    Public Function PredicateFunction(ByVal bo As wpmKeyword) As Boolean Implements IKeywordPredicateClass.PredicateFunction
        Return LCase(Trim(bo.Code)) = LCase(Trim(Me.code))
    End Function
End Class
