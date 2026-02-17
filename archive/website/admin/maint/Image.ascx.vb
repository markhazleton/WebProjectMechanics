Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data

Public Class admin_maint_Image
    Inherits ApplicationUserControl
    Private Property reqImageID As String
    Public Property reqImage As LocationImage

    ' Image
    Public Const STR_ImageID As String = "ImageID"
    Public Const STR_SELECTImageList As String = "SELECT [ImageID], [ImageName], [ImageFileName], [ImageThumbFileName], [ImageDescription], [ImageComment], [ImageDate], [Active], [ModifiedDT], [VersionNo], [ContactID], [CompanyID], [title], [medium], [size], [price], [color], [subject], [sold] FROM [Image]"
    Public Const STR_SELECTImageByImageID As String = ""
    Public Const STR_UPDATE_Image As String = "UPDATE [Image] SET [ImageName] = @ImageName, [ImageFileName] = @ImageFileName, [ImageThumbFileName] = @ImageThumbFileName, [ImageDescription] = @ImageDescription, [ImageComment] = @ImageComment, [ImageDate] = @ImageDate, [Active] = @Active, [ModifiedDT] = @ModifiedDT, [VersionNo] = @VersionNo, [ContactID] = @ContactID, [CompanyID] = @CompanyID, [title] = @title WHERE [ImageID] = @ImageID;"
    Public Const STR_INSERT_Image As String = "INSERT INTO [Image] ([ImageName], [ImageFileName], [ImageThumbFileName], [ImageDescription], [ImageComment], [ImageDate], [Active], [ModifiedDT], [VersionNo], [ContactID], [CompanyID], [title]) VALUES (@ImageName, @ImageFileName, @ImageThumbFileName, @ImageDescription, @ImageComment, @ImageDate, @Active, @ModifiedDT, @VersionNo, @ContactID, @CompanyID, @title)"
    Public Const STR_DELETE_Image As String = "DELETE FROM [Image] WHERE [ImageID] = @ImageID"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqImageID = GetProperty(STR_ImageID, Session(STR_ImageID))

        Session(STR_ImageID) = String.Empty

        If Not IsPostBack Then
            If reqImageID <> String.Empty Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
                GetImageEdit(masterPage.myCompany)
            ElseIf reqImageID = "new" Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Site Images"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "ImageDescription"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Comment", .Value = "ImageComment"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Active", .Value = "Active"})
                myListHeader.DetailKeyName = "ImageID"
                myListHeader.DetailFieldName = "ImageName"
                myListHeader.DetailPath = "/admin/maint/default.aspx?type=Image&ImageID={0}"
                Dim myList As New List(Of Object)
                myList.AddRange(masterPage.myCompany.SiteImageList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        reqImage = masterPage.myCompany.SiteImageList.FindImageByImageID(reqImageID)
        With reqImage
            .ImageName = ImageNameTextBox.Text
            .ImageComment = ImageCommentTextBox.Text
            .ImageDescription = ImageDescriptionTextBox.Text
            .ImageDate = wpm_GetDBDate(ImageDateTextBox.Text)
            .ModifiedDT = Now()
            .VersionNumber = VersionNoTextBox.Text
            .ContactID = ContactIDTextBox.Text
            .Title = titleTextBox.Text
            .Active = activeCheckBox.Checked
        End With
        reqImage.updateImage()
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        OnUpdated(Me)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        OnUpdated(Me)
    End Sub

    Private Sub GetImageEdit(ByRef myCompany As ActiveCompany)
        If IsNumeric(reqImageID) AndAlso CInt(reqImageID) > 0 Then
            reqImage = myCompany.SiteImageList.FindImageByImageID(reqImageID)
            With reqImage
                ImageIDLiteral.Text = .ImageID
                ImageNameTextBox.Text = .ImageName
                ImageCommentTextBox.Text = .ImageComment
                ImageDescriptionTextBox.Text = .ImageDescription
                imgThumb.ImageUrl = String.Format("/runtime/catalog/FindImage.ashx?h=150&img={1}{0}", .ImageFileName, wpm_SiteGallery)
                ImageDateTextBox.Text = .ImageDate
                ModifiedDTTextBox.Text = .ModifiedDT
                VersionNoTextBox.Text = .VersionNumber
                ContactIDTextBox.Text = .ContactID
                titleTextBox.Text = .Title
                activeCheckBox.Checked = .Active
            End With
        End If
    End Sub

End Class


'Dim cmdString As String = "UPDATE [Image] SET [ImageName] = @ImageName, [ImageFileName] = @ImageFileName, [ImageThumbFileName] = @ImageThumbFileName, [ImageDescription] = @ImageDescription, [ImageComment] = @ImageComment, [ImageDate] = @ImageDate, [Active] = @Active, [ModifiedDT] = @ModifiedDT, [VersionNo] = @VersionNo, [ContactID] = @ContactID, [CompanyID] = @CompanyID, [title] = @title, [medium] = @medium, [size] = @size, [price] = @price, [color] = @color, [subject] = @subject, [sold] = @sold WHERE [ImageID] = @ImageID;"

'cmdString = "UPDATE [Image] SET [ImageName] = @ImageName, [ImageDescription] = @ImageDescription, [ImageComment] = @ImageComment WHERE [ImageID] = @ImageID;"

'Dim iRowsAffected As Integer = 0
'Using conn As New OleDbConnection(wpm_SQLDBConnString)
'    Try
'        conn.Open()
'        Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = cmdString}
'            wpm_AddParameterValue("@ImageName", ImageNameTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@ImageFileName", ImageFileNameTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@ImageThumbFileName", ImageThumbFileNameTextBox.Text, SqlDbType.Text, cmd)
'            wpm_AddParameterValue("@ImageDescription", ImageDescriptionTextBox.Text, SqlDbType.Text, cmd)
'            wpm_AddParameterValue("@ImageComment", ImageCommentTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@ImageDate", ImageDateTextBox.Text, SqlDbType.Date, cmd)
'            'wpm_AddParameterValue("@Active", activeCheckBox.Checked, SqlDbType.Bit, cmd)
'            'wpm_AddParameterValue("@ModifiedDT", Now, SqlDbType.Date, cmd)
'            'wpm_AddParameterValue("@VersionNo", VersionNoTextBox.Text, SqlDbType.Int, cmd)
'            'wpm_AddParameterValue("@ContactID", ContactIDTextBox.Text, SqlDbType.Int, cmd)
'            'wpm_AddParameterValue("@CompanyID", wpm_DomainConfig.CompanyID , SqlDbType.Int, cmd)
'            'wpm_AddParameterValue("@title", titleTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@medium", mediumTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@size", sizeTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@price", priceTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@color", colorTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@subject", subjectTextBox.Text, SqlDbType.Text, cmd)
'            'wpm_AddParameterValue("@sold", soldCheckBox.Checked, SqlDbType.Bit, cmd)
'            wpm_AddParameterValue("@ImageID", reqImageID, SqlDbType.Int, cmd)
'            iRowsAffected = cmd.ExecuteNonQuery()
'        End Using
'    Catch ex As Exception
'        ApplicationLogging.SQLUpdateError(STR_UPDATE_PageAlias, "PageAlias.acsx - cmd_Update_Click")
'    End Try
'End Using
