Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions

Public Class LocationAliasList
    Inherits List(Of LocationAlias)

    Public Sub New(ByVal CompanyID As String)
        GetCompanyLocationAliases(CompanyID)
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub GetCompanyLocationAliases(ByVal CompanyID As String)
        Using mydt As DataTable = ApplicationDAL.GetPageAliasList(CompanyID)
            For Each myrow As DataRow In mydt.Rows
                Dim myPageAlias As New LocationAlias() With {.PageAliasID = wpm_GetDBString(myrow.Item("PageAliasID")),
                                                          .PageURL = wpm_GetDBString(myrow.Item("PageURL")),
                                                          .TargetURL = wpm_GetDBString(myrow.Item("TargetURL")),
                                                          .AliasType = wpm_GetDBString(myrow.Item("AliasType"))}
                Add(myPageAlias)
            Next
        End Using
    End Sub
    Public Function LookupTargetURL(ByVal reqPageURL As String) As String
        Dim LinkURL As String = String.Empty
        Dim mySearch As String = String.Empty
        For Each myPageAlias As LocationAlias In Me

            Select Case myPageAlias.PageURL
                Case "*"
                    mySearch = ".{0,}"
                Case "+"
                    mySearch = ".{1,}"
                Case "?"
                    mySearch = ".{0,1}"
                Case Else
                    mySearch = String.Empty
            End Select

            If mySearch <> String.Empty Then
                Dim rToL As New Regex(mySearch, RegexOptions.Singleline Or RegexOptions.RightToLeft)
                If rToL.IsMatch(reqPageURL) Then
                    LinkURL = myPageAlias.TargetURL
                    Exit For
                End If
            End If

            If (wpm_CheckForMatch(reqPageURL, myPageAlias.PageURL)) Then
                LinkURL = myPageAlias.TargetURL
                Exit For
            End If
        Next
        Return LinkURL
    End Function

    Private ReadOnly report As New StringBuilder()
    Private webPage As String
    Private countOfMatches As Int32
    Private ReadOnly ResultLabel As New StringBuilder()

    Private Sub scrapeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        webPage = GrabUrl()
        Dim myDelegate As New MatchEvaluator(AddressOf MatchHandler)

        Dim linksExpression As New Regex( _
  "\<a				(?# Find the opening ANCHOR tag )" & _
  ".+?				(?# followed, minimally by everything up to the href attribute ) " & _
  "href=['""]			(?# up to the opening Href attribute ) " & _
  "(?!http\:\/\/)			(?# assert that the next sequence is not Http://) " & _
  "(?!mailto\:)			(?# ...or mailto:) " & _
  "(?<foundAnchor>[^'"">]+?)	(?# now, match everything up to the next ' or "" into a group named 'foundAnchor') " & _
  "[^>]*?				(?# followed, minimally by everything up to the closing tag ) " & _
  "\>				(?# then the end of the opening ANCHOR tag)", _
  RegexOptions.Multiline Or _
  RegexOptions.IgnoreCase Or _
  RegexOptions.IgnorePatternWhitespace _
 )

        Dim newWebPage As String = linksExpression.Replace(webPage, myDelegate)

        ResultLabel.Append(String.Format("<h2>Report Result for </h2><b>Found and fixed the following {0} anchors...</b><br><br>{1}", countOfMatches, report.ToString().Replace(Environment.NewLine, "<br>")))
        ResultLabel.Append("<h2>Fixed Page</h2>" & HttpContext.Current.Server.HtmlEncode(newWebPage))

    End Sub
    Private Function MatchHandler(ByVal m As Match) As String
        Dim link As String = m.Groups("foundAnchor").Value
        Dim rToL As New Regex("^", RegexOptions.Multiline Or RegexOptions.RightToLeft)
        Dim col, row As Int32
        Dim lineBegin As Int32 = rToL.Match(webPage, m.Index).Index

        row = rToL.Matches(webPage, m.Index).Count
        col = m.Index - lineBegin

        report.AppendFormat( _
            "Link <b>{0}</b>, fixed at row: {1}, col: {2}{3}", _
            HttpContext.Current.Server.HtmlEncode(m.Groups(0).Value), _
            row, _
            col, _
            Environment.NewLine _
        )
        Dim newLink As String
        If link.StartsWith("/") Then
            newLink = link.Substring(1)
        Else
            newLink = link
        End If
        countOfMatches += 1
        Return m.Groups(0).Value.Replace(link, "http://projectmechanics.com/" & newLink)
    End Function
    Private Shared Function GrabUrl() As String
        Dim wc As New WebClient()
        'TO DO:  Implement url validity check on Url value
        Dim s As Stream = wc.OpenRead("http://projectmechanics.com")
        Dim sr As StreamReader = New StreamReader(s)
        GrabUrl = sr.ReadToEnd
        s.Close()
        wc.Dispose()
    End Function


End Class
