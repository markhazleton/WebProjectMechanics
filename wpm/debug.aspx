<script language="VB" runat="Server">
Dim I As String
Dim key As Object
Dim item As Object
Dim name As Object
Dim myStringBuilder As StringBuilder = New StringBuilder()

</script>
<% 
    If Not IsNothing(Request.QueryString.GetValues("error")) Then
        If (Request.QueryString.Item("error") = "NOCONFIG") Then
            myStringBuilder.Append("<h1>Configuration File Not Found</h1><hr/>")
        End If
        If (Request.QueryString.Item("error") = "NODATABASE") Then
            myStringBuilder.Append("<h1>Database File Not Found</h1><hr/>")
        End If
    End If
    myStringBuilder.Append(("<html><body><table><tr><td style=""background-color:green;"">Application</td></tr><tr><td>"))
    myStringBuilder.Append("<table border=1>")
    For Each appContent As System.String In Application.Contents
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(appContent)
        myStringBuilder.Append("</td><td>&nbsp;")
        myStringBuilder.Append(Application.Contents.Item(appContent.ToString))
        myStringBuilder.Append("</td></tr>")
    Next appContent
    myStringBuilder.Append("</table>")
    myStringBuilder.Append(("</td></tr>"))
    myStringBuilder.Append(("<tr><td style=""background-color:green;"">Session</td></tr><tr><td>"))
    myStringBuilder.Append("<table border=1>")
    For Each sessionContent As Object In Session.Contents
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(sessionContent)
        myStringBuilder.Append("</td><td>&nbsp;")
        myStringBuilder.Append(Session.Contents.Item(sessionContent))
        myStringBuilder.Append("</td></tr>")
    Next sessionContent
    myStringBuilder.Append("</table>")
    myStringBuilder.Append(("</td></tr>"))
    myStringBuilder.Append(("<tr><td style=""background-color:green;"">ServerVariables</td></tr><tr><td>"))
    If Not IsNothing(Request.ServerVariables) Then
        myStringBuilder.Append("<table border=1>")
        For Each ServerVariable As Object In Request.ServerVariables
            myStringBuilder.Append("<tr><td>")
            myStringBuilder.Append(ServerVariable)
            myStringBuilder.Append("</td><td>&nbsp;")
            myStringBuilder.Append(Request.ServerVariables(ServerVariable))
            myStringBuilder.Append("</td></tr>")
        Next ServerVariable
        myStringBuilder.Append("</table>")
    End If
    myStringBuilder.Append(("</td></tr>"))
    myStringBuilder.Append(("<tr><td>ClientCertificate</td><td>"))
    myStringBuilder.Append("<table border=1>")
    For Each ClientCert As Object In Request.ClientCertificate
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(ClientCert)
        myStringBuilder.Append("</td><td>")
        myStringBuilder.Append(Request.ClientCertificate(ClientCert))
        myStringBuilder.Append("</td></tr>")
    Next ClientCert
    myStringBuilder.Append("</table></td></tr>")
    myStringBuilder.Append(("<tr><td>Cookies</td><td>"))
    Dim MyCookieNamesArray() As String = Request.Cookies.AllKeys
    myStringBuilder.Append("<table border=1>")
    For Each CookieName As Object In MyCookieNamesArray
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(CookieName)
        myStringBuilder.Append("</td><td>")
        myStringBuilder.Append(Request.Cookies(CookieName).Value)
        myStringBuilder.Append("</td></tr>")
    Next CookieName
    myStringBuilder.Append("</table>")
    myStringBuilder.Append(("</td></tr></table></body></html>"))
    Response.Write(myStringBuilder.ToString)
%>

