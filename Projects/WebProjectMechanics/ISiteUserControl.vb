Public Interface ISiteUserControl
    Property yourSite() As wpmSite
    Event SaveSite(ByVal yourSite As wpmSite)
    Event CancelEdit()

End Interface
