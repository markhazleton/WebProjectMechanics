
Public Class Evaluator
    Friend mEnvironmentFunctionsList As ArrayList
    Public RaiseVariableNotFoundException As Boolean
    Public ReadOnly Syntax As eParserSyntax
    Public ReadOnly CaseSensitive As Boolean

    Sub New(Optional ByVal syntax As eParserSyntax = eParserSyntax.vb, Optional ByVal caseSensitive As Boolean = False)
        Me.Syntax = syntax
        Me.CaseSensitive = caseSensitive
        mEnvironmentFunctionsList = New ArrayList
    End Sub

    Public Sub AddEnvironmentFunctions(ByVal obj As Object)
        If obj Is Nothing Then Exit Sub
        If Not mEnvironmentFunctionsList.Contains(obj) Then
            mEnvironmentFunctionsList.Add(obj)
        End If
    End Sub

    Public Sub RemoveEnvironmentFunctions(ByVal obj As Object)
        If mEnvironmentFunctionsList.Contains(obj) Then
            mEnvironmentFunctionsList.Remove(obj)
        End If
    End Sub

    Public Function Parse(ByVal str As String) As opCode
        Return New parser(Me).Parse(str)
    End Function

    Public Shared Function ConvertToString(ByVal value As Object) As String
        Dim sReturn As String = String.Empty
        If TypeOf value Is String Then
            sReturn = DirectCast(value, String)
        ElseIf value Is Nothing Then
            sReturn = String.Empty
        ElseIf TypeOf value Is Date Then
            Dim d As Date = DirectCast(value, Date)
            If d.TimeOfDay.TotalMilliseconds > 0 Then
                sReturn = d.ToString
            Else
                sReturn = d.ToShortDateString()
            End If
        ElseIf TypeOf value Is Decimal Then
            Dim d As Decimal = DirectCast(value, Decimal)
            If (d Mod 1) <> 0 Then
                sReturn = d.ToString("#,##0.00")
            Else
                sReturn = d.ToString("#,##0")
            End If
        ElseIf TypeOf value Is Double Then
            Dim d As Double = DirectCast(value, Double)
            If (d Mod 1) <> 0 Then
                sReturn = d.ToString("#,##0.00")
            Else
                sReturn = d.ToString("#,##0")
            End If
        ElseIf TypeOf value Is Object Then
            sReturn = value.ToString
        End If
        Return sReturn
    End Function

    Public Class parserException
        Inherits Exception
        Public ReadOnly formula As String
        Public ReadOnly pos As Integer

        Friend Sub New(ByVal str As String, ByVal formula As String, ByVal pos As Integer)
            MyBase.New(str)
            Me.formula = formula
            Me.pos = pos
        End Sub

    End Class

End Class
