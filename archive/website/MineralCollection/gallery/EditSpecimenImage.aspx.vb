Imports WebProjectMechanics
Imports System
Imports wpmMineralCollection

Public Class Gallery_EditSpecimenImage
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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
                ddlCollectionItemID.DataSource = (From i In mycon.vwCollectionItems Where i.CollectionID = ddlCollection.SelectedValue Order By i.SpecimenNumber Ascending Select i.CollectionItemID, CollectionItemNM = String.Format("{0} - {1} ({2})", i.SpecimenNumber, i.Nickname, i.MineralNM)).ToList()
                ddlCollectionItemID.DataTextField = "CollectionItemNM"
                ddlCollectionItemID.DataValueField = "CollectionItemID"
                ddlCollectionItemID.DataBind()

                If myImage.CollectionItemImageID > 0 Then
                    myImage = (From i In mycon.vwCollectionItemImages Where i.CollectionItemImageID = myImage.CollectionItemImageID Select New SpecimenImage With {.CollectionItemID = i.CollectionItemID, .CollectionItemImageID = i.CollectionItemImageID, .DisplayOrder = i.DisplayOrder, .ImageDS = i.ImageDS, .ImageFileNM = i.ImageFileNM, .ImageNM = i.ImageNM, .ImageType = i.ImageType, .MineralNM = i.MineralNM, .ModifiedDT = i.ModifiedDT, .ModifiedID = i.ModifiedID, .NickName = i.Nickname, .PrimaryMineralID = i.PrimaryMineralID, .SpecimenDS = i.Description, .SpecimenNotes = i.SpecimenNotes, .SpecimenNumber = i.SpecimenNumber}).Single
                ElseIf myImage.ImageFileNM <> String.Empty Then
                    Try
                        myImage = (From i In mycon.vwCollectionItemImages Where i.ImageFileNM = myImage.ImageFileNM Select New SpecimenImage With {.CollectionItemID = i.CollectionItemID, .CollectionItemImageID = i.CollectionItemImageID, .DisplayOrder = i.DisplayOrder, .ImageDS = i.ImageDS, .ImageFileNM = i.ImageFileNM, .ImageNM = i.ImageNM, .ImageType = i.ImageType, .MineralNM = i.MineralNM, .ModifiedDT = i.ModifiedDT, .ModifiedID = i.ModifiedID, .NickName = i.Nickname, .PrimaryMineralID = i.PrimaryMineralID, .SpecimenDS = i.Description, .SpecimenNotes = i.SpecimenNotes, .SpecimenNumber = i.SpecimenNumber}).Single
                    Catch ex As Exception
                        ' Image Not found set 
                        hfCollectionItemImageID.Value = 0
                    End Try
                Else
                    'myImage.CollectionItemImageID = wpm_GetIntegerProperty("CollectionItemImageID", -1)
                    'myImage.ImageFileNM = wpm_GetProperty("ImageFileNM", String.Empty)
                End If
                If myImage.CollectionItemImageID > 0 Then
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
        If Not IsNumeric(ddlCollectionItemID.SelectedValue) Then
            sbError.Append("Error: You must select a Specimen before you can save your changes.")
        Else

            If CInt(hfCollectionItemImageID.Value) < 1 Then
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
                    If ex.Message.Contains("UK_CollectionItemImageOrder") Then
                        sbError.Append("Error: The Display Order number is already in use, please select another number. ")
                    Else
                        sbError.AppendFormat("Error: {0}", ex.Message)
                    End If
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
                    If ex.Message.Contains("UK_CollectionItemImageOrder") Then
                        sbError.Append("Error: The Display Order number is already in use, please select another number. ")
                    Else
                        sbError.AppendFormat("Error: {0}", ex.Message)
                    End If
                End Try
            End If
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
            Try
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCollectionImageItem = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = hfCollectionItemImageID.Value).Single
                    myCon.CollectionItemImages.DeleteOnSubmit(myCollectionImageItem)
                    myCon.SubmitChanges()
                End Using
                Response.Redirect("/MineralCollection/gallery/ListImages.aspx?Filter=NotAssigned")
            Catch ex As Exception
                pnlAlert.Visible = True
                pnlAlert.Controls.Add(New Literal With {.Text = String.Format("<div class=""alert alert-danger"" role=""alert""><strong>An Error Has Occured</strong><br>{0}</div>", ex.ToString)})
            End Try
        Else
            If String.IsNullOrEmpty(myFile) Then
                ApplicationLogging.ErrorLog("Error - a valid file was not provided with DELETE request", "MineralCollection_ListImages.Page_Load-Delete")
                pnlAlert.Visible = True
                pnlAlert.Controls.Add(New Literal With {.Text = "<div class=""alert alert-danger"" role=""alert""><strong>Error - a valid file was not provided with DELETE request</strong></div>"})
            Else
                ' Remove Thumbnail
                wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, "thumbnails")), myFile.ToLower().Replace(".jpg", ".png"))
                If wpm_DeleteFile(Server.MapPath(String.Format("/{0}/{1}/", wpm_SiteGallery, STR_ImageFolder)), myFile) Then
                    Response.Redirect("/MineralCollection/gallery/ListImages.aspx?Filter=NotAssigned")
                Else
                    ApplicationLogging.ErrorLog("Error - unable to DELETE ", "MineralCollection_ListImages.Page_Load-Delete")
                    pnlAlert.Visible = True
                    pnlAlert.Controls.Add(New Literal With {.Text = "<div class=""alert alert-danger"" role=""alert""><strong>Error - unable to DELETE File</strong></div>"})
                End If
            End If
        End If
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        Response.Redirect("/MineralCollection/gallery/ListImages.aspx")
    End Sub
End Class
