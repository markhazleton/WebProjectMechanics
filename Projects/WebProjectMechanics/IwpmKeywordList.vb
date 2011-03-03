Public Interface IwpmKeywordList
    Function AddToList(ByVal ValueToAdd As String) As Boolean
    Function FindKeywordValue(ByVal code As String) As wpmKeyword
    Overloads Function Sort() As Boolean
End Interface
