Public Class LocationKeywordList
    Inherits List(Of LocationKeyword)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of LocationKeyword))
        MyBase.New(collection)

    End Sub
    Public Function AddToList(ByVal ValueToAdd As String) As Boolean
        If (FindKeywordValue(wpm_RemoveTags(ValueToAdd)) Is Nothing) Then
            Dim myKeyword As New LocationKeyword(wpm_RemoveTags(ValueToAdd))
            Me.Add(myKeyword)
        End If
        Return True
    End Function
    Private Function FindKeyword(ByVal code As String) As LocationKeyword
        For Each p As LocationKeyword In Me
            If p.Code = code Then Return p
        Next
        Return Nothing
    End Function
    Function FindKeywordValue(ByVal code As String) As LocationKeyword
        Dim pred As New LocationKeywordPredicateClass(code)
        Return Me.Find(AddressOf pred.PredicateFunction)
    End Function

    Public Overloads Function Sort() As Boolean
        Dim myDefaultComp As LocationKeywordDefaultOrderCompare = New LocationKeywordDefaultOrderCompare() With {.Direction = UtilitySortDirection.ASC}
        Me.Sort(myDefaultComp)
        Return True
    End Function

End Class
