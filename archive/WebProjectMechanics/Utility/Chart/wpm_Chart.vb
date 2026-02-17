Imports System.IO
Imports System.Web.UI.DataVisualization.Charting
Imports System.Xml.Serialization
Public Class wpm_Chart
    Property ChartName As String
    Property ChartTitles As New List(Of String)
    Property SeriesDataList As New List(Of SeriesData)
    Property ChartType As SeriesChartType
    Property ChartStyle As String
    '2D vs 3D
    Property ChartPalette As String
    Property ChartWidth As Int16
    Property ChartHeight As Int16

    Property DataSetName As String
    Property SourceData As New wpm_DataGrid
    Private ChartData As New wpm_DataGrid
    Public ReadOnly Property ChartDataGrid As wpm_DataGrid
        Get
            Return ChartData
        End Get
    End Property

    Property xColumn As New DataGridColumn
    Property xColumnSelected As New List(Of String)
    Property yColumn As New DataGridColumn
    Property yColumnSelected As New List(Of String)
    Property FilterColumn As New DataGridColumn
    Property FilterColumnSelected As New List(Of String)
    Property ValueColumn As New DataGridColumn
    Property AggregateFunction As String
    ' Valid values: Sum, Average, Count

    Public Sub ReturnChartObject(ByRef yourchart As Chart)
        Try
            ChartData.GridColumns.AddRange((From i In SourceData.GridColumns).ToList())
        Catch ex As Exception
            ApplicationLogging.ErrorLog("wpm_Chart.ReturnChartObject", ex.ToString)
        End Try

        For Each myRow In SourceData.GridRows
            If FilterColumnSelected.Contains(myRow.Value(FilterColumn.Index)) AndAlso
                xColumnSelected.Contains(myRow.Value(xColumn.Index)) AndAlso
                 yColumnSelected.Contains(myRow.Value(yColumn.Index)) AndAlso
                  wpm_GetDBDouble(myRow.Value(ValueColumn.Index)) > 0 Then
                ChartData.GridRows.Add(myRow)
            End If
        Next

        If ChartData.GridRows.Count > 0 Then
            For Each SelectedYColumn In yColumnSelected
                Dim thisSeriesData = (From s In (From i In ChartData.GridRows Where yColumnSelected.Contains(i.Value(yColumn.Index))).ToList() Where s.Value(yColumn.Index) = SelectedYColumn)
                Dim AxisLabel As String = String.Empty
                Dim newSeriesData As New SeriesData
                Dim iRunningScore As Double
                Dim iCount As Integer
                newSeriesData = New SeriesData
                For Each Axis In xColumnSelected
                    If Axis = String.Empty Then
                        AxisLabel = "Not Answered"
                    Else
                        AxisLabel = Axis
                    End If
                    iCount = 0
                    iRunningScore = 0
                    For Each AxisData In (From i In thisSeriesData Where i.Value(xColumn.Index) = Axis).ToList
                        iCount += 1
                        iRunningScore = iRunningScore + wpm_GetDBDouble(AxisData.Value(ValueColumn.Index))
                    Next
                    If iCount > 0 Then
                        Select Case AggregateFunction
                            Case "Sum"
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = Math.Round(iRunningScore, 2)})
                            Case "Average"
                                If iCount > 0 Then
                                    newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = Math.Round(iRunningScore / iCount, 2)})
                                Else
                                    newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = 0})
                                End If
                            Case "Count"
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = iCount})
                            Case Else
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = iRunningScore})
                        End Select
                    End If
                Next
                If SelectedYColumn = String.Empty Then
                    newSeriesData.SeriesName = "Unknown"
                Else
                    newSeriesData.SeriesName = SelectedYColumn
                End If
                If newSeriesData.Count > 0 Then
                    SeriesDataList.Add(newSeriesData)
                End If
            Next
            With yourchart
                .Legends.Clear()
                .Series.Clear()
                .ChartAreas.Clear()
                .Titles.Clear()

                For Each i In ChartTitles
                    If Not String.IsNullOrEmpty(i) Then
                        Dim myTitle As New Title
                        myTitle.Text = i
                        myTitle.Font = New System.Drawing.Font("Verdana", 12, Drawing.FontStyle.Bold)
                        .Titles.Add(myTitle)
                    End If
                Next
                .ChartAreas.Add(New ChartArea With {.Name = ChartName, .BackColor = Drawing.Color.White})
                .ChartAreas(0).AxisX.Interval = 1
                For Each s In SeriesDataList
                    Try
                        .Series.Add(s.GetSeriesData)
                    Catch

                    End Try
                Next
                .Legends.Add(New Legend With {.Name = "Master Legend"})
                .Width = ChartWidth
                .Height = ChartHeight

                Try
                    If ChartStyle = "3D" Then
                        .ChartAreas(0).Area3DStyle.Enable3D = True
                    Else
                        .ChartAreas(0).Area3DStyle.Enable3D = False
                    End If
                    For Each s As Series In .Series
                        s.ChartType = ChartType
                    Next
                    .Palette = CType(ChartPalette, ChartColorPalette)
                Catch ex As Exception
                    ApplicationLogging.ErrorLog("wpm_Chart.ReturnChartObject", ex.ToString)
                End Try
            End With
        End If
    End Sub

    Public Function SaveXML(ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New StreamWriter(sPath, False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(wpm_Chart))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Error Saving File - {0}", ex), "wpm_Chart.SaveXML")
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error Before Saving File  - {0}", ex), "wpm_Chart.SaveXML")
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function

    Public Function GetXML(ByVal sPath As String) As wpm_chart
        Dim myChartConfig As New wpm_chart
        Dim x As New XmlSerializer(GetType(wpm_chart))
        Try
            Using objStreamReader As New StreamReader(sPath)
                myChartConfig = CType(x.Deserialize(objStreamReader), wpm_chart)
                objStreamReader.Close()
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("wpm_chart.GetXML", ex.ToString)
        End Try
        Return myChartConfig
    End Function

End Class
