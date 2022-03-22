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
    myStringBuilder.Append(("<html><body><table style='width:100%;'><tr><td style=""background-color:green;"">Application</td></tr></table>"))
    myStringBuilder.Append("<table border=1>")
    For Each appContent As System.String In Application.Contents
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(appContent)
        myStringBuilder.Append("</td><td>&nbsp;")
        myStringBuilder.Append(Application.Contents.Item(appContent.ToString))
        myStringBuilder.Append("</td></tr>")
    Next
    myStringBuilder.Append("</table>")
    myStringBuilder.Append(("<table style='width:100%;'>"))
    myStringBuilder.Append(("<tr><td style=""background-color:green;"">Session</td></tr></table>"))
    myStringBuilder.Append("<table border=1>")
    For Each sessionContent As Object In Session.Contents
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(sessionContent)
        myStringBuilder.Append("</td><td>&nbsp;")
        myStringBuilder.Append(Session.Contents.Item(sessionContent))
        myStringBuilder.Append("</td></tr>")
    Next
    myStringBuilder.Append("</table><br/>")
    myStringBuilder.Append(("<table style='width:100%;background-color:green;'>"))
    myStringBuilder.Append(("<tr><td>ServerVariables</td></tr></table>"))
    If Not IsNothing(Request.ServerVariables) Then
        myStringBuilder.Append("<table border=1>")
        For Each ServerVariable As Object In Request.ServerVariables
            myStringBuilder.Append("<tr><td>")
            myStringBuilder.Append(ServerVariable)
            myStringBuilder.Append("</td><td>&nbsp;")
            myStringBuilder.Append(Request.ServerVariables(ServerVariable))
            myStringBuilder.Append("</td></tr>")
        Next
        myStringBuilder.Append("</table><br/>")
    End If
    myStringBuilder.Append(("<table style='width:100%;background-color:green;'>"))
    myStringBuilder.Append(("<tr><td>ClientCertificate</td></tr></table>"))
    myStringBuilder.Append("<table border=1>")
    For Each ClientCert As Object In Request.ClientCertificate
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(ClientCert)
        myStringBuilder.Append("</td><td>")
        myStringBuilder.Append(Request.ClientCertificate(ClientCert))
        myStringBuilder.Append("</td></tr>")
    Next
    myStringBuilder.Append("</table><br/>")
    myStringBuilder.Append(("<table style='width:100%;background-color:green;'>"))

    myStringBuilder.Append(("<tr><td>Cookies</td></tr></table>"))
    Dim MyCookieNamesArray() As String = Request.Cookies.AllKeys
    myStringBuilder.Append("<table border=1>")
    For Each CookieName As Object In MyCookieNamesArray
        myStringBuilder.Append("<tr><td>")
        myStringBuilder.Append(CookieName)
        myStringBuilder.Append("</td><td>")
        myStringBuilder.Append(Request.Cookies(CookieName).Value)
        myStringBuilder.Append("</td></tr>")
    Next 
    myStringBuilder.Append("</table><br/>")
    myStringBuilder.Append(("</body></html>"))
    Response.Write(myStringBuilder.ToString)
%>

