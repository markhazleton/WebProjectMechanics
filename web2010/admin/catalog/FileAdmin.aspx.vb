Imports WebProjectMechanics
Imports System.IO
Imports System
Imports System.Text

Public Class Admin_Catalog_FileAdmin
    Inherits ApplicationPage
    Private Const STR_ImageFolder As String = "image"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder("<hr/>")
        ProcessSiteGallery(mySB)
        myContent.Text = mySB.ToString
    End Sub

    Public Function ProcessSiteGallery(ByRef mysb As StringBuilder) As Boolean
        ProcessGalleryFiles(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), mysb)
        ProcessImageFolder(mysb)
        Return True
    End Function

    Public Function ProcessImageFolder(ByRef mySB As StringBuilder) As Boolean
        Dim myPath As String
        Dim myRelPath As String

        If Not FileProcessing.VerifyFolderExists(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder))) Then
            FileProcessing.CreateFolder(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)))
        End If

        Dim myFolders() As String = Directory.GetDirectories(Server.MapPath(wpm_SiteGallery.ToString & String.Format("/{0}/", STR_ImageFolder)))
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & String.Format("{0}/", STR_ImageFolder)), "")

            ProcessGalleryFiles(myPath, mySB)
        Next
        Return True
    End Function
    Public Function ProcessGalleryFiles(ByVal FolderPath As String, ByRef mySB As StringBuilder) As Boolean
        Dim myFileName As String
        Dim myImage As LocationImage
        Dim myReturn As Boolean = True
        Try
            mySB.Append(String.Format("<h1>{0}</h1><ul>", FolderPath))
            For Each sFileName As String In My.Computer.FileSystem.GetFiles(FolderPath)
                myFileName = Replace(Replace(sFileName, Server.MapPath(wpm_SiteGallery.ToString), ""), "\", "/")
                myImage = masterPage.myCompany.SiteImageList.FindImageByImageFileName(myFileName)
                If myImage Is Nothing Then
                    mySB.Append("<li style=""color:red;"">" & myFileName)
                    mySB.Append("</li>")
                Else
                    mySB.Append("<li>" & myFileName)
                    mySB.Append(String.Format("(<a href=""/admin/maint/default.aspx?Type=Image&ImageID={0}"">Edit</a>,<a href=""/admin/maint/default.aspx?Type=Image&ImageID={0}&action=delete"">Delete</a> ) ", myImage.ImageID))
                    mySB.Append("</li>")
                End If
            Next
            mySB.Append("</ul>")
        Catch ex As Exception
            ApplicationLogging.AuditLog(ex.ToString, "ProcessGalleryFiles - FileAdmin.aspx")
            myReturn = False
        End Try
        Return myReturn
    End Function

End Class
