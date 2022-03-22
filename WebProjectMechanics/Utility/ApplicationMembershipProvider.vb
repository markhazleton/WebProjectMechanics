Imports System.Collections.Specialized
'Adapted from: http://msdn2.microsoft.com/en-us/library/6tc47t75(VS.80).aspx
Imports System.Configuration
Imports System.Configuration.Provider
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web.Security

' This provider works with the following schema for the table of user data.
' 
'CREATE TABLE [dbo].[Users](
'	[UserID] [uniqueidentifier] NOT NULL,
'	[Username] [nvarchar](255) NOT NULL,
'	[ApplicationName] [nvarchar](255) NOT NULL,
'	[Email] [nvarchar](128) NOT NULL,
'	[Comment] [nvarchar](255) NULL,
'	[Password] [nvarchar](128) NOT NULL,
'	[PasswordQuestion] [nvarchar](255) NULL,
'	[PasswordAnswer] [nvarchar](255) NULL,
'	[IsApproved] [bit] NULL,
'	[LastActivityDate] [datetime] NULL,
'	[LastLoginDate] [datetime] NULL,
'	[LastPasswordChangedDate] [datetime] NULL,
'	[CreationDate] [datetime] NULL,
'	[IsOnLine] [bit] NULL,
'	[IsLockedOut] [bit] NULL,
'	[LastLockedOutDate] [datetime] NULL,
'	[FailedPasswordAttemptCount] [int] NULL,
'	[FailedPasswordAttemptWindowStart] [datetime] NULL,
'	[FailedPasswordAnswerAttemptCount] [int] NULL,
'	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
'PRIMARY KEY CLUSTERED 
'(
'	[UserID] ASC
')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
') ON [PRIMARY]
' 

Public NotInheritable Class ApplicationMembershipProvider
    Inherits MembershipProvider

#Region "Class Variables"

    Private Const _newPasswordLength As Integer = 8
    Private _sqlConnectionString As String
    Private _enablePasswordReset As Boolean
    Private _enablePasswordRetrieval As Boolean
    Private _requiresQuestionAndAnswer As Boolean
    Private _requiresUniqueEmail As Boolean
    Private _maxInvalidPasswordAttempts As Integer
    Private _passwordAttemptWindow As Integer
    Private _passwordFormat As MembershipPasswordFormat
    Private _minRequiredNonAlphanumericCharacters As Integer
    Private _minRequiredPasswordLength As Integer
    Private _passwordStrengthRegularExpression As String
    Private _machineKey As MachineKeySection

    ' Used when determining encryption key values.

#End Region

#Region "Enums"

    Private Enum FailureType
        Password = 1
        PasswordAnswer = 2
    End Enum

#End Region

#Region "Properties"

    Public Overrides Property ApplicationName() As String

    Public Overrides ReadOnly Property EnablePasswordReset() As Boolean
        Get
            Return _enablePasswordReset
        End Get
    End Property


    Public Overrides ReadOnly Property EnablePasswordRetrieval() As Boolean
        Get
            Return _enablePasswordRetrieval
        End Get
    End Property


    Public Overrides ReadOnly Property RequiresQuestionAndAnswer() As Boolean
        Get
            Return _requiresQuestionAndAnswer
        End Get
    End Property


    Public Overrides ReadOnly Property RequiresUniqueEmail() As Boolean
        Get
            Return _requiresUniqueEmail
        End Get
    End Property


    Public Overrides ReadOnly Property MaxInvalidPasswordAttempts() As Integer
        Get
            Return _maxInvalidPasswordAttempts
        End Get
    End Property


    Public Overrides ReadOnly Property PasswordAttemptWindow() As Integer
        Get
            Return _passwordAttemptWindow
        End Get
    End Property


    Public Overrides ReadOnly Property PasswordFormat() As MembershipPasswordFormat
        Get
            Return _passwordFormat
        End Get
    End Property


    Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters() As Integer
        Get
            Return _minRequiredNonAlphanumericCharacters
        End Get
    End Property


    Public Overrides ReadOnly Property MinRequiredPasswordLength() As Integer
        Get
            Return _minRequiredPasswordLength
        End Get
    End Property


    Public Overrides ReadOnly Property PasswordStrengthRegularExpression() As String
        Get
            Return _passwordStrengthRegularExpression
        End Get
    End Property

#End Region

#Region "Initialization"

    Public Overrides Sub Initialize(ByVal name As String, ByVal config As NameValueCollection)
        If config Is Nothing Then _
          Throw New ArgumentNullException("config")

        If name Is Nothing OrElse name.Length = 0 Then _
          name = "HDIMembershipProvider"

        If String.IsNullOrEmpty(config("description")) Then
            config.Remove("description")
            config.Add("description", "How Do I: Sample Membership provider")
        End If

        ' Initialize the abstract base class.
        MyBase.Initialize(name, config)

        ApplicationName = GetConfigValue(config("applicationName"), System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
        _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config("maxInvalidPasswordAttempts"), "5"))
        _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config("passwordAttemptWindow"), "10"))
        _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config("minRequiredAlphaNumericCharacters"), "1"))
        _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config("minRequiredPasswordLength"), "7"))
        _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config("passwordStrengthRegularExpression"), String.Empty))
        _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config("enablePasswordReset"), "True"))
        _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config("enablePasswordRetrieval"), "True"))
        _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config("requiresQuestionAndAnswer"), "False"))
        _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config("requiresUniqueEmail"), "True"))

        Dim temp_format As String = config("passwordFormat")
        If temp_format Is Nothing Then
            temp_format = "Hashed"
        End If

        Select Case temp_format
            Case "Hashed"
                _passwordFormat = MembershipPasswordFormat.Hashed
            Case "Encrypted"
                _passwordFormat = MembershipPasswordFormat.Encrypted
            Case "Clear"
                _passwordFormat = MembershipPasswordFormat.Clear
            Case Else
                Throw New ProviderException("Password format not supported.")
        End Select

        Dim ConnectionStringSettings As ConnectionStringSettings = _
          ConfigurationManager.ConnectionStrings(config("connectionStringName"))

        If ConnectionStringSettings Is Nothing OrElse ConnectionStringSettings.ConnectionString.Trim() = String.Empty Then
            Throw New ProviderException("Connection string cannot be blank.")
        End If

        _sqlConnectionString = ConnectionStringSettings.ConnectionString

        ' Get encryption and decryption key information from the configuration.
        Dim cfg As System.Configuration.Configuration = _
          WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath)
        _machineKey = CType(cfg.GetSection("system.web/machineKey"), MachineKeySection)

        If _machineKey.ValidationKey.Contains("AutoGenerate") Then _
          If PasswordFormat <> MembershipPasswordFormat.Clear Then _
            Throw New ProviderException("Hashed or Encrypted passwords " & _
                                        "are not supported with auto-generated keys.")

    End Sub

#End Region

#Region "Implemented Abstract Methods from MembershipProvider"

    ''' <summary>
    ''' Change the user password.
    ''' </summary>
    ''' <param name="username">UserName</param>
    ''' <param name="oldPwd">Old password.</param>
    ''' <param name="newPwd">New password.</param>
    ''' <returns>T/F if password was changed.</returns>
    Public Overrides Function ChangePassword(ByVal username As String, _
    ByVal oldPwd As String, _
    ByVal newPwd As String) As Boolean

        If Not ValidateUser(username, oldPwd) Then _
          Return False

        Dim args As ValidatePasswordEventArgs = _
          New ValidatePasswordEventArgs(username, newPwd, True)

        OnValidatingPassword(args)

        If args.Cancel Then
            If Not args.FailureInformation Is Nothing Then
                Throw args.FailureInformation
            Else
                Throw New Exception("Change password canceled due to New password validation failure.")
            End If
        End If

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_ChangePassword", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar, 255).Value = EncodePassword(newPwd)
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.ChangePassword", e.Message)
                Return False
            Finally
                _sqlConnection.Close()
            End Try
        End Using

        Return True

    End Function
    ''' <summary>
    ''' Change the question and answer for a password validation.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="password">Password.</param>
    ''' <param name="newPwdQuestion">New question text.</param>
    ''' <param name="newPwdAnswer">New answer text.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ChangePasswordQuestionAndAnswer( _
    ByVal username As String, _
    ByVal password As String, _
    ByVal newPwdQuestion As String, _
    ByVal newPwdAnswer As String) As Boolean

        If Not ValidateUser(username, password) Then _
          Return False

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_ChangePasswordQuestionAnswer", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@returnValue", SqlDbType.Int, 0).Direction = ParameterDirection.ReturnValue
            _sqlCommand.Parameters.Add("@question", SqlDbType.NVarChar, 255).Value = newPwdQuestion
            _sqlCommand.Parameters.Add("@answer", SqlDbType.NVarChar, 255).Value = EncodePassword(newPwdAnswer)
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
                If (_sqlCommand.Parameters("@returnValue").ToString <> "0") Then
                    Return False
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.ChangePasswordQuestionAndAnswer", e.Message)
                Return False
            Finally
                _sqlConnection.Close()
            End Try
        End Using

        Return True

    End Function
    ''' <summary>
    ''' Create a new user.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="password">Password.</param>
    ''' <param name="email">Email address.</param>
    ''' <param name="passwordQuestion">Security quesiton for password.</param>
    ''' <param name="passwordAnswer">Security quesiton answer for password.</param>
    ''' <param name="isApproved"></param>
    ''' <param name="userID"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    Public Overrides Function CreateUser( _
    ByVal username As String, _
    ByVal password As String, _
    ByVal email As String, _
    ByVal passwordQuestion As String, _
    ByVal passwordAnswer As String, _
    ByVal isApproved As Boolean, _
    ByVal userID As Object, _
    ByRef status As MembershipCreateStatus _
    ) _
    As MembershipUser

        Dim _args As ValidatePasswordEventArgs = _
          New ValidatePasswordEventArgs(username, password, True)

        OnValidatingPassword(_args)

        If _args.Cancel Then
            status = MembershipCreateStatus.InvalidPassword
            Return Nothing
        End If

        If (RequiresUniqueEmail AndAlso (GetUserNameByEmail(email) <> String.Empty)) Then
            status = MembershipCreateStatus.DuplicateEmail
            Return Nothing
        End If

        Dim _membershipUser As MembershipUser = GetUser(username, False)

        If _membershipUser Is Nothing Then
            ' Dim createDate As DateTime = DateTime.Now

            Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
                Dim _sqlCommand As SqlCommand = New SqlCommand("User_Ins", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
                _sqlCommand.Parameters.Add("@returnValue", SqlDbType.Int, 0).Direction = ParameterDirection.ReturnValue
                _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
                _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
                _sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar, 255).Value = EncodePassword(password)
                _sqlCommand.Parameters.Add("@email", SqlDbType.NVarChar, 128).Value = email
                _sqlCommand.Parameters.Add("@passwordQuestion", SqlDbType.NVarChar, 255).Value = passwordQuestion
                _sqlCommand.Parameters.Add("@passwordAnswer", SqlDbType.NVarChar, 255).Value = EncodePassword(passwordAnswer)
                _sqlCommand.Parameters.Add("@isApproved", SqlDbType.Bit).Value = isApproved
                _sqlCommand.Parameters.Add("@comment", SqlDbType.NVarChar, 255).Value = String.Empty
                Try
                    _sqlConnection.Open()
                    _sqlCommand.ExecuteNonQuery()
                    If (_sqlCommand.Parameters("@returnValue").ToString = "0") Then
                        status = MembershipCreateStatus.Success
                    Else
                        status = MembershipCreateStatus.UserRejected
                    End If
                Catch e As SqlException
                    'Add exception handling here.
                    ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.CreateUser", e.Message)

                    status = MembershipCreateStatus.ProviderError
                Finally
                    _sqlConnection.Close()
                End Try
            End Using

            Return GetUser(username, False)
        Else
            status = MembershipCreateStatus.DuplicateUserName
        End If

        Return Nothing
    End Function
    ''' <summary>
    ''' Delete a user.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="deleteAllRelatedData">Whether to delete all related data.</param>
    ''' <returns>T/F if the user was deleted.</returns>
    Public Overrides Function DeleteUser( _
    ByVal username As String, _
    ByVal deleteAllRelatedData As Boolean _
    ) As Boolean

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_Del", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@returnValue", SqlDbType.Int, 0).Direction = ParameterDirection.ReturnValue
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
                If (_sqlCommand.Parameters("@returnValue").ToString = "0") Then
                    If deleteAllRelatedData Then
                        ' Process commands to delete all data for the user in the database.
                    End If
                Else
                    Return False
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.DeleteUser", e.Message)
            Finally
                _sqlConnection.Close()
            End Try
        End Using

        Return True

    End Function
    ''' <summary>
    ''' Get a collection of users.
    ''' </summary>
    ''' <param name="pageIndex">Page index.</param>
    ''' <param name="pageSize">Page size.</param>
    ''' <param name="totalRecords">Total # of records to retrieve.</param>
    ''' <returns>Collection of MembershipUser objects.</returns>
    Public Overrides Function GetAllUsers( _
    ByVal pageIndex As Integer, _
    ByVal pageSize As Integer, _
    ByRef totalRecords As Integer _
    ) _
    As MembershipUserCollection

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("Users_Sel", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _users As MembershipUserCollection = New MembershipUserCollection()
            Dim _sqlDataReader As SqlDataReader = Nothing
            totalRecords = 0
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Dim _counter As Integer = 0
                Dim _startIndex As Integer = pageSize * pageIndex
                Dim _endIndex As Integer = _startIndex + pageSize - 1
                Do While _sqlDataReader.Read()
                    If _counter >= _startIndex Then
                        _users.Add(GetUserFromReader(_sqlDataReader))
                    End If
                    If _counter >= _endIndex Then _sqlCommand.Cancel()
                    _counter += 1
                Loop
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetAllUsers", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            Return _users
        End Using

    End Function
    ''' <summary>
    ''' Gets the number of users currently on-line.
    ''' </summary>
    ''' <returns>
    ''' # of users on-line.</returns>
    Public Overrides Function GetNumberOfUsersOnline() As Integer

        Dim _onlineSpan As TimeSpan = New TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0)
        Dim _compareTime As DateTime = DateTime.Now.Subtract(_onlineSpan)

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("Users_NumberOnline", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            _sqlCommand.Parameters.Add("@compareDate", SqlDbType.DateTime).Value = _compareTime
            Dim numOnline As Integer = 0
            Try
                _sqlConnection.Open()
                numOnline = CInt(_sqlCommand.ExecuteScalar())
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetNumberOfUsersOnline", e.Message)
            Finally
                _sqlConnection.Close()
            End Try
            Return numOnline
        End Using

    End Function
    ''' <summary>
    ''' Get the password for a user.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="answer">Answer to security question.</param>
    ''' <returns>Password for the user.</returns>
    Public Overrides Function GetPassword( _
    ByVal username As String, _
    ByVal answer As String _
    ) _
    As String

        If Not EnablePasswordRetrieval Then
            Throw New ProviderException("Password Retrieval Not Enabled.")
        End If

        If PasswordFormat = MembershipPasswordFormat.Hashed Then
            Throw New ProviderException("Cannot retrieve Hashed passwords.")
        End If

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_GetPassword", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _password As String = String.Empty
            Dim _passwordAnswer As String = String.Empty
            Dim _sqlDataReader As SqlDataReader = Nothing
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CType(CommandBehavior.SingleRow + CommandBehavior.CloseConnection, CommandBehavior))
                If _sqlDataReader.HasRows Then
                    _sqlDataReader.Read()
                    If _sqlDataReader.GetBoolean(2) Then Throw New MembershipPasswordException("The supplied user is locked out.")
                    _password = _sqlDataReader.GetString(0)
                    _passwordAnswer = _sqlDataReader.GetString(1)
                Else
                    Throw New MembershipPasswordException("The supplied user name is not found.")
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetPassword", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            If RequiresQuestionAndAnswer AndAlso Not CheckPassword(answer, _passwordAnswer) Then
                UpdateFailureCount(username, FailureType.PasswordAnswer)
                Throw New MembershipPasswordException("Incorrect password answer.")
            End If
            If PasswordFormat = MembershipPasswordFormat.Encrypted Then
                _password = UnEncodePassword(_password)
            End If
            Return _password
        End Using
    End Function
    Public Overrides Function GetUser( _
    ByVal username As String, _
    ByVal userIsOnline As Boolean _
    ) _
    As MembershipUser

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_Sel", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _membershipUser As MembershipUser = Nothing
            Dim _sqlDataReader As SqlDataReader = Nothing
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                If _sqlDataReader.HasRows Then
                    _sqlDataReader.Read()
                    _membershipUser = GetUserFromReader(_sqlDataReader)
                    If userIsOnline Then
                        Dim _sqlUpdateCommand As SqlCommand = New SqlCommand("User_UpdateActivityDate_ByUserName", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
                        _sqlUpdateCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
                        _sqlUpdateCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
                        _sqlUpdateCommand.ExecuteNonQuery()
                    End If
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetUser", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            Return _membershipUser
        End Using
    End Function
    ''' <summary>
    ''' Get a user based upon provider key and if they are on-line.
    ''' </summary>
    ''' <param name="userID">Provider key.</param>
    ''' <param name="userIsOnline">T/F whether the user is on-line.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetUser( _
    ByVal userID As Object, _
    ByVal userIsOnline As Boolean _
    ) _
    As MembershipUser

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_SelByUserID", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
            Dim _membershipUser As MembershipUser = Nothing
            Dim _sqlDataReader As SqlDataReader = Nothing
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                If _sqlDataReader.HasRows Then
                    _sqlDataReader.Read()
                    _membershipUser = GetUserFromReader(_sqlDataReader)
                    If userIsOnline Then
                        Dim _sqlUpdateCommand As SqlCommand = New SqlCommand("User_UpdateActivityDate_ByUserID", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
                        _sqlUpdateCommand.Parameters.Add("@userID", SqlDbType.NVarChar, 255).Value = userID
                        _sqlUpdateCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
                        _sqlUpdateCommand.ExecuteNonQuery()
                    End If
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetUser", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            Return _membershipUser
        End Using

    End Function

    ''' <summary>
    ''' Unlock a user.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <returns>T/F if unlocked.</returns>
    Public Overrides Function UnlockUser( _
    ByVal username As String _
    ) _
    As Boolean

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_Unlock", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@returnValue", SqlDbType.Int, 0).Direction = ParameterDirection.ReturnValue
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            'Dim _rowsAffected As Integer = 0
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
                If (_sqlCommand.Parameters("@returnValue").ToString = "0") Then
                    Return False
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.UnlockUser", e.Message)
                Return False
            Finally
                _sqlConnection.Close()
            End Try
        End Using

        Return True

    End Function
    ''' <summary>
    ''' Get user name from their email.
    ''' </summary>
    ''' <param name="email">Email address</param>
    ''' <returns>User name.</returns>
    Public Overrides Function GetUserNameByEmail(ByVal email As String) As String
        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("UserName_Sel_ByEmail", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@email", SqlDbType.NVarChar, 128).Value = email
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _username As String = String.Empty
            Try
                _sqlConnection.Open()
                _username = CStr(_sqlCommand.ExecuteScalar())
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.GetUserNameByEmail", e.Message)
            Finally
                _sqlConnection.Close()
            End Try
            If (_username Is Nothing) Then
                Return String.Empty
            Else
                _username.Trim()
            End If
            Return _username
        End Using

    End Function
    ''' <summary>
    ''' Reset the user password.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="answer">Answer to security question.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ResetPassword( _
    ByVal username As String, _
    ByVal answer As String _
    ) _
    As String

        If Not EnablePasswordReset Then
            Throw New NotSupportedException("Password Reset is not enabled.")
        End If

        If answer Is Nothing AndAlso RequiresQuestionAndAnswer Then
            UpdateFailureCount(username, FailureType.PasswordAnswer)

            Throw New ProviderException("Password answer required for password Reset.")
        End If

        Dim _newPassword As String = _
          Membership.GeneratePassword( _
          _newPasswordLength, _
          MinRequiredNonAlphanumericCharacters _
          )

        Dim _args As ValidatePasswordEventArgs = New ValidatePasswordEventArgs(username, _newPassword, True)

        OnValidatingPassword(_args)

        If _args.Cancel Then
            If Not _args.FailureInformation Is Nothing Then
                Throw _args.FailureInformation
            Else
                Throw New MembershipPasswordException("Reset password canceled due to password validation failure.")
            End If
        End If

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_GetPasswordAnswer", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _rowsAffected As Integer = 0
            Dim _passwordAnswer As String = String.Empty
            Dim _sqlDataReader As SqlDataReader = Nothing
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CType(CommandBehavior.SingleRow + CommandBehavior.CloseConnection, CommandBehavior))
                If _sqlDataReader.HasRows Then
                    _sqlDataReader.Read()
                    If _sqlDataReader.GetBoolean(1) Then Throw New MembershipPasswordException("The supplied user is locked out.")
                    _passwordAnswer = _sqlDataReader.GetString(0)
                Else
                    Throw New MembershipPasswordException("The supplied user name is not found.")
                End If
                If RequiresQuestionAndAnswer AndAlso Not CheckPassword(answer, _passwordAnswer) Then
                    UpdateFailureCount(username, FailureType.PasswordAnswer)
                    Throw New MembershipPasswordException("Incorrect password answer.")
                End If
                Dim _sqlUpdateCommand As SqlCommand = New SqlCommand("User_UpdatePassword", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
                _sqlUpdateCommand.Parameters.Add("@password", SqlDbType.NVarChar, 255).Value = EncodePassword(_newPassword)
                _sqlUpdateCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
                _sqlUpdateCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
                _rowsAffected = _sqlUpdateCommand.ExecuteNonQuery()
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.ResetPassword", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            If _rowsAffected > 0 Then
                Return _newPassword
            Else
                Throw New MembershipPasswordException("User not found, or user is locked out. Password not Reset.")
            End If
        End Using
    End Function
    ''' <summary>
    ''' Update the user information.
    ''' </summary>
    ''' <param name="_membershipUser">MembershipUser object containing data.</param>
    Public Overrides Sub UpdateUser(ByVal _membershipUser As MembershipUser)

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_Upd", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@email", SqlDbType.NVarChar, 128).Value = _membershipUser.Email
            _sqlCommand.Parameters.Add("@comment", SqlDbType.NVarChar, 255).Value = _membershipUser.Comment
            _sqlCommand.Parameters.Add("@isApproved", SqlDbType.Bit).Value = _membershipUser.IsApproved
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = _membershipUser.UserName
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.UpdateUser", e.Message)
            Finally
                _sqlConnection.Close()
            End Try
        End Using
    End Sub
    ''' <summary>
    ''' Validate the user based upon username and password.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="password">Password.</param>
    ''' <returns>T/F if the user is valid.</returns>
    Public Overrides Function ValidateUser( _
    ByVal username As String, _
    ByVal password As String _
    ) _
    As Boolean

        Dim _isValid As Boolean = False

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("User_Validate", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _sqlDataReader As SqlDataReader = Nothing
            Dim _isApproved As Boolean = False
            Dim _storedPassword As String = String.Empty
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.SingleRow)
                If _sqlDataReader.HasRows Then
                    _sqlDataReader.Read()
                    _storedPassword = _sqlDataReader.GetString(0)
                    _isApproved = _sqlDataReader.GetBoolean(1)
                Else
                    Return False
                End If
                _sqlDataReader.Close()
                If CheckPassword(password, _storedPassword) Then
                    If _isApproved Then
                        _isValid = True
                        Dim _sqlUpdateCommand As SqlCommand = New SqlCommand("User_UpdateLoginDate", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
                        _sqlUpdateCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = username
                        _sqlUpdateCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
                        _sqlUpdateCommand.ExecuteNonQuery()
                    End If
                Else
                    _sqlConnection.Close()
                    UpdateFailureCount(username, FailureType.Password)
                End If
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.ValidateUser", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
                If Not _sqlConnection Is Nothing And _sqlConnection.State = ConnectionState.Open Then _sqlConnection.Close()
            End Try
        End Using

        Return _isValid
    End Function
    ''' <summary>
    ''' Find all users matching a search string.
    ''' </summary>
    ''' <param name="usernameToMatch">Search string of user name to match.</param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="totalRecords">Total records found.</param>
    ''' <returns>Collection of MembershipUser objects.</returns>
    Public Overrides Function FindUsersByName( _
    ByVal usernameToMatch As String, _
    ByVal pageIndex As Integer, _
    ByVal pageSize As Integer, _
    ByRef totalRecords As Integer _
    ) _
    As MembershipUserCollection

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("Users_Sel_ByUserName", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar, 255).Value = usernameToMatch
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _membershipUsers As MembershipUserCollection = New MembershipUserCollection()
            Dim _sqlDataReader As SqlDataReader = Nothing
            Dim _counter As Integer = 0
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Dim _startIndex As Integer = pageSize * pageIndex
                Dim _endIndex As Integer = _startIndex + pageSize - 1
                Do While _sqlDataReader.Read()
                    If _counter >= _startIndex Then
                        Dim _membershipUser As MembershipUser = GetUserFromReader(_sqlDataReader)
                        _membershipUsers.Add(_membershipUser)
                    End If
                    If _counter >= _endIndex Then _sqlCommand.Cancel()
                    _counter += 1
                Loop
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.FindUsersByName", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            totalRecords = _counter
            Return _membershipUsers
        End Using
    End Function
    ''' <summary>
    ''' Find all users matching a search string of their email.
    ''' </summary>
    ''' <param name="emailToMatch">Search string of email to match.</param>
    ''' <param name="pageIndex"></param>
    ''' <param name="pageSize"></param>
    ''' <param name="totalRecords">Total records found.</param>
    ''' <returns>Collection of MembershipUser objects.</returns>
    Public Overrides Function FindUsersByEmail( _
    ByVal emailToMatch As String, _
    ByVal pageIndex As Integer, _
    ByVal pageSize As Integer, _
    ByRef totalRecords As Integer) _
    As MembershipUserCollection

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("Users_Sel_ByUserName", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@email", SqlDbType.NVarChar, 255).Value = emailToMatch
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Dim _membershipUsers As MembershipUserCollection = New MembershipUserCollection()
            Dim _sqlDataReader As SqlDataReader = Nothing
            Dim _counter As Integer = 0
            Try
                _sqlConnection.Open()
                _sqlDataReader = _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Dim _startIndex As Integer = pageSize * pageIndex
                Dim _endIndex As Integer = _startIndex + pageSize - 1
                Do While _sqlDataReader.Read()
                    If _counter >= _startIndex Then
                        Dim _membershipUser As MembershipUser = GetUserFromReader(_sqlDataReader)
                        _membershipUsers.Add(_membershipUser)
                    End If
                    If _counter >= _endIndex Then _sqlCommand.Cancel()
                    _counter += 1
                Loop
            Catch e As SqlException
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.FindUsersByEmail", e.Message)
            Finally
                If Not _sqlDataReader Is Nothing Then _sqlDataReader.Close()
            End Try
            totalRecords = _counter
            Return _membershipUsers
        End Using
    End Function

#End Region

#Region "Utility Functions"
    ''' <summary>
    ''' Create a MembershipUser object from a data reader.
    ''' </summary>
    ''' <param name="_sqlDataReader">Data reader.</param>
    ''' <returns>MembershipUser object.</returns>
    Private Function GetUserFromReader( _
    ByVal _sqlDataReader As SqlDataReader _
    ) _
    As MembershipUser

        Dim _userID As Object = _sqlDataReader.GetValue(0)
        Dim _username As String = _sqlDataReader.GetString(1)
        Dim _email As String = _sqlDataReader.GetString(2)

        Dim _passwordQuestion As String
        If _sqlDataReader.GetValue(3) IsNot DBNull.Value Then
            _passwordQuestion = _sqlDataReader.GetString(3)
        Else
            _passwordQuestion = String.Empty
        End If

        Dim _comment As String
        If _sqlDataReader.GetValue(4) IsNot DBNull.Value Then
            _comment = _sqlDataReader.GetString(4)
        Else
            _comment = String.Empty
        End If

        Dim _isApproved As Boolean = _sqlDataReader.GetBoolean(5)
        Dim _isLockedOut As Boolean = _sqlDataReader.GetBoolean(6)
        Dim _creationDate As DateTime = _sqlDataReader.GetDateTime(7)

        Dim _lastLoginDate As DateTime
        If _sqlDataReader.GetValue(8) IsNot DBNull.Value Then
            _lastLoginDate = _sqlDataReader.GetDateTime(8)
        Else
            _lastLoginDate = New DateTime()
        End If

        Dim _lastActivityDate As DateTime = _sqlDataReader.GetDateTime(9)
        Dim _lastPasswordChangedDate As DateTime = _sqlDataReader.GetDateTime(10)

        Dim _lastLockedOutDate As DateTime
        If _sqlDataReader.GetValue(11) IsNot DBNull.Value Then
            _lastLockedOutDate = _sqlDataReader.GetDateTime(11)
        Else
            _lastLockedOutDate = New DateTime()
        End If

        Dim _membershipUser As MembershipUser = New MembershipUser( _
        Name, _
        _username, _
        _userID, _
        _email, _
        _passwordQuestion, _
        _comment, _
        _isApproved, _
        _isLockedOut, _
        _creationDate, _
        _lastLoginDate, _
        _lastActivityDate, _
        _lastPasswordChangedDate, _
        _lastLockedOutDate _
        )

        Return _membershipUser

    End Function
    ''' <summary>
    ''' Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration
    ''' </summary>
    ''' <param name="hexString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function HexToByte(ByVal hexString As String) As Byte()
        Dim ReturnBytes((hexString.Length \ 2) - 1) As Byte
        For i As Integer = 0 To ReturnBytes.Length - 1
            ReturnBytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
        Next
        Return ReturnBytes
    End Function
    ''' <summary>
    ''' Update password and answer failure information.
    ''' </summary>
    ''' <param name="username">User name.</param>
    ''' <param name="failureType">Type of failure</param>
    ''' <remarks></remarks>
    Private Sub UpdateFailureCount(ByVal username As String, ByVal failureType As FailureType)

        Using _sqlConnection As SqlConnection = New SqlConnection(_sqlConnectionString)
            Dim _sqlCommand As SqlCommand = New SqlCommand("Users_FailedPassword_Upd", _sqlConnection) With {.CommandType = CommandType.StoredProcedure}
            _sqlCommand.Parameters.Add("@failureType", SqlDbType.Int, 0).Value = failureType
            _sqlCommand.Parameters.Add("@passwordAttemptWindow", SqlDbType.Int, 0).Value = _passwordAttemptWindow
            _sqlCommand.Parameters.Add("@maxInvalidPasswordAttempts", SqlDbType.Int, 0).Value = _maxInvalidPasswordAttempts
            _sqlCommand.Parameters.Add("@userName", SqlDbType.NVarChar, 255).Value = username
            _sqlCommand.Parameters.Add("@applicationName", SqlDbType.NVarChar, 255).Value = ApplicationName
            Try
                _sqlConnection.Open()
                _sqlCommand.ExecuteNonQuery()
            Catch ex As Exception
                'Add exception handling here.
                ApplicationLogging.MembershipProviderError("ApplicationMembershipProvider.UpdateFailureCount", ex.InnerException.ToString)

            End Try
        End Using

    End Sub
    ''' <summary>
    ''' Check the password format based upon the MembershipPasswordFormat.
    ''' </summary>
    ''' <param name="password">Password</param>
    ''' <param name="dbpassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckPassword(ByVal password As String, ByVal dbpassword As String) As Boolean
        Dim pass1 As String = password
        Dim pass2 As String = dbpassword

        Select Case PasswordFormat
            Case MembershipPasswordFormat.Encrypted
                pass2 = UnEncodePassword(dbpassword)
            Case MembershipPasswordFormat.Hashed
                pass1 = EncodePassword(password)
            Case MembershipPasswordFormat.Clear
                ' Do Nothing
            Case Else
                ' Do Nothing
        End Select

        If pass1 = pass2 Then
            Return True
        End If

        Return False
    End Function
    ''' <summary>
    ''' Encode password.
    ''' </summary>
    ''' <param name="password">Password.</param>
    ''' <returns>Encoded password.</returns>
    Private Function EncodePassword(ByVal password As String) As String
        Dim encodedPassword As String = password

        Select Case PasswordFormat
            Case MembershipPasswordFormat.Clear

            Case MembershipPasswordFormat.Encrypted
                encodedPassword = _
                  Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)))
            Case MembershipPasswordFormat.Hashed
                Using hash As HMACSHA1 = New HMACSHA1() With {.Key = HexToByte(_machineKey.ValidationKey)}
                    encodedPassword = _
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)))
                End Using
            Case Else
                Throw New ProviderException("Unsupported password format.")
        End Select

        Return encodedPassword
    End Function
    ''' <summary>
    ''' UnEncode password.
    ''' </summary>
    ''' <param name="encodedPassword">Password.</param>
    ''' <returns>Unencoded password.</returns>
    Private Function UnEncodePassword(ByVal encodedPassword As String) As String
        Dim password As String = encodedPassword

        Select Case PasswordFormat
            Case MembershipPasswordFormat.Clear

            Case MembershipPasswordFormat.Encrypted
                password = _
                  Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)))
            Case MembershipPasswordFormat.Hashed
                Throw New ProviderException("Cannot unencode a hashed password.")
            Case Else
                Throw New ProviderException("Unsupported password format.")
        End Select

        Return password
    End Function

    ''' <summary>
    ''' GetConfigValue - Checks for blank, returns default if current value is NullorEmpty
    ''' </summary>
    ''' <param name="configValue"></param>
    ''' <param name="defaultValue"></param>
    ''' <returns></returns>
    Private Shared Function GetConfigValue(ByVal configValue As String, ByVal defaultValue As String) As String
        If String.IsNullOrEmpty(configValue) Then _
          Return defaultValue

        Return configValue
    End Function


#End Region

End Class