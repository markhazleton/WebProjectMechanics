Imports System.Text

Public Class PivotParameter
    Public Property Name As String
    Public Property CSVFile As String
    Public Property Cols As New List(Of String)
    Public Property Rows As New List(Of String)
    Public Property Vals As New List(Of String)
    ' Public Property Exclusions As New Dictionary(Of String,List(Of String))
    'Public Property Inclusions As New Dictionary(Of String,List(Of String))
    Public Property AggregatorName As String
    Public Property rendererName As String
    Public Property unusedAttrsVertical As String
    Public Property autoSortUnusedAttrs As String

    Public Function GetPivotParm() As String
        Return (String.Format("{0} {1} {2} {3} {4} ",
                       GetStringList(Cols, "cols"),
                       GetStringList(Rows, "rows"),
                       GetStringList(Vals, "vals"),
                       GetStringItem(AggregatorName, "aggregatorName"),
                       GetStringItem(rendererName, "rendererName")
                       )) & "derivedAttributes: {},"
    End Function

    Private Function GetStringItem(ByVal myItem As String, ByVal myName As String) As String
        Dim mySB As New StringBuilder
        If Not String.IsNullOrEmpty(myItem) then
            mySB.Append(String.Format("{0}:", myName))
            mySB.Append(String.Format("""{0}""", myItem))
            mySB.Append(",")
        End If
        Return mySB.ToString()

    End Function
    Private Function GetStringList(ByVal myStringList As List(Of String), ByVal sName As String) As String
        Dim mySB As New StringBuilder
        If myStringList.Count > 0 Then
            mySB.Append(String.Format("{0}:[", sName))
            For Each myVal In myStringList
                If myVal = myStringList.Last Then
                    mySB.Append(String.Format("""{0}""", myVal))
                Else
                    mySB.Append(String.Format("""{0}"",", myVal))
                End If
            Next
            mySB.Append("],")
        End If
        Return mySB.ToString()
    End Function

End Class
