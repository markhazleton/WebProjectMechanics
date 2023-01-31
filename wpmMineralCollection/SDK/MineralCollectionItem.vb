
Public Class MineralCollectionItem
    Public Property CollectionItemID As Integer
    Public Property CollectionID As Integer
    Public Property CollectionNM As String 
    Public Property SpecimenNumber As Double
    Public Property ImageFileNM As String 
    Public Property Nickname As String
    Public Property PrimaryMineralID As System.Nullable(Of Integer)
    Public Property PrimaryMineralNM As String 
    Public Property MineralVariety As String
    Public Property MineNM As String
    Public Property PurchaseDate As System.Nullable(Of Date)
    Public Property PurchasePrice As System.Nullable(Of Decimal)
    Public Property Value As System.Nullable(Of Decimal)
    Public Property ShowWherePurchased As String
    Public Property PurchasedFromCompanyID As System.Nullable(Of Integer)
    Public Property PurchasedFromCompanyNM As String 
    Public Property StorageLocation As String
    Public Property SpecimenNotes As String
    Public Property Description As String
    Public Property ExCollection As String
    Public Property HeightCm As System.Nullable(Of Double)
    Public Property WidthCm As System.Nullable(Of Double)
    Public Property ThicknessCm As System.Nullable(Of Double)
    Public Property HeightIn As System.Nullable(Of Double)
    Public Property WidthIn As System.Nullable(Of Double)
    Public Property ThicknessIn As System.Nullable(Of Double)
    Public Property WeightGr As String
    Public Property WeightKg As String
    Public Property SaleDT As System.Nullable(Of Date)
    Public Property SalePrice As System.Nullable(Of Decimal)
    Public Property LocationCityID As System.Nullable(Of Integer)
    Public Property City As String 
    Public Property LocationStateID As System.Nullable(Of Integer)
    Public Property StateNM As String 
    Public Property LocationCountryID As System.Nullable(Of Integer)
    Public Property CountryNM As String 
    Public Property ModifiedID As Integer
    Public Property ModifiedDT As Date
    Public Property IsFeatured As Integer
    Public Property IsSold As Integer
    Public Property Images As new MineralImageList
    Public Property CollectionItemMinerals As new MineralItemList
End Class