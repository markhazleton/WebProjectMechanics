
Public Class LocationImagePresenter
    ReadOnly yourLocationImage As ILocationImage
    Dim myLocationImage As LocationImage
    Public Status As String

    Public Sub New(ByVal myLocationImage As ILocationImage)
        yourLocationImage = myLocationImage
        Status = "New Image"
    End Sub

    Public Sub updateMyLocationImage(ByRef ReqImageID As String)
        GetImageValues(ReqImageID)
    End Sub

    Public Sub DeleteImage(ByVal ReqImageID As String)
        If LocationImage.DeleteImage(ReqImageID) Then
            Status = "Image Deleted"
        Else

            Status = "Error on Image Delete"
        End If
    End Sub
    Public Sub SetImageValues(ByRef ReqImageID As String)
        myLocationImage = New LocationImage(ReqImageID)
        yourLocationImage.ImageId = myLocationImage.ImageID
        yourLocationImage.ImageName = myLocationImage.ImageName
        yourLocationImage.ImageFileName = myLocationImage.ImageFileName
        yourLocationImage.ImageThumbFileName = myLocationImage.ImageThumbFileName
        yourLocationImage.ImageDescription = myLocationImage.ImageDescription
        yourLocationImage.ImageComment = myLocationImage.ImageComment
        yourLocationImage.ImageDate = myLocationImage.ImageDate
        yourLocationImage.Active = myLocationImage.Active
        yourLocationImage.ModifiedDate = myLocationImage.ModifiedDT
        yourLocationImage.VersionNumber = myLocationImage.VersionNumber
        yourLocationImage.ContactId = myLocationImage.ContactID
        yourLocationImage.LocationID = myLocationImage.LocationID
        yourLocationImage.ParentLocationID = myLocationImage.ParentLocationID
        yourLocationImage.Title = myLocationImage.Title
    End Sub
    Private Sub GetImageValues(ByRef ReqImageID As String)
        myLocationImage = New LocationImage(ReqImageID)
        myLocationImage.ImageID = yourLocationImage.ImageId
        myLocationImage.ImageName = yourLocationImage.ImageName
        myLocationImage.ImageFileName = yourLocationImage.ImageFileName
        myLocationImage.ImageDescription = yourLocationImage.ImageDescription
        myLocationImage.ImageComment = yourLocationImage.ImageComment
        myLocationImage.ImageDate = yourLocationImage.ImageDate
        myLocationImage.Active = yourLocationImage.Active
        myLocationImage.ModifiedDT = yourLocationImage.ModifiedDate
        myLocationImage.VersionNumber = yourLocationImage.VersionNumber
        myLocationImage.ContactID = yourLocationImage.ContactId
        myLocationImage.LocationID = yourLocationImage.LocationID
        myLocationImage.ParentLocationID = yourLocationImage.ParentLocationID
        myLocationImage.Title = yourLocationImage.Title
        If myLocationImage.updateImage() Then
            Status = "Image has been updated"
        Else
            Status = "Error on Image Update"
        End If
    End Sub
    Public Function GetThubmailURL(ByVal ImageWidth As Integer, ByVal SiteGallery As String) As String
        If myLocationImage Is Nothing Then
            Return String.Empty
        Else
            Return String.Format("/runtime/catalog/FindImage.ashx?h={0}&w={0}&img={1}", ImageWidth, Replace(Replace(SiteGallery & myLocationImage.ImageFileName, "\", "/"), "//", "/"))
        End If
    End Function
End Class
