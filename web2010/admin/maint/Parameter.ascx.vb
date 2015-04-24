Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_Parameter
    Inherits ApplicationUserControl

    Private Property reqParameterID As String

    Public Const STR_SELECT_CompanySiteParameterList As String = "SELECT CompanySiteParameterID, CompanyID, SiteParameterTypeID, SortOrder, ParameterValue, PageID, SiteCategoryGroupID FROM CompanySiteParameter;"

    Public Const STR_UPDATE_CompanySiteParameter As String = "UPDATE CompanySiteParameter SET CompanyID=@CompanyID, SiteParameterTypeID=@SiteParameterTypeID, SortOrder=@SortOrder, ParameterValue=@ParameterValue, PageID=@PageID, SiteCategoryGroupID=@SiteCategoryGroupID WHERE (((CompanySiteParameterID)=@CompanySiteParameterID));"

    Public Const STR_INSERT_CompanySiteParameter As String = "INSERT INTO CompanySiteParameter ( CompanyID, SiteParameterTypeID, SortOrder, ParameterValue, PageID, SiteCategoryGroupID ) VALUES ( @CompanyID, @SiteParameterTypeID, @SortOrder, @ParameterValue, @PageID, @SiteCategoryGroupID ); "

    Public Const STR_DELETE_CompanySiteParameter As String = "DELETE FROM [CompanySiteParameter] WHERE (((CompanySiteParameterID)=@CompanySiteParameterID));"


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqParameterID = wpm_GetProperty("ParameterID", String.Empty)
        litParameterID.Text = reqParameterID
        If Not IsPostBack Then
            If reqParameterID = "NEW" Or reqParameterID = "0" Then
                ' Insert Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                PopulateParameterType(reqParameterID)

            ElseIf reqParameterID <> String.Empty Then
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

        If myParameter is Nothing then 
            myParameter = New Parameter With {.ParameterTypeID= wpm_GetIntegerProperty("ParameterTypeID",0)  }
        End If

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
        ddlLocation.DataSource = (From i In masterPage.myCompany.LocationList Where i.RecordSource = "Page" Or i.RecordSource = "Category" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
        ddlLocation.DataTextField = "Location"
        ddlLocation.DataValueField = "LocationID"
        ddlLocation.DataBind()

        With myParameter
            hfParameterID.Value = .ParameterID
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
        Dim myParameterType As Parameter = GetSiteParameterByParameterID(hfParameterID.Value)
        With myParameterType
            .CompanyID = ddlCompany.SelectedValue
            .ParameterTypeID = ddlParameterTypeID.SelectedValue
            .SortOrder = wpm_GetDBInteger(tbSortOrder.Text, 999)
            .ParameterValue = wpm_GetDBString(tbParameterValue.Text)
            .SiteCategoryTypeID = ddlLocation.SelectedValue.Replace("CAT-","")
            .LocationID = ddlLocation.SelectedValue
            .LocationGroupID = ddlLocationGroupID.SelectedValue
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_CompanySiteParameter}
                    wpm_AddParameterValue("@CompanyID", myParameterType.CompanyID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeID", myParameterType.ParameterTypeID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SortOrder", myParameterType.SortOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@ParameterValue", myParameterType.ParameterValue, cmd)
                    wpm_AddParameterValue("@PageID", myParameterType.LocationID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID", myParameterType.LocationGroupID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@CompanySiteParameterID", myParameterType.CompanySiteParameterID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_CompanySiteParameter, "Parameter.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterByParameterID(String.Empty)
        With myParameterType
            .CompanyID = ddlCompany.SelectedValue
            .ParameterTypeID = ddlParameterTypeID.SelectedValue
            .SortOrder = wpm_GetDBInteger(tbSortOrder.Text, 999)
            .ParameterValue = wpm_GetDBString(tbParameterValue.Text)
            .SiteCategoryTypeID = ddlLocation.SelectedValue.Replace("CAT-","")
            .LocationID = ddlLocation.SelectedValue
            .LocationGroupID = ddlLocationGroupID.SelectedValue
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_CompanySiteParameter}
                    wpm_AddParameterValue("@CompanyID", myParameterType.CompanyID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeID", myParameterType.ParameterTypeID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SortOrder", myParameterType.SortOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@ParameterValue", myParameterType.ParameterValue, cmd)
                    wpm_AddParameterValue("@PageID", myParameterType.LocationID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID", myParameterType.LocationGroupID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_CompanySiteParameter, "Parameter.acsx - cmd_SaveNew_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterByParameterID(String.Empty)
        With myParameterType
            .CompanyID = ddlCompany.SelectedValue
            .ParameterTypeID = ddlParameterTypeID.SelectedValue
            .SortOrder = wpm_GetDBInteger(tbSortOrder.Text, 999)
            .ParameterValue = wpm_GetDBString(tbParameterValue.Text)
            .SiteCategoryTypeID = ddlLocation.SelectedValue.Replace("CAT-","")
            .LocationID = ddlLocation.SelectedValue
            .LocationGroupID = ddlLocationGroupID.SelectedValue
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_CompanySiteParameter}
                    wpm_AddParameterValue("@CompanyID", myParameterType.CompanyID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeID", myParameterType.ParameterTypeID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SortOrder", myParameterType.SortOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@ParameterValue", myParameterType.ParameterValue, cmd)
                    wpm_AddParameterValue("@PageID", myParameterType.LocationID, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID", myParameterType.LocationGroupID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_CompanySiteParameter, "Parameter.acsx - cmd_SaveNew_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myParameter As Parameter = GetSiteParameterByParameterID(hfParameterID.Value)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_CompanySiteParameter}

                    wpm_AddParameterValue("@CompanySiteParameterID", myParameter.CompanySiteParameterID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_CompanySiteParameter, "Parameter.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Private Function GetSiteParameterByParameterID(reqParameterID As String) As Parameter
        Dim myParameter = (From i In masterPage.myCompany.SiteParameterList Where i.ParameterID = reqParameterID).SingleOrDefault
        If myParameter Is Nothing Then
            myParameter = New Parameter
        End If
        Return myParameter
    End Function

End Class
