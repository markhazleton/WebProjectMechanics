
Public Class SpecimenImageList
    Inherits List(Of SpecimenImage)
    Private _SearchImageFileName As String
    Private _SearchImageID As String
    Public Sub GetCollectionImages(ByVal CollectionID As Integer)
        Using myCon As New DataController()
            AddRange((From i In myCon.vwCollectionItemImages
                      Where i.CollectionID = CollectionID
                      Select New SpecimenImage With
                             {.CollectionItemID = wpm_GetDBInteger(i.CollectionItemID),
                              .CollectionItemImageID = wpm_GetDBInteger(i.CollectionItemImageID),
                              .DisplayOrder = wpm_GetDBInteger(i.DisplayOrder),
                              .ImageDS = i.ImageDS,
                              .ImageFileNM = i.ImageFileNM,
                              .ImageNM = i.ImageNM,
                              .ImageType = i.ImageType,
                              .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                              .ModifiedID = wpm_GetDBInteger(i.ModifiedID),
                              .NickName = i.Nickname,
                              .PrimaryMineralID = wpm_GetDBInteger(i.PrimaryMineralID),
                              .MineralNM = i.MineralNM,
                              .SpecimenNumber = wpm_GetDBString(i.SpecimenNumber),
                              .SpecimenNotes = i.SpecimenNotes,
                              .SpecimenDS = i.Description
                             }).ToList)
        End Using
    End Sub
    Public Function FindImageByImageID(ByVal ImageID As String) As SpecimenImage
        _SearchImageID = ImageID
        Return Find(AddressOf FindImageByID)
    End Function
    Private Function FindImageByID(ByVal Image As SpecimenImage) As Boolean
        If Image.CollectionItemImageID = CInt(_SearchImageID) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function FindImageByImageFileName(ByVal ImageFileNM As String) As SpecimenImage
        _SearchImageFileName = ImageFileNM
        Try
            Return Me.Find(AddressOf FindImageByImageFileName)
        Catch ex As Exception
            Return New SpecimenImage With {.ImageFileNM = ImageFileNM}
        End Try
    End Function
    Private Function FindImageByImageFileName(ByVal Image As SpecimenImage) As Boolean
        If wpm_CheckForMatch(Image.ImageFileNM, _SearchImageFileName) Then
            Return True
        Else
            Return False
        End If
    End Function



End Class