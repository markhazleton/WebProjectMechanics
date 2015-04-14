Public Class Location
    Inherits UtilityItemForCollection
    Implements IComparable(Of Location)
    Implements ILocation

    Public Property RecordSource As New String(CType(String.Empty, Char())) Implements ILocation.RecordSource
    Public Property LocationID() As New String(CType(String.Empty, Char())) Implements ILocation.LocationID
    Public Property ArticleID() As Integer Implements ILocation.ArticleID
    Public Property LocationTypeCD As New String(CType(String.Empty, Char())) Implements ILocation.LocationTypeCD
    Public Property LocationTypeID As Integer Implements ILocation.LocationTypeID
    Public Property GroupID As Integer Implements ILocation.GroupID

    Public Property LocationName() As String Implements ILocation.LocationName
    Public Property LocationTitle() As String Implements ILocation.LocationTitle
    Public Property LocationDescription() As String Implements ILocation.LocationDescription
    Public Property LocationKeywords() As String Implements ILocation.LocationKeywords
    Public Property LocationFileName() As String Implements ILocation.LocationFileName
    Public Property LocationBody As String Implements ILocation.LocationBody
    Public Property LocationSummary As String Implements ILocation.LocationSummary
    Public Property IncludeInNavigation As Boolean Implements ILocation.IncludeInNavigation
    Public Property RowsPerPage As Integer Implements ILocation.RowsPerPage
    Public Property ImagesPerRow As Integer Implements ILocation.ImagesPerRow


    Public Property ParentLocationID() As String Implements ILocation.ParentLocationID
    Public Property DefaultOrder() As Integer Implements ILocation.DefaultOrder

    Public Property SiteCategoryID() As New String(CType(String.Empty, Char())) Implements ILocation.SiteCategoryID
    Public Property SiteCategoryName() As New String(CType(String.Empty, Char())) Implements ILocation.SiteCategoryName
    Public Property LocationGroupID() As New String(CType(String.Empty, Char())) Implements ILocation.LocationGroupID
    Public Property LocationGroupNM() As New String(CType(String.Empty, Char())) Implements ILocation.LocationGroupNM

    Public Property SiteTypeID As New String(CType(String.Empty, Char())) Implements ILocation.SiteTypeID
    
    Public Property ActiveFL() As Boolean Implements ILocation.ActiveFL

    Public Property LevelNBR() As Integer Implements ILocation.LevelNBR
    Public Property TransferURL() As New String(CType(String.Empty, Char())) Implements ILocation.TransferURL

    Public Property BreadCrumbURL() As New String(CType(String.Empty, Char())) Implements ILocation.BreadCrumbURL
    Public Property BreadCrumbHTML() As New String(CType(String.Empty, Char())) Implements ILocation.BreadCrumbHTML
    Public Property MainMenuURL() As New String(CType(String.Empty, Char())) Implements ILocation.MainMenuURL
    Public Property MainMenuLocationID() As New String(CType(String.Empty, Char())) Implements ILocation.MainMenuLocationID
    Public Property MainMenuLocationName() As New String(CType(String.Empty, Char())) Implements ILocation.MainMenuLocationName

    Public Property LocationTrailList() As New List(Of LocationTrail) Implements ILocation.LocationTrailList

    Private _ModifiedDT As DateTime
    Public Property ModifiedDT As DateTime Implements ILocation.ModifiedDT
        Get
            If IsNothing(_ModifiedDT) Or _ModifiedDT = DateTime.MinValue Then
                _ModifiedDT = Now()
            End If
            Return _ModifiedDT
        End Get
        Set(value As DateTime)
            _ModifiedDT = value
        End Set
    End Property

    Public ReadOnly Property ParentCategoryID As String Implements ILocation.ParentCategoryID
        Get
            If ParentLocationID.Contains("CAT") Then
                Return ParentLocationID.Replace("CAT-", String.Empty)
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public ReadOnly Property ParentPageID As String Implements ILocation.ParentPageID
        Get
            If ParentLocationID.Contains("CAT") Then
                Return String.Empty
            Else
                Return ParentLocationID
            End If
        End Get
    End Property
    Public ReadOnly Property GetSiteCategoryID As String Implements ILocation.GetSiteCategoryID
        Get
            If RecordSource = "Category" Then
                Return LocationID.Replace("CAT-", String.Empty)
            ElseIf String.IsNullOrEmpty(SiteCategoryID) Then
                If ParentLocationID.Contains("CAT") Then
                    Return ParentLocationID.Replace("CAT-", String.Empty)
                Else
                    Return String.Empty
                End If
            Else
                Return SiteCategoryID
            End If
        End Get
    End Property
    Public ReadOnly Property DisplayInMenu() As Boolean Implements ILocation.DisplayInMenu
        Get
            If (RecordSource = "Category" Or (RecordSource = "Page")) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property TransferParms As String Implements ILocation.TransferParms
        Get
            Dim sTransferParms As New String(CType(String.Empty, Char()))
            Select Case RecordSource
                Case "Page"
                    If ArticleID < 1 Then
                        sTransferParms = String.Format("{0}&a={1}", String.Format("c={0}", LocationID), ArticleID)
                    Else
                        sTransferParms = String.Format("c={0}", LocationID)
                    End If
                Case "Article"
                    If LocationID <> "" Then
                        sTransferParms = String.Format("{0}&c={1}", String.Format("a={0}", ArticleID), ParentLocationID)
                    Else
                        sTransferParms = String.Format("a={0}", ArticleID)
                    End If
                Case "Image"
                    If LocationID <> "" Then
                        sTransferParms = String.Format("{0}&c={1}", String.Format("a={0}", ArticleID), ParentLocationID)
                    Else
                        sTransferParms = String.Format("i={0}", ArticleID)
                    End If
                Case "Category"
                    sTransferParms = "c=" & LocationID
                Case Else
                    sTransferParms = String.Format("c={0}&rc={1}", LocationID, RecordSource)
            End Select
            Return sTransferParms
        End Get
    End Property
    Public ReadOnly Property LocationURL As String Implements ILocation.LocationURL
        Get
            If wpm_SiteConfig.Use404Processing Then
                If wpm_CheckForMatch(LocationTypeCD, "NO FILE NAME") Then
                    If LocationFileName.IndexOf("?") > 0 Then
                        If InStr(LocationFileName, "c=") = 0 Then
                            LocationFileName = String.Format("{0}&{1}", LocationFileName, TransferParms)
                        End If
                    Else
                        LocationFileName = String.Format("{0}?{1}", LocationFileName, TransferParms)
                    End If
                    Return LocationFileName
                Else
                    Return wpm_FormatPageNameURL(LocationName)
                End If
            Else
                Return TransferURL
            End If
        End Get
    End Property
    Public Function SetLocationID() As String
        Dim sTransferURL As New String(CType(TransferURL, Char()))
        ' Check for Recursive Parent and Zero Parent
        If ParentLocationID = "0" Then
            ParentLocationID = String.Empty
        End If
        If (LocationID = ParentLocationID And ParentLocationID <> "") Then
            ParentLocationID = ""
            ApplicationLogging.ErrorLog(String.Format("PageID=ParentPageID ({0} - {1}) ", LocationName, LocationID), "Location.SetLocationID")
        End If
        Select Case RecordSource
            Case "Page"
                ' Do Nothing
            Case "Article"
                If LocationID <> "" Then
                    ParentLocationID = LocationID
                    LocationID = String.Format("ART-{0}", ArticleID)
                Else
                    LocationID = String.Format("ART-{0}", ArticleID)
                End If
            Case "Image"
                If LocationID <> "" Then
                    ParentLocationID = LocationID
                    LocationID = String.Format("IMG-{0}", ArticleID)
                Else
                    LocationID = String.Format("IMG-{0}", ArticleID)
                End If
            Case "Category"
                SiteCategoryID = LocationID
                LocationID = String.Format("CAT-{0}", LocationID)
                If ParentLocationID <> "" Then
                    ParentLocationID = "CAT-" & ParentLocationID
                End If
            Case Else
                ' do nothing 
        End Select
        If (LocationFileName.Trim) <> String.Empty Then
            sTransferURL = LocationFileName
            LocationTypeCD = "NOFILENAME"
        ElseIf sTransferURL.IndexOf("?") > 0 Then
            If InStr(sTransferURL, "c=") = 0 Then
                sTransferURL = String.Format("{0}&{1}", sTransferURL, TransferParms)
            End If
        Else
            sTransferURL = String.Format("{0}?{1}", sTransferURL, TransferParms)
        End If

        If LocationTypeCD = "NO FILE NAME" Then
            If LocationFileName.IndexOf("?") > 0 Then
                If InStr(LocationFileName, "c=") = 0 Then
                    LocationFileName = String.Format("{0}&{1}", LocationFileName, TransferParms)
                End If
            Else
                LocationFileName = String.Format("{0}?{1}", LocationFileName, TransferParms)
            End If
            TransferURL = LocationFileName
            BreadCrumbHTML = LocationFileName
        Else
            TransferURL = sTransferURL
        End If
        If RecordSource = "Page" And ParentLocationID = String.Empty And SiteCategoryID <> String.Empty Then
            ParentLocationID = "CAT-" & SiteCategoryID
        End If

        If wpm_CheckForMatch(LocationTypeCD, "MODULE") Then
            RecordSource = "Page"
        End If
        If LocationName = String.Empty Then
            LocationName = LocationTitle
        End If
        Return LocationID
    End Function
    Public Function CopyLocation(ByRef SourceLocation As Location) As Location
        ModifiedDT = SourceLocation.ModifiedDT
        ActiveFL = SourceLocation.ActiveFL
        RecordSource = SourceLocation.RecordSource
        LocationID = SourceLocation.LocationID
        ParentLocationID = SourceLocation.ParentLocationID
        ArticleID = SourceLocation.ArticleID
        LocationName = SourceLocation.LocationName
        LocationFileName = SourceLocation.LocationFileName
        LocationTitle = SourceLocation.LocationTitle
        LocationKeywords = SourceLocation.LocationKeywords
        LocationDescription = SourceLocation.LocationDescription
        BreadCrumbHTML = SourceLocation.BreadCrumbHTML
        LevelNBR = SourceLocation.LevelNBR
        LocationTrailList = SourceLocation.LocationTrailList
        LocationTypeCD = SourceLocation.LocationTypeCD
        LocationTypeID = SourceLocation.LocationTypeID
        GroupID = SourceLocation.GroupID
        TransferURL = SourceLocation.TransferURL
        BreadCrumbURL = SourceLocation.BreadCrumbURL
        SiteCategoryID = SourceLocation.SiteCategoryID
        SiteCategoryName = SourceLocation.SiteCategoryName
        LocationGroupNM = SourceLocation.LocationGroupNM
        LocationGroupID = SourceLocation.LocationGroupID
        RowsPerPage  = SourceLocation.RowsPerPage
        ImagesPerRow = SourceLocation.ImagesPerRow
        DefaultOrder = SourceLocation.DefaultOrder
        If (SourceLocation.RecordSource = "Image" Or SourceLocation.RecordSource = "Article") Then
            ArticleID = SourceLocation.ArticleID
            LocationName = SourceLocation.LocationName
            LocationKeywords = SourceLocation.LocationKeywords
            LocationDescription = SourceLocation.LocationDescription
        End If
        Return Me
    End Function

    Public Function CompareTo(ByVal other As Location) As Integer Implements System.IComparable(Of Location).CompareTo
        Return DefaultOrder.CompareTo(other.DefaultOrder)
    End Function

End Class