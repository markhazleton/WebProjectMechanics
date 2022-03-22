Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_Parameter
    Inherits ApplicationUserControl
    '
    '  3 Types of Parameters
    '
    '  PT - Parameter Type the default list of parameters with their default values
    '  SP - Site Parameter created for a specific Company and/or Page
    '

    Private Property reqParameterID As String
    Private Property reqParameterTypeID As String

    Public Const STR_INSERT_CompanySiteParameter As String = "INSERT INTO CompanySiteParameter ( CompanyID, SiteParameterTypeID, SortOrder, ParameterValue, PageID, SiteCategoryGroupID ) VALUES ( @CompanyID, @SiteParameterTypeID, @SortOrder, @ParameterValue, @PageID, @SiteCategoryGroupID ); "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqParameterID = wpm_GetProperty("ParameterID", String.Empty)
        reqParameterTypeID = wpm_GetProperty("ParameterTypeID", String.Empty)

        If Not IsPostBack Then
            If reqParameterID = String.Empty Then
                ShowList()
            Else
                pnlEdit.Visible = True
                dtList.Visible = False
                If reqParameterID = "NEW" Or reqParameterID = "0" Then
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                    cmd_SaveNew.Visible = False
                Else
                    ' Edit Mode
                    cmd_Update.Visible = True
                    cmd_Insert.Visible = False
                    cmd_Delete.Visible = True
                    cmd_Cancel.Visible = True
                    cmd_SaveNew.Visible = True
                End If
                PopulateParameterType(reqParameterID)
            End If
        End If
    End Sub


    Public Sub PopulateParameterType(ByVal reqParameterID As String)
        If reqParameterID.Substring(0, 2) = "PT" Then
            Response.Redirect(String.Format("/admin/maint/default.aspx?Type=ParameterType&ParameterTypeID={0}", reqParameterID.Replace("PT-", String.Empty)))
            Exit Sub
        End If

        litParameterID.Text = reqParameterID
        Dim myParameter As Parameter = GetSiteParameterByParameterID(reqParameterID)

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
        If reqParameterID.Substring(0, 2) = "SP" Then
            ddlLocation.DataSource = (From i In masterPage.myCompany.LocationList Where i.RecordSource = "Page" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
        Else
            ddlLocation.DataSource = (From i In masterPage.myCompany.LocationList Where i.RecordSource = "Category" Order By i.LocationName Select i.LocationID, Location = String.Format("{0} ({1})", i.LocationName, i.RecordSource)).ToList()
        End If
        ddlLocation.DataTextField = "Location"
        ddlLocation.DataValueField = "LocationID"
        ddlLocation.DataBind()

        With myParameter
            hfParameterID.Value = .ParameterID

            If .ParameterTypeID < 1 Then
                ddlParameterTypeID.SelectedValue = reqParameterTypeID
            Else
                ddlParameterTypeID.SelectedValue = .ParameterTypeID
            End If

            ddlLocationGroupID.SelectedValue = .LocationGroupID
            tbParameterValue.Text = .ParameterValue
            tbSortOrder.Text = .SortOrder
            If reqParameterID.Substring(0, 2) = "SP" Then
                ddlLocation.SelectedValue = .LocationID
            Else
                ddlLocation.SelectedValue = "CAT-" & .LocationID
            End If

            wmp_LoadCompanyDropDow(ddlCompany, .CompanyID, False)
            hfRecordSource.Value = .RecordSource
        End With
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim mySQL As String = String.Empty

        Dim myParameter As Parameter = GetSiteParameterByParameterID(hfParameterID.Value)
        With myParameter
            .CompanyID = ddlCompany.SelectedValue
            .ParameterTypeID = ddlParameterTypeID.SelectedValue
            .SortOrder = wpm_GetDBInteger(tbSortOrder.Text, 999)
            .ParameterValue = wpm_GetDBString(tbParameterValue.Text)
            .LocationID = ddlLocation.SelectedValue.Replace("CAT-", String.Empty)
            .LocationGroupID = ddlLocationGroupID.SelectedValue
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                If myParameter.RecordSource = "CompanySiteParameter" Then
                    mySQL = "UPDATE CompanySiteParameter SET CompanyID=@CompanyID, SiteParameterTypeID=@SiteParameterTypeID, SortOrder=@SortOrder, ParameterValue=@ParameterValue, PageID=@PageID, SiteCategoryGroupID=@SiteCategoryGroupID WHERE (((CompanySiteParameterID)=@CompanySiteParameterID));"
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = mySQL}
                        wpm_AddParameterValue("@CompanyID", myParameter.CompanyID, SqlDbType.BigInt, cmd)
                        wpm_AddParameterValue("@SiteParameterTypeID", myParameter.ParameterTypeID, SqlDbType.BigInt, cmd)
                        wpm_AddParameterValue("@SortOrder", myParameter.SortOrder, SqlDbType.BigInt, cmd)
                        wpm_AddParameterStringValue("@ParameterValue", myParameter.ParameterValue, cmd)
                        wpm_AddParameterValue("@PageID", myParameter.LocationID, SqlDbType.BigInt, cmd)
                        wpm_AddParameterValue("@SiteCategoryGroupID", myParameter.LocationGroupID, SqlDbType.BigInt, cmd)
                        wpm_AddParameterValue("@CompanySiteParameterID", myParameter.CompanySiteParameterID, SqlDbType.BigInt, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Else
                    ' do nothing
                End If
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(mySQL, "Parameter.acsx - cmd_Update_Click")
                strErrorMessage = String.Format("Update Error:<br/>{0}", ex.Message)
            End Try
        End Using

        If String.IsNullOrWhiteSpace(strErrorMessage) Then
            OnUpdated(Me)
        Else
            litError.Text = String.Format("<blockquote>{0}</blockquote>", strErrorMessage)
        End If
    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)

        Dim myParameterType As Parameter = GetSiteParameterByParameterID(hfParameterID.Value)
        With myParameterType
            .CompanyID = ddlCompany.SelectedValue
            .ParameterTypeID = ddlParameterTypeID.SelectedValue
            .SortOrder = wpm_GetDBInteger(tbSortOrder.Text, 999)
            .ParameterValue = wpm_GetDBString(tbParameterValue.Text)
            .LocationID = ddlLocation.SelectedValue.Replace("CAT-", String.Empty)
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
                    wpm_AddParameterValue("@SiteCategoryID", Replace(myParameterType.LocationID, "CAT-", ""), SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryTypeID", DBNull.Value, SqlDbType.BigInt, cmd)
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
                    wpm_AddParameterValue("@SiteCategoryID", Replace(myParameterType.LocationID, "CAT-", ""), SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryTypeID", DBNull.Value, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID", myParameterType.LocationGroupID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_CompanySiteParameter, "Parameter.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myParameter As Parameter = GetSiteParameterByParameterID(hfParameterID.Value)
        Dim mySQL As String = String.Empty
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                mySQL = "DELETE FROM [CompanySiteParameter] WHERE (((CompanySiteParameterID)=@CompanySiteParameterID));"
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = mySQL}
                    wpm_AddParameterValue("@CompanySiteParameterID", myParameter.CompanySiteParameterID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(mySQL, "Parameter.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Private Sub ShowList()

        LoadDisplyTableParameter(wpm_GetIntegerProperty("CompanyID", 0), wpm_GetIntegerProperty("ParameterTypeID", 0), dtList)
        pnlEdit.Visible = False
        dtList.Visible = True
    End Sub
    Private Function GetSiteParameterByParameterID(reqParameterID As String) As Parameter
        If String.IsNullOrWhiteSpace(reqParameterID) Then
            reqParameterID = "TP-0"
        End If

        Dim myParameter = New Parameter With {.ParameterTypeID = wpm_GetDBInteger(reqParameterID)}
        If reqParameterID.Substring(0, 2) = "SP" Then
            For Each myRow In wpm_GetDataTable("SELECT CompanySiteParameter.CompanySiteParameterID, CompanySiteParameter.CompanyID, CompanySiteParameter.SiteParameterTypeID, CompanySiteParameter.SortOrder, CompanySiteParameter.ParameterValue, CompanySiteParameter.PageID, CompanySiteParameter.SiteCategoryGroupID, Company.SiteCategoryTypeID, Company.CompanyName, SiteCategoryGroup.SiteCategoryGroupNM, Page.PageName, SiteParameterType.SiteParameterTypeNM, 'CompanySiteParameter' AS RecordSource FROM SiteParameterType RIGHT JOIN (Page RIGHT JOIN (SiteCategoryGroup RIGHT JOIN (CompanySiteParameter LEFT JOIN Company ON CompanySiteParameter.CompanyID = Company.CompanyID) ON SiteCategoryGroup.SiteCategoryGroupID = CompanySiteParameter.SiteCategoryGroupID) ON Page.PageID = CompanySiteParameter.PageID) ON SiteParameterType.SiteParameterTypeID = CompanySiteParameter.SiteParameterTypeID where CompanySiteParameter.CompanySiteParameterID=" & reqParameterID.Replace("SP-", String.Empty), "CompanySiteParameter").Rows
                With myParameter
                    .CompanySiteParameterID = wpm_GetDBInteger(myRow("CompanySiteParameterID"))
                    .ParameterID = "SP-" & wpm_GetDBString(myRow("CompanySiteParameterID"))
                    .ParameterNM = String.Format("{0}-{1}", wpm_GetDBString(myRow("SiteParameterTypeNM")), wpm_GetDBString(myRow("CompanySiteParameterID")))
                    .RecordSource = wpm_GetDBString(myRow("RecordSource"))
                    .ParameterTypeID = wpm_GetDBInteger(myRow("SiteParameterTypeID"))
                    .ParameterTypeNM = wpm_GetDBString(myRow("SiteParameterTypeNM"))
                    .SortOrder = wpm_GetDBInteger(myRow("SortOrder"))
                    .CompanyID = wpm_GetDBString(myRow("CompanyID"))
                    .CompanyNM = wpm_GetDBString(myRow("CompanyName"))
                    .LocationID = wpm_GetDBString(myRow("PageID"))
                    .LocationNM = wpm_GetDBString(myRow("PageName"))
                    .LocationGroupID = wpm_GetDBString(myRow("SiteCategoryGroupID"))
                    .LocationGroupNM = wpm_GetDBString(myRow("SiteCategoryGroupNM"))
                    .ParameterValue = wpm_GetDBString(myRow("ParameterValue"))
                    .RecordSource = "CompanySiteParameter"
                End With
            Next
        Else
            myParameter = (From i In masterPage.myCompany.SiteParameterList Where i.ParameterID = reqParameterID).SingleOrDefault
        End If

        If myParameter Is Nothing Then
            myParameter = New Parameter
        End If
        Return myParameter
    End Function

    Public Sub LoadDisplyTableParameter(ByVal CompanyID As Integer, ByVal ParameterTypeID As Integer, ByRef myControl As UserControl)
        Dim myListHeader As New DisplayTableHeader()
        Dim myList As New List(Of Object)()
        Dim sWhere As String = String.Format("where ( Company.CompanyID is null or Company.CompanyID = {0}) ) ", wpm_CurrentSiteID)
        If GetProperty("ALL", "FALSE") = "TRUE" Then
            sWhere = String.Empty
        ElseIf ParameterTypeID > 0 Then
            sWhere = String.Format("where SiteParameterType.SiteParameterTypeID = {0} ", ParameterTypeID)
        ElseIf CompanyID > 0 Then
            sWhere = String.Format("where Company.CompanyID = {0} ", CompanyID)
        ElseIf wpm_GetIntegerProperty("SiteCategoryTypeID", 0) > 0 Then
            sWhere = String.Format("where SiteCategoryType.SiteCategoryTypeID = {0} ", wpm_GetIntegerProperty("SiteCategoryTypeID", 0))
        End If
        Dim STR_SELECT_ParameterList As String = String.Format("SELECT CompanySiteParameter.CompanySiteParameterID, CompanySiteParameter.CompanyID, CompanySiteParameter.SiteParameterTypeID, CompanySiteParameter.SortOrder, CompanySiteParameter.ParameterValue, CompanySiteParameter.PageID, CompanySiteParameter.SiteCategoryGroupID, Company.SiteCategoryTypeID, Company.CompanyName, SiteCategoryGroup.SiteCategoryGroupNM, Page.PageName, SiteParameterType.SiteParameterTypeNM, 'CompanySiteParameter' AS RecordSource, SiteCategoryType.SiteCategoryTypeNM FROM SiteCategoryType RIGHT JOIN (SiteParameterType RIGHT JOIN (Page RIGHT JOIN (SiteCategoryGroup RIGHT JOIN (CompanySiteParameter LEFT JOIN Company ON CompanySiteParameter.CompanyID = Company.CompanyID) ON SiteCategoryGroup.SiteCategoryGroupID = CompanySiteParameter.SiteCategoryGroupID) ON Page.PageID = CompanySiteParameter.PageID) ON SiteParameterType.SiteParameterTypeID = CompanySiteParameter.SiteParameterTypeID) ON SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID  {0} ", sWhere)
        myListHeader = New DisplayTableHeader() With {.TableTitle = "Parameter (<a href='/admin/maint/default.aspx?type=Parameter&ALL=TRUE'>All Parameters</a> , <a href='/admin/maint/default.aspx?type=Parameter&ParameterID=NEW&ParameterTypeID=" & ParameterTypeID & "'>Add New Parameter</a>)"}
        myListHeader.AddLinkHeaderItem("Parameter Type", "ParameterTypeNM", "/admin/maint/default.aspx?Type=Parameter&ParameterTypeID={0}", "ParameterTypeID", "ParameterTypeNM")
        myListHeader.AddHeaderItem("Location", "LocationNM")
        myListHeader.AddLinkHeaderItem("Location Group", "LocationGroupID", "/admin/maint/default.aspx?Type=Parameter&LocationGroupID={0}", "LocationGroupID", "LocationGroupNM")
        myListHeader.AddLinkHeaderItem("Site", "CompanyNM", "/admin/maint/default.aspx?Type=Parameter&CompanyID={0}", "CompanyID", "CompanyNM")
        myListHeader.AddLinkHeaderItem("SiteCategoryTypeNM", "SiteCategoryTypeNM", "/admin/maint/default.aspx?Type=Parameter&SiteCategoryTypeID={0}", "SiteCategoryTypeID", "SiteCategoryTypeNM")
        myListHeader.AddHeaderItem("SortOrder", "SortOrder")
        myListHeader.DetailKeyName = "ParameterID"
        myListHeader.DetailFieldName = "ParameterNM"
        myListHeader.DetailPath = "/admin/maint/default.aspx?type=Parameter&ParameterID={0}&ParameterTypeID=" & ParameterTypeID & " "
        myList = New List(Of Object)()
        For Each myRow As DataRow In wpm_GetDataTable(String.Format("{0} ", STR_SELECT_ParameterList, wpm_CurrentSiteID), "Parameter").Rows
            myList.Add(New Parameter() With
                       {
                       .ParameterID = wpm_GetDBString(myRow("CompanySiteParameterID")),
                       .ParameterNM = String.Format("{0}-{1}", wpm_GetDBString(myRow("SiteParameterTypeNM")), wpm_GetDBString(myRow("CompanySiteParameterID"))),
                       .RecordSource = wpm_GetDBString(myRow("RecordSource")),
                       .ParameterTypeID = wpm_GetDBInteger(myRow("SiteParameterTypeID")),
                       .ParameterTypeNM = wpm_GetDBString(myRow("SiteParameterTypeNM")),
                       .SortOrder = wpm_GetDBInteger(myRow("SortOrder")),
                       .CompanyID = wpm_GetDBString(myRow("CompanyID")),
                       .CompanyNM = wpm_GetDBString(myRow("CompanyName")),
                       .LocationID = wpm_GetDBString(myRow("PageID")),
                       .LocationNM = wpm_GetDBString(myRow("PageName")),
                       .LocationGroupID = wpm_GetDBString(myRow("SiteCategoryGroupID")),
                       .LocationGroupNM = wpm_GetDBString(myRow("SiteCategoryGroupNM"))})
        Next
        Dim myDisplayTable = TryCast(myControl, Icontrols_DisplayTable)
        If Not myDisplayTable Is Nothing Then
            myDisplayTable.BuildTable(myListHeader, myList)
        End If
    End Sub



End Class
