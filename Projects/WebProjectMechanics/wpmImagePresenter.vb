
Public Class wpmImagePresenter
    Dim yourUI As IImageRow
    Dim myimage As wpmImage
    Public Status As String

    Public Sub New(ByVal MyUI As IImageRow)
        yourUI = MyUI
        Status = "New Image"
    End Sub

    Public Sub updateMyUi(ByRef ReqImageID As String)
        GetImageValues(ReqImageID)
    End Sub

    Public Sub DeleteImage(ByVal ReqImageID As String)
        If wpmImage.DeleteImage(ReqImageID) Then
            Status = "Image Deleted"
        Else
            Status = "Error on Image Delete"
        End If
    End Sub
    Public Sub SetImageValues(ByRef ReqImageID As String)
        myimage = New wpmImage(ReqImageID)
        yourUI.ImageId = myimage.ImageID
        yourUI.ImageName = myimage.ImageName
        yourUI.ImageFileName = myimage.ImageFileName
        yourUI.ImageThumbFileName = myimage.ImageThumbFileName
        yourUI.ImageDescription = myimage.ImageDescription
        yourUI.ImageComment = myimage.ImageComment
        yourUI.ImageDate = myimage.ImageDate
        yourUI.Active = myimage.Active
        yourUI.ModifiedDate = myimage.ModifiedDT
        yourUI.VersionNumber = myimage.VersionNumber
        yourUI.ContactId = myimage.ContactID
        yourUI.CompanyId = myimage.CompanyID
        yourUI.Title = myimage.Title
        yourUI.Medium = myimage.Medium
        yourUI.Size = myimage.Size
        yourUI.Price = myimage.Price
        yourUI.Color = myimage.Color
        yourUI.Subject = myimage.Subject
        yourUI.Sold = myimage.Sold
    End Sub
    Private Sub GetImageValues(ByRef ReqImageID As String)
        myimage = New wpmImage(ReqImageID)
        myimage.ImageID = yourUI.ImageId
        myimage.ImageName = yourUI.ImageName
        myimage.ImageFileName = yourUI.ImageFileName
        myimage.ImageDescription = yourUI.ImageDescription
        myimage.ImageComment = yourUI.ImageComment
        myimage.ImageDate = yourUI.ImageDate
        myimage.Active = yourUI.Active
        myimage.ModifiedDT = yourUI.ModifiedDate
        myimage.VersionNumber = yourUI.VersionNumber
        myimage.ContactID = yourUI.ContactId
        myimage.CompanyID = yourUI.CompanyId
        myimage.Title = yourUI.Title
        myimage.Medium = yourUI.Medium
        myimage.Size = yourUI.Size
        myimage.Price = yourUI.Price
        myimage.Color = yourUI.Color
        myimage.Subject = yourUI.Subject
        myimage.Sold = yourUI.Sold
        If myimage.updateImage() Then
            Status = "Image has been updated"
        Else
            Status = "Error on Image Update"
        End If
    End Sub
    Public Function GetThubmailURL(ByVal ImageWidth As Integer, ByVal SiteGallery As String) As String
        If myimage Is Nothing Then
            Return String.Empty
        Else
            Return String.Format("/wpm/catalog/ImageResize.aspx?w={0}&img={1}", ImageWidth, Replace(Replace(SiteGallery & myimage.ImageFileName, "\", "/"), "//", "/"))
        End If
    End Function
End Class
