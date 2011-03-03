Imports WebProjectMechanics

Partial Class wpm_login_login
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        myStringBuilder.Append(Session("msg"))
        Session("msg") = ""
        If IsPostBack Then
            If wpmUser.ProcessLogin(GetProperty("Username", ""), GetProperty("Password", ""), pageActiveSite.Session) Then
                'We are logged In
                If wpmUser.IsAdmin Then
                End If

                If (wpmSession.GetLogin_Link() = "") Then
                    Response.Redirect("/")
                Else
                    Response.Redirect(wpmSession.GetLogin_Link())
                End If
            Else
                'We are NOT logged In
                myStringBuilder.Append("<sc" & "ript language=""JavaScript"" type=""text/JavaScript"">" & vbCrLf)
                myStringBuilder.Append("<!--" & vbCrLf)
                myStringBuilder.Append("function MM_openBrWindow(theURL,winName,features) { window.open(theURL,winName,features); }" & vbCrLf)
                myStringBuilder.Append("//-->" & vbCrLf)
                myStringBuilder.Append("</scr" & "ipt>" & vbCrLf)
                myStringBuilder.Append("<h1>Login Failed</h1><h2>The combination of email address and password were not found.</h2>")
                ' **************************************************************************
                ' Update the Session User Object
                ' **************************************************************************
                wpmSession.SetContactID("")
                wpmSession.SetUserName("")
                wpmSession.SetUserGroup("4")
            End If
        End If
        myMessage.Text = myStringBuilder.ToString
        Session("CurrentPageID") = "LoginPage"
    End Sub
End Class
