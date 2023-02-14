Imports WebProjectMechanics
Imports wpmMineralCollection
            Imports System.IO

Public Class MineralCollection_controls_SpecimenImageForm
    Inherits CollectionUserControl
    Implements IMineralCollectionImageForm

    Sub Foo



    End Sub


    Public Sub SetSpecimenImage(ByVal mySpecimenImage As SpecimenImage) Implements IMineralCollectionImageForm.SetSpecimenImage
        With mySpecimenImage
            hfCollectionItemImageID.Value = .CollectionItemImageID
            hfModifiedDT.Value = .ModifiedDT
            hfModifiedID.Value = .ModifiedID
            ddDisplayOrder.SelectedValue = .DisplayOrder
            tbImageNM.Text = .ImageNM
            tbImageDescription.Text = .ImageDS
            tbImageFileNM.Text = .ImageFileNM
            hfCollectionItemImageID.Value = .CollectionItemID
            ddlImageType.SelectedValue = .ImageType
            imgSpecimen.ImageUrl = Display.GetImageURL(.ImageFileNM)
        End With
    End Sub
    Public Function GetSpecimenImage() As SpecimenImage Implements IMineralCollectionImageForm.GetSpecimenImage
        Dim myImage As New SpecimenImage
        With myImage
            .CollectionItemImageID = wpm_GetDBInteger(hfCollectionItemImageID.Value)
            .ModifiedDT = wpm_GetDBDate(hfModifiedDT.Value)
            .ModifiedID = wpm_GetDBInteger(hfModifiedID.Value)
            .DisplayOrder = wpm_GetDBInteger(ddDisplayOrder.SelectedValue)
            .ImageNM = wpm_GetDBString(tbImageNM.Text)
            .ImageDS = wpm_GetDBString(tbImageDescription.Text)
            .ImageFileNM = wpm_GetDBString(tbImageFileNM.Text)
            .CollectionItemID = wpm_GetDBInteger(ddlCollectionItemID.SelectedValue)
            .ImageType = wpm_GetDBString(ddlImageType.SelectedValue)
        End With
        Return myImage
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim myImage As New SpecimenImage
            myImage.CollectionItemImageID = wpm_GetIntegerProperty("CollectionItemImageID", -1)
            myImage.ImageFileNM = wpm_GetProperty("ImageFileNM", String.Empty)


            Using mycon As New wpmMineralCollection.DataController()
                ddlCollection.DataSource = (From i In mycon.Collections Order By i.CollectionNM).ToList()
                ddlCollection.DataTextField = "CollectionNM"
                ddlCollection.DataValueField = "CollectionID"
                ddlCollection.DataBind()
                ddlCollection.SelectedIndex = 0

                ddlCollectionItemID.AppendDataBoundItems = True
                ddlCollectionItemID.Items.Add(New ListItem With {.Value = String.Empty, .Text = "No Specimen Selected"})
                ddlCollectionItemID.DataSource = (From i In mycon.CollectionItems Where i.CollectionID = ddlCollection.SelectedValue Order By i.SpecimenNumber Ascending).ToList()
                ddlCollectionItemID.DataTextField = "SpecimenNumber"
                ddlCollectionItemID.DataValueField = "CollectionItemID"
                ddlCollectionItemID.DataBind()

                If myImage.CollectionItemImageID > 0 Then
                    myImage = (From i In mycon.vwCollectionItemImages Where i.CollectionItemImageID = myImage.CollectionItemImageID Select New SpecimenImage With {.CollectionItemID = i.CollectionItemID, .CollectionItemImageID = i.CollectionItemImageID, .DisplayOrder = i.DisplayOrder, .ImageDS = i.ImageDS, .ImageFileNM = i.ImageFileNM, .ImageNM = i.ImageNM, .ImageType = i.ImageType, .MineralNM = i.MineralNM, .ModifiedDT = i.ModifiedDT, .ModifiedID = i.ModifiedID, .NickName = i.Nickname, .PrimaryMineralID = i.PrimaryMineralID, .SpecimenDS = i.Description, .SpecimenNotes = i.SpecimenNotes, .SpecimenNumber = i.SpecimenNumber}).Single
                ElseIf myImage.ImageFileNM <> String.Empty Then
                    myImage = (From i In mycon.vwCollectionItemImages Where i.ImageFileNM = myImage.ImageFileNM Select New SpecimenImage With {.CollectionItemID = i.CollectionItemID, .CollectionItemImageID = i.CollectionItemImageID, .DisplayOrder = i.DisplayOrder, .ImageDS = i.ImageDS, .ImageFileNM = i.ImageFileNM, .ImageNM = i.ImageNM, .ImageType = i.ImageType, .MineralNM = i.MineralNM, .ModifiedDT = i.ModifiedDT, .ModifiedID = i.ModifiedID, .NickName = i.Nickname, .PrimaryMineralID = i.PrimaryMineralID, .SpecimenDS = i.Description, .SpecimenNotes = i.SpecimenNotes, .SpecimenNumber = i.SpecimenNumber}).Single
                Else
                    myImage.CollectionItemImageID = wpm_GetIntegerProperty("CollectionItemImageID", -1)
                    myImage.ImageFileNM = wpm_GetProperty("ImageFileNM", String.Empty)
                End If
                If myImage.CollectionItemImageID <> 0 Then
                    With myImage
                        hfCollectionItemImageID.Value = .CollectionItemImageID
                        tbImageNM.Text = .ImageNM
                        tbImageDescription.Text = .ImageDS
                        ddDisplayOrder.SelectedValue = .DisplayOrder
                        ddlImageType.SelectedValue = .ImageType
                        ddlCollectionItemID.SelectedValue = .CollectionItemID
                        tbImageFileNM.Text = .ImageFileNM
                    End With
                Else
                    hfCollectionItemImageID.Value = 0
                    tbImageFileNM.Text = myImage.ImageFileNM
                    tbImageNM.Text = myImage.ImageFileNM
                    tbImageDescription.Text = myImage.ImageFileNM
                End If
                imgSpecimen.ImageUrl = wpmMineralCollection.Display.GetImageURL(myImage.ImageFileNM)
                tbImageFileNM.Enabled = False
            End Using
        End If

    End Sub

    Private Function GetCollectionItem(ByVal sImageFileName As String) As wpmMineralCollection.CollectionItemImage
        Dim myItem As New wpmMineralCollection.CollectionItemImage
        Try
            Using myCon As New wpmMineralCollection.DataController()
                myItem = ((From i In myCon.CollectionItemImages Where i.ImageFileNM = sImageFileName).Single)
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("BrowseImages.GetCollectionItem", ex.ToString)
        End Try
        Return myItem
    End Function
    Protected Sub cmd_SaveChanges_Click(sender As Object, e As EventArgs)
        Dim sbError As New StringBuilder

        If CInt(hfCollectionItemImageID.Value) = 0 Then
            Try
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCollectionImageItem As New wpmMineralCollection.CollectionItemImage
                    With myCollectionImageItem
                        .CollectionItemID = ddlCollectionItemID.SelectedValue
                        .ImageFileNM = tbImageFileNM.Text
                        .ImageNM = tbImageNM.Text
                        .ImageDS = tbImageDescription.Text
                        .DisplayOrder = ddDisplayOrder.SelectedValue
                        .ImageType = ddlImageType.SelectedValue
                        .ModifiedDT = Now()
                        .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
                    End With
                    myCon.CollectionItemImages.InsertOnSubmit(myCollectionImageItem)
                    myCon.SubmitChanges()
                End Using
            Catch ex As Exception
                sbError.Append(String.Format("Error: {0}", ex.Message))
            End Try
        Else
            Try
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCollectionImageItem = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = hfCollectionItemImageID.Value).Single
                    With myCollectionImageItem
                        .CollectionItemID = ddlCollectionItemID.SelectedValue
                        .ImageNM = tbImageNM.Text
                        .ImageDS = tbImageDescription.Text
                        .DisplayOrder = ddDisplayOrder.SelectedValue
                        .ImageType = ddlImageType.SelectedValue
                        .ModifiedDT = Now()
                        .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
                    End With
                    myCon.SubmitChanges()
                End Using
            Catch ex As Exception
                sbError.Append(String.Format("Error: {0}", ex.Message))
            End Try
        End If
        If sbError.Length > 0 Then
            pnlAlert.Visible = True
            pnlAlert.Controls.Add(New Literal With {.Text = String.Format("<div class=""alert alert-danger"" role=""alert""><strong>An Error Has Occured</strong><br>{0}</div>", sbError.ToString)})
        Else
            Response.Redirect("/MineralCollection/gallery/ListImages.aspx")

        End If
    End Sub
    Protected Sub cmd_DeleteFile_Click(sender As Object, e As EventArgs)
        Dim myFile = tbImageFileNM.Text
        If CInt(hfCollectionItemImageID.Value) > 0 Then
            Using myCon As New wpmMineralCollection.DataController()
                Dim myCollectionImageItem = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = hfCollectionItemImageID.Value).Single
                myCon.CollectionItemImages.DeleteOnSubmit(myCollectionImageItem)
                myCon.SubmitChanges()
            End Using
        End If
        If String.IsNullOrEmpty(myFile) Then
            ApplicationLogging.ErrorLog("Error - a valid file was not provided with DELETE request", "MineralCollection_ListImages.Page_Load-Delete")
        Else
            ' Remove Thumbnail
            wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, "thumbnails")), myFile.Replace(".jpg", ".png"))

            If wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), myFile) Then
                Response.Redirect("/MineralCollection/gallery/ListImages.aspx")
            Else
                ApplicationLogging.ErrorLog("Error - unable to DELETE ", "MineralCollection_ListImages.Page_Load-Delete")
            End If
        End If
        Response.Redirect("/MineralCollection/gallery/ListImages.aspx")
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("/MineralCollection/gallery/ListImages.aspx")
    End Sub



End Class
