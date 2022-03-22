Imports System.Data.OleDb
Imports System.Web.UI.WebControls

Public Module UtilityDB
    Public Function wpm_GetNullableIndex(ByRef sIndex As String) As String
        If String.IsNullOrEmpty(sIndex) Then
            Return "Null"
        Else
            Return sIndex
        End If
    End Function
    Public Function wpm_GetBoolean(ByRef bValue As Boolean) As String
        If (bValue) Then
            Return "True"
        Else
            Return "False"
        End If
    End Function
    Public Sub wpm_AddParameterStringValue(ByVal ParameterName As String, ByVal ParameterValue As String, ByRef sqlcmd As OleDbCommand)
        If ParameterValue Is Nothing OrElse ParameterValue.Trim.Length = 0 Then
            sqlcmd.Parameters.AddWithValue(ParameterName, DBNull.Value)
        Else
            sqlcmd.Parameters.AddWithValue(ParameterName, ParameterValue.Trim)
        End If
    End Sub
    Public Sub wpm_AddParameterValue(ByVal ParameterName As String, ByVal ParameterValue As Object, ByVal parmDataType As SqlDbType, ByRef sqlcmd As OleDbCommand)
        If parmDataType = SqlDbType.DateTime Then
            If CDate(ParameterValue) = DateTime.MinValue Then
                sqlcmd.Parameters.AddWithValue(ParameterName, DBNull.Value)
            Else
                sqlcmd.Parameters.AddWithValue(ParameterName, wpm_GetDateTimeWithoutMilliseconds(CType(ParameterValue, DateTime)))
            End If
        ElseIf parmDataType = SqlDbType.Int Or parmDataType = SqlDbType.BigInt Then
            If IsNumeric(ParameterValue) AndAlso CInt(ParameterValue) > 0 Then
                sqlcmd.Parameters.AddWithValue(ParameterName, ParameterValue)
            Else
                sqlcmd.Parameters.AddWithValue(ParameterName, DBNull.Value)
            End If
        ElseIf parmDataType = SqlDbType.Text Then
            If ParameterValue Is Nothing OrElse ParameterValue.ToString.Trim.Length = 0 Then
                sqlcmd.Parameters.AddWithValue(ParameterName, DBNull.Value)
            Else
                sqlcmd.Parameters.AddWithValue(ParameterName, ParameterValue.ToString.Trim)
            End If
        Else
            If ParameterValue Is Nothing Then
                sqlcmd.Parameters.AddWithValue(ParameterName, DBNull.Value)
            Else
                sqlcmd.Parameters.AddWithValue(ParameterName, ParameterValue)
            End If
        End If

    End Sub
    Public Function wpm_GetDateTimeWithoutMilliseconds(ByVal dtmDate As DateTime) As DateTime
        Try
            Dim dtmReturnDate As DateTime = New DateTime(dtmDate.Year, _
                                        dtmDate.Month, _
                                        dtmDate.Day, _
                                        dtmDate.Hour, _
                                        dtmDate.Minute, _
                                        dtmDate.Second)
            Return dtmReturnDate
        Catch ex As Exception
            ApplicationLogging.ErrorLog("UtilityDB.GetDateTimeWithoutMilliseconds", ex.Message)
            Return New DateTime(DateTime.Now.Year, _
                                        DateTime.Now.Month, _
                                        DateTime.Now.Day, _
                                        DateTime.Now.Hour, _
                                        DateTime.Now.Minute, _
                                        DateTime.Now.Second)
        End Try
    End Function
    Public Function wpm_GetDataTable(ByVal sSQL As String, ByVal sTableName As String) As DataTable
        Using dataTable As New DataTable
            Using RecConn As New OleDbConnection() With {.ConnectionString = wpm_SQLDBConnString}
                Try
                    RecConn.Open()
                    Using myCommand As New OleDbCommand(sSQL, RecConn)
                        Dim myDR As OleDbDataReader = myCommand.ExecuteReader
                        dataTable.Load(myDR)
                    End Using
                    RecConn.Close()
                Catch ex As Exception
                    ApplicationLogging.SQLSelectError(sSQL, String.Format("Error on UtilityDB.wpm_GetDataTable - {0} ({1})", sTableName, ex.Message))
                    wpm_AddGenericError(String.Format("Error on UtilityDB.wpm_GetDataTable - {0} ({1})", sTableName, ex.Message))
                End Try
            End Using
            Return dataTable
        End Using
    End Function
    Public Function wpm_RunInsertSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = wpm_SQLDBConnString
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLInsertError(sSQL, String.Format("Error on UtilityDB.wpm_RunInsertSQL - {0} ({1})", sTableName, ex.Message))
                wpm_AddGenericError(String.Format("Error on UtilityDB.wpm_RunInsertSQL - {0} ({1})", sTableName, ex.Message))
            End Try
        End Using
        Return iRowsAffected
    End Function
    Public Function wpm_RunUpdateSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = wpm_SQLDBConnString
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLUpdateError(sSQL, String.Format("Error on UtilityDB.wpm_RunUpdateSQL - {0} ({1})", sTableName, ex.Message))
                wpm_AddGenericError(String.Format("Error on UtilityDB.wpm_RunUpdateSQL - {0} ({1})", sTableName, ex.Message))
            End Try
        End Using
        Return iRowsAffected
    End Function
    Public Function wpm_RunDeleteSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Dim sConnStr As String = wpm_SQLDBConnString
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = sConnStr
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                ApplicationLogging.SQLDeleteError(sSQL, String.Format("Error on UtilityDB.wpm_RunDeleteSQL - {0} ({1})", sTableName, ex.Message))
                wpm_AddGenericError(String.Format("Error on UtilityDB.wpm_RunDeleteSQL - {0} ({1})", sTableName, ex.Message))
            End Try
        End Using
        Return iRowsAffected
    End Function

    Public Function wpm_LoadCMB(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByVal LoadSQL As String, ByVal TextColumn As String, ByVal ValColumn As String, ByVal bRequired As Boolean) As Boolean
        myCMB.Items.Clear()
        myCMB.Enabled = True
        Using myDataTable As DataTable = wpm_GetDataTable(LoadSQL, String.Format("{0}", "LoadCMB"))
            If myDataTable.Rows.Count > 1 Then
                If Not bRequired Then
                    myCMB.Items.Add(New ListItem() With {.Value = String.Empty, .Text = "Please Select", .Selected = True})
                End If
                For Each myRow As DataRow In myDataTable.Rows
                    myCMB.Items.Add(New ListItem() With {.Text = wpm_GetDBString(myRow.Item(TextColumn)), .Value = wpm_GetDBString(myRow.Item(ValColumn)), .Selected = False})
                Next
                myCMB.SelectedValue = CurrentValue
            Else
                If myDataTable.Rows.Count = 1 Then
                    myCMB.Items.Add(New ListItem() With {.Text = wpm_GetDBString(myDataTable.Rows(0).Item(TextColumn)), .Value = wpm_GetDBString(myDataTable.Rows(0).Item(ValColumn)), .Selected = True})
                    myCMB.Enabled = False
                End If
            End If
        End Using
        Return True
    End Function

End Module


