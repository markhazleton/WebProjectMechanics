Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_Company
    Inherits ApplicationUserControl

    ' Company 
    Public Const STR_CompanyID As String = "CompanyID"
    Public Const STR_SELECTCompanyList As String = "SELECT Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate,  Company.DefaultSiteTemplate,  Company.HomePageID,  Company.DefaultArticleID,  Company.ActiveFL,  Company.UseBreadCrumbURL,  Company.City,  Company.StateOrProvince,  Company.PostalCode,  Company.Country,  Company.FromEmail, Company.SMTP, Company.Component FROM Company;"
    Public Const STR_SELECT_CompanyByCompanyID As String = "SELECT Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate,  Company.DefaultSiteTemplate,  Company.HomePageID,  Company.DefaultArticleID,  Company.ActiveFL,  Company.UseBreadCrumbURL,  Company.City,  Company.StateOrProvince,  Company.PostalCode,  Company.Country,  Company.FromEmail FROM Company where [Company].[CompanyID]={0};"
    Public Const STR_UPDATE_Company As String = "UPDATE [Company] SET [CompanyName] = @CompanyName, [City] = @City, [Address] = @Address, [PostalCode] = @PostalCode, [StateOrProvince] = @StateOrProvince, [Country] = @Country, [PhoneNumber] = @PhoneNumber, [FaxNumber] = @FaxNumber, [DefaultPaymentTerms] = @DefaultPaymentTerms, [DefaultInvoiceDescription] = @DefaultInvoiceDescription, [GalleryFolder] = @GalleryFolder, [SiteURL] = @SiteURL, [SiteTitle] = @SiteTitle, [SiteTemplate] = @SiteTemplate, [DefaultSiteTemplate] = @DefaultSiteTemplate, [DefaultArticleID] = @DefaultArticleID, [Component] = @Component, [FromEmail] = @FromEmail, [SMTP] = @SMTP, [HomePageID] = @HomePageID, [ActiveFL] = @ActiveFL, [UseBreadCrumbURL] = @UseBreadCrumbURL, [SingleSiteGallery] = @SingleSiteGallery WHERE [CompanyID] = @CompanyID;"
    Public Const STR_INSERT_Company As String = "INSERT INTO [Company] ([CompanyName], [City], [Address], [PostalCode], [StateOrProvince], [Country], [PhoneNumber], [FaxNumber], [GalleryFolder], [SiteURL], [SiteTitle], [SiteTemplate], [DefaultSiteTemplate], [DefaultArticleID], [Component], [FromEmail], [SMTP], [HomePageID], [ActiveFL], [UseBreadCrumbURL], [SingleSiteGallery]) VALUES (@CompanyName, @City, @Address, @PostalCode, @StateOrProvince, @Country, @PhoneNumber, @FaxNumber, @GalleryFolder, @SiteURL, @SiteTitle, @SiteTemplate, @DefaultSiteTemplate, @DefaultArticleID, @Component, @FromEmail, @SMTP, @HomePageID, @ActiveFL, @UseBreadCrumbURL, @SingleSiteGallery);"
    Public Const STR_DELETE_Company As String = "DELETE FROM [Company] WHERE [Company].[CompanyID]={0};"
        ' Site Template
    Public Const STR_SELECTSiteTemplateList As String = "SELECT SiteTemplate.[TemplatePrefix] as TemplatePrefix , SiteTemplate.[Name] as TemplateName FROM SiteTemplate;"

    Private Property reqCompanyID As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqCompanyID = GetProperty(STR_CompanyID, String.Empty)
        If Not IsPostBack Then
            If reqCompanyID <> String.Empty Then
                If reqCompanyID = "NEW" Or reqCompanyID = 0 Then
                    CompanyIDLabel.Text = -1
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                    GetCompanyForEdit(reqCompanyID)
                Else
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Edit Mode
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    GetCompanyForEdit(reqCompanyID)
                End If
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Dim myTemplateList As New List(Of Company)
                For Each myRow In wpm_GetDataTable(STR_SELECTCompanyList, "Company").Rows
                    myTemplateList.Add(New Company().SetCompanyValue(myRow))
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site/Company  <a href='/admin/maint/default.aspx?type=Company&CompanyID=0'>Add New Company</a> ", .DetailKeyName = "CompanyID", .DetailFieldName = "CompanyNM", .DetailPath = "/admin/maint/default.aspx?type=Company&CompanyID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "SiteGallery", .Value = "SiteGallery"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "SitePrefix",
                                                                              .Value = "SitePrefix",
                                                                              .KeyField = True,
                                                                              .LinkPath = "/admin/maint/default.aspx?type=SiteTemplate&TemplatePrefix={0}",
                                                                              .LinkTextName = "SitePrefix",
                                                                              .LinkKeyName = "SitePrefix"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "DefaultSitePrefix",
                                                                              .Value = "DefaultSitePrefix",
                                                                              .KeyField = True,
                                                                              .LinkPath = "/admin/maint/default.aspx?type=SiteTemplate&TemplatePrefix={0}",
                                                                              .LinkTextName = "SitePrefix",
                                                                              .LinkKeyName = "SitePrefix"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "CompanyURL", .Value = "CompanyURL"})
                Dim myList As New List(Of Object)
                myList.AddRange(myTemplateList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub
    Private Sub GetCompanyForEdit(ByVal myCompanyID As String)
        Dim myCompany As New Company
        Dim ArticleList As new ArticleList
        If IsNumeric(myCompanyID) AndAlso CInt(myCompanyID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_CompanyByCompanyID, myCompanyID), "Company").Rows
                myCompany.SetCompanyValue(myRow)
                With myCompany
                    CompanyIDLabel.Text = .CompanyID
                    CompanyNameTextBox.Text = .CompanyNM
                    CityTextBox.Text = .SiteCity
                    StateOrProvinceTextBox.Text = .SiteState
                    PostalCodeTextBox.Text = .PostalCode
                    SiteURLTextBox.Text = .DomainName
                    CountryTextBox.Text = .SiteCountry
                    GalleryFolderTextBox.Text = .SiteGallery
                    SiteTitleTextBox.Text = .CompanyTitle
                    DefaultArticleIDDropDown.SelectedValue = .DefaultArticleID
                    SiteTemplateDropDown.SelectedValue = .SitePrefix
                    DefaultSiteTemplateDropDown.SelectedValue = .DefaultSitePrefix
                    HomePageIDDropDown.SelectedValue = .HomeLocationID
                    UseBreadCrumbURLCheckBox.Checked = .UseBreadCrumbURL
                    ActiveFLCheckBox.Checked = True
                    SingleSiteGalleryCheckBox.Checked = True
                End With
            Next
            ArticleList.PopulateCompanyArticleList(myCompanyID)
            wpm_LoadArticleDropDown(DefaultArticleIDDropDown, myCompany.DefaultArticleID, ArticleList, False)

            myCompany.Locations.GetLocationListFromDb("NAME", myCompanyID, 4)
            wpm_LoadLocationDropDown(HomePageIDDropDown, myCompany.HomeLocationID, myCompany.Locations, False)
        End If

        wpm_LoadCMB(SiteTemplateDropDown, myCompany.SitePrefix, STR_SELECTSiteTemplateList, "TemplatePrefix", "TemplatePrefix", True)
        wpm_LoadCMB(DefaultSiteTemplateDropDown, myCompany.DefaultSitePrefix, STR_SELECTSiteTemplateList, "TemplatePrefix", "TemplatePrefix", True)
    End Sub
    Private Function GetCompanyForUpdate() As Company
        Dim myCompany As New Company
        For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_CompanyByCompanyID, CompanyIDLabel.Text), "Company").Rows
            myCompany.SetCompanyValue(myRow)
        Next
        With myCompany
            .CompanyID = CompanyIDLabel.Text
            .CompanyNM = CompanyNameTextBox.Text
            .SiteCity = CityTextBox.Text
            .SiteState = StateOrProvinceTextBox.Text
            .PostalCode = PostalCodeTextBox.Text
            .DomainName = SiteURLTextBox.Text
            .SiteCountry = CountryTextBox.Text
            .SiteGallery = GalleryFolderTextBox.Text
            .CompanyTitle = SiteTitleTextBox.Text
            .SitePrefix = SiteTemplateDropDown.SelectedValue
            .DefaultSitePrefix = DefaultSiteTemplateDropDown.SelectedValue
            .HomeLocationID = wpm_GetDBInteger(HomePageIDDropDown.SelectedValue)
            .DefaultArticleID = wpm_GetDBInteger(DefaultArticleIDDropDown.SelectedValue)
            .UseBreadCrumbURL = UseBreadCrumbURLCheckBox.Checked
        End With
        Return myCompany
    End Function
    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        With GetCompanyForUpdate()
            Dim iRowsAffected As Integer = 0
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_Company}
                        wpm_AddParameterStringValue("@CompanyName", .CompanyNM, cmd)
                        wpm_AddParameterStringValue("@City", .SiteCity, cmd)
                        wpm_AddParameterStringValue("@Address", AddressTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@PostalCode", PostalCodeTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@StateOrProvince", .SiteState, cmd)
                        wpm_AddParameterStringValue("@Country", .SiteCountry, cmd)
                        wpm_AddParameterStringValue("@PhoneNumber", PhoneNumberTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@FaxNumber", FaxNumberTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@GalleryFolder", .SiteGallery, cmd)
                        wpm_AddParameterStringValue("@SiteURL", .CompanyURL, cmd)
                        wpm_AddParameterStringValue("@SiteTitle", .CompanyTitle, cmd)
                        wpm_AddParameterStringValue("@SiteTemplate", .SitePrefix, cmd)
                        wpm_AddParameterStringValue("@DefaultSiteTemplate", .DefaultSitePrefix, cmd)
                        wpm_AddParameterValue("@DefaultArticleID", .DefaultArticleID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@Component", .Component, cmd)
                        wpm_AddParameterStringValue("@FromEmail", .FromEmail, cmd)
                        wpm_AddParameterStringValue("@SMTP", .SMTP, cmd)
                        wpm_AddParameterValue("@HomePageID", .HomeLocationID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ActiveFL", ActiveFLCheckBox.Checked, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@UseBreadCrumbURL", .UseBreadCrumbURL, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@SingleSiteGallery", SingleSiteGalleryCheckBox.Checked, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@CompanyID", .CompanyID, SqlDbType.Int, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_UPDATE_Company, "Company.acsx - cmd_Update_Click")
                End Try
            End Using
        End With
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        With GetCompanyForUpdate()
            Dim iRowsAffected As Integer = 0
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_Company}
                        wpm_AddParameterStringValue("@CompanyName", .CompanyNM, cmd)
                        wpm_AddParameterStringValue("@City", .SiteCity, cmd)
                        wpm_AddParameterStringValue("@Address", AddressTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@PostalCode", PostalCodeTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@StateOrProvince", .SiteState, cmd)
                        wpm_AddParameterStringValue("@Country", .SiteCountry, cmd)
                        wpm_AddParameterStringValue("@PhoneNumber", PhoneNumberTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@FaxNumber", FaxNumberTextBox.Text, cmd)
                        wpm_AddParameterStringValue("@GalleryFolder", .SiteGallery, cmd)
                        wpm_AddParameterStringValue("@SiteURL", .CompanyURL, cmd)
                        wpm_AddParameterStringValue("@SiteTitle", .CompanyTitle, cmd)
                        wpm_AddParameterStringValue("@SiteTemplate", .SitePrefix, cmd)
                        wpm_AddParameterStringValue("@DefaultSiteTemplate", .DefaultSitePrefix, cmd)
                        wpm_AddParameterValue("@DefaultArticleID", .DefaultArticleID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@Component", .Component, cmd)
                        wpm_AddParameterStringValue("@FromEmail", .FromEmail, cmd)
                        wpm_AddParameterStringValue("@SMTP", .SMTP, cmd)
                        wpm_AddParameterValue("@HomePageID", .HomeLocationID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ActiveFL", ActiveFLCheckBox.Checked, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@UseBreadCrumbURL", .UseBreadCrumbURL, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@SingleSiteGallery", SingleSiteGalleryCheckBox.Checked, SqlDbType.Bit, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_INSERT_Company, "Company.acsx - cmd_Insert_Click")
                End Try
            End Using
        End With
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub
End Class

