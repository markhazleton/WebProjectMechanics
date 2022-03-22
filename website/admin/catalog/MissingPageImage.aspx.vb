Imports System.Data
Imports WebProjectMechanics
Imports System

Partial Class MissingPageImage
    Inherits AdminPage
    Public myCatalog As LocationImageList
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        wpm_ListPageURL = ("~/admin/catalog/MissingPageImage.aspx")
        Dim myContents As New StringBuilder
        Dim myAdminForm As New StringBuilder
        If wpm_GetProperty("DEL", "") <> "" Then
            DeleteImage(wpm_GetProperty("DEL", ""))
        End If
        If Request.ServerVariables("REQUEST_METHOD") = "POST" Then
            ProcessForm()
            Response.Redirect("/admin/catalog/MissingPageImage.aspx")
        Else
            myAdminForm.Append("<form name=""frmPageImage"" method=""post"" action=""/admin/catalog/MissingPageImage.aspx"">")
            myAdminForm.Append(GetPageList(masterPage.myCompany.CurrentLocationID, True))
            myAdminForm.Append("<input type=""submit"" value=""Add Checked to Page""><hr/>")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', true);"" value=""Check All"">&nbsp;&nbsp;")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', false);"" value=""UnCheck All""><br />")
            myAdminForm.Append(GetImageList())
            myAdminForm.Append("</form>")
            litOne.Text = myContents.ToString
            litTwo.Text = myAdminForm.ToString
            wpm_ResetSessionVariables()
        End If
    End Sub

    Public Shared Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            wpm_RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            ApplicationLogging.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            wpm_RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            ApplicationLogging.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try
        Return result
    End Function

    Private Function ProcessForm() As String
        Dim Item As Object
        Dim sSQL As String
        Dim iPageImagePostion As Integer
        Dim iPageID As Integer
        Dim sPageID As String = wpm_GetDBString(Request.Form.GetValues("x_PageID")(0))
        If sPageID = String.Empty Then
            ApplicationLogging.AuditLog("No PageID Received", "MissingPageImage.ProcessForm")
            iPageID = 0
        Else
            iPageID = CInt(sPageID)
            sSQL = String.Format("SELECT Max(PageImage.PageImagePosition) FROM PageImage WHERE PageImage.PageID={0} ", iPageID)
            For Each myrow As DataRow In wpm_GetDataTable(sSQL, "MissingPageImage.ProcessForm").Rows
                iPageImagePostion = wpm_GetDBInteger(myrow.Item(0))
                Exit For
            Next
            If Not IsNothing(Request.Form.GetValues("ImageCheckBox")) Then
                For Each Item In Request.Form.GetValues("ImageCheckBox")
                    iPageImagePostion = iPageImagePostion + 1
                    sSQL = String.Format("INSERT INTO PageImage ( PageID, ImageID, PageImagePosition ) SELECT {0},{1} ,{2};", iPageID, Item, iPageImagePostion)
                    wpm_RunInsertSQL(sSQL, "MissingPageImage.ProcessForm")
                Next
            End If
        End If
        ReviewImageCatalog()
        Return " "
    End Function

    Private Function GetPageList(ByVal sPageID As String, ByVal bRequired As Boolean) As String
        Dim sSQL As String = String.Format("SELECT PageID,PageName FROM Page WHERE Page.CompanyID={0} order by PageName ", wpm_CurrentSiteID)
        Dim x_PageIDList As String = ""
        If (sPageID & "x") <> "x" Then
            sPageID = CInt(sPageID)
        End If
        If (bRequired) Then
            x_PageIDList = "<SELECT name='x_PageID'>"
        Else
            x_PageIDList = "<SELECT name='x_PageID'><OPTION value=''>Please Select</OPTION>"
        End If
        For Each location As Location In ( From i In masterPage.myCompany.LocationList.GetCatalogLocations Order By i.LocationName Select i).ToArray
            x_PageIDList = String.Format("{0}<OPTION value='{1}'", x_PageIDList, location.LocationID)
            x_PageIDList = String.Format("{0}>{1}</option>", x_PageIDList, location.LocationName)
        Next
        x_PageIDList = x_PageIDList & "</SELECT>"
        Return x_PageIDList
    End Function

    Private Function GetImageList() As String
        Dim sReturn As String = ""
        Dim strSQL As String = String.Format("SELECT Image.ImageID, Image.ImageName, Image.ImageThumbFileName, Image.ImageFileName FROM [Image] LEFT JOIN PageImage ON Image.ImageID = PageImage.ImageID WHERE (((PageImage.PageImageID) Is Null)) and companyid={0} order by Image.ImageName ", wpm_CurrentSiteID)
        For Each myrow As DataRow In wpm_GetDataTable(strSQL, "GetImageList").Rows
            sReturn = String.Format("{0}<SPAN class=""thumb""><img src=""/runtime/catalog/FindImage.ashx?h=200&w=200&img={1}{2}"" alt=""{3} ({4}) ""><br/>", sReturn, wpm_SiteGallery, myrow.Item(3), myrow.Item(1), myrow.Item(0))
            sReturn = String.Format("{0}<label for=""ImageCheckBox""><input type=""CHECKBOX"" name=""ImageCheckBox"" value=""{1}"" id=""ImageCheckBox{1}"">{2}</label><br /></SPAN>", sReturn, myrow.Item(0), myrow.Item(1))
        Next
        Return sReturn
    End Function
    Public Function ReviewImageCatalog() As Boolean
        Dim sNewPagePath As String = (String.Empty)
        Dim sNewImagePath As String = (String.Empty)
        Dim sCurImagePath As String = (String.Empty)
        Using dtPageImage As DataTable = ApplicationDAL.GetPageImageByCompany(wpm_CurrentSiteID)
            For Each myRow As DataRow In dtPageImage.Rows
                ' Determine Folder for Page 
                sNewPagePath = (HttpContext.Current.Server.MapPath(String.Format("{0}image/{1}", wpm_SiteGallery, wpm_FixInvalidCharacters(wpm_GetDBString(myRow.Item("PageName"))))))
                sCurImagePath = HttpContext.Current.Server.MapPath(wpm_SiteGallery & wpm_GetDBString(myRow.Item("ImageFileName")))
                If Left(sCurImagePath, Len(sNewPagePath)) = sNewPagePath Then
                    sNewImagePath = String.Empty
                Else
                    sNewImagePath = String.Format("image/{0}/{1}", wpm_FixInvalidCharacters(wpm_GetDBString(myRow.Item("PageName"))), wpm_GetDBString(myRow.Item("ImageFileName")))
                End If
                If sNewImagePath <> String.Empty Then
                    ApplicationLogging.ErrorLog(sNewImagePath, sCurImagePath)
                End If
            Next
        End Using
        Return True
    End Function


End Class
