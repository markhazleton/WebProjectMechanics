Public Interface IApplicationFilter
    Function GetFilterClause(ByVal FieldType As String) As String
    Function GetFilterList() As SQLFilterList
End Interface
