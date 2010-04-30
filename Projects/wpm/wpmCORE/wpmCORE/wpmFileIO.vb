Imports System.Web

Public Class wpmFileIO

    Public Shared Function CreateFile(ByRef path As String, ByVal content As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Using sw As New StreamWriter(path, False)
                sw.WriteLine(content)
            End Using
            bReturn = True
        Catch ex As Exception
            wpmLog.AuditLog("Error with mhFIO.CreateFile - " & path, ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Shared Function DeleteFile(ByVal sPath As String) As Boolean
        Try
            My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
        Catch
        End Try
    End Function
    Public Shared Function ReadTextFile(ByVal path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = False
        Try
            ' Create an instance of StreamReader to read from a file.
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(path)
            Dim line As String
            sFileContents.Append(vbCrLf)
            ' Read and display the lines from the file until the end 
            ' of the file is reached.
            Do
                line = sr.ReadLine()
                sFileContents.Append(line & vbCrLf)
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
    Public Shared Function ReadFile(ByRef path As String, ByRef sFileContents As StringBuilder) As Boolean
        Dim bReturn As Boolean = True
        Try
            Using sr As New StreamReader(path)
                sFileContents.Append(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            bReturn = False
            wpmLog.AuditLog("Error Reading File", path)
        End Try
        Return bReturn
    End Function
    Public Shared Function VerifyFolderExists(ByVal sPath As String) As Boolean
        Return IO.Directory.Exists(sPath)
    End Function
    Public Shared Function CreateFolder(ByVal sPath As String) As Boolean
        Dim myDirInfo As System.IO.DirectoryInfo
        myDirInfo = IO.Directory.CreateDirectory(sPath)
        Return myDirInfo.Exists
    End Function
    Public Shared Function MoveFile(ByVal fromPath As String, ByVal toPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            IO.Directory.Move(fromPath, toPath)
            bReturn = False
        Catch ex As Exception
            wpmLog.AuditLog(ex.ToString, "mhFIO.MoveFile from (" & fromPath & ") to (" & toPath & ") ")
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
    Public Shared Function RemoveSubFolders(ByVal sPath As String) As Boolean
        Return True
    End Function
    Public Shared Function GetFileListHTML(ByVal sPath As String) As String
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            mySB.Append("<br/>" & baseFile)
        Next
        For Each FoundDir As String In My.Computer.FileSystem.GetDirectories(sPath)
            mySB.Append("<p><strong>" & FoundDir & "</strong><ul>")
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(FoundDir)
                mySB.Append("<li>" & foundFile & "</li>")
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
        Dim sPath As String = wpmConfig.wpmConfigFile & sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmLog.AuditLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Shared Function GetValidPath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmLog.AuditLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Shared Function GetFileDate(ByVal path As String) As Date
        Return IO.File.GetLastWriteTime(path)
    End Function
    Public Shared Function GetFileAgeMinutes(ByVal path As String) As Double
        Dim iFileAge As Double = 100000
        If IsValidPath(path) Then
            iFileAge = System.DateTime.Now.Subtract(GetFileDate(path)).TotalMinutes
        End If
        Return iFileAge
    End Function
    Public Shared Function ClearFolder(ByVal sFolderPath As String) As Boolean
        Dim bError As Boolean = False
        Try
            Dim fileEntries As String() = Directory.GetFiles(sFolderPath)
            ' Process the list of files found in the directory.
            Dim fileName As String
            For Each fileName In fileEntries
                wpmFileIO.DeleteFile(fileName)
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
