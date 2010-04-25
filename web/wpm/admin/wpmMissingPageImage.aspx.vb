Imports System.Data.Common
Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

Partial Class wpm_MissingPageImage
    Inherits AspNetMaker7_WPMGen
    '
    ' ASP.NET Page_Load event
    '
    Public mySiteMap As wpmActiveSite
    Public myCatalog As wpmSiteImageList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = EW_RESPONSE_BUFFER
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        mySiteMap = New wpmActiveSite(Session)
        Session("ListPageURL") = ("~/wpm/admin/wpmMissingPageImage.aspx")
        Dim myLinkDirectory As New wpmLinkDirectory(mySiteMap)
        Dim myContents As New StringBuilder
        Dim myAdminForm As New StringBuilder
        wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/admin/MissingPageImage.text"), myContents)
        If GetProperty("DEL", "") <> "" Then
            DeleteImage(GetProperty("DEL", ""))
        End If

        If Request.ServerVariables("REQUEST_METHOD") = "POST" Then
            ProcessForm()
            Response.Redirect("/wpm/admin/wpmMissingPageImage.aspx")
        Else
            myAdminForm.Append("<form name=""frmPageImage"" method=""post"" action=""/wpm/admin/wpmMissingPageImage.aspx"">")
            myAdminForm.Append(GetPageList(mySiteMap.CurrentPageID, True))
            myAdminForm.Append("<input type=""submit"" value=""Add Checked to Page""><hr/>")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', true);"" value=""Check All"">&nbsp;&nbsp;")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', false);"" value=""UnCheck All""><br />")
            myAdminForm.Append(GetImageList(mySiteMap.SiteGallery))
            myAdminForm.Append("</form>")
            litOne.Text = myContents.ToString
            litTwo.Text = myAdminForm.ToString
            mySiteMap.ResetSessionVariables()
        End If


    End Sub

    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            wpmDB.RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            wpmDB.RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try
        Return result
    End Function

    Private Function ProcessForm() As String
        Dim Item As Object
        Dim sSQL As String
        Dim iPageImagePostion As Integer
        Dim iPageID As Integer
        Dim sPageID As String
        Dim myImage As wpmImage

        sPageID = wpmUTIL.GetDBString(Request.Form.GetValues("x_PageID")(0))
        If sPageID = String.Empty Then
            wpmUTIL.AuditLog("No PageID Received", "wpmMissingPageImage.ProcessForm")
            iPageID = 0
        Else
            iPageID = CInt(sPageID)
            sSQL = "SELECT Max(PageImage.PageImagePosition) FROM PageImage WHERE PageImage.PageID=" & iPageID & " "
            For Each myrow As DataRow In wpmDB.GetDataTable(sSQL, "wpmMissingPageImage.ProcessForm").Rows
                iPageImagePostion = wpmUTIL.GetDBInteger(myrow.Item(0))
                Exit For
            Next
            If Not IsNothing(Request.Form.GetValues("ImageCheckBox")) Then
                For Each Item In Request.Form.GetValues("ImageCheckBox")
                    iPageImagePostion = iPageImagePostion + 1
                    sSQL = "INSERT INTO PageImage ( PageID, ImageID, PageImagePosition ) " & _
                                          "SELECT " & iPageID & "," & Item & " ," & iPageImagePostion & ";"
                    wpmDB.RunInsertSQL(sSQL, "wpmMissingPageImage.ProcessForm")

                    ' myImage = New wpmImage(Item)
                    ' myImage.ImageFileName = ""
                    ' myImage.updateImage()
                Next
            End If
        End If

        ReviewImageCatalog()

        Return " "
    End Function

    Private Function GetPageList(ByVal sPageID As String, ByVal bRequired As Boolean) As String
        Dim sSQL As String = "SELECT PageID,PageName FROM Page WHERE Page.CompanyID=" & Me.mySiteMap.CompanyID & " order by PageName "
        Dim x_PageIDList As String = ""
        If (sPageID & "x") <> "x" Then
            sPageID = CInt(sPageID)
        End If
        If (bRequired) Then
            x_PageIDList = "<SELECT name='x_PageID'>"
        Else
            x_PageIDList = "<SELECT name='x_PageID'><OPTION value=''>Please Select</OPTION>"
        End If
        For Each location As wpmLocation In pageActiveSite.LocationList.GetCatalogLocations()
            x_PageIDList = x_PageIDList & "<OPTION value='" & location.PageID & "'"
            x_PageIDList = x_PageIDList & ">" & location.PageName & "</option>"
        Next
        x_PageIDList = x_PageIDList & "</SELECT>"
        Return x_PageIDList
    End Function

    Private Function GetImageList(ByVal sDirectory As String) As String
        Dim sReturn As String = ""
        Dim strSQL As String = "SELECT Image.ImageID, Image.ImageName, Image.ImageThumbFileName, Image.ImageFileName FROM [Image] LEFT JOIN PageImage ON Image.ImageID = PageImage.ImageID WHERE (((PageImage.PageImageID) Is Null)) and companyid=" & mySiteMap.CompanyID & " order by Image.ImageName "
        For Each myrow As DataRow In wpmDB.GetDataTable(strSQL, "GetImageList").Rows
            sReturn = sReturn & "<SPAN class=""thumb""><img src=""/wpm/catalog/ImageResize.aspx?w=200&img=" & mySiteMap.SiteGallery & myrow.Item(3) & """ alt=""" & myrow.Item(1) & _
            " (" & myrow.Item(0) & ") ""><br/>"
            sReturn = sReturn & "<label for=""ImageCheckBox""><input type=""CHECKBOX"" name=""ImageCheckBox"" value=""" & myrow.Item(0) & """ id=""ImageCheckBox" & myrow.Item(0) & """>" & myrow.Item(1) & "</label><br /></SPAN>"
        Next
        Return sReturn
    End Function
    Public Function ReviewImageCatalog() As Boolean
        Dim sNewPagePath As String = (String.Empty)
        Dim sNewImagePath As String = (String.Empty)
        Dim sCurImagePath As String = (String.Empty)
        Dim dtPageImage As DataTable = wpmDataCon.GetPageImage(mySiteMap.CompanyID)

        For Each myRow As DataRow In dtPageImage.Rows
            ' Determine Folder for Page 
            sNewPagePath = (HttpContext.Current.Server.MapPath(pageActiveSite.SiteGallery & "image/" & wpmUTIL.FixInvalidCharacters(wpmUTIL.GetDBString(myRow.Item("PageName")))))

            sCurImagePath = HttpContext.Current.Server.MapPath(pageActiveSite.SiteGallery & wpmUTIL.GetDBString(myRow.Item("ImageFileName")))

            If Left(sCurImagePath, Len(sNewPagePath)) = sNewPagePath Then
                sNewImagePath = String.Empty
            Else
                sNewImagePath = "image/" & wpmUTIL.FixInvalidCharacters(wpmUTIL.GetDBString(myRow.Item("PageName"))) & "/" & wpmUTIL.GetDBString(myRow.Item("ImageFileName"))
            End If

            If sNewImagePath <> String.Empty Then
                ' If wpmFileIO.MoveFile(HttpContext.Current.Server.MapPath(pageActiveSite.SiteGallery & wpmUTIL.GetDBString(myRow.Item("ImageFileName"))), sNewPagePath & "/" & wpmUTIL.GetDBString(myRow.Item("ImageFileName"))) Then
                wpmUTIL.WriteLog(sNewImagePath, sCurImagePath, HttpContext.Current.Server.MapPath("~/access_db/log/PageImage_Issues.csv"))
            End If


        Next
    End Function


End Class
