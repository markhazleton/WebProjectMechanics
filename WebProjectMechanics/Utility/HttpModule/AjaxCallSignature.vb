
Imports System.ComponentModel
Imports System.Reflection
Imports System.Web.Script.Serialization

Public Class AjaxCallSignature
    Public Sub New(context As HttpContext)
        Dim qItem As String = Nothing

        args = New Dictionary(Of String, Object)()
        method = String.Empty
        Dim nullKeyParameter As String = context.Request.QueryString(qItem)

        If New List(Of String)() From { _
            "POST", _
            "PUT", _
            "DELETE" _
        }.Contains(context.Request.RequestType.ToUpper()) Then
            Dim requestParams As String() = context.Request.Params.AllKeys
            For Each item As String In requestParams
                If item.ToLower() = "method" Then
                    method = context.Request.Params(item)
                ElseIf item.ToLower().StartsWith("args[") Then
                    Dim key As String = item.Trim().TrimEnd("]"c).Substring(5)
                    key = key.Trim().Replace("][", "+")

                    Dim value As String = context.Request.Params(item)
                    args.Add(key, value)
                Else
                    Dim key As String = item
                    Dim value As String = context.Request.Params(item)
                    args.Add(key, value)
                End If
            Next
        ElseIf context.Request.RequestType.ToUpper() = "GET" Then
            ' evaluate the data passed as json
            If Not String.IsNullOrEmpty(nullKeyParameter) Then
                If nullKeyParameter.ToLower() = "help" Then
                    method = "help"
                    Return
                Else
                    Dim json As Object = Nothing
                    Dim serializer As New JavaScriptSerializer()
                    json = serializer.DeserializeObject(context.Request.QueryString(qItem))

                    Try
                        Dim dict As Dictionary(Of String, [Object]) = DirectCast(json, Dictionary(Of String, [Object]))

                        If dict.ContainsKey("method") Then
                            method = dict("method").ToString()
                        Else
                            Throw New Exception("Invalid BaseHandler call. MethodName parameter is mandatory in json object.")
                        End If

                        If dict.ContainsKey("returntype") Then
                            returnType = dict("returntype").ToString()
                        End If

                        If dict.ContainsKey("args") Then
                            args = DirectCast(dict("args"), Dictionary(Of String, [Object]))
                        Else
                            args = New Dictionary(Of String, Object)()
                        End If
                    Catch
                        Throw New InvalidCastException("Unable to cast json object to AjaxCallSignature")
                    End Try
                End If
            End If

            ' evaluate data passed as querystring params
            For Each key As String In context.Request.QueryString.Keys
                If key Is Nothing Then
                    Continue For
                End If

                If key.ToLower() = "method" Then
                    If String.IsNullOrEmpty(method) Then
                        method = context.Request.QueryString(key)
                    Else
                        Throw New Exception("Method name was already specified on the json data. Specify the method name only once, either on QueryString params or on the json data.")
                    End If
                ElseIf key.ToLower() = "returntype" Then
                    returnType = context.Request.QueryString(key)
                ElseIf key.ToLower().StartsWith("args[") Then
                    Dim _key As String = key.Trim().Substring(5).TrimEnd("]"c).Replace("][", "+")
                    args.Add(_key, context.Request.QueryString(key))
                Else
                    args.Add(key, context.Request.QueryString(key))
                End If
            Next
        End If
    End Sub

    Public Property method() As String
        Get
            Return m_method
        End Get
        Set(value As String)
            m_method = value
        End Set
    End Property
    Private m_method As String
    Public Property returnType() As String
        Get
            Return m_returnType
        End Get
        Set(value As String)
            m_returnType = value
        End Set
    End Property
    Private m_returnType As String
    Public Property args() As Dictionary(Of String, Object)
        Get
            Return m_args
        End Get
        Set(value As Dictionary(Of String, Object))
            m_args = value
        End Set
    End Property
    Private m_args As Dictionary(Of String, Object)

    Public Function Invoke(handler As BaseHandler, context As HttpContext) As Object
        Dim m As MethodInfo = Nothing
        If String.IsNullOrEmpty(method) Then
            ' call the request method
            ' if no method is passed then well call the method by HTTP verb (GET, POST, DELETE, UPDATE)
            method = context.Request.RequestType.ToUpper()
        End If
        m = handler.[GetType]().GetMethod(method)

        Dim a As New List(Of Object)()

        If m Is Nothing Then
            If method.ToLower() = "help" Then
                m = handler.[GetType]().BaseType.GetMethod("Help")
            Else
                Throw New Exception(String.Format("Method {0} not found on Handler {1}.", method, Me.[GetType]().ToString()))
            End If
        Else
            ' evaluate the handler and method attributes against Http allowed verbs
            '
            '				 The logic here is:
            '				 *	-> if no attribute is found means it allows every verb
            '				 *	-> is a method have verb attbibutes defined then it will ignore the ones on the class
            '				 *	-> verb attributes on the class are applied to all methods without verb attribues
            '				 

            Dim handlerSupportedVerbs = handler.[GetType]().GetCustomAttributes(GetType(HttpVerbAttribute), True).Cast(Of HttpVerbAttribute)()
            Dim methodSupportedVerbs = m.GetCustomAttributes(GetType(HttpVerbAttribute), True).Cast(Of HttpVerbAttribute)()

            Dim VerbAllowedOnMethod As Boolean = (methodSupportedVerbs.Count() = 0)
            Dim VerbAllowedOnHandler As Boolean = (handlerSupportedVerbs.Count() = 0)
            If methodSupportedVerbs.Count() > 0 Then
                VerbAllowedOnMethod = methodSupportedVerbs.FirstOrDefault(Function(x) x.HttpVerb = context.Request.RequestType.ToUpper()) IsNot Nothing
            ElseIf handlerSupportedVerbs.Count() > 0 Then
                VerbAllowedOnHandler = handlerSupportedVerbs.FirstOrDefault(Function(x) x.HttpVerb = context.Request.RequestType.ToUpper()) IsNot Nothing
            End If

            If Not VerbAllowedOnMethod OrElse Not VerbAllowedOnHandler Then
                Throw New HttpVerbNotAllowedException()
            End If

            ' security validation: Search for RequireAuthenticationAttribute on the method
            '		value=true the user must be authenticated (only supports FromsAuthentication for now
            '		value=false invoke the method
            Dim attrs As Object() = m.GetCustomAttributes(GetType(RequireAuthenticationAttribute), True)
            If attrs IsNot Nothing AndAlso attrs.Length > 0 Then

                If Not context.Request.IsAuthenticated AndAlso DirectCast(attrs(0), RequireAuthenticationAttribute).RequireAuthentication Then
                    Throw New InvalidOperationException("Method [" + m.Name + "] Requires authentication")
                End If
            End If
        End If

        For Each param As ParameterInfo In m.GetParameters()
            a.Add(ProcessProperty(param.Name, param.ParameterType, String.Empty))
        Next

        ' OnMethodInvoke -> Invoke -> AfterMethodInvoke
        Dim cancelInvoke As New OnMethodInvokeArgs(m)
        handler.OnMethodInvoke(cancelInvoke)

        Dim invokeResult As Object = Nothing
        If Not cancelInvoke.Cancel Then
            invokeResult = m.Invoke(handler, a.ToArray())
            handler.AfterMethodInvoke(invokeResult)
        End If

        Return invokeResult
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="propertyName"></param>
    ''' <param name="propertyType"></param>
    ''' <param name="parentNamespace">represents the full path of the parent node of the input parameter</param>
    ''' <returns></returns>
    Private Function ProcessProperty(propertyName As String, propertyType As Type, parentNamespace As String) As Object
        If propertyType.IsArray OrElse (propertyType.IsGenericType AndAlso propertyType.GetGenericTypeDefinition() = GetType(List(Of ))) Then
            Return HydrateArray(propertyName, propertyType, parentNamespace)
        ElseIf propertyType.IsClass AndAlso Not propertyType.Equals(GetType([String])) Then
            Return HydrateClass(propertyName, propertyType, parentNamespace)
        Else
            Return HydrateValue(propertyName, propertyType, parentNamespace)
        End If
    End Function

    Private Function HydrateArray(propertyName As String, propertyType As Type, parentNamespace As String) As Object
        Dim result As Array = Nothing
        Dim elementType As Type

        If propertyType.IsGenericType Then
            elementType = propertyType.GetGenericArguments()(0)
        Else
            elementType = propertyType.GetElementType()
        End If

        Dim propFQN As [String] = If([String].IsNullOrEmpty(parentNamespace), propertyName, Convert.ToString(parentNamespace & Convert.ToString("+")) & propertyName)

        If elementType.IsValueType Then
            Dim conv As TypeConverter = TypeDescriptor.GetConverter(elementType)
            Dim values As String() = args(propFQN + "+").ToString().Split(New Char() {","c})

            result = Array.CreateInstance(elementType, values.Length)

            For i As Integer = 0 To values.Length - 1
                result.SetValue(conv.ConvertFromString(values(i)), i)
            Next
        Else
            ' get the properties in the current nesting depth
            Dim objectProperties = args.Keys.ToList().FindAll(Function(k) k.StartsWith(propFQN + "+"))

            ' get the number of items in the array
            Dim max_index As Integer = 0
            For Each p As String In objectProperties
                Dim idx As String = p.Remove(0, propFQN.Length + 1)
                idx = idx.Substring(0, idx.IndexOf("+"c))
                Dim i As Integer = Convert.ToInt32(idx)
                If i > max_index Then
                    max_index = i
                End If
            Next

            ' create the instance of the array
            result = Array.CreateInstance(elementType, max_index + 1)
            For i As Integer = 0 To max_index
                Dim nsPrefix As [String] = [String].Format("{0}+{1}", propFQN, i.ToString())
                result.SetValue(ProcessProperty((propertyName & Convert.ToString("+")) + i.ToString(), result.[GetType]().GetElementType(), parentNamespace), i)
            Next
        End If

        If propertyType.IsGenericType Then
            Return Activator.CreateInstance(propertyType, New Object() {result})
        Else
            Return result
        End If
    End Function

    ''' <summary>
    ''' Hydrates CLR primitive types
    ''' </summary>
    ''' <returns></returns>
    Public Function HydrateValue(propertyName As String, propertyType As Type, parentNamespace As [String]) As Object
        Dim propFQN As [String] = If(String.IsNullOrEmpty(parentNamespace), propertyName, Convert.ToString(parentNamespace + "+") & propertyName)
        If args.Keys.Contains(propFQN) Then
            ' its usual to pass an empty json string property but casting it to certain types will throw an exception
            If String.IsNullOrEmpty(args(propFQN).ToString()) OrElse args(propFQN).ToString() = "null" OrElse args(propFQN).ToString() = "undefined" Then
                ' handle numerics. convert null or empty input values to 0
                If propertyType.Equals(GetType(System.Int16)) OrElse propertyType.Equals(GetType(System.Int32)) OrElse propertyType.Equals(GetType(System.Int64)) OrElse propertyType.Equals(GetType(System.Decimal)) OrElse propertyType.Equals(GetType(System.Double)) OrElse propertyType.Equals(GetType(System.Byte)) Then
                    args(propFQN) = 0
                ElseIf propertyType.Equals(GetType(System.Guid)) Then
                    args(propFQN) = New Guid()
                ElseIf propertyType.IsGenericType AndAlso propertyType.GetGenericTypeDefinition() = GetType(Nullable(Of )) Then
                    args(propFQN) = Nothing
                End If
            End If

            ' evaluate special types that are not directly casted from string
            Dim conv As TypeConverter = TypeDescriptor.GetConverter(propertyType)
            If args(propFQN) Is Nothing OrElse propertyType = args(propFQN).[GetType]() Then
                Return args(propFQN)
            Else
                Return conv.ConvertFromInvariantString(args(propFQN).ToString())
            End If
        Else
            ' if there are missing arguments try passing null
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Hydrates complex types
    ''' </summary>
    Public Function HydrateClass(propertyName As String, propertyType As Type, parentNamespace As String) As Object
        Dim argumentObject = Activator.CreateInstance(propertyType)

        ' search for properties on the current namespace
        Dim nsPrefix As String = If(String.IsNullOrEmpty(parentNamespace), propertyName, Convert.ToString(parentNamespace & Convert.ToString("+")) & propertyName)

        Dim objectProperties = args.Keys.ToList().FindAll(Function(k) k.StartsWith(nsPrefix))

        ' loop through them 
        For Each p As String In objectProperties
            Dim propName As [String] = p.Remove(0, nsPrefix.Length + 1).Split("+"c)(0)

            argumentObject.[GetType]().GetProperty(propName).SetValue(argumentObject, ProcessProperty(propName, argumentObject.[GetType]().GetProperty(propName).PropertyType, nsPrefix), Nothing)
        Next

        Return argumentObject
    End Function

End Class

