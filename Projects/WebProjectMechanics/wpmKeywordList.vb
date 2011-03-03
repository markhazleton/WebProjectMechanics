Public Class wpmKeywordList
    Inherits List(Of wpmKeyword)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
        
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of wpmKeyword))
        MyBase.New(collection)
        
    End Sub
    Public Function AddToList(ByVal ValueToAdd As String) As Boolean
        If (FindKeywordValue(wpmUtil.RemoveTags(ValueToAdd)) Is Nothing) Then
            Dim myKeyword As New wpmKeyword(wpmUtil.RemoveTags(ValueToAdd))
            Me.Add(myKeyword)
        End If
        Return True
    End Function
    Private Function FindKeyword(ByVal code As String) As wpmKeyword
        For Each p As wpmKeyword In Me
            If p.Code = code Then Return p
        Next
        Return Nothing
    End Function
    Function FindKeywordValue(ByVal code As String) As wpmKeyword
        Dim pred As New KeywordPredicateClass(code)
        Return Me.Find(AddressOf pred.PredicateFunction)
    End Function

    Public Overloads Function Sort() As Boolean
        Dim myDefaultComp As KeywordDefaultOrderCompare = New KeywordDefaultOrderCompare() With {.Direction = wpmSortDirection.ASC}
        Me.Sort(myDefaultComp)
        Return True
    End Function

End Class
