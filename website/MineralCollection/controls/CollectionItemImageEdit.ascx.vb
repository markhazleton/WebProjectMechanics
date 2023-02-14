Imports WebProjectMechanics
Imports System.IO

Public Class MineralCollection_CollectionItemImageEdit
    Inherits ApplicationUserControl
    Public Property reqCollectionItemImageID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqCollectionItemImageID = GetIntegerProperty("CollectionItemImageID", -1)
        hfCollectionItemID.Value = GetIntegerProperty("CollectionItemID", 2)
        hfCollectionItemImageID.Value = GetIntegerProperty("CollectionItemImageID", -1)
        If Not IsPostBack Then
            If reqCollectionItemImageID > 0 Then
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                ddlImage.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCollectionItemImages = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = reqCollectionItemImageID).ToList()
                    Dim myCollectionItemImage As New wpmMineralCollection.CollectionItemImage
                    If myCollectionItemImages.Count > 0 Then
                        myCollectionItemImage = myCollectionItemImages(0)
                    Else
                        Throw New Exception("CollectionItemImageID is not found")
                    End If
                    With myCollectionItemImage
                        ddlImage.SelectedValue = .ImageFileNM
                        tbPosition.Text = .DisplayOrder
                        tbImageName.Text = .ImageNM
                        tbImageDescription.Text = .ImageDS
                        ddlImageType.SelectedValue = .ImageType
                        litImage.Text = wpmMineralCollection.Display.GetThumbnailHTML(.ImageFileNM)
                    End With
                End Using
            ElseIf reqCollectionItemImageID = 0 Then
                ' Insert Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                LoadImageDropDown()
                tbPosition.Text = 1
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myImageFiles As New List(Of Object)
                    myImageFiles.AddRange((From i In myCon.CollectionItemImages Where i.CollectionItemID = hfCollectionItemID.Value ).ToList())

                    Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Specimen Images (<a href='/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=" & hfCollectionItemID.Value &"&CollectionItemImageID=0'>Add Specimen Image</a>)",
                                                                        .DetailFieldName = "ImageFileNM",
                                                                        .DetailKeyName = "CollectionItemImageID",
                                                                        .DetailPath = "/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=" & hfCollectionItemID.Value & "&CollectionItemImageID={0}"}
                    myTableHeader.AddHeaderThumbnailItem("Thumbnail",
                                                         "ImageFileNM",
                                                         "/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=" & hfCollectionItemID.Value & "&CollectionItemImageID={0}", 
                                                         "CollectionItemImageID", 
                                                         "ImageFileNM", 
                                                         "/sites/nrc/thumbnails/")
                    myTableHeader.AddHeaderItem("ImageType", "ImageType")
                    myTableHeader.AddHeaderItem("DisplayOrder", "DisplayOrder")
                    myTableHeader.AddHeaderItem("Name", "ImageNM")
                    myTableHeader.AddHeaderItem("Description", "ImageDS")
                    dtList.BuildTable(myTableHeader, myImageFiles)
                End Using
            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionImageItem = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = reqCollectionItemImageID).Single
            With myCollectionImageItem
                .ImageNM = tbImageName.Text
                .ImageDS = tbImageDescription.Text
                .DisplayOrder = tbPosition.Text
                .ImageType = ddlImageType.SelectedValue
                .ModifiedDT = Now()
                .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            End With
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionImageItem As New wpmMineralCollection.CollectionItemImage
            With myCollectionImageItem
                .CollectionItemID = hfCollectionItemID.Value
                .ImageFileNM = ddlImage.SelectedValue
                .ImageNM = tbImageName.Text
                .ImageDS = tbImageDescription.Text
                .DisplayOrder = tbPosition.Text
                .ImageType = ddlImageType.SelectedValue
                .ModifiedDT = Now()
                .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            End With
            myCon.CollectionItemImages.InsertOnSubmit(myCollectionImageItem)
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionImageItem = (From i In myCon.CollectionItemImages Where i.CollectionItemImageID = reqCollectionItemImageID).Single
            myCon.CollectionItemImages.DeleteOnSubmit(myCollectionImageItem)
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Private Sub LoadImageDropDown()
        ' Load Image List for current item OR from folder that are NOT associated with a specimen already
        Dim myImages As New List(Of LookupItem)
        Using myCon As New wpmMineralCollection.DataController()
            ' Get Images that have NOT been Assigned
            Dim myDir As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("/sites/nrc/images/"))
            Dim dbImages = (From o In myCon.CollectionItemImages Select o.ImageFileNM).ToList()
            Dim myColl = (From i As FileInfo In myDir.GetFiles("*.jpg") Where Not dbImages.Contains(i.Name))
            ddlImage.Items.Clear()
            ddlImage.DataSource = myColl
            ddlImage.DataTextField = "Name"
            ddlImage.DataValueField = "Name"
            ddlImage.DataBind()
        End Using
    End Sub

    Protected Sub ddlImage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlImage.SelectedIndexChanged
        pnlImageThumb.Visible = True
        litImage.Text = wpmMineralCollection.Display.GetThumbnailHTML(ddlImage.SelectedValue)
    End Sub
End Class

