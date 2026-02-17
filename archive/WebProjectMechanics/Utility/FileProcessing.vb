Imports System.IO
Imports System.Text
Imports System.Web.UI.WebControls

Public Module FileProcessing
    Public Function GetValidFolderPath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String
        sRelativeFilePath = Replace(sRelativeFilePath, "\", "/")
        sRelativeFilePath = Replace(sRelativeFilePath, "//", "/")
        Try
            sPath = HttpContext.Current.Server.MapPath(sRelativeFilePath)
        Catch ex As Exception
            sPath = String.Empty
            ApplicationLogging.ErrorLog(String.Format("Invalid File - {0}", sPath), sCallingProcess)
        End Try
        Return sPath
    End Function
    Public Function IsImageExtension(ByRef sExtension As String) As Boolean
        Select Case sExtension.ToLower
            Case ".gif"
                Return True
            Case ".jpg"
                Return True
            Case ".png"
                Return True
            Case ".jpeg"
                Return True
            Case Else
                Return False
        End Select
    End Function
    Public Function GetImageFileCollection(ByVal sRelativeFolderPath As String) As List(Of String)
        Dim myList As New List(Of String)
        Dim myPath As String = GetValidFolderPath(sRelativeFolderPath, "FileProcessing")
        If IsValidFolder(myPath) Then
            For Each baseFile As FileInfo In My.Computer.FileSystem.GetDirectoryInfo(myPath).GetFiles()
                If IsImageExtension(baseFile.Extension.ToLower) Then
                    myList.Add(baseFile.Name)
                End If
            Next
        End If
        Return myList
    End Function
    Public Function CreateFile(ByRef path As String, ByVal content As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Using sw As New StreamWriter(path, False)
                Try
                    sw.WriteLine(content)
                    bReturn = True
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Error with IO.CreateFile - {0}", path), ex.ToString)
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error with IO.CreateFile - {0}", path), ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Function DeleteFile(ByVal sPath As String) As Boolean
        Try
            My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error with IO.DeteleteFile {0}", sPath), ex.ToString)
        End Try
        Return True
    End Function
    Public Function GetTextFileContents(ByVal FullPath As String) As String
        Dim myFileContents As New StringBuilder
        ReadTextFile(FullPath, myFileContents)
        Return myFileContents.ToString
    End Function
    Public Function ReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = False
        Try
            ' Create an instance of StreamReader to read from a file.
            Using sr As StreamReader = New StreamReader(path)
                Dim line As String
                sFileContents.Append(vbCrLf)
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    sFileContents.Append(line & vbCrLf)
                Loop Until line Is Nothing
                sFileContents.Append(vbCrLf)
            End Using
            bReturn = True
        Catch E As Exception
            ' Let the user know what went wrong.
            ApplicationLogging.ErrorLog("Error Reading Text File", path)
        End Try
        Return bReturn
    End Function
    Public Function ReadFile(ByRef path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = True
        Try
            Using sr As New StreamReader(path)
                sFileContents.Append(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            bReturn = False
            ApplicationLogging.ErrorLog("Error Reading File", path)
        End Try
        Return bReturn
    End Function
    Public Function VerifyFolderExists(ByVal sPath As String) As Boolean
        Return Directory.Exists(sPath)
    End Function
    Public Function CreateFolder(ByVal sPath As String) As Boolean
        Dim myDirInfo As DirectoryInfo = Directory.CreateDirectory(sPath)
        Return myDirInfo.Exists
    End Function
    Public Function MoveFile(ByVal fromPath As String, ByVal toPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Directory.Move(fromPath, toPath)
            bReturn = False
        Catch ex As Exception
            ApplicationLogging.ErrorLog(ex.ToString, String.Format("wpmFileProcessing.MoveFile from ({0}) to ({1}) ", fromPath, toPath))
        Finally
            bReturn = True
        End Try
        Return bReturn
    End Function
    Public Function GetFolders(ByVal sPath As String) As String()
        Return IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath(sPath))
    End Function
    Public Function RemoveFolder(ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            Directory.Delete(sPath, True)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
    Public Function RemoveSubFolders() As Boolean
        Return True
    End Function
    Public Function GetDirectoryMenuHTML(ByVal sPath As String) As String
        Dim iMenuItemCount As Integer
        Dim mysb As New StringBuilder
        mysb.Append(String.Format("<div class=""{0}""><ul class=""{0}"">", "DirectoryMenu"))
        ' mysb.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", sPath, sPath.Replace("/", "")))
        For Each baseDir As DirectoryInfo In My.Computer.FileSystem.GetDirectoryInfo(HttpContext.Current.Server.MapPath(sPath)).GetDirectories
            If Not (baseDir.Extension.ToLower = ".svn") Then
                iMenuItemCount = iMenuItemCount + 1
                mysb.Append(String.Format("<li><a href=""{0}/{1}"">{2}</a></li>", sPath, baseDir.Name, baseDir.Name.Replace(".aspx", String.Empty)).Replace("//", "/"))
            End If
        Next
        mysb.Append("</ul></div><br/><br/>")
        If iMenuItemCount = 0 Then
            mysb.Length = 0
        End If
        Return mysb.ToString
    End Function
    Public Function GetFileDropDownList(ByVal sPath As String, ByVal sExt As String) As DropDownList
        Using myDD As New DropDownList()
            For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
                If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                    Dim li As New ListItem() With {.Text = Path.GetFileName(baseFile), .Value = baseFile}
                    myDD.Items.Add(li)
                End If
            Next
            Return myDD
        End Using
    End Function
    Public Function GetASPXMenuHTML(ByVal sPath As String) As String
        Dim mySB As New StringBuilder
        Dim iMenuItemCount As Integer
        mySB.Append(String.Format("<div class=""{0}""><ul class=""{0}"">", "FileMenu"))
        For Each baseFile As FileInfo In My.Computer.FileSystem.GetDirectoryInfo(sPath).GetFiles()
            If baseFile.Extension.ToLower = ".aspx" AndAlso baseFile.Name.ToLower <> "default.aspx" Then
                iMenuItemCount = iMenuItemCount + 1
                mySB.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", baseFile.Name, baseFile.Name.Replace(".aspx", String.Empty)))
            End If
        Next
        mySB.Append("</ul></div><br/><br/>")
        If iMenuItemCount = 0 Then
            mySB.Length = 0
        End If
        Return mySB.ToString
    End Function
    Public Function GetFileListHTML(ByVal sPath As String) As String
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            mySB.Append("<br/>" & baseFile)
        Next
        For Each FoundDir As String In My.Computer.FileSystem.GetDirectories(sPath)
            mySB.Append(String.Format("<p><strong>{0}</strong><ul>", FoundDir))
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(FoundDir)
                mySB.Append(String.Format("<li>{0}</li>", foundFile))
            Next
            mySB.Append("</ul></p>")
        Next
        Return mySB.ToString
    End Function
    Public Function GetFolderFiles(ByVal sPath As String, ByVal sExt As String) As List(Of LookupItem)
        Dim sFolderPath As String = sPath
        Try
            If Not IsValidFolder(sPath) Then
                If IsValidFolder(HttpContext.Current.Server.MapPath(sPath)) Then
                    sFolderPath = HttpContext.Current.Server.MapPath(sPath)
                End If
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog(ex.ToString, "FileProcessing.GetFolderFiles")
        End Try

        Return (From i In My.Computer.FileSystem.GetFiles(sFolderPath) Select New LookupItem With {.Name = Right(i, Len(i) - i.LastIndexOf("\") - 1), .Value = Right(i, Len(i) - i.LastIndexOf("\") - 1)}).ToList()
    End Function

    Public Function GetFileList(ByVal sPath As String, ByVal sExt As String) As String
        Dim mySB As New StringBuilder
        Dim sFolderPath As String = sPath

        Try
            If Not IsValidFolder(sPath) Then
                If IsValidFolder(HttpContext.Current.Server.MapPath(sPath)) Then
                    sFolderPath = HttpContext.Current.Server.MapPath(sPath)
                End If
            End If
        Catch ex As Exception

        End Try

        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sFolderPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                mySB.Append("<br/>" & baseFile)
            End If
        Next
        Return mySB.ToString
    End Function
    Public Function IsValidFolder(ByVal sFolderPath As String) As Boolean
        Return My.Computer.FileSystem.DirectoryExists(sFolderPath)
    End Function
    Public Function FileExists(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Function IsValidPath(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Function GetValidAbsolutePath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = wpm_SiteConfig.ConfigFolderPath & sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = String.Empty
            ApplicationLogging.ErrorLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Function GetValidPath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = String.Empty
            ApplicationLogging.ErrorLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Function GetFileDate(ByVal path As String) As Date
        Return File.GetLastWriteTime(path)
    End Function
    Public Function GetFileAgeMinutes(ByVal path As String) As Double
        If IsValidPath(path) Then
            Return DateTime.Now.Subtract(GetFileDate(path)).TotalMinutes
        Else
            Return 100000
        End If
    End Function
    Public Function ClearFolder(ByVal sFolderPath As String) As Boolean
        Dim bError As Boolean = False
        Try
            Dim fileEntries As String() = Directory.GetFiles(sFolderPath)
            ' Process the list of files found in the directory.
            Dim fileName As String
            For Each fileName In fileEntries
                DeleteFile(fileName)
            Next fileName
        Catch ex As Exception
            bError = True
        End Try
        Return bError
    End Function
    Public Function CompareFileName(ByVal ImageFileName As String, ByVal DirectoryFileName As String) As Boolean
        Dim bReturn As Boolean = False
        If DirectoryFileName <> String.Empty And Not IsNothing(DirectoryFileName) Then
            ImageFileName = (Replace(ImageFileName.ToLower, "/", "\"))
            DirectoryFileName = (Replace(DirectoryFileName.ToLower, "/", "\"))
            If ImageFileName = DirectoryFileName Then
                bReturn = True
            End If
        End If
        Return bReturn
    End Function


End Module
