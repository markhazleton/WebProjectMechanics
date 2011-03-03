Imports System.Data.OleDb
Public Class wpmDB
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
        Using dataTable As New DataTable
            Using RecConn As New OleDbConnection() With {.ConnectionString = wpmApp.ConnStr}
                Try
                    Using myCommand As New OleDbCommand(sSQL, RecConn)
                        Using dataAdapter As New OleDbDataAdapter(myCommand)
                            dataAdapter.Fill(dataTable)
                        End Using
                    End Using
                Catch ex As Exception
                    wpmLogging.SQLAudit(sSQL, String.Format("Error on wpmDB.GetDataTable - {0} ({1})", sTableName, ex.Message))
                End Try
            End Using
            Return dataTable
        End Using
    End Function
    Public Shared Function RunInsertSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = wpmApp.ConnStr
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                wpmLogging.SQLAudit(sSQL, "RunInsertSQL - " & sTableName)
            End Try
        End Using
        Return iRowsAffected
    End Function

    Public Shared Function RunUpdateSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = wpmApp.ConnStr
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                wpmLogging.SQLAudit(sTableName & " - error on update", sSQL)
            End Try
        End Using
        Return iRowsAffected
    End Function
    Public Shared Function RunDeleteSQL(ByVal sSQL As String, ByVal sTableName As String) As Integer
        Dim iRowsAffected As Integer = 0
        Dim sConnStr As String = wpmApp.ConnStr
        Using RecConn As New OleDbConnection
            Try
                RecConn.ConnectionString = sConnStr
                RecConn.Open()
                Using command As OleDbCommand = New OleDbCommand(sSQL, RecConn)
                    iRowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                Call wpmLogging.SQLAudit(String.Format("{0} - ERRROR ON DELETE - {1}", sTableName, sSQL), ex.ToString)
            End Try
        End Using
        Return iRowsAffected
    End Function
End Class