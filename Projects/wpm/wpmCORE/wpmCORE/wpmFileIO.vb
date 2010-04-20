Imports System.Web

Public Class wpmFileIO
    Public Shared Function SaveTemplateFile(ByVal sTemplate As String, ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            wpmFileIO.CreateFile(sPath, sTemplate)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
    Public Shared Function SaveHTML(ByVal sPageName As String, ByVal sContent As String) As Boolean
        Dim sPath As String = App.Config.ConfigFolderPath & "\html\"
        Dim bReturn As Boolean = False
        If Trim(sPageName) <> "" Then
            If (wpmFileIO.VerifyFolderExists(sPath)) Then
                If App.Config.DefaultExtension = String.Empty Then
                    sPath = sPath & sPageName & ".html"
                Else
                    sPath = sPath & sPageName
                End If
                bReturn = wpmFileIO.CreateFile(sPath, sContent)
            End If
        End If
        Return bReturn
    End Function

    Public Shared Function SavePageFile(ByVal sPageName As String, ByVal sContent As String, ByVal RawURL As String) As Boolean
        Dim sPath As String = App.Config.ConfigFolderPath & "\gen\"
        Dim sSavePageName As String = GetTransferURL(RawURL)
        Dim bReturn As Boolean = False
        If sSavePageName <> "" Then
            sPageName = sSavePageName
        End If
        If Trim(sPageName) <> "" Then
            If (wpmFileIO.VerifyFolderExists(sPath)) Then
                sPath = sPath & sPageName
                bReturn = wpmFileIO.CreateFile(sPath, sContent)
            End If
        End If
        Return bReturn
    End Function
    Public Shared Function GetTransferURL(ByVal refer As String) As String
        Dim indexc As Integer = 0
        Dim realpage As String = ""
        Dim iQuestionMark As Integer = 0
        ' Get Full relative path of request
        Dim strPath As String = GetPath(refer)
        ' Remove the QueryString for purposes of finding the right page
        iQuestionMark = InStr(strPath, "?")
        If (iQuestionMark > 0) Then
            strPath = Left(strPath, iQuestionMark - 1)
        End If
        Return strPath
    End Function
    Public Shared Function GetPath(ByVal refer As String) As String
        Dim sPath As String = ("")
        Dim indexDomain As Integer = 0
        Dim sBasePath As String = ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"))
        indexDomain = InStr(refer, sBasePath)
        If ((refer.Length - sBasePath.Length) - indexDomain > 0) Then
            sPath = Right(refer, (refer.Length - sBasePath.Length) - indexDomain)
        End If
        Return Right(Replace(sPath, "\", "/"), sPath.Length - InStrRev(sPath, "/"))
    End Function

    Public Shared Function CreateFile(ByRef path As String, ByVal content As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            Using sw As New StreamWriter(path, False)
                sw.WriteLine(content)
            End Using
            bReturn = True
        Catch ex As Exception
            wpmUTIL.AuditLog("Error with mhFIO.CreateFile - " & path, ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Shared Function DeleteFile(ByVal sPath As String) As Boolean
        Try
            My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.DeletePermanently)
        Catch
        End Try
    End Function
    Public Shared Function DeleteFile(ByRef path As String, ByRef ReturnMsg As String) As Boolean
        If (IO.File.Exists(path)) Then
            IO.File.Delete(path)
        End If
        Return True
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
            wpmUTIL.AuditLog("Error Reading Text File", path)
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
            wpmUTIL.AuditLog("Error Reading File", path)
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
    Public Shared Function CopyFile(ByVal fromPath As String, ByVal toPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            My.Computer.FileSystem.CopyFile(fromPath, toPath, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
            bReturn = True
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
    Public Shared Function MoveFile(ByVal fromPath As String, ByVal toPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            IO.Directory.Move(fromPath, toPath)
            bReturn = False
        Catch ex As Exception
            wpmUTIL.AuditLog(ex.ToString, "mhFIO.MoveFile from (" & fromPath & ") to (" & toPath & ") ")
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

    Sub RemoveFile(ByRef sFilePathAndName As String)
        If (IO.File.Exists(sFilePathAndName)) Then
            IO.File.Delete(sFilePathAndName)
        End If
    End Sub
    'Public Shared Function BuildFileListArray(ByRef objFSO As Object, ByVal strSearchFolder As String, ByVal strVirtualDirectory As String, ByVal strFolder As String, ByRef currentSlot As Integer, ByRef theFiles As Object) As Boolean
    '    Dim objFolder, objSubFolder, objSubFile
    '    Dim objThisFolder, objthisFile, strURL
    '    Dim fext As String
    '    objFolder = objFSO.GetFolder(strFolder)
    '    objSubFolder = objFolder.SubFolders
    '    objSubFile = objFolder.Files
    '    '-- if there are subfolders under it then run sub again for each one
    '    For Each objThisFolder In objSubFolder
    '        BuildFileListArray(objFSO, strSearchFolder, strVirtualDirectory, objThisFolder.Path, currentSlot, theFiles)
    '    Next
    '    '-- if there are files in the folder
    '    For Each objthisFile In objSubFile
    '        '-- get the virtual path
    '        strURL = strVirtualDirectory & Right(objthisFile.Path, (Len(objthisFile.Path) - Len(strSearchFolder)))
    '        strURL = Replace(strURL, "\", "/")
    '        fext = InStrRev(objthisFile.Name, ".")
    '        If fext < 1 Then fext = "" Else fext = Mid(objthisFile.Name, fext + 1)
    '        currentSlot = currentSlot + 1
    '        If currentSlot > UBound(theFiles) Then
    '            ReDim Preserve theFiles(currentSlot + 99)
    '        End If
    '        ' note that what we put here is an array!
    '        theFiles(currentSlot) = New Object() {objthisFile.Name, fext, objthisFile.Type, objthisFile.Size, objthisFile.DateCreated, objthisFile.DateLastModified, objthisFile.DateLastAccessed, Replace(Right(objthisFile.Path, (Len(objthisFile.Path) - Len(HttpContext.Current.Server.MapPath("/")))), "\", "/")}
    '    Next
    '    BuildFileListArray = True
    'End Function
    'Public Shared Function BubbleSortArray(ByVal REQsortBy As Integer, ByVal REQpriorSort As Integer, ByRef theArray As Object, ByRef FileCount As Integer, ByRef priorSort As Integer) As Boolean
    '    Dim sortBy As Integer
    '    Dim req As String
    '    Dim reverse As Boolean
    '    Dim mark As Boolean
    '    Dim kind As Integer = 0
    '    Dim minmax As Object
    '    Dim temp() As Object
    '    Dim minmaxSlot As Integer
    '    Dim j As Integer
    '    Dim i As Integer
    '    req = REQsortBy.ToString
    '    If Len(req) < 1 Then sortBy = 0 Else sortBy = CShort(req)
    '    req = REQpriorSort.ToString
    '    If Len(req) < 1 Then priorSort = -1 Else priorSort = CShort(req)
    '    If sortBy = priorSort Then
    '        reverse = True
    '        priorSort = -1
    '    Else
    '        reverse = False
    '        priorSort = sortBy
    '    End If
    '    If (FileCount > -1) Then
    '        If VarType(theArray(0)(sortBy)) = 8 Then
    '            If reverse Then kind = 1 Else kind = 2 ' sorting strings...
    '        Else
    '            If reverse Then kind = 3 Else kind = 4 ' non-strings (numbers, dates)
    '        End If
    '    End If
    '    For i = FileCount To 0 Step -1
    '        minmax = theArray(0)(sortBy)
    '        minmaxSlot = 0
    '        For j = 1 To i
    '            Select Case kind ' which kind of sort are we doing?
    '                ' after the "is bigger/smaller" test (as appropriate),
    '                ' mark will be true if we need to "remember" this slot...
    '                Case 1 ' string, reverse...we do case INsensitive!
    '                    mark = (StrComp(theArray(j)(sortBy), minmax, CompareMethod.Text) < 0)
    '                Case 2 ' string, forward...we do case INsensitive!
    '                    mark = (StrComp(theArray(j)(sortBy), minmax, CompareMethod.Text) > 0)
    '                Case 3 ' non-string, reverse ...
    '                    mark = (theArray(j)(sortBy) < minmax)
    '                Case 4 ' non-string, forward ...
    '                    mark = (theArray(j)(sortBy) > minmax)
    '            End Select
    '            ' so is the current slot bigger/smaller than the remembered one?
    '            If mark Then
    '                ' yep, so remember this one instead!
    '                minmax = theArray(j)(sortBy)
    '                minmaxSlot = j
    '            End If
    '        Next
    '        ' is the last slot the min (or max), as it should be?
    '        If minmaxSlot <> i Then
    '            ' nope...so do the needed swap...
    '            temp = theArray(minmaxSlot)
    '            theArray(minmaxSlot) = theArray(i)
    '            theArray(i) = temp.Clone()
    '        End If
    '    Next
    '    Return True
    'End Function
    Public Shared Function IsValidFolder(ByVal sFolderPath As String) As Boolean
        Return My.Computer.FileSystem.DirectoryExists(sFolderPath)
    End Function
    Public Shared Function FileExists(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Shared Function IsValidPath(ByVal sPath As String) As Boolean
        Dim sFolderPath As String = ("")
        Dim bFileExists As Boolean = False
        bFileExists = My.Computer.FileSystem.FileExists(sPath)
        'If Not bFileExists Then
        '  sFolderPath = My.Computer.FileSystem.GetParentPath(sPath)
        '  bFileExists = My.Computer.FileSystem.DirectoryExists(sFolderPath)
        'End If
        Return bFileExists
    End Function
    Public Shared Function GetValidAbsolutePath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = wpmConfig.wpmConfigFile & sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmUTIL.AuditLog("Invalid File - " & sPath, sCallingProcess)
        End If
        Return sPath
    End Function
    Public Shared Function GetValidPath(ByVal sRelativeFilePath As String, ByVal sCallingProcess As String) As String
        Dim sPath As String = sRelativeFilePath
        sPath = Replace(sPath, "\\", "\")
        sPath = Replace(sPath, "//", "/")
        If Not IsValidPath(sPath) Then
            sPath = ""
            wpmUTIL.AuditLog("Invalid File - " & sPath, sCallingProcess)
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
    Sub PutFile(ByRef sFilePathAndName As String, ByRef sFileContents As String)
        wpmFileIO.CreateFile(sFilePathAndName, sFileContents)
    End Sub
    Function GetFile(ByRef sFilePathAndName As String) As String
        Dim sbContents As New StringBuilder
        wpmFileIO.ReadFile(sFilePathAndName, sbContents)
        Return sbContents.ToString
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
