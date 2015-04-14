Imports System.Xml

Public Class ConfigurationBase
    Private ReadOnly myDoc As XmlDocument
    Private Property ConfigFilePath() As String
    Public Sub New(ByVal myConfigFilePath As String)
        If myConfigFilePath Is Nothing Then
            myConfigFilePath = "~/App_Data/wpm_configfile.xml"
        End If
        myDoc = New XmlDocument()
        Try
            ConfigFilePath = HttpContext.Current.Server.MapPath(myConfigFilePath)
        Catch ex As Exception
            ApplicationLogging.ConfigLog("Can't map path to config file: " & myConfigFilePath, ex.ToString)
        End Try

        Try
            myDoc.Load(ConfigFilePath)
        Catch
            Try
                MakeXML(ConfigFilePath)
                myDoc.Load(ConfigFilePath)
            Catch ex As Exception
                ApplicationLogging.ConfigLog("Can't Create Config File: " & ConfigFilePath, ex.ToString)
            End Try
        End Try
    End Sub
    Public Sub New()
        ConfigFilePath = HttpContext.Current.Server.MapPath("~/App_Data/wpm_configfile.xml")
    End Sub

    Private Function GetConfigValue(ByVal ConfigName As String) As String
        If myDoc IsNot Nothing Then
            Try
                Return myDoc.GetElementsByTagName(ConfigName).Item(0).ChildNodes.Item(0).Value
            Catch
                Return String.Empty
            End Try
        Else
            Return String.Empty
        End If
    End Function
    Public Function GetConfigValue(ByVal ConfigName As String, ByVal ConfigValaue As String) As String
        Dim myReturn As String = GetConfigValue(ConfigName)
        If myReturn = String.Empty Then
            myReturn = ConfigValaue
        End If
        HttpContext.Current.Application(ConfigName) = myReturn
        Return myReturn
    End Function
    Public Sub SetConfigValue(ByVal ConfigName As String, ByVal ConfigValue As String)
        If wpm_CheckForMatch(GetConfigValue(ConfigName), ConfigValue) Then
            Exit Sub
        Else
            If myDoc IsNot Nothing Then
                Dim myElement As XmlElement
                Dim myRoot As XmlElement = myDoc.DocumentElement
                Dim myConfig As XmlNodeList = myDoc.GetElementsByTagName(ConfigName)
                Try
                    If myConfig.Count = 0 Then
                        myElement = myDoc.CreateElement(ConfigName)
                        myElement.InnerText = ConfigValue
                        myRoot.AppendChild(myElement)
                    Else
                        myConfig.Item(0).InnerText = ConfigValue
                    End If
                    ApplicationLogging.AuditLog("Change Site Config:" & ConfigName, ConfigValue)
                    myDoc.Save(HttpContext.Current.Server.MapPath("~/app_data/wpm_configfile_NEW.xml"))
                    HttpContext.Current.Application(ConfigName) = ConfigValue
                Catch ex As Exception
                    ApplicationLogging.ErrorLog("ErrorSetting Value: " & ConfigFilePath, ex.ToString)
                End Try
            End If
        End If
    End Sub

    Private Shared Function MakeXML(ByVal myFileName As String) As Boolean
        Dim xmlDoc As New XmlDocument()
        Dim xmlDeclaration As XmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)
        Dim rootNode As XmlElement = xmlDoc.CreateElement("Configuration")
        xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement)
        xmlDoc.AppendChild(rootNode)
        xmlDoc.Save(myFileName)
        Return True
    End Function
End Class
