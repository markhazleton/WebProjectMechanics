
Partial Class KitchenSink
    Inherits System.Web.UI.Page
    Public Property Themes As Dictionary(Of String, String)
        Get
            Return TryCast(Application("siteThemes"), Dictionary(Of String, String))
        End Get
        Set(value As Dictionary(Of String, String))
            Application("siteThemes") = value
        End Set
    End Property
    Public Property ThemeName As String
        Get
            Return Application("ThemeName")
        End Get
        Set(value As String)
            Application("ThemeName") = value
        End Set
    End Property


    Protected Sub ddlThemeChoice_SelectedIndexChanged(sender As Object, e As EventArgs)
        ThemeName = ddlThemeChoice.SelectedValue
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If String.IsNullOrEmpty(ThemeName) Then
            ThemeName = "Yeti"
        End If
        If Not IsPostBack Then
            ddlThemeChoice.DataSource = Themes
            ddlThemeChoice.DataTextField = "key"
            ddlThemeChoice.DataValueField = "key"
            ddlThemeChoice.DataBind()
            ddlThemeChoice.SelectedValue = ThemeName
        End If

    End Sub

End Class
