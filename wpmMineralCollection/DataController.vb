﻿Public Class DataController
    Inherits DataClasses1DataContext
    Public Property ReturnValue As String

    Public Sub New()
        MyBase.New("Data Source=Data Source=c:\\websites\\MineralCollection.db")
    End Sub

    Public Sub New(sConnectionStr As String)
        MyBase.New(sConnectionStr)
    End Sub
    Public Sub New(ByVal connection As IDbConnection)
        MyBase.New(connection)
    End Sub
    Public Sub New(ByVal connection As String, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
        MyBase.New(connection, mappingSource)
    End Sub
    Public Sub New(ByVal connection As IDbConnection, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
        MyBase.New(connection, mappingSource)
    End Sub
    Public Overloads Sub SubmitChanges()
        ReturnValue = String.Empty
        Try
            MyBase.SubmitChanges()
        Catch ex As Exception
            WebProjectMechanics.ApplicationLogging.SQLExceptionLog("DataController.SubmitChanges", ex)
            ReturnValue = $"ERROR ON SUBMIT: {ex.Message}"
        End Try
    End Sub
End Class




