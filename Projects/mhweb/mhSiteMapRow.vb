Public Class mhSiteMapRow
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _PageID = value
        End Set
    End Property
    Private _PageTypeCD As String
    Public Property PageTypeCD() As String
        Get
            Return _PageTypeCD
        End Get
        Set(ByVal value As String)
            _PageTypeCD = value
        End Set
    End Property
    Private _myBreadCrumbRows As New mhBreadCrumbRows
    Public Property BreadCrumbRows() As mhBreadCrumbRows
        Get
            Return _myBreadCrumbRows
        End Get
        Set(ByVal value As mhBreadCrumbRows)
            _myBreadCrumbRows = value
        End Set
    End Property

    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _PageTitle As String
    Public Property PageTitle() As String
        Get
            Return _PageTitle
        End Get
        Set(ByVal value As String)
            _PageTitle = value
        End Set
    End Property
    Private _PageDescription As String
    Public Property PageDescription() As String
        Get
            Return _PageDescription
        End Get
        Set(ByVal value As String)
            _PageDescription = value
        End Set
    End Property
    Private _ParentPageID As String
    Public Property ParentPageID() As String
        Get
            Return _ParentPageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _ParentPageID = value
        End Set
    End Property
    Private _RecordSource As String

    Public Property RecordSource() As String
        Get
            Return _RecordSource
        End Get
        Set(ByVal value As String)
            _RecordSource = value
        End Set
    End Property
    Private _PageKeywords As String

    Public Property PageKeywords() As String
        Get
            Return _PageKeywords
        End Get
        Set(ByVal value As String)
            _PageKeywords = value
        End Set
    End Property

    Private _TransferURL As String
    Public Property TransferURL() As String
        Get
            Return _TransferURL
        End Get
        Set(ByVal value As String)
            _TransferURL = value
        End Set
    End Property
    Private _DisplayURL As String
    Public Property DisplayURL() As String
        Get
            Return _DisplayURL
        End Get
        Set(ByVal value As String)
            _DisplayURL = LCase(value)
        End Set
    End Property
    Private _BreadCrumbURL As String
    Public Property BreadCrumbURL() As String
        Get
            Return _BreadCrumbURL
        End Get
        Set(ByVal value As String)
            _BreadCrumbURL = LCase(value)
        End Set
    End Property

    Private _LevelNBR As Integer
    Public Property LevelNBR() As Integer
        Get
            Return _LevelNBR
        End Get
        Set(ByVal value As Integer)
            _LevelNBR = value
        End Set
    End Property

    Private _PageFileName As String
    Public Property PageFileName() As String
        Get
            Return _PageFileName
        End Get
        Set(ByVal value As String)
            _PageFileName = value
        End Set
    End Property

    Private _ModifiedDate As Date

    Public Property ModifiedDate() As Date
        Get
            Return _ModifiedDate
        End Get
        Set(ByVal value As Date)
            _ModifiedDate = value
        End Set
    End Property

    Private _ArticleID As String
    Public Property ArticleID() As String
        Get
            Return _ArticleID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If
            _ArticleID = value
        End Set
    End Property
    Private _ActiveFL As Boolean

    Public Property ActiveFL() As Boolean
        Get
            Return _ActiveFL
        End Get
        Set(ByVal value As Boolean)
            _ActiveFL = value
        End Set
    End Property

    Private _BreadCrumbHTML As String
    Public Property BreadCrumbHTML() As String
        Get
            Return _BreadCrumbHTML
        End Get
        Set(ByVal value As String)
            _BreadCrumbHTML = value
        End Set
    End Property
    Private _MainMenuURL As String
    Public Property MainMenuURL() As String
        Get
            Return _MainMenuURL
        End Get
        Set(ByVal value As String)
            _MainMenuURL = value
        End Set
    End Property
    Private _MainMenuPageID As String
    Public Property MainMenuPageID() As String
        Get
            Return _MainMenuPageID
        End Get
        Set(ByVal value As String)
            If value = "0" Then
                value = ""
            End If

            _MainMenuPageID = value
        End Set
    End Property
    Private _MainMenuPageName As String
    Public Property MainMenuPageName() As String
        Get
            Return _MainMenuPageName
        End Get
        Set(ByVal value As String)
            _MainMenuPageName = value
        End Set
    End Property
    Private _TopMenu As String
    Public Property TopMenu() As String
        Get
            Return _TopMenu
        End Get
        Set(ByVal value As String)
            _TopMenu = value
        End Set
    End Property
    Private _SubMenu As String
    Public Property SubMenu() As String
        Get
            Return _SubMenu
        End Get
        Set(ByVal value As String)
            _SubMenu = value
        End Set
    End Property
    Private _siteCategoryName As String
    Public Property SiteCategoryName() As String
        Get
            Return _siteCategoryName
        End Get
        Set(ByVal Value As String)
            _siteCategoryName = Value
        End Set
    End Property

    Private _siteCategoryID As String
    Public Property SiteCategoryID() As String
        Get
            Return _siteCategoryID
        End Get
        Set(ByVal Value As String)
            If Value = "0" Then
                Value = ""
            End If
            _siteCategoryID = Value
        End Set
    End Property

    Private _siteCategoryGroupName As String
    Public Property SiteCategoryGroupName() As String
        Get
            Return _siteCategoryGroupName
        End Get
        Set(ByVal Value As String)
            _siteCategoryGroupName = Value
        End Set
    End Property

End Class

Public Class mhSiteMapRows
    Inherits List(Of mhSiteMapRow)

    Public Function PopulateSiteMapCol(ByVal OrderBy As String, ByVal CompanyID As String, ByVal GroupID As String, ByRef mySiteFile As mhSiteFile) As Boolean
        Dim bReturn As Boolean = True
        Dim sTransferURL As String = ("")
        Dim sTransferParms As String = ("")
        For Each myrow As DataRow In mhDataCon.GetSiteMap(OrderBy, CompanyID, GroupID).Rows
            Dim MySiteRow As New mhSiteMapRow
            MySiteRow.PageID = mhUTIL.GetDBString(myrow("PageID"))
            MySiteRow.PageName = mhUTIL.GetDBString(myrow("PageName"))
            MySiteRow.PageTitle = mhUTIL.GetDBString(myrow("PageTitle"))
            MySiteRow.ActiveFL = mhUTIL.GetDBBoolean(myrow("Active"))
            MySiteRow.ArticleID = mhUTIL.GetDBString(myrow("ArticleID"))
            MySiteRow.ModifiedDate = mhUTIL.GetDBDate(myrow("ModifiedDT"))
            MySiteRow.PageDescription = mhUTIL.GetDBString(myrow("Description"))
            MySiteRow.PageFileName = mhUTIL.GetDBString(myrow("PageFileName"))
            MySiteRow.PageKeywords = mhUTIL.GetDBString(myrow("PageKeywords"))
            MySiteRow.ParentPageID = mhUTIL.GetDBString(myrow("ParentPageID"))
            MySiteRow.RecordSource = mhUTIL.GetDBString(myrow("PageSource"))
            sTransferURL = mhUTIL.GetDBString(myrow("TransferURL"))
            MySiteRow.SiteCategoryID = mhUTIL.GetDBString(myrow("SiteCategoryID"))
            MySiteRow.SiteCategoryName = mhUTIL.GetDBString(myrow("SitecategoryName"))
            MySiteRow.SiteCategoryGroupName = mhUTIL.GetDBString(myrow("SiteCategoryGroupName"))
            MySiteRow.PageTypeCD = mhUTIL.GetDBString(myrow("PageTypeCD"))
            If Trim(sTransferURL) = String.Empty Then
                sTransferURL = MySiteRow.PageFileName
            End If
            ' Check for Recursive Parent
            If (MySiteRow.PageID = MySiteRow.ParentPageID And MySiteRow.ParentPageID <> "") Then
                MySiteRow.ParentPageID = ""
                mhUTIL.AuditLog("PageID=ParentPageID (" & MySiteRow.PageName & " - " & MySiteRow.PageID & ") ", "mhSiteMapRow.PopulateSiteMapCol")
            End If

            Select Case MySiteRow.RecordSource
                Case "Page"
                    sTransferParms = "c=" & MySiteRow.PageID
                    If MySiteRow.ArticleID <> "" Then
                        sTransferParms = sTransferParms & "&a=" & MySiteRow.ArticleID
                    End If
                Case "Article"
                    sTransferParms = "a=" & MySiteRow.ArticleID
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = sTransferParms & "&c=" & MySiteRow.PageID
                    End If
                Case "Image"
                    sTransferParms = "i=" & MySiteRow.ArticleID
                    If MySiteRow.PageID <> "" Then
                        sTransferParms = sTransferParms & "&c=" & MySiteRow.PageID
                    End If
                Case "Category"
                    If Trim(MySiteRow.PageFileName) <> String.Empty Then
                        sTransferURL = MySiteRow.PageFileName
                        MySiteRow.PageTypeCD = "NOFILENAME"

                    End If
                    MySiteRow.PageID = "CAT-" & MySiteRow.PageID
                    If MySiteRow.ParentPageID <> "" Then
                        MySiteRow.ParentPageID = "CAT-" & MySiteRow.ParentPageID
                    End If
                    sTransferParms = "c=" & MySiteRow.PageID
                Case Else
                    sTransferParms = "c=" & MySiteRow.PageID & "&rc=" & MySiteRow.RecordSource
            End Select

            If sTransferURL.IndexOf("?") > 0 Then
                If InStr(sTransferURL, "c=") = 0 Then
                    sTransferURL = sTransferURL & "&" & sTransferParms
                End If
            Else
                sTransferURL = sTransferURL & "?" & sTransferParms
            End If

            If MySiteRow.PageTypeCD = "NO FILE NAME" Then
                MySiteRow.TransferURL = "/default.aspx"
                MySiteRow.DisplayURL = MySiteRow.PageFileName
                MySiteRow.BreadCrumbHTML = MySiteRow.PageFileName
            Else
                MySiteRow.TransferURL = sTransferURL
                sTransferURL = ""
                sTransferParms = ""
                MySiteRow.DisplayURL = mhUTIL.FormatPageNameURL(MySiteRow.PageName)
            End If
            If MySiteRow.RecordSource = "Page" And MySiteRow.ParentPageID = String.Empty And MySiteRow.SiteCategoryID <> String.Empty Then
                MySiteRow.ParentPageID = "CAT-" & MySiteRow.SiteCategoryID
            End If

            Me.Add(MySiteRow)
        Next
        Return bReturn
    End Function
    Public Function BuildBreadcrumbRows(ByVal CompanyName As String) As Boolean
        Dim LevelCount As Integer = 1
        Dim sBreadCrumbHTML As String = String.Empty
        For Each myRow As mhSiteMapRow In Me
            If Trim(myRow.ParentPageID) <> String.Empty Then
                BuildParentBreadcrumbs(myRow.BreadCrumbRows, myRow.ParentPageID, LevelCount)
            End If
            myRow.BreadCrumbRows.Add(GetBreadcrumbRow(myRow, LevelCount))
            Dim myFullPathURL As New StringBuilder("")
            For Each BCrow As mhBreadCrumbRow In myRow.BreadCrumbRows
                If BCrow.LevelNBR < LevelCount Then
                    BCrow.MenuLevelNBR = (LevelCount - BCrow.LevelNBR)
                    sBreadCrumbHTML = "<li><a href=""" & BCrow.DisplayURL & """ title=""" & BCrow.PageDescription & """>" & BCrow.PageName & "</a></li>" & sBreadCrumbHTML
                    myFullPathURL.Insert(0, "/" & mhUTIL.FormatPageNameForURL(BCrow.PageName))
                Else
                    BCrow.MenuLevelNBR = LevelCount
                End If
            Next
            myFullPathURL.Append(myRow.DisplayURL)
            myRow.BreadCrumbURL = myFullPathURL.ToString
            myRow.BreadCrumbHTML = "<ul><li><a href=""/"">" & mhUTIL.FormatPageNameForURL(CompanyName) & "</a></li>" & sBreadCrumbHTML & "</ul>"
            myRow.LevelNBR = LevelCount
            ' Reset Variables
            LevelCount = 1
            sBreadCrumbHTML = ""
        Next
        Return True
    End Function
    Private Function GetBreadcrumbRow(ByVal myrow As mhSiteMapRow, ByVal LevelNBR As Integer) As mhBreadCrumbRow
        Dim bcRow As New mhBreadCrumbRow
        bcRow.MenuLevelNBR = 0
        bcRow.LevelNBR = LevelNBR
        bcRow.PageID = myrow.PageID
        bcRow.PageName = myrow.PageName
        bcRow.ParentPageID = myrow.ParentPageID
        bcRow.DisplayURL = myrow.BreadCrumbURL
        bcRow.PageDescription = myrow.PageDescription
        Return bcRow
    End Function
    Private Function BuildParentBreadcrumbs(ByRef BreadCrumbRows As mhBreadCrumbRows, ByVal pageID As String, ByRef LevelCount As Integer) As Boolean
        For Each myrow As mhSiteMapRow In Me
            If LevelCount < 10 Then
                If myrow.RecordSource = "Page" Or myrow.RecordSource = "Category" Then
                    If myrow.PageID = pageID Then
                        BreadCrumbRows.Add(GetBreadcrumbRow(myrow, LevelCount))
                        LevelCount = LevelCount + 1
                        If Trim(myrow.ParentPageID) <> "" Then
                            BuildParentBreadcrumbs(BreadCrumbRows, myrow.ParentPageID, LevelCount)
                        End If
                        Exit For
                    End If
                End If
            End If
        Next
        Return True
    End Function
    Public Function GetPageRow(ByVal CurrentPageID As String, ByVal CurrentArticleID As String) As mhSiteMapRow
        Dim sbSubMenu As New StringBuilder("")
        Dim ReturnMapRow As New mhSiteMapRow
        For Each myRow As mhSiteMapRow In Me
            If myRow.RecordSource = "Page" Or myRow.RecordSource = "Category" Then
                If myRow.PageID = CurrentPageID Then
                    ReturnMapRow.ModifiedDate = myRow.ModifiedDate
                    ReturnMapRow.ActiveFL = myRow.ActiveFL
                    ReturnMapRow.RecordSource = myRow.RecordSource
                    ReturnMapRow.PageID = myRow.PageID
                    ReturnMapRow.ParentPageID = myRow.ParentPageID
                    ReturnMapRow.ArticleID = myRow.ArticleID
                    ReturnMapRow.PageName = myRow.PageName
                    ReturnMapRow.PageTitle = myRow.PageTitle
                    ReturnMapRow.PageKeywords = myRow.PageKeywords
                    ReturnMapRow.PageDescription = myRow.PageDescription
                    ReturnMapRow.PageFileName = myRow.PageFileName
                    ReturnMapRow.BreadCrumbHTML = myRow.BreadCrumbHTML
                    ReturnMapRow.LevelNBR = myRow.LevelNBR
                    ReturnMapRow.BreadCrumbRows = myRow.BreadCrumbRows
                    ReturnMapRow.PageTypeCD = myRow.PageTypeCD
                    ReturnMapRow.DisplayURL = myRow.DisplayURL
                    ReturnMapRow.TransferURL = myRow.TransferURL
                    ReturnMapRow.SiteCategoryID = myRow.SiteCategoryID
                    ReturnMapRow.SiteCategoryName = myRow.SiteCategoryName
                    ReturnMapRow.SiteCategoryGroupName = myRow.SiteCategoryGroupName
                    '
                    ' Need to check if current selected page is an Article or Image record for a true page
                    '
                    If myRow.RecordSource = "Page" Then
                        For Each mySiteMapRow As mhSiteMapRow In Me
                            If mySiteMapRow.RecordSource = "Article" Or mySiteMapRow.RecordSource = "Image" Then
                                If mySiteMapRow.PageID = myRow.PageID Then
                                    If mySiteMapRow.ArticleID = CurrentArticleID Then
                                        ReturnMapRow.ArticleID = mySiteMapRow.ArticleID
                                        ReturnMapRow.PageName = mySiteMapRow.PageName
                                        ReturnMapRow.PageKeywords = mySiteMapRow.PageKeywords
                                        ReturnMapRow.PageDescription = mySiteMapRow.PageDescription
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Next
        Return ReturnMapRow
    End Function

End Class
Public Class mhBreadCrumbRow
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            _PageID = value
        End Set
    End Property
    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _PageDescription As String
    Public Property PageDescription() As String
        Get
            Return _PageDescription
        End Get
        Set(ByVal value As String)
            _PageDescription = value
        End Set
    End Property
    Private _ParentPageID As String
    Public Property ParentPageID() As String
        Get
            Return _ParentPageID
        End Get
        Set(ByVal value As String)
            _ParentPageID = value
        End Set
    End Property
    Private _DisplayURL As String
    Public Property DisplayURL() As String
        Get
            Return _DisplayURL
        End Get
        Set(ByVal value As String)
            _DisplayURL = value
        End Set
    End Property
    Private _LevelNBR As Integer
    Public Property LevelNBR() As Integer
        Get
            Return _LevelNBR
        End Get
        Set(ByVal value As Integer)
            _LevelNBR = value
        End Set
    End Property
    Private _MenuLevelNBR As Integer
    Public Property MenuLevelNBR() As Integer
        Get
            Return _MenuLevelNBR
        End Get
        Set(ByVal value As Integer)
            _MenuLevelNBR = value
        End Set
    End Property

End Class
Public Class mhBreadCrumbRows
    Inherits List(Of mhBreadCrumbRow)
End Class
