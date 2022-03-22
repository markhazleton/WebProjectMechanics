Imports WebProjectMechanics
Imports System.IO
Imports System
Imports System.Text
Imports ImageResizer

Public Class admin_ListImages
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If wpm_GetProperty("action", String.Empty) = "delete" Then
            Dim myFile = wpm_GetProperty("ImageFileName", String.Empty)
            If String.IsNullOrEmpty(myFile) Then
                ApplicationLogging.ErrorLog("Error - a valid file was not provided with DELETE request", "Admin_ListImages.Page_Load-Delete")
            Else
                If wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), myFile) Then
                    Response.Redirect("/admin/gallery/ListImages.aspx")
                Else
                    ApplicationLogging.ErrorLog("Error - unable to DELETE ", "MineralCollection_ListImages.Page_Load-Delete")
                End If
            End If
        End If
        ProcessSiteGallery()
    End Sub

    Public Function ProcessSiteGallery() As Boolean
        ProcessGalleryFiles(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), "NotAssigned")
        ProcessImageFolder()
        Return True
    End Function

    Public Function ProcessImageFolder() As Boolean
        Dim myPath As String
        Dim myRelPath As String
        If Not FileProcessing.VerifyFolderExists(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder))) Then
            FileProcessing.CreateFolder(Server.MapPath(String.Format("{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)))
        End If
        Dim myFolders() As String = Directory.GetDirectories(Server.MapPath(wpm_SiteGallery.ToString & String.Format("/{0}/", STR_ImageFolder)))
        For y As Integer = 0 To myFolders.Length - 1
            myPath = myFolders(y).ToString
            myRelPath = Replace(myFolders(y).ToString, Server.MapPath(Session("SiteGallery") & String.Format("{0}/", STR_ImageFolder)), "")
            ProcessGalleryFiles(myPath, "NotAssigned")
        Next
        Return True
    End Function
    Public Function ProcessGalleryFiles(ByVal FolderPath As String, ByVal reqFilter As String) As Boolean
        Dim myFilter As String = wpm_GetProperty("Filter", reqFilter)

        Dim myReturn As Boolean = True
        Dim myImageFiles As New List(Of Object)



        ' GetCollectionImages(2)
        Try

            Dim dbImages = (From o In curCompany.SiteImageList Select o.ImageFileName.Replace("/image/", String.Empty)).ToList()

            Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath(curCompany.SiteImageFolder))
            Dim myFiles = (From i In myDir.GetFiles("*.jpg")).ToArray

            Dim myNewCol As New List(Of LocationImage)
            Dim bMatch As Boolean = False


            For Each myFile In myFiles
                bMatch = False
                For Each myMatch In (From i In dbImages Where i.Replace("image/", String.Empty) = myFile.Name)
                    bMatch = True
                Next
                If Not bMatch Then
                    myNewCol.Add(New LocationImage With {.ImageFileName = myFile.Name, .ImageName = myFile.Name})
                End If
            Next

            Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") Where Not dbImages.Contains(i.Name.ToLower()))

            Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Images (Filter: <a href='/admin/Gallery/ListImages.aspx?Filter=ALL'>All Images</a> | <a href='/admin/Gallery/ListImages.aspx?Filter=Assigned'>Assigned</a> | <a href='/admin/Gallery/ListImages.aspx?Filter=NotAssigned'>Not Assigned</a>)",
                                                                .DetailFieldName = "ImageFileName",
                                                                .DetailKeyName = "ImageFileName",
                                                                .DetailPath = "/admin/Gallery/EditSpecimenImage.aspx?ImageFileNM={0}"}
            myTableHeader.AddHeaderThumbnailItem("Thumbnail",
                                            "ImageFileName",
                                            "/admin/Gallery/EditImage.aspx?ImageFileName={0}",
                                            "ImageFileName",
                                            "ImageFileName",
                                             curCompany.SiteImageFolder)

            myTableHeader.AddHeaderItem("ImageType", "ImageType")
            myTableHeader.AddHeaderItem("DisplayOrder", "DisplayOrder")
            myTableHeader.AddHeaderItem("Image Name", "ImageName")
            myTableHeader.AddHeaderItem("Image Description", "ImageDS")
            '  myTableHeader.AddLinkHeaderItem("Primary Mineral", "MineralNM", "/admin/maint/default.aspx?Type=Image&ImageID={0}", "PrimaryMineralID", "MineralNM")
            dtList.BuildTable(myTableHeader, myNewCol)
        Catch ex As Exception
            ApplicationLogging.AuditLog(ex.ToString, "ProcessGalleryFiles - ListImages.aspx")
            myReturn = False
        End Try
        Return myReturn
    End Function
End Class
