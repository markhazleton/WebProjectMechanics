Imports System.ComponentModel
Imports System.Xml.Serialization

<Serializable()> Public Class wpmSiteList
    Inherits List(Of wpmSite)
    Private SearchDomain As String

    Public Sub New()

    End Sub
    Private ReadOnly Property FileName() As String
        Get
            Return App.Config.ConfigFolderPath & "wpmSiteList.xml"
        End Get
    End Property
    Private Sub Save(ByVal fname As String, ByVal obj As wpmSiteList)
        Dim sw As New StreamWriter(fname)
        Try
            Dim xs As New XmlSerializer(GetType(wpmSiteList))
            xs.Serialize(sw, obj)
        Finally
            sw.Close()
        End Try
    End Sub

    Public Function AddSite(ByVal DomainName As String, ByVal CompanyID As String, ByVal SQLDBConnString As String) As Boolean
        Dim bReturn As Boolean = False
        If Me.Count > 0 Then
            If GetSiteByDomain(DomainName).CompanyID.ToString() = String.Empty Then
                Dim newSite As New wpmSite
                newSite.CompanyID = CompanyID
                newSite.DomainName = DomainName
                newSite.SQLDBConnString = SQLDBConnString
                Me.Add(newSite)
                bReturn = True
            End If
        Else
            Dim newSite As New wpmSite
            newSite.CompanyID = CompanyID
            newSite.DomainName = DomainName
            newSite.SQLDBConnString = SQLDBConnString
            Me.Add(newSite)
        End If
        Me.Save(FileName, Me)
        Return bReturn
    End Function
    Public Function GetSiteByDomain(ByVal inDomain As String) As wpmSite
        Dim FoundSite As New wpmSite
        FoundSite.CompanyID = String.Empty
        FoundSite.SQLDBConnString = String.Empty
        SearchDomain = inDomain
        For Each mySite As wpmSite In Me.FindAll(AddressOf FindSiteByDomain)
            FoundSite.CompanyID = mySite.CompanyID
            FoundSite.SQLDBConnString = mySite.SQLDBConnString
            FoundSite.AccessDatabasePath = mySite.AccessDatabasePath
        Next
        Return FoundSite
    End Function

    Private Function FindSiteByDomain(ByVal Site As wpmSite) As Boolean
        If Site.DomainName.ToLower = SearchDomain.ToLower Then
            Return True
        Else
            Return False
        End If
    End Function

End Class

<Serializable()> _
Public Class wpmSite
    Public DomainName As String
    Public CompanyID As String
    Public AccessDatabasePath As String = String.Empty
    Public SQLDBConnString As String = String.Empty
End Class

<DataObject(True)> _
Public Class wpmSiteListAdapter
    Private Property _data() As wpmSiteList
        Get
            If App.SiteList Is Nothing Then
                Return New wpmSiteList()
            Else
                Return App.SiteList
            End If
        End Get
        Set(ByVal value As wpmSiteList)
            App.SiteList = value
        End Set
    End Property

    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
 Public Function GetAll() As wpmSiteList
        Return _data
    End Function

    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Sub DeleteSite(ByVal Site As wpmSite)
        Dim SiteList As wpmSiteList = _data
        SiteList.Remove(Site)
        _data = SiteList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Sub UpdateSite(ByVal Site As wpmSite)
        Dim SiteList As wpmSiteList = _data
        SiteList.Remove(Site)
        SiteList.Add(Site)
        _data = SiteList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Sub InsertSite(ByVal Site As wpmSite)
        Dim SiteList As wpmSiteList = _data
        SiteList.Add(Site)
        _data = SiteList
    End Sub
End Class