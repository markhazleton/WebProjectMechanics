<script language="VB" runat="Server">
Dim I As String
Dim key As Object
Dim item As Object
Dim name As Object
Dim myStringBuilder As StringBuilder = New StringBuilder()

</script>
<%
If Not IsNothing(request.QueryString.GetValues("error")) Then
  if (request.QueryString.Item("error")="NOCONFIG") then
    myStringBuilder.Append("<h1>Configuration File Not Found</h1><hr/>")
  end if
  if (request.QueryString.Item("error")="NODATABASE") then
    myStringBuilder.Append("<h1>Database File Not Found</h1><hr/>")
  end if
End If

myStringBuilder.Append(("<html><body><table><tr><td>Application</td><td>"))
myStringBuilder.Append("<table border=1>")
For Each item In Application.Contents
	  myStringBuilder.Append("<tr><td>")
	  myStringBuilder.Append(item)
	  myStringBuilder.Append("</td><td>&nbsp;")
	  myStringBuilder.Append(Application.Contents.Item(item))
	  myStringBuilder.Append("</td></tr>")
Next item
myStringBuilder.Append("</table>")

myStringBuilder.Append(("</td></tr>"))

myStringBuilder.Append(("<tr><td>Session</td><td>"))
myStringBuilder.Append("<table border=1>")
For	Each item In Session.Contents
	  myStringBuilder.Append("<tr><td>")
	  myStringBuilder.Append(item)
	  myStringBuilder.Append("</td><td>&nbsp;")
	  myStringBuilder.Append(Session.Contents.Item(item))
	  myStringBuilder.Append("</td></tr>")

Next item
myStringBuilder.Append("</table>")
myStringBuilder.Append(("</td></tr>"))


myStringBuilder.Append(("<tr><td>ServerVariables</td><td>"))

If Not IsNothing(Request.ServerVariables) Then
  myStringBuilder.Append("<table border=1>")
	For	Each name In Request.ServerVariables
	  myStringBuilder.Append("<tr><td>")
	  myStringBuilder.Append(name)
	  myStringBuilder.Append("</td><td>&nbsp;")
	  myStringBuilder.Append(Request.ServerVariables(name))
	  myStringBuilder.Append("</td></tr>")
	Next name
  myStringBuilder.Append("</table>")
End If

myStringBuilder.Append(("</td></tr>"))

myStringBuilder.Append(("<tr><td>ClientCertificate</td><td>"))
myStringBuilder.Append("<table border=1>")
For	Each name In Request.ClientCertificate
  myStringBuilder.Append("<tr><td>")
  myStringBuilder.Append(name)
  myStringBuilder.Append("</td><td>")
  myStringBuilder.Append(Request.ClientCertificate(name))
  myStringBuilder.Append("</td></tr>")
Next name
myStringBuilder.Append("</table></td></tr>")

myStringBuilder.Append(("<tr><td>Cookies</td><td>"))
Dim MyCookieNamesArray() As String = Request.Cookies.AllKeys
myStringBuilder.Append("<table border=1>")
For Each name in MyCookieNamesArray
  myStringBuilder.Append("<tr><td>")
  myStringBuilder.Append(name)
  myStringBuilder.Append("</td><td>")
  myStringBuilder.Append(Request.Cookies(name).Value)
  myStringBuilder.Append("</td></tr>")
next name
myStringBuilder.Append("</table>")

myStringBuilder.Append(("</td></tr></table></body></html>"))
response.write(myStringBuilder.tostring)
%>

