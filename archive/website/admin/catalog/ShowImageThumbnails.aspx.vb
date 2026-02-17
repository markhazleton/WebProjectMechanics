Imports WebProjectMechanics
Imports System

Partial Class Admin_ShowImageThumbnails
    Inherits ApplicationPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim mySB As New StringBuilder
        Dim ImageList As LocationImageList
        Dim bFixFolder As Boolean = False
        Dim CurrentFilePath As String
        Dim NewFilePath As String
        Dim NewFolderPath As String
        For Each location As Location In masterPage.myCompany.LocationList
            If (location.LocationTypeCD.ToUpper) = "CATALOG" And (location.RecordSource.ToUpper) = "PAGE" Then
                masterPage.myCompany.SelectCurrentPageRow(location.LocationID, "")
                ImageList = New LocationImageList(masterPage.myCompany)
                If ImageList.Images.Count = 0 Then
                    mySB.Append(String.Format("<h1>{0} has no images</h1>", location.LocationName))
                Else
                    NewFolderPath = Server.MapPath(String.Format("{0}imageS/{1}", wpm_SiteGallery, wpm_FixInvalidCharacters(ImageList.CatalogPageName)))
                    mySB.Append(String.Format("<h1>{0}</h1>", ImageList.CatalogPageName))
                    mySB.Append(String.Format("<strong>{0}</strong><br/>", NewFolderPath))
                    mySB.Append("<table><tr>")
                    For Each Image As LocationImage In ImageList.Images
                        CurrentFilePath = Server.MapPath(ImageList.GetImagePath(Image.ImageFileName))
                        NewFilePath = Server.MapPath(String.Format("{0}images/{1}/{2}", wpm_SiteGallery, wpm_FixInvalidCharacters(ImageList.CatalogPageName), Replace(Image.ImageFileName, "image/", "")))
                        If FileProcessing.FileExists((CurrentFilePath)) Then
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
