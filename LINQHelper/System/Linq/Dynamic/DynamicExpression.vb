Option Strict On

Option Explicit On

Imports System.Linq.Expressions

Namespace System.Linq.Dynamic
    Public Module DynamicExpression
        Public Function Parse(ByVal resultType As Type, ByVal expression As String, ByVal ParamArray values() As Object) As Expression
            Dim parser As New ExpressionParser(Nothing, expression, values)
            Return parser.Parse(resultType)
        End Function

        Public Function ParseLambda(ByVal itType As Type, ByVal resultType As Type, ByVal expressionStr As String, ByVal ParamArray values() As Object) As LambdaExpression
            Return ParseLambda(New ParameterExpression() {Expression.Parameter(itType, "")}, resultType, expressionStr, values)
        End Function

        Public Function ParseLambda(ByVal parameters() As ParameterExpression, ByVal resultType As Type, ByVal expressionStr As String, ByVal ParamArray values() As Object) As LambdaExpression
            Dim parser As New ExpressionParser(parameters, expressionStr, values)
            Return Expression.Lambda(parser.Parse(resultType), parameters)
        End Function

        Public Function ParseLambda(Of T, S)(ByVal expression As String, ByVal ParamArray values() As Object) As Expression(Of Func(Of T, S))
            Return DirectCast(ParseLambda(GetType(T), GetType(S), expression, values), Expression(Of Func(Of T, S)))
        End Function

        Public Function CreateClass(ByVal ParamArray properties() As DynamicProperty) As Type
            Return ClassFactory.Instance.GetDynamicClass(properties)
        End Function

        Public Function CreateClass(ByVal properties As IEnumerable(Of DynamicProperty)) As Type
            Return ClassFactory.Instance.GetDynamicClass(properties)
        End Function
    End Module
End Namespace
