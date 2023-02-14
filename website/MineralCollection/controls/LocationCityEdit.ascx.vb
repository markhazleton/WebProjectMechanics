Imports WebProjectMechanics
Public Class MineralCollection_LocationCityEdit
    Inherits ApplicationUserControl

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfLocationCityID.Value = GetIntegerProperty("LocationCityID", -1)
        Session("ContactID") = 999

        If Not IsPostBack Then
            If CInt(hfLocationCityID.Value) > 0 Then

                pnlEdit.Visible = True
                dtList.Visible = False
                SetupDropdowns()
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myLocationCities = (From i In myCon.LocationCities Where i.LocationCityID = hfLocationCityID.Value).ToList()
                    Dim MyLocationCity As New wpmMineralCollection.LocationCity
                    If myLocationCities.Count > 0 Then
                        MyLocationCity = myLocationCities(0)
                    Else
                        Throw New Exception("LocationCityID is not found")
                    End If

                    With MyLocationCity
                        tbCityDS.Text = .CityDS
                        tbCity.Text = .City
                        If wpm_GetDBInteger(.LocationStateID, -1) > 0 Then
                            ddlState.SelectedValue = .LocationStateID
                        End If
                        If wpm_GetDBInteger(.LocationCountryID, -1) > 0 Then
                            ddlCountry.SelectedValue = .LocationCountryID
                        End If
                    End With
                End Using
            ElseIf CInt(hfLocationCityID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                SetupDropdowns()
                ' Insert Mode
                tbCityDS.Text = String.Empty
                tbCity.Text = String.Empty
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True


                Dim myObjects As New List(Of Object)
                Using myCon As New wpmMineralCollection.DataController()
                    If wpm_GetIntegerProperty("LocationCountryID", 0) > 0 Then
                        myObjects.AddRange((From i In myCon.LocationCities Where i.LocationCountryID = wpm_GetIntegerProperty("LocationCountryID", 0) Select i.LocationCityID, i.City, i.CityDS, i.ModifiedDT, i.ModifiedID, i.LocationCountryID, CountryNM = i.LocationCountry.CountryNM, i.LocationStateID, i.LocationState.StateNM, SpecimenCount = i.CollectionItems.Count).ToList())
                    ElseIf wpm_GetIntegerProperty("LocationStateID", 0) > 0 Then
                        myObjects.AddRange((From i In myCon.LocationCities Where i.LocationStateID = wpm_GetIntegerProperty("LocationStateID", 0) Select i.LocationCityID, i.City, i.CityDS, i.ModifiedDT, i.ModifiedID, i.LocationCountryID, CountryNM = i.LocationCountry.CountryNM, i.LocationStateID, i.LocationState.StateNM, SpecimenCount = i.CollectionItems.Count).ToList())
                    Else
                        myObjects.AddRange((From i In myCon.LocationCities Select i.LocationCityID, i.City, i.CityDS, i.ModifiedDT, i.ModifiedID, i.LocationCountryID, CountryNM = i.LocationCountry.CountryNM, i.LocationStateID, i.LocationState.StateNM, SpecimenCount = i.CollectionItems.Count).ToList())

                    End If


                End Using
                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "City  (<a href='/MineralCollection/admin.aspx?Type=City&LocationCityID=0'>Add New City</a>  )",
                                                                    .DetailFieldName = "City",
                                                                    .DetailKeyName = "LocationCityID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=City&LocationCityID={0}"}

                myTableHeader.AddHeaderItem("CityDS", "CityDS")
                myTableHeader.AddLinkHeaderItem("StateNM", "StateNM", "/MineralCollection/Admin.aspx?Type=City&LocationStateID={0}", "LocationStateID", "StateNM")
                myTableHeader.AddLinkHeaderItem("CountryNM", "CountryNM", "/MineralCollection/Admin.aspx?Type=City&LocationCountryID={0}", "LocationCountryID", "CountryNM")
                myTableHeader.AddHeaderItem("ModifiedDT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("SpecimenCount", "SpecimenCount", "/MineralCollection/Admin.aspx?Type=Specimen&LocationCityID={0}", "LocationCityID", "SpecimenCount")
                dtList.BuildTable(myTableHeader, myObjects)
            End If
        End If





    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=City")

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCity = (From i In myCon.LocationCities Where i.LocationCityID = hfLocationCityID.Value).Single
        With myLocationCity
            .City = tbCity.Text.Trim
            .CityDS = tbCityDS.Text.Trim
            If wpm_GetDBInteger(ddlState.SelectedValue, -1) > 0 Then
                .LocationStateID = ddlState.SelectedValue
            End If
            If wpm_GetDBInteger(ddlCountry.SelectedValue, -1) > 0 Then
                .LocationCountryID = ddlCountry.SelectedValue
            End If
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=City")
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCity As New wpmMineralCollection.LocationCity
        With myLocationCity
            .City = tbCity.Text.Trim
            .CityDS = tbCityDS.Text.Trim
            If wpm_GetDBInteger(ddlState.SelectedValue, -1) > 0 Then
                .LocationStateID = ddlState.SelectedValue
            End If
            If wpm_GetDBInteger(ddlCountry.SelectedValue, -1) > 0 Then
                .LocationCountryID = ddlCountry.SelectedValue
            End If
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.LocationCities.InsertOnSubmit(myLocationCity)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=City")
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCity = (From i In myCon.LocationCities Where i.LocationCityID = hfLocationCityID.Value).Single

        myCon.LocationCities.DeleteOnSubmit(myLocationCity)
        myCon.SubmitChanges()
        ' Response.Redirect(ResponseURL)
        OnUpdated(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=City")
    End Sub

    Private Sub SetupDropdowns()
        Using myCon As New wpmMineralCollection.DataController()
            ddlState.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
            ddlState.AppendDataBoundItems = True
            ddlState.DataSource = (From i In myCon.LocationStates Order By i.StateNM)
            ddlState.DataTextField = "StateNM"
            ddlState.DataValueField = "LocationStateID"
            ddlState.DataBind()
            ddlState.SelectedIndex = -1

            ddlCountry.Items.Add(New ListItem With {.Value = String.Empty, .Text = "Please Select"})
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = (From i In myCon.LocationCountries Order By i.CountryNM)
            ddlCountry.DataTextField = "CountryNM"
            ddlCountry.DataValueField = "LocationCountryID"
            ddlCountry.DataBind()
            ddlCountry.SelectedIndex = -1
        End Using
    End Sub
    Public Function ShowState(ByRef myState As wpmMineralCollection.LocationState) As String
        If myState Is Nothing Then
            Return String.Empty
        Else
            Return myState.StateNM
        End If
    End Function
    Public Function ShowCountry(ByRef myCountry As wpmMineralCollection.LocationCountry) As String
        If myCountry Is Nothing Then
            Return String.Empty
        Else
            Return myCountry.CountryNM
        End If
    End Function


End Class
