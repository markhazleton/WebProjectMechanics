Public Class SQLFilterClause
    Public Property Field As String
    Public Property FieldOperator As SQLFilterOperator
    Public Property Conjunction As SQLFilterConjunction
    Public Property FieldType As String
    Public Property Argument As String
    Public ReadOnly Property ClauseConjunction As String
        Get
            Select Case Conjunction
                Case SQLFilterConjunction.andConjunction
                    Return " and "
                Case SQLFilterConjunction.orConjunction
                    Return " or "
                Case Else
                    Return " and "
            End Select
        End Get
    End Property
    Public ReadOnly Property Statement As String
        Get
            If Field = String.Empty Then
                Return _Argument
            Else
                Select Case _FieldOperator
                    Case SQLFilterOperator.Equal
                        If IsNumeric(_Argument) Then
                            Return String.Format(" {0} {1} {2} ", Field, " = ", _Argument)
                        Else
                            Return String.Format(" {0}.Contains(""{1}"") ", Field, _Argument)
                        End If
                    Case SQLFilterOperator.NotEqual
                        Return String.Format(" {0} {1} '{2}' ", Field, " <> ", _Argument)
                    Case SQLFilterOperator.LessThanEqual
                        Return String.Format(" {0} {1} '{2}' ", Field, " <= ", _Argument)
                    Case SQLFilterOperator.LessThan
                        Return String.Format(" {0} {1} '{2}' ", Field, " < ", _Argument)
                    Case SQLFilterOperator.GreaterThanEqual
                        Return String.Format(" {0} {1} '{2}' ", Field, " >= ", _Argument)
                    Case SQLFilterOperator.GreaterThan
                        Return String.Format(" {0} {1} '{2}' ", Field, " > ", _Argument)
                    Case SQLFilterOperator.dbLike
                        Return String.Format(" {0} {1} '{2}' ", Field, " like ", _Argument)
                    Case SQLFilterOperator.dbIn
                        Return String.Format(" {0} {1} ({2}) ", Field, " in ", _Argument)
                    Case SQLFilterOperator.dbBetween
                        Return String.Format(" {0} {1} {2} ", Field, " between ", _Argument)
                    Case SQLFilterOperator.dbIsNotNull
                        Return String.Format(" {0} is not null ", Field)
                    Case SQLFilterOperator.dbIsNull
                        Return String.Format(" {0} is null ", Field)
                    Case Else
                        Return String.Format(" {0} {1} '{2}' ", Field, " = ", _Argument)
                End Select
            End If
        End Get
    End Property
    Public Sub New(ByVal myField As String, ByVal myOperator As SQLFilterOperator, ByVal myArgument As String, ByVal myConjunction As SQLFilterConjunction, ByVal myFieldType As String)
        Field = myField
        _FieldOperator = myOperator
        _Argument = myArgument
        Conjunction = myConjunction
        FieldType = myFieldType
    End Sub
    Public Sub New()

    End Sub
End Class
