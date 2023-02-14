Imports LINQHelper.System.Linq.Dynamic

Public Class MineralCollectionListView
    Private Const INT_IndexBase As Integer = 0
    Property CurrentIndex As Integer
    Property CurrentCollectionItemID As Integer = 0
    Property CurrentCollectionItem As New MineralCollectionItem
    Property myResults As New List(Of MineralCollectionItem)
    ReadOnly Property MaxIndex As Integer
        Get
            Return myResults.Count() - 1
        End Get
    End Property
    Property MySQLFilter As New SQLFilterList
    WriteOnly Property FeaturedOnly As Boolean
        Set(value As Boolean)
            If value Then
                MySQLFilter.Add(New SQLFilterClause("IsFeatured", SQLFilterOperator.Equal, "-1", SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    WriteOnly Property SoldOnly As Boolean
        Set(value As Boolean)
            If value Then
                MySQLFilter.Add(New SQLFilterClause("IsSold", SQLFilterOperator.Equal, "-1", SQLFilterConjunction.andConjunction, "CollectionItem"))
            Else
                MySQLFilter.Add(New SQLFilterClause("IsSold", SQLFilterOperator.Equal, "0", SQLFilterConjunction.andConjunction, "CollectionItem"))

            End If
        End Set
    End Property
    WriteOnly Property CompanyID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                MySQLFilter.Add(New SQLFilterClause("PurchasedFromCompanyID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    WriteOnly Property Description As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                MySQLFilter.Add(New SQLFilterClause("Description", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    WriteOnly Property MineNM As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                MySQLFilter.Add(New SQLFilterClause("MineNM", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    WriteOnly Property SpecimenNumber As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                MySQLFilter.Add(New SQLFilterClause("SpecimenNumber", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    WriteOnly Property MineralID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        MySQLFilter.Add(New SQLFilterClause("MineralID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                    End If
                End If
            End If
        End Set
    End Property
    WriteOnly Property LocationCityID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        MySQLFilter.Add(New SQLFilterClause("LocationCityID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                    End If
                End If
            End If
        End Set
    End Property
    WriteOnly Property LocationStateID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        MySQLFilter.Add(New SQLFilterClause("LocationStateID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                    End If
                End If
            End If

        End Set
    End Property
    WriteOnly Property LocationCountryID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        MySQLFilter.Add(New SQLFilterClause("LocationCountryID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                    End If
                End If
            End If
        End Set
    End Property
    WriteOnly Property CollectionItemID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    CurrentCollectionItemID = wpm_GetDBInteger(value, 0)
                End If
            End If
        End Set
    End Property
    WriteOnly Property CollectionID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                MySQLFilter.Add(New SQLFilterClause("CollectionID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property

    Public Property ResultText As String

    Public Sub ResetCriteria()
        MySQLFilter.Clear()
        CurrentIndex = 0
    End Sub

    Public Function GetCurrentItem() As MineralCollectionItem

        If CurrentCollectionItemID > 0 Then
            Try
                Using mycon As New DataController()
                    CurrentCollectionItem = (From i In mycon.vwCollectionItems Where i.CollectionItemID = CurrentCollectionItemID Select New MineralCollectionItem With {.CollectionItemID = i.CollectionItemID,
                                                    .CollectionID = i.CollectionID,
                                                    .CollectionNM = i.CollectionNM,
                                                    .SpecimenNumber = i.SpecimenNumber,
                                                    .ImageFileNM = i.ImageFileNM,
                                                    .Nickname = i.Nickname,
                                                    .PrimaryMineralID = i.PrimaryMineralID,
                                                    .PrimaryMineralNM = i.PrimaryMineralNM,
                                                    .MineralVariety = i.MineralVariety,
                                                    .MineNM = i.MineNM,
                                                    .PurchaseDate = i.PurchaseDate,
                                                    .PurchasePrice = i.PurchasePrice,
                                                    .Value = i.Value,
                                                    .ShowWherePurchased = i.ShowWherePurchased,
                                                    .PurchasedFromCompanyID = i.PurchasedFromCompanyID,
                                                    .PurchasedFromCompanyNM = i.CompanyNM,
                                                    .StorageLocation = i.StorageLocation,
                                                    .SpecimenNotes = i.SpecimenNotes,
                                                    .Description = i.Description,
                                                    .ExCollection = i.ExCollection,
                                                    .HeightCm = i.HeightCm,
                                                    .WidthCm = i.WidthCm,
                                                    .ThicknessCm = i.ThicknessCm,
                                                    .HeightIn = i.HeightIn,
                                                    .WidthIn = i.WidthIn,
                                                    .ThicknessIn = i.ThicknessIn,
                                                    .WeightGr = i.WeightGr,
                                                    .WeightKg = i.WeightKg,
                                                    .SaleDT = i.SaleDT,
                                                    .SalePrice = i.SalePrice,
                                                    .LocationCityID = i.LocationCityID,
                                                    .City = i.City,
                                                    .LocationStateID = i.LocationStateID,
                                                    .StateNM = i.StateNM,
                                                    .LocationCountryID = i.LocationCountryID,
                                                    .CountryNM = i.CountryNM,
                                                    .IsSold = i.IsSold,
                                                    .IsFeatured = i.IsFeatured}).SingleOrDefault()

                    Dim iOrder As Integer = 0
                    Dim myImageList As New List(Of MineralImage)
                    myImageList.AddRange((From i In mycon.CollectionItemImages Where i.CollectionItemID = CurrentCollectionItemID Order By i.DisplayOrder
                                          Select New MineralImage With {.CollectionItemID = i.CollectionItemID,
                                                                        .CollectionItemImageID = i.CollectionItemImageID,
                                                                        .DisplayOrder = i.DisplayOrder,
                                                                        .ImageDS = i.ImageDS,
                                                                        .ImageFileNM = i.ImageFileNM,
                                                                        .ImageNM = i.ImageNM,
                                                                        .ImageType = i.ImageType,
                                                                        .ModifiedDT = i.ModifiedDT,
                                                                        .ModifiedID = i.ModifiedID}).ToList())
                    For Each myImage As MineralImage In myImageList
                        If myImage.ImageType = "Photo" Then
                            myImage.DisplayOrder = iOrder
                            iOrder = iOrder + 1

                            CurrentCollectionItem.Images.Add(myImage)
                        End If
                    Next
                    For Each myImage As MineralImage In myImageList
                        If myImage.ImageType <> "Photo" Then
                            myImage.DisplayOrder = iOrder
                            iOrder = iOrder + 1
                            CurrentCollectionItem.Images.Add(myImage)
                        End If
                    Next
                End Using
            Catch ex As Exception
                ApplicationLogging.ErrorLog("MineralCollectionView.GetCurrentItem", ex.ToString())
            End Try
        End If
        CurrentCollectionItemID = -1
        Return CurrentCollectionItem
    End Function
    Public Function GetList() As List(Of MineralCollectionItem)
        myResults.Clear()
        Using mycon As New DataController()
            Try
                If MySQLFilter.FindField("MineralID").Count > 0 Then
                    myResults.AddRange((From i In mycon.vwMineralCollectionItems.Where(MySQLFilter.GetLINQWhere).OrderBy("SpecimenNumber")
                                        Select New MineralCollectionItem With {
                                            .CollectionItemID = i.CollectionItemID,
                                            .CollectionID = i.CollectionID,
                                            .CollectionNM = i.CollectionNM,
                                            .SpecimenNumber = i.SpecimenNumber,
                                            .ImageFileNM = i.ImageFileNM,
                                            .Nickname = i.Nickname,
                                            .PrimaryMineralID = i.PrimaryMineralID,
                                            .PrimaryMineralNM = i.PrimaryMineralNM,
                                            .MineralVariety = i.MineralVariety,
                                            .MineNM = i.MineNM,
                                            .PurchaseDate = i.PurchaseDate,
                                            .PurchasePrice = i.PurchasePrice,
                                            .Value = i.Value,
                                            .ShowWherePurchased = i.ShowWherePurchased,
                                            .PurchasedFromCompanyID = i.PurchasedFromCompanyID,
                                            .PurchasedFromCompanyNM = i.CompanyNM,
                                            .StorageLocation = i.StorageLocation,
                                            .SpecimenNotes = i.SpecimenNotes,
                                            .Description = i.Description,
                                            .ExCollection = i.ExCollection,
                                            .HeightCm = i.HeightCm,
                                            .WidthCm = i.WidthCm,
                                            .ThicknessCm = i.ThicknessCm,
                                            .HeightIn = i.HeightIn,
                                            .WidthIn = i.WidthIn,
                                            .ThicknessIn = i.ThicknessIn,
                                            .WeightGr = i.WeightGr,
                                            .WeightKg = i.WeightKg,
                                            .SaleDT = i.SaleDT,
                                            .SalePrice = i.SalePrice,
                                            .LocationCityID = i.LocationCityID,
                                            .City = i.City,
                                            .LocationStateID = i.LocationStateID,
                                            .StateNM = i.StateNM,
                                            .LocationCountryID = i.LocationCountryID,
                                            .CountryNM = i.CountryNM,
                                            .IsSold = i.IsSold,
                                            .IsFeatured = i.IsFeatured}).ToList())
                ElseIf MySQLFilter.Count < 1 Then
                    myResults.AddRange((From i In mycon.vwCollectionItems.OrderBy("SpecimenNumber")
                                        Select New MineralCollectionItem With {
                                            .CollectionItemID = i.CollectionItemID,
                                            .CollectionID = i.CollectionID,
                                            .CollectionNM = i.CollectionNM,
                                            .SpecimenNumber = i.SpecimenNumber,
                                            .ImageFileNM = i.ImageFileNM,
                                            .Nickname = i.Nickname,
                                            .PrimaryMineralID = i.PrimaryMineralID,
                                            .PrimaryMineralNM = i.PrimaryMineralNM,
                                            .MineralVariety = i.MineralVariety,
                                            .MineNM = i.MineNM,
                                            .PurchaseDate = i.PurchaseDate,
                                            .PurchasePrice = i.PurchasePrice,
                                            .Value = i.Value,
                                            .ShowWherePurchased = i.ShowWherePurchased,
                                            .PurchasedFromCompanyID = i.PurchasedFromCompanyID,
                                            .PurchasedFromCompanyNM = i.CompanyNM,
                                            .StorageLocation = i.StorageLocation,
                                            .SpecimenNotes = i.SpecimenNotes,
                                            .Description = i.Description,
                                            .ExCollection = i.ExCollection,
                                            .HeightCm = i.HeightCm,
                                            .WidthCm = i.WidthCm,
                                            .ThicknessCm = i.ThicknessCm,
                                            .HeightIn = i.HeightIn,
                                            .WidthIn = i.WidthIn,
                                            .ThicknessIn = i.ThicknessIn,
                                            .WeightGr = i.WeightGr,
                                            .WeightKg = i.WeightKg,
                                            .SaleDT = i.SaleDT,
                                            .SalePrice = i.SalePrice,
                                            .LocationCityID = i.LocationCityID,
                                            .City = i.City,
                                            .LocationStateID = i.LocationStateID,
                                            .StateNM = i.StateNM,
                                            .LocationCountryID = i.LocationCountryID,
                                            .CountryNM = i.CountryNM,
                                            .IsSold = i.IsSold,
                                            .IsFeatured = i.IsFeatured}).ToList())

                Else

                    myResults.AddRange((From i In mycon.vwCollectionItems.Where(MySQLFilter.GetLINQWhere).OrderBy("SpecimenNumber")
                                        Select New MineralCollectionItem With {
                                            .CollectionItemID = i.CollectionItemID,
                                            .CollectionID = i.CollectionID,
                                            .CollectionNM = i.CollectionNM,
                                            .SpecimenNumber = i.SpecimenNumber,
                                            .ImageFileNM = i.ImageFileNM,
                                            .Nickname = i.Nickname,
                                            .PrimaryMineralID = i.PrimaryMineralID,
                                            .PrimaryMineralNM = i.PrimaryMineralNM,
                                            .MineralVariety = i.MineralVariety,
                                            .MineNM = i.MineNM,
                                            .PurchaseDate = i.PurchaseDate,
                                            .PurchasePrice = i.PurchasePrice,
                                            .Value = i.Value,
                                            .ShowWherePurchased = i.ShowWherePurchased,
                                            .PurchasedFromCompanyID = i.PurchasedFromCompanyID,
                                            .PurchasedFromCompanyNM = i.CompanyNM,
                                            .StorageLocation = i.StorageLocation,
                                            .SpecimenNotes = i.SpecimenNotes,
                                            .Description = i.Description,
                                            .ExCollection = i.ExCollection,
                                            .HeightCm = i.HeightCm,
                                            .WidthCm = i.WidthCm,
                                            .ThicknessCm = i.ThicknessCm,
                                            .HeightIn = i.HeightIn,
                                            .WidthIn = i.WidthIn,
                                            .ThicknessIn = i.ThicknessIn,
                                            .WeightGr = i.WeightGr,
                                            .WeightKg = i.WeightKg,
                                            .SaleDT = i.SaleDT,
                                            .SalePrice = i.SalePrice,
                                            .LocationCityID = i.LocationCityID,
                                            .City = i.City,
                                            .LocationStateID = i.LocationStateID,
                                            .StateNM = i.StateNM,
                                            .LocationCountryID = i.LocationCountryID,
                                            .CountryNM = i.CountryNM,
                                            .IsSold = i.IsSold,
                                            .IsFeatured = i.IsFeatured}).ToList())
                End If
                If MaxIndex < 0 Then
                    ResultText = String.Format("No Results Found", CurrentIndex, MaxIndex)
                ElseIf MaxIndex = 0 Then
                    ResultText = String.Format("{1} Result Found", CurrentIndex + 1, MaxIndex + 1)
                    CurrentCollectionItemID = myResults(0).CollectionItemID
                    GetCurrentItem()
                Else
                    ResultText = String.Format("{1} Results Found", CurrentIndex + 1, MaxIndex + 1)
                End If
            Catch ex As Exception
                ApplicationLogging.ErrorLog("MineralCollectionListView.GetList  - multiple criteria", ex.ToString)
            End Try

            'If myResults.Count = 1 Then
            '    If MySQLFilter.FindField("MineralID").Count > 0 Then
            '        CurrentCollectionItemID = TryCast(myResults(0), vwMineralCollectionItem).CollectionItemID
            '    Else
            '        CurrentCollectionItemID = TryCast(myResults(0), vwCollectionItem).CollectionItemID
            '    End If
            '    myResults.Clear()
            '    Dim sWhere = " CollectionItemID = " & CurrentCollectionItemID
            '    myResults.AddRange((From i In mycon.CollectionItems.Where(sWhere).OrderBy("SpecimenNumber")).ToList())
            'End If

        End Using
        Return myResults

    End Function

End Class