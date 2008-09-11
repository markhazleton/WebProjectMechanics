<%@ Page Language="vb" %>
<%@  Import Namespace="System.IO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<HEAD>
<title>Multiple File Upload</title>
</HEAD>
<BODY>
     
<ASP:PANEL id="MultipleFileUploadForm"     runat="server"  visible="true">
   <FORM id="Form1" method="post" encType="multipart/form-data" runat="server">
    <H1>ASP.NET Multiple File Upload Example</H1>
    <P>Select the Files to Upload to the Server:
       <BR>
      <INPUT id="File1" type="file" name="File1" runat="server">
    </P>
    <P>
    <INPUT id="File2"  type="file" name="File2" runat="server"></P>
        <P>
       <INPUT id="Submit1" type="submit" value="Upload Files" name="Submit1" runat="server" OnServerClick="UploadMultipleFiles_Clicked">
    </P>
   </FORM>
</ASP:PANEL>
<ASP:LABEL id="ResultMsg" runat="server" Visible="False" ForeColor="#ff0033"></ASP:LABEL>

</BODY>
</HTML>

<script language="vb" runat="server">
    Sub UploadMultipleFiles_Clicked(ByVal Sender As Object, ByVal e As EventArgs)
        'Variable to hold the result
        Dim m_strResultMessage As String = ""
        'Variable to hold the FileName
        Dim m_strFileName As String
        'Variable FolderName where the files  will be saved
        Dim m_strFolderName As String = "C:\TempMultipleFiles\"
        'Variable to hold the File
        Dim m_objFile As HttpPostedFile
        'Variable used in the Loop
        Dim i As Integer
        Try
            'Loop Through the Files
            For i = 0 To Request.Files.Count - 1
                'Get the HttpPostedFile
                m_objFile = Request.Files(i)
                'Check that the File exists has a name and is not empty
                If Not (m_objFile Is Nothing Or m_objFile.FileName = "" Or m_objFile.ContentLength < 1) Then

                    'Get the name of the file
                    m_strFileName = m_objFile.FileName
                    m_strFileName = Path.GetFileName(m_strFileName)

                    'Creates the folder if it does not exists
                    If (Not Directory.Exists(m_strFolderName)) Then
                        Directory.CreateDirectory(m_strFolderName)
                    End If
                    'Save each uploaded file
                    m_objFile.SaveAs(m_strFolderName & m_strFileName)
                    'Assign the File Name and File Type to Result
                    m_strResultMessage = m_strResultMessage & "Uploaded File: " & m_objFile.FileName & " of type " & m_objFile.ContentType & " <br> "
                    'Hide the Multiple Form Upload Panel
                    MultipleFileUploadForm.Visible = False
                End If
            Next
            'If no files where selected provide a user friendly message
            If m_strResultMessage = "" Then
                m_strResultMessage = "Select atleast one file to upload."
            End If
        Catch errorVariable As Exception
            'Trap the exception
            m_strResultMessage = errorVariable.ToString()
        End Try
        'Unhide the Result Label 
        ResultMsg.Visible = True
        'Assign the Result to ResultMsg Label Text
        ResultMsg.Text = m_strResultMessage
    End Sub
</script>

