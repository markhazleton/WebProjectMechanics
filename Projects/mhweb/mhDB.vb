Public Class mhDB
    Const adOpenForwardOnly As Short = 0
    Const adOpenKeyset As Short = 1
    Const adOpenDynamic As Short = 2
    Const adOpenStatic As Short = 3
    Const adLockReadOnly As Short = 1
    Const adLockPessimistic As Short = 2
    Const adLockOptimistic As Short = 3
    Const adLockBatchOptimistic As Short = 4
    Const adUseServer As Short = 2
    Const adUseClient As Short = 3
    Const adForAppending As Short = 8
    Private _DataErrorMessage As String

    Public Sub New()
        MyBase.New()
    End Sub
    Public ReadOnly Property GetDataErrorMessage() As String
        Get
            Return _DataErrorMessage
        End Get
    End Property

    Public Shared Function GetDataTable(ByVal sSQL As String, ByVal sTableName As String) As DataTable
        Dim dataTable As New DataTable
        Try
            Dim sConnStr As String = mhConfig.ConnStr
            Dim RecConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
            RecConn.ConnectionString = sConnStr
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand(sSQL, RecConn)
            Dim dataAdapter As System.Data.OleDb.OleDbDataAdapter = New System.Data.OleDb.OleDbDataAdapter(command)
            dataAdapter.Fill(dataTable)
        Catch ex As Exception
            mhUTIL.SQLAudit(sSQL, "Error on mhDB.GetDataTable - " & sTableName & " (" & ex.Message & ")")
            mhUTIL.AuditLog("Error on mhDB.GetDataTable - " & sTableName, ex.ToString)
        End Try
        Return dataTable
    End Function
    Public Shared Function RunInsertSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Try
            Dim sConnStr As String = mhConfig.ConnStr
            Dim RecConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
            RecConn.ConnectionString = sConnStr
            RecConn.Open()
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand(sSQL, RecConn)
            iRowsAffected = command.ExecuteNonQuery()
            RecConn.Close()
            RecConn = Nothing
        Catch ex As Exception
            mhUTIL.SQLAudit(sSQL, "RunInsertSQL - " & sTableName)
            mhUTIL.AuditLog("ERROR ON INSERT", ex.ToString)
        End Try
        Return iRowsAffected
    End Function

    Public Shared Function RunUpdateSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Try
            Dim sConnStr As String = mhConfig.ConnStr
            Dim RecConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
            RecConn.ConnectionString = sConnStr
            RecConn.Open()
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand(sSQL, RecConn)
            iRowsAffected = command.ExecuteNonQuery()
            RecConn.Close()
            RecConn = Nothing
        Catch ex As Exception
            mhUTIL.SQLAudit(sTableName & " - error on update", sSQL)
            mhUTIL.AuditLog("ERROR ON UPDATE", ex.ToString)
        End Try
        Return iRowsAffected
    End Function
    Public Shared Function RunDeleteSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Try
            Dim sConnStr As String = mhConfig.ConnStr
            Dim RecConn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection
            RecConn.ConnectionString = sConnStr
            RecConn.Open()
            Dim command As System.Data.OleDb.OleDbCommand = New System.Data.OleDb.OleDbCommand(sSQL, RecConn)
            iRowsAffected = command.ExecuteNonQuery()
            RecConn.Close()
            RecConn = Nothing
        Catch ex As Exception
            Call mhUTIL.SQLAudit(sTableName & " - ERRROR ON DELETE - " & sSQL, ex.ToString)
        End Try
        Return iRowsAffected
    End Function
End Class