
Partial Class aspmaker_fckeditor_mhImageEdit
    Inherits mhPage
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            Dim myUpdateArticle As New mhArticle(tbArticleID.Text)
            myUpdateArticle.ArticleBody = FCKeditor1.Value
            myUpdateArticle.ArticleName = tbArticleTitle.Text
            myUpdateArticle.ArticleModDate = Now()
            If myUpdateArticle.updateArticleBody() Then
                Response.Redirect(Session("ListPageURL"))
            End If
        Else
            application("FCKeditor:UserFilesPath") = Session("SiteGallery")
            tbArticleID.Text = GetProperty("a", "")
            If tbArticleID.Text <> "" Then
                Dim myArticle As New mhArticle(tbArticleID.Text)
                FCKeditor1.Value = myArticle.ArticleBody
                tbArticleTitle.Text = myArticle.ArticleName
                PopulateContactDropDownList(myArticle.ContactID, Session("CompanyID"), Me.DropDownListContact)
            End If
        End If
    End Sub

    Private Function PopulateContactDropDownList(ByVal sContactID As String, ByVal CompanyID As String, ByRef myDDL As DropDownList) As Boolean
        Dim sqlwrk As String
        Dim mydt As DataTable
        sqlwrk = "SELECT ContactID, PrimaryContact & ' (' & LogonName & ')' FROM Contact where Contact.CompanyID=" & CompanyID & " ORDER BY PrimaryContact "
        mydt = mhDB.GetDataTable(sqlwrk, "Browse_Image.PopulateContactDropDown")
        For Each row As DataRow In mydt.Rows
            Dim MyLI As New ListItem
            MyLI.Text = mhUTIL.GetDBString(row(1))
            MyLI.Value = mhUTIL.GetDBString(row(0))
            If MyLI.Value = sContactID Then
                MyLI.Selected = True
            End If
            myDDL.Items.Add(MyLI)
        Next
        Return True
    End Function

End Class
