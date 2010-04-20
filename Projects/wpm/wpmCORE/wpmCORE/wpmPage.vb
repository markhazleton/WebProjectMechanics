Public Class wpmPage
    Inherits System.Web.UI.Page

    '    Public mySession As wpmSession
    Private bInPortal As Boolean = False
    Public pageActiveSite As wpmActiveSite

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        pageActiveSite = New wpmActiveSite(Session)
        MyBase.OnPreInit(e)
    End Sub

    Public ReadOnly Property InPortal() As Boolean
        Get
            Return bInPortal
        End Get
    End Property

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        Try
            bInPortal = False
        Catch
            bInPortal = False
        End Try
        MyBase.OnLoad(e)
    End Sub

    Protected Function GetPortalUserSetting(ByVal PropertyNM As String) As String
        Dim sReturn As String = ("")
        Return sReturn
    End Function

    Protected Function SetPortalUserSetting(ByVal PropertyNM As String, ByVal ValueNM As String) As Boolean
        Return False
    End Function

    Protected Function GetPortalUserNM() As String
        Return ""
    End Function

    Protected Function GetPortalLogin() As String
        Return ""
    End Function
    Protected Function GetPortalUserInfo() As String
        Dim sReturn As String = ""
        Return sReturn
    End Function
    Protected Function GetSessionProperty(ByVal myProperty As String) As String
        Dim myVal As String = ""
        If Len(Session(myProperty)) > 0 Then
            myVal = Session(myProperty).ToString
        End If
        Return myVal
    End Function
    Protected Function ClearSessionProperty(ByVal myProperty As String) As Boolean
        Session(myProperty) = ""
    End Function
    Protected Function SetSessionProperty(ByVal myProperty As String, ByVal myValue As String) As Boolean
        If Trim(myValue) <> "" Then
            Session(myProperty) = myValue
        End If
        Return True
    End Function
    Protected Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        Else
            myValue = curValue
        End If
        Return myValue
    End Function

End Class