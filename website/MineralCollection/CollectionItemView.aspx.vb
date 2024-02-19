Imports wpmMineralCollection
Imports LINQHelper.System.Linq.Dynamic
Imports WebProjectMechanics

Public Class MineralCollection_CollectionItemView
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

        If wpm_IsAdmin Then
            cbShowSold.Enabled = True
            cbShowSold.Visible = True
        Else
            cbShowSold.Enabled = False
            cbShowSold.Visible = False
            cbShowSold.Checked = False

        End If

        If wpm_IsAdmin Or wpm_IsEditor Then
            cbShowFeatured.Visible = True
            If Not IsPostBack AndAlso Request.QueryString.Count = 0 Then
                cbShowFeatured.Checked = True
            End If
        Else
            cbShowFeatured.Enabled = False
            cbShowFeatured.Checked = True
        End If
        ShowAdminSearchFields(True)
        If Not IsPostBack Then
            LoadDropdowns()
            For Each s As String In Request.QueryString
                Select Case s
                    Case "CollectionItemID"
                        myListView.CollectionItemID = Request.QueryString("CollectionItemID")
                    Case "CollectionID"
                        myListView.CollectionID = Request.QueryString("CollectionID")
                    Case "SpecimenNumber"
                        myListView.SpecimenNumber = Request.QueryString("SpecimenNumber")
                    Case "Description"
                        myListView.Description = Request.QueryString("Description")
                    Case "MineralID"
                        myListView.MineralID = Request.QueryString("MineralID")
                    Case "CompanyID"
                        myListView.CompanyID = Request.QueryString("CompanyID")
                    Case "MineNM"
                        myListView.MineNM = Request.QueryString("MineNM")
                    Case "LocationCityID"
                        myListView.LocationCityID = Request.QueryString("LocationCityID")
                    Case "LocationStateID"
                        myListView.LocationStateID = Request.QueryString("LocationStateID")
                    Case "LocationCountryID"
                        myListView.LocationCountryID = Request.QueryString("LocationCountryID")
                End Select
            Next
            SetSearchFields()

            myListView.ResetCriteria()
            myListView.MineralID = ddPrimaryMineral.SelectedValue
            myListView.CollectionID = ddlCollection.SelectedValue
            myListView.SpecimenNumber = tbSpecimenNumber.Text
            myListView.Description = tbDescription.Text
            myListView.MineNM = ddMineNM.SelectedValue
            myListView.LocationCityID = ddCity.SelectedValue
            myListView.LocationCountryID = ddCountry.SelectedValue
            myListView.LocationStateID = ddState.SelectedValue
            myListView.FeaturedOnly = cbShowFeatured.Checked
            myListView.SoldOnly = cbShowSold.Checked
            GetRows(myListView)
            wpm_ListPageURL = HttpContext.Current.Request.RawUrl
        End If
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
#Region "Search Implementation"
    Private Sub SetSearchFields()
        ddPrimaryMineral.SelectedValue = String.Empty
        ddlCollection.SelectedValue = String.Empty
        tbSpecimenNumber.Text = String.Empty
        tbDescription.Text = String.Empty
        ddMineNM.SelectedValue = String.Empty
        ddCity.SelectedValue = String.Empty
        ddCountry.SelectedValue = String.Empty
        ddState.SelectedValue = String.Empty

        For Each MyFilter As SQLFilterClause In myListView.MySQLFilter
            Select Case MyFilter.Field
                Case "CollectionItemID"
                    FormView1.AllowPaging = False
                Case "CollectionID"
                    ' Do Nothing
                Case "IsSold"
                    cbShowSold.Checked = wpm_GetDBBoolean(MyFilter.Argument)
                Case "IsFeatured"
                    cbShowFeatured.Checked = wpm_GetDBBoolean(MyFilter.Argument)
                Case "SpecimenNumber"
                    tbSpecimenNumber.Text = MyFilter.Argument
                Case "Description"
                    tbDescription.Text = MyFilter.Argument
                Case "MineralID"
                    ddPrimaryMineral.SelectedValue = MyFilter.Argument
                Case "CompanyID"
                    ' Do Nothing
                Case "MineNM"
                    ddMineNM.SelectedValue = MyFilter.Argument
                Case "LocationCityID"
                    ddCity.SelectedValue = MyFilter.Argument
                Case "LocationStateID"
                    ddState.SelectedValue = MyFilter.Argument
                Case "LocationCountryID"
                    ddCountry.SelectedValue = MyFilter.Argument
                Case Else
                    ' Do Nothing
            End Select
        Next
    End Sub
    Public Sub cmd_ShowSearch_Click()
        ToggleSearchFields(True)
    End Sub
    Public Sub cmd_HideSearch_Click()
        ToggleSearchFields(False)
    End Sub
    Public Sub ToggleSearchFields(ByVal bShowFields As Boolean)
        If bShowFields Then
            pnlFilter.Visible = True
        Else
            pnlFilter.Visible = False
        End If
    End Sub
    Private Sub GetRows(ByVal reqListView As MineralCollectionListView)
        Dim myResults As New List(Of Object)
        myResults.AddRange(reqListView.GetList())
        litResults.Text = reqListView.ResultText
        If myResults.Count = 1 Then
            ' Single Specimen Display
            FormView1.Visible = True
            rptCollectionItems.Visible = True
            GetCurrentRow(reqListView)
        Else
            Try
                FormView1.Visible = False
                rptCollectionItems.Visible = True
                rptCollectionItems.DataSource = myResults
                rptCollectionItems.DataBind()
            Catch ex As Exception
                ApplicationLogging.ErrorLog("CollectionItemView.GetRows - multiple or no result found", ex.ToString)
            End Try
        End If
        If reqListView.CurrentCollectionItemID > 0 Then
            FormView1.Visible = True
            rptCollectionItems.Visible = True
            GetCurrentRow(reqListView)
        End If
    End Sub
    Private Sub GetCurrentRow(ByVal reqListView As MineralCollectionListView)
        Dim myItems As New List(Of MineralCollectionItem)
        myItems.Add(reqListView.GetCurrentItem())
        FormView1.DataSource = myItems
        FormView1.DataBind()
    End Sub

    Private Sub LoadDropdowns()
        Dim myMineralApp As New MineralApplication()

        ddPrimaryMineral.Items.Add(New ListItem With {.Value = 0, .Text = "All Minerals"})
        SetupDropdown(myMineralApp.MineralLookup, ddPrimaryMineral)

        ddMineNM.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Mines"})
        SetupDropdown(myMineralApp.MineLookup, ddMineNM)

        ddCity.Items.Add(New ListItem With {.Value = 0, .Text = "All Cities"})
        SetupDropdown(myMineralApp.CityLookup, ddCity)

        ddState.Items.Add(New ListItem With {.Value = 0, .Text = "All States"})
        SetupDropdown(myMineralApp.StateLookup, ddState)

        ddCountry.Items.Add(New ListItem With {.Value = 0, .Text = "All Countries"})
        SetupDropdown(myMineralApp.CountryLookup, ddCountry)

        ddlCollection.Items.Add(New ListItem With {.Value = String.Empty, .Text = "All Collections"})
        SetupDropdown(myMineralApp.CollectionLookup, ddlCollection)

    End Sub
    Protected Sub cmd_Search_Click(sender As Object, e As EventArgs) Handles cmd_Search.Click
        myListView.ResetCriteria()
        myListView.MineralID = ddPrimaryMineral.SelectedValue
        myListView.CollectionID = ddlCollection.SelectedValue
        myListView.SpecimenNumber = tbSpecimenNumber.Text
        myListView.Description = tbDescription.Text
        myListView.MineNM = ddMineNM.SelectedValue
        myListView.LocationCityID = ddCity.SelectedValue
        myListView.LocationCountryID = ddCountry.SelectedValue
        myListView.LocationStateID = ddState.SelectedValue
        myListView.FeaturedOnly = cbShowFeatured.Checked
        myListView.SoldOnly = cbShowSold.Checked
        rptCollectionItems.Visible = True
        GetRows(myListView)
    End Sub
    Protected Sub ShowAdminSearchFields(ByVal IsAdmin As Boolean)
        ddlCollection.Visible = False
        LabelCollection.Visible = False
    End Sub
    Public Function AdminSectionStart(ByVal ShowAdmin As Boolean) As String
        If ShowAdmin Then
            Return "<hr/><div class="""" style='background-color:#ccc;'><strong>Administration Information</strong>"
        Else
            Return ""
        End If
    End Function
    Public Function AdminSectionEnd(ByVal ShowAdmin As Boolean) As String
        If ShowAdmin Then
            Return "</div>"
        Else
            Return ""
        End If
    End Function
#End Region


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
    Public Function GetSold(ByVal IsSold As Integer, ByVal IsFeatured As Integer) As String
        If IsSold <> 0 Then
            Return "(<span style='color:red;'>SOLD</span>)"
        ElseIf IsFeatured <> 0 Then
            Return "(<span style='color:blue;'>FEATURED</span>)"
        End If
        Return String.Empty
    End Function
    Public Function GetThumbnailClassByOrder(ByVal iOrder As Integer) As String
        If iOrder = 0 Then
            Return " selected "
        Else
            Return " "
        End If
    End Function
    Public Function GetCarouselImageClassByOrder(ByVal iOrder As Integer) As String
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
