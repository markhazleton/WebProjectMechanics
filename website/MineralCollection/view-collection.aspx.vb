
Imports WebProjectMechanics
Imports wpmMineralCollection

Partial Class MineralCollection_view_collection
    Inherits ApplicationPage
    Public isAdmin As Boolean = False

    Public Property myListView As MineralCollectionListView
        Get
            Try
                If CType(Session("wpm_MineralListView"), MineralCollectionListView) Is Nothing Then
                    Session("wpm_MineralListView") = New MineralCollectionListView
                End If
                Return CType(Session("wpm_MineralListView"), MineralCollectionListView)
            Catch ex As Exception
                Session("wpm_MineralListView") = New MineralCollectionListView
                Return CType(Session("wpm_MineralListView"), MineralCollectionListView)
            End Try
        End Get
        Set(value As MineralCollectionListView)
            Session("wpm_MineralListView") = value
        End Set
    End Property

    Protected Sub Page_Init1(sender As Object, e As EventArgs) Handles Me.Init
        If Not wpm_IsUser Then
            Response.Redirect("/")
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        isAdmin = wpm_IsAdmin
        SetPageName("View Collection")

        myListView.ResetCriteria()
        myListView.FeaturedOnly = True
        myListView.SoldOnly = False
        GetRows(myListView)


        If wpm_IsAdmin Or wpm_IsEditor Then
            If Not IsPostBack AndAlso Request.QueryString.Count = 0 Then
            End If
        Else
        End If
        If Not IsPostBack Then
        End If
        wpm_ListPageURL = HttpContext.Current.Request.RawUrl
    End Sub
    Public Property CurrentIndex As Integer
        Get
            Return CInt(Session("curIndex"))
        End Get
        Set(value As Integer)
            Session("curIndex") = value
        End Set
    End Property
    Public Property MaxIndex As Integer
        Get
            Return CInt(Session("MaxIndex"))
        End Get
        Set(value As Integer)
            Session("MaxIndex") = value
        End Set
    End Property
    Private Sub GetRows(ByVal reqListView As MineralCollectionListView)
        Dim myResults As New List(Of Object)
        myResults.AddRange(reqListView.GetList())
        If myResults.Count = 1 Then
            ' Single Specimen Display
        Else
            Try
                rptItem.Visible = True
                rptItem.DataSource = myResults
                rptItem.DataBind()
            Catch ex As Exception
                ApplicationLogging.ErrorLog("CollectionItemView.GetRows - multiple or no result found", ex.ToString)
            End Try
        End If
    End Sub

    Public Function GetImageURL(ByVal FileName As String) As String
        Return String.Format("/sites/nrc/images/{0}", FileName)
    End Function
    Public Function GetThumbnailURL(ByVal FileName As String) As String
        If String.IsNullOrEmpty(FileName) Then
            Return "/admin/images/spacer.gif"
        Else
            Return String.Format("/sites/nrc/Thumbnails/{0}", FileName.ToLower().Replace(".jpg", ".png"))
        End If
    End Function

    Public Function GetThumnailClassByOrder(ByVal iOrder As Integer) As String
        If iOrder = 0 Then
            Return " selected "
        Else
            Return " "
        End If
    End Function
    Public Function GetCaroselImageClassByOrder(ByVal iOrder As Integer) As String
        If iOrder = 0 Then
            Return " active item "
        Else
            Return " item "
        End If
    End Function

    Public Function GetImageList(ByRef myImageList As List(Of CollectionItemImage)) As List(Of CollectionItemImage)
        Return (From i In myImageList Order By i.DisplayOrder Select i).ToList
    End Function

End Class
