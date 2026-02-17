Imports System.Web.UI.WebControls

Public Class ApplicationPage
    Inherits Page
    Public ReadOnly Property curCompany As ActiveCompany
        Get
            Return masterPage.myCompany
        End Get
    End Property
    Public masterPage As ApplicationMasterPage
    Public Shared StartTimer As Long
    Public Shared Property StartTimerProperty As Long
        Get
            Return StartTimer
        End Get
        Set(value As Long)
            If StartTimer = value Then
                Return
            End If
            StartTimer = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        MyBase.OnPreInit(e)
        UpdateDataSourceConnection()
        masterPage = DirectCast(Me.Page.Master, ApplicationMasterPage)
    End Sub
    Public Sub CheckAdmin()
        If Not (wpm_IsAdmin) Then
            wpm_LoginRedirectURL = Request.Url.AbsoluteUri
            Response.Redirect(String.Format("{0}login/login.aspx", wpm_SiteConfig.AdminFolder))
        End If
    End Sub

    Private Function UpdateDataSourceConnection() As Boolean
        For Each masterControl As Control In Page.Controls
            If TypeOf masterControl Is MasterPage Then
                For Each formControl As Control In masterControl.Controls
                    If TypeOf formControl Is System.Web.UI.HtmlControls.HtmlForm Then
                        For Each contentControl As Control In formControl.Controls
                            If TypeOf contentControl Is ContentPlaceHolder Then
                                For Each childControl As Control In contentControl.Controls
                                    If TypeOf childControl Is AccessDataSource Then
                                        If Not (wpm_AccessDatabasePath Is Nothing) Then
                                            TryCast(childControl, AccessDataSource).DataFile = wpm_AccessDatabasePath
                                        End If
                                    ElseIf TypeOf childControl Is SqlDataSource Then
                                        TryCast(childControl, SqlDataSource).ConnectionString = wpm_SQLDBConnString
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            End If
        Next
        Return True
    End Function
    Public Function GetPageHistory() As String
        Dim mysb As New System.Text.StringBuilder("<hr/><h2>Session History</h2><br/><table border=""1"">")
        mysb.Append("<thead><tr><td>Timestamp</td><td>Page Name</td><td>Previous Page</td></tr></thead>")
        For Each ph As LocationHistory In GetSessionPageHistory()
            mysb.Append(String.Format("<tr><td>{0}</td><td><a href=""{1}"">{2}</a></td><td>{3}</td></tr>", ph.ViewTime, ph.RequestURL, ph.PageName, ph.PageSource))
        Next
        mysb.Append("</table></br><hr/>")
        Return mysb.ToString
    End Function
    Public Function GetSessionPageHistory() As LocationHistoryList
        Dim myPageHistory As LocationHistoryList
        Try
            myPageHistory = CType(HttpContext.Current.Session("PageHistory"), LocationHistoryList)
            If myPageHistory Is Nothing Then
                myPageHistory = New LocationHistoryList()
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Error when reading Session variable (PageHisotry) - " & ex.ToString, "GetSessionPageHistory")
            myPageHistory = New LocationHistoryList()
        End Try
        Return myPageHistory
    End Function
    Public Function FormatTableCell(ByVal cell_value As String) As String
        Return String.Format("<td>{0}</td>", cell_value)
    End Function
    Public Function FormatVariableLine(ByVal var_name As String, ByVal var_value As String) As String
        Dim tmpStr As New String(CType(String.Empty, Char()))
        If var_name <> "Submit" Then
            tmpStr = String.Format("{0}<b>{1}</b>:<br/>{2}", tmpStr, var_name.ToUpper(), vbCrLf)
            tmpStr = tmpStr & var_value & vbCrLf
            tmpStr = String.Format("{0}<br/>{1}", tmpStr, vbCrLf)
        End If
        Return tmpStr
    End Function
    Public Function GetFormFilePath(ByVal HTMLFileName As String) As String
        If Not FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "form") Then
            FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "form")
        End If
        If Not FileProcessing.VerifyFolderExists(String.Format("{0}form\{1}", wpm_SiteConfig.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty))) Then
            FileProcessing.CreateFolder(String.Format("{0}form\{1}", wpm_SiteConfig.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty)))
        End If
        Return String.Format("{0}form\{1}\{2}", wpm_SiteConfig.ConfigFolderPath, Replace(HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME"), "www.", String.Empty), HTMLFileName)
    End Function
    Public Function SaveMailToFile(ByRef pageActiveSite As ActiveCompany, ByRef sOutFile As String) As Boolean
        Dim filename As String = GetFormFilePath(Replace(Replace(Replace(String.Format("{0}-{1}", wpm_HostName, Format(System.DateTime.Now(), "yyyy:MM:dd:HH:mm:ss")), " ", "-"), ",", String.Empty), ":", "-") & ".html")
        Return FileProcessing.CreateFile(filename, String.Format("{0}<br/><br/><hr/>Sent to:{1}<br/>", sOutFile, pageActiveSite.FromEmail))
    End Function
    Public Sub SetupDropdown(ByRef LookupItemList As List(Of LookupItem), ByRef cmb As DropDownList)
        cmb.AppendDataBoundItems = True
        cmb.DataSource = LookupItemList
        cmb.DataTextField = "Name"
        cmb.DataValueField = "Value"
        cmb.DataBind()
        cmb.SelectedIndex = -1
    End Sub
    Public Function GetClass(ByVal TypeName As String) As String
        If wpm_CheckForMatch(TypeName, wpm_GetProperty("Type", String.Empty)) Then
            Return "active"
        Else
            Return String.Empty
        End If
    End Function

    Public Sub SetPageName(ByVal PageName As String)
        With masterPage.myCompany.CurLocation
            .LocationTitle = PageName
            .LocationName = PageName
            .LocationSummary = PageName
            .LocationID = String.Empty
            .HideGlobalContent = True
            .MainMenuLocationName = PageName
            .RecordSource = "Page"
            .SiteCategoryName = PageName
        End With
    End Sub
End Class

