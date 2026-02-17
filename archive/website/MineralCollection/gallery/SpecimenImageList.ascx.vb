Imports WebProjectMechanics
Imports LINQHelper.System.Linq.Dynamic
Imports wpmMineralCollection
Imports ImageResizer

Public Class MineralCollection_controls_SpecimenImageList
    Inherits ApplicationUserControl
    Implements IApplicationList


    Dim myImage As New SpecimenImage

    Public Sub GetData(sWhere As String, OutputCSV As Boolean) Implements IApplicationList.GetData
        myImage.CollectionItemImageID = wpm_GetIntegerProperty("CollectionItemImageID", -1)
        If myImage.CollectionItemImageID = 0 Then
            ' New Image
            dtList.Visible = False
            SpecimenImageForm.Visible = True
            SpecimenImageForm.SetSpecimenImage(myImage)
        ElseIf myImage.CollectionItemImageID > 0 Then
            ' Edit Image
            dtList.Visible = False
            SpecimenImageForm.Visible = True
            Using mycon As New DataController
                For Each myConImage In (From i In mycon.CollectionItemImages Where i.CollectionItemImageID = myImage.CollectionItemImageID Select i)
                    With myConImage
                        myImage.CollectionItemID = .CollectionItemID
                        myImage.DisplayOrder = .DisplayOrder
                        myImage.ImageDS = .ImageDS
                        myImage.ImageFileNM = .ImageFileNM
                        myImage.ImageNM = .ImageNM
                        myImage.ImageType = .ImageType
                        myImage.ModifiedDT = .ModifiedDT
                        myImage.ModifiedID = .ModifiedID
                    End With
                Next
            End Using
            SpecimenImageForm.SetSpecimenImage(myImage)
        Else
            ' List All with Where clause
            dtList.Visible = True
            SpecimenImageForm.Visible = False
            Dim myObjects As New List(Of Object)
            Using myCon As New DataController()
                If String.IsNullOrEmpty(sWhere) Then
                    myObjects.AddRange((From i In myCon.vwCollectionItemImages Select i).ToList())
                Else
                    myObjects.AddRange((From i In myCon.vwCollectionItemImages.Where(sWhere) Select i).ToList())
                End If
            End Using
            Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Specimen Images",
                                                                .DetailFieldName = "ImageFileNM",
                                                                .DetailKeyName = "CollectionItemImageID",
                                                                .DetailPath = "/MineralCollection/admin.aspx?Type=Image&CollectionItemImageID={0}"}
            myTableHeader.AddLinkHeaderItem("Thumbnail", "ImageFileNM", "/MineralCollection/Admin.aspx?Type=Image&CollectionItemImageID={0}", "CollectionItemImageID", "ImageFileNM")
            myTableHeader.AddHeaderItem("ImageType", "ImageType")
            myTableHeader.AddHeaderItem("DisplayOrder", "DisplayOrder")
            myTableHeader.AddLinkHeaderItem("SpecimenNumber", "SpecimenNumber", "/MineralCollection/Admin.aspx?Type=Image&CollectionItemID={0}", "CollectionItemID", "SpecimenNumber")
            myTableHeader.AddLinkHeaderItem("Primary Mineral", "MineralNM", "/MineralCollection/Admin.aspx?Type=Image&MineralID={0}", "PrimaryMineralID", "MineralNM")
            myTableHeader.AddLinkHeaderItem("Variety", "MineralVariety", "/MineralCollection/Admin.aspx?Type=Image&MineralVariety={0}", "MineralVariety", "MineralVariety")
            myTableHeader.AddLinkHeaderItem("Purchased From", "CompanyNM", "/MineralCollection/Admin.aspx?Type=Image&CompanyID={0}", "PurchasedFromCompanyID", "CompanyNM")
            myTableHeader.AddLinkHeaderItem("Show Where Purchased", "ShowWherePurchased", "/MineralCollection/Admin.aspx?Type=Image&ShowWherePurchased={0}", "ShowWherePurchased", "ShowWherePurchased")
            myTableHeader.AddLinkHeaderItem("Mine", "MineNM", "/MineralCollection/Admin.aspx?Type=Image&MineNM={0}", "MineNM", "MineNM")
            myTableHeader.AddLinkHeaderItem("Country", "CountryNM", "/MineralCollection/Admin.aspx?Type=Image&LocationCountryID={0}", "LocationCountryID", "CountryNM")
            dtList.BuildTable(myTableHeader, myObjects)
        End If
    End Sub
    Protected Sub cmd_FileUpload_Click(sender As Object, e As EventArgs)
        If FileUpload1.HasFile Then
            Try
                Dim ImageFilePath As String = Server.MapPath("/sites/nrc/images/") & FileUpload1.FileName
                FileUpload1.SaveAs(ImageFilePath)
                litUpload.Text = String.Format("<br/>File name: {0}<br>File Size: {1} kb<br>Content type: {2}", FileUpload1.PostedFile.FileName, FileUpload1.PostedFile.ContentLength, FileUpload1.PostedFile.ContentType)
                ' Create Thumbnail
                Dim ThumbPath As String = GenerateThumbnail(ImageFilePath)
            Catch ex As Exception
                litUpload.Text = "ERROR: " & ex.Message
            End Try
        Else
            litUpload.Text = "You have not specified a file."
        End If
    End Sub
    Public Function GenerateThumbnail(original As String) As String
        'Dim settings As New ImageResizer.Instructions With {
        '    .Width = 200,
        '    .Height = 200,
        '    .Format = "png"
        '}
        'Dim ThumbPath As String = String.Empty
        'If FileUpload1.PostedFile.ContentType.Contains("jpeg") Then
        '    ThumbPath = FileUpload1.PostedFile.FileName.Replace(".jpg", "").Replace(".jpeg", "")
        'End If
        'ThumbPath = HttpContext.Current.Server.MapPath("/sites/nrc/thumbnails/") & ThumbPath
        'Return ImageBuilder.Current.Build(New ImageJob(original, ThumbPath, settings, False, True)).FinalPath
        Return original
    End Function

    Public Sub GetData1(sFilterList As SQLFilterList, OutputCSV As Boolean) Implements IApplicationList.GetData
        GetData(sFilterList.GetLINQWhere,OutputCSV)
    End Sub
End Class

