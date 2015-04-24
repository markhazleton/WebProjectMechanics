Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_SiteType
    Inherits ApplicationUserControl

    ' SiteType
    Public Const STR_SiteTypeID As String = "SiteTypeID"




    Public Const STR_SELECTSiteTypeList As String = "SELECT SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null) AS DefaultSiteCategoryNM, Count(SiteCategory_1.SiteCategoryID) AS SiteLocationCount, SiteCategoryType.SiteCategoryComment, SiteCategoryType.SiteCategoryFileName, SiteCategoryType.SiteCategoryTransferURL  FROM SiteCategoryType AS SiteCategoryType_1 RIGHT JOIN ((SiteCategory RIGHT JOIN SiteCategoryType ON SiteCategory.SiteCategoryID = SiteCategoryType.DefaultSiteCategoryID) LEFT JOIN SiteCategory AS SiteCategory_1 ON SiteCategoryType.SiteCategoryTypeID = SiteCategory_1.SiteCategoryTypeID) ON SiteCategoryType_1.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID  GROUP BY SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null), SiteCategoryType.SiteCategoryComment, SiteCategoryType.SiteCategoryFileName, SiteCategoryType.SiteCategoryTransferURL;  "


    Public Const STR_SELECTSiteTypeBySiteTypeID As String = "SELECT SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null) AS DefaultSiteCategoryNM, Count(SiteCategory_1.SiteCategoryID) AS SiteLocationCount, SiteCategoryType.SiteCategoryComment, SiteCategoryType.SiteCategoryFileName, SiteCategoryType.SiteCategoryTransferURL  FROM SiteCategoryType AS SiteCategoryType_1 RIGHT JOIN ((SiteCategory RIGHT JOIN SiteCategoryType ON SiteCategory.SiteCategoryID = SiteCategoryType.DefaultSiteCategoryID) LEFT JOIN SiteCategory AS SiteCategory_1 ON SiteCategoryType.SiteCategoryTypeID = SiteCategory_1.SiteCategoryTypeID) ON SiteCategoryType_1.SiteCategoryTypeID = SiteCategory.SiteCategoryTypeID where SiteCategoryType.SiteCategoryTypeID={0} GROUP BY SiteCategoryType.SiteCategoryTypeID, SiteCategoryType.SiteCategoryTypeNM, SiteCategoryType.SiteCategoryTypeDS, SiteCategoryType.DefaultSiteCategoryID, IIf(SiteCategoryType_1.SiteCategoryTypeID=SiteCategoryType.SiteCategoryTypeID,SiteCategory.CategoryName,Null), SiteCategoryType.SiteCategoryComment, SiteCategoryType.SiteCategoryFileName, SiteCategoryType.SiteCategoryTransferURL;  "

    Public Const STR_UPDATE_SiteType As String = "UPDATE SiteCategoryType SET SiteCategoryType.SiteCategoryTypeNM = @SiteCategoryType, SiteCategoryType.SiteCategoryTypeDS = @SiteCategoryTypeDS, SiteCategoryType.SiteCategoryComment = @SiteCategoryComment, SiteCategoryType.SiteCategoryFileName = @SiteCategoryFileName, SiteCategoryType.SiteCategoryTransferURL = @SiteCategoryTransferURL, SiteCategoryType.DefaultSiteCategoryID = @DefaultSiteCategoryID where SiteCategoryType.SiteCategoryTypeID=@SiteCategoryTypeID ;"

    Public Const STR_INSERT_SiteType As String = "INSERT INTO [Company] ([CompanyName], [City], [Address], [PostalCode], [StateOrProvince], [Country], [PhoneNumber], [FaxNumber], [DefaultPaymentTerms], [DefaultInvoiceDescription], [GalleryFolder], [SiteURL], [SiteTitle], [SiteTemplate], [DefaultSiteTemplate], [DefaultArticleID], [Component], [FromEmail], [SMTP], [HomePageID], [ActiveFL], [UseBreadCrumbURL], [SingleSiteGallery], [SiteCategoryTypeID]) VALUES (@CompanyName, @City, @Address, @PostalCode, @StateOrProvince, @Country, @PhoneNumber, @FaxNumber, @DefaultPaymentTerms, @DefaultInvoiceDescription, @GalleryFolder, @SiteURL, @SiteTitle, @SiteTemplate, @DefaultSiteTemplate, @DefaultArticleID, @Component, @FromEmail, @SMTP, @HomePageID, @ActiveFL, @UseBreadCrumbURL, @SingleSiteGallery, @SiteCategoryTypeID);"
    Public Const STR_DELETE_SiteType As String = "DELETE FROM [SiteCategoryType] WHERE [SiteCategoryType].[SiteCategoryTypeID]=@SiteCategoryTypeID;"


    Public Const STR_SiteCategoryBySiteType As String = "SELECT SiteCategory.SiteCategoryID, SiteCategory.CategoryKeywords, SiteCategory.CategoryName, SiteCategory.CategoryTitle, SiteCategory.CategoryDescription, SiteCategory.GroupOrder, SiteCategory.ParentCategoryID, SiteCategory.CategoryFileName, SiteCategory.SiteCategoryTypeID, SiteCategory.SiteCategoryGroupID, SiteCategoryGroup.SiteCategoryGroupNM FROM SiteCategoryGroup RIGHT JOIN (SiteCategory AS ParentSiteCategory RIGHT JOIN SiteCategory ON ParentSiteCategory.SiteCategoryID = SiteCategory.ParentCategoryID) ON SiteCategoryGroup.SiteCategoryGroupID = SiteCategory.SiteCategoryGroupID WHERE (((SiteCategory.SiteCategoryTypeID)={0} )); "


    Private Property reqSiteTypeID As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteTypeID = GetProperty(STR_SiteTypeID, String.Empty)
        If Not IsPostBack Then
            If reqSiteTypeID <> String.Empty Then
                GetSiteTypeForEdit(reqSiteTypeID)

                If reqSiteTypeID = "NEW" Or reqSiteTypeID = 0 Then
                    dtList.Visible = False
                    pnlEdit.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Update.Visible = False
                    cmd_Cancel.Visible = True
                    cmd_Insert.Visible = True
                    ' Insert Mode
                Else
                    dtList.Visible = False
                    pnlEdit.Visible = True
                    cmd_Delete.Visible = True
                    cmd_Update.Visible = True
                    cmd_Cancel.Visible = True
                    cmd_Insert.Visible = False
                    ' Edit Mode


                    dtSiteTypeUsage.Visible = True
                    Dim myListHeader As New DisplayTableHeader() With {.TableTitle = String.Format("Categories ( <a href='/admin/maint/default.aspx?type=Location&LocationID=NEW&SiteTypeID={0}'>New Category</a>)", reqSiteTypeID)}

                    myListHeader.AddHeaderItem("LocationTitle", "LocationTitle")
                    myListHeader.AddHeaderItem("LocationKeywords", "LocationKeywords")
                    myListHeader.AddHeaderItem("DefaultOrder", "DefaultOrder")
                    myListHeader.AddHeaderItem("LocationGroupNM", "LocationGroupNM")


                    myListHeader.DetailKeyName = "LocationID"
                    myListHeader.DetailFieldName = "LocationName"
                    myListHeader.DetailPath = "/admin/maint/default.aspx?type=Location&LocationID={0}"
                    Dim myList As New List(Of Object)

                    For Each myRow In wpm_GetDataTable(String.Format(STR_SiteCategoryBySiteType, reqSiteTypeID), "SiteCategory").Rows

                        myList.Add(New Location With {.LocationID = "CAT-" & wpm_GetDBInteger(myRow("SiteCategoryID")),
                                                      .LocationName = wpm_GetDBString(myRow("CategoryName")),
                                                      .LocationKeywords = wpm_GetDBString(myRow("CategoryKeywords")),
                                                      .LocationTitle = wpm_GetDBString(myRow("CategoryTitle")),
                                                      .DefaultOrder = wpm_GetDBInteger(myRow("GroupOrder")),
                                                      .LocationGroupNM = wpm_GetDBString(myRow("SiteCategoryGroupNM")),
                                                      .LocationGroupID = wpm_GetDBInteger(myRow("SiteCategoryGroupID"))
                                                     })
                    Next
                    dtSiteTypeUsage.BuildTable(myListHeader, myList)



                End If
            Else
                ' Show the list
                dtList.Visible = True
                pnlEdit.Visible = False

                Dim myTemplateList As New List(Of SiteType)
                For Each myRow In wpm_GetDataTable(STR_SELECTSiteTypeList, "SiteType").Rows
                    myTemplateList.Add(New SiteType With {.SiteTypeID = wpm_GetDBString(myRow("SiteCategoryTypeID")),
                                                          .SiteTypeNM = wpm_GetDBString(myRow("SiteCategoryTypeNM")),
                                                          .SiteTypeDS = wpm_GetDBString(myRow("SiteCategoryTypeDS")),
                                                          .DefaultSiteTypeLocationID = wpm_GetDBString(myRow("DefaultSiteCategoryID")),
                                                          .DefaultSiteTypeLocationNM = wpm_GetDBString(myRow("DefaultSiteCategoryNM")),
                                                          .SiteTypeLocationCount = wpm_GetDBInteger(myRow("SiteLocationCount"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site Type", .DetailKeyName = "SiteTypeID", .DetailFieldName = "SiteTypeNM", .DetailPath = "/admin/maint/default.aspx?type=SiteType&SiteTypeID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "Description", .Value = "SiteTypeDS"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "DefaultSiteTypeLocationNM", .Value = "DefaultSiteTypeLocationNM"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "SiteTypeLocationCount", .Value = "SiteTypeLocationCount"})

                dtList.BuildTable(myListHeader, myTemplateList)
            End If
        End If
    End Sub
    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim iRowsAffected As Integer = 0
        Dim mySiteType As SiteType = GetSiteTypeBySiteTypeID(wpm_GetDBString(litSiteTypeID.Text))
        GetSiteTypeForUpdate(mySiteType)
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_SiteType}
                    wpm_AddParameterStringValue("@SiteCategoryTypeNM", mySiteType.SiteTypeNM, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryTypeDS", mySiteType.SiteTypeDS, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryComment", mySiteType.SiteTypeComment, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryFileName", mySiteType.SiteTypeFileName, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryTransferURL", mySiteType.SiteTypeTransferURL, cmd)
                    wpm_AddParameterStringValue("@DefaultSiteCategoryID", mySiteType.DefaultSiteTypeLocationID, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryTypeID", mySiteType.SiteTypeID, cmd)
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
        Dim mySiteType As SiteType = GetSiteTypeBySiteTypeID(wpm_GetDBString(litSiteTypeID.Text))
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_SiteType}
                    wpm_AddParameterStringValue("@SiteCategoryTypeID", mySiteType.SiteTypeID, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_SiteType, "SiteType.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub
    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub




    Private Function GetSiteTypeBySiteTypeID(ByVal reqSiteTypeID As String) As SiteType
        Dim mySiteType As New SiteType With {.SiteTypeID = reqSiteTypeID}
        If IsNumeric(reqSiteTypeID) AndAlso CInt(reqSiteTypeID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECTSiteTypeBySiteTypeID, reqSiteTypeID), "SiteType").Rows
                With mySiteType
                    .SiteTypeID = wpm_GetDBString(myRow("SiteCategoryTypeID"))
                    .SiteTypeNM = wpm_GetDBString(myRow("SiteCategoryTypeNM"))
                    .SiteTypeDS = wpm_GetDBString(myRow("SiteCategoryTypeDS"))
                    .SiteTypeComment = wpm_GetDBString(myRow("SiteCategoryComment"))
                    .SiteTypeFileName = wpm_GetDBString(myRow("SiteCategoryFileName"))
                    .SiteTypeTransferURL = wpm_GetDBString(myRow("SiteCategoryTransferURL"))
                    .DefaultSiteTypeLocationID = wpm_GetDBString(myRow("DefaultSiteCategoryID"))
                End With
                Exit For
            Next
        Else
            mySiteType.SiteTypeID = "0"
        End If
        Return mySiteType
    End Function
    Private Sub GetSiteTypeForUpdate(ByRef mySiteType As SiteType)
        mySiteType.SiteTypeNM = tbSiteTypeNM.Text
        mySiteType.SiteTypeDS = tbSiteTypeDS.Text
        mySiteType.SiteTypeNM = tbSiteTypeNM.Text
        mySiteType.SiteTypeID = litSiteTypeID.Text
        mySiteType.SiteTypeComment = tbSiteTypeComment.Text
        mySiteType.SiteTypeFileName = tbSiteTypeFileName.Text
        mySiteType.SiteTypeID = litSiteTypeID.Text
    End Sub

    Private Sub GetSiteTypeForEdit(reqSiteTypeID As String)
        Dim mySiteType As SiteType = GetSiteTypeBySiteTypeID(reqSiteTypeID)
        tbSiteTypeNM.Text = mySiteType.SiteTypeNM
        tbSiteTypeDS.Text = mySiteType.SiteTypeDS
        tbSiteTypeNM.Text = mySiteType.SiteTypeNM
        tbSiteTypeComment.Text = mySiteType.SiteTypeComment
        tbSiteTypeFileName.Text = mySiteType.SiteTypeFileName
        litSiteTypeID.Text = mySiteType.SiteTypeID
    End Sub



End Class
