Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_ParameterType
    Inherits ApplicationUserControl


    ' Parameter Type 
    Public Const STR_SiteParameterTypeID As String = "SiteParameterTypeID"
    Public Const STR_SELECTSiteParameterTypeList As String = "SELECT SiteParameterType.[SiteParameterTypeID], SiteParameterType.[SiteParameterTypeNM], SiteParameterType.[SiteParameterTypeDS], SiteParameterType.[SiteParameterTypeOrder], SiteParameterType.[SiteParameterTemplate] FROM SiteParameterType;"

    Public Const STR_SELECT_SiteParameterTypeBySiteParameterTypeID As String = "SELECT SiteParameterType.[SiteParameterTypeID], SiteParameterType.[SiteParameterTypeNM], SiteParameterType.[SiteParameterTypeDS], SiteParameterType.[SiteParameterTypeOrder], SiteParameterType.[SiteParameterTemplate] FROM SiteParameterType where SiteParameterType.[SiteParameterTypeID]=@SiteParameterTypeID ;"

    Public Const STR_UPDATE_SiteParameterType As String = "UPDATE SiteParameterType SET SiteParameterType.SiteParameterTypeNM = @SiteParameterTypeNM ,   SiteParameterType.[SiteParameterTypeDS]=@SiteParameterTypeDS, SiteParameterType.[SiteParameterTypeOrder]=@SiteParameterTypeOrder, SiteParameterType.[SiteParameterTemplate]=@SiteParameterTemplate WHERE (((SiteParameterType.SiteParameterTypeID)=@SiteParameterTypeID));"

    Public Const STR_INSERT_SiteParameterType As String = "INSERT INTO SiteParameterType ( SiteParameterTypeNM  , SiteParameterTypeDS , SiteParameterTypeOrder, SiteParameterTemplate ) VALUES ( @SiteParameterTypeNM, @SiteParameterTypeDS, @SiteParameterTypeOrder, @SiteParameterTemplate ); "

    Public Const STR_DELETE_SiteParameterType As String = "DELETE FROM [SiteParameterType] WHERE (((SiteParameterType.SiteParameterTypeID)=@SiteParameterTypeID));"

    Private Property reqSiteParameterTypeID As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteParameterTypeID = wpm_GetIntegerProperty("ParameterTypeID", -1)
        litParameterTypeID.Text = reqSiteParameterTypeID
        If Not IsPostBack Then
            cmd_Delete.Visible=False
            If reqSiteParameterTypeID > 0 Then
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                PopulateParameterType()
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Cancel.Visible = True
                cmd_SaveNew.Visible = True
            Else
                If reqSiteParameterTypeID = 0 Then
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Cancel.Visible = True
                    cmd_SaveNew.Visible = False
                Else
                    ' Show the list
                    pnlEdit.Visible = False
                    dtList.Visible = True
                    Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Parameter Types ( <a href='/admin/maint/default.aspx?type=ParameterType&ParameterTypeID=0&' >Add New Type</a>)"}
                    myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "ParameterTypeDS"})
                    myListHeader.DetailKeyName = "ParameterTypeID"
                    myListHeader.DetailFieldName = "ParameterTypeNM"
                    myListHeader.AddHeaderItem("SortOrder", "SortOrder")
                    myListHeader.DetailPath = "/admin/maint/default.aspx?type=ParameterType&ParameterTypeID={0}"
                    Dim myList As New List(Of Object)
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.RecordSource = "SiteParameterType" Select i).ToList())
                    dtList.BuildTable(myListHeader, myList)
                End If
            End If
        End If
    End Sub


    Public Sub PopulateParameterType()
        Dim myParameterType = (From i In masterPage.myCompany.SiteParameterList Where i.RecordSource = "SiteParameterType" And i.ParameterTypeID = litParameterTypeID.Text).SingleOrDefault
        With myParameterType
            tbParameterTypeNM.Text = .ParameterTypeNM
            tbParameterTypeDS.Text = .ParameterTypeDS
            tbParameterTemplate.Text = .ParameterValue
            tbSortOrder.Text = .SortOrder
            hfRecordSource.Value = .RecordSource
        End With


        LoadDisplyTableParameter(0, myParameterType.ParameterTypeID,dtParameterUsage)


        'Dim myListHeader As New DisplayTableHeader() With {.TableTitle = String.Format("Parameter Usage ( <a href='/admin/maint/default.aspx?type=Parameter&ParameterID=NEW&ParameterTypeID={0}'>New Parameter Usage</a>)", myParameterType.ParameterTypeID)}
        'myListHeader.AddHeaderItem("Location", "LocationNM", "/admin/maint/default.aspx?Type=Parameter&LocationID={0}", "LocationID", "LocationNM")
        'myListHeader.AddHeaderItem("Company", "CompanyNM", "/admin/maint/default.aspx?Type=Parameter&CompanyID={0}", "CompanyID", "CompanyNM")
        'myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "ParameterTypeDS", .Value = "ParameterTypeDS"})
        'myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "SortOrder", .Value = "SortOrder"})
        'myListHeader.DetailKeyName = "ParameterID"
        'myListHeader.DetailFieldName = "ParameterNM"
        'myListHeader.DetailPath = "/admin/maint/default.aspx?type=Parameter&ParameterID={0}"
        Dim myList As New List(Of Object)

        myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.ParameterTypeID = myParameterType.ParameterTypeID And i.ParameterID <> myParameterType.ParameterID Select i).ToList())

'        dtParameterUsage.BuildTable(myListHeader, myList)

        If myList.Count > 0 Then
            cmd_Delete.Visible = False
        Else
            cmd_Delete.Visible = True
        End If

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterTypeByParameterTypeID(litParameterTypeID.Text)
        With myParameterType
            .ParameterTypeNM = wpm_GetDBString(tbParameterTypeNM.Text)
            .ParameterTypeDS = wpm_GetDBString(tbParameterTypeDS.Text)
            .ParameterTypeOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
            .ParameterTemplate = wpm_GetDBString(tbParameterTemplate.Text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_SiteParameterType}
                    wpm_AddParameterStringValue("@SiteParameterTypeNM", myParameterType.ParameterTypeNM, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTypeDS", myParameterType.ParameterTypeDS, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeOrder", myParameterType.ParameterTypeOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTemplate", myParameterType.ParameterTemplate, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeID", myParameterType.ParameterTypeID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_SiteParameterType, "ParameterType.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterTypeByParameterTypeID("0")
        With myParameterType
            .ParameterTypeNM = wpm_GetDBString(tbParameterTypeNM.Text)
            .ParameterTypeDS = wpm_GetDBString(tbParameterTypeDS.Text)
            .ParameterTypeOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
            .ParameterTemplate = wpm_GetDBString(tbParameterTemplate.Text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteParameterType}
                    wpm_AddParameterStringValue("@SiteParameterTypeNM", myParameterType.ParameterTypeNM, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTypeDS", myParameterType.ParameterTypeDS, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeOrder", myParameterType.ParameterTypeOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTemplate", myParameterType.ParameterTemplate, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_SiteParameterType, "ParameterType.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterTypeByParameterTypeID("0")
        With myParameterType
            .ParameterTypeNM = wpm_GetDBString(tbParameterTypeNM.Text)
            .ParameterTypeDS = wpm_GetDBString(tbParameterTypeDS.Text)
            .ParameterTypeOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
            .ParameterTemplate = wpm_GetDBString(tbParameterTemplate.Text)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteParameterType}
                    wpm_AddParameterStringValue("@SiteParameterTypeNM", myParameterType.ParameterTypeNM, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTypeDS", myParameterType.ParameterTypeDS, cmd)
                    wpm_AddParameterValue("@SiteParameterTypeOrder", myParameterType.ParameterTypeOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterStringValue("@SiteParameterTemplate", myParameterType.ParameterTemplate, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_SiteParameterType, "ParameterType.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim myParameterType As Parameter = GetSiteParameterTypeByParameterTypeID(litParameterTypeID.Text)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_SiteParameterType}
                    wpm_AddParameterValue("@SiteParameterTypeID", myParameterType.ParameterTypeID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_SiteParameterType, "ParameterType.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Private Function GetSiteParameterTypeByParameterTypeID(ByVal reqParameterTypeID As String) As Parameter
        Dim myParameterType As New Parameter With {.ParameterTypeID = reqParameterTypeID}
        If IsNumeric(reqParameterTypeID) AndAlso CInt(reqParameterTypeID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_SiteParameterTypeBySiteParameterTypeID, reqParameterTypeID), "ParameterType").Rows
                With myParameterType
                    .ParameterTypeID = wpm_GetDBString(myRow("ParameterTypeID"))
                    .ParameterTypeNM = wpm_GetDBString(myRow("ParameterTypeNM"))
                    .ParameterValue = wpm_GetDBString(myRow("SiteParameterTemplate"))
                    .ParameterTypeOrder = wpm_GetDBInteger(myRow("SiteParameterTypeOrder", 99))
                End With
                Exit For
            Next
        Else
            myParameterType.ParameterTypeID = "0"
        End If
        Return myParameterType
    End Function


End Class
