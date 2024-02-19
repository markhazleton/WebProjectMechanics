Imports WebProjectMechanics
Imports wpmMineralCollection

Public Class MineralCollection_CollectionItemEdit
    Inherits MineralCollection_EditUserControl

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfCollectionItemID.Value = wpm_GetIntegerProperty("CollectionItemID", -1)
        wpm_SetContactID("999")

        If Not IsPostBack Then
            If CInt(hfCollectionItemID.Value) > 0 Then
                pnlEdit.Visible = True
                If wpm_GetIntegerProperty("CollectionItemImageID", -1) <> -1 Then
                    pnlCollectionItem.Visible = False
                    CollectionItemImageEdit1.Visible = True
                    CollectionItemMineralEdit1.Visible = False
                ElseIf wpm_GetIntegerProperty("CollectionItemMineralID", -1) <> -1 Then
                    pnlCollectionItem.Visible = False
                    CollectionItemImageEdit1.Visible = False
                    CollectionItemMineralEdit1.Visible = True
                Else
                    pnlCollectionItem.Visible = True
                    CollectionItemImageEdit1.Visible = True
                    CollectionItemMineralEdit1.Visible = True
                End If

                If pnlCollectionItem.Visible Then
                    ' Edit Mode
                    SetupDropdowns()
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    Dim myCollectionItems = (From i In myCon.CollectionItems Where i.CollectionItemID = hfCollectionItemID.Value).ToList()
                    Dim MyCollectionItem As New CollectionItem
                    If myCollectionItems.Count > 0 Then
                        MyCollectionItem = myCollectionItems(0)
                    Else
                        Throw New Exception("CollectionItemID is not found")
                    End If
                    With MyCollectionItem
                        hfCollectionItemID.Value = .CollectionItemID
                        ddlCollection.SelectedValue = .CollectionID

                        SetSelectedValue(ddPrimaryMineral, wpm_GetDBInteger(.PrimaryMineralID))
                        SetSelectedValue(ddPurchasedFromCompany, wpm_GetDBInteger(.PurchasedFromCompanyID))

                        SetSelectedValue(ddlLocationCity, wpm_GetDBInteger(.LocationCityID))
                        SetSelectedValue(ddlLocationState, wpm_GetDBInteger(.LocationStateID))
                        SetSelectedValue(ddlLocationCountry, wpm_GetDBInteger(.LocationCountryID))

                        tbDescription.Text = .Description
                        tbNickname.Text = .Nickname
                        cbIsFeatured.Checked = wpm_GetDBBoolean(.IsFeatured)
                        cbIsSold.Checked = wpm_GetDBBoolean(.IsSold)
                        SetDateTextbox(tbDate_of_Purchase, .PurchaseDate)

                        tbEx_Collection.Text = .ExCollection
                        tbHeightCm.Text = wpm_GetDBDouble(.HeightCm)
                        tbHeightIn.Text = wpm_GetDBDouble(.HeightIn)
                        tbMine_Name.Text = .MineNM
                        tbPurchase_Price.Text = wpm_GetDBDouble(.PurchasePrice)

                        SetDateTextbox(tbSaleDT, .SaleDT)

                        tbSalePrice.Text = wpm_GetDBDouble(.SalePrice)

                        tbShow_Purchase.Text = .ShowWherePurchased
                        tbSpecimenNotes.Text = .SpecimenNotes
                        tbSpecimenNumber.Text = .SpecimenNumber
                        tbStorage_Location.Text = .StorageLocation
                        tbThicknessCm.Text = wpm_GetDBDouble(.ThicknessCm)
                        tbThicknessIn.Text = wpm_GetDBDouble(.ThicknessIn)
                        tbValue.Text = wpm_GetDBDouble(.Value)
                        tbVariety.Text = .MineralVariety
                        tbWeightGr.Text = wpm_GetDBDouble(.WeightGr)
                        tbWeightKg.Text = wpm_GetDBDouble(.WeightKg)
                        tbWidthCm.Text = wpm_GetDBDouble(.WidthCm)
                        tbWidthIn.Text = wpm_GetDBDouble(.WidthIn)
                        For Each myImage In .CollectionItemImages
                            pnlThumbnails.Controls.Add(New Literal With {.Text = String.Format("<div class=""col-xs-6 col-md-3""><a class=""thumbnail"" href=""/MineralCollection/Admin.aspx?Type=Specimen&amp;CollectionItemID={2}&amp;CollectionItemImageID={1}""><img width=""150px"" src=""{0}"" alt=""{3}""></a></div>", Display.GetThumbnailURL(myImage.ImageFileNM), myImage.CollectionItemImageID, myImage.CollectionItemID, myImage.ImageNM)})
                        Next
                    End With
                End If
            ElseIf CInt(hfCollectionItemID.Value) = 0 Then
                pnlEdit.Visible = True
                ' Insert Mode
                Using myCon As New DataController()
                    tbSpecimenNumber.Text = (From i In myCon.CollectionItems Select i.SpecimenNumber).Max + 1
                End Using
                tbDescription.Text = String.Empty
                tbNickname.Text = String.Empty
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                ' Edit Mode
                SetupDropdowns()
            End If
        End If
        Session("CollectionItemID") = hfCollectionItemID.Value
    End Sub

    Private Sub SetupDropdowns()
        Dim myMineralApp As New MineralApplication()
        SetupDropdown(myMineralApp.CollectionLookup, ddlCollection)
        SetupDropdown(myMineralApp.MineralLookup, ddPrimaryMineral)
        ddlLocationCity.Items.Add(New ListItem With {.Value = 0, .Text = "Any City"})
        SetupDropdown(myMineralApp.CityLookup, ddlLocationCity)
        ddlLocationState.Items.Add(New ListItem With {.Value = 0, .Text = "Any State or Province"})
        SetupDropdown(myMineralApp.StateLookup, ddlLocationState)
        ddlLocationCountry.Items.Add(New ListItem With {.Value = 0, .Text = "Any Country"})
        SetupDropdown(myMineralApp.CountryLookup, ddlLocationCountry)
        ddPurchasedFromCompany.Items.Add(New ListItem With {.Value = 0, .Text = "Please Select"})
        SetupDropdown(myMineralApp.PurchasedFromLookup, ddPurchasedFromCompany)
    End Sub


    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect("/MineralCollection/Admin.aspx?Type=Specimen")
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myCollectionItem As CollectionItem = (From i In myCon.CollectionItems Where i.CollectionItemID = hfCollectionItemID.Value).Single
        With myCollectionItem
            .CollectionID = ddlCollection.SelectedValue
            .PrimaryMineralID = ddPrimaryMineral.SelectedValue
            If CInt(ddPurchasedFromCompany.SelectedValue) > 0 Then
                .PurchasedFromCompanyID = ddPurchasedFromCompany.SelectedValue
            End If
            If CInt(ddlLocationCity.SelectedValue) > 0 Then
                .LocationCityID = wpm_GetDBInteger(ddlLocationCity.SelectedValue)
            End If
            If CInt(ddlLocationState.SelectedValue) > 0 Then
                .LocationStateID = wpm_GetDBInteger(ddlLocationState.SelectedValue)
            End If
            If CInt(ddlLocationCountry.SelectedValue) > 0 Then
                .LocationCountryID = wpm_GetDBInteger(ddlLocationCountry.SelectedValue)
            End If
            .Nickname = tbNickname.Text.Trim
            .IsFeatured = cbIsFeatured.Checked
            .IsSold = cbIsSold.Checked
            .Description = tbDescription.Text.Trim
            .PurchaseDate = GetDBDate(tbDate_of_Purchase.Text)
            .ExCollection = wpm_GetDBString(tbEx_Collection.Text)

            .HeightCm = wpm_GetDBDouble(tbHeightCm.Text)
            .HeightIn = wpm_GetDBDouble(tbHeightIn.Text)
            .MineNM = wpm_GetDBString(tbMine_Name.Text)

            .PurchasePrice = wpm_GetDBDouble(tbPurchase_Price.Text)
            .SaleDT = GetDBDate(tbSaleDT.Text)
            .SalePrice = wpm_GetDBDouble(tbSalePrice.Text)

            .ShowWherePurchased = tbShow_Purchase.Text
            .SpecimenNotes = wpm_GetDBString(tbSpecimenNotes.Text)
            .SpecimenNumber = wpm_GetDBString(tbSpecimenNumber.Text)
            .StorageLocation = wpm_GetDBString(tbStorage_Location.Text)
            .ThicknessCm = wpm_GetDBDouble(tbThicknessCm.Text)
            .ThicknessIn = wpm_GetDBDouble(tbThicknessIn.Text)
            .Value = wpm_GetDBDouble(tbValue.Text)
            .MineralVariety = wpm_GetDBString(tbVariety.Text)
            .WeightGr = wpm_GetDBDouble(tbWeightGr.Text)
            .WeightKg = wpm_GetDBDouble(tbWeightKg.Text)
            .WidthCm = wpm_GetDBDouble(tbWidthCm.Text)
            .WidthIn = wpm_GetDBDouble(tbWidthIn.Text)

            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        If SaveChanges(AlertBox) Then
            OnUpdated(Me)
            Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen", hfCollectionItemID.Value))
        End If
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCollectionItem As New CollectionItem
        With myCollectionItem
            .CollectionID = ddlCollection.SelectedValue
            .PrimaryMineralID = ddPrimaryMineral.SelectedValue
            If CInt(ddPurchasedFromCompany.SelectedValue) > 0 Then
                .PurchasedFromCompanyID = ddPurchasedFromCompany.SelectedValue
            End If
            If CInt(ddlLocationCity.SelectedValue) > 0 Then
                .LocationCityID = wpm_GetDBInteger(ddlLocationCity.SelectedValue)
            End If
            If CInt(ddlLocationState.SelectedValue) > 0 Then
                .LocationStateID = wpm_GetDBInteger(ddlLocationState.SelectedValue)
            End If
            If CInt(ddlLocationCountry.SelectedValue) > 0 Then
                .LocationCountryID = wpm_GetDBInteger(ddlLocationCountry.SelectedValue)
            End If
            .Nickname = tbNickname.Text.Trim
            .Description = tbDescription.Text.Trim
            .PurchaseDate = GetDBDate(tbDate_of_Purchase.Text)
            .ExCollection = wpm_GetDBString(tbEx_Collection.Text)
            .IsFeatured = wpm_GetDBBoolean(cbIsFeatured.Checked)
            .HeightCm = wpm_GetDBDouble(tbHeightCm.Text)
            .HeightIn = wpm_GetDBDouble(tbHeightIn.Text)
            .MineNM = wpm_GetDBString(tbMine_Name.Text)

            .PurchasePrice = wpm_GetDBDouble(tbPurchase_Price.Text)
            .SaleDT = GetDBDate(tbSaleDT.Text)
            .SalePrice = wpm_GetDBDouble(tbSalePrice.Text)

            .ShowWherePurchased = tbShow_Purchase.Text
            .SpecimenNotes = wpm_GetDBString(tbSpecimenNotes.Text)
            .SpecimenNumber = wpm_GetDBString(tbSpecimenNumber.Text)
            .StorageLocation = wpm_GetDBString(tbStorage_Location.Text)
            .ThicknessCm = wpm_GetDBDouble(tbThicknessCm.Text)
            .ThicknessIn = wpm_GetDBDouble(tbThicknessIn.Text)
            .Value = wpm_GetDBDouble(tbValue.Text)
            .MineralVariety = wpm_GetDBString(tbVariety.Text)
            .WeightGr = wpm_GetDBDouble(tbWeightGr.Text)
            .WeightKg = wpm_GetDBDouble(tbWeightKg.Text)
            .WidthCm = wpm_GetDBDouble(tbWidthCm.Text)
            .WidthIn = wpm_GetDBDouble(tbWidthIn.Text)

            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.CollectionItems.InsertOnSubmit(myCollectionItem)
        If SaveChanges(AlertBox) Then
            hfCollectionItemID.Value = (From i In myCon.CollectionItems Where i.SpecimenNumber = myCollectionItem.SpecimenNumber Select i.CollectionItemID).Single
            OnUpdated(Me)
            Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
        End If
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCollectionItem = (From i In myCon.CollectionItems Where i.CollectionItemID = hfCollectionItemID.Value).Single
        myCon.CollectionItems.DeleteOnSubmit(myCollectionItem)
        If SaveChanges(AlertBox) Then
            OnUpdated(Me)
            Response.Redirect("/MineralCollection/Admin.aspx?Type=Specimen")
        End If
    End Sub

    Public Shared Function GetSelectedItemValue(ByRef cb As DropDownList) As String
        If cb.SelectedItem Is Nothing Then
            Return String.Empty
        Else
            Return cb.SelectedItem.Value.ToString
        End If
    End Function

    Public Shared Sub SetSelectedValue(ByRef myCMB As DropDownList, ByVal Value As Object)
        If Value Is Nothing Then
            myCMB.SelectedIndex = 0
        Else
            myCMB.SelectedValue = Value
        End If
    End Sub
    Public Shared Sub SetDateTextbox(ByRef myTB As TextBox, ByVal value As Object)
        If value Is Nothing Then
            myTB.Text = String.Empty
        Else
            If IsDate(value) Then
                myTB.Text = GetDBDate(value)
            Else
                myTB.Text = String.Empty
            End If
        End If
    End Sub


    Protected Sub cmd_RefreshImages_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub
End Class
