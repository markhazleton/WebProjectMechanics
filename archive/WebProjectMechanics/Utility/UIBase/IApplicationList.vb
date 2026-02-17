
Public Interface IApplicationList
    Sub GetData(ByVal sWhere As String, ByVal OutputCSV As Boolean)
    Sub GetData(ByVal sFilterList As SQLFilterList, ByVal OutputCSV As Boolean)

End Interface
