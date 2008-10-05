Public Class mhPageAlias
    Private _PageAliasID As String
    Public Property PageAliasID() As String
        Get
            Return _PageAliasID
        End Get
        Set(ByVal value As String)
            _PageAliasID = value
        End Set
    End Property
    Private _PageURL As String
    Public Property PageURL() As String
        Get
            Return _PageURL
        End Get
        Set(ByVal value As String)
            _PageURL = value
        End Set
    End Property
    Private _TransferURL As String
    Public Property TransferURL() As String
        Get
            Return _TransferURL
        End Get
        Set(ByVal value As String)
            _TransferURL = value
        End Set
    End Property
    Private _AliasType As String
    Public Property AliasType() As String
        Get
            Return _AliasType
        End Get
        Set(ByVal value As String)
            _AliasType = value
        End Set
    End Property

End Class

Public Class mhPageAliasRows
    Inherits List(Of mhPageAlias)

    Public Sub New(ByVal CompanyID As String)
        Dim mydt As DataTable = mhDataCon.GetPageAliasList(CompanyID)
        For Each myrow As DataRow In mydt.Rows
            Dim myPageAlias As New mhPageAlias()
            myPageAlias.PageAliasID = mhUTIL.GetDBString(myrow.Item("PageAliasID"))
            myPageAlias.PageURL = mhUTIL.GetDBString(myrow.Item("PageURL"))
            myPageAlias.TransferURL = mhUTIL.GetDBString(myrow.Item("TargetURL"))
            myPageAlias.AliasType = mhUTIL.GetDBString(myrow.Item("AliasType"))
            Me.Add(myPageAlias)
        Next
    End Sub

    Public Function LookupTargetURL(ByVal reqPageURL As String) As String
        Dim LinkURL As String = String.Empty
        reqPageURL = Replace(reqPageURL, ":80", "")
        reqPageURL = Replace(reqPageURL, "/mhweb/404.aspx?404;", "")
        reqPageURL = Replace(reqPageURL, "http://", "")
        reqPageURL = Replace(reqPageURL, "https://", "")
        reqPageURL = Replace(reqPageURL, HttpContext.Current.Request.Url.Host, "")
        For Each myPageAlias As mhPageAlias In Me
            If (mhUTIL.CheckForMatch(reqPageURL, myPageAlias.PageURL)) Then
                LinkURL = myPageAlias.TransferURL
                Exit For
            End If
        Next
        Return LinkURL
    End Function

End Class
