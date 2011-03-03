Imports System.IO
Imports System.Xml.Serialization

<Serializable()> Public Class wpmSiteSettings
    Public mySite As wpmSite

    Public Sub New()
        mySite = New wpmSite
    End Sub

    Shared Function Load(ByVal fname As String) As wpmSiteSettings
        If (fname.Trim) = String.Empty Then
            Return New wpmSiteSettings
        Else
            If wpmFileProcessing.IsValidPath(fname) Then
                Using sr As New StreamReader(fname)
                    Try
                        Dim xs As New XmlSerializer(GetType(wpmSiteSettings))
                        Return DirectCast(xs.Deserialize(sr), wpmSiteSettings)
                    Catch ex As Exception
                        wpmLogging.ErrorLog("SiteSettings error on Load", ex.ToString)
                        Return New wpmSiteSettings
                    End Try
                End Using
            Else
                Return New wpmSiteSettings
            End If
        End If
    End Function

    Shared Sub Save(ByVal fname As String, ByVal obj As wpmSiteSettings)
        Using sw As New StreamWriter(fname)
            Try
                Dim xs As New XmlSerializer(GetType(wpmSiteSettings))
                xs.Serialize(sw, obj)
            Catch ex As Exception
                wpmLogging.ErrorLog("SiteSettings on Save", ex.ToString)
            End Try
        End Using
    End Sub
End Class

