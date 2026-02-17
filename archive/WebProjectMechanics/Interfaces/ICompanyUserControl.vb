Public Interface ICompanyUserControl
    Property yourCompany() As DomainConfiguration
    Event SaveCompany(ByVal yourCompany As DomainConfiguration)
    Event CancelEdit()
End Interface
