Imports WebProjectMechanics

Public Class admin_maint_SiteCategory
    Inherits ApplicationUserControl
    Private Const STR_SiteCategoryID As String = "SiteCategoryID"
    Private Const STR_LocationTypeCD As String = "LocationTypeCD"
    Public Property reqSiteCategoryID As String
    Public Property reqLocatinTypeCD As String
    Public Property reqRecordSource As String

    Public Property reqLocation As Location

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteCategoryID = GetProperty(STR_SiteCategoryID, String.Empty)
        reqLocatinTypeCD = "category"
        If Not IsPostBack Then
            If reqSiteCategoryID <> String.Empty AndAlso reqSiteCategoryID <> "NEW" Then
                pnlEdit.Visible = True
                dtLocationList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetLocationForEdit(masterPage.myCompany)

            ElseIf reqSiteCategoryID = "NEW" Then
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

                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site Categories (<a href='/admin/maint/default.aspx?type=SiteCategory'>All Site Categories</a> , <a href='/admin/maint/default.aspx?type=SiteCategory&SiteCategoryID=NEW'>Add New Site Category</a>)"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Location Group",
                                                                              .Value = "LocationGroupID",
                                                                              .LinkKeyName = "LocationGroupID",
                                                                              .LinkTextName = "LocationGroupNM",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=SiteCategory&LocationGroupID={0}"})
                myListHeader.AddHeaderItem("LocationKeywords", "LocationKeywords")
                myListHeader.DetailKeyName = "SiteCategoryID"
                myListHeader.DetailFieldName = "SiteCategoryName"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=SiteCategory&SiteCategoryID={0}"

                Dim myList As New List(Of Object)

                For Each myrow In wpm_GetDataTable("SELECT SiteCategory.SiteCategoryID, SiteCategory.CategoryKeywords, SiteCategory.CategoryName, SiteCategory.CategoryTitle, SiteCategory.CategoryDescription, SiteCategory.GroupOrder, SiteCategory.ParentCategoryID, SiteCategory.CategoryFileName, SiteCategory.SiteCategoryTypeID, SiteCategory.SiteCategoryGroupID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryGroup.SiteCategoryGroupNM, SiteCategoryGroup.SiteCategoryGroupDS, SiteCategoryGroup.SiteCategoryGroupOrder, SiteCategory_Parent.CategoryName AS ParentCategoryNM FROM (SiteCategoryType RIGHT JOIN (SiteCategoryGroup RIGHT JOIN SiteCategory ON SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID) ON SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID) LEFT JOIN SiteCategory AS SiteCategory_Parent ON SiteCategory.ParentCategoryID = SiteCategory_Parent.SiteCategoryID;", "SiteCategory").Rows
                    myList.Add(New Location With {.LocationID = wpm_GetDBInteger(myrow("SiteCategoryID")),
                                                  .LocationName = wpm_GetDBString(myrow("CategoryName")),
                                                  .LocationKeywords = wpm_GetDBString(myrow("CategoryKeywords")),
                                                  .LocationGroupID = wpm_GetDBInteger(myrow("SiteCategoryGroupID")),
                                                  .LocationGroupNM = wpm_GetDBString(myrow("SiteCategoryGroupNM")),
                                                  .RecordSource = "SiteCategory",
                                                  .ParentLocationID = wpm_GetDBInteger(myrow("ParentCategoryID")),
                                                  .SiteCategoryID = wpm_GetDBInteger(myrow("SiteCategoryID")),
                                                  .SiteCategoryName = wpm_GetDBString(myrow("CategoryName")),
                                                  .LocationDescription = wpm_GetDBString(myrow("CategoryDescription")),
                                                  .ModifiedDT = Now
                                                 })
                Next
                dtLocationList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myLocation As New Location
        With myLocation
            .SiteCategoryID = LocationIDLit.Text
            .LocationID = LocationIDLit.Text
            .LocationTypeID = ddlLocationType.SelectedValue
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbTitle.Text
            .LocationDescription = tbLocationDS.Text
            .LocationKeywords = tbKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = tbLocationOrder.Text
            .LocationGroupID = ddlLocationGroup.SelectedValue
            .LocationFileName = tbFileName.Text
            .RecordSource = "Category"
        End With
        If myLocation.DefaultOrder < 1 Then
            myLocation.DefaultOrder = 1
        End If
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, 4)
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs) Handles cmd_SaveNew.Click
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(reqSiteCategoryID.ToString(), 0)
        With myLocation
            .LocationID = -1
            .SiteCategoryID = -1
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
            .RecordSource = "Category"
        End With
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, 4)
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myLocation As New Location
        With myLocation
            .LocationID = -1
            .SiteCategoryID = -1
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
            .RecordSource = "Category"
        End With

        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, 4)
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myLocation As New Location
        With myLocation
            .SiteCategoryID = LocationIDLit.Text
            .LocationID = LocationIDLit.Text
            .LocationTypeID = ddlLocationType.SelectedValue
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbTitle.Text
            .LocationDescription = tbLocationDS.Text
            .LocationKeywords = tbKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = tbLocationOrder.Text
            .LocationGroupID = ddlLocationGroup.SelectedValue
            .LocationFileName = tbFileName.Text
            .RecordSource = "Category"
        End With
        wpm_DeletePageDB(myLocation)
        OnUpdated(Me)
    End Sub

    Private Sub GetLocationForEdit(ByRef myCompany As ActiveCompany)
        reqLocation = New Location
        reqSiteCategoryID = GetProperty(STR_SiteCategoryID, String.Empty)

        For Each myrow In wpm_GetDataTable(String.Format("SELECT SiteCategory.SiteCategoryID, SiteCategory.CategoryKeywords, SiteCategory.CategoryName, SiteCategory.CategoryTitle, SiteCategory.CategoryDescription, SiteCategory.GroupOrder, SiteCategory.ParentCategoryID, SiteCategory.CategoryFileName, SiteCategory.SiteCategoryTypeID, SiteCategory.SiteCategoryGroupID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryGroup.SiteCategoryGroupNM, SiteCategoryGroup.SiteCategoryGroupDS, SiteCategoryGroup.SiteCategoryGroupOrder, SiteCategory_Parent.CategoryName AS ParentCategoryNM FROM (SiteCategoryType RIGHT JOIN (SiteCategoryGroup RIGHT JOIN SiteCategory ON SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID) ON SiteCategoryType.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID) LEFT JOIN SiteCategory AS SiteCategory_Parent ON SiteCategory.ParentCategoryID = SiteCategory_Parent.SiteCategoryID where SiteCategory.SiteCategoryID={0};", reqSiteCategoryID), "SiteCategory").Rows
            With reqLocation
                .LocationID = wpm_GetDBInteger(myrow("SiteCategoryID"))
                .LocationName = wpm_GetDBString(myrow("CategoryName"))
                .LocationKeywords = wpm_GetDBString(myrow("CategoryKeywords"))
                .LocationGroupID = wpm_GetDBInteger(myrow("SiteCategoryGroupID"))
                .LocationGroupNM = wpm_GetDBString(myrow("SiteCategoryGroupNM"))
                .LocationTitle = wpm_GetDBString(myrow("CategoryTitle"))
                .LocationFileName = wpm_GetDBString(myrow("CategoryFileName"))
                .RecordSource = "SiteCategory"

                .ParentLocationID = wpm_GetDBInteger(myrow("ParentCategoryID"))

                If wpm_GetDBInteger(.ParentLocationID)>0 then 
                    .ParentLocationID = String.Format("CAT-{0}",.ParentLocationID)
                End If

                .SiteCategoryID = wpm_GetDBInteger(myrow("SiteCategoryID"))
                .SiteCategoryName = wpm_GetDBString(myrow("CategoryName"))
                .LocationDescription = wpm_GetDBString(myrow("CategoryDescription"))
                .DefaultOrder = wpm_GetDBInteger(myrow("GroupOrder"))
                .ModifiedDT = Now
            End With
        Next


        ddlLocationGroup.Items.Clear()
        ddlLocationGroup.AppendDataBoundItems = True
        ddlLocationGroup.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
        ddlLocationGroup.DataSource = (From i In myCompany.LocationGroupList Order By i.LocationGroupNM Select i.LocationGroupID, i.LocationGroupNM)
        ddlLocationGroup.DataTextField = "LocationGroupNM"
        ddlLocationGroup.DataValueField = "LocationGroupID"
        ddlLocationGroup.DataBind()
        ddlParentLocation.Items.Clear()

        ddlParentLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
        ddlParentLocation.AppendDataBoundItems = True
        ddlParentLocation.DataSource = (From i In myCompany.LocationList Where i.RecordSource = "Category" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
        ddlParentLocation.DataTextField = "Location"
        ddlParentLocation.DataValueField = "LocationID"
        ddlParentLocation.DataBind()


        ddlLocationType.Items.Clear()
        ddlLocationType.Enabled = False

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
            tbFileName.Text = .LocationFileName
        End With
    End Sub

End Class
