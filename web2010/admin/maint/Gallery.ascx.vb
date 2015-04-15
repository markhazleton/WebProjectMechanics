Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_LocationGallery
    Inherits ApplicationUserControl
    Private Const STR_LocationCD As String = "LocationID"
    Private Const STR_LocationTypeCD As String = "LocationTypeCD"
    Private Const STR_RecordSource As String = "RecordSource"
    Public Property reqLocationID As String
    Public Property reqLocatinTypeCD As String
    Public Property reqRecordSource As String

    Public Property reqLocation As Location

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqLocationID = GetProperty(STR_LocationCD, String.Empty)
        reqLocatinTypeCD = GetProperty(STR_LocationTypeCD, String.Empty)
        reqRecordSource = GetProperty(STR_RecordSource, String.Empty)
        If Not IsPostBack Then
            If reqLocationID <> String.Empty AndAlso reqLocationID <> "NEW" Then
                pnlEdit.Visible = True
                dtLocationList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetLocationForEdit(masterPage.myCompany)

            ElseIf reqLocationID = "NEW" Then
                pnlEdit.Visible = True
                dtLocationList.Visible = False
                ' Insert Mode
                cmd_SaveNew.Visible = False
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                GetLocationForEdit(masterPage.myCompany)
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtLocationList.Visible = True

                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Locations (<a href='/admin/maint/default.aspx?type=Location'>All Locations</a> , <a href='/admin/maint/default.aspx?type=Location&LocationID=NEW'>Add New Location</a>)"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "LocationDescription"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Title", .Value = "LocationTitle"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "SiteTypeID", .Value = "SiteTypeID"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Location Type",
                                                                              .Value = "LocationTypeCD",
                                                                              .LinkKeyName = "LocationTypeCD",
                                                                              .LinkTextName = "LocationTypeCD",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Location&LocationTypeCD={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "ParentLocationID",
                                                                              .Value = "ParentLocationID",
                                                                              .LinkKeyName = "ParentLocationID",
                                                                              .LinkTextName = "ParentLocationID",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Location&LocationID={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Record Source",
                                                                              .Value = "RecordSource",
                                                                              .LinkKeyName = "RecordSource",
                                                                              .LinkTextName = "RecordSource",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Location&RecordSource={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Location Group",
                                                                              .Value = "LocationGroupID",
                                                                              .LinkKeyName = "LocationGroupID",
                                                                              .LinkTextName = "LocationGroupNM",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Location&LocationGroupID={0}"})
                myListHeader.DetailKeyName = "LocationID"
                myListHeader.DetailFieldName = "LocationName"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=Location&LocationID={0}"
                Dim myList As New List(Of Object)

                If reqLocatinTypeCD <> String.Empty Then
                    myList.AddRange(From i In masterPage.myCompany.LocationList Where i.LocationTypeCD = reqLocatinTypeCD Select i)
                Else
                    If reqRecordSource <> String.Empty Then
                        myList.AddRange(From i In masterPage.myCompany.LocationList Where i.RecordSource = reqRecordSource Select i)
                    Else
                        myList.AddRange(masterPage.myCompany.LocationList)
                    End If
                End If

                dtLocationList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(reqLocationID.ToString(), 0)
        With myLocation
            .GroupID = ddlGroup.SelectedValue
            .LocationTypeID = ddlLocationType.SelectedValue
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbTitle.Text
            .LocationDescription = tbLocationDS.Text
            .LocationKeywords = tbKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = tbLocationOrder.Text
            .LocationGroupID = ddlLocationGroup.SelectedValue
            .LocationFileName = tbFileName.Text
            .ActiveFL = cbActive.Checked
            .RecordSource = "Page"
            .ImagesPerRow = wpm_GetDBInteger(tbImagesPerRow.Text, 3)
            .RowsPerPage = wpm_GetDBInteger(tbRowsPerPage.Text, 3)
        End With
        If myLocation.DefaultOrder < 1 Then
            myLocation.DefaultOrder = 1
        End If
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, CInt(ddlGroup.SelectedValue))
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs) Handles cmd_SaveNew.Click
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(reqLocationID.ToString(), 0)
        With myLocation
            .LocationID = -1
            .GroupID = ddlGroup.SelectedValue
            .LocationTypeID = ddlLocationType.SelectedValue
            .LocationTypeCD = ddlLocationType.SelectedItem.Text
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbTitle.Text
            .LocationDescription = tbLocationDS.Text
            .LocationKeywords = tbKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = tbLocationOrder.Text
            .LocationGroupID = ddlLocationGroup.SelectedValue
            .LocationFileName = tbFileName.Text
            .ActiveFL = cbActive.Checked
            .RecordSource = "Page"
            .ImagesPerRow = wpm_GetDBInteger(tbImagesPerRow.Text, 3)
            .RowsPerPage = wpm_GetDBInteger(tbRowsPerPage.Text, 3)
        End With
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, CInt(ddlGroup.SelectedValue))
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(reqLocationID.ToString(), 0)
        With myLocation
            .LocationID = -1
            .GroupID = ddlGroup.SelectedValue
            .LocationTypeID = ddlLocationType.SelectedValue
            .LocationTypeCD = ddlLocationType.SelectedItem.Text
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbTitle.Text
            .LocationDescription = tbLocationDS.Text
            .LocationKeywords = tbKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = tbLocationOrder.Text
            .LocationGroupID = ddlLocationGroup.SelectedValue
            .LocationFileName = tbFileName.Text
            .ActiveFL = cbActive.Checked
            .RecordSource = "Page"
            .ImagesPerRow = wpm_GetDBInteger(tbImagesPerRow.Text, 3)
            .RowsPerPage = wpm_GetDBInteger(tbRowsPerPage.Text, 3)
        End With
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, CInt(ddlGroup.SelectedValue))
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(reqLocationID.ToString(), 0)
        wpm_DeletePageDB(myLocation)
        OnUpdated(Me)
    End Sub

    Private Sub GetLocationForEdit(ByRef myCompany As ActiveCompany)
        reqLocation = myCompany.LocationList.FindLocation(reqLocationID, 0)

        If reqLocation.RecordSource = "Image" Then
            Response.Redirect(String.Format("/admin/maint/default.aspx?Type=Image&ImageID={0}", reqLocation.ArticleID))
        End If
        If reqLocation.RecordSource = "Article" Then
            Response.Redirect(String.Format("/admin/maint/default.aspx?Type=Article&ArticleID={0}", reqLocation.ArticleID))
        End If

        If reqLocation.LocationTypeCD = "Blog" Then
            Response.Redirect(String.Format("/admin/maint/default.aspx?Type=Blog&LocationID={0}", reqLocation.LocationID))
        End If


        Dim myImages = myCompany.GetLocationImages(reqLocation.LocationID)
        Dim myThumbSB As New StringBuilder

        For Each myImage In myImages
            pnlPageImages.Controls.Add(New Literal With {.Text = String.Format("<div class=""col-xs-6 col-md-3""><a class=""thumbnail"" href=""/admin/maint/default.aspx?Type=Image&ImageID={2}""><img width=""150px"" src=""/runtime/catalog/FindImage.ashx?w=150&img={1}{0}"" alt=""{3}""></a></div>", myImage.ImageThumbFileName, wpm_SiteGallery, myImage.ImageID, myImage.ImageName)})
        Next
        ddlLocationGroup.Items.Clear()
        ddlLocationGroup.AppendDataBoundItems = True
        ddlLocationGroup.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
        ddlLocationGroup.DataSource = (From i In myCompany.LocationGroupList Order By i.LocationGroupNM Select i.LocationGroupID, i.LocationGroupNM)
        ddlLocationGroup.DataTextField = "LocationGroupNM"
        ddlLocationGroup.DataValueField = "LocationGroupID"
        ddlLocationGroup.DataBind()
        ddlParentLocation.Items.Clear()
        If reqLocation.RecordSource = "Category" Then
            ddlParentLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
            ddlParentLocation.AppendDataBoundItems = True
            ddlParentLocation.DataSource = (From i In myCompany.LocationList Where i.RecordSource = "Category" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
            ddlParentLocation.DataTextField = "Location"
            ddlParentLocation.DataValueField = "LocationID"
            ddlParentLocation.DataBind()
            ddlLocationType.Items.Clear()
            ddlLocationType.Items.Add(myCompany.SiteCategoryTypeNM)
        Else
            ddlParentLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
            ddlParentLocation.AppendDataBoundItems = True
            ddlParentLocation.DataSource = (From i In myCompany.LocationList Where i.RecordSource = "Page" Or i.RecordSource = "Category" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
            ddlParentLocation.DataTextField = "Location"
            ddlParentLocation.DataValueField = "LocationID"
            ddlParentLocation.DataBind()
            wpm_LoadCMB(ddlLocationType, reqLocation.LocationTypeCD, "SELECT PageType.PageTypeID, PageType.PageTypeCD FROM PageType ORDER BY PageType.PageTypeCD;", "PageTypeCD", "PageTypeID", False)
        End If
        wmp_LoadCompanyDropDow(ddlCompany, wpm_DomainConfig.CompanyID, True)
        ddlCompany.Enabled = True
        With reqLocation
            LocationIDLit.Text = .LocationID
            tbLocationNM.Text = .LocationName
            tbTitle.Text = .LocationTitle
            tbLocationDS.Text = .LocationDescription
            tbKeywords.Text = .LocationKeywords
            ddlParentLocation.SelectedValue = .ParentLocationID
            tbLocationOrder.Text = .DefaultOrder
            ddlLocationGroup.SelectedValue = .LocationGroupID
            ddlLocationType.SelectedValue = .LocationTypeID
            ddlGroup.SelectedValue = .GroupID
            cbActive.Checked = .ActiveFL
            tbFileName.Text = .LocationFileName
            tbRowsPerPage.Text = .RowsPerPage
            tbImagesPerRow.Text = .ImagesPerRow


        End With
    End Sub

End Class
