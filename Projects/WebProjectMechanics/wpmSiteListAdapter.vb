Imports System.ComponentModel


'<DataObject(True)> _
'Public Class wpmSiteListAdapter
'    Private Shared Sub SetData(ByVal value As wpmSiteList)
'        wpmApp.SiteList = value
'    End Sub
'    Private Shared Function GetData() As wpmSiteList
'        If wpmApp.SiteList Is Nothing Then
'            Return New wpmSiteList()
'        Else
'            Return wpmApp.SiteList
'        End If
'    End Function

'    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
'    Public Shared Function GetAll() As wpmSiteList
'        Return GetData()
'    End Function

'    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
'    Public Shared Sub DeleteSite(ByVal Site As wpmSite)
'        Dim SiteList As wpmSiteList = GetData()
'        SiteList.Remove(Site)
'        SetData(SiteList)
'    End Sub

'    <DataObjectMethod(DataObjectMethodType.Update, True)> _
'    Public Shared Sub UpdateSite(ByVal Site As wpmSite)
'        Dim SiteList As wpmSiteList = GetData()
'        SiteList.Remove(Site)
'        SiteList.Add(Site)
'        SetData(SiteList)
'    End Sub

'    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
'    Public Shared Sub InsertSite(ByVal Site As wpmSite)
'        Dim SiteList As wpmSiteList = GetData()
'        SiteList.Add(Site)
'        SetData(SiteList)
'    End Sub
'End Class