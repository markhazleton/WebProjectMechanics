Imports System.IO
Imports System.Text
Imports System.Web.UI.WebControls

Public Class wpmFileProcessing

    Public Shared Function CreateFile(ByRef path As String, ByVal content As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Using sw As New StreamWriter(path, False)
                sw.WriteLine(content)
            End Using
            bReturn = True
        Catch ex As Exception
            wpmLogging.AuditLog(String.Format("Error with IO.CreateFile - {0}", path), ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Shared Function DeleteFile(ByVal sPath As String) As Boolean
        Try
            My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
        Catch ex As Exception
            wpmLogging.ErrorLog(String.Format("Error with IO.DeteleteFile {0}", sPath), ex.ToString)
        End Try
        Return True
    End Function
    Public Shared Function GetTextFileContents(ByVal FullPath) As String
        Dim myFileContents As New StringBuilder
        ReadTextFile(FullPath, myFileContents)
        Return myFileContents.ToString
    End Function
    Public Shared Function ReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
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
            wpmLogging.AuditLog("Error Reading Text File", path)
        End Try
        Return bReturn
    End Function
    Public Shared Function ReadFile(ByRef path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = True
        Try
            Using sr As New StreamReader(path)
                sFileContents.Append(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            bReturn = False
            wpmLogging.AuditLog("Error Reading File", path)
        End Try
        Return bReturn
    End Function
    Public Shared Function VerifyFolderExists(ByVal sPath As String) As Boolean
        Return IO.Directory.Exists(sPath)
    End Function
    Public Shared Function CreateFolder(ByVal sPath As String) As Boolean
        Dim myDirInfo As DirectoryInfo = IO.Directory.CreateDirectory(sPath)
        Return myDirInfo.Exists
    End Function
    Public Shared Function MoveFile(ByVal fromPath As String, ByVal toPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            IO.Directory.Move(fromPath, toPath)
            bReturn = False
        Catch ex As Exception
            wpmLogging.AuditLog(ex.ToString, String.Format("mhFIO.MoveFile from ({0}) to ({1}) ", fromPath, toPath))
        Finally
            bReturn = True
        End Try
        Return bReturn
    End Function
    Public Shared Function RemoveFolder(ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            IO.Directory.Delete(sPath)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
    Public Shared Function RemoveSubFolders() As Boolean
        Return True
    End Function
    Public Shared Function GetDirectoryMenuHTML(ByVal sPath As String) As String
        Dim iMenuItemCount As Integer
        Dim mysb As New StringBuilder
        mysb.Append(String.Format("<div class=""{0}""><ul class=""{0}"">", "DirectoryMenu"))
        ' mysb.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", sPath, sPath.Replace("/", "")))
        For Each baseDir As DirectoryInfo In My.Computer.FileSystem.GetDirectoryInfo(HttpContext.Current.Server.MapPath(sPath)).GetDirectories
            If Not (baseDir.Extension.ToLower = ".svn") Then
                iMenuItemCount = iMenuItemCount + 1
                mysb.Append(String.Format("<li><a href=""{0}/{1}"">{2}</a></li>", sPath, baseDir.Name, baseDir.Name.Replace(".aspx", "")).Replace("//", "/"))
            End If
        Next
        mysb.Append("</ul></div><br/><br/>")
        If iMenuItemCount = 0 Then
            mysb.Length = 0
        End If
        Return mysb.ToString
    End Function

    Public Shared Function GetFileDropDownList(ByVal sPath As String, ByVal sExt As String) As DropDownList
        Dim myDD As New DropDownList
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                Dim li As New ListItem() With {.Text = IO.Path.GetFileName(baseFile), .Value = baseFile}
                myDD.Items.Add(li)
            End If
        Next
        Return myDD
    End Function

    Public Shared Function GetASPXMenuHTML(ByVal sPath As String) As String
        Dim mySB As New StringBuilder
        Dim iMenuItemCount As Integer
        mySB.Append(String.Format("<div class=""{0}""><ul class=""{0}"">", "FileMenu"))
        For Each baseFile As FileInfo In My.Computer.FileSystem.GetDirectoryInfo(sPath).GetFiles()
            If baseFile.Extension.ToLower = ".aspx" AndAlso baseFile.Name.ToLower <> "default.aspx" Then
                iMenuItemCount = iMenuItemCount + 1
                mySB.Append(String.Format("<li><a href=""{0}"">{1}</a></li>", baseFile.Name, baseFile.Name.Replace(".aspx", "")))
            End If
        Next
        mySB.Append("</ul></div><br/><br/>")
        If iMenuItemCount = 0 Then
            mySB.Length = 0
        End If
        Return mySB.ToString
    End Function
    Public Shared Function GetFileListHTML(ByVal sPath As String) As String
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
    Public Shared Function GetFileList(ByVal sPath As String, ByVal sExt As String) As String
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                mySB.Append("<br/>" & baseFile)
            End If
        Next
        Return mySB.ToString
    End Function
    Public Shared Function IsValidFolder(ByVal sFolderPath As String) As Boolean
        Return My.Computer.FileSystem.DirectoryExists(sFolderPath)
    End Function
    Public Shared Function FileExists(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Shared Function IsValidPath(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Shared Function GetValidAbsolutePath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = wpmApp.Config.ConfigFolderPath & sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmLogging.AuditLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Shared Function GetValidPath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmLogging.AuditLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Shared Function GetFileDate(ByVal path As String) As Date
        Return IO.File.GetLastWriteTime(path)
    End Function
    Public Shared Function GetFileAgeMinutes(ByVal path As String) As Double
        If IsValidPath(path) Then
            Return System.DateTime.Now.Subtract(GetFileDate(path)).TotalMinutes
        Else
            Return 100000
        End If
    End Function
    Public Shared Function ClearFolder(ByVal sFolderPath As String) As Boolean
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
    Public Shared Function CompareFileName(ByVal ImageFileName As String, ByVal DirectoryFileName As String) As Boolean
        Dim bReturn As Boolean = False
        If DirectoryFileName <> "" And Not IsNothing(DirectoryFileName) Then
            ImageFileName = (Replace(ImageFileName.ToLower, "/", "\"))
            DirectoryFileName = (Replace(DirectoryFileName.ToLower, "/", "\"))
            If ImageFileName = DirectoryFileName Then
                bReturn = True
            End If
        End If
        Return bReturn
    End Function


End Class
