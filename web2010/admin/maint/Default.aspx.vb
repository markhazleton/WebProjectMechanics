Imports WebProjectMechanics

Public Class Admin_Maint_Default
    Inherits AdminPage


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        rptNavigation.DataSource = LoadMenu()
        rptNavigation.DataBind()

        Select Case wpm_GetProperty("Type", String.Empty)
            Case "SiteTemplate"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/SiteTemplate.ascx"), ApplicationUserControl)
                If wpm_GetProperty("TemplateCD", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=SiteTemplate"
                End If
            Case "Location"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Location.ascx"), ApplicationUserControl)
                If wpm_GetProperty("LocationID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Location"
                End If
            Case "Article"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Article.ascx"), ApplicationUserControl)
                If wpm_GetProperty("ArticleID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Article"
                End If
            Case "Blog"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Blog.ascx"), ApplicationUserControl)
                If wpm_GetProperty("LocationID", String.Empty) = String.Empty Then
                    myControl = DirectCast(Page.LoadControl("~/admin/maint/Location.ascx"), ApplicationUserControl)
                Else
                    If wpm_GetProperty("ArticleID", String.Empty) = String.Empty Then
                        wpm_ListPageURL = String.Format("/admin/maint/default.aspx?Type=Blog&LocationID={0}", wpm_GetProperty("LocationID", String.Empty))
                    End If
                End If
            Case "Gallery"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Gallery.ascx"), ApplicationUserControl)
                If wpm_GetProperty("LocationID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Gallery"
                End If
            Case "Part"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Part.ascx"), ApplicationUserControl)
                If wpm_GetProperty("PartID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Part"
                End If
            Case "SiteType"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/SiteType.ascx"), ApplicationUserControl)
                If wpm_GetProperty("SiteTypeID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=SiteType"
                End If
            Case "PageType"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/PageType.ascx"), ApplicationUserControl)
                If wpm_GetProperty("PageTypeID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=PageType"
                End If
            Case "ParameterType"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/ParameterType.ascx"), ApplicationUserControl)
                If wpm_GetProperty("ParameterTypeID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=ParameterType"
                End If
            Case "Parameter"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Parameter.ascx"), ApplicationUserControl)
                If wpm_GetProperty("ParameterID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Parameter"
                End If
            Case "Image"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Image.ascx"), ApplicationUserControl)
                If wpm_GetProperty("ImageID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Image"
                End If
            Case "Company"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Company.ascx"), ApplicationUserControl)
                If wpm_GetProperty("CompanyID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Company"
                End If
            Case "Contact"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Contact.ascx"), ApplicationUserControl)
                If wpm_GetProperty("ContactID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Contact"
                End If
            Case "PageAlias"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/PageAlias.ascx"), ApplicationUserControl)
                If wpm_GetProperty("PageAliasID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=PageAlias"
                End If
            Case "Recipe"
                myControl = DirectCast(Page.LoadControl("~/admin/maint/Recipe.ascx"), ApplicationUserControl)
                If wpm_GetProperty("RecipeID", String.Empty) = String.Empty Then
                    wpm_ListPageURL = "/admin/maint/default.aspx?Type=Recipe"
                End If
            Case Else
                '
                '
                '
        End Select
        AddHandler myControl.cmd_Updated, AddressOf RedirectToListPage
        AddHandler myControl.cmd_Canceled, AddressOf RedirectToListPage
        pnlMaintenance.Controls.Add(myControl)
    End Sub

End Class
