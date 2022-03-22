Imports WebProjectMechanics

Public Class admin_maint_Part
    Inherits ApplicationUserControl
    Private Const STR_PartID As String = "PartID"
    Dim _myList As PartList

    Public Property reqPartID As String
        Get
            Return litPartID.Text
        End Get
        Set(value As String)
            litPartID.Text = value
        End Set
    End Property
    Public Property reqPart As Part


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' myHTMLControl = DirectCast(HTMLTextBox, IHTMLControl)
        _myList = New PartList()
        _myList.GetSiteParts(wpm_CurrentSiteID)

        If Not IsPostBack Then


            reqPart = _myList.FindPart(GetProperty(STR_PartID, String.Empty))

            wpm_LoadCMB(ddlType, reqPart.PartTypeCD, "SELECT LinkType.LinkTypeCD  FROM LinkType ORDER BY LinkType.LinkTypeCD;", "LinkTypeCD", "LinkTypeCD", True)
            wpm_LoadCMB(ddlCategory, reqPart.PartCategoryID, "SELECT [ID], [Title] FROM [LinkCategory] ", "Title", "ID", True)
            wpm_LoadCMB(ddlContact, reqPart.UserID, "Select ContactID, PrimaryContact from Contact order by PrimaryContact", "PrimaryContact", "ContactID", True)
            wpm_LoadCMB(ddlSite, reqPart.CompanyID, "Select CompanyID, CompanyName from Company order by CompanyName", "CompanyName", "CompanyID", False)
            LoadCMB(ddlLocation, reqPart.LocationID, masterPage.myCompany.LocationList)


            If IsNumeric(reqPart.PartID) Then
                reqPartID = reqPart.PartID
                pnlEdit.Visible = True
                dtPartList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_SaveNew.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetPartForEdit(_myList)

            ElseIf reqPart.PartID.ToUpper() = "NEW" And Not (GetProperty("PartID", String.Empty) = String.Empty) Then

                ddlSite.SelectedValue = wpm_CurrentSiteID
                ddlLocation.SelectedValue = masterPage.myCompany.CurLocation.LocationID
                tbDisplayOrder.Text = "000"
                ddlContact.SelectedValue = wpm_ContactID
                pnlEdit.Visible = True
                dtPartList.Visible = False
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_SaveNew.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtPartList.Visible = True

                Dim myWebHomePath As String = HttpContext.Current.Server.MapPath(wpm_SiteConfig.AdminFolder)
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Part (Content) (<a href='/admin/maint/default.aspx?type=Part'>All Parts</a> , <a href='/admin/maint/default.aspx?type=Part&PartID=NEW'>Add New Part</a>)"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "Description"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "PartTypeCD",
                                                                              .Value = "PartTypeCD",
                                                                              .LinkKeyName = "PartTypeCD",
                                                                              .LinkTextName = "PartTypeCD",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Part&PartTypeCD={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "LocationNM",
                                                                              .Value = "LocationID",
                                                                              .LinkKeyName = "LocationID",
                                                                              .LinkTextName = "LocationNM",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Part&LocationID={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "CompanyNM",
                                                                              .Value = "CompanyID",
                                                                              .LinkKeyName = "CompanyID",
                                                                              .LinkTextName = "CompanyNM",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Part&CompanyID={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Record Source",
                                                                              .Value = "PartSource",
                                                                              .LinkKeyName = "PartSource",
                                                                              .LinkTextName = "PartSource",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Part&PartSource={0}"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "PartSortOrder", .Value = "PartSortOrder"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "PartCategoryTitle",
                                                                              .Value = "PartCategoryTitle",
                                                                              .LinkKeyName = "PartCategoryTitle",
                                                                              .LinkTextName = "PartCategoryTitle",
                                                                              .LinkPath = "/admin/maint/default.aspx?type=Part&PartCategoryTitle={0}"})
                myListHeader.DetailKeyName = "PartID"
                myListHeader.DetailFieldName = "Title"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=Part&PartID={0}"
                dtPartList.BuildTable(myListHeader, _myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        reqPart = _myList.FindPart(reqPartID)
        With reqPart
            .AmazonIndex = String.Empty
            .CompanyID = ddlSite.SelectedValue
            .Description = tbDescription.Text
            .LocationID = ddlLocation.SelectedValue
            .ModifiedDT = Now()
            .PartCategoryID = ddlCategory.SelectedValue
            .PartCategoryTitle = String.Empty
            .PartID = reqPartID
            .PartSortOrder = tbDisplayOrder.Text
            .PartTypeCD = ddlType.SelectedValue
            .Title = tbTitle.Text
            .URL = tbURL.Text
            .UserID = ddlContact.SelectedValue
            .View = True
        End With
        reqPart.UpdatePart()
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        reqPart = _myList.FindPart(reqPartID)
        With reqPart
            .AmazonIndex = String.Empty
            .CompanyID = ddlSite.SelectedValue
            .Description = tbDescription.Text
            .LocationID = ddlLocation.SelectedValue
            .ModifiedDT = Now()
            .PartCategoryID = ddlCategory.SelectedValue
            .PartCategoryTitle = String.Empty
            .PartID = -1
            .PartSortOrder = tbDisplayOrder.Text
            .PartTypeCD = ddlType.SelectedValue
            .Title = tbTitle.Text
            .URL = tbURL.Text
            .UserID = ddlContact.SelectedValue
            .View = True
        End With
        reqPart.UpdatePart()
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        reqPart = New Part
        With reqPart
            .AmazonIndex = String.Empty
            .CompanyID = ddlSite.SelectedValue
            .Description = tbDescription.Text
            .LocationID = ddlLocation.SelectedValue
            .ModifiedDT = Now()
            .PartCategoryID = ddlCategory.SelectedValue
            .PartCategoryTitle = String.Empty
            .PartID = -1
            .PartSortOrder = tbDisplayOrder.Text
            .PartTypeCD = ddlType.SelectedValue
            .SiteCategoryGroupID = String.Empty
            .Title = tbTitle.Text
            .URL = tbURL.Text
            .UserID = ddlContact.SelectedValue
            .View = True
        End With
        reqPart.UpdatePart()
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        ' Remind to set JS Are You Sure on Delete!
        reqPart = _myList.FindPart(reqPartID)
        With reqPart
            .AmazonIndex = String.Empty
            .CompanyID = ddlSite.SelectedValue
            .Description = tbDescription.Text
            .LocationID = ddlLocation.SelectedValue
            .ModifiedDT = Now()
            .PartCategoryID = ddlCategory.SelectedValue
            .PartCategoryTitle = String.Empty
            .PartID = reqPartID
            .PartSortOrder = tbDisplayOrder.Text
            .PartTypeCD = ddlType.SelectedValue
            .SiteCategoryGroupID = String.Empty
            .Title = tbTitle.Text
            .URL = tbURL.Text
            .UserID = ddlContact.SelectedValue
            .View = True
        End With
        reqPart.DeletePart()
        OnUpdated(Me)
    End Sub

    Private Sub GetPartForEdit(ByRef PartList As PartList)
        reqPart = PartList.FindPart(reqPartID)
        With reqPart
            ddlCategory.SelectedValue = .PartCategoryID
            ddlSite.SelectedValue = .CompanyID
            ddlLocation.SelectedValue = .LocationID
            tbTitle.Text = .Title
            tbDescription.Text = .Description
            tbURL.Text = .URL
            tbDisplayOrder.Text = .PartSortOrder
            tbAmazonIndex.Text = .AmazonIndex
        End With
    End Sub

    Public Function LoadCMB(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByRef Locations As LocationList) As Boolean
        myCMB.Items.Clear()
        myCMB.Enabled = True
        If Locations.Count > 1 Then
            myCMB.Items.Add(New ListItem() With {.Value = String.Empty, .Text = "All Locations", .Selected = True})
            For Each Loc As Location In (From I In Locations Order By I.LocationName Select I).ToList()
                myCMB.Items.Add(New ListItem() With {.Text = Loc.LocationName, .Value = Loc.LocationID, .Selected = False})
            Next
            myCMB.SelectedValue = CurrentValue
        Else
            If Locations.Count = 1 Then
                myCMB.Items.Add(New ListItem() With {.Text = Locations(0).LocationName, .Value = Locations(0).LocationID, .Selected = True})
                myCMB.Enabled = False
            End If
        End If
        Return True
    End Function

End Class
