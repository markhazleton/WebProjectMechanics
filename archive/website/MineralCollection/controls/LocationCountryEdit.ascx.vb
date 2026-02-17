Imports WebProjectMechanics
Public Class MineralCollection_controls_LocationCountryEdit
    Inherits ApplicationUserControl
    Private Const str_RedirectURL As String = "/MineralCollection/Admin.aspx?Type=Country"


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfLocationCountryID.Value = GetIntegerProperty("LocationCountryID", -1)
        Session("ContactID") = 999
        If Not IsPostBack Then
            If CInt(hfLocationCountryID.Value) > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myLocationCountrys = (From i In myCon.LocationCountries Where i.LocationCountryID = hfLocationCountryID.Value Order By i.CountryNM Ascending).ToList()
                    Dim MyLocationCountry As New wpmMineralCollection.LocationCountry
                    If myLocationCountrys.Count > 0 Then
                        MyLocationCountry = myLocationCountrys(0)
                    Else
                        Throw New Exception("LocationCountryID is not found")
                    End If
                    With MyLocationCountry
                        tbCountryDS.Text = .CountryDS
                        tbCountry.Text = .CountryNM
                        hfLocationCountryID.Value = .LocationCountryID
                        hfModifiedDT.Value = .ModifiedDT
                        hfModifiedID.Value = .ModifiedID
                    End With
                End Using
            ElseIf CInt(hfLocationCountryID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                tbCountryDS.Text = String.Empty
                tbCountry.Text = String.Empty
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
                    myObjects.AddRange((From i In myCon.LocationCountries Select i.LocationCountryID, i.CountryNM, i.CountryDS, i.ModifiedDT, i.ModifiedID, SpecimenCount = i.CollectionItems.Count).ToList())
                End Using
                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Country  (<a href='/MineralCollection/admin.aspx?Type=Collection&CollectionID=0'>Add New Country</a>  )",
                                                                    .DetailFieldName = "CountryNM",
                                                                    .DetailKeyName = "LocationCountryID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=Country&LocationCountryID={0}"}

                myTableHeader.AddHeaderItem("CountryDS", "CountryDS")
                myTableHeader.AddHeaderItem("ModifiedDT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("SpecimenCount", "SpecimenCount", "/MineralCollection/Admin.aspx?Type=Specimen&LocationCountryID={0}", "LocationCountryID", "SpecimenCount")
                dtList.BuildTable(myTableHeader, myObjects)



            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect(str_RedirectURL)

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCountry = (From i In myCon.LocationCountries Where i.LocationCountryID = hfLocationCountryID.Value).Single
        With myLocationCountry
            .CountryNM = tbCountry.Text.Trim
            .CountryDS = tbCountryDS.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(str_RedirectURL)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCountry As New wpmMineralCollection.LocationCountry
        With myLocationCountry
            .CountryNM = tbCountry.Text.Trim
            .CountryDS = tbCountryDS.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.LocationCountries.InsertOnSubmit(myLocationCountry)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(str_RedirectURL)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myLocationCountry = (From i In myCon.LocationCountries Where i.LocationCountryID = hfLocationCountryID.Value).Single

        myCon.LocationCountries.DeleteOnSubmit(myLocationCountry)
        myCon.SubmitChanges()
        ' Response.Redirect(ResponseURL)
        OnUpdated(Me)
        Response.Redirect(str_RedirectURL)
    End Sub

End Class
