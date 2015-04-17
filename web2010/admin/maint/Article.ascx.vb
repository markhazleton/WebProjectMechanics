Imports WebProjectMechanics

Public Class admin_maint_Article
    Inherits ApplicationUserControl
    Private Const STR_ArticleID As String = "ArticleID"
    Private myHTMLControl As IHTMLControl

    Public Property reqArticleID As String
    Public Property reqArticle As Article

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        myHTMLControl = DirectCast(HTMLTextBox, IHTMLControl)
        reqArticleID = GetProperty(STR_ArticleID, Session(STR_ArticleID))
        Session(STR_ArticleID) = String.Empty
        If Not IsPostBack Then
            ddlPage.DataSource = (From i In masterPage.myCompany.LocationList Where i.RecordSource = "Page" Select New LookupItem With {.Name = String.Format("{0} = ({1})", i.LocationName, i.LocationTypeCD), .Value = i.LocationID}).ToList
            ddlPage.DataTextField = "Name"
            ddlPage.DataValueField = "Value"
            ddlPage.DataBind()
            If reqArticleID = "new" or reqArticleID = "0" Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                tbAuthor.Text = wpm_GetUserName
                tbPubDate.Text = Now().ToString("yyyy-MM-ddTHH:mm")
            ElseIf reqArticleID <> String.Empty Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                GetSiteTemplateForEdit(masterPage.myCompany)
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site Articles (<a href=""/admin/maint/default.aspx?type=Article&ArticleID=0"">Add Article</a>)"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Summary", .Value = "ArticleSummary"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "ArticleDescription"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = True,
                                                                              .Name = "Page Name",
                                                                              .Value = "ArticlePageID",
                                                                              .LinkPath = "/admin/maint/default.aspx?Type=Location&LocationID={0}",
                                                                              .LinkTextName = "PageName",
                                                                              .LinkKeyName = "ArticlePageID"
                                                                             })
                myListHeader.AddHeaderItem("Publish Date","ArticleModDate")
                myListHeader.DetailKeyName = "ArticleID"
                myListHeader.DetailFieldName = "ArticleName"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=Article&ArticleID={0}"
                Dim myList As New List(Of Object)
                myList.AddRange(masterPage.myCompany.ArticleList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        reqArticle = masterPage.myCompany.ArticleList.FindArticle(reqArticleID)
        With reqArticle
            .ArticleBody = myHTMLControl.GetHTML()
            .ArticleName = wpm_GetDBString(tbTitle.Text)
            .ArticleDescription = wpm_GetDBString(tbDescription.Text)
            .ArticleSummary = wpm_GetDBString(tbSummary.Text)
            .CompanyID = wpm_CurrentSiteID
            .ContactID = wpm_ContactID
            .ArticlePageID = ddlPage.SelectedValue
            .ArticleAuthor = wpm_GetDBString(tbAuthor.Text)
            .IsArticleActive = True
            .ArticleModDate = wpm_GetDBDate(tbPubDate.Text)
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
            .ArticlePageID = ddlPage.SelectedValue
            .ArticleAuthor = wpm_GetDBString(tbAuthor.Text)
            .IsArticleActive = True
            .ArticleModDate = wpm_GetDBDate(tbPubDate.Text)
        End With
        reqArticle.UpdateArticle()

        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        reqArticle = masterPage.myCompany.ArticleList.FindArticle(reqArticleID)
        OnUpdated(Me)
    End Sub

    Private Sub GetSiteTemplateForEdit(ByRef myCompany As ActiveCompany)
        reqArticle = myCompany.ArticleList.FindArticle(reqArticleID)
        With reqArticle

            tbPubDate.Text = .ArticleModDate.ToString("yyyy-MM-ddTHH:mm")
            ArticleIDLabel.Text = .ArticleID
            tbAuthor.Text = wpm_GetDBString(.ArticleAuthor)
            ddlPage.SelectedValue = .ArticlePageID
            myHTMLControl.SetHTML(.ArticleBody)
            tbTitle.Text = wpm_GetDBString(.ArticleName)
            tbDescription.Text = wpm_GetDBString(.ArticleDescription)
            tbSummary.Text = wpm_GetDBString(.ArticleSummary)
        End With
    End Sub

End Class
