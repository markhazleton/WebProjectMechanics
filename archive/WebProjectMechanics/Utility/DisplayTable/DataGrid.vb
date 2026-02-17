Imports System.Linq
Imports System.Reflection
Imports System.Text

Public Class wpm_DataGrid

    Public Sub New()
        GridColumns = New ColumnColl()
        GridRows = New RowColl()
    End Sub
    Public Property Title() As String
        Get
            Return m_Title
        End Get
        Set
            m_Title = Value
        End Set
    End Property
    Private m_Title As String
    Public Property DetailPath() As String
        Get
            Return m_DetailPath
        End Get
        Set
            m_DetailPath = Value
        End Set
    End Property
    Private m_DetailPath As String
    Public Property DetailKeyName() As String
        Get
            Return m_DetailKeyName
        End Get
        Set
            m_DetailKeyName = Value
        End Set
    End Property
    Private m_DetailKeyName As String
    Public Property DetailFieldName() As String
        Get
            Return m_DetailFieldName
        End Get
        Set
            m_DetailFieldName = Value
        End Set
    End Property
    Private m_DetailFieldName As String
    Public Property DetailKeyGridIndex() As Integer
        Get
            Return m_DetailKeyGridIndex
        End Get
        Set
            m_DetailKeyGridIndex = Value
        End Set
    End Property
    Private m_DetailKeyGridIndex As Integer
    Public Property DetailFieldGridIndex() As Integer
        Get
            Return m_DetailFieldGridIndex
        End Get
        Set
            m_DetailFieldGridIndex = Value
        End Set
    End Property
    Private m_DetailFieldGridIndex As Integer
    Public Property DetailDisplayName() As String
        Get
            Return m_DetailDisplayName
        End Get
        Set
            m_DetailDisplayName = Value
        End Set
    End Property
    Private m_DetailDisplayName As String


    Public Property GridColumns() As ColumnColl
        Get
            Return m_GridColumns
        End Get
        Set
            m_GridColumns = Value
        End Set
    End Property
    Private m_GridColumns As ColumnColl
    Public Property GridRows() As RowColl
        Get
            Return m_GridRows
        End Get
        Set
            m_GridRows = Value
        End Set
    End Property
    Private m_GridRows As RowColl

    Public Class GridColumn
        Public Sub New()
            ViewOnSummary = True
            ViewOnPhone = True
            ColumnValues = New List(Of String)()
        End Sub
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set
                m_Name = Value
            End Set
        End Property
        Private m_Name As String
        Public Property DisplayName() As String
            Get
                Return m_DisplayName
            End Get
            Set
                m_DisplayName = Value
            End Set
        End Property
        Private m_DisplayName As String
        Public Property SourceName() As String
            Get
                Return m_SourceName
            End Get
            Set
                m_SourceName = Value
            End Set
        End Property
        Private m_SourceName As String
        Public Property Format() As DisplayFormat
            Get
                Return m_Format
            End Get
            Set
                m_Format = Value
            End Set
        End Property
        Private m_Format As DisplayFormat
        Public Property Index() As Integer
            Get
                Return m_Index
            End Get
            Set
                m_Index = Value
            End Set
        End Property
        Private m_Index As Integer
        ' public string Value { get; set; }
        Public Property KeyField() As Boolean
            Get
                Return m_KeyField
            End Get
            Set
                m_KeyField = Value
            End Set
        End Property
        Private m_KeyField As Boolean
        Public Property LinkPath() As String
            Get
                Return m_LinkPath
            End Get
            Set
                m_LinkPath = Value
            End Set
        End Property
        Private m_LinkPath As String
        Public Property LinkKeyName() As String
            Get
                Return m_LinkKeyName
            End Get
            Set
                m_LinkKeyName = Value
            End Set
        End Property
        Private m_LinkKeyName As String
        Public Property LinkKeyIndex() As Integer
            Get
                Return m_LinkKeyIndex
            End Get
            Set
                m_LinkKeyIndex = Value
            End Set
        End Property
        Private m_LinkKeyIndex As Integer
        Public Property LinkTextName() As String
            Get
                Return m_LinkTextName
            End Get
            Set
                m_LinkTextName = Value
            End Set
        End Property
        Private m_LinkTextName As String
        Public Property LinkTextIndex() As Integer
            Get
                Return m_LinkTextIndex
            End Get
            Set
                m_LinkTextIndex = Value
            End Set
        End Property
        Private m_LinkTextIndex As Integer
        Public Property ViewOnSummary() As Boolean
            Get
                Return m_ViewOnSummary
            End Get
            Set
                m_ViewOnSummary = Value
            End Set
        End Property
        Private m_ViewOnSummary As Boolean
        Public Property ViewOnPhone() As Boolean
            Get
                Return m_ViewOnPhone
            End Get
            Set
                m_ViewOnPhone = Value
            End Set
        End Property
        Private m_ViewOnPhone As Boolean
        Public Property ThumbnailPath() As String
            Get
                Return m_ThumbnailPath
            End Get
            Set
                m_ThumbnailPath = Value
            End Set
        End Property
        Private m_ThumbnailPath As String
        'public string DataType { get; set; }
        Public Property MinValue() As String
            Get
                Return m_MinValue
            End Get
            Set
                m_MinValue = Value
            End Set
        End Property
        Private m_MinValue As String
        Public Property MaxValue() As String
            Get
                Return m_MaxValue
            End Get
            Set
                m_MaxValue = Value
            End Set
        End Property
        Private m_MaxValue As String
        Public Property UniqueValues() As Integer
            Get
                Return m_UniqueValues
            End Get
            Set
                m_UniqueValues = Value
            End Set
        End Property
        Private m_UniqueValues As Integer
        Public Property MostCommon() As String
            Get
                Return m_MostCommon
            End Get
            Set
                m_MostCommon = Value
            End Set
        End Property
        Private m_MostCommon As String
        Public Property LeastCommon() As String
            Get
                Return m_LeastCommon
            End Get
            Set
                m_LeastCommon = Value
            End Set
        End Property
        Private m_LeastCommon As String
        Public Property ColumnValues() As List(Of String)
            Get
                Return m_ColumnValues
            End Get
            Set
                m_ColumnValues = Value
            End Set
        End Property
        Private m_ColumnValues As List(Of String)

        Private ColDictionary As New Dictionary(Of String, Integer)()
        Public Sub UpdateDictionary(ColValue As String)
            If ColDictionary.ContainsKey(GetStringValue(ColValue)) Then
                Dim value As Integer = 0
                If (ColDictionary.TryGetValue(GetStringValue(ColValue), value)) Then
                    ColDictionary(GetStringValue(ColValue)) = value + 1
                End If
            Else
                ColDictionary.Add(GetStringValue(ColValue), 1)
                UniqueValues = UniqueValues + 1
            End If
        End Sub
        Private Sub FixColumnName()
            DisplayName = DisplayName.Replace(" ", String.Empty)
            DisplayName = DisplayName.Replace("?", String.Empty)
        End Sub


        Public Sub SetCommonValues()
            If ColDictionary.Count > 0 Then
                FixColumnName()
            End If
        End Sub

        Public Function GetFormatTableCell(PropValue As String, LinkURL As String) As String
            Dim myReturn As String = String.Empty
            If IsNumeric(PropValue) Then
                Select Case Format
                    Case DisplayFormat.Currency
                        PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString("c")
                        myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case DisplayFormat.Text
                        PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString()
                        myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case DisplayFormat.Number
                        PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString()
                        myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case DisplayFormat.Percent
                        PropValue = Math.Round(Convert.ToDouble(PropValue), 4).ToString()
                        myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case DisplayFormat.Float
                        PropValue = Math.Round(Convert.ToDouble(PropValue), 4).ToString("N4")
                        myReturn = (String.Format("<td {2} style='text-align: right; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case DisplayFormat.Thumbnail
                        myReturn = (String.Format("<td {2} style='text-align: right; ' ><img width='50px' src='{1}' alt='{0}' />{0}</td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                    Case Else
                        PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString()
                        myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
                        Exit Select
                End Select
            Else
                myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{1}' >{0}</a></td>", PropValue, LinkURL, GetTDCssClass()))
            End If
            Return myReturn
        End Function

        Private Function IsNumeric(propValue As String) As Boolean
            Dim retNum As Double
            Dim isNum As Boolean = [Double].TryParse(Convert.ToString(propValue), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, retNum)
            Return isNum
        End Function

        Public Function GetFormatTableCell(gRow As GridRow) As String
            Dim myReturn As String = String.Empty
            Dim PropValue As String = gRow.Value(Index)

            If String.IsNullOrEmpty(LinkPath) Then
                If IsNumeric(PropValue) Then
                    Select Case Format
                        Case DisplayFormat.ShortDate
                            PropValue = GetShortDate(PropValue)
                            myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case DisplayFormat.Currency
                            PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString("c")
                            myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case DisplayFormat.Text
                            PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString()
                            myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case DisplayFormat.Number
                            PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString("N0")
                            myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case DisplayFormat.Percent
                            PropValue = Math.Round(Convert.ToDouble(PropValue), 4).ToString("P")
                            myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case DisplayFormat.Float
                            PropValue = Math.Round(Convert.ToDouble(PropValue), 4).ToString("N4")
                            myReturn = (String.Format("<td {1} style='text-align: right; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                        Case Else
                            PropValue = Math.Round(Convert.ToDecimal(PropValue), 2).ToString()
                            myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                            Exit Select
                    End Select
                Else
                    myReturn = (String.Format("<td {1} style='text-align: left; ' >{0}</td>", PropValue, GetTDCssClass()))
                End If
            Else
                Dim linkURL As String = String.Format(LinkPath, PropValue)
                Dim linkText As String = PropValue

                If LinkTextIndex > 0 Then
                    linkText = gRow.Value(LinkTextIndex - 1)
                End If
                If LinkKeyIndex > 0 Then
                    linkURL = String.Format(LinkPath, gRow.Value(LinkKeyIndex - 1))
                End If

                myReturn = (String.Format("<td {2} style='text-align: left; ' ><a href='{0}'>{1}</a></td>", linkURL, linkText, GetTDCssClass()))
            End If


            Return myReturn
        End Function

        Private Function GetShortDate(propValue As String) As String
            Return DateTime.Parse(propValue).ToShortDateString()
        End Function

        Public Function GetTDCssClass() As String
            If ViewOnPhone Then
                Return String.Empty
            Else
                Return " class='hidden-xs' "
            End If
        End Function
    End Class
    Public Class GridRow
        Public Sub New()
            Value = New List(Of String)()
        End Sub
        Public Property name() As String
            Get
                Return m_name
            End Get
            Set
                m_name = Value
            End Set
        End Property
        Private m_name As String
        Public Property Value() As List(Of String)
            Get
                Return m_Value
            End Get
            Set
                m_Value = Value
            End Set
        End Property
        Private m_Value As List(Of String)
    End Class

    Public Class RowColl
        Inherits List(Of GridRow)
    End Class
    Public Class ColumnColl
        Inherits List(Of GridColumn)
    End Class




    Public Function GetDisplayTableHeader() As String
        Dim myrow As New StringBuilder()
        If DetailFieldName <> String.Empty Then
            If DetailDisplayName <> String.Empty Then
                myrow.AppendLine(String.Format("<th>{0}</th>", FormatHeaderColumn(DetailDisplayName)))
            Else
                myrow.AppendLine(String.Format("<th>{0}</th>", FormatHeaderColumn(DetailFieldName)))
            End If
        End If
        For Each gCol As GridColumn In GridColumns
            myrow.AppendLine(String.Format("<th {1}  >{0}</th>", FormatHeaderColumn(gCol.DisplayName), gCol.GetTDCssClass()))
        Next
        Return myrow.ToString()
    End Function
    Private Function FormatHeaderColumn(myColName As String) As String
        Dim newColumnName As String = Nothing
        Select Case StrFun.Right(myColName, 2)
            Case "NM"
                newColumnName = StrFun.Left(myColName, myColName.Length - 2) & Convert.ToString(" Name")
                Exit Select
            Case "CD"
                newColumnName = StrFun.Left(myColName, myColName.Length - 2) & Convert.ToString(" Code")
                Exit Select
            Case "DS"
                newColumnName = StrFun.Left(myColName, myColName.Length - 2) & Convert.ToString(" Description")
                Exit Select
            Case "ID"
                newColumnName = StrFun.Left(myColName, myColName.Length - 2) & Convert.ToString(" Id")
                Exit Select
            Case Else
                newColumnName = myColName
                Exit Select
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

    Public Function GetPropertyValue(obj As Object, myCol As GridColumn) As String
        Dim objType As Type = Nothing
        Dim pInfo As PropertyInfo = Nothing
        Dim PropValue As New Object()
        If myCol.Name.Contains(".") Then
            Dim PropertyNameArray As String() = myCol.Name.Split("."c)
            If PropertyNameArray.Count = 2 Then
                Try
                    objType = obj.[GetType]()
                    pInfo = objType.GetProperty(PropertyNameArray(0))
                    PropValue = pInfo.GetValue(obj, System.Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)

                    objType = PropValue.[GetType]()
                    pInfo = objType.GetProperty(PropertyNameArray(1))
                    PropValue = pInfo.GetValue(PropValue, System.Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                Catch
                    PropValue = String.Empty
                End Try
            End If
        Else
            Try
                objType = obj.[GetType]()
                pInfo = objType.GetProperty(myCol.Name)
                Dim myDateValue As DateTime
                Select Case myCol.Format
                    Case DisplayFormat.ShortDate
                        If DateTime.TryParse(pInfo.GetValue(obj, BindingFlags.GetProperty, Nothing, Nothing, Nothing).ToString(), myDateValue) Then
                            PropValue = myDateValue.[Date].ToString("yyyy-MM-dd")
                        Else
                            PropValue = pInfo.GetValue(obj, System.Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                        End If
                        Exit Select
                    Case DisplayFormat.LongDate
                        If DateTime.TryParse(pInfo.GetValue(obj, BindingFlags.GetProperty, Nothing, Nothing, Nothing).ToString(), myDateValue) Then
                            PropValue = myDateValue.ToString("yyyy-MM-dd:HH.mm")
                        Else
                            PropValue = pInfo.GetValue(obj, System.Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                        End If
                        Exit Select
                    Case Else
                        PropValue = pInfo.GetValue(obj, System.Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
                        Exit Select

                End Select
            Catch
                PropValue = String.Empty
            End Try
            If PropValue Is Nothing Then
                PropValue = String.Empty
            End If
        End If

        PropValue = ClearLineFeeds(PropValue.ToString())

        Return PropValue.ToString()
    End Function
    Public Shared Function GetStringValue(myString As String) As String
        If String.IsNullOrEmpty(myString) Then
            myString = String.Empty
        End If
        Return myString
    End Function
    Public Shared Function ClearLineFeeds(sTextToCovert As String) As String
        sTextToCovert = sTextToCovert.Replace(vbLf, String.Empty)
        Return sTextToCovert
    End Function
    Public Enum DisplayFormat
        Number
        Currency
        Text
        Percent
        Float
        Thumbnail
        Hidden
        ShortDate
        LongDate
        Link
    End Enum
    Public Enum ColumnFunction
        Value
        Sum
        Count
        Average
    End Enum

    Public HeaderItems As New List(Of GridColumn)()
    Public Sub AddHeaderItem(DisplayName As String, Name As String)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.DisplayName = DisplayName
                Exit For
            End If
        Next
    End Sub
    Public Sub AddHeaderItem(DisplayName As String, Name As String, ShowOnPhone As Boolean)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.ViewOnPhone = ShowOnPhone
                x.DisplayName = DisplayName
                Exit For
            End If
        Next
    End Sub
    Public Sub AddHeaderItem(DisplayName As String, Name As String, ShowOnPhone As Boolean, Display As DisplayFormat)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.ViewOnPhone = ShowOnPhone
                x.DisplayName = DisplayName
                x.Format = Display
                Exit For
            End If
        Next
    End Sub


    Public Sub AddLinkHeaderItem(DisplayName As String, Name As String, LinkPath As String, LinkKeyName As String)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.ViewOnPhone = True
                x.DisplayName = DisplayName
                x.LinkKeyName = LinkKeyName
                x.LinkPath = LinkPath
                Exit For
            End If
        Next
    End Sub
    Public Sub AddLinkHeaderItem(DisplayName As String, Name As String, LinkPath As String, LinkKeyName As String, LinkTextName As String)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.ViewOnPhone = True
                x.DisplayName = DisplayName
                x.LinkKeyName = LinkKeyName
                x.LinkPath = LinkPath
                x.LinkTextName = LinkTextName
                Exit For
            End If
        Next
    End Sub
    Public Sub AddLinkHeaderItem(DisplayName As String, Name As String, LinkPath As String, LinkKeyName As String, LinkTextName As String, ThumbnailPath As String)
        For Each x As GridColumn In GridColumns
            If x.Name = Name Then
                x.ViewOnSummary = True
                x.ViewOnPhone = True
                x.DisplayName = DisplayName
                x.LinkKeyName = LinkKeyName
                x.LinkPath = LinkPath
                x.LinkTextName = LinkTextName
                x.ThumbnailPath = ThumbnailPath
                Exit For
            End If
        Next
    End Sub

    Public Function GetCSV() As String
        Dim csv As New StringBuilder()
        For Each column As GridColumn In GridColumns
            'Add the Header row for CSV file.
            csv.Append([String].Concat("""", column.DisplayName.ToString().Replace("""", """"""), """") + ","c)
        Next
        'Add new line.
        csv.Append(Environment.NewLine)
        For Each row As GridRow In GridRows
            For Each column As GridColumn In GridColumns
                'Add the Data rows.
                csv.Append(QuoteCSVValue(column, row.Value(column.Index)) & Convert.ToString(","c))
            Next
            'Add new line.
            csv.Append(Environment.NewLine)
        Next
        Return csv.ToString()
    End Function

    Private Shared Function QuoteCSVValue(col As GridColumn, value As Object) As String
        Dim valType = value.[GetType]()
        Select Case col.Format
            Case DisplayFormat.Currency
                Return value.ToString()
            Case DisplayFormat.Float
                Return value.ToString()
            Case DisplayFormat.Percent
                Return value.ToString()
            Case DisplayFormat.Number
                Return value.ToString()
            Case DisplayFormat.ShortDate
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
                'if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                '{
                '    return String.Concat("\"", ((DateTime)value).ToShortDateString().Replace("\"", "\"\""), "\"");
                '}
                'else
                '{
                '    return String.Concat("\"", ((DateTime)value).ToString().Replace("\"", "\"\""), "\"");
                '}
            Case DisplayFormat.LongDate
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
                'if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                '{
                '    return String.Concat("\"", ((DateTime)value).ToShortDateString().Replace("\"", "\"\""), "\"");
                '}
                'else
                '{
                '    return String.Concat("\"", ((DateTime)value).ToString().Replace("\"", "\"\""), "\"");
                '}
            Case DisplayFormat.Thumbnail
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
            Case DisplayFormat.Link
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
            Case DisplayFormat.Hidden
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
            Case DisplayFormat.Text
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
            Case Else
                Return [String].Concat("""", value.ToString().Replace("""", """"""), """")
        End Select
    End Function



    Public Shared Function IsNumeric(Expression As Object) As Boolean
        Dim retNum As Double

        Dim isNum As Boolean = [Double].TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, retNum)
        Return isNum
    End Function
    Private Class StrFun
        Public Shared Function Left(param As String, length As Integer) As String
            'we start at 0 since we want to get the characters starting from the
            'left and with the specified lenght and assign it to a variable
            Dim result As String = param.Substring(0, length)
            'return the result of the operation
            Return result
        End Function
        Public Shared Function Right(param As String, length As Integer) As String
            'start at the index based on the lenght of the sting minus
            'the specified lenght and assign it a variable
            Dim result As String = param.Substring(param.Length - length, length)
            'return the result of the operation
            Return result
        End Function

        Public Shared Function Mid(param As String, startIndex As Integer, length As Integer) As String
            'start at the specified index in the string ang get N number of
            'characters depending on the lenght and assign it to a variable
            Dim result As String = param.Substring(startIndex, length)
            'return the result of the operation
            Return result
        End Function

        Public Shared Function Mid(param As String, startIndex As Integer) As String
            'start at the specified index and return all characters after it
            'and assign it to a variable
            Dim result As String = param.Substring(startIndex)
            'return the result of the operation
            Return result
        End Function

    End Class


End Class

Public Enum GridAggregateType
    Sum
    Avg
    Count
End Enum
