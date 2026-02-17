Imports System.Text

Public Class wpm_PivotTable
    Public Property myColList As New ColumnList
    Public Property myRowList As New RowList
    Public Property myValList As New ValList
    Public Property myExclusionList As New ExclusionList
    Public Property unusedAttrsVertical As String
    Public Property autoSortUnusedAttrs As String
    Public Property aggregatorName As String
    Public Property rendererName As String
    Public Property RequestDataSet As DataSet
    Public Property TargetDIVID As String = "output"
    Public Property ApplicationID As String
    Public Property ApplicationUserID As String


    Public Function RenderJS() As String
        Dim mySB As New StringBuilder
        Dim CSVPath As String = String.Empty

        Select Case RequestDataSet
            Case DataSet.Summary
                CSVPath = String.Format("/Co_Apps/SurveyApp/module/CookiesHandler.ashx?method=GetSurveySummaryCSV&ApplicationID={0}&ApplicationUserID={1}", ApplicationID, ApplicationUserID)
            Case DataSet.QuestionGroup
                CSVPath = String.Format("/Co_Apps/SurveyApp/module/CookiesHandler.ashx?method=GetSurveyGroupSummaryCSV&ApplicationID={0}&ApplicationUserID={1}", ApplicationID, ApplicationUserID)
            Case DataSet.Answers
                CSVPath = String.Format("/Co_Apps/SurveyApp/module/CookiesHandler.ashx?method=GetSurveyAnswersCSV&ApplicationID={0}&ApplicationUserID={1}", ApplicationID, ApplicationUserID)
            Case Else
                CSVPath = String.Format("/Co_Apps/SurveyApp/module/CookiesHandler.ashx?method=GetSurveySummaryCSV&ApplicationID={0}&ApplicationUserID={1}", ApplicationID, ApplicationUserID)
        End Select
        mySB.AppendLine("  <script type='text/javascript'>                  ")
        mySB.AppendLine("  google.load('visualization', '1', { packages: ['corechart', 'charteditor'] });       ")
        mySB.AppendLine("     ")
        mySB.AppendLine("  $(function () {                ")
        mySB.AppendLine("     var derivers = $.pivotUtilities.derivers;             ")
        mySB.AppendLine("     var renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.gchart_renderers);           ")
        mySB.AppendFormat("     $.get('{0}', function (mps)     ", CSVPath)
        mySB.AppendLine(" {    ")
        mySB.AppendFormat("  $('#{0}').pivotUI($.csv.toArrays(mps) ", TargetDIVID)
        mySB.AppendLine(" , { ")

        mySB.Append(myColList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myRowList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myValList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myExclusionList.RenderJS)
        mySB.Append(" , ")

        mySB.AppendFormat(" unusedAttrsVertical: '{0}', ", unusedAttrsVertical)
        mySB.AppendFormat(" autoSortUnusedAttrs: {0}, ", autoSortUnusedAttrs)

        mySB.AppendFormat(" aggregatorName: '{0}', ", aggregatorName)
        mySB.AppendFormat(" rendererName: '{0}', ", rendererName)

        'mySB.AppendLine(" onRefresh: function (config) { ")
        'mySB.AppendLine(" //delete some bulky default values ")
        'mySB.AppendLine(" delete config['rendererOptions']; ")
        'mySB.AppendLine(" delete config['localeStrings']; ")
        'mySB.AppendLine(" $('#config_json').text(JSON.stringify(config, undefined, 2));} ")

        mySB.AppendLine(" } ") ' close line 27 (Arguement List) 

        mySB.AppendFormat("  );            ")
        mySB.AppendLine("     ")
        mySB.Append("     });               ")
        mySB.Append("  });                  ")
        mySB.Append("  </script>             ")
        mySB.Append("     ")
        Return mySB.ToString
    End Function
    Public Function RenderTableJS() As String
        Dim mySB As New StringBuilder
        Dim CSVPath As String = String.Empty

        mySB.AppendLine("  <script type='text/javascript'>                  ")
        mySB.AppendLine("  google.load('visualization', '1', { packages: ['corechart', 'charteditor'] });       ")
        mySB.AppendLine("     ")
        mySB.AppendLine("  $(function () {                ")
        mySB.AppendLine("     var derivers = $.pivotUtilities.derivers;             ")
        mySB.AppendLine("     var renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.gchart_renderers);           ")

        mySB.AppendLine("     var input = $(""table.pivot_data"");           ")

        mySB.AppendLine("  $('#output').pivotUI(input, { ")
        mySB.AppendLine("  ")

        mySB.Append(myColList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myRowList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myValList.RenderJS)
        mySB.Append(" , ")

        mySB.Append(myExclusionList.RenderJS)
        mySB.Append(" , ")

        mySB.AppendFormat(" unusedAttrsVertical: '{0}', ", unusedAttrsVertical)
        mySB.AppendFormat(" autoSortUnusedAttrs: {0}, ", autoSortUnusedAttrs)

        mySB.AppendFormat(" aggregatorName: '{0}', ", aggregatorName)
        mySB.AppendFormat(" rendererName: '{0}', ", rendererName)

        'mySB.AppendLine(" onRefresh: function (config) { ")
        'mySB.AppendLine(" //delete some bulky default values ")
        'mySB.AppendLine(" delete config['rendererOptions']; ")
        'mySB.AppendLine(" delete config['localeStrings']; ")
        'mySB.AppendLine(" $('#config_json').text(JSON.stringify(config, undefined, 2));} ")

        mySB.AppendLine(" } ") ' close line 27 (Arguement List) 

        mySB.AppendFormat("  );            ")
        mySB.AppendLine("     ")
        mySB.Append("     });               ")
        mySB.Append("  });                  ")
        mySB.Append("  </script>             ")
        mySB.Append("     ")
        Return mySB.ToString

    End Function



    Public Enum DataSet
        Summary = 1
        QuestionGroup = 2
        Answers = 3
        ApplicationUsers = 4
    End Enum



    Public Class ColumnItem
        Public Property Name As String
        Public Property DataType As String

    End Class

    Public Class ColumnList
        Inherits System.Collections.Generic.List(Of ColumnItem)

        Public Function RenderJS() As String
            Dim output As New StringBuilder
            output.Append(" cols: [")
            For Each c In Me
                output.AppendFormat("'{0}'", c.Name)
                If Not c Is Me.Last Then output.Append(",")
            Next
            output.Append("]")
            Return output.ToString
        End Function
    End Class
    Public Class RowItem
        Public Property Name As String
    End Class

    Public Class RowList
        Inherits System.Collections.Generic.List(Of RowItem)

        Public Function RenderJS() As String
            Dim output As New StringBuilder
            output.Append("rows: [")
            For Each c In Me
                output.AppendFormat("'{0}'", c.Name)
                If Not c Is Me.Last Then output.Append(",")
            Next
            output.Append("]")
            Return output.ToString
        End Function
    End Class

    Public Class ValItem
        Public Property Name As String
    End Class

    Public Class ValList
        Inherits System.Collections.Generic.List(Of ValItem)

        Public Function RenderJS() As String
            Dim output As New StringBuilder
            output.Append("vals:[")
            For Each c In Me
                output.AppendFormat("'{0}'", c.Name)
                If Not c Is Me.Last Then output.Append(",")
            Next
            output.Append("]")
            Return output.ToString
        End Function
    End Class

    Public Class ExclusionItem
        Public Property Name As String
        Public Property Value As String

    End Class

    Public Class ExclusionList
        Inherits System.Collections.Generic.List(Of ExclusionItem)

        Public Function RenderJS() As String
            Dim output As New StringBuilder
            output.Append(" exclusions: {")
            For Each c In Me
                output.AppendFormat("'{0}': ['{1}']", c.Name, c.Value)
                If Not c Is Me.Last Then output.Append(",")
            Next
            output.Append("}")
            Return output.ToString
        End Function
    End Class

End Class

