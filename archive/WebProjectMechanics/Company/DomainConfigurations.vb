Imports System.IO
Imports System.Xml.Serialization

<Serializable()> Public Class DomainConfigurations
    Public Configuration As DomainConfiguration

    Public Sub New()
        Configuration = New DomainConfiguration
    End Sub

    Shared Function Load(ByVal fname As String) As DomainConfigurations
        Dim mySiteSettings As New DomainConfigurations
        If (fname.Trim) = String.Empty Then
            mySiteSettings = New DomainConfigurations
        Else
            If FileProcessing.IsValidPath(fname) Then
                Using sr As New StreamReader(fname)
                    Try
                        Dim xs As New XmlSerializer(GetType(DomainConfigurations))
                        mySiteSettings = DirectCast(xs.Deserialize(sr), DomainConfigurations)
                    Catch ex As Exception
                        ApplicationLogging.ConfigLog(String.Format("DomainConfigurations.Load error on Load: {0}", fname), ex.ToString)
                        mySiteSettings = New DomainConfigurations
                    End Try
                End Using
            Else
                mySiteSettings = New DomainConfigurations
            End If
        End If
        If CDbl(mySiteSettings.Configuration.CompanyID) = 0 Then
            ApplicationLogging.ConfigLog(String.Format("DomainConfigurations.Load error on Load: {0}", fname), "CompanyID is 0")
        End If
        Return mySiteSettings
    End Function

    Shared Sub Save(ByVal fname As String, ByVal obj As DomainConfigurations)
        Try
            Using sw As New StreamWriter(fname)
                Dim xs As New XmlSerializer(GetType(DomainConfigurations))
                xs.Serialize(sw, obj)
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("DomainConfigurations.Save", ex.ToString)
        End Try
    End Sub
End Class
