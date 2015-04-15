Imports WebProjectMechanics

Public Class admin_maint_Parameter
    Inherits ApplicationUserControl

    Private Property reqParameterID As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqParameterID = wpm_GetProperty("ParameterID", String.Empty)
        litParameterTypeID.Text = reqParameterID
        If Not IsPostBack Then
            If reqParameterID <> String.Empty Then
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                PopulateParameterType(reqParameterID)
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True


                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Parameter (<a href='/admin/maint/default.aspx?type=Parameter'>All Parameters</a> , <a href='/admin/maint/default.aspx?type=Parameter&ParameterID=NEW'>Add New Parameter</a>)"}

                myListHeader.AddHeaderItem("Parameter Type", "ParameterTypeNM", "/admin/maint/default.aspx?Type=Parameter&ParameterTypeID={0}", "ParameterTypeID", "ParameterTypeNM")
                myListHeader.AddHeaderItem("Location", "LocationNM", "/admin/maint/default.aspx?Type=Parameter&LocationID={0}", "LocationID", "LocationNM")
                myListHeader.AddHeaderItem("Location Group", "LocationGroupID", "/admin/maint/default.aspx?Type=Parameter&LocationGroupID={0}", "LocationGroupID", "LocationGroupNM")
                myListHeader.AddHeaderItem("Site", "CompanyNM", "/admin/maint/default.aspx?Type=Parameter&CompanyID={0}", "CompanyID", "CompanyNM")

                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "ParameterTypeDS", .Value = "ParameterTypeDS"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "SortOrder", .Value = "SortOrder"})

                myListHeader.DetailKeyName = "ParameterID"
                myListHeader.DetailFieldName = "ParameterNM"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=Parameter&ParameterID={0}"
                Dim myList As New List(Of Object)

                If wpm_GetIntegerProperty("ParameterTypeID", 0) > 0 Then
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.ParameterTypeID = wpm_GetIntegerProperty("ParameterTypeID", 0) Select i).ToList())
                ElseIf wpm_GetIntegerProperty("CompanyID", 0) > 0 Then
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.CompanyID = wpm_GetIntegerProperty("CompanyID", 0).ToString() Select i).ToList())
                ElseIf wpm_GetProperty("LocationID", String.Empty) <> String.Empty Then
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.LocationID = wpm_GetProperty("LocationID", String.Empty) Select i).ToList())
                Else
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Select i).ToList())

                End If

                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub


    Public Sub PopulateParameterType(ByVal reqParameterID As String)
        If reqParameterID.Substring(0, 2) = "PT" Then
            Response.Redirect(String.Format("/admin/maint/default.aspx?Type=ParameterType&ParameterTypeID={0}", reqParameterID.Replace("PT-", String.Empty)))
        End If
        Dim myParameter As Parameter = (From i In masterPage.myCompany.SiteParameterList Where i.ParameterID = reqParameterID).SingleOrDefault
        'Public Property ParameterTypeID As Integer
        'Public Property SiteCategoryTypeID() As String
        'Public Property ParameterValue() As String


        ddlParameterTypeID.DataSource = (From i In masterPage.myCompany.SiteParameterList Order By i.ParameterTypeNM Where i.RecordSource = "SiteParameterType" Select i.ParameterTypeID, i.ParameterTypeNM).ToList()

        ddlParameterTypeID.DataTextField = "ParameterTypeNM"
        ddlParameterTypeID.DataValueField = "ParameterTypeID"
        ddlParameterTypeID.DataBind()


        ddlLocationGroupID.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Groups"})
        ddlLocationGroupID.AppendDataBoundItems = True
        ddlLocationGroupID.DataSource = (From i In masterPage.myCompany.LocationGroupList Order By i.LocationGroupNM Select i.LocationGroupID, i.LocationGroupNM).ToList()
        ddlLocationGroupID.DataTextField = "LocationGroupNM"
        ddlLocationGroupID.DataValueField = "LocationGroupID"
        ddlLocationGroupID.DataBind()


        ddlLocation.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Locations"})
        ddlLocation.AppendDataBoundItems = True
        ddlLocation.DataSource = (From i In masterPage.myCompany.LocationList Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
        ddlLocation.DataTextField = "Location"
        ddlLocation.DataValueField = "LocationID"
        ddlLocation.DataBind()

        With myParameter
            hfCompanySiteParameterID.Value = .CompanySiteParameterID
            ddlParameterTypeID.SelectedValue = .ParameterTypeID
            ddlLocationGroupID.SelectedValue = .LocationGroupID
            tbParameterTypeDS.Text = .ParameterTypeDS
            tbParameterTypeDS.Enabled = False
            tbParameterValue.Text = .ParameterValue
            tbSortOrder.Text = .SortOrder
            ddlLocation.SelectedValue = .LocationID
            wmp_LoadCompanyDropDow(ddlCompany, .CompanyID, False)
            hfRecordSource.Value = .RecordSource
        End With
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)
    End Sub
End Class
