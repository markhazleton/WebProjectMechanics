Imports System.Reflection
Imports System.Text

Public Class DisplayTableHeader
    Public HeaderItems As New List(Of DisplayTableHeaderItem)
    Public TableTitle As String
    Public DetailPath As String
    Public DetailKeyName As String
    Public DetailFieldName As String
    Public DetailKeyGridIndex As Integer
    Public DetailFieldGridIndex As Integer
    Public DetailDisplayName As String

    Public Sub AddHeaderItem(ByVal Name As String, ByVal Value As String)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = Name, .Value = Value})
    End Sub
    Public Sub AddHeaderItem(ByVal Name As String, ByVal Value As String, ByVal ShowOnPhone As Boolean)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = Name, .Value = Value, .ViewOnPhone = ShowOnPhone})
    End Sub
    Public Sub AddHeaderItem(ByVal Name As String, ByVal Value As String, ByVal ShowOnPhone As Boolean, ByVal Display As DisplayFormat)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = Name,
                                                         .Value = Value,
                                                         .ViewOnPhone = ShowOnPhone,
                                                         .DisplayFormat = Display})
    End Sub


    Public Sub AddLinkHeaderItem(ByVal DisplayName As String, ByVal Value As String, ByVal LinkPath As String, ByVal LinkKeyName As String)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = DisplayName,
                                                         .Value = Value,
                                                         .KeyField = True,
                                                         .LinkKeyName = LinkKeyName,
                                                         .LinkPath = LinkPath,
                                                         .LinkTextName = Value})
    End Sub
    Sub AddLinkHeaderItem(DisplayName As String, Value As String, LinkPath As String, LinkKeyName As String, LinkTextName As String)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = DisplayName,
                                                         .Value = Value,
                                                         .KeyField = True,
                                                         .LinkKeyName = LinkKeyName,
                                                         .LinkPath = LinkPath,
                                                         .LinkTextName = LinkTextName})
    End Sub
    Sub AddLinkHeaderItem(DisplayName As String, Value As String, LinkPath As String, LinkKeyName As String, LinkTextName As String, ThumbnailPath As String)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = DisplayName,
                                                         .Value = Value,
                                                         .KeyField = True,
                                                         .LinkKeyName = LinkKeyName,
                                                         .LinkPath = LinkPath,
                                                         .LinkTextName = LinkTextName,
                                                         .ThumbnailPath = ThumbnailPath})
    End Sub
    Sub AddHeaderThumbnailItem(DisplayName As String, Value As String, LinkPath As String, LinkKeyName As String, LinkTextName As String, ThumbPath As String)
        HeaderItems.Add(New DisplayTableHeaderItem With {.Name = DisplayName,
                                                         .Value = Value,
                                                         .KeyField = True,
                                                         .LinkKeyName = LinkKeyName,
                                                         .LinkPath = LinkPath,
                                                         .ThumbnailPath = ThumbPath,
                                                         .LinkTextName = LinkTextName})
    End Sub

    Private Sub SetHeadersFromDataTable(ByRef myDT As DataTable)
        Dim objType As Type
        Dim pInfo As PropertyInfo
        Dim PropValue As New Object
        HeaderItems.Clear()
        For Each myItem In myDT.Columns
            Try
                objType = myItem.GetType()
                For Each pInfo In objType.GetProperties()
                    AddHeaderItem(pInfo.Name, pInfo.Name)
                Next
            Catch ex As Exception
                PropValue = String.Empty
            End Try
            Exit For
        Next
    End Sub

    Public Function GetDisplayTableHeader() As String
        Dim myrow As New StringBuilder
        Dim myTableHeaders As New List(Of String)

        If DetailFieldName <> String.Empty Then
            myrow.AppendLine(String.Format("<th>{0}</th>", FormatHeaderColumn(DetailFieldName)))
        End If
        For Each myHeaderItem In HeaderItems
            myrow.AppendLine(String.Format("<th {1}  >{0}</th>", FormatHeaderColumn(myHeaderItem.Name), GetTDCssClass(myHeaderItem)))
        Next
        Return myrow.ToString()
    End Function
    Private Function FormatHeaderColumn(ByVal myColName As String) As String
        Dim newColumnName As String
        Select Case Right(myColName, 2)
            Case "NM"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Name"
            Case "CD"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Code"
            Case "DS"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Description"
            Case "ID"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Id"
            Case Else
                newColumnName = myColName
        End Select

        Dim newstring As String = String.Empty
        For i As Integer = 0 To newColumnName.Length - 1
            If Char.IsUpper(newColumnName(i)) AndAlso i > 0 Then
                newstring += " "
            End If
            newstring += newColumnName(i).ToString()
        Next
        Return newstring
    End Function
    Private Function GetTDCssClass(ByRef p1 As DisplayTableHeaderItem) As String
        If p1.ViewOnPhone Then
            Return String.Empty
        Else
            Return " class='hidden-xs' "
        End If
    End Function



    'Public HeaderItems As New List(Of DisplayTableHeaderItem)
    'Public TableTitle As String
    'Public DetailPath As String
    'Public DetailKeyName As String 
    'Public DetailFieldName As String 

    'Public Sub AddHeaderItem(ByVal Name As String, ByVal Value As String)
    '    HeaderItems.Add(New DisplayTableHeaderItem With {.Name = Name, 
    '                                                     .Value = Value})
    'End Sub
    'Public Sub AddHeaderItem(ByVal Name As String, ByVal Value As String, ByVal LinkPath As String, ByVal LinkKeyName As String, ByVal LinkTextName As String )
    '    HeaderItems.Add(New DisplayTableHeaderItem With {.Name = Name, 
    '                                                     .Value = Value,
    '                                                     .KeyField = True,
    '                                                     .LinkKeyName = LinkKeyName,
    '                                                     .LinkPath = LinkPath,
    '                                                     .LinkTextName= LinkTextName})
    'End Sub
    'Public Function GetHeaders(ByRef myrows As List(Of Object)) As DisplayTableHeader
    '    Dim ReturnHeader As New DisplayTableHeader
    '    Dim objType As Type
    '    Dim pInfo As PropertyInfo
    '    Dim PropValue As New Object
    '    For Each myItem In myrows
    '        Try
    '            objType = myItem.GetType()
    '            For Each pInfo In objType.GetProperties()
    '                ReturnHeader.AddHeaderItem(pInfo.Name, pInfo.Name)
    '            Next
    '        Catch ex As Exception
    '            PropValue = String.Empty
    '        End Try
    '        Exit For
    '    Next
    '    Return ReturnHeader
    'End Function
    'Public Function GetPropertyValue(ByVal obj As Object, ByVal PropName As String) As Object
    '    Dim objType As Type
    '    Dim pInfo As PropertyInfo
    '    Dim PropValue As New Object
    '    If PropName.Contains(".") Then
    '        Dim PropertyNameArray = Split(PropName, ".")
    '        If PropertyNameArray.Length = 2 Then
    '            Try
    '                objType = obj.GetType()
    '                pInfo = objType.GetProperty(PropertyNameArray(0))
    '                PropValue = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
    '                objType = PropValue.GetType()
    '                pInfo = objType.GetProperty(PropertyNameArray(1))
    '                PropValue = pInfo.GetValue(PropValue, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
    '            Catch ex As Exception
    '                PropValue = String.Empty
    '            End Try
    '        End If
    '    Else
    '        Try
    '            objType = obj.GetType()
    '            pInfo = objType.GetProperty(PropName)
    '            PropValue = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
    '        Catch ex As Exception
    '            PropValue = String.Empty
    '        End Try
    '        If PropValue Is Nothing Then
    '            PropValue = String.Empty
    '        End If
    '    End If
    '    Return PropValue
    'End Function
    'Public Function AddComma(value As String) As String
    '    Dim mySB As New StringBuilder
    '    If value Is Nothing Then
    '        mySB.Append(String.Format("""{0}""", String.Empty))
    '    ElseIf IsNumeric(value) Then
    '        mySB.Append(String.Format("{0}", value))
    '    Else
    '        mySB.Append(String.Format("""{0}""", value.Replace(","c, " "c).Replace(""""c, "'"c)))
    '    End If
    '    Return mySB.ToString
    'End Function

    'Public Function GetCSV(ByVal myheader As DisplayTableHeader, ByVal myRows As List(Of Object)) As String
    '    Dim myReturn As New StringBuilder
    '    Dim myrow As New StringBuilder
    '    Dim iRow As Integer = 1
    '    Try
    '        myrow = New StringBuilder
    '        For Each p In myheader.HeaderItems
    '            If iRow < myheader.HeaderItems.Count Then
    '                myrow.Append(AddComma(p.Name) & ",")
    '                iRow += 1
    '            Else
    '                myrow.Append(AddComma(p.Name))
    '                myrow.Append(Environment.NewLine)
    '            End If
    '        Next
    '        myReturn.Append(myrow.ToString)

    '        For Each i In myRows
    '            myrow = New StringBuilder
    '            iRow = 1
    '            For Each p In myheader.HeaderItems
    '                If iRow < myheader.HeaderItems.Count Then
    '                    myrow.Append(AddComma(CStr(GetPropertyValue(i, p.Value))) & ",")
    '                    iRow += 1
    '                Else
    '                    myrow.Append(AddComma(CStr(GetPropertyValue(i, p.Value))))
    '                    myrow.Append(Environment.NewLine)
    '                End If
    '            Next
    '            myReturn.Append(myrow.ToString)
    '        Next
    '    Catch ex As Exception
    '        ApplicationLogging.ErrorLog(ex.ToString, "DisplayTable.ascx.GetSCV")
    '    End Try
    '    Return myReturn.ToString
    'End Function


End Class

