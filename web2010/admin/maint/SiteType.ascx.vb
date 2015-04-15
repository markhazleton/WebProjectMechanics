Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_SiteType
    Inherits ApplicationUserControl

    ' SiteType
    Public Const STR_SiteTypeID As String = "SiteTypeID"
    Public Const STR_SELECTSiteTypeList As String = "SELECT SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null) AS DefaultSiteCategoryNM, Count(SiteCategory_1.SiteCategoryID) AS SiteLocationCount  FROM (SiteCategoryType AS SiteCategoryType_1 RIGHT JOIN (SiteCategory RIGHT JOIN SiteCategoryType ON SiteCategory.SiteCategoryID = SiteCategoryType.DefaultSiteCategoryID) ON SiteCategoryType_1.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID) INNER JOIN SiteCategory AS SiteCategory_1 ON SiteCategoryType.SiteCategoryTypeID = SiteCategory_1.SiteCategoryTypeID GROUP BY SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null);"
    Public Const STR_UPDATE_SiteType As String = "UPDATE [Company] SET [CompanyName] = @CompanyName, [City] = @City, [Address] = @Address, [PostalCode] = @PostalCode, [StateOrProvince] = @StateOrProvince, [Country] = @Country, [PhoneNumber] = @PhoneNumber, [FaxNumber] = @FaxNumber, [DefaultPaymentTerms] = @DefaultPaymentTerms, [DefaultInvoiceDescription] = @DefaultInvoiceDescription, [GalleryFolder] = @GalleryFolder, [SiteURL] = @SiteURL, [SiteTitle] = @SiteTitle, [SiteTemplate] = @SiteTemplate, [DefaultSiteTemplate] = @DefaultSiteTemplate, [DefaultArticleID] = @DefaultArticleID, [Component] = @Component, [FromEmail] = @FromEmail, [SMTP] = @SMTP, [HomePageID] = @HomePageID, [ActiveFL] = @ActiveFL, [UseBreadCrumbURL] = @UseBreadCrumbURL, [SingleSiteGallery] = @SingleSiteGallery, [SiteCategoryTypeID] = @SiteCategoryTypeID WHERE [CompanyID] = @CompanyID;"
    Public Const STR_INSERT_SiteType As String = "INSERT INTO [Company] ([CompanyName], [City], [Address], [PostalCode], [StateOrProvince], [Country], [PhoneNumber], [FaxNumber], [DefaultPaymentTerms], [DefaultInvoiceDescription], [GalleryFolder], [SiteURL], [SiteTitle], [SiteTemplate], [DefaultSiteTemplate], [DefaultArticleID], [Component], [FromEmail], [SMTP], [HomePageID], [ActiveFL], [UseBreadCrumbURL], [SingleSiteGallery], [SiteCategoryTypeID]) VALUES (@CompanyName, @City, @Address, @PostalCode, @StateOrProvince, @Country, @PhoneNumber, @FaxNumber, @DefaultPaymentTerms, @DefaultInvoiceDescription, @GalleryFolder, @SiteURL, @SiteTitle, @SiteTemplate, @DefaultSiteTemplate, @DefaultArticleID, @Component, @FromEmail, @SMTP, @HomePageID, @ActiveFL, @UseBreadCrumbURL, @SingleSiteGallery, @SiteCategoryTypeID);"
    Public Const STR_DELETE_SiteType As String = "DELETE FROM [SiteCategoryType] WHERE [SiteCategoryType].[SiteCategoryTypeID]={0};"

    Private Property reqSiteTypeID As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteTypeID = GetProperty(STR_SiteTypeID, String.Empty)
        If Not IsPostBack Then
            If reqSiteTypeID <> String.Empty Then
                If reqSiteTypeID = "NEW" Or reqSiteTypeID = 0 Then
                    dtList.Visible = False
                    ' Insert Mode
                Else
                    dtList.Visible = False
                    ' Edit Mode
                    dtList.Visible = False
                End If
            Else
                ' Show the list
                dtList.Visible = True

                Dim myTemplateList As New List(Of SiteType)
                For Each myRow In wpm_GetDataTable(STR_SELECTSiteTypeList, "SiteType").Rows
                    myTemplateList.Add(New SiteType With {.SiteTypeID = wpm_GetDBString(myRow("SiteCategoryTypeID")),
                                                          .SiteTypeNM = wpm_GetDBString(myrow("SiteCategoryTypeNM")),
                                                          .SiteTypeDS = wpm_GetDBString(myrow("SiteCategoryTypeDS")),
                                                          .DefaultSiteTypeLocationID = wpm_GetDBString(myrow("DefaultSiteCategoryID")),
                                                          .DefaultSiteTypeLocationNM = wpm_GetDBString(myrow("DefaultSiteCategoryNM")),
                                                          .SiteTypeLocationCount = wpm_GetDBInteger(myrow("SiteLocationCount"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site Type", .DetailKeyName = "SiteTypeID", .DetailFieldName = "SiteTypeNM", .DetailPath = "/admin/maint/default.aspx?type=SiteType&SiteTypeID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "Description",.Value = "SiteTypeDS"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "DefaultSiteTypeLocationNM",.Value = "DefaultSiteTypeLocationNM"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "SiteTypeLocationCount",.Value = "SiteTypeLocationCount"})

                dtList.BuildTable(myListHeader,myTemplateList)
            End If
        End If
    End Sub
    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
            Dim iRowsAffected As Integer = 0
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_SiteType}
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_UPDATE_SiteType, "SiteType.acsx - cmd_Update_Click")
                End Try
            End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
            Dim iRowsAffected As Integer = 0
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteType}
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(STR_INSERT_SiteType, "SiteType.acsx - cmd_Insert_Click")
                End Try
            End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub



End Class
