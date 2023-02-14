Imports WebProjectMechanics
Imports System

Partial Class MineralCollection_Admin
    Inherits AdminPage
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SpecimenList1.Visible = False
        pnlSearchOptions.Visible = False
        pnlFilter.Visible = False

        Select Case wpm_GetProperty("Type", String.Empty)
            Case "Specimen"
                If wpm_GetIntegerProperty("CollectionItemID", -1) = 0 Then
                    'Add New Specimen
                    myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/CollectionItemEdit.ascx"), ApplicationUserControl)
                    SetListPageURL("CollectionItemID", "Specimen")
                    pnlMaintenance.Visible = True
                    pnlMaintenance.Controls.Add(myControl)
                ElseIf wpm_GetIntegerProperty("CollectionItemID", -1) > 0 Then
                    'Edit Specimen
                    myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/CollectionItemEdit.ascx"), ApplicationUserControl)
                    SetListPageURL("CollectionItemID", "Specimen")
                    pnlMaintenance.Visible = True
                    pnlMaintenance.Controls.Add(myControl)
                Else
                    'List Specimens
                    pnlSearchOptions.Visible = True
                    pnlFilter.Visible = True
                    SpecimenList1.Visible = True
                    TryCast(SpecimenList1, IApplicationList).GetData(TryCast(SpecimenFilter1, IApplicationFilter).GetFilterClause(String.Empty), False)
                End If
            Case "Image"
                If wpm_GetIntegerProperty("CollectionItemImageID", -1) = 0 Then
                    Response.Redirect("/MineralCollection/Gallery/EditSpecimenImage.aspx?CollectionItemImageID=" & wpm_GetIntegerProperty("CollectionItemImageID", -1))
                ElseIf wpm_GetIntegerProperty("CollectionItemImageID", -1) > 0 Then
                    Response.Redirect("/MineralCollection/Gallery/EditSpecimenImage.aspx?CollectionItemImageID=" & wpm_GetIntegerProperty("CollectionItemImageID", -1))
                Else
                    Response.Redirect("/MineralCollection/Gallery/ListImages.aspx?Filter=Assigned")
                End If

            Case "Mineral"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/MineralEdit.ascx"), ApplicationUserControl)
                SetListPageURL("MineralID", "Mineral")
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case "Company"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/CompanyEdit.ascx"), ApplicationUserControl)
                SetListPageURL("CompanyID", "Company")
                AddHandler myControl.cmd_Updated, AddressOf RedirectToListPage
                AddHandler myControl.cmd_Canceled, AddressOf RedirectToListPage
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case "Collection"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/CollectionEdit.ascx"), ApplicationUserControl)
                SetListPageURL("CollectionID", "Collection")
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case "City"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/LocationCityEdit.ascx"), ApplicationUserControl)
                SetListPageURL("LocationCityID", "City")
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case "State"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/LocationStateEdit.ascx"), ApplicationUserControl)
                SetListPageURL("LocationStateID", "State")
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case "Country"
                myControl = DirectCast(Page.LoadControl("~/MineralCollection/controls/LocationCountryEdit.ascx"), ApplicationUserControl)
                SetListPageURL("LocationCountryID", "Country")
                pnlMaintenance.Visible = True
                pnlMaintenance.Controls.Add(myControl)
            Case Else
                'List Specimens
                pnlSearchOptions.Visible = True
                pnlFilter.Visible = True
                SpecimenList1.Visible = True
                TryCast(SpecimenList1, IApplicationList).GetData(TryCast(SpecimenFilter1, IApplicationFilter).GetFilterClause(String.Empty), False)
        End Select
    End Sub

    Protected Sub cmd_Search_Click(sender As Object, e As EventArgs)
        Select Case wpm_GetProperty("Type", String.Empty)
            Case "Specimen"
                SpecimenList1.Visible = True
                TryCast(SpecimenList1, IApplicationList).GetData(TryCast(SpecimenFilter1, IApplicationFilter).GetFilterClause(String.Empty), False)
            Case Else
                SpecimenList1.Visible = False
        End Select
    End Sub
End Class
