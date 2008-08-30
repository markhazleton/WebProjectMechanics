Public Class mhImageRow
    Private _ImageID As String
    Public Property ImageID() As String
        Get
            Return _ImageID
        End Get
        Set(ByVal value As String)
            _ImageID = value
        End Set
    End Property
    Private _imageName As String
    Public Property ImageName() As String
        Get
            Return _imageName
        End Get
        Set(ByVal Value As String)
            _imageName = mhUTIL.FixInvalidCharacters(Value)
        End Set
    End Property
    Private _imageFileName As String
    Public Property ImageFileName() As String
        Get
            Return _imageFileName
        End Get
        Set(ByVal Value As String)
            _imageFileName = Value
        End Set
    End Property

    Private _ImageThumbFileName As String
    Public Property ImageThumbFileName() As String
        Get
            Return _ImageThumbFileName
        End Get
        Set(ByVal Value As String)
            _ImageThumbFileName = Value
        End Set
    End Property

    Private _imageDescription As String
    Public Property ImageDescription() As String
        Get
            Return _imageDescription
        End Get
        Set(ByVal Value As String)
            _imageDescription = Value
        End Set
    End Property
    Private _imageComment As String
    Public Property ImageComment() As String
        Get
            Return _imageComment
        End Get
        Set(ByVal Value As String)
            _imageComment = Value
        End Set
    End Property
    Private _imageDate As Date
    Public Property ImageDate() As Date
        Get
            Return _imageDate
        End Get
        Set(ByVal Value As Date)
            _imageDate = Value
        End Set
    End Property
    Private _active As Boolean
    Public Property Active() As Boolean
        Get
            Return _active
        End Get
        Set(ByVal Value As Boolean)
            _active = Value
        End Set
    End Property
    Private _modifiedDT As Date
    Public Property ModifiedDT() As Date
        Get
            Return _modifiedDT
        End Get
        Set(ByVal Value As Date)
            _modifiedDT = Value
        End Set
    End Property
    Private _versionNumber As Integer
    Public Property VersionNumber() As Integer
        Get
            Return _versionNumber
        End Get
        Set(ByVal Value As Integer)
            _versionNumber = Value
        End Set
    End Property
    Private _contactID As String
    Public Property ContactID() As String
        Get
            Return _contactID
        End Get
        Set(ByVal Value As String)
            _contactID = Value
        End Set
    End Property
    Private _companyID As String
    Public Property CompanyID() As String
        Get
            Return _companyID
        End Get
        Set(ByVal Value As String)
            _companyID = Value
        End Set
    End Property
    Private _title As String
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal Value As String)
            _title = Value
        End Set
    End Property
    Private _medium As String
    Public Property Medium() As String
        Get
            Return _medium
        End Get
        Set(ByVal Value As String)
            _medium = Value
        End Set
    End Property
    Private _size As String
    Public Property Size() As String
        Get
            Return _size
        End Get
        Set(ByVal Value As String)
            _size = Value
        End Set
    End Property
    Private _price As String
    Public Property Price() As String
        Get
            Return _price
        End Get
        Set(ByVal Value As String)
            _price = Value
        End Set
    End Property
    Private _color As String
    Public Property Color() As String
        Get
            Return _color
        End Get
        Set(ByVal Value As String)
            _color = Value
        End Set
    End Property
    Private _subject As String
    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal Value As String)
            _subject = Value
        End Set
    End Property
    Private _sold As Boolean
    Public Property Sold() As Boolean
        Get
            Return _sold
        End Get
        Set(ByVal Value As Boolean)
            _sold = Value
        End Set
    End Property

    Private Sub PopulateDefault()
        ModifiedDT = Now()
        ImageDate = Now()
    End Sub
    Public Sub New()
        PopulateDefault()
    End Sub
    Public Sub New(ByVal myImageID As String)
        Dim mydt As DataTable = mhDataCon.GetImageByID(myImageID)
        If mydt.Rows.Count = 1 Then
            For Each myrow As DataRow In mydt.Rows
                _ImageID = mhUTIL.GetDBString(myrow("ImageID"))
                _imageName = mhUTIL.GetDBString(myrow("ImageName"))
                _imageFileName = mhUTIL.GetDBString(myrow("ImageFileName"))
                _imageDescription = mhUTIL.GetDBString(myrow("ImageDescription"))
                _imageComment = mhUTIL.GetDBString(myrow("ImageComment"))
                _imageDate = mhUTIL.GetDBDate(myrow("ImageDate"))
                _active = mhUTIL.GetDBBoolean(myrow("Active"))
                _modifiedDT = mhUTIL.GetDBDate(myrow("ModifiedDT"))
                _contactID = mhUTIL.GetDBString(myrow("ContactID"))
                _companyID = mhUTIL.GetDBString(myrow("CompanyID"))
                _title = mhUTIL.GetDBString(myrow("Title"))
                _medium = mhUTIL.GetDBString(myrow("Medium"))
                _size = mhUTIL.GetDBString(myrow("Size"))
                _price = mhUTIL.GetDBString(myrow("Price"))
                _color = mhUTIL.GetDBString(myrow("Color"))
                _subject = mhUTIL.GetDBString(myrow("Subject"))
                _sold = mhUTIL.GetDBBoolean(myrow("Sold"))
            Next
        Else
            PopulateDefault()
        End If
        mydt.Clear()
        mydt = Nothing
    End Sub
    Public Function createImage() As String
        Try
            'Dim command As New OleDbCommand("INSERT INTO Image " _
            '& "(title,ImageName,ContactID,ImageDescription,ImageComment,ImageFileName,Size,Price,Color,Subject,ImageDate,Medium,Sold,CompanyID) " & _
            '"VALUES (@title,@ImageName,@ContactID,@ImageDescription,@ImageComment,@ImageFileName,@ImageThumbFileName,@Size,@Price,@Color,@Subject,@ImageDate,@Medium,@Sold,@CompanyID)", connection)
            'command.Parameters.AddWithValue("@title", Me.Title)
            'command.Parameters.AddWithValue("@ImageName", Me.ImageName)
            'command.Parameters.AddWithValue("@ContactID", Me.ContactID)
            'command.Parameters.AddWithValue("@ImageDescription", Me.ImageDescription)
            'command.Parameters.AddWithValue("@ImageComment", Me.ImageComment)
            'command.Parameters.AddWithValue("@ImageFileName", Me.ImageFileName)
            'command.Parameters.AddWithValue("@Size", Me.Size)
            'command.Parameters.AddWithValue("@Price", Me.Price)
            'command.Parameters.AddWithValue("@Color", Me.Color)
            'command.Parameters.AddWithValue("@Subject", Me.Subject)
            'command.Parameters.AddWithValue("@ImageDate", Me.ImageDate)
            'command.Parameters.AddWithValue("@Medium", Me.Medium)
            'command.Parameters.AddWithValue("@sold", Me.Sold)
            'command.Parameters.AddWithValue("@CompanyID", Me.CompanyID)
            'Command.ExecuteNonQuery()
            'Command.Parameters.Clear()
            'Command.CommandText = "select @@IDENTITY"
            'newImageID = Command.ExecuteScalar().ToString
            'Me.ImageID = newImageID
            Dim mySQL As String
            mySQL = "insert into [Image] ([ImageName],[ImageFileName],[CompanyID],[Active]) values(""" & Me.ImageName & """,""" & Me.ImageFileName & """," & Me.CompanyID & ",TRUE);"
            mhDB.RunInsertSQL(mySQL, "CreateImage-" & Me.ImageFileName)
        Catch ex As Exception
            mhUTIL.SQLAudit(Command.ToString, ex.ToString)
        End Try
        Return ""
    End Function
    Public Function updateImage() As Boolean
        Dim result As Boolean = False
        If Me.ImageID = "" Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            Dim connection As New OleDbConnection(mhConfig.ConnStr)

            If Me.ImageDescription = String.Empty Then
                Me.ImageDescription = "(missing description)"
            End If

            Try
                connection.Open()
                Dim command As New OleDbCommand("UPDATE [image] set " & _
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
                command.Parameters.AddWithValue("@title", Me.Title)
                command.Parameters.AddWithValue("@ImageName", Me.ImageName)
                command.Parameters.AddWithValue("@ImageDescription", Me.ImageDescription)
                command.Parameters.AddWithValue("@ImageComment", Me.ImageComment)
                command.Parameters.AddWithValue("@Size", Me.Size)
                command.Parameters.AddWithValue("@Price", Me.Price)
                command.Parameters.AddWithValue("@Color", Me.Color)
                command.Parameters.AddWithValue("@Subject", Me.Subject)
                command.Parameters.AddWithValue("@Medium", Me.Medium)
                command.Parameters.AddWithValue("@Active", Me.Active)
                command.Parameters.AddWithValue("@sold", Me.Sold)
                command.Parameters.AddWithValue("@ContactID", Me.ContactID)
                command.Parameters.AddWithValue("@ImageID", Me.ImageID)
                If command.ExecuteNonQuery() > 0 Then
                    result = True
                End If
            Catch ex As Exception
                mhUTIL.SQLAudit("mhImage.updateImage", ex.ToString)
            Finally
                connection.Close()
                connection = Nothing
            End Try
            Return result
        End If
    End Function
    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim connection As New OleDbConnection(mhConfig.ConnStr)
        Try
            Dim command As New OleDbCommand("delete from pageimage where imageid=" & ReqImageID)
            If command.ExecuteNonQuery() > 0 Then
                result = True
            End If
        Catch ex As Exception
            mhUTIL.SQLAudit("Error on mhImage.DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            Dim command As New OleDbCommand("delete from image where imageid=" & ReqImageID)
            If command.ExecuteNonQuery() > 0 Then
                result = True
            End If
        Catch ex As Exception
            mhUTIL.SQLAudit("Error on mhImage.DeleteImage-image", ex.ToString)
        End Try
        connection.Close()
        connection = Nothing

        Return result
    End Function
End Class

Public Interface IImageRow
    Property ImageID() As String
    Property ImageName() As String
    Property ImageFileName() As String
    Property ImageThumbFileName() As String
    Property ImageDescription() As String
    Property ImageComment() As String
    Property ImageDate() As Date
    Property Active() As Boolean
    Property ModifiedDate() As Date
    Property VersionNumber() As Integer
    Property ContactID() As String
    Property CompanyID() As String
    Property Title() As String
    Property Medium() As String
    Property Size() As String
    Property Price() As String
    Property Color() As String
    Property Subject() As String
    Property Sold() As Boolean
End Interface

Public Class mhImagePresenter
    Dim yourUI As IImageRow
    Dim myimage As mhImageRow
    Public Status As String

    Public Sub New(ByVal MyUI As IImageRow)
        yourUI = MyUI
        Status = "New Image"
    End Sub

    Public Sub updateMyUi(ByRef ReqImageID As String)
        GetImageValues(ReqImageID)
    End Sub

    Public Sub DeleteImage(ByVal ReqPageID As String, ByVal ReqImageID As String)
        If myimage.DeleteImage(ReqImageID) Then
            Status = "Image Deleted"
        Else
            Status = "Error on Image Delete"
        End If
    End Sub
    Public Sub SetImageValues(ByRef ReqImageID As String)
        myimage = New mhImageRow(ReqImageID)
        yourUI.ImageID = myimage.ImageID
        yourUI.ImageName = myimage.ImageName
        yourUI.ImageFileName = myimage.ImageFileName
        yourUI.ImageThumbFileName = myimage.ImageThumbFileName
        yourUI.ImageDescription = myimage.ImageDescription
        yourUI.ImageComment = myimage.ImageComment
        yourUI.ImageDate = myimage.ImageDate
        yourUI.Active = myimage.Active
        yourUI.ModifiedDate = myimage.ModifiedDT
        yourUI.VersionNumber = myimage.VersionNumber
        yourUI.ContactID = myimage.ContactID
        yourUI.CompanyID = myimage.CompanyID
        yourUI.Title = myimage.Title
        yourUI.Medium = myimage.Medium
        yourUI.Size = myimage.Size
        yourUI.Price = myimage.Price
        yourUI.Color = myimage.Color
        yourUI.Subject = myimage.Subject
        yourUI.Sold = myimage.Sold
    End Sub
    Private Sub GetImageValues(ByRef ReqImageID As String)
        myimage = New mhImageRow(ReqImageID)
        myimage.ImageID = yourUI.ImageID
        myimage.ImageName = yourUI.ImageName
        myimage.ImageFileName = yourUI.ImageFileName
        myimage.ImageDescription = yourUI.ImageDescription
        myimage.ImageComment = yourUI.ImageComment
        myimage.ImageDate = yourUI.ImageDate
        myimage.Active = yourUI.Active
        myimage.ModifiedDT = yourUI.ModifiedDate
        myimage.VersionNumber = yourUI.VersionNumber
        myimage.ContactID = yourUI.ContactID
        myimage.CompanyID = yourUI.CompanyID
        myimage.Title = yourUI.Title
        myimage.Medium = yourUI.Medium
        myimage.Size = yourUI.Size
        myimage.Price = yourUI.Price
        myimage.Color = yourUI.Color
        myimage.Subject = yourUI.Subject
        myimage.Sold = yourUI.Sold
        If myimage.updateImage() Then
            Status = "Image has been updated"
        Else
            Status = "Error on Image Update"
        End If
    End Sub
End Class

Public Class mhImageRows
    Inherits List(Of mhImageRow)

    Public Sub New(ByVal CompanyID As String)
        Dim mydt As DataTable = mhDataCon.GetImageList(CompanyID)
        For Each myrow As DataRow In mydt.Rows
            Dim myImage As New mhImageRow()
            myImage.ImageID = mhUTIL.GetDBString(myrow.Item("ImageID"))
            myImage.CompanyID = mhUTIL.GetDBString(myrow.Item("CompanyID"))
            myImage.VersionNumber = mhUTIL.GetDBInteger(myrow.Item("VersionNo"))
            myImage.Title = mhUTIL.GetDBString(myrow.Item("Title"))
            myImage.ImageName = mhUTIL.GetDBString(myrow.Item("ImageName"))
            myImage.ContactID = mhUTIL.GetDBString(myrow.Item("ContactID"))
            myImage.ImageDescription = mhUTIL.GetDBString(myrow.Item("ImageDescription"))
            myImage.ImageComment = mhUTIL.GetDBString(myrow.Item("ImageComment"))
            myImage.ImageFileName = mhUTIL.GetDBString(myrow.Item("ImageFileName"))
            myImage.ImageThumbFileName = mhUTIL.GetDBString(myrow.Item("ImageFileName"))
            myImage.Size = mhUTIL.GetDBString(myrow.Item("Size"))
            myImage.Price = mhUTIL.GetDBString(myrow.Item("Price"))
            myImage.Color = mhUTIL.GetDBString(myrow.Item("Color"))
            myImage.Subject = mhUTIL.GetDBString(myrow.Item("Subject"))
            myImage.ImageDate = mhUTIL.GetDBDate(myrow.Item("ImageDate"))
            myImage.Medium = mhUTIL.GetDBString(myrow.Item("Medium"))
            myImage.Sold = mhUTIL.GetDBBoolean(myrow.Item("Sold"))
            Me.Add(myImage)
        Next
    End Sub
    Public Sub New(ByVal CompanyID As String, ByVal AssignedPageImage As Boolean)
        Dim mydt As New DataTable
        If (AssignedPageImage) Then
            mydt = mhDataCon.GetUnlinkedImageList(CompanyID)
        Else
            mydt = mhDataCon.GetImageList(CompanyID)
        End If

        For Each myrow As DataRow In mydt.Rows
            Dim myImage As New mhImageRow()
            myImage.Title = mhUTIL.GetDBString(myrow.Item("Title"))
            myImage.ImageName = mhUTIL.GetDBString(myrow.Item("ImageName"))
            myImage.ContactID = mhUTIL.GetDBString(myrow.Item("ContactID"))
            myImage.ImageDescription = mhUTIL.GetDBString(myrow.Item("ImageDescription"))
            myImage.ImageComment = mhUTIL.GetDBString(myrow.Item("ImageComment"))
            myImage.ImageFileName = mhUTIL.GetDBString(myrow.Item("ImageFileName"))
            myImage.Size = mhUTIL.GetDBString(myrow.Item("Size"))
            myImage.Price = mhUTIL.GetDBString(myrow.Item("Price"))
            myImage.Color = mhUTIL.GetDBString(myrow.Item("Color"))
            myImage.Subject = mhUTIL.GetDBString(myrow.Item("Subject"))
            myImage.ImageDate = mhUTIL.GetDBDate(myrow.Item("ImageDate"))
            myImage.Medium = mhUTIL.GetDBString(myrow.Item("Medium"))
            myImage.Sold = mhUTIL.GetDBBoolean(myrow.Item("Sold"))
            Me.Add(myImage)
        Next
    End Sub



End Class
