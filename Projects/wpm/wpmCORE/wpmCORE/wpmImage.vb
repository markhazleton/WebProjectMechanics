Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Web

Public Class wpmImage
    Implements IEquatable(Of wpmImage)
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
            _imageName = wpmUTIL.FixInvalidCharacters(Value)
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
    Private _ImageURL As String
    Public Property ImageURL() As String
        Get
            Return _ImageURL
        End Get
        Set(ByVal value As String)
            _ImageURL = value
        End Set
    End Property
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
                _ImageID = wpmUTIL.GetDBString(myrow("ImageID"))
                _imageName = wpmUTIL.GetDBString(myrow("ImageName"))
                _imageFileName = wpmUTIL.GetDBString(myrow("ImageFileName"))
                _imageDescription = wpmUTIL.GetDBString(myrow("ImageDescription"))
                _imageComment = wpmUTIL.GetDBString(myrow("ImageComment"))
                _imageDate = wpmUTIL.GetDBDate(myrow("ImageDate"))
                _active = wpmUTIL.GetDBBoolean(myrow("Active"))
                _modifiedDT = wpmUTIL.GetDBDate(myrow("ModifiedDT"))
                _contactID = wpmUTIL.GetDBString(myrow("ContactID"))
                _companyID = wpmUTIL.GetDBString(myrow("CompanyID"))
                _title = wpmUTIL.GetDBString(myrow("Title"))
                _medium = wpmUTIL.GetDBString(myrow("Medium"))
                _size = wpmUTIL.GetDBString(myrow("Size"))
                _price = wpmUTIL.GetDBString(myrow("Price"))
                _color = wpmUTIL.GetDBString(myrow("Color"))
                _subject = wpmUTIL.GetDBString(myrow("Subject"))
                _sold = wpmUTIL.GetDBBoolean(myrow("Sold"))
            Next
        Else
            PopulateDefault()
        End If
        mydt.Clear()
        mydt = Nothing
    End Sub
    Public Function createImage() As String
        Dim mySQL As String = "insert into [Image] ([ImageName],[ImageFileName],[CompanyID],[Active]) values(""" & Me.ImageName & """,""" & Me.ImageFileName & """," & Me.CompanyID & ",TRUE);"
        Try
            wpmDB.RunInsertSQL(mySQL, "CreateImage-" & Me.ImageFileName)
        Catch ex As Exception
            wpmLog.SQLAudit(mySQL, ex.ToString)
        End Try
        Return ""
    End Function
    Public Function updateImage() As Boolean
        Dim result As Boolean = False
        If Me.ImageID = "" Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            Dim connection As New OleDbConnection(wpmConfig.ConnStr)

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
                wpmLog.SQLAudit("wpmImage.updateImage", ex.ToString)
            Finally
                connection.Close()
                connection = Nothing
            End Try
            Return result
        End If
    End Function
    Public Function updateImagePath() As Boolean
        Dim result As Boolean = False
        If Me.ImageID = "" Then
            Throw New Exception("Record Does not exist in Image table")
        Else
            Dim connection As New OleDbConnection(wpmConfig.ConnStr)
            Try
                connection.Open()
                Dim command As New OleDbCommand("UPDATE [image] set " & _
                            "[image].[ImageFileName]=@ImageFileName " & _
                            "WHERE [image].[ImageID]=@ImageID ", connection)
                command.Parameters.AddWithValue("@title", Me.ImageFileName)
                command.Parameters.AddWithValue("@ImageID", Me.ImageID)
                If command.ExecuteNonQuery() > 0 Then
                    result = True
                End If
            Catch ex As Exception
                wpmLog.SQLAudit("wpmImage.updateImagePath", ex.ToString)
            Finally
                connection.Close()
                connection = Nothing
            End Try
            Return result
        End If
    End Function

    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim connection As New OleDbConnection(wpmConfig.ConnStr)
        Try
            Dim command As New OleDbCommand("delete from pageimage where imageid=" & ReqImageID)
            If command.ExecuteNonQuery() > 0 Then
                result = True
            End If
        Catch ex As Exception
            wpmLog.SQLAudit("Error on wpmImage.DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            Dim command As New OleDbCommand("delete from image where imageid=" & ReqImageID)
            If command.ExecuteNonQuery() > 0 Then
                result = True
            End If
        Catch ex As Exception
            wpmLog.SQLAudit("Error on wpmImage.DeleteImage-image", ex.ToString)
        End Try
        connection.Close()
        connection = Nothing

        Return result
    End Function

    Public Function Equals1(ByVal other As wpmImage) As Boolean Implements System.IEquatable(Of wpmImage).Equals
        Return Me.ImageID.Equals(other.ImageID)
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

Public Class wpmImagePresenter
    Dim yourUI As IImageRow
    Dim myimage As wpmImage
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
        myimage = New wpmImage(ReqImageID)
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
        myimage = New wpmImage(ReqImageID)
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
    Public Function GetThubmailURL(ByVal ImageWidth As Integer, ByVal SiteGallery As String) As String
        Return "/wpm/catalog/ImageResize.aspx?w=" & ImageWidth & "&img=" & Replace(Replace(SiteGallery & myimage.ImageFileName, "\", "/"), "//", "/")
    End Function
End Class

<DataObject(True)> _
Public Class wpmImageAdapter
    Private Property _data() As wpmImageList
        Get
            If HttpContext.Current.Session("_wpmImageList") Is Nothing Then
                Return New wpmImageList()
            Else
                Return DirectCast(HttpContext.Current.Session("_wpmImageList"), wpmImageList)
            End If
        End Get
        Set(ByVal value As wpmImageList)
            HttpContext.Current.Session("_wpmImageList") = value
        End Set
    End Property

    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
 Public Function GetAll() As wpmImageList
        Return _data
    End Function

    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Sub DeleteImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Remove(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Sub UpdateImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Remove(Image)
        ImageList.Add(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Sub InsertImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Add(Image)
        _data = ImageList
    End Sub
End Class