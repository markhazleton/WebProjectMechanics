Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class Part
    Public Property Title() As String
    Public Property URL() As String
    Public Property Description() As String
    Public Property PartCategoryID() As String
    Public Property PartTypeCD() As String
    Public Property PartCategoryTitle() As String
    Public Property PartID() As String
    Public Property SiteCategoryTypeID() As String
    Public Property SiteCategoryGroupID() As String
    Public Property View() As Boolean
    Public Property ModifiedDT() As Date
    Public Property PartSortOrder() As Integer
    Public Property UserID() As String
    Public Property AmazonIndex() As String
    Public Property PartSource() As String
    Public Property CompanyID() As String
    Private _LocationID As String
    Public Property LocationID() As String
        Get
            Return _LocationID
        End Get
        Set(ByVal value As String)
            If value = "CAT-" Then
                value = ""
            End If
            _LocationID = value
        End Set
    End Property

    Sub UpdatePart()
        If CInt(PartID) > 0 Then
            UpdatePartDB(New Integer)
        Else
            InsertPartDB(New Integer)
        End If
    End Sub

    Sub DeletePart()
        wpm_RunDeleteSQL("DELETE FROM [Link] WHERE [ID] =" & PartID, "Link")
    End Sub

    Private Sub UpdatePartDB(ByRef iRowsAffected As Integer)
        Dim sSQL As String = "UPDATE [Link] SET [CompanyID] =@CompanyID, [LinkTypeCD] =@LinkTypeCD, [CategoryID] =@CategoryID, [PageID] =@PageID, [Title] =@Title, [Description] =@Description, [URL] = @URL, [DateAdd] =@DateAdd, [Ranks] = @Ranks, [Views] = @Views, [UserName] = @UserName, [UserID] = @UserID, [ASIN] = @ASIN, [SiteCategoryGroupID] = @SiteCategoryGroupID WHERE [ID] = @ID"
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            conn.Open()
            Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                Try
                    wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                    wpm_AddParameterValue("@CategoryID", PartCategoryID, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@PageID", LocationID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@Title", Title, cmd)
                    wpm_AddParameterStringValue("@Description", Description, cmd)
                    wpm_AddParameterStringValue("@URL", URL, cmd)
                    wpm_AddParameterValue("@DateAdd", ModifiedDT, SqlDbType.DateTime, cmd)
                    wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@Views", View, SqlDbType.Bit, cmd)
                    wpm_AddParameterStringValue("@UserName", String.Empty, cmd)
                    wpm_AddParameterValue("@UserID", UserID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID ", SiteCategoryGroupID, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@ID", PartID, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError(cmd.CommandText, "Part.UpdatePartDB")
                End Try
            End Using
        End Using
    End Sub

    Private Sub InsertPartDB(ByRef iRowsAffected As Integer)
        Dim sSQL As String = "INSERT INTO [Link] ( [CompanyID], [LinkTypeCD], [CategoryID], [PageID], [Title], [Description], [URL], [DateAdd], [Ranks], [Views], [UserName], [UserID], [ASIN], [SiteCategoryGroupID]) VALUES (@CompanyID,@LinkTypeCD,@CategoryID, @PageID, @Title, @Description, @URL, @DateAdd, @Ranks, @Views, @UserName, @UserID,  @ASIN, @SiteCategoryGroupID )"
        Using conn As New OleDbConnection(wpm_SQLDBConnString)
            conn.Open()
            Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                Try
                    wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                    wpm_AddParameterValue("@CategoryID", PartCategoryID, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@PageID", LocationID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@Title", Title, cmd)
                    wpm_AddParameterStringValue("@Description", Description, cmd)
                    wpm_AddParameterStringValue("@URL", URL, cmd)
                    wpm_AddParameterValue("@DateAdd", Now(), SqlDbType.DateTime, cmd)
                    wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                    wpm_AddParameterValue("@Views", View, SqlDbType.Bit, cmd)
                    wpm_AddParameterStringValue("@UserName", String.Empty, cmd)
                    wpm_AddParameterValue("@UserID", UserID, SqlDbType.Int, cmd)
                    wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                    wpm_AddParameterValue("@SiteCategoryGroupID ", SiteCategoryGroupID, SqlDbType.Int, cmd)
                    iRowsAffected = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    ApplicationLogging.SQLInsertError(ex.Message, "Part.InsertPartDB-{" & cmd.CommandText & "}")
                End Try
            End Using
        End Using
    End Sub




End Class