Imports LumenWorks.Framework.IO.Csv
Imports System.Data
Imports System.IO
Imports WebProjectMechanics

Public Class wpm_admin_ReadLogFile
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            file_DatabindListbox(Server.MapPath(wpm_SiteConfig.ConfigFolder & "log/"), ".csv")
        End If
    End Sub
    Private Sub file_DatabindListbox(ByVal sPath As String, ByVal sExt As String)
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                Dim li As New ListItem
                li.Text = IO.Path.GetFileName(baseFile)
                li.Value = baseFile
                myFileListBox.Items.Add(li)
            End If
        Next
    End Sub

    Private Function FetchFromCSV(ByVal filePath As String) As List(Of ErrorLog)
        Dim myErrorList As New List(Of ErrorLog)
        Using csvReader As New CsvReader(New StreamReader(filePath), True, ","c)
            Dim fieldCount As Integer = csvReader.FieldCount
            Dim headers As String() = csvReader.GetFieldHeaders()
            While csvReader.ReadNextRecord()
                Try
                    Dim newLog As New ErrorLog
                    With newLog
                        .UserName = csvReader(0)
                        .HostName = csvReader(1)
                        .RequestURL = csvReader(2)
                        .ModDate = csvReader(3)
                        .ModTime = csvReader(4)
                        .MessageOne = csvReader(5)
                        .MessageTwo = csvReader(6)
                    End With
                    myErrorList.Add(newLog)
                Catch ex As Exception

                End Try

            End While
        End Using
        Return myErrorList
    End Function
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim myObjects As New List(Of Object)
        myObjects.AddRange(FetchFromCSV(myFileListBox.SelectedValue))
        dtList.BuildTable(myObjects)
    End Sub
    Private Class ErrorLog
        Public Property UserName As String
        Public Property HostName As String
        Public Property RequestURL As String
        Public Property ModDate As String
        Public Property ModTime As String
        Public Property MessageOne As String
        Public Property MessageTwo As String
    End Class

End Class

'Private Sub Afile_DatabindListbox(ByVal sPath As String, ByVal sExt As String)
'    For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
'        If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
'            Dim myItem As New ListItem
'            With myItem
'                .Text = IO.Path.GetFileName(baseFile)
'                .Value = baseFile
'            End With
'            myFileListBox.Items.Add(myItem)
'        End If
'    Next
'End Sub
'Public Shared Function AFetchFromCSVFileLong(ByVal filePath As String) As DataTable
'    Dim csvTable As New DataTable()
'    Using csvReader As New CsvReader(New StreamReader(filePath), True, ","c)

'        Dim fieldCount As Integer = csvReader.FieldCount
'        Dim headers As String() = csvReader.GetFieldHeaders()

'        ' this bit could be modified to fine-tune the columns
'        For Each headerLabel As String In headers
'            csvTable.Columns.Add(headerLabel, GetType(String))
'        Next
'        csvTable.Columns.Add("RowNumber", GetType(Integer))
'        Dim iRowNumber As Integer = 1

'        While csvReader.ReadNextRecord()
'            Dim newRow As DataRow = csvTable.NewRow()
'            ' this bit could be modified to do type conversions, skip columns, etc
'            For i As Integer = 0 To fieldCount - 1
'                newRow(i) = csvReader(i)
'            Next
'            newRow(fieldCount) = iRowNumber
'            iRowNumber = iRowNumber + 1

'            csvTable.Rows.Add(newRow)
'        End While
'    End Using
'    Return csvTable
'End Function
'
'Private Shared Function ReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
'    Dim bReturn As Boolean = False
'    Try
'        ' Create an instance of StreamReader to read from a file.
'        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
'        Dim line As String
'        sFileContents.Append(vbCrLf & "<br/>")
'        ' Read and display the lines from the file until the end 
'        ' of the file is reached.
'        Do
'            line = sr.ReadLine()
'            sFileContents.Append(String.Format("{0}{1}<br/>", line, vbCrLf))
'        Loop Until line Is Nothing
'        sFileContents.Append(vbCrLf)
'        sr.Close()
'        bReturn = True
'    Catch E As Exception
'        ' Let the user know what went wrong.
'        ApplicationLogging.AuditLog("Error Reading Text File", path)
'    End Try
'    Return bReturn
'End Function
'Private Shared Function ReadCSVFileToArray(ByVal Path As String, ByRef sFileContents As StringBuilder) As Boolean
'    Dim num_rows As Long
'    Dim num_cols As Long
'    Dim x As Integer
'    Dim y As Integer
'    Dim strarray(1, 1) As String


'    'Check if file exist
'    If File.Exists(Path) Then
'        Dim tmpstream As StreamReader = File.OpenText(Path)
'        Dim strlines() As String
'        Dim strline() As String

'        'Load content of file to strLines array
'        strlines = tmpstream.ReadToEnd().Split(Environment.NewLine)

'        ' Redimension the array.
'        num_rows = UBound(strlines)
'        strline = strlines(0).Split(",")
'        num_cols = UBound(strline)
'        ReDim strarray(num_rows, num_cols)

'        ' Copy the data into the array.
'        For x = 0 To num_rows - 1
'            strline = strlines(x).Split(",")
'            For y = 0 To num_cols - 1
'                strarray(x, y) = strline(y)
'            Next
'        Next

'        ' Display the data in textbox
'        sFileContents.Append("<table border=2>")
'        For x = 0 To num_rows
'            sFileContents.Append("<tr>")
'            For y = 0 To num_cols
'                sFileContents.Append(String.Format("<td>{0}</td>", strarray(x, y)))
'            Next
'            sFileContents.Append("</tr>")
'        Next
'        sFileContents.Append("</table>")
'    End If
'    Return True
'End Function
'Public Shared Function AReadCSVFileToArray(ByVal Path As String, ByRef sFileContents As StringBuilder) As String
'    Dim num_rows As Long
'    Dim num_cols As Long
'    Dim x As Integer
'    Dim y As Integer
'    Dim strarray(1, 1) As String
'    Dim strlines() As String
'    Dim strline() As String

'    If File.Exists(Path) Then
'        Using tmpstream As StreamReader = File.OpenText(Path)
'            'Load content of file to strLines array
'            strlines = tmpstream.ReadToEnd().Split(Environment.NewLine)
'            ' Redimension the array.
'            num_rows = UBound(strlines)
'            strline = strlines(0).Split(",")
'            num_cols = UBound(strline)
'            ReDim strarray(num_rows, num_cols)
'            ' Copy the data into the array.
'            For x = 0 To num_rows - 1
'                strline = strlines(x).Split(",")
'                For y = 0 To num_cols - 1
'                    strarray(x, y) = strline(y)
'                Next
'            Next
'            ' Display the data in textbox
'            sFileContents.Append("<table border=2>")
'            For x = 0 To num_rows
'                sFileContents.Append("<tr>")
'                For y = 0 To num_cols
'                    sFileContents.Append(String.Format("<td>{0}</td>", strarray(x, y)))
'                Next
'                sFileContents.Append("</tr>")
'            Next
'        End Using
'        sFileContents.Append("</table>")
'    End If
'    Return sFileContents.ToString
'End Function
'Public Shared Function AReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
'    Dim bReturn As Boolean = False
'    Try
'        ' Create an instance of StreamReader to read from a file.
'        Using sr As StreamReader = New StreamReader(path)
'            Dim line As String
'            sFileContents.Append(vbCrLf & "<br/>")
'            ' Read and display the lines from the file until the end 
'            ' of the file is reached.
'            Do
'                line = sr.ReadLine()
'                sFileContents.Append(String.Format("{0}{1}<br/>", line, vbCrLf))
'            Loop Until line Is Nothing
'            sFileContents.Append(vbCrLf)
'            sr.Close()
'        End Using
'        bReturn = True
'    Catch E As Exception
'        ' Let the user know what went wrong.
'        ApplicationLogging.AuditLog("Error Reading Text File", path)
'    End Try
'    Return bReturn
'End Function
    'Private Sub GetCSVTable(ByVal myCSV As String)
    '    Dim mySB As New StringBuilder("")
    '    Dim iCol As Integer
    '    Dim iRow As Integer
    '    mySB.Append("<div class=""table-responsive""><table class=""table table-striped table-bordered table-hover"" id=""dataTables-admin"">")
    '    For Each myRow As DataRow In FetchFromCSVFileLong(myCSV).Rows
    '        If iRow = 0 Then
    '            iRow = iRow + 1
    '            mySB.Append("<tr>")
    '            For iCol = 0 To myRow.ItemArray.Length - 1
    '                mySB.Append(String.Format("<th>{0}</th>", String.Format("Column {0}", iCol)))
    '            Next
    '            mySB.Append("</tr>")
    '        End If
    '        mySB.Append("<tr>")
    '        For iCol = 0 To myRow.ItemArray.Length - 1
    '            mySB.Append(String.Format("<td>{0}</td>", myRow.Item(iCol)))
    '        Next
    '        mySB.Append("</tr>")
    '    Next
    '    mySB.Append("</table></div>")
    '    MyHTML.Text = mySB.ToString
    'End Sub
    'Private Shared Function FetchFromCSVFileLong(ByVal filePath As String) As DataTable
    '    Dim csvTable As New DataTable()
    '    Using csvReader As New CsvReader(New StreamReader(filePath), True, ","c)
    '        Dim fieldCount As Integer = csvReader.FieldCount
    '        Dim headers As String() = csvReader.GetFieldHeaders()

    '        ' this bit could be modified to fine-tune the columns
    '        For Each headerLabel As String In headers
    '            csvTable.Columns.Add(headerLabel, GetType(String))
    '        Next

    '        While csvReader.ReadNextRecord()
    '            Dim newRow As DataRow = csvTable.NewRow()
    '            ' this bit could be modified to do type conversions, skip columns, etc
    '            For i As Integer = 0 To fieldCount - 1
    '                newRow(i) = csvReader(i)
    '            Next
    '            csvTable.Rows.Add(newRow)
    '        End While
    '    End Using
    '    Return csvTable
    'End Function
