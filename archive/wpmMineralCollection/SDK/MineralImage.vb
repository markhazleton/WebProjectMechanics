
Public Class MineralImage
    Implements ICollectionItemImage
    Public Property CollectionItemID As Integer Implements ICollectionItemImage.CollectionItemID
    Public Property CollectionItemImageID As Integer Implements ICollectionItemImage.CollectionItemImageID
    Public Property DisplayOrder As Integer Implements ICollectionItemImage.DisplayOrder
    Public Property ImageDS As String Implements ICollectionItemImage.ImageDS
    Public Property ImageFileNM As String Implements ICollectionItemImage.ImageFileNM
    Public Property ImageNM As String Implements ICollectionItemImage.ImageNM
    Public Property ImageType As String Implements ICollectionItemImage.ImageType
    Public Property ModifiedDT As Date Implements ICollectionItemImage.ModifiedDT
    Public Property ModifiedID As Integer Implements ICollectionItemImage.ModifiedID
End Class
