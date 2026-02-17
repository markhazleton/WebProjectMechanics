Imports LINQHelper.System.Linq.Dynamic


Public Class MineralApplication

    Public Property StorageLocationLookup As New List(Of LookupItem)
    Public Property MineLookup As New List(Of LookupItem)
    Public Property MineralLookup As New List(Of LookupItem)
    Public Property PurchasedFromLookup As New List(Of LookupItem)
    Public Property CityLookup As New List(Of LookupItem)
    Public Property StateLookup As New List(Of LookupItem)
    Public Property CountryLookup As New List(Of LookupItem)
    Public Property CollectionLookup As New List(Of LookupItem)
    Public Property ShowLookup As New List(Of LookupItem)

    Public Sub New()
        Using myCon As New DataController()
            ShowLookup.AddRange(((From i In myCon.CollectionItems Where i.ShowWherePurchased.Trim <> String.Empty Order By i.ShowWherePurchased Select New LookupItem With {.Value = i.ShowWherePurchased, .Name = i.ShowWherePurchased}).Distinct).ToList)
            StorageLocationLookup.AddRange(((From i In myCon.CollectionItems Select New LookupItem With {.Name = i.StorageLocation.Trim, .Value = i.StorageLocation.Trim}).Distinct).ToList())
            MineLookup.AddRange(((From i In myCon.CollectionItems Where i.MineNM.Trim <> String.Empty Order By i.MineNM.Trim Select New LookupItem With {.Name = i.MineNM, .Value = i.MineNM}).Distinct).ToList())
            PurchasedFromLookup.AddRange(((From i In myCon.Companies Order By i.CompanyNM Select New LookupItem With {.Name = i.CompanyNM, .Value = i.CompanyID.ToString()}).Distinct).ToList())
            MineralLookup.AddRange(((From i In myCon.Minerals Order By i.MineralNM Select New LookupItem With {.Name = i.MineralNM, .Value = i.MineralID.ToString()}).Distinct).ToList())
            CityLookup.AddRange(((From i In myCon.LocationCities Order By i.LocationCountry.CountryNM, i.City Select New LookupItem With {.Value = i.LocationCityID.ToString(), .Name = $"{i.City} - ({i.LocationState.StateNM}, {i.LocationCountry.CountryNM})"})).ToList())
            StateLookup.AddRange(((From i In myCon.LocationStates Order By i.LocationCountry.CountryNM, i.StateNM Select New LookupItem With {.Value = i.LocationStateID.ToString(), .Name = $"{i.StateNM} - ({i.LocationCountry.CountryNM})"})).ToList())
            CountryLookup.AddRange(((From i In myCon.LocationCountries Order By i.CountryNM Select New LookupItem With {.Value = i.LocationCountryID.ToString, .Name = i.CountryNM}).Distinct).ToList())
            CollectionLookup.AddRange(((From i In myCon.Collections Order By i.CollectionNM Select New LookupItem With {.Name = i.CollectionNM, .Value = i.CollectionID.ToString()}).Distinct).ToList())
        End Using
    End Sub
End Class