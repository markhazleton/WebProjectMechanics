
Public Class wpmSiteListControler
    Private WithEvents mySite As ISiteUserControl
#Region "Public Events"
    Public Event ListChange()
    Public Event CancelEdit()
    Public Event InvalidSite(ByVal errormessage As String)
#End Region
    Private Shared ReadOnly Property FileName As String
        Get
            Return wpmApp.Config.SiteListPath
        End Get
    End Property
    Sub New(ByRef yourview As Object)
        mySite = CType(yourview, ISiteUserControl)
    End Sub

    Public Shared Function GetSiteList() As List(Of String)
        Dim mylist As New List(Of String)
        mylist.AddRange(From p In New wpmSiteList(FileName).GetSiteList Order By p.DomainName Ascending Select p.DomainName)
        Return mylist
    End Function

    Sub ShowASite(ByVal SiteName As String)
        mySite.yourSite = New wpmSiteList(FileName).FindSiteByName(SiteName)
    End Sub

    Sub RemoveASite(ByVal SiteName As String)
        Dim mySiteList As New wpmSiteList(FileName)
        mySiteList.RemoveSiteByName(SiteName)
        RaiseEvent ListChange()
    End Sub

    Private Sub mysite_CancelEdit() Handles mySite.CancelEdit
        RaiseEvent CancelEdit()
    End Sub

    Private Sub mysite_SaveSite(ByVal yourSite As wpmSite) Handles mySite.SaveSite
        Dim ErrorMessage As String = String.Empty
        If New wpmSiteBL(yourSite).IsValid(ErrorMessage) Then
            Dim mySiteList As New wpmSiteList(FileName)
            mySiteList.Update(yourSite)
            RaiseEvent ListChange()
        Else
            RaiseEvent InvalidSite(ErrorMessage)
        End If
    End Sub

End Class
