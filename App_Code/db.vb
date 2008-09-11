Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Text
Imports System.Web.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports EW.Data
Imports EW.Web
Namespace PMGEN

	''' <summary>
	''' Summary description for Db
	''' </summary>

	Public Class Db
		Public Sub New()
			MyBase.New
		End Sub
		Dim _DataErrorMessage As String

		''' <summary>
		''' Gets the data error message.
		''' </summary>
		''' <return>a string repersents the error message.</return>

		Public ReadOnly Property GetDataErrorMessage As String
			Get
				Return _DataErrorMessage
			End Get
		End Property

		''' <summary>
		'''  Gets the database provider name.
		''' </summary>
		''' <return>a string repersents the database provider name.</return>

		Public Shared ReadOnly Property ProviderName As String
			Get
			  Return "System.Data.OleDb"
			End Get
		End Property

		''' <summary>
		''' Gets the connection string.
		''' </summary>
		''' <return>a string repersents the database connection string.</return>

		Public Shared ReadOnly Property ConnStr As String
			Get
			  Return mhConfig.GetSiteDB()			
			End Get
		End Property

		''' <summary>
		''' Gets the database SQL identifier start quote character.
		''' </summary>
		''' <return>a string repersents the database SQL identifier start quote character.</return>

		Public Shared ReadOnly Property QuoteS As String
			Get
				Return "["
			End Get
		End Property

		''' <summary>
		''' Gets the database SQL identifier end quote character.
		''' </summary>
		''' <return>a string repersents the database SQL identifier end quote character.</return>

		Public Shared ReadOnly Property QuoteE As String
			Get
				Return "]"
			End Get
		End Property

		''' <summary>
		''' Gets the database type.
		''' </summary>
		''' <return>a enum value repersents the database type.</return>

		Public Shared Readonly Property DbType As Database.DbmsType
			Get
				Return Database.DbmsType.Access
			End Get
		End Property

		''' <summary>
		''' Returns a string of escaped SQL pharse.
		''' </summary>
		''' <return>a string of escaped SQL pharse.</return>

		Public Shared Function AdjustSql(ByVal value As Object) As String
			Dim sWrk As String = Convert.ToString(value).Replace("'", "''")

			'sWrk = sWrk.Replace("[", "[[]")
			Return sWrk
		End Function
		Dim Shared dbFactory As DbProviderFactory = Nothing
		Private Shared Sub LoadDbProviderFactory()
			dbFactory = Database.GetDbProviderFactory(ProviderName)
		End Sub

		''' <summary>
		''' Returns a new instance of the DbConnection class with the ConnStr property.
		''' </summary>
		''' <returns></returns>

		Public Shared Function GetConnection() As DbConnection 
			Try
				Return GetConnection(ConnStr)
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DbConnection class with the specified connection string.
		''' </summary>
		''' <param name="connectionString">connectionString: The connection used to open the database.</param>
		''' <returns></returns>

		Public Shared Function GetConnection(ByVal connectionString As String) As DbConnection 
			Try
				If (dbFactory Is Nothing) Then LoadDbProviderFactory()
				Dim conn As DbConnection = dbFactory.CreateConnection()
				conn.ConnectionString = connectionString
				Return conn
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DataAdapter class with an SQL SELECT statement and DbConnection.
		''' </summary>
		''' <param name="selectCommandText">A string that is a SQL SELECT statement or stored procedure to be used by the DataAdapter.SelectCommand property of the DataAdapter.</param>
		''' <param name="conn">An DbConnection that represents the connection.</param>
		''' <returns></returns>

		Public Shared Function GetDataAdapter(ByVal selectCommandText As String, ByRef conn As DbConnection) As DbDataAdapter 
			Try
				Return GetDataAdapter(selectCommandText, conn, Nothing)
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DataAdapter class with an SQL SELECT statement and DbConnection.
		''' </summary>
		''' <param name="selectCommandText">A string that is a SQL SELECT statement or stored procedure to be used by the DataAdapter.SelectCommand property of the DataAdapter.</param>
		''' <param name="conn">An DbConnection that represents the connection.</param>
		''' <returns></returns>

		Public Shared Function GetDataAdapter(ByVal selectCommandText As String, ByRef conn As DbConnection, ByRef parameters As DbParameterCollection) As DbDataAdapter 
			Try
				If (dbFactory Is Nothing) Then LoadDbProviderFactory()
				Dim da As DbDataAdapter = dbFactory.CreateDataAdapter()
				Dim command As DbCommand = GetCommand(selectCommandText, conn, parameters)
				da.SelectCommand = command
				Return da
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DbCommand class with the text of the query and a DbConnection using ConnStr.
		''' </summary>
		''' <param name="cmdText">The text of the query.</param>
		''' <returns></returns>

		Public Shared Function GetCommand(ByVal cmdText As String) As DbCommand 
			Try
				Return GetCommand(cmdText, GetConnection())
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DbCommand class with the text of the query and a DbConnection.
		''' </summary>
		''' <param name="cmdText">The text of the query.</param>
		''' <param name="conn">A DbConnection that represents the connection to a data source.</param>
		''' <returns></returns>

		Public Shared Function GetCommand(ByVal cmdText As String, ByRef conn As DbConnection) As DbCommand 
			Try
				Return GetCommand(cmdText, conn, Nothing)
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a new instance of the DbCommand class with the text of the query and a DbConnection.
		''' </summary>
		''' <param name="cmdText">The text of the query.</param>
		''' <param name="conn">A DbConnection that represents the connection to a data source.</param>
		''' <returns></returns>

		Public Shared Function GetCommand(ByVal cmdText As String, ByRef conn As DbConnection, ByRef parameters As DbParameterCollection) As DbCommand 
			Try
				If (dbFactory Is Nothing) Then LoadDbProviderFactory()
				Dim command As DbCommand = dbFactory.CreateCommand()
				command.CommandText = cmdText
				command.Connection = conn
				CopyParameters(command, parameters)
				Return command
			Catch
				Throw
			End Try
		End Function

		'
		''' <summary>
		''' Builds a DataReader with an SQL SELECT statement.
		''' </summary>
		''' <returns>A DbParameterCollection object</returns>

		Public Shared Function GetParameterCollection() As DbParameterCollection
			Try
				If (dbFactory Is Nothing) Then LoadDbProviderFactory()
				Return dbFactory.CreateCommand().Parameters
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Builds a DataReader with an SQL SELECT statement.
		''' </summary>
		''' <param name="selectCommandText">A string that is a SQL SELECT statement or stored procedure to be used by the DataAdapter.SelectCommand property of the DataAdapter.</param>
		''' <returns></returns>

		Public Shared Function GetDataReader(ByVal selectCommandText As String) As DbDataReader 
			Try
				Dim conn As DbConnection = GetConnection()
				Dim command As DbCommand = GetCommand(selectCommandText, conn)
				If (conn.State = ConnectionState.Closed) Then
					conn.Open()
				End If
				Return command.ExecuteReader(CommandBehavior.CloseConnection)
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Builds a DataReader with an SQL SELECT statement with an active connection.
		''' </summary>
		''' <param name="selectCommandText">A string that is a SQL SELECT statement or stored procedure to be used by the DataAdapter.SelectCommand property of the DataAdapter.</param>
		''' <param name="conn">An active DbConnection object.</param>
		''' <returns></returns>

		Public Shared Function GetDataReader(ByVal selectCommandText As String, ByRef conn As DbConnection) As DbDataReader 
			Try
				Return GetDataReader(selectCommandText, Nothing, conn)
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Builds a DataReader using an active connection with an SQL SELECT statement and parameters.
		''' </summary>
		''' <param name="selectCommandText">A string that is a SQL SELECT statement or stored procedure to be used by the DataAdapter.SelectCommand property of the DataAdapter.</param>
		''' <param name="conn">An active DbConnection object.</param>
		''' <param name="parameters">A DbParameterCollection object.</param>
		''' <returns></returns>

		Public Shared Function GetDataReader(ByVal selectCommandText As String, ByRef parameters As DbParameterCollection, ByRef conn As DbConnection) As DbDataReader 
			Try
				Dim command As DbCommand = GetCommand(selectCommandText, conn, parameters)
				If (conn.State = ConnectionState.Closed) Then
					conn.Open()
				End If
				Return command.ExecuteReader()
			Catch
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a DataView by given SQL Command
		''' </summary>
		''' <return>a DataView of given SQL Command.</return>

		Public Shared Function GetDataView(ByVal sqlCommand As String) As DataView
			Try 

				' Create a new Connection object
				Using oConn As DbConnection = GetConnection(ConnStr)

					' Create a new DataAdapter using the Connection object and SQL statement
					Using oDa As DbDataAdapter = GetDataAdapter(sqlCommand, oConn)

						' Create a new DataSet object to fill with Data
						Dim oDs As DataSet = New DataSet()

						' Fill the DataSet with Data from the DataAdapter object
						oDa.Fill(oDs, "ewDataSet")
						Return oDs.Tables(0).DefaultView
					End Using
					oConn.Close()
				End Using
			Catch e As OleDbException
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a DataView by given SQL Command and parameters collection
		''' </summary>
		''' <return>a DataView of given SQL Command and parameters collection.</return>

		Public Shared Function GetDataView(ByVal sqlCommand As String, ByRef parameters As DbParameterCollection) As DataView
			Try 

				' Create a new Connection object
				Using oConn As DbConnection = GetConnection(ConnStr)

					' Create a new DataAdapter using the Connection object and SQL statement
					Using oDa As DbDataAdapter = GetDataAdapter(sqlCommand, oConn, parameters)

						' Create a new DataSet object to fill with Data
						Dim oDs As DataSet = New DataSet()

						' Fill the DataSet with Data from the DataAdapter object
						oDa.Fill(oDs, "ewDataSet")
						oConn.Close()
						Return oDs.Tables(0).DefaultView
					End Using
				End Using
			Catch e As OleDbException
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a DataView by given SQL Command, page size and start row index
		''' </summary>
		''' <return>a DataView of given SQL Command.</return>

		Public Shared Function GetDataViewPage(ByVal sqlCommand As String, ByVal PageSize As Integer, ByVal StartRow As Integer) As DataView
			Try
				Return GetDataViewPage(sqlCommand, Nothing, PageSize, StartRow)
			Catch oErr As OleDbException
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a DataView by given SQL Command with parameters, page size and start row index
		''' </summary>
		''' <return>a DataView of given SQL Command.</return>

		Public Shared Function GetDataViewPage(ByVal sqlCommand As String, ByRef parameters As DbParameterCollection, ByVal PageSize As Integer, ByVal StartRow As Integer) As DataView
			Try

				' Create a new Connection Object using the Connection String
				Using oConn As DbConnection = GetConnection(ConnStr)

					' Create a new DataAdapter using the Connection Object and SQL statement
					Using oDa As DbDataAdapter = GetDataAdapter(sqlCommand, oConn, parameters)

						' Create a new DataSet object to fill with Data
						Dim oDs As DataSet = New DataSet()

						' Fill the DataSet with Data from the DataAdapter Object
						oDa.Fill(oDs, StartRow, PageSize, "ewDataSet")
						Return oDs.Tables(0).DefaultView
					End Using
				End Using
			Catch oErr As OleDbException
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a number of records by given SQL Command.
		''' </summary>
		''' <return>an integer of number of records.</return>

		Public Shared Function GetDataViewCount(ByVal sqlCommand As String) As Integer
			Try 
				Return GetDataViewCount(sqlCommand, Nothing)
			Catch e As Exception
				Throw
			End Try
		End Function

		''' <summary>
		''' Returns a number of records by given SQL Command with parameters.
		''' </summary>
		''' <return>an integer of number of records.</return>

		Public Shared Function GetDataViewCount(ByVal sqlCommand As String, ByRef parameters As DbParameterCollection) As Integer
			Try 
				Dim dv As DataView = Db.GetDataView(sqlCommand, parameters)
				If((dv Is Nothing) OrElse (dv.Count = 0)) Then Return 0 Else Return Convert.ToInt32(dv(0)(0))
			Catch e As Exception
				Throw
			End Try
		End Function

		''' <summary>
		''' Copy all the parameter object in the collection to the DbCommand.Parameters collection
		''' </summary>

		Private Shared Sub CopyParameters(ByVal command As DbCommand, ByRef parameters As DbParameterCollection)
			If (Not (parameters) Is Nothing AndAlso parameters.Count > 0) Then
				Dim parameterArray() As DbParameter = Array.CreateInstance(parameters(0).GetType(), parameters.Count)
				parameters.CopyTo(parameterArray, 0)
				parameters.Clear
				parameters = Nothing
				If ((parameterArray IsNot Nothing) AndAlso (parameterArray.Length > 0)) Then
					Dim i As Integer = 0
					Do While (i < parameterArray.Length)
						command.Parameters.Add(parameterArray(i))
						i = (i + 1)
					Loop
				End If
			End If
		End Sub

		''' <summary>
		''' Execute a SQL Command, return error message if any.
		''' </summary>
		''' <return>a string of error if there is problem on executing; otherwise, an empty string return.</return>

		Public Shared Function ExecuteNonQuery(ByVal sqlCommand As String) As String
			Dim rowCount As Integer
			Dim previousConnectionState As ConnectionState = ConnectionState.Closed
			Dim sErrorMsg As String = string.Empty
			Dim conn As DbConnection = Nothing
			Try
				conn = GetConnection(ConnStr)
				If (conn.State = ConnectionState.Closed) Then
					conn.Open
					previousConnectionState = conn.State
				End If
				Dim cmd As DbCommand = conn.CreateCommand
				cmd.CommandText = sqlCommand
				rowCount = cmd.ExecuteNonQuery
			Catch e As Exception
				sErrorMsg = e.Message
			Finally
				If (previousConnectionState <> ConnectionState.Closed) Then
					conn.Close
				End If
			End Try
			Return sErrorMsg
		End Function

		''' <summary>
		''' Return a DataView for Linking
		''' </summary>
		''' <return>a result DataView.</return>

		Public Shared Function DataViewLink(ByVal connStr As String, ByVal tableName As String, ByVal linkField As String, ByVal dispField As String, ByVal dispField2 As String, ByVal filterField As String, ByVal orderBy As String, ByVal orderType As String, ByVal isDistinct As Boolean, ByVal filter As String) As DataView
			Try
				Dim sSql As StringBuilder = New StringBuilder
				sSql.Append("SELECT")
				If isDistinct Then
					sSql.Append(" DISTINCT")
				End If
				sSql.Append(String.Concat(" ", QuoteS, linkField, QuoteE, ", ", QuoteS, dispField, QuoteE))
				If dispField2.Length > 0 Then
					sSql.Append(String.Concat(", ", QuoteS, dispField2, QuoteE))
				End If
				If filterField.Length > 0 Then
					sSql.Append(String.Concat(", ", QuoteS, filterField, QuoteE))
				End If
				sSql.Append(String.Concat(" FROM ", QuoteS, tableName, QuoteE))
				If filter.Length > 0 Then
					sSql.Append(" WHERE " & filter)
				End If
				If orderBy.Length > 0 Then
					sSql.Append(String.Concat(" ORDER BY ", QuoteS, orderBy, QuoteE, " ", orderType))
				End If

				' Create a new Connection object using the Connection String
				Using oConn As New OleDbConnection(connStr)

					' Create a new DataAdapter using the Connection object and SQL statement
					Using oDa As New OleDbDataAdapter(sSql.ToString(), oConn)

						' Create a new DataSet object to fill with Data
						Dim oDs As DataSet = New DataSet()

						' Fill the DataSet with Data from the DataAdapter object
						oDa.Fill(oDs, "ewDataSet")

						' Create the TextField and ValueField Columns
						oDs.Tables(0).Columns.Add("ewValueField", Type.GetType("System.String"), "[" & linkField & "]")
						If (dispField2.Length = 0) Then
							oDs.Tables(0).Columns.Add("ewTextField", Type.GetType("System.String"), "[" & dispField & "]")
						Else
							oDs.Tables(0).Columns.Add("ewTextField", Type.GetType("System.String"), "[" & dispField & "] + ', ' + [" & dispField2 & "]")
						End If 
						Return oDs.Tables(0).DefaultView
					End Using
				End Using
			Catch oErr As OleDbException
				Return Nothing
			End Try
		End Function

		''' <summary>
		''' Return a DataReader for Linking
		''' </summary>
		''' <return>a result DataReader.</return>

		Public Shared Function DataReaderLink(ByVal providerName As String, ByVal connStr As String, ByVal tableName As String, ByVal linkField As String, ByVal dispField As String, ByVal dispField2 As String, ByVal filterField As String, ByVal orderBy As String, ByVal orderType As String, ByVal isDistinct As Boolean, ByVal selectFilter As String, ByVal sourceFilter As String) As DbDataReader
			Dim sSql As StringBuilder = New StringBuilder
			sSql.Append("SELECT")
			If isDistinct Then
				sSql.Append(" DISTINCT")
			End If
			sSql.Append(" " + QuoteS + linkField + QuoteE + ", " + QuoteS + dispField + QuoteE)
			If dispField2.Length > 0 Then
				sSql.Append(", " + QuoteS + dispField2 + QuoteE)
			End If
			If filterField.Length > 0 Then
				sSql.Append(", " + QuoteS + filterField + QuoteE)
			End If
			sSql.Append(" FROM " + QuoteS + tableName + QuoteE)
			sSql.Append(" WHERE " + sourceFilter)
			If selectFilter.Length > 0 Then
				sSql.Append(" AND " + selectFilter)
			End If
			If orderBy.Length > 0 Then
				sSql.Append(" ORDER BY " + QuoteS + orderBy + QuoteE + " " + orderType)
			End If
			Dim dataSource As SqlDataSource = New SqlDataSource(providerName, connStr, sSql.ToString())
			dataSource.DataSourceMode = SqlDataSourceMode.DataReader
			Return TryCast(dataSource.Select(DataSourceSelectArguments.Empty), DbDataReader)
		End Function

		''' <summary>
		''' Return a DataReader for maximum auto number
		''' </summary>
		''' <return>a DataReader contains maximum auto number.</return>

		Public Shared Function DataReaderAutoNumber(ByVal providerName As String, ByVal connStr As String, ByVal tableName As String, ByVal autoNumberField As String) As DbDataReader
			Dim sSql As string = String.Concat("SELECT MAX(", QuoteS, autoNumberField, QuoteE, ") FROM ", QuoteS, tableName, QuoteE)
			Dim dataSource As SqlDataSource = New SqlDataSource(providerName, connStr, sSql)
			dataSource.DataSourceMode = SqlDataSourceMode.DataReader
			Return TryCast(dataSource.Select(DataSourceSelectArguments.Empty), DbDataReader)
		End Function

		''' <summary>
		''' Return Error Message based on Error object
		''' </summary>

		Public Shared Function DataErrorMessage(oErr As OleDbException) As String
			Dim sDbErrMsg As String = String.Empty
			Dim i As Integer = 0
			Do While (i  <= (oErr.Errors.Count - 1))
				sDbErrMsg += String.Concat("Message: ", oErr.Errors(i).Message, "<br />Native Error: ", oErr.Errors(i).NativeError, "<br />Source: ", oErr.Errors(i).Source, "<br />SQL State: ", oErr.Errors(i).SQLState, "<br />")
				i = (i + 1)
			Loop
			Return sDbErrMsg
		End Function

		''' <summary>
		''' Get Last insert auto-generated ID
		''' </summary>

		Public Shared Function LastInsertedID(conn As OleDbConnection) As Integer
			Using cmd As New OleDbCommand("SELECT @@IDENTITY", conn)
				Dim nId As Integer = Convert.ToInt32(cmd.ExecuteScalar())
				Return nId
			End Using
		End Function
	End Class
End Namespace
