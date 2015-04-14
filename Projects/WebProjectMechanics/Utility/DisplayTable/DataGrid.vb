Imports System.Reflection
Imports System.Text

Public Class wpm_DataGrid
    Property Title As String
    Property GridColumns As New ColumnColl
    Property GridRows As New RowColl
    Public Class GridColumn
        Property DataType As String
        Property Name As String
    End Class
    Public Class ColumnColl
        Inherits List(Of GridColumn)
    End Class
    Public Class GridRow
        Property name As String
        Property Value As New List(Of String)
    End Class
    Public Class RowColl
        Inherits List(Of GridRow)
    End Class

    Public Sub SetHeaders(ByRef myrows As List(Of Object))
        Dim objType As Type
        Dim pInfo As PropertyInfo
        Dim PropValue As New Object
        For Each myItem In myrows
            Try
                objType = myItem.GetType()
                For Each pInfo In objType.GetProperties()
                    GridColumns.Add(New GridColumn With {.DataType = "string", .Name = pInfo.Name})
                Next
            Catch ex As Exception
                PropValue = String.Empty
            End Try
            Exit For
        Next
    End Sub

    Public Function GetPropertyValue(ByVal obj As Object, ByVal PropName As String) As Object
        Dim objType As Type
        Dim pInfo As PropertyInfo
        Dim PropValue As New Object
        If PropName.Contains(".") Then
            Dim PropertyNameArray = Split(PropName, ".")

            If PropertyNameArray.Count = 2 Then
                Try
                    objType = obj.GetType()
                    pInfo = objType.GetProperty(PropertyNameArray(0))
                    PropValue = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)

                    objType = PropValue.GetType()
                    pInfo = objType.GetProperty(PropertyNameArray(1))
                    PropValue = pInfo.GetValue(PropValue, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                Catch ex As Exception
                    PropValue = String.Empty
                End Try
            End If
        Else
            Try
                objType = obj.GetType()
                pInfo = objType.GetProperty(PropName)
                PropValue = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
            Catch ex As Exception
                PropValue = String.Empty
            End Try
            If PropValue Is Nothing Then
                PropValue = String.Empty
            End If
        End If
        Return PropValue
    End Function

    Public Function GetCSV() As String
        Dim csv As New StringBuilder
        For Each column In GridColumns
            'Add the Header row for CSV file.
            csv.Append(column.Name + ","c)
        Next
        'Add new line.
        csv.Append(vbCr & vbLf)
        For Each row In GridRows
            For Each myValue In row.Value
                'Add the Data rows.
                csv.Append((myValue).ToString().Replace(",", ";") + ","c)
            Next
            'Add new line.
            csv.Append(vbCr & vbLf)
        Next
        Return csv.ToString()
    End Function

End Class
