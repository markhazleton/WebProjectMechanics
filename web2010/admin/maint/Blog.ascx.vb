Imports WebProjectMechanics

Public Class admin_maint_Blog
    Inherits ApplicationUserControl
    Private Const STR_ArticleID As String = "ArticleID"
    Private Const STR_LocationID As String = "LocationID"
    Private myHTMLControl As IHTMLControl

    Public Property reqArticle As Article

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        myHTMLControl = DirectCast(HTMLTextBox, IHTMLControl)
        hfArticleID.Value = GetProperty(STR_ArticleID, Session(STR_ArticleID))
        hfLocationID.Value = GetProperty(STR_LocationID, Session(STR_LocationID))

        Session(STR_ArticleID) = String.Empty
        Session(STR_LocationID) = String.Empty

        ddlParentLocation.Items.Clear()
        ddlParentLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
        ddlParentLocation.AppendDataBoundItems = True
        ddlParentLocation.DataSource = (From i In masterPage.myCompany.LocationList Where i.RecordSource = "Page" Select i.LocationID, i.LocationName).ToList()
        ddlParentLocation.DataTextField = "LocationName"
        ddlParentLocation.DataValueField = "LocationID"
        ddlParentLocation.DataBind()

        If Not IsPostBack Then
            ' Get Blog Location Information 
            Dim BlogLocation As Location = masterPage.myCompany.LocationList.FindLocation(hfLocationID.Value, 0)
            With BlogLocation
                LocationIDLit.Text = .LocationID
                tbLocationNM.Text = .LocationName
                ddlParentLocation.SelectedValue = .ParentLocationID
                tbLocationOrder.Text = .DefaultOrder
                tbLocationTitle.Text = .LocationTitle
                tbLocationDescription.Text = .LocationDescription
                tbLocationKeywords.Text = .LocationKeywords
                ddlGroup.SelectedValue = .GroupID
                tbRowsPerPage.Text = .RowsPerPage
                tbImagesPerRow.Text = .ImagesPerRow
                cbActive.Checked = .ActiveFL
            End With

            If hfArticleID.Value <> String.Empty Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetSiteTemplateForEdit()
            ElseIf hfArticleID.Value = "new" Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = String.Format("Blog Articles (<a href=""/admin/maint/default.aspx?type=Blog&LocationID={0}&ArticleID=0"">Add Blog Article</a>)", hfLocationID.Value)}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Summary", .Value = "ArticleSummary"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "ArticleDescription"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Page Name",
                                                                              .Value = "ArticlePageID",
                                                                              .LinkPath = String.Format("/admin/maint/default.aspx?Type=Blog&LocationID={0}&ArticleID=", hfLocationID.Value) & "{0}",
                                                                              .LinkTextName = "PageName",
                                                                              .LinkKeyName = "ArticlePageID"
                                                                             })
                myListHeader.DetailKeyName = "ArticleID"
                myListHeader.DetailFieldName = "ArticleName"
                myListHeader.DetailPath = String.Format("/admin/maint/default.aspx?Type=Blog&LocationID={0}&ArticleID=", hfLocationID.Value) & "{0}"
                Dim myList As New List(Of Object)
                myList.AddRange((From i In masterPage.myCompany.ArticleList Where i.ArticlePageID = hfLocationID.Value Select i).ToList())
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        reqArticle = masterPage.myCompany.ArticleList.FindArticle(hfArticleID.Value)
        With reqArticle
            .ArticleBody = myHTMLControl.GetHTML()
            .ArticleName = wpm_GetDBString(tbTitle.Text)
            .ArticleDescription = wpm_GetDBString(tbDescription.Text)
            .ArticleSummary = wpm_GetDBString(tbSummary.Text)
            .CompanyID = wpm_CurrentSiteID
            .ContactID = wpm_ContactID
            .ArticlePageID = hfLocationID.Value
            .ArticleAuthor = wpm_GetDBString(tbAuthor.Text)
            .IsArticleActive = True
        End With
        reqArticle.UpdateArticle()
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        reqArticle = New Article With {.ArticleID = -1}
        With reqArticle
            .ArticleBody = myHTMLControl.GetHTML()
            .ArticleName = wpm_GetDBString(tbTitle.Text)
            .ArticleDescription = wpm_GetDBString(tbDescription.Text)
            .ArticleSummary = wpm_GetDBString(tbSummary.Text)
            .CompanyID = wpm_CurrentSiteID
            .ContactID = wpm_ContactID
            .ArticlePageID = hfLocationID.Value
            .ArticleAuthor = wpm_GetDBString(tbAuthor.Text)
            .IsArticleActive = True
        End With
        reqArticle.UpdateArticle()
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        reqArticle = masterPage.myCompany.ArticleList.FindArticle(hfArticleID.Value)
        OnUpdated(Me)
    End Sub

    Private Sub GetSiteTemplateForEdit()
        reqArticle = masterPage.myCompany.ArticleList.FindArticle(hfArticleID.Value)
        With reqArticle
            ArticleIDLabel.Text = .ArticleID
            tbAuthor.Text = wpm_GetDBString(.ArticleAuthor)
            myHTMLControl.SetHTML(.ArticleBody)
            tbTitle.Text = wpm_GetDBString(.ArticleName)
            tbDescription.Text = wpm_GetDBString(.ArticleDescription)
            tbSummary.Text = wpm_GetDBString(.ArticleSummary)
        End With
    End Sub

    Protected Sub cmd_UpdateLocation_Click(sender As Object, e As EventArgs)
        Dim myLocation As Location = masterPage.myCompany.LocationList.FindLocation(hfLocationID.Value, 0)
        With myLocation
            .GroupID = wpm_GetDBInteger(ddlGroup.SelectedValue)
            .LocationName = tbLocationNM.Text
            .LocationTitle = tbLocationTitle.Text
            .LocationDescription = tbLocationDescription.Text
            .LocationKeywords = tbLocationKeywords.Text
            .ParentLocationID = ddlParentLocation.SelectedValue
            .DefaultOrder = wpm_GetDBInteger( tbLocationOrder.Text)
            .ActiveFL = wpm_GetDBBoolean(cbActive.Checked)
            .RowsPerPage = wpm_GetDBInteger(tbRowsPerPage.text)
            .ImagesPerRow = wpm_GetDBInteger( tbImagesPerRow.text)
        End With
        If myLocation.DefaultOrder < 1 Then
            myLocation.DefaultOrder = 1
        End If
        wpm_UpdateLocation(myLocation, wpm_CurrentSiteID, CInt(ddlGroup.SelectedValue))
        OnUpdated(Me)
    End Sub
End Class
