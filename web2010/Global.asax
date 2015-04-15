﻿<%@ Application Language="VB" %>
<%@ Import Namespace="WebProjectMechanics" %>

<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs

        ' Get the exception object.
        Dim exc As Exception = Server.GetLastError

        ' Handle HTTP errors (avoid trapping HttpUnhandledException
        ' which is generated when a non-HTTP exception 
        ' such as the ones generated by buttons 1-3 in 
        ' Default.aspx is not handled at the page level).
        If (exc.GetType Is GetType(HttpException)) Then
            ' The Complete Error Handling Example generates
            ' some errors using URLs with "NoCatch" in them;
            ' ignore these here to simulate what would happen
            ' if a global.asax handler were not implemented.
            If exc.Message.Contains("NoCatch") Or exc.Message.Contains("maxUrlLength") Then
                Return
            End If

            'Redirect HTTP errors to HttpError page
            Server.Transfer("~/HttpErrorPage.aspx")
        End If

        ' For other kinds of errors give the user some information
        ' but stay on the default page
        Response.Write("<h2>Global Page Error</h2>" & vbLf)
        Response.Write("<p>" & exc.Message + "</p>" & vbLf)
        Response.Write(("Return to the <a href='Default.aspx'>" _
          & "Default Page</a>" & vbLf))

        ' Log the exception 
        ExceptionUtility.LogException(exc, "DefaultPage")

        ' Clear the error from the server
        Server.ClearError()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
</script>
