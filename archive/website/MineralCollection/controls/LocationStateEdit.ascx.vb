Imports WebProjectMechanics
Imports wpmMineralCollection

Public Class MineralCollection_LocationStateEdit
    Inherits MineralCollection_EditUserControl

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfLocationStateID.Value = GetIntegerProperty("LocationStateID", -1)
        Session("ContactID") = 999

        If Not IsPostBack Then
            If CInt(hfLocationStateID.Value) > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True

                ddlCountry.Items.Add(New ListItem With {.Value = 0, .Text = "Please Select"})
                ddlCountry.AppendDataBoundItems = True
                ddlCountry.DataSource = (From i In myCon.LocationCountries Order By i.CountryNM)
                ddlCountry.DataTextField = "CountryNM"
                ddlCountry.DataValueField = "LocationCountryID"
                ddlCountry.DataBind()
                ddlCountry.SelectedIndex = -1

                Dim myLocationStates = (From i In myCon.LocationStates Where i.LocationStateID = hfLocationStateID.Value).ToList()
                Dim MyLocationState As New wpmMineralCollection.LocationState
                If myLocationStates.Count > 0 Then
                    MyLocationState = myLocationStates(0)
                Else
                    Throw New Exception("LocationStateID is not found")
                End If

                With MyLocationState
                    hfLocationStateID.Value = .LocationStateID
                    tbStateDS.Text = .StateDS
                    tbStateNM.Text = .StateNM
                    If wpm_GetDBInteger(.LocationCountryID, -1) > 0 Then
                        ddlCountry.SelectedValue = .LocationCountryID
                    End If
                End With
            ElseIf CInt(hfLocationStateID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                    ddlCountry.Items.Clear()
                    ddlCountry.Items.Add(New ListItem With {.Value = 0, .Text = "Please Select"})
                    ddlCountry.AppendDataBoundItems = True
                    ddlCountry.DataSource = (From i In myCon.LocationCountries Order By i.CountryNM)
                    ddlCountry.DataTextField = "CountryNM"
                    ddlCountry.DataValueField = "LocationCountryID"
                    ddlCountry.DataBind()
                    ddlCountry.SelectedIndex = -1
                tbStateDS.Text = String.Empty
                tbStateNM.Text = String.Empty
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Dim myObjects As New List(Of Object)
                    If wpm_GetIntegerProperty("LocationCountryID", 0) > 0 Then
                        myObjects.AddRange((From i In myCon.LocationStates Where i.LocationCountryID = wpm_GetIntegerProperty("LocationCountryID", 0) Select i.LocationStateID, i.StateNM, i.StateDS, i.ModifiedDT, i.ModifiedID, i.LocationCountryID, CountryNM = i.LocationCountry.CountryNM, SpecimenCount = i.CollectionItems.Count).ToList())
                    Else
                        myObjects.AddRange((From i In myCon.LocationStates Select i.LocationStateID, i.StateNM, i.StateDS, i.ModifiedDT, i.ModifiedID, i.LocationCountryID, CountryNM = i.LocationCountry.CountryNM, SpecimenCount = i.CollectionItems.Count).ToList())

                    End If


                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "State  (<a href='/MineralCollection/admin.aspx?Type=State&LocationStateID=0'>Add New State</a>  )",
                                                                    .DetailFieldName = "StateNM",
                                                                    .DetailKeyName = "LocationStateID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=State&LocationStateID={0}"}

                myTableHeader.AddHeaderItem("StateDS", "StateDS")
                myTableHeader.AddLinkHeaderItem("CountryNM", "CountryNM", "/MineralCollection/Admin.aspx?Type=State&LocationCountryID={0}", "LocationCountryID", "CountryNM")
                myTableHeader.AddHeaderItem("ModifiedDT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("SpecimenCount", "SpecimenCount", "/MineralCollection/Admin.aspx?Type=Specimen&LocationStateID={0}", "LocationStateID", "SpecimenCount")
                dtList.BuildTable(myTableHeader, myObjects)
            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=State")

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myLocationState = (From i In myCon.LocationStates Where i.LocationStateID = hfLocationStateID.Value).Single
        With myLocationState
            .StateNM = tbStateNM.Text.Trim
            .StateDS = tbStateDS.Text.Trim
            .LocationCountryID = ddlCountry.SelectedValue
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        If myCon.ReturnValue = String.Empty Then
            OnUpdated(Me)
            Response.Redirect("/MineralCollection/Admin.aspx?Type=State")
        End If
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myLocationState As New wpmMineralCollection.LocationState
        With myLocationState
            .StateNM = tbStateNM.Text.Trim
            .StateDS = tbStateDS.Text.Trim
            .LocationCountryID = ddlCountry.SelectedValue
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.LocationStates.InsertOnSubmit(myLocationState)

        If SaveChanges(AlertBox) then 
            OnUpdated(Me)
            Response.Redirect("/MineralCollection/Admin.aspx?Type=State")
        End If
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myLocationState = (From i In myCon.LocationStates Where i.LocationStateID = hfLocationStateID.Value).Single
        myCon.LocationStates.DeleteOnSubmit(myLocationState)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=State")
    End Sub

End Class
