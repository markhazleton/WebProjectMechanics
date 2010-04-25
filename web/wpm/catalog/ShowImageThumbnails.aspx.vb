Partial Class wpm_catalog_ShowImageThumbnails
    Inherits AspNetMaker7_WPMGen
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        Dim ImageList As wpmPageImageList
        Dim bFixFolder As Boolean = False
        Dim CurrentFilePath As String
        Dim NewFilePath As String
        Dim NewFolderPath As String
        For Each location As wpmLocation In pageActiveSite.LocationList
            If (location.PageTypeCD.ToUpper) = "CATALOG" And (location.RecordSource.ToUpper) = "PAGE" Then
                pageActiveSite.SelectCurrentPageRow(location.PageID, "")
                ImageList = New wpmPageImageList(pageActiveSite)
                If ImageList.Count = 0 Then
                    mySB.Append("<h1>" & location.PageName & " has no images</h1>")
                Else
                    NewFolderPath = Server.MapPath(pageActiveSite.SiteGallery & "image/" & wpmUTIL.FixInvalidCharacters(ImageList.CatalogPageName))
                    mySB.Append("<h1>" & ImageList.CatalogPageName & "</h1>")
                    mySB.Append("<strong>" & NewFolderPath & "</strong><br/>")
                    mySB.Append("<table><tr>")
                    For Each Image As wpmImage In ImageList
                        CurrentFilePath = Server.MapPath(ImageList.GetImagePath(Image.ImageFileName))
                        NewFilePath = Server.MapPath(pageActiveSite.SiteGallery & "image/" & wpmUTIL.FixInvalidCharacters(ImageList.CatalogPageName) & "/" & Replace(Image.ImageFileName, "image/", ""))
                        If wpmFileIO.FileExists((CurrentFilePath)) Then
                            mySB.Append("<td>")
                            mySB.Append("<span style=""color:green;"">")
                            mySB.Append(CurrentFilePath)
                            mySB.Append("</span>")
                            mySB.Append("</td>")
                            bFixFolder = True
                        Else
                            mySB.Append("<td>")
                            mySB.Append("<span style=""color:red;"">")
                            mySB.Append(CurrentFilePath)
                            mySB.Append("</span>")
                            mySB.Append("</td>")
                            bFixFolder = False
                        End If
                        mySB.Append("</tr><tr>")
                    Next
                    mySB.Append("</tr></table><hr/>")
                End If
                ImageList = Nothing
            End If
        Next
        myMain.Text = mySB.ToString
    End Sub

End Class
