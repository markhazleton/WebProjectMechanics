Imports WebProjectMechanics
Imports System

Partial Class wpm_login_login
    Inherits ApplicationPage

    Public Readonly Property ReturnPageURL As String 
        Get
            Return Session("wpm_LoginRedirectURL")
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetPageName("Login")
        
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        myStringBuilder.Append(Session("msg"))
        Session("msg") = ""
        If IsPostBack Then
            If wpm_ProcessUserLogin(wpm_GetProperty("Username", ""), wpm_GetProperty("Password", "")) Then
                If (ReturnPageURL = "" Or ReturnPageURL = "/admin/login/login.aspx") Then
                    Response.Redirect("/")
                Else
                    Response.Redirect(ReturnPageURL)
                End If
            Else
                'We are NOT logged In
                myStringBuilder.Append(String.Format("<script language=""JavaScript"" type=""text/JavaScript"">{0}", vbCrLf))
                myStringBuilder.Append("<!--" & vbCrLf)
                myStringBuilder.Append("function MM_openBrWindow(theURL,winName,features) { window.open(theURL,winName,features); }" & vbCrLf)
                myStringBuilder.Append("//-->" & vbCrLf)
                myStringBuilder.Append(String.Format("</script>{0}", vbCrLf))
                myStringBuilder.Append("<h1>Login Failed</h1><h2>The combination of email address and password were not found.</h2>")
                ' **************************************************************************
                ' Update the Session User Object
                ' **************************************************************************
                wpm_SetContactID(String.Empty)
                wpm_SetUserName(String.Empty)
                wpm_SetUserGroup("4")
            End If
        End If
        myMessage.Text = myStringBuilder.ToString
        wpm_CurrentPageID = "LoginPage"
    End Sub
End Class
