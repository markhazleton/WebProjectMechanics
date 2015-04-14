Imports System.Data.OleDb

Public Module LocationBuildHTML
    Public Function wpm_GetHTMLFilePath(ByVal HTMLFileName As String) As String
        If Not FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "sites") Then
            FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "sites")
        End If
        If Not FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, wpm_HostName())) Then
            FileProcessing.CreateFolder(String.Format("{0}sites\{1}", wpm_SiteConfig.ConfigFolderPath, wpm_HostName()))
        End If
        If Not FileProcessing.VerifyFolderExists(String.Format("{0}sites\{1}\html", wpm_SiteConfig.ConfigFolderPath, wpm_HostName())) Then
            FileProcessing.CreateFolder(String.Format("{0}sites\{1}\html", wpm_SiteConfig.ConfigFolderPath, wpm_HostName()))
        End If
        Return String.Format("{0}sites\{1}\html\{2}", wpm_SiteConfig.ConfigFolderPath, wpm_HostName(), HTMLFileName)
    End Function
    Public Function wpm_SaveHTML(ByVal sPageName As String, ByVal sContent As String) As Boolean
        Dim sPath As String = wpm_GetHTMLFilePath(wpm_RemoveInvalidCharacters(sPageName))
        Dim bReturn As Boolean = False
        If Trim(sPageName) <> "" Then
            sPath = sPath & ".html"
            bReturn = FileProcessing.CreateFile(sPath, sContent)
        End If
        Return bReturn
    End Function
    Public Function wpm_BuildNaviagtionLink(ByRef myLoc As Location, ByVal IsSelected As Boolean) As String
        If (IsSelected) Then
            Return wpm_BuildClassLink(myLoc, "selected")
        Else
            Return wpm_BuildClassLink(myLoc, "")
        End If
    End Function
    Public Function wpm_BuildClassLink(ByRef myLoc As Location, ByVal LinkClass As String) As String
        If LinkClass = String.Empty Then
            Return (String.Format("<a href=""{0}""><span>{1}</span></a>", wpm_GetSiteMapRowURL(myLoc, False), myLoc.LocationName))
        Else
            Return (String.Format("<a class=""{0}"" href=""{1}""><span>{2}</span></a>", LinkClass, wpm_GetSiteMapRowURL(myLoc, False), myLoc.LocationName))
        End If
    End Function
    Public Function wpm_GetSiteMapRowURL(ByVal myLoc As Location, ByRef UseBreadCrumbURL As Boolean) As String
        If StrConv(Left(myLoc.TransferURL, 4), VbStrConv.Lowercase) = "http" Then
            Return myLoc.TransferURL
        Else
            If wpm_SiteConfig.Use404Processing Then
                If UseBreadCrumbURL Then
                    Return myLoc.BreadCrumbURL
                Else
                    Return myLoc.LocationURL
                End If
            Else
                Return myLoc.TransferURL
            End If
        End If
    End Function
    Public Function wpm_BuildClassLink(ByVal myLoc As Location, ByVal LinkClass As String, ByRef UseBreadcrumbURL As Boolean) As String
        If UseBreadcrumbURL Then
            Return (String.Format("<a class=""{0}"" href=""{1}"">{2}</a>", LinkClass, myLoc.BreadCrumbURL, myLoc.LocationName))
        Else
            Return (String.Format("<a class=""{0}"" href=""{1}"">{2}</a>", LinkClass, myLoc.LocationURL, myLoc.LocationName))
        End If
    End Function



    Public Function wpm_DeletePageDB(ByVal myLoc As Location) As Boolean
        Dim result As Boolean = False
        If myLoc.RecordSource = "Page" Then
            If wpm_RunDeleteSQL("delete from page where pageid=" & myLoc.LocationID, "Page") > 0 Then
                result = True
            End If
        End If
        Return result
    End Function

    Public Function wpm_UpdateLocation(ByVal myLoc As Location, ByVal CompanyID As Integer, ByVal GroupID As Integer) As Boolean
        Dim bReturn As Boolean = False
        Dim iRowsAffected As Integer = 0
        Select Case myLoc.RecordSource
            Case "Page"
                If CInt(myLoc.LocationID) > 0 Then
                    wpm_UpdatePageDB(myLoc, CompanyID, GroupID, iRowsAffected)
                Else
                    wpm_InsertPageDB(myLoc, CompanyID, GroupID, iRowsAffected)
                End If
            Case "Category"
                If wpm_RunUpdateSQL(String.Format(" UPDATE SiteCategory SET CategoryKeywords = '{0}', CategoryName = '{1}', CategoryTitle = '{2}', CategoryDescription = '{3}', GroupOrder = {4}, ParentCategoryID = {5}, SiteCategoryGroupID = {6} WHERE (SiteCategory.SiteCategoryID = {7}) ", myLoc.LocationKeywords, myLoc.LocationName, myLoc.LocationTitle, myLoc.LocationDescription, myLoc.DefaultOrder, wpm_GetNullableIndex(myLoc.ParentCategoryID), wpm_GetNullableIndex(myLoc.LocationGroupID), wpm_GetNullableIndex(myLoc.GetSiteCategoryID)), "Page") = 1 Then
                    bReturn = True
                Else
                    bReturn = False
                End If
            Case Else
                bReturn = False
        End Select
        Return bReturn
    End Function
    Public Sub wpm_UpdatePageDB(ByVal myLoc As Location, ByVal CompanyID As Integer, ByVal GroupID As Integer, ByRef iRowsAffected As Integer)
        Const sSQL As String = "UPDATE [Page] SET [PageOrder]=@PageOrder, [PageName]=@PageName , [PageTitle]=@PageTitle , [PageDescription]=@PageDescription , [PageKeywords]=@PageKeywords , [PageTypeID]=@PageTypeID ,  [ParentPageID]=@ParentPageID, [Active]=@Active, [AllowMessage]=@AllowMessage , [CompanyID]=@CompanyID , [PageFileName]=@PageFileName,  [GroupID]=@GroupID , [ModifiedDT]=now() , [VersionNo]=[VersionNo]+1 , [SiteCategoryID]=@SiteCategoryID , [SiteCategoryGroupID]=@SiteCategoryGroupID, [RowsPerPage]=@RowsPerPage, [ImagesPerRow]=@ImagesPerRow   where [PageID]=@PageID"
        With myLoc
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterValue("@PageOrder", .DefaultOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@PageName", .LocationName, cmd)
                        wpm_AddParameterStringValue("@PageTitle", .LocationTitle, cmd)
                        wpm_AddParameterStringValue("@PageDescription", .LocationDescription, cmd)
                        wpm_AddParameterStringValue("@PageKeywords", .LocationKeywords, cmd)
                        wpm_AddParameterValue("@PageTypeID", .LocationTypeID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ParentPageID", .ParentPageID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@Active", .ActiveFL, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@AllowMessage", False, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@PageFileName", .LocationFileName, cmd)
                        wpm_AddParameterValue("@GroupID", GroupID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@SiteCategoryID", .SiteCategoryID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@SiteCategoryGroupID", wpm_GetDBInteger(.LocationGroupID), SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@RowsPerPage", wpm_GetDBInteger(.RowsPerPage), SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ImagesPerRow", wpm_GetDBInteger(.ImagesPerRow), SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@PageID", .LocationID, SqlDbType.Int, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLUpdateError(cmd.CommandText, "Location.UpdatePageDB")
                    End Try
                End Using
            End Using
        End With
    End Sub
    Public Sub wpm_InsertPageDB(ByVal myLoc As Location, ByVal CompanyID As Integer, ByVal GroupID As Integer, ByRef iRowsAffected As Integer)
        Const sSQL As String = "Insert Into [Page] ([PageOrder], [PageName], [PageTitle], [PageDescription], [PageKeywords], [PageTypeID], [ImagesPerRow], [RowsPerPage], [ParentPageID], [Active], [AllowMessage], [CompanyID], [PageFileName],  [GroupID], [ModifiedDT], [VersionNo], [SiteCategoryID], [SiteCategoryGroupID]) values (@PageOrder, @PageName , @PageTitle , @PageDescription , @PageKeywords , @PageTypeID , @ImagesPerRow , @RowsPerPage, @ParentPageID, @Active, @AllowMessage , @CompanyID , @PageFileName,  @GroupID , now() , 1 , @SiteCategoryID , @SiteCategoryGroupID)"
        With myLoc
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterValue("@PageOrder", .DefaultOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@PageName", .LocationName, cmd)
                        wpm_AddParameterStringValue("@PageTitle", .LocationTitle, cmd)
                        wpm_AddParameterStringValue("@PageDescription", .LocationDescription, cmd)
                        wpm_AddParameterStringValue("@PageKeywords", .LocationKeywords, cmd)
                        wpm_AddParameterValue("@PageTypeID", .LocationTypeID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ImagesPerRow", .ImagesPerRow, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@RowsPerPage", .RowsPerPage, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ParentPageID", .ParentPageID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@Active", .ActiveFL, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@AllowMessage", False, SqlDbType.Bit, cmd)
                        wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@PageFileName", .LocationFileName, cmd)
                        wpm_AddParameterValue("@GroupID", GroupID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@SiteCategoryID", .SiteCategoryID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@SiteCategoryGroupID", .LocationGroupID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@PageID", .LocationID, SqlDbType.Int, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLUpdateError(cmd.CommandText, "Location.UpdatePageDB")
                    End Try
                End Using
            End Using
        End With
    End Sub

    Public Function wpm_UpdateDefaultOrder(ByRef myLoc As Location) As Boolean
        Dim bReturn As Boolean = False
        Select Case myLoc.RecordSource
            Case "Page"
                If wpm_RunUpdateSQL(String.Format("Update [Page] Set [Page].[PageOrder] = {0} where [Page].[PageID] = {1}", myLoc.DefaultOrder, myLoc.LocationID), "Page") = 1 Then
                    bReturn = True
                Else
                    bReturn = False
                End If
            Case Else
                bReturn = False
        End Select
        Return bReturn
    End Function


End Module
