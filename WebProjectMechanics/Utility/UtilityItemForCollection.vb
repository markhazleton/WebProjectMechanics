Public Class UtilityItemForCollection
    Public Function SetProperty(ByVal PropertyName As String, ByVal PropertyValue As String) As Boolean
        Dim bReturn As Boolean = False
        For Each myProperty In Me.GetType.GetProperties
            If myProperty.Name = PropertyName Then
                myProperty.SetValue(Me, PropertyValue, Nothing)
                bReturn = True
                Exit For
            ElseIf wpm_CheckForMatch(myProperty.Name, PropertyName) Then
                myProperty.SetValue(Me, PropertyValue, Nothing)
                bReturn = True
                Exit For
            End If
        Next
        Return bReturn
    End Function

    Public Function GetProperty(ByVal PropertyName As String) As Object
        Dim myValue As Object
        For Each myProperty In Me.GetType.GetProperties
            If myProperty.Name = PropertyName Then
                myValue = myProperty.GetValue(Me, Nothing)
                Exit For
            ElseIf wpm_CheckForMatch(myProperty.Name, PropertyName) Then
                myValue = myProperty.GetValue(Me, Nothing)
                Exit For
            End If
        Next
        Return True
        Return myValue
    End Function

    Public Function IsValidProperty(ByVal PropertyName As String) As Boolean
        Dim bReturn As Boolean = False
        For Each myProperty In Me.GetType.GetProperties
            If myProperty.Name = PropertyName Then
                bReturn = True
                Exit For
            End If
        Next
        Return bReturn
    End Function

End Class
