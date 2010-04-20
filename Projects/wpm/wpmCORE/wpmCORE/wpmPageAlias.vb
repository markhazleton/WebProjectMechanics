Imports System.Web

Public Class wpmPageAlias
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

Public Class wpmPageAliasList
    Inherits List(Of wpmPageAlias)

    Public Sub New(ByVal CompanyID As String)
        Dim mydt As DataTable = wpmDataCon.GetPageAliasList(CompanyID)
        For Each myrow As DataRow In mydt.Rows
            Dim myPageAlias As New wpmPageAlias()
            myPageAlias.PageAliasID = wpmUTIL.GetDBString(myrow.Item("PageAliasID"))
            myPageAlias.PageURL = wpmUTIL.GetDBString(myrow.Item("PageURL"))
            myPageAlias.TransferURL = wpmUTIL.GetDBString(myrow.Item("TargetURL"))
            myPageAlias.AliasType = wpmUTIL.GetDBString(myrow.Item("AliasType"))
            Me.Add(myPageAlias)
        Next
    End Sub

    Public Function LookupTargetURL(ByVal reqPageURL As String) As String
        Dim LinkURL As String = String.Empty
        reqPageURL = reqPageURL.Replace(":80", String.Empty)
        reqPageURL = reqPageURL.Replace("/wpm/404.aspx?404;", String.Empty)
        reqPageURL = reqPageURL.Replace("/mhweb/404.aspx?404;", String.Empty)
        reqPageURL = reqPageURL.Replace("http://", String.Empty)
        reqPageURL = reqPageURL.Replace("https://", String.Empty)
        reqPageURL = reqPageURL.Replace(HttpContext.Current.Request.Url.Host, String.Empty)
        For Each myPageAlias As wpmPageAlias In Me
            If (wpmUTIL.CheckForMatch(reqPageURL, myPageAlias.PageURL)) Then
                LinkURL = myPageAlias.TransferURL
                Exit For
            End If
        Next
        If LinkURL = String.Empty Then
            For Each myPageAlias As wpmPageAlias In Me
                If myPageAlias.PageURL = "*" Then
                    LinkURL = myPageAlias.TransferURL
                    Exit For
                End If
            Next
        End If
        Return LinkURL
    End Function

End Class
