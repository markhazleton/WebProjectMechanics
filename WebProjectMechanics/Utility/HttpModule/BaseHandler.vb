
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web.SessionState

Partial Public MustInherit Class BaseHandler
    Implements IHttpHandler
    Implements IRequiresSessionState

    Private _context As HttpContext = Nothing
    Public Property context() As HttpContext
        Get
            Return _context
        End Get
        Private Set(value As HttpContext)
            _context = Value
        End Set
    End Property

    Public Overridable Function [GET]() As Object
        Return "Default GET Response"
    End Function
    Public Overridable Function POST() As Object
        Return "Default POST Response"
    End Function
    Public Overridable Function PUT() As Object
        Return "Default PUT Response"
    End Function
    Public Overridable Function DELETE() As Object
        Return "Default DELETE Response"
    End Function

    ''' <summary>
    ''' Intercept the execution right before the handler method is called
    ''' </summary>
    ''' <param name="e"></param>
    Public Overridable Sub OnMethodInvoke(e As OnMethodInvokeArgs)
    End Sub

    ''' <summary>
    ''' Intercept the execution right after the handler method is called
    ''' </summary>
    Public Overridable Sub AfterMethodInvoke(result As Object)
    End Sub

    ''' <summary>
    ''' Method used to handle the request as a normal ASHX.
    ''' To use this method just pass handlerequest=true on the request query string.
    ''' </summary>
    Public Overridable Sub HandleRequest()
    End Sub

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Me.context = context
        ' it's possible to the requestor to be able to handle everything himself, overriding all this implemention
        Dim handleRequest__1 As String = context.Request("handlerequest")
        If Not String.IsNullOrEmpty(handleRequest__1) AndAlso handleRequest__1.ToLower() = "true" Then
            HandleRequest()
            Return
        End If
        Dim ajaxCall = New AjaxCallSignature(context)

        'context.Response.ContentType = string.Empty;
        If Not String.IsNullOrEmpty(ajaxCall.returnType) Then
            Select Case ajaxCall.returnType
                Case "json"
                    context.Response.ContentType = "application/json"
                    Exit Select
                Case "xml"
                    context.Response.ContentType = "application/xml"
                    Exit Select
                Case "jpg", "jpeg", "image/jpg"
                    context.Response.ContentType = "image/jpg"
                    Exit Select
                Case Else
                    Exit Select
            End Select
        End If

        ' call the requested method
        Dim result As Object = ajaxCall.Invoke(Me, context)

        ' if neither on the arguments or the actual method the content type was set then make sure to use the default content type
        If String.IsNullOrEmpty(context.Response.ContentType) AndAlso Not SkipContentTypeEvaluation Then
            context.Response.ContentType = DefaultContentType()
        End If

        ' only skip transformations if the requestor explicitly said so
        If result Is Nothing Then
            context.Response.Write(String.Empty)
        ElseIf Not SkipDefaultSerialization Then
            Select Case context.Response.ContentType.ToLower()
                Case "application/json"
                    Dim jsonSerializer As New JavaScriptSerializer()
                    Dim json As String = jsonSerializer.Serialize(result)
                    context.Response.Write(json)
                    Exit Select
                Case "application/xml"
                    Dim xmlSerializer As New System.Xml.Serialization.XmlSerializer(result.[GetType]())
                    Dim xmlSb As New StringBuilder()
                    Dim xmlWriter As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(xmlSb)
                    xmlSerializer.Serialize(xmlWriter, result)
                    context.Response.Write(xmlSb.ToString())
                    Exit Select
                Case "text/html"
                    context.Response.Write(result)
                    Exit Select
                Case Else
                    Throw New Exception(String.Format("Unsuported content type [{0}]", context.Response.ContentType))
            End Select
        Else
            context.Response.Write(result)
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Returns the default content type returned by the handler.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function DefaultContentType() As String
        Return "application/json"
    End Function

    Public Sub SetResponseContentType(value As String)
        context.Response.ContentType = value
    End Sub

    ''' <summary>
    ''' Setting this to false will make the handler to respond with exacly what the called method returned.
    ''' If true the handler will try to serialize the content based on the ContentType set.
    ''' </summary>
    Public Property SkipDefaultSerialization() As Boolean
        Get
            Return m_SkipDefaultSerialization
        End Get
        Set(value As Boolean)
            m_SkipDefaultSerialization = Value
        End Set
    End Property
    Private m_SkipDefaultSerialization As Boolean

    ''' <summary>
    ''' Setting this to true will avoid the handler to change the content type wither to its default value or to its specified value on the request.
    ''' This is useful if you're handling the request yourself and need to specify it yourself.
    ''' </summary>
    Public Property SkipContentTypeEvaluation() As Boolean
        Get
            Return m_SkipContentTypeEvaluation
        End Get
        Set(value As Boolean)
            m_SkipContentTypeEvaluation = Value
        End Set
    End Property
    Private m_SkipContentTypeEvaluation As Boolean

    ''' <summary>
    ''' Prints an help page discribing the available methods on this handler.
    ''' </summary>
    ''' <returns></returns>
    Public Function Help() As String
        context.Response.ContentType = "text/html"

        Dim sb As New StringBuilder()

        sb.AppendLine("<style>")
        sb.AppendLine(".MainHeader { background-color: FFFFE0; border: 1px dashed red; padding: 0 10 0 10; }")
        sb.AppendLine("h3 { background-color: #DCDCDC; }")
        sb.AppendLine("ul { background-color: #FFFFFF; }")
        sb.AppendLine(".type { color: gray; }")
        sb.AppendLine("</style>")

        sb.AppendLine("<div class='MainHeader'><h2>Handler available methods</h2></div>")

        Dim methods As MethodInfo() = Me.[GetType]().GetMethods()
        ' All methods found on this type
        Dim excludeMethods As MethodInfo() = Me.[GetType]().BaseType.GetMethods()
        ' methods from the base class are not to be shown
        For Each m As MethodInfo In methods
            ' do nothing if the current method belongs to the base type.
            ' I'm not supporting overrides here, I'm only searching by name, if more than one method with the same name exist they all will be ignored.
            If excludeMethods.FirstOrDefault(Function(c) c.Name = m.Name) IsNot Nothing Then
                Continue For
            End If

            ' get description (search for System.ComponentModel.DescriptionAtrribute)
            Dim methodDescription As [String] = "<i>No description available</i>"
            For Each attr As object In m.GetCustomAttributes(True)
                If TypeOf attr Is DescriptionAttribute Then
                    methodDescription = DirectCast(attr, DescriptionAttribute).Description
                End If
            Next

            ' get method arguments
            Dim parameters As ParameterInfo() = m.GetParameters()

            Dim RequiresAuthentication As Boolean = False
            Dim attrs As Object() = m.GetCustomAttributes(GetType(RequireAuthenticationAttribute), True)
            If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
                RequiresAuthentication = DirectCast(attrs(0), RequireAuthenticationAttribute).RequireAuthentication
            End If

            sb.AppendLine(String.Format("<h3>{0}{1}</h3>", m.Name, (If(RequiresAuthentication, " <span style=""color:#f00"">[Requires Authentication]</span>", String.Empty))))
            sb.AppendLine(String.Format("<b>Description: </b><i>{0}</i>", methodDescription))

            sb.AppendLine("<table><tr><td width=""250px"">")
            sb.AppendLine("<table width=""100%"">")
            For Each p As ParameterInfo In parameters
                sb.AppendLine(String.Format("<tr><td>{0}</td><td><span class='type'>[{1}]</span></td></tr>", p.Name, p.ParameterType))
            Next

            sb.AppendLine("</table>")

            sb.AppendLine("</td><td style=""border-left: 1px dashed #DCDCDC; padding-left: 8px;"">")

            Dim getJSONSample As String = String.Format("<pre>$.getJSON({0}{1}'{2}', {0}{1}{{method: ""{3}"", returntype: ""json"", args: {{", vbLf, vbTab, context.Request.Url.LocalPath, m.Name)
            For Each p As ParameterInfo In m.GetParameters()
                getJSONSample += String.Format(" {0}: """",", p.Name)
            Next
            getJSONSample = getJSONSample.TrimEnd(","c) + " "
            getJSONSample += "}}, " & vbLf & vbTab & "function() { alert('Success!'); });</pre>"
            sb.AppendLine(getJSONSample)

            sb.AppendLine("</td>")
            sb.AppendLine("</tr></table>")
        Next
        Return sb.ToString()
    End Function

End Class
