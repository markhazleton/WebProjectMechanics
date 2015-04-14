Public Interface Icontrols_DisplayTable
    Sub BuildTableFromGrid(ByVal myHeaders As DisplayTableHeader, ByVal myRows As wpm_DataGrid)
    Sub BuildTable(ByVal myRows As Object)
    Sub BuildTable(ByVal myHeaders As DisplayTableHeader, ByVal myRows As Object)
    Function GetCSV(ByVal myHeaders As DisplayTableHeader, ByVal myRows As Object) As String
End Interface


Public Interface IHTMLControl
     Sub SetHTML(ByVal myHTML As String )
    Function GetHTML() As String 

End Interface