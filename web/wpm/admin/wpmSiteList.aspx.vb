Imports LumenWorks.Framework.IO.Csv

Partial Class wpm_admin_ReadLogFile
    Inherits AspNetMaker7_WPMGen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            file_DatabindListbox(Server.MapPath(App.Config.ConfigFolder), ".mdb")
            GridView1.AllowSorting = False
            GridView1.AllowPaging = False
        End If
    End Sub
    Private Sub GetCSVTable(ByVal myCSV As String)
        Dim mySB As New StringBuilder("")
        mySB.Append("<table border=2>")
        For Each myRow As DataRow In FetchFromCSVFileLong(myCSV).Rows
            mySB.Append("<tr>")
            For i = 0 To myRow.ItemArray.Length - 1
                mySB.Append("<td>" & myRow.Item(i).ToString & "</td>")
            Next
            mySB.Append("</tr>")
        Next
        mySB.Append("</table>")
        MyHTML.Text = mySB.ToString
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


    Public Function FetchFromCSVFileLong(ByVal filePath As String) As DataTable
        Dim delimiter As Char = ","c
        Dim hasHeader As Boolean = True
        Dim csvTable As New DataTable()
        Using csvReader As New CsvReader(New StreamReader(filePath), hasHeader, delimiter)

            Dim fieldCount As Integer = csvReader.FieldCount
            Dim headers As String() = csvReader.GetFieldHeaders()

            ' this bit could be modified to fine-tune the columns
            For Each headerLabel As String In headers
                csvTable.Columns.Add(headerLabel, GetType(String))
            Next

            While csvReader.ReadNextRecord()
                Dim newRow As DataRow = csvTable.NewRow()
                ' this bit could be modified to do type conversions, skip columns, etc
                For i As Integer = 0 To fieldCount - 1
                    newRow(i) = csvReader(i)
                Next
                csvTable.Rows.Add(newRow)
            End While
        End Using
        Return csvTable
    End Function


    Private Function ReadCSVFileToArray(ByVal Path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim num_rows As Long
        Dim num_cols As Long
        Dim x As Integer
        Dim y As Integer
        Dim strarray(1, 1) As String


        'Check if file exist
        If File.Exists(Path) Then
            Dim tmpstream As StreamReader = File.OpenText(Path)
            Dim strlines() As String
            Dim strline() As String

            'Load content of file to strLines array
            strlines = tmpstream.ReadToEnd().Split(Environment.NewLine)

            ' Redimension the array.
            num_rows = UBound(strlines)
            strline = strlines(0).Split(",")
            num_cols = UBound(strline)
            ReDim strarray(num_rows, num_cols)

            ' Copy the data into the array.
            For x = 0 To num_rows - 1
                strline = strlines(x).Split(",")
                For y = 0 To num_cols - 1
                    strarray(x, y) = strline(y)
                Next
            Next

            ' Display the data in textbox
            sFileContents.Append("<table border=2>")
            For x = 0 To num_rows
                sFileContents.Append("<tr>")
                For y = 0 To num_cols
                    sFileContents.Append("<td>" & strarray(x, y) & "</td>")
                Next
                sFileContents.Append("</tr>")
            Next
            sFileContents.Append("</table>")
        End If
        Return True
    End Function


    Public Shared Function ReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = False
        Try
            ' Create an instance of StreamReader to read from a file.
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
            Dim line As String
            sFileContents.Append(vbCrLf & "<br/>")
            ' Read and display the lines from the file until the end 
            ' of the file is reached.
            Do
                line = sr.ReadLine()
                sFileContents.Append(line & vbCrLf & "<br/>")
            Loop Until line Is Nothing
            sFileContents.Append(vbCrLf)
            sr.Close()
            bReturn = True
        Catch E As Exception
            ' Let the user know what went wrong.
            wpmLog.AuditLog("Error Reading Text File", path)
        End Try
        Return bReturn
    End Function

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'GetCSVTable(myFileListBox.SelectedValue)
        GridView1.DataSource = FetchFromCSVFileLong(myFileListBox.SelectedValue)
        GridView1.DataBind()
    End Sub


    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        GridView1.DataSource = FetchFromCSVFileLong(myFileListBox.SelectedValue)
        GridView1.DataBind()
    End Sub
End Class
