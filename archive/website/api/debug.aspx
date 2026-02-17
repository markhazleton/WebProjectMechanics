<%@ Page Language="VB" %>
<!DOCTYPE html>
<html>
<head>
    <title>API Debug Information</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .info { background: #f0f8ff; padding: 10px; margin: 10px 0; border-radius: 5px; }
        .error { background: #ffe6e6; padding: 10px; margin: 10px 0; border-radius: 5px; }
    </style>
</head>
<body>
    <h1>API Debug Information</h1>
    
    <div class="info">
        <h3>Server Information</h3>
        <p><strong>Server Time:</strong> <%= DateTime.Now.ToString() %></p>
        <p><strong>Framework Version:</strong> <%= Environment.Version.ToString() %></p>
        <p><strong>Server Name:</strong> <%= Environment.MachineName %></p>
        <p><strong>Application Path:</strong> <%= Request.ApplicationPath %></p>
        <p><strong>Physical Path:</strong> <%= Server.MapPath("~") %></p>
    </div>
    
    <div class="info">
        <h3>Request Information</h3>
        <p><strong>Current URL:</strong> <%= Request.Url.ToString() %></p>
        <p><strong>HTTP Method:</strong> <%= Request.HttpMethod %></p>
        <p><strong>User Agent:</strong> <%= Request.UserAgent %></p>
        <p><strong>Is Authenticated:</strong> <%= Request.IsAuthenticated.ToString() %></p>
    </div>
    
    <div class="info">
        <h3>API Files Status</h3>
        <%
        Try
            Dim apiPath As String = Server.MapPath("~/api/")
            Dim files() As String = {"SampleApi.ashx", "LocationApi.ashx", "UserApi.ashx", "CompanyApi.ashx", "index.html"}
            
            For Each fileName As String In files
                Dim fullPath As String = System.IO.Path.Combine(apiPath, fileName)
                If System.IO.File.Exists(fullPath) Then
                    Response.Write("<p><strong>" & fileName & ":</strong> ? Exists</p>")
                Else
                    Response.Write("<p><strong>" & fileName & ":</strong> ? Missing</p>")
                End If
            Next
        Catch ex As Exception
            Response.Write("<div class='error'>Error checking files: " & ex.Message & "</div>")
        End Try
        %>
    </div>
    
    <div class="info">
        <h3>Quick API Tests</h3>
        <p><a href="SampleApi.ashx" target="_blank">Test Sample API</a></p>
        <p><a href="LocationApi.ashx" target="_blank">Test Location API</a></p>
        <p><a href="UserApi.ashx?method=GetCurrentUser" target="_blank">Test User API</a></p>
        <p><a href="CompanyApi.ashx" target="_blank">Test Company API</a></p>
    </div>
    
    <div class="info">
        <h3>Static File Test</h3>
        <p><a href="test.html" target="_blank">Test Simple HTML</a></p>
        <p><a href="index.html" target="_blank">Test API Documentation</a></p>
    </div>
</body>
</html>