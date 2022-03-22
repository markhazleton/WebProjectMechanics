Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_SiteCategoryGroup
    Inherits ApplicationUserControl


    ' SiteCategoryGroup
    Public Const STR_SiteCategoryGroupID As String = "SiteCategoryGroupID"

    Public Const STR_SELECTSiteCategoryGroupList As String = "SELECT SiteCategoryGroup.[SiteCategoryGroupID], SiteCategoryGroup.[SiteCategoryGroupNM], SiteCategoryGroup.[SiteCategoryGroupDS], SiteCategoryGroup.[SiteCategoryGroupOrder] FROM SiteCategoryGroup;"

    Public Const STR_SELECT_SiteCategoryGroupBySiteCategoryGroupID As String = "SELECT SiteCategoryGroup.[SiteCategoryGroupID], SiteCategoryGroup.[SiteCategoryGroupNM], SiteCategoryGroup.[SiteCategoryGroupDS], SiteCategoryGroup.[SiteCategoryGroupOrder] FROM SiteCategoryGroup where SiteCategoryGroup.[SiteCategoryGroupID]=@SiteCategoryGroupID ;"

    Public Const STR_UPDATE_SiteCategoryGroup As String = "UPDATE SiteCategoryGroup SET SiteCategoryGroup.SiteCategoryGroupNM = @SiteCategoryGroupNM ,   SiteCategoryGroup.[SiteCategoryGroupDS]=@SiteCategoryGroupDS, SiteCategoryGroup.[SiteCategoryGroupOrder]=@SiteCategoryGroupOrder WHERE SiteCategoryGroup.SiteCategoryGroupID=@SiteCategoryGroupID ;"

    Public Const STR_INSERT_SiteCategoryGroup As String = "INSERT INTO SiteCategoryGroup ( SiteCategoryGroupNM  , SiteCategoryGroupDS , SiteCategoryGroupOrder ) VALUES ( @SiteCategoryGroupNM, @SiteCategoryGroupDS, @SiteCategoryGroupOrder ); "

    Public Const STR_DELETE_SiteCategoryGroup As String = "DELETE FROM [SiteCategoryGroup] WHERE (((SiteCategoryGroup.SiteCategoryGroupID)=@SiteCategoryGroupID));"

    Private Property reqSiteCategoryGroupID As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteCategoryGroupID = wpm_GetIntegerProperty("SiteCategoryGroupID", -1)
        litSiteCategoryGroupID.Text = reqSiteCategoryGroupID
        If Not IsPostBack Then
            cmd_Delete.Visible = False
            If reqSiteCategoryGroupID > 0 Then
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                PopulateSiteCategoryGroup()
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Cancel.Visible = True
                cmd_SaveNew.Visible = True
            Else
                If reqSiteCategoryGroupID = 0 Then
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
                    Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Location Group ( <a href='/admin/maint/default.aspx?type=SiteCategoryGroup&SiteCategoryGroupID=0&' >Add New Group</a>)"}
                    myListHeader.DetailKeyName = "LocationGroupID"
                    myListHeader.DetailFieldName = "LocationGroupNM"
                    myListHeader.DetailDisplayName = "Group Name"
                    myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "LocationGroupDS"})
                    myListHeader.AddHeaderItem("Order", "LocationGroupOrder")
                    myListHeader.DetailPath = "/admin/maint/default.aspx?type=SiteCategoryGroup&SiteCategoryGroupID={0}"
                    Dim myList As New List(Of Object)
                    myList.AddRange((From i In masterPage.myCompany.LocationGroupList Select i).ToList())
                    dtList.BuildTable(myListHeader, myList)
                End If
            End If
        End If
    End Sub
    Public Sub PopulateSiteCategoryGroup()
        Dim mySiteCategoryGroup = (From i In masterPage.myCompany.LocationGroupList Where i.LocationGroupID = litSiteCategoryGroupID.Text).SingleOrDefault
        With mySiteCategoryGroup
            litSiteCategoryGroupID.Text = .LocationGroupID
            tbSiteCategoryGroupNM.Text = .LocationGroupNM
            tbSiteCategoryGroupDS.Text = .LocationGroupDS
            tbSortOrder.Text = .LocationGroupOrder
        End With

        LoadDisplyTableParameter(0, mySiteCategoryGroup.LocationGroupID, dtParameterUsage)

        Dim myList As New List(Of Object)

        '        myList.AddRange((From i In masterPage.myCompany.LocationGroupList Where i.LocationGroupID= mySiteCategoryGroup.LocationGroupID And i.ParameterID <> mySiteCategoryGroup.ParameterID Select i).ToList())

        If myList.Count > 0 Then
            cmd_Delete.Visible = False
        Else
            cmd_Delete.Visible = True
        End If

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        Dim mySiteCategoryGroup As LocationGroup = GetSiteCategoryGroupBySiteCategoryGroupID(litSiteCategoryGroupID.Text)
        With mySiteCategoryGroup
            .LocationGroupNM = wpm_GetDBString(tbSiteCategoryGroupNM.text)
            .LocationGroupDS = wpm_GetDBString(tbSiteCategoryGroupDS.Text)
            .LocationGroupOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_UPDATE_SiteCategoryGroup}
                    wpm_AddParameterStringValue("@SiteCategoryGroupNM", mySiteCategoryGroup.LocationGroupNM, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryGroupDS", mySiteCategoryGroup.LocationGroupDS, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupOrder", mySiteCategoryGroup.LocationGroupOrder, SqlDbType.BigInt, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID", mySiteCategoryGroup.LocationGroupID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_UPDATE_SiteCategoryGroup, "SiteCategoryGroup.acsx - cmd_Update_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        Dim mySiteCategoryGroup As LocationGroup = GetSiteCategoryGroupBySiteCategoryGroupID("0")
        With mySiteCategoryGroup
            .LocationGroupNM = wpm_GetDBString(tbSiteCategoryGroupNM.Text)
            .LocationGroupDS = wpm_GetDBString(tbSiteCategoryGroupDS.Text)
            .LocationGroupOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteCategoryGroup}
                    wpm_AddParameterStringValue("@SiteCategoryGroupNM", mySiteCategoryGroup.LocationGroupNM, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryGroupDS", mySiteCategoryGroup.LocationGroupDS, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupOrder", mySiteCategoryGroup.LocationGroupOrder, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_SiteCategoryGroup, "SiteCategoryGroup.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        Dim mySiteCategoryGroup As LocationGroup = GetSiteCategoryGroupBySiteCategoryGroupID("0")
        With mySiteCategoryGroup
            .LocationGroupNM = wpm_GetDBString(tbSiteCategoryGroupNM.Text)
            .LocationGroupDS = wpm_GetDBString(tbSiteCategoryGroupDS.Text)
            .LocationGroupOrder = wpm_GetDBInteger(tbSortOrder.Text, 99)
        End With
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_INSERT_SiteCategoryGroup}
                    wpm_AddParameterStringValue("@SiteCategoryGroupNM", mySiteCategoryGroup.LocationGroupNM, cmd)
                    wpm_AddParameterStringValue("@SiteCategoryGroupDS", mySiteCategoryGroup.LocationGroupDS, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupOrder", mySiteCategoryGroup.LocationGroupOrder, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_INSERT_SiteCategoryGroup, "SiteCategoryGroup.acsx - cmd_Insert_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        Dim mySiteCategoryGroup As LocationGroup = GetSiteCategoryGroupBySiteCategoryGroupID(litSiteCategoryGroupID.Text)
        Dim iRowsAffected As Integer = 0
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            Try
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = STR_DELETE_SiteCategoryGroup}
                    wpm_AddParameterValue("@SiteCategoryGroupID", mySiteCategoryGroup.LocationGroupID, SqlDbType.BigInt, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(STR_DELETE_SiteCategoryGroup, "SiteCategoryGroup.acsx - cmd_Delete_Click")
            End Try
        End Using
        OnUpdated(Me)
    End Sub

    Private Function GetSiteCategoryGroupBySiteCategoryGroupID(ByVal reqSiteCategoryGroupID As String) As LocationGroup
        Dim mySiteCategoryGroup As New LocationGroup With {.LocationGroupID = reqSiteCategoryGroupID}
        If IsNumeric(reqSiteCategoryGroupID) AndAlso CInt(reqSiteCategoryGroupID) > 0 Then
            For Each myRow In wpm_GetDataTable(String.Format(STR_SELECT_SiteCategoryGroupBySiteCategoryGroupID, reqSiteCategoryGroupID), "SiteCategoryGroup").Rows
                With mySiteCategoryGroup
                    .LocationGroupID = wpm_GetDBString(myRow("SiteCategoryGroupID"))
                    .LocationGroupNM = wpm_GetDBString(myRow("SiteCategoryGroupNM"))
                    .LocationGroupDS = wpm_GetDBString(myRow("SiteCategoryGroupDS"))
                    .LocationGroupOrder = wpm_GetDBInteger(myRow("SiteCategoryGroupOrder", 99))
                End With
                Exit For
            Next
        Else
            mySiteCategoryGroup.LocationGroupOrder = "0"
        End If
        Return mySiteCategoryGroup
    End Function
    Public Sub LoadDisplyTableParameter(ByVal CompanyID As Integer, ByVal SiteCategoryGroupID As Integer, ByRef myControl As UserControl)
    End Sub

End Class
