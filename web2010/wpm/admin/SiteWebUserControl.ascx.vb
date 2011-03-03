Imports WebProjectMechanics

Partial Class wpmSiteWebUserControl
    Inherits System.Web.UI.UserControl
    Implements ISiteUserControl

#Region "Interface Implementation"
    Public Event SaveSite(ByVal YourSite As wpmSite) Implements ISiteUserControl.SaveSite
    Public Event CancelEdit() Implements ISiteUserControl.CancelEdit

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        RaiseEvent SaveSite(yourSite)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RaiseEvent CancelEdit()
    End Sub

    Public Property yourSite() As wpmSite Implements ISiteUserControl.yourSite
        Get
            Dim mySite As New wpmSite
            mySite.DomainName = tbDomain.Text
            mySite.CompanyID = tbCompanyID.Text
            mySite.SQLDBConnString = tbSQLDBConnString.Text
            mySite.AccessDatabasePath = tbAccessDatabasePath.Text
            Return mySite
        End Get
        Set(ByVal value As wpmSite)
            If Not value Is Nothing Then
                tbCompanyID.Text = value.CompanyID
                tbDomain.Text = value.DomainName
                tbSQLDBConnString.Text = value.SQLDBConnString
                tbAccessDatabasePath.Text = value.AccessDatabasePath
            End If
        End Set
    End Property

#End Region

End Class
