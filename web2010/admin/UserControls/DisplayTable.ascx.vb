Imports System.Reflection
Imports WebProjectMechanics

Public Class controls_DisplayTable
    Inherits System.Web.UI.UserControl
    Implements Icontrols_DisplayTable

    Public Property myCSV As String
        Get
            Return HttpUtility.HtmlDecode(hfCSV.Value)
        End Get
        Set(value As String)
            hfCSV.Value = HttpUtility.HtmlEncode(value)
        End Set
    End Property
    Public Sub BuildTable(myRows As Object) Implements Icontrols_DisplayTable.BuildTable
        BuildTable(GetHeaders(myRows), myRows)
    End Sub
    Private Function FormatHeaderColumn(ByVal myColName As String) As String
        Dim newColumnName As String
        Select Case Right(myColName, 2)
            Case "NM"
                newColumnName = Left(myColName, Len(myColName) - 2)
            Case "CD"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Code"
            Case "DS"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Description"
            Case "ID"
                newColumnName = Left(myColName, Len(myColName) - 2) & " Id"
            Case Else
                newColumnName = myColName
        End Select

        Dim newstring As String = ""
        For i As Integer = 0 To newColumnName.Length - 1
            If Char.IsUpper(newColumnName(i)) AndAlso i > 0 Then
                newstring += " "
            End If
            newstring += newColumnName(i).ToString()
        Next
        Return newstring
    End Function

    Public Sub BuildTableFromGrid(myHeader As DisplayTableHeader, myDataGrid As wpm_DataGrid) Implements Icontrols_DisplayTable.BuildTableFromGrid
        Dim myTableHeaders As New List(Of String)
        Dim myrow As New StringBuilder
        Dim myTableRows As New List(Of String)
        tblTitle.Text = myHeader.TableTitle
        myrow = New StringBuilder
        For gridindex = 0 To myDataGrid.GridColumns.Count - 1
            Dim x = gridindex
            For Each myHeaderItem In (From i In myHeader.HeaderItems Where i.Value = myDataGrid.GridColumns(x).Name)
                myHeaderItem.GridIndex = x
            Next
            If myHeader.DetailKeyName = myDataGrid.GridColumns(x).Name Then
                myHeader.DetailKeyGridIndex = x
            End If
            If myHeader.DetailFieldName = myDataGrid.GridColumns(x).Name Then
                myHeader.DetailFieldGridIndex = x
            End If
        Next
        If myHeader.DetailFieldName <> String.Empty Then
            myrow.AppendLine(String.Format("<th>{0}</th>", FormatHeaderColumn(myHeader.DetailFieldName)))
        End If
        For Each myHeaderItem In myHeader.HeaderItems
            myrow.AppendLine(String.Format("<th {1}  >{0}</th>", FormatHeaderColumn(myHeaderItem.Name), GetTDCssClass(myHeaderItem)))
        Next
        Dim GridColumnName As New String(String.Empty)
        myTableHeaders.Add(myrow.ToString)
        For Each i In myDataGrid.GridRows
            myrow = New StringBuilder
            If myHeader.DetailFieldName <> String.Empty Then
                Try
                    If myHeader.DetailKeyGridIndex <= i.Value.Count AndAlso myHeader.DetailFieldGridIndex <= i.Value.Count Then
                        Dim myTest1 = i.Value(myHeader.DetailKeyGridIndex)
                        Dim MyTest2 = i.Value(myHeader.DetailFieldGridIndex)
                        myrow.AppendLine(String.Format("<td><a href='{0}' >{1}</a></td>", String.Format(myHeader.DetailPath, myTest1), MyTest2))
                    End If
                Catch ex As Exception
                    WebProjectMechanics.ApplicationLogging.ErrorLog("DisplayTable-BuildTable with Grid", ex.ToString)
                End Try
            End If
            For Each myHeaderItem In myHeader.HeaderItems
                If IsNumeric(i.Value(myHeaderItem.GridIndex)) Then
                    myrow.AppendLine(String.Format("<td {1}   style='text-align: right; ' >{0}</td>", i.Value(myHeaderItem.GridIndex), GetTDCssClass(myHeaderItem)))
                Else
                    myrow.AppendLine(String.Format("<td {1} >{0}</td>", i.Value(myHeaderItem.GridIndex), GetTDCssClass(myHeaderItem)))
                End If
            Next
            myTableRows.Add(myrow.ToString())
        Next
        rptHeaderRow.DataSource = myTableHeaders
        rptHeaderRow.DataBind()
        rptDataRows.DataSource = myTableRows
        rptDataRows.DataBind()
    End Sub

    Public Sub BuildTable(ByVal myHeader As DisplayTableHeader, ByVal myRows As Object) Implements Icontrols_DisplayTable.BuildTable
        Dim myTableHeaders As New List(Of String)
        Dim myrow As New StringBuilder
        Dim myTableRows As New List(Of String)
        Dim PropValue As String
        tblTitle.Text = myHeader.TableTitle

        myrow = New StringBuilder
        If myHeader.DetailFieldName <> String.Empty Then
            myrow.AppendLine(String.Format("<th {1}>{0}</th>", myHeader.DetailFieldName, String.Empty))
        End If
        For Each p In myHeader.HeaderItems
            myrow.AppendLine(String.Format("<th {1}>{0}</th>", FormatHeaderColumn(p.Name), GetTDCssClass(p)))
        Next
        myTableHeaders.Add(myrow.ToString)

        For Each i In myRows
            myrow = New StringBuilder

            If myHeader.DetailFieldName <> String.Empty Then
                myrow.AppendLine(String.Format("<td ><a href='{0}' >{1}</a></td>", String.Format(myHeader.DetailPath, GetPropertyValue(i, myHeader.DetailKeyName)), GetPropertyValue(i, myHeader.DetailFieldName)))
            End If
            For Each p In myHeader.HeaderItems
                If p.Name = "Thumbnail" Then
                    myrow.AppendLine(String.Format("<td><a href='{0}' ><img width='50px' src='/sites/nrc/thumbnails/{1}' alt='{1}' /></a></td>", String.Format(p.LinkPath, GetPropertyValue(i, p.LinkKeyName)), GetPropertyValue(i, p.LinkTextName).Replace(".jpg", ".png")))
                Else
                    If p.KeyField Then
                        myrow.AppendLine(String.Format("<td><a href='{0}' >{1}</a></td>", String.Format(p.LinkPath, GetPropertyValue(i, p.LinkKeyName)), GetPropertyValue(i, p.LinkTextName)))
                    Else
                        PropValue = GetPropertyValue(i, p.Value).ToString
                        If IsNumeric(PropValue) Then
                            PropValue = Math.Round(CDec(PropValue), 2)
                            myrow.AppendLine(String.Format("<td style='text-align: right; ' >{0}</td>", PropValue))
                        Else
                            myrow.AppendLine(String.Format("<td>{0}</td>", PropValue))
                        End If
                    End If
                End If
            Next
            myTableRows.Add(myrow.ToString())
        Next

        rptHeaderRow.DataSource = myTableHeaders
        rptHeaderRow.DataBind()

        rptDataRows.DataSource = myTableRows
        rptDataRows.DataBind()
    End Sub
    Public Function GetCSV(ByVal myheader As DisplayTableHeader, ByVal myRows As Object) As String Implements Icontrols_DisplayTable.GetCSV
        Dim myReturn As New StringBuilder
        Dim myrow As New StringBuilder
        Dim iRow As Integer = 1
        Try
            myrow = New StringBuilder
            For Each p In myheader.HeaderItems
                If iRow < myheader.HeaderItems.Count Then
                    myrow.Append(AddComma(p.Name) & ",")
                    iRow += 1
                Else
                    myrow.Append(AddComma(p.Name))
                    myrow.Append(Environment.NewLine)
                End If
            Next
            myReturn.Append(myrow.ToString)

            For Each i In myRows
                myrow = New StringBuilder
                iRow = 1
                For Each p In myheader.HeaderItems
                    If iRow < myheader.HeaderItems.Count Then
                        myrow.Append(AddComma(GetPropertyValue(i, p.Value)) & ",")
                        iRow += 1
                    Else
                        myrow.Append(AddComma(GetPropertyValue(i, p.Value)))
                        myrow.Append(Environment.NewLine)
                    End If
                Next
                myReturn.Append(myrow.ToString)
            Next
        Catch ex As Exception
            WebProjectMechanics.ApplicationLogging.ErrorLog(ex.ToString, "DisplayTable.ascx.GetSCV")
        End Try
        Return myReturn.ToString
    End Function
    Private Function GetHeaders(ByRef myrows As Object) As DisplayTableHeader
        Dim ReturnHeader As New DisplayTableHeader
        Dim objType As Type
        Dim pInfo As PropertyInfo
        Dim PropValue As New Object
        For Each myItem In myrows
            Try
                objType = myItem.GetType()
                For Each pInfo In objType.GetProperties()
                    ReturnHeader.AddHeaderItem(pInfo.Name, pInfo.Name)
                Next
            Catch ex As Exception
                PropValue = String.Empty
            End Try
            Exit For
        Next
        Return ReturnHeader
    End Function
    Private Function GetPropertyValue(ByVal obj As Object, ByVal PropName As String) As Object
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
    Private Function AddComma(value As String) As String
        Dim mySB As New StringBuilder
        If value Is Nothing Then
            mySB.Append(String.Format("""{0}""", String.Empty))
        ElseIf IsNumeric(value) Then
            mySB.Append(String.Format("{0}", value))
        Else
            mySB.Append(String.Format("""{0}""", value.Replace(","c, " "c).Replace(""""c, "'"c)))
        End If
        Return mySB.ToString
    End Function
    Protected Sub lbGetCSV_Click(sender As Object, e As EventArgs)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ClearHeaders()
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=datasets.csv")
        HttpContext.Current.Response.ContentType = "text/csv"
        HttpContext.Current.Response.AddHeader("Pragma", "public")
        HttpContext.Current.Response.Write(myCSV)
        Response.Flush()
        Response.End()
    End Sub
    Private Function GetTDCssClass(ByRef p1 As DisplayTableHeaderItem) As String
        If p1.ViewOnPhone Then
            Return String.Empty
        Else
            Return " class='hidden-xs' "
        End If
    End Function






End Class
