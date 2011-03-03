Imports WebProjectMechanics
Imports System.IO

Partial Class wpm_file_admin
    Inherits wpmPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pageActiveSite = New wpmActiveSite(Session)
        Dim mySB As New StringBuilder("<hr/>")
        ProcessSiteGallery(mySB)
        myContent.Text = mySB.ToString
    End Sub

    Public Function ProcessSiteGallery(ByRef mysb As StringBuilder) As Boolean
        ProcessGalleryFiles(Server.MapPath(pageActiveSite.SiteGallery.ToString & "/image/"), mysb)
        ProcessImageFolder(mysb)
        Return True
    End Function

    Public Function ProcessImageFolder(ByRef mySB As StringBuilder) As Boolean
        Dim myPath As String
        Dim myRelPath As String

        If Not wpmFileProcessing.VerifyFolderExists(Server.MapPath(pageActiveSite.SiteGallery.ToString & "/image/")) Then
            wpmFileProcessing.CreateFolder(Server.MapPath(pageActiveSite.SiteGallery.ToString & "/image/"))
        End If

        Dim myFolders() As String = Directory.GetDirectories(Server.MapPath(pageActiveSite.SiteGallery.ToString & "/image/"))
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & "image/"), "")
            ProcessGalleryFiles(myPath, mySB)
        Next
        Return True
    End Function
    Public Function ProcessGalleryFiles(ByVal FolderPath As String, ByRef mySB As StringBuilder) As Boolean
        Dim myFileName As String
        Dim myImage As wpmImage
        Dim myReturn As Boolean = True
        Try
            mySB.Append("<h1>" & FolderPath & "</h1><ul>")
            For Each sFileName As String In My.Computer.FileSystem.GetFiles(FolderPath)
                myFileName = Replace(Replace(sFileName, Server.MapPath(pageActiveSite.SiteGallery.ToString), ""), "\", "/")
                myImage = pageActiveSite.SiteImageList.FindImageByImageFileName(myFileName)
                If myImage Is Nothing Then
                    mySB.Append("<li style=""color:red;"">" & myFileName)
                    mySB.Append("</li>")
                Else
                    mySB.Append("<li>" & myFileName)
                    mySB.Append("(<a href=""/wpmgen/Image_Edit.aspx?ImageID=" & myImage.ImageID & """>Edit</a>,<a href=""/wpmgen/image_delete.aspx?ImageID=" & myImage.ImageID & """>Delete</a> ) ")
                    mySB.Append("</li>")
                End If
            Next
            mySB.Append("</ul>")
        Catch ex As Exception
            wpmLogging.AuditLog(ex.ToString, "ProcessGalleryFiles - FileAdmin.aspx")
            myReturn = False
        End Try
        Return myReturn
    End Function

End Class
