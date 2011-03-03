Imports System.IO
Imports System.Xml.Serialization
Imports System.Web.Configuration
Imports System.Text

Public NotInheritable Class wpmApp
    Public Shared Config As New wpmFileConfig
    Public Shared SiteList As wpmSiteList = LoadSiteList()

#Region "Message Stack"
    Public Shared MessageStack As New List(Of String)
    Public Shared Function AddMessage(ByVal Message As String) As Boolean
        MessageStack.Add(Message)
        Return True
    End Function
    Public Shared Function ResetMessageStack() As Boolean
        MessageStack.Clear()
        Return True
    End Function
    Public Shared Function GetMessageList() As String
        If MessageStack.Count > 0 Then
            Dim myReturn As New StringBuilder("<ul>")
            For Each myMessage As String In MessageStack
                myReturn.Append(String.Format("<li>{0}</li>", myMessage))
            Next
            myReturn.Append("</ul>")
            Return myReturn.ToString
        Else
            Return String.Empty
        End If
    End Function
#End Region
    Public Shared ReadOnly Property wpmConfigFile() As String
        Get
            If Not IsNothing(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")) Then
                Return String.Format("{0}{1}.xml", HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings.Item("wpmConfigFolder")), Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            Else
                Return String.Format("{0}{1}.xml", HttpContext.Current.Server.MapPath("~/access_db/"), Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            End If
        End Get
    End Property
    Public Shared ReadOnly Property ConnStr() As String
        Get
            Dim mySite As wpmSite = wpmApp.SiteList.FindSiteByDomain(Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", ""))
            If Not (mySite.AccessDatabasePath Is Nothing) AndAlso (mySite.AccessDatabasePath.Trim <> String.Empty) Then
                Return String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", HttpContext.Current.Server.MapPath(mySite.AccessDatabasePath))
            Else
                Return mySite.SQLDBConnString
            End If
        End Get
    End Property

    Private Shared Function LoadSiteList() As wpmSiteList
        If wpmFileProcessing.IsValidPath(wpmApp.Config.SiteListPath) Then
            Using sr As New StreamReader(wpmApp.Config.SiteListPath)
                Try
                    Dim xs As New XmlSerializer(GetType(wpmSiteList))
                    SiteList = DirectCast(xs.Deserialize(sr), wpmSiteList)
                Catch ex As Exception
                    wpmLogging.ErrorLog("Error Loading Site List", ex.ToString)
                End Try
            End Using
        End If
        If SiteList Is Nothing Then
            SiteList = New wpmSiteList(wpmApp.Config.SiteListPath)
        End If
        Return SiteList
    End Function
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
