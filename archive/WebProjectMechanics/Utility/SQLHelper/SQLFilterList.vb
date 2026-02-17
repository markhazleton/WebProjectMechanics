Imports System.Text

Public Class SQLFilterList
    Inherits List(Of SQLFilterClause)
    Public Sub New()
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of SQLFilterClause))
        MyBase.New(collection)
    End Sub
    Private SearchField As String
    Private Function FindClauseByField(ByVal FilterClause As SQLFilterClause) As boolean
        If FilterClause.Field = SearchField Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function FindField(ByVal reqSearchField As String) As list(of SQLFilterClause)
        SearchField = reqSearchField
        Return FindAll(AddressOf FindClauseByField)
    End Function

    Public Function GetWhereClause() As String
        Dim myReturn As StringBuilder = New StringBuilder(String.Empty)
        Dim iLoopIndex As Integer
        If Count = 1 Then
            myReturn.Append(String.Format("where {0} {1}", Me(0).Statement, vbCrLf))
        ElseIf Count > 1 Then
            For iLoopIndex = 0 To (Count - 1)
                If iLoopIndex = 0 Then
                    myReturn.Append(String.Format("where {0} {1}", Me(iLoopIndex).Statement, vbCrLf))
                Else
                    myReturn.Append(String.Format("{1}  {0} {2}", Me(iLoopIndex).Statement, Me(iLoopIndex).ClauseConjunction, vbCrLf))
                End If
            Next iLoopIndex
        End If
        Return myReturn.ToString
    End Function

    Public Function GetLINQWhere() As String
        Dim myReturn As StringBuilder = New StringBuilder(String.Empty)
        Dim iLoopIndex As Integer
        If Count = 1 Then
            myReturn.Append(String.Format("{0} {1}", Me(0).Statement, String.Empty))
        ElseIf Count > 1 Then
            For iLoopIndex = 0 To (Count - 1)
                If iLoopIndex = 0 Then
                    myReturn.Append(String.Format(" {0} {1}", Me(iLoopIndex).Statement, String.Empty))
                Else
                    myReturn.Append(String.Format("{1}  {0} {2}", Me(iLoopIndex).Statement, Me(iLoopIndex).ClauseConjunction, String.Empty))
                End If
            Next iLoopIndex
        End If
        Return myReturn.ToString
    End Function

    Public Function GetWhereClause(ByVal FieldType As String) As String
        Dim myReturn As StringBuilder = New StringBuilder(String.Empty)
        Dim iClauseCount As Integer = 0
        Dim iLoopIndex As Integer
        If Count = 1 Then
            If Me(0).FieldType = FieldType Then
                myReturn.Append(String.Format("where {0} {1}", Me(0).Statement, vbCrLf))
                iClauseCount = iClauseCount + 1
            End If
        ElseIf Count > 1 Then
            For iLoopIndex = 0 To (Count - 1)
                If Me(iLoopIndex).FieldType = FieldType Then
                    If iClauseCount = 0 Then
                        myReturn.Append(String.Format("where {0} {1}", Me(iLoopIndex).Statement, vbCrLf))
                        iClauseCount = iClauseCount + 1
                    Else
                        myReturn.Append(String.Format("{1}  {0} {2}", Me(iLoopIndex).Statement, Me(iLoopIndex).ClauseConjunction, vbCrLf))
                        iClauseCount = iClauseCount + 1
                    End If
                End If
            Next iLoopIndex
        End If
        Return myReturn.ToString
    End Function

End Class