Imports System.IO
Imports System.Xml.Serialization

Public Class PivotParameterList
    Inherits List(Of PivotParameter)
    Public Function GetParameterString(ByVal CSVName As String) As String
        Try
            Return ((From i In Me Where i.CSVFile = CSVName).SingleOrDefault)?.GetPivotParm
        Catch ex As Exception
            Return "derivedAttributes: {},"
        End Try
    End Function
    Public Function AddToList(ByVal myPivotParameter As PivotParameter) As Boolean
        If CanAdd(myPivotParameter) Then
            Me.Add(myPivotParameter)
        Else
            Me.RemoveAt(IndexOf(myPivotParameter))
            Me.Add(myPivotParameter)
        End If
        Return True
    End Function

    Public Overloads Function IndexOf(ByVal item As PivotParameter) As Integer
        Dim myParm = MyBase.Where(Function(c) c.CSVFile = item.CSVFile).FirstOrDefault
        Return MyBase.IndexOf(myParm)
    End Function

    Public Function SaveXML(ByVal sFilePath As String) As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New StreamWriter(sFilePath, False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(PivotParameterList))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Error Saving File - {0}", ex), "PivotParameterList.SaveXML")
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error Before Saving File  - {0}", ex), "PivotParameterList.SaveXML")
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function


    Public Function GetXML(ByVal sPath As String) As PivotParameterList
        Dim mySurvey As New PivotParameterList
        Dim x As New XmlSerializer(GetType(PivotParameterList))
        Try
            Using objStreamReader As New StreamReader(sPath)
                mySurvey = CType(x.Deserialize(objStreamReader), PivotParameterList)
                objStreamReader.Close()
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("PivotParameterList.GetXML", ex.ToString)
        End Try
        Return mySurvey
    End Function


    Protected Overridable Function CanAdd(ByVal newParm As PivotParameter) As Boolean
        Return Not Me.Contains(newParm, New PivotParameterEqualityComparer)
    End Function

End Class
