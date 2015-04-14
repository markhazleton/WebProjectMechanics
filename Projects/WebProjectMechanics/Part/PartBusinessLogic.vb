Imports System.Text
Imports System.Data.OleDb

Public Class PartBusinessLogic
    Private myPart As Part = New Part
    Private ReadOnly _thePartID As String
    Public Sub New(ByRef ThePart As Part)
        myPart = ThePart
        _thePartID = ThePart.PartID
    End Sub
    Public Sub New(ByRef ThePart As IPartEdit)
        CopyPartEdit(ThePart)
        _thePartID = ThePart.PartID
    End Sub

    Sub New(ByVal thePartID As String)
        myPart = New Part()
        _thePartID = thePartID
        myPart = New ActiveCompany().PartList.FindPart(thePartID)
    End Sub
    Public Sub New()
        _thePartID = String.Empty
        myPart = New Part
    End Sub

    Public Function IsValid(ByRef ErrorMessage As String) As Boolean
        Dim bReturn As Boolean = True
        ' Validate Part
        If _thePartID = String.Empty Then
            bReturn = False
            ErrorMessage = "Part ID Is Missing"
        End If
        ErrorMessage = String.Empty
        Return bReturn
    End Function

    Public Function SetPartI(ByRef returnPart As IPart) As IPart
        Return CopyPart(myPart, returnPart)
    End Function

    Private Sub CopyPartEdit(ByRef fromPart As IPartEdit)
        myPart.Title = fromPart.Title
        myPart.URL = fromPart.URL
        myPart.Description = fromPart.Description
        myPart.PartCategoryID = fromPart.PartCategoryID
        myPart.PartTypeCD = fromPart.PartTypeCD
        myPart.PartCategoryTitle = fromPart.LinkCategoryTitle
        myPart.PartID = fromPart.PartID
        myPart.SiteCategoryTypeID = fromPart.SiteCategoryTypeID
        myPart.SiteCategoryGroupID = fromPart.SiteCategoryGroupID
        myPart.LocationID = fromPart.LocationID
        myPart.View = fromPart.View
        myPart.ModifiedDT = Now()
        myPart.PartSortOrder = fromPart.PartSortOrder
        myPart.UserID = fromPart.UserID
        myPart.AmazonIndex = fromPart.AmazonIndex
        myPart.PartSource = fromPart.PartSource
        myPart.CompanyID = fromPart.CompanyID
    End Sub
    Private Shared Function CopyPart(ByRef fromPart As Part, ByRef toPart As IPart) As IPart
        toPart.Title = fromPart.Title
        toPart.URL = fromPart.URL
        toPart.Description = fromPart.Description
        toPart.PartCategoryID = fromPart.PartCategoryID
        toPart.PartTypeCD = fromPart.PartTypeCD
        toPart.LinkCategoryTitle = fromPart.PartCategoryTitle
        toPart.PartID = fromPart.PartID
        toPart.SiteCategoryTypeID = fromPart.SiteCategoryTypeID
        toPart.SiteCategoryGroupID = fromPart.SiteCategoryGroupID
        toPart.LocationID = fromPart.LocationID
        toPart.View = fromPart.View
        toPart.ModifiedDT = Now()
        toPart.PartSortOrder = fromPart.PartSortOrder
        toPart.UserID = fromPart.UserID
        toPart.AmazonIndex = fromPart.AmazonIndex
        toPart.PartSource = fromPart.PartSource
        toPart.CompanyID = fromPart.CompanyID
        Return toPart
    End Function

    Public Function UpdatePart() As Boolean
        Dim sqlUpdate As New StringBuilder
        Dim iRowsAffected As Integer

        Dim sSQL As String = String.Empty
        If myPart.PartSource = "Link" Then
            sSQL = "UPDATE LINK SET [URL] = @URL, [LinkTypeCD]=@LinkTypeCD, [CategoryID]=@CategoryID, [PageID]=@PageID, [Title]= @Title, [Description]=@Description, [DateAdd]=now(), [Ranks]= @Ranks, [UserName]= @UserName, [UserID]=@UserID, [ASIN]=@ASIN  WHERE [CompanyID] = @CompanyID and [ID]=@PartID"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                        wpm_AddParameterStringValue("@URL", myPart.URL, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", myPart.PartTypeCD, cmd)
                        wpm_AddParameterStringValue("@CategoryID", myPart.PartCategoryID, cmd)
                        wpm_AddParameterStringValue("@PageID", myPart.LocationID, cmd)
                        wpm_AddParameterStringValue("@Title", myPart.Title, cmd)
                        wpm_AddParameterStringValue("@Description", myPart.Description, cmd)
                        wpm_AddParameterValue("@Ranks", myPart.PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@UserName", myPart.UserID, cmd)
                        wpm_AddParameterStringValue("@UserID", myPart.UserID, cmd)
                        wpm_AddParameterStringValue("@ASIN", myPart.AmazonIndex, cmd)
                        wpm_AddParameterStringValue("@CompanyID", myPart.CompanyID, cmd)
                        wpm_AddParameterStringValue("@LinkID", myPart.PartID, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLSelectError(sqlUpdate.ToString(), String.Format("Error on UtilityDB.RunUpdateSQL - {0} ({1})", "PartBuisnessLogic.UpdatePart", ex.Message))
                End Try
            End Using
        ElseIf myPart.PartSource = "SiteLink" Then
            sSQL = "UPDATE SiteLink SET [URL] = @URL, [LinkTypeCD]=@LinkTypeCD, [CategoryID]=@CategoryID, [Title]= @Title, [Description]=@Description, [DateAdd]=now(), [Ranks]= @Ranks, [UserName]= @UserName, [UserID]=@UserID, [ASIN]=@ASIN  WHERE [ID]=@PartID"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    conn.Open()
                    Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                        wpm_AddParameterStringValue("@URL", myPart.URL, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", myPart.PartTypeCD, cmd)
                        wpm_AddParameterStringValue("@CategoryID", myPart.PartCategoryID, cmd)
                        wpm_AddParameterStringValue("@Title", myPart.Title, cmd)
                        wpm_AddParameterStringValue("@Description", myPart.Description, cmd)
                        wpm_AddParameterValue("@Ranks", myPart.PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@UserName", myPart.UserID, cmd)
                        wpm_AddParameterStringValue("@UserID", myPart.UserID, cmd)
                        wpm_AddParameterStringValue("@ASIN", myPart.AmazonIndex, cmd)
                        wpm_AddParameterStringValue("@LinkID", myPart.PartID, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLSelectError(sqlUpdate.ToString(), String.Format("Error on UtilityDB.RunUpdateSQL - {0} ({1})", "PartBuisnessLogic.UpdatePart", ex.Message))
                End Try
            End Using
        End If

        If iRowsAffected = 1 Then
            Return True
        Else
            Return False
        End If
    End Function


End Class
