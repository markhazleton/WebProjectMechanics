
Partial Class mhweb_file_admin
    Inherits mhPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySiteMap As New mhSiteMap(Session)
        Dim myImageList As DataTable = mhweb.mhDataCon.GetImageList(mySiteMap.mySession.CompanyID)
        Dim mySB As New StringBuilder("<hr/>")
        Dim myFileName As String
        Dim myImageFileName As String
        For Each sFileName As String In My.Computer.FileSystem.GetFiles(Server.MapPath(mySiteMap.mySiteFile.SiteGallery.ToString & "/image/"))
            myFileName = Replace(Replace(sFileName, Server.MapPath(mySiteMap.mySiteFile.SiteGallery.ToString), ""), "\", "/")
            mySB.Append("<br/>" & myFileName)
            For Each myrow As DataRow In myImageList.Rows
                myImageFileName = mhUTIL.GetDBString(myrow("ImageFileName"))
                If LCase(myImageFileName) = LCase(myFileName) Then
                    mySB.Append("(<a href=""/aspmaker/image_edit.aspx?ImageID=" & mhUTIL.GetDBString(myrow("ImageID")) & """>Edit</a>,<a href=""/aspmaker/image_delete.aspx?ImageID=" & mhUTIL.GetDBString(myrow("ImageID")) & """>Delete</a> ) ")
                    Exit For
                End If
            Next
        Next
        ' FixFolders(mySiteMap)
        myContent.Text = mySB.ToString
    End Sub

    Public Function FixFolders(ByRef mySiteMap As mhSiteMap) As Boolean
        Dim sNewPagePath As String = ("")
        Dim sNewImagePath As String = ("")
        Dim sNewThumbnailPath As String = ("")
        Dim dtPageImage As DataTable = mhDataCon.GetPageImage(mySiteMap.mySession.CompanyID)

        For Each myRow As DataRow In dtPageImage.Rows
            ' Create a Folder for Page 
            sNewPagePath = (HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "pages/" & Trim(mhUTIL.GetDBString(myRow.Item("PageName")))))
            mhfio.CreateFolder(sNewPagePath)

            ' Create a Folder for Page/Image and move this image file
            sNewImagePath = (HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "pages/" & Trim(mhUTIL.GetDBString(myRow.Item("PageName"))) & "/image"))
            mhfio.CreateFolder(sNewImagePath)
            mhfio.MoveFile(HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & mhUTIL.GetDBString(myRow.Item("ImageFileName"))), sNewPagePath & "/" & mhUTIL.GetDBString(myRow.Item("ImageFileName")))

            ' Create a Folder for Page/Thumbnail and move this image file
            sNewThumbnailPath = (HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & "pages/" & Trim(mhUTIL.GetDBString(myRow.Item("PageName"))) & "/thumbnail"))
            mhfio.CreateFolder(sNewThumbnailPath)
            mhfio.MoveFile(HttpContext.Current.Server.MapPath(mySiteMap.mySiteFile.SiteGallery & mhUTIL.GetDBString(myRow.Item("ImageThumbFileName"))), sNewPagePath & "/" & mhUTIL.GetDBString(myRow.Item("ImageThumbFileName")))
        Next
    End Function

End Class
