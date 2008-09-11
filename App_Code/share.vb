Imports System
Imports System.IO
Imports System.Text
Imports System.Web
Imports EW.Web
Imports EW.Data
Namespace PMGEN
	Public class Share
		Public Sub New()
		End Sub

		''' <summary>
		''' Gets the project Name
		''' </summary>
		''' <returns>a string of the project name.</returns>

		Public Shared ReadOnly Property ProjectName As String
			Get 
				Return "PMGEN"
			End Get
		End Property

		''' <summary>
		''' Gets the randomkey
		''' </summary>
		''' <returns>a string of the randomkey.</returns>		

		Public Shared ReadOnly Property RandomKey As String
			Get 
				Return "qOFD9A#vukY5&1C4"
			End Get
		End Property

		''' <summary>
		''' Gets the sender email address
		''' </summary>
		''' <returns>a string of the sender email address.</returns>

		Public Shared ReadOnly Property SenderEmail As String
			Get 
				Return "mark.hazleton@projectmechanics.com"
			End Get
		End Property

		''' <summary>
		''' Gets the recipient email address
		''' </summary>
		''' <returns>a string of the recipient email address.</returns>

		Public Shared ReadOnly Property RecipientEmail As String
			Get 
				Return "mark.hazleton@projectmechanics.com"
			End Get
		End Property

		''' <summary>
		''' Gets the SMPT server
		''' </summary>
		''' <returns>a string of the SMPT server.</returns>		

		Public Shared ReadOnly Property SmptServer As String
			Get
				Return "localhost"
			End Get
		End Property

		''' <summary>
		''' Gets the SMPT server port
		''' </summary>
		''' <returns>an integer of the SMPT server port.</returns>

		Public Shared ReadOnly Property SmtpServerPort As Integer
			Get 
				Return 25
			End Get
		End Property

		''' <summary>
		''' Gets the audit trail path relative to application path
		''' </summary>
		''' <returns>a string of audit trail path.</returns>	

		Public Shared ReadOnly Property AuditPath As String
			Get 
				Return "~/access_db/log/"
			End Get
		End Property

		''' <summary>
		''' Gets the value separator string
		''' </summary>
		''' <returns>a string of value separator.</returns>	

		Public Shared Function ValueSeparator(ByVal rowcnt As Integer) As String
			Return ", "
		End Function

		''' <summary>
		''' Upload Class
		''' </summary>

		Public Class Upload

			''' <summary>
			''' Gets the maximum upload file size
			''' </summary>
			''' <returns>the maximum size of upload file.</returns>

			Public Shared Function MaxUploadSize() As Integer
				Return 2000000
			End Function

			''' <summary>
			''' Gets the allow file type
			''' </summary>
			''' <returns>a string of the allow file type.</returns>

			Public Shared Function GetAllowFileTypes() As String
				Return "gif,jpg,jpeg,bmp,png,doc,xls,pdf,zip"
			End Function

			''' <summary>
			''' Gets the prohibited file type
			''' </summary>
			''' <returns>a string of the prohibited file type.</returns>

			Public Shared Function GetProhibitedFileTypes() As String
				Return "exe,dll,aspx,asp,php,cfm"
			End Function

			''' <summary>
			''' Checks whether the file type is valid
			''' </summary>
			''' <returns>True if the file type is valid; otherwise, false.</returns>

			Public Shared Function ValidFileType(ByVal filename As String) As Boolean
				Dim strFileType As String = IIf((filename.LastIndexOf(".") > 0), filename.Substring(filename.LastIndexOf(".")+1), String.Empty)
				Dim strfileTypes As String = GetProhibitedFileTypes() ' prohibited file type list, comma separated
				Dim fileList() As String = strfileTypes.Split(","C)
				For Each s As String In fileList
					If (String.Compare(s, strFileType, True) = 0) Then
						Return false
					End If
				Next
                Dim strAllowFileTypes As String = GetAllowFileTypes() ' allowed file type list, comma separated
                fileList = strAllowFileTypes.Split(","C)
                For Each s As String In fileList
					If (String.Compare(s, strFileType, True) = 0) Then
						Return True
					End If
				Next
                Return False
			End Function

			''' <summary>
			''' Gets custom file name 
			''' </summary>
			''' <returns>a string of file name.</returns>

			Public Shared Function GetFileName(ByVal filename As String) As String
				Dim strOutFileName As String = filename

				' Add your logic here
				Return strOutFileName
			End Function

			''' <summary>
			''' Gets unique file name that unique in specific folder name
			''' </summary>
			''' <returns>a string of unique file name.</returns>

			Public Shared Function UniqueFileName(ByVal folderName As String, ByVal filename As String) As String
				Dim strFilename As String = System.IO.Path.GetFileNameWithoutExtension(filename)
				Dim strExt As String = System.IO.Path.GetExtension(filename)

				' get file extension (eg. ".JPG")
				If Not System.IO.Directory.Exists(folderName) Then

					' if the folder is not exist, then create the folder
					System.IO.Directory.CreateDirectory(folderName)

					' create folder
				End If
				Dim index As Integer = 0
				Dim strSuffix As String = ""

				' Check to see if filename exists
				While System.IO.File.Exists(folderName & strFilename & strSuffix & strExt)
					index += 1
					strSuffix = "(" & index & ")"
				End While

				' Return unique file name
				Return (strFilename & strSuffix & strExt)
            End Function

			''' <summary>
			''' Gets upload folder name of a specific field name in a specific table
			''' </summary>
			''' <returns>a string of upload folder name for that field.</returns>

            Public Shared Function UploadFolder(ByVal tableName As String, ByVal fieldName As String) As String
                Dim strFolder As String = String.Empty
                Select Case (tableName)
                End Select
                If (strFolder = string.Empty) Then
					Return "~/UserFiles/"
				Else
					Return strFolder
				End If
            End Function
		End Class

		''' <summary>
		''' Audit Trail Class
		''' </summary>

		Public Class AuditTrail

			''' <summary>
			''' Write audit trail
			''' </summary>

			Public Shared Sub WriteAuditTrail(ByVal folderName As String, ByVal pfx As String, ByVal curDate As String, ByVal curTime As String, ByVal id As String, ByVal user As String, ByVal action As String, ByVal table As String, ByVal fieldName As String, ByVal keyValue As String, ByVal oldValue As String, ByVal newValue As String)
				Dim strUserwrk As String
				Dim strMsg As String
				Dim strFn As String
				Dim strHeader As String
				Dim bWriteHeader As Boolean
				Dim fs As System.IO.FileStream
				strUserwrk = user
				If (strUserwrk = string.Empty) Then
					strUserwrk = "-1"
				End If

				' assume Administrator if no user
				strHeader = String.Concat("date", ""& vbTab, "time", ""& vbTab, "id", ""& vbTab, "user", ""& vbTab, "action", ""& vbTab, "table", ""& vbTab, "field", ""& vbTab, "key value", ""& vbTab, "old value", ""& vbTab, "new value")
				strMsg = String.Concat(curDate, ""& vbTab, curTime, ""& vbTab, id, ""& vbTab, strUserwrk, ""& vbTab, action, ""& vbTab, table, ""& vbTab, fieldName, ""& vbTab, keyValue, ""& vbTab, oldValue, ""& vbTab, newValue)
				If Not System.IO.Directory.Exists(folderName) Then
					System.IO.Directory.CreateDirectory(folderName)

					' Create Folder
				End If
				strFn = String.Concat(folderName, pfx, "_", DateTime.Today.ToString("yyyyMMdd"), ".txt")

				' form log file name
				bWriteHeader = Not System.IO.File.Exists(strFn)
				fs = New System.IO.FileStream(strFn, System.IO.FileMode.Append, System.IO.FileAccess.Write)
				Dim fsw As System.IO.StreamWriter = New System.IO.StreamWriter(fs)
				If bWriteHeader Then

					' if the log file is just created, then add a header line to the log file
					fsw.WriteLine(strHeader)

					' Write header line
				End If
				fsw.WriteLine(strMsg)

				' output message
				fsw.Close()
			End Sub
        End Class
	End Class
End Namespace
