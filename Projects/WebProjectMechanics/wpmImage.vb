Imports System.Data.OleDb

Public Class wpmImage
    Implements IEquatable(Of wpmImage)
    Public Property ImageID() As String
    Private _imageName As String
    Public Property ImageName() As String
        Get
            Return _imageName
        End Get
        Set(ByVal Value As String)
            _imageName = wpmUtil.FixInvalidCharacters(Value)
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
    Public Property Title() As String
    Public Property Medium() As String
    Public Property Size() As String
    Public Property Price() As String
    Public Property Color() As String
    Public Property Subject() As String
    Public Property Sold() As Boolean
    Public Property ImageURL() As String
    Private Sub PopulateDefault()
        ModifiedDT = System.DateTime.Now()
        ImageDate = System.DateTime.Now()
    End Sub
    Public Sub New()
        PopulateDefault()
    End Sub
    Public Sub New(ByVal myImageID As String)
        Dim mydt As DataTable = wpmDataCon.GetImageByID(myImageID)
        If mydt.Rows.Count = 1 Then
            For Each myrow As DataRow In mydt.Rows
                ImageID = wpmUtil.GetDBString(myrow("ImageID"))
                _imageName = wpmUtil.GetDBString(myrow("ImageName"))
                ImageFileName = wpmUtil.GetDBString(myrow("ImageFileName"))
                ImageDescription = wpmUtil.GetDBString(myrow("ImageDescription"))
                ImageComment = wpmUtil.GetDBString(myrow("ImageComment"))
                ImageDate = wpmUtil.GetDBDate(myrow("ImageDate"))
                Active = wpmUtil.GetDBBoolean(myrow("Active"))
                ModifiedDT = wpmUtil.GetDBDate(myrow("ModifiedDT"))
                ContactID = wpmUtil.GetDBString(myrow("ContactID"))
                CompanyID = wpmUtil.GetDBString(myrow("CompanyID"))
                Title = wpmUtil.GetDBString(myrow("Title"))
                Medium = wpmUtil.GetDBString(myrow("Medium"))
                Size = wpmUtil.GetDBString(myrow("Size"))
                Price = wpmUtil.GetDBString(myrow("Price"))
                Color = wpmUtil.GetDBString(myrow("Color"))
                Subject = wpmUtil.GetDBString(myrow("Subject"))
                Sold = wpmUtil.GetDBBoolean(myrow("Sold"))
            Next
        Else
            PopulateDefault()
        End If
        mydt.Clear()
        mydt = Nothing
    End Sub
    Public Function createImage() As String
        Dim mySQL As String = String.Format("insert into [Image] ([ImageName],[ImageFileName],[CompanyID],[Active]) values(""{0}"",""{1}"",{2},TRUE);", ImageName, ImageFileName, CompanyID)
        Try
            wpmDB.RunInsertSQL(mySQL, "CreateImage-" & ImageFileName)
        Catch ex As Exception
            wpmLogging.SQLAudit(mySQL, ex.ToString)
        End Try
        Return ""
    End Function
    Public Function updateImage() As Boolean
        Dim result As Boolean = False
        If ImageID = "" Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            If ImageDescription = String.Empty Then
                ImageDescription = "(missing description)"
            End If
            Using connection As New OleDbConnection(wpmApp.ConnStr)
                Try
                    connection.Open()
                    Using command As New OleDbCommand("UPDATE [image] set " & _
                                "[image].[title]=@title, " & _
                                "[image].[ImageName]=@ImageName, " & _
                                "[image].[ImageDescription]=@ImageDescription, " & _
                                "[image].[ImageComment]=@ImageComment, " & _
                                "[image].[Size]=@Size, " & _
                                "[image].[Price]=@Price, " & _
                                "[image].[Color]=@Color, " & _
                                "[image].[Subject]=@Subject, " & _
                                "[image].[Medium]=@Medium, " & _
                                "[image].[Active]=@Active, " & _
                                "[image].[Sold]=@Sold, " & _
                                "[image].[ContactID]=@ContactID " & _
                                "WHERE [image].[ImageID]=@ImageID ", connection)
                        command.Parameters.AddWithValue("@title", Title)
                        command.Parameters.AddWithValue("@ImageName", ImageName)
                        command.Parameters.AddWithValue("@ImageDescription", ImageDescription)
                        command.Parameters.AddWithValue("@ImageComment", ImageComment)
                        command.Parameters.AddWithValue("@Size", Size)
                        command.Parameters.AddWithValue("@Price", Price)
                        command.Parameters.AddWithValue("@Color", Color)
                        command.Parameters.AddWithValue("@Subject", Subject)
                        command.Parameters.AddWithValue("@Medium", Medium)
                        command.Parameters.AddWithValue("@Active", Active)
                        command.Parameters.AddWithValue("@sold", Sold)
                        command.Parameters.AddWithValue("@ContactID", ContactID)
                        command.Parameters.AddWithValue("@ImageID", ImageID)
                        If command.ExecuteNonQuery() > 0 Then
                            result = True
                        End If
                    End Using
                Catch ex As Exception
                    wpmLogging.SQLAudit("wpmImage.updateImage", ex.ToString)
                End Try
            End Using
            Return result
        End If
    End Function
    Public Function updateImagePath() As Boolean
        Dim result As Boolean = False
        If ImageID = "" Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            Using connection As New OleDbConnection(wpmApp.ConnStr)
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
                    wpmLogging.SQLAudit("wpmImage.updateImagePath", ex.ToString)
                End Try
            End Using
            Return result
        End If
    End Function

    Public Shared Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Using connection As New OleDbConnection(wpmApp.ConnStr)
            Try
                Using command As New OleDbCommand("delete from pageimage where imageid=" & ReqImageID, connection)
                    If command.ExecuteNonQuery() > 0 Then
                        result = True
                    End If
                End Using
            Catch ex As Exception
                wpmLogging.SQLAudit("Error on wpmImage.DeleteImage-pageimage", ex.ToString)
            End Try
            Try
                Using command As New OleDbCommand("delete from image where imageid=" & ReqImageID, connection)
                    If command.ExecuteNonQuery() > 0 Then
                        result = True
                    End If
                End Using
            Catch ex As Exception
                wpmLogging.SQLAudit("Error on wpmImage.DeleteImage-image", ex.ToString)
            End Try
        End Using
        Return result
    End Function

    Public Function Equals1(ByVal other As wpmImage) As Boolean Implements System.IEquatable(Of wpmImage).Equals
        Return ImageID.Equals(other.ImageID)
    End Function
End Class
