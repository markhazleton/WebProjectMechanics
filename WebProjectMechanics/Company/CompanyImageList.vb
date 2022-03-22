Public Class CompanyImageList
    Inherits List(Of LocationImage)
    Private _SearchImageFileName As String
    Private _SearchImageID As String
    Public Sub New(ByVal CompanyID As String)
        GetCompanyImages(CompanyID)
    End Sub
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of LocationImage))
        MyBase.New(collection)

    End Sub

    Public Sub GetCompanyImages(ByVal CompanyID As String)
        Using mydt As DataTable = ApplicationDAL.GetImageList(CompanyID)
            For Each myrow As DataRow In mydt.Rows
                Dim newLocationImage = New LocationImage() With {.ImageID = wpm_GetDBString(myrow.Item("ImageID")),
                                                                 .Active = wpm_GetDBBoolean(myrow.Item("Active")),
                                                                 .ContactID = wpm_GetDBString(myrow.Item("ContactID")),
                                                                 .LocationID = String.Format("IMG-{0}", wpm_GetDBString(myrow.Item("ImageID"))),
                                                                 .ParentLocationID = wpm_GetDBString(myrow.Item("PageID")),
                                                                 .VersionNumber = wpm_GetDBInteger(myrow.Item("VersionNo")),
                                                                 .Title = wpm_GetDBString(myrow.Item("Title")),
                                                                 .ImageName = wpm_GetDBString(myrow.Item("ImageName")),
                                                                 .ImageDescription = wpm_GetDBString(myrow.Item("ImageDescription")),
                                                                 .ImageComment = wpm_GetDBString(myrow.Item("ImageComment")),
                                                                 .ImageFileName = wpm_GetDBString(myrow.Item("ImageFileName")),
                                                                 .ImageThumbFileName = wpm_GetDBString(myrow.Item("ImageFileName")),
                                                                 .ImageDate = wpm_GetDBDate(myrow.Item("ImageDate")),
                                                                 .ModifiedDT = wpm_GetDBDate(myrow.Item("ModifiedDT")),
                                                                 .DisplayOrder = wpm_GetDBInteger(myrow.Item("PageImagePosition")),
                                                                 .ImageURL = wpm_FixInvalidCharacters(String.Format("{0}-{1}{2}", myrow.Item("PageName"), myrow.Item("ImageName"), wpm_SiteConfig.DefaultExtension))}
                Add(newLocationImage)
            Next
        End Using
    End Sub
    Public Function FindImageByImageID(ByVal ImageID As String) As LocationImage
        _SearchImageID = ImageID
        Return Find(AddressOf FindImageByID)
    End Function
    Private Function FindImageByID(ByVal Image As LocationImage) As Boolean
        If Image.ImageID = _SearchImageID Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function FindImageByImageFileName(ByVal ImageFileName As String) As LocationImage
        _SearchImageFileName = ImageFileName
        Return Me.Find(AddressOf FindImageByImageFileName)
    End Function
    Private Function FindImageByImageFileName(ByVal Image As LocationImage) As Boolean
        If Image.ImageFileName.Replace("\", "/").ToLower = _SearchImageFileName.Replace("\", "/").ToLower Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
