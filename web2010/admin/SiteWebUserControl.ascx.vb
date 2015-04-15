Imports WebProjectMechanics

Partial Class wpmSiteWebUserControl
    Inherits System.Web.UI.UserControl
    Implements ICompanyUserControl

#Region "Interface Implementation"
    Public Event SaveCompany(ByVal YourCompany As DomainConfiguration) Implements ICompanyUserControl.SaveCompany

    Public Event CancelEdit() Implements ICompanyUserControl.CancelEdit

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        RaiseEvent SaveCompany(yourCompany)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RaiseEvent CancelEdit()
    End Sub

    Public Property yourCompany As DomainConfiguration Implements ICompanyUserControl.yourCompany
        Get
            Dim myCompany As New DomainConfiguration() With {.DomainName = tbDomain.Text, .CompanyID = tbCompanyID.Text, .SQLDBConnString = tbSQLDBConnString.Text, .AccessDatabasePath = tbAccessDatabasePath.Text}
            Return myCompany
        End Get
        Set(ByVal value As DomainConfiguration)
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
