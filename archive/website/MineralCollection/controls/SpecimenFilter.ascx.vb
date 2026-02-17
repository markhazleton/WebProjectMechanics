Imports WebProjectMechanics
Imports wpmMineralCollection

Public Class MineralCollection_SpecimenFilter
    Inherits ApplicationUserControl
    Implements IApplicationFilter


    Private mySQLFilter As New SQLFilterList
    Public isAdmin As Boolean = False

    Private Sub LoadDropdowns()
        Dim myMineralApp As New MineralApplication()

        ddPrimaryMineral.Items.Add(New ListItem With {.Value = 0, .Text = "All Minerals"})
        SetupDropdown(myMineralApp.MineralLookup, ddPrimaryMineral)

        ddMineNM.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Mines"})
        SetupDropdown(myMineralApp.MineLookup, ddMineNM)

        ddCity.Items.Add(New ListItem With {.Value = 0, .Text = "All Cities"})
        SetupDropdown(myMineralApp.CityLookup, ddCity)

        ddState.Items.Add(New ListItem With {.Value = 0, .Text = "All States"})
        SetupDropdown(myMineralApp.StateLookup, ddState)

        ddCountry.Items.Add(New ListItem With {.Value = 0, .Text = "All Countries"})
        SetupDropdown(myMineralApp.CountryLookup, ddCountry)

        ddStorageLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Locations"})
        SetupDropdown(myMineralApp.StorageLocationLookup, ddStorageLocation)

        ddShowWherePurchased.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Shows"})
        SetupDropdown(myMineralApp.ShowLookup, ddShowWherePurchased)

        ddPurchasedFromCompany.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All"})
        SetupDropdown(myMineralApp.PurchasedFromLookup, ddPurchasedFromCompany)


        ddlCollection.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Collections"})
        SetupDropdown(myMineralApp.CollectionLookup, ddlCollection)

    End Sub

    Protected Sub Page_Init1(sender As Object, e As EventArgs) Handles Me.Init
        If Not IsPostBack Then
            LoadDropdowns()
            mySQLFilter.Clear()
            CollectionItemID = Request.QueryString("CollectionItemID")
            CollectionID = wpm_GetProperty(Request.QueryString("CollectionID"), ddlCollection.SelectedValue)
            SpecimenNumber = Request.QueryString("SpecimenNumber")
            Description = Request.QueryString("Description")
            MineralID = Request.QueryString("MineralID")
            MineralVariety = Request.QueryString("MineralVariety")
            StorageLocation = Request.QueryString("StorageLocation")
            CompanyID = Request.QueryString("CompanyID")
            MineNM = Request.QueryString("MineNM")
            ShowWherePurchased = Request.QueryString("ShowWherePurchased")
            LocationCityID = Request.QueryString("LocationCityID")
            LocationStateID = Request.QueryString("LocationStateID")
            LocationCountryID = Request.QueryString("LocationCountryID")
        End If
    End Sub

    Public WriteOnly Property CompanyID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("PurchasedFromCompanyID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                ddPurchasedFromCompany.SelectedValue = value
            End If
        End Set
    End Property
    Public WriteOnly Property StorageLocation As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("StorageLocation", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                ddStorageLocation.SelectedValue = value
            End If
        End Set
    End Property
    Public WriteOnly Property Description As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("Description", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                tbDescription.Text = value
            End If
        End Set
    End Property
    Public WriteOnly Property MineNM As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("MineNM", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                ddMineNM.SelectedValue = value
            End If
        End Set
    End Property
    Public WriteOnly Property ShowWherePurchased As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("ShowWherePurchased", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                ddShowWherePurchased.SelectedValue = value
            End If
        End Set
    End Property
    Public WriteOnly Property SpecimenNumber As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("SpecimenNumber", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                tbSpecimenNumber.Text = value
            End If
        End Set
    End Property
    Public WriteOnly Property MineralID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        mySQLFilter.Add(New SQLFilterClause("PrimaryMineralID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                        ddPrimaryMineral.SelectedValue = value
                    End If
                End If
            End If
        End Set
    End Property
    Public WriteOnly Property LocationCityID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        mySQLFilter.Add(New SQLFilterClause("LocationCityID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                        ddCity.SelectedValue = value
                    End If
                End If
            End If
        End Set
    End Property
    Public WriteOnly Property LocationStateID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        mySQLFilter.Add(New SQLFilterClause("LocationStateID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                        ddState.SelectedValue = value
                    End If
                End If
            End If
        End Set
    End Property
    Public WriteOnly Property LocationCountryID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If IsNumeric(value) Then
                    If CInt(value) > 0 Then
                        mySQLFilter.Add(New SQLFilterClause("LocationCountryID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                        ddCountry.SelectedValue = value
                    End If
                End If
            End If
        End Set
    End Property
    Public WriteOnly Property CollectionItemID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("CollectionItemID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
            End If
        End Set
    End Property
    Public WriteOnly Property CollectionID As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("CollectionID", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                ddlCollection.SelectedValue = value
            End If
        End Set
    End Property
    Public WriteOnly Property MineralVariety As String
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                mySQLFilter.Add(New SQLFilterClause("MineralVariety", SQLFilterOperator.Equal, value, SQLFilterConjunction.andConjunction, "CollectionItem"))
                tbMineralVariety.Text = value
            End If
        End Set
    End Property
    Protected Sub ShowAdminSearchFields(ByVal IsAdmin As Boolean)
        ddlCollection.Visible = False
        LabelCollection.Visible = False

        lblStorageLocation.Visible = IsAdmin
        ddStorageLocation.Visible = IsAdmin

        LabelddShowWherePurchased.Visible = IsAdmin
        ddShowWherePurchased.Visible = IsAdmin
    End Sub

    Public Function GetFilterClause(FieldType As String) As String Implements IApplicationFilter.GetFilterClause
        Return GetSQLFilterList().GetLINQWhere()
    End Function
    Private Function GetSQLFilterList() As SQLFilterList
        mySQLFilter.Clear()
        MineralID = ddPrimaryMineral.SelectedValue
        CollectionID = ddlCollection.SelectedValue
        SpecimenNumber = tbSpecimenNumber.Text
        Description = tbDescription.Text
        StorageLocation = ddStorageLocation.SelectedValue
        MineNM = ddMineNM.SelectedValue
        MineralVariety = tbMineralVariety.Text
        ShowWherePurchased = ddShowWherePurchased.SelectedValue
        LocationCityID = ddCity.SelectedValue
        LocationCountryID = ddCountry.SelectedValue
        LocationStateID = ddState.SelectedValue
        CompanyID = ddPurchasedFromCompany.SelectedValue
        Return mySQLFilter
    End Function

    Public Function GetFilterList() As SQLFilterList Implements IApplicationFilter.GetFilterList
        Return GetSQLFilterList()
    End Function
End Class
