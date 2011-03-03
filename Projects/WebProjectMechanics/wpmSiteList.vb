Imports System.Xml.Serialization
Imports System.IO

Public Class wpmSiteList
    Dim mySiteList As New List(Of wpmSite)

    Public Function GetSiteList() As List(Of wpmSite)
        Return mySiteList
    End Function

    Private FilePath As String

    Public Sub New(ByVal myPath As String)
        FilePath = myPath
        LoadList()
    End Sub

    Public Sub LoadList()
        If File.Exists(FilePath) Then
            Using reader As New StreamReader(FilePath)
                Dim serializer As New XmlSerializer(GetType(List(Of wpmSite)))
                mySiteList = DirectCast(serializer.Deserialize(reader), List(Of wpmSite))
            End Using
        Else
            mySiteList.Clear()
            mySiteList.Add(New wpmSite With {.DomainName = "LOCALHOST", .CompanyID = String.Empty, .AccessDatabasePath = String.Empty, .SQLDBConnString = String.Empty})
        End If
        SaveXML()
    End Sub

    Public Sub xmlloadlist()
        mySiteList.Clear()
        If File.Exists(FilePath) Then
            Dim myxdoc As New XDocument
            myxdoc = XDocument.Load(FilePath)
            For Each p In myxdoc...<Site>
                Dim newSite As New wpmSite() With {.DomainName = p.<Name>.Value.ToLower}
                mySiteList.Add(newSite)
            Next
        Else
            mySiteList.Add(New wpmSite With {.DomainName = String.Empty, .CompanyID = String.Empty, .AccessDatabasePath = String.Empty, .SQLDBConnString = String.Empty})
        End If
        SaveXML()


    End Sub

    Public Sub Update(ByVal mySite As wpmSite)
        Dim bFound As Boolean
        LoadList()

        For Each p As wpmSite In mySiteList
            If p.DomainName.ToLower = mySite.DomainName.ToLower Then
                p.CompanyID = mySite.CompanyID
                p.DomainName = mySite.DomainName.ToLower
                p.SQLDBConnString = mySite.SQLDBConnString
                p.AccessDatabasePath = mySite.AccessDatabasePath
                bFound = True
                Exit For
            End If
        Next
        If Not bFound Then
            mySiteList.Add(mySite)
        End If
        SaveXML()
    End Sub



    Private Sub SaveXML()
        Using oStmW As New StreamWriter(FilePath)
            Dim oXS As XmlSerializer = New XmlSerializer(GetType(List(Of wpmSite)))
            oXS.Serialize(oStmW, mySiteList)
        End Using
    End Sub


    Public Function FindSiteByDomain(ByVal findDomainName As String) As wpmSite
        Dim mySite As New wpmSite
        For Each p As wpmSite In mySiteList
            If p.DomainName.ToLower = findDomainName.ToLower Then
                mySite = p
                Exit For
            End If
        Next
        Return mySite
    End Function

    Public Function FindSiteByName(ByVal findSiteName As String) As wpmSite
        Dim mySite As New wpmSite
        For Each p As wpmSite In mySiteList
            If p.DomainName.ToLower = findSiteName.ToLower Then
                mySite = p
                Exit For
            End If
        Next
        Return mySite
    End Function
    Public Function RemoveSiteByName(ByVal findSiteName As String) As wpmSiteList
        LoadList()
        For Each p As wpmSite In mySiteList
            If p.DomainName.ToLower = findSiteName.ToLower Then
                mySiteList.Remove(p)
                Exit For
            End If
        Next
        SaveXML()
        Return Me
    End Function

End Class




'<Serializable()> Public Class wpmSiteList
'    Inherits List(Of wpmSite)
'    Implements IwpmSiteList
'    Private SearchDomain As String

'    Public Sub New()

'    End Sub
'    Public Sub New(ByVal capacity As Integer)
'        MyBase.New(capacity)
'    End Sub
'    Public Sub New(ByVal collection As IEnumerable(Of wpmSite))
'        MyBase.New(collection)
'    End Sub

'    Private Shared ReadOnly Property FileName() As String
'        Get
'            Return wpmApp.Config.ConfigFolderPath & "wpmSiteList.xml"
'        End Get
'    End Property
'    Private Shared Sub Save(ByVal fname As String, ByVal obj As wpmSiteList)
'        Using sw As New StreamWriter(fname)
'            Try
'                Dim xs As New XmlSerializer(GetType(wpmSiteList))
'                xs.Serialize(sw, obj)
'            Catch ex As Exception
'                wpmLogging.ErrorLog("Error on SiteList Save", ex.ToString)
'            End Try
'        End Using
'    End Sub

'    Public Function AddSite(ByVal DomainName As String, ByVal CompanyID As String, ByVal SQLDBConnString As String) As Boolean Implements IwpmSiteList.AddSite
'        Dim bReturn As Boolean = False
'        If Count > 0 Then
'            If GetSiteByDomain(DomainName).CompanyID.ToString() = String.Empty Then
'                Dim newSite As New wpmSite() With {.CompanyID = CompanyID, .DomainName = DomainName, .SQLDBConnString = SQLDBConnString}
'                Add(newSite)
'                bReturn = True
'            End If
'        Else
'            Dim newSite As New wpmSite() With {.CompanyID = CompanyID, .DomainName = DomainName, .SQLDBConnString = SQLDBConnString}
'            Add(newSite)
'        End If
'        wpmSiteList.Save(FileName, Me)
'        Return bReturn
'    End Function
'    Public Function GetSiteByDomain(ByVal inDomain As String) As wpmSite Implements IwpmSiteList.GetSiteByDomain
'        Dim FoundSite As New wpmSite() With {.CompanyID = String.Empty, .SQLDBConnString = String.Empty, .AccessDatabasePath = String.Empty}
'        SearchDomain = inDomain
'        For Each mySite As wpmSite In FindAll(AddressOf FindSiteByDomain)
'            FoundSite.CompanyID = mySite.CompanyID
'            FoundSite.SQLDBConnString = mySite.SQLDBConnString
'            FoundSite.AccessDatabasePath = mySite.AccessDatabasePath
'        Next
'        Return FoundSite
'    End Function

'    Private Function FindSiteByDomain(ByVal Site As wpmSite) As Boolean
'        If Site.DomainName.ToLower = SearchDomain.ToLower Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function

'End Class
