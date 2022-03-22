Imports System.Data.OleDb

Public Class LocationImage
    Implements IEquatable(Of LocationImage)
    Public Property ImageID() As String
    Private _imageName As String
    Public Property ImageName() As String
        Get
            Return _imageName
        End Get
        Set(ByVal Value As String)
            _imageName = wpm_FixInvalidCharacters(Value)
        End Set
    End Property
    Public Property ImageFileName() As String
    Public Property ImageThumbFileName() As String
    Public Property ImageDescription() As String
    Public Property ImageComment() As String
    Public Property ImageDate() As Date
    Public Property Active() As Boolean
    Public Property ModifiedDT() As Date
    Public Property VersionNumber() As Integer
    Public Property ContactID() As String
    Public Property CompanyID() As String
    Public Property LocationID() As String
    Public Property ParentLocationID As String
    Public Property Title() As String
    Public Property ImageURL() As String
    Public Property DisplayOrder As Integer

    Private Sub PopulateDefault()
        ModifiedDT = DateTime.Now()
        ImageDate = DateTime.Now()
    End Sub
    Public Sub New()
        PopulateDefault()
    End Sub
    Public Sub New(ByVal myImageID As String)
        Using mydt As DataTable = ApplicationDAL.GetImageByID(myImageID)
            If mydt.Rows.Count = 1 Then
                For Each myrow As DataRow In mydt.Rows
                    ImageID = wpm_GetDBString(myrow("ImageID"))
                    LocationID = String.Format("ART-{0}", wpm_GetDBString(myrow("ImageID")))
                    ParentLocationID = wpm_GetDBString(myrow("PageID"))
                    ImageName = wpm_GetDBString(myrow("ImageName"))
                    ImageFileName = wpm_GetDBString(myrow("ImageFileName"))
                    ImageDescription = wpm_GetDBString(myrow("ImageDescription"))
                    ImageComment = wpm_GetDBString(myrow("ImageComment"))
                    ImageDate = wpm_GetDBDate(myrow("ImageDate"))
                    Active = wpm_GetDBBoolean(myrow("Active"))
                    ModifiedDT = wpm_GetDBDate(myrow("ModifiedDT"))
                    ContactID = wpm_GetDBString(myrow("ContactID"))
                    Title = wpm_GetDBString(myrow("Title"))
                    ImageURL = wpm_FixInvalidCharacters(String.Format("{0}-{1}{2}", myrow.Item("PageName"), myrow.Item("ImageName"), wpm_SiteConfig.DefaultExtension))
                Next
            Else
                PopulateDefault()
            End If
            mydt.Clear()
        End Using
    End Sub
    Public Function createImage() As Integer
        Dim result As Integer = -1

        If ContactID = String.Empty Then
            ContactID = CStr(1)
        End If

        Dim thisConnection As New OleDbConnection(wpm_SQLDBConnString)
        Using nonqueryCommand As OleDbCommand = thisConnection.CreateCommand()
            Try
                thisConnection.Open()
                nonqueryCommand.CommandText = "INSERT INTO [Image] ([CompanyID], [ImageName], [ImageFileName],[Title],[ContactID], [ModifiedDT],[ImageDate]) VALUES (@CompanyID,@ImageName, @ImageFileName, @Title,@ContactID, @ModifiedDT,@ImageDT)"
                nonqueryCommand.Parameters.Add("@CompanyID", OleDbType.Integer, 50)
                nonqueryCommand.Parameters.Add("@ImageName", OleDbType.VarChar, 50)
                nonqueryCommand.Parameters.Add("@ImageFileName", OleDbType.VarChar, 254)
                nonqueryCommand.Parameters.Add("@Title", OleDbType.VarChar, 50)
                nonqueryCommand.Parameters.Add("@ContactID", OleDbType.Integer, 50)
                nonqueryCommand.Parameters.Add("@ModifiedDT", OleDbType.Date, 50)
                nonqueryCommand.Parameters.Add("@ImageDT", OleDbType.Date, 50)
                nonqueryCommand.Parameters("@CompanyID").Value = CompanyID
                nonqueryCommand.Parameters("@ImageName").Value = ImageName
                nonqueryCommand.Parameters("@ImageFileName").Value = ImageFileName
                nonqueryCommand.Parameters("@Title").Value = ImageFileName
                nonqueryCommand.Parameters("@ContactID").Value = wpm_GetDBInteger(ContactID, 0)
                nonqueryCommand.Parameters("@ModifiedDT").Value = ModifiedDT
                nonqueryCommand.Parameters("@ImageDT").Value = ImageDate
                result = nonqueryCommand.ExecuteNonQuery()
            Catch ex As Exception
                result = -1
                ApplicationLogging.SQLInsertError("LocationImage.createImage", ex.ToString)
            Finally
                thisConnection.Close()
            End Try
        End Using
        Return result
    End Function
    Public Function updateImage() As Boolean
        Dim result As Boolean = False
        If ImageID = String.Empty Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            If ImageDescription = String.Empty Then
                ImageDescription = ImageName
            End If
            Using connection As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    connection.Open()
                    Using command As New OleDbCommand("UPDATE [image] set " & _
                                "[image].[title]=@title, " & _
                                "[image].[ImageName]=@ImageName, " & _
                                "[image].[ImageDescription]=@ImageDescription, " & _
                                "[image].[ImageComment]=@ImageComment, " & _
                                "[image].[ContactID]=@ContactID, [image].[ModifiedDT]=now(), [image].[ImageDate]=@ImageDate " & _
                                "WHERE [image].[ImageID]=@ImageID ", connection)
                        command.Parameters.AddWithValue("@title", Title)
                        command.Parameters.AddWithValue("@ImageName", ImageName)
                        command.Parameters.AddWithValue("@ImageDescription", ImageDescription)
                        command.Parameters.AddWithValue("@ImageComment", ImageComment)
                        command.Parameters.AddWithValue("@ContactID", ContactID)
                        wpm_AddParameterValue("@ImageDate", ImageDate, SqlDbType.Date, command)

                        command.Parameters.AddWithValue("@ImageID", ImageID)
                        If command.ExecuteNonQuery() > 0 Then
                            result = True
                        End If
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError("LocationImage.updateImage", ex.ToString)
                End Try
            End Using
            Return result
        End If
    End Function
    Public Function updateImagePath() As Boolean
        Dim result As Boolean = False
        If ImageID = String.Empty Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            Using connection As New OleDbConnection(wpm_SQLDBConnString)
                Try
                    connection.Open()
                    Using command As New OleDbCommand("UPDATE [image] set " & _
                                "[image].[ImageFileName]=@ImageFileName " & _
                                "WHERE [image].[ImageID]=@ImageID ", connection)
                        command.Parameters.AddWithValue("@title", ImageFileName)
                        command.Parameters.AddWithValue("@ImageID", ImageID)
                        If command.ExecuteNonQuery() > 0 Then
                            result = True
                        End If
                    End Using
                Catch ex As Exception
                    ApplicationLogging.SQLUpdateError("LocationImage.updateImagePath", ex.ToString)
                End Try
            End Using
            Return result
        End If
    End Function

    Public Shared Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Using connection As New OleDbConnection(wpm_SQLDBConnString)
            Try
                Using command As New OleDbCommand("delete from pageimage where imageid=" & ReqImageID, connection)
                    If command.ExecuteNonQuery() > 0 Then
                        result = True
                    End If
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError("Error on LocationImage.DeleteImage-pageimage", ex.ToString)
            End Try
            Try
                Using command As New OleDbCommand("delete from image where imageid=" & ReqImageID, connection)
                    If command.ExecuteNonQuery() > 0 Then
                        result = True
                    End If
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLDeleteError("Error on LocationImage.DeleteImage-image", ex.ToString)
            End Try
        End Using
        Return result
    End Function

    Public Function Equals1(ByVal other As LocationImage) As Boolean Implements IEquatable(Of LocationImage).Equals
        Return ImageID.Equals(other.ImageID)
    End Function
End Class
