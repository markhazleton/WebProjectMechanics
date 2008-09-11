Public Class mhPageAlias
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
            myPageAlias.PageURL = mhUTIL.GetDBString(myrow.Item("PageURL"))
            myPageAlias.TransferURL = mhUTIL.GetDBString(myrow.Item("TargetURL"))
            myPageAlias.AliasType = mhUTIL.GetDBString(myrow.Item("AliasType"))
            Me.Add(myPageAlias)
        Next
    End Sub

End Class
