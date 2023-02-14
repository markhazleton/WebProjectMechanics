<%@ WebHandler Language="VB" Class="ExportCollection" %>

Imports System
Imports System.Web
Imports WebProjectMechanics
Public Class ExportCollection : Implements IHttpHandler, IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If IsAdmin(context) Then
            ExportToCSV()
        Else
            context.Response.Redirect("/")
        End If
    End Sub

    Private Function IsAdmin(ByVal context As HttpContext) As Boolean
        If CStr(context.Session.Item("wpm_UserGroupID")) = "1" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Sub ExportToCSV()
        Using mycon As New wpmMineralCollection.DataController()
            Dim returnSB As New StringBuilder(String.Empty)
            Dim myValues As New StringBuilder(String.Empty)
            Dim columnNames As New List(Of String)
            Dim iRowNum As Integer = 0
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=MineralCollection.csv")
            HttpContext.Current.Response.ContentType = "text/csv"
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            For Each objectItem In mycon.CollectionItems
                If columnNames.Count = 0 Then
                    columnNames = GetColumnNames(objectItem)
                    columnNames.Add("Photos")
                    columnNames.Add("Minerals")

                    returnSB.Append(wpm_ClearLineFeeds("""Row"""))
                    For Each colName In columnNames
                        returnSB.Append(AddComma(colName))
                    Next
                    iRowNum = 1
                    returnSB.Append(Environment.NewLine)
                    returnSB.Append(wpm_ClearLineFeeds(String.Format("""{0}""", iRowNum)))


                End If


                For Each colName In columnNames
                    myValues.Clear()
                    Select Case colName
                        Case "Photos"
                            For Each myPhoto As wpmMineralCollection.CollectionItemImage In objectItem.CollectionItemImages
                                myValues.Append(myPhoto.ImageFileNM & "~")
                            Next
                            returnSB.Append(AddComma(myValues.ToString))
                            myValues.Clear()
                        Case "Minerals"
                            For Each myMineral As wpmMineralCollection.CollectionItemMineral In objectItem.CollectionItemMinerals
                                myValues.Append(myMineral.Mineral.MineralNM & "~")
                            Next
                            returnSB.Append(AddComma(myValues.ToString))
                            myValues.Clear()
                        Case "PrimaryMineralID"
                            returnSB.Append(AddComma(objectItem.Mineral.MineralNM & " (" & objectItem.PrimaryMineralID & ")"))
                        Case "CollectionID"
                            returnSB.Append(AddComma(objectItem.Collection.CollectionNM))
                        Case "PurchasedFromCompanyID"
                            If objectItem.PurchasedFromCompanyID Is Nothing Then
                                returnSB.Append(AddComma(""))
                            Else
                                returnSB.Append(AddComma(objectItem.Company.CompanyNM))
                            End If
                        Case "LocationCityID"
                            If objectItem.LocationCityID Is Nothing Then
                                returnSB.Append(AddComma(""))
                            Else
                                returnSB.Append(AddComma(objectItem.LocationCity.City & " (" & objectItem.LocationCityID & ")"))
                            End If
                        Case "LocationStateID"
                            If objectItem.LocationStateID Is Nothing Then
                                returnSB.Append(AddComma(""))
                            Else
                                returnSB.Append(AddComma(objectItem.LocationState.StateNM & " (" & objectItem.LocationStateID & ")"))
                            End If
                        Case "LocationCountryID"
                            If objectItem.LocationCountryID Is Nothing Then
                                returnSB.Append(AddComma(""))
                            Else
                                returnSB.Append(AddComma(objectItem.LocationCountry.CountryNM & " (" & objectItem.LocationCountryID & ")"))
                            End If
                        Case Else
                            returnSB.Append(AddComma(GetProperty(objectItem, colName)))
                    End Select
                Next
                returnSB.Append(Environment.NewLine)
                iRowNum = iRowNum + 1
                returnSB.Append(wpm_ClearLineFeeds(String.Format("""{0}""", iRowNum)))

            Next
            HttpContext.Current.Response.Write(returnSB.ToString())
        End Using
    End Sub


    Private Function WriteColumnName(ByRef ColumnNames As List(Of String)) As String
        Dim returnSB As New StringBuilder(String.Empty)
        For Each colName In ColumnNames
            returnSB.Append(AddComma(colName))
        Next
        returnSB.Append(Environment.NewLine)
        Return returnSB.ToString()
    End Function

    Private Function GetColumnNames(ByVal ObjectItem As Object) As List(Of String)
        Dim myColumns As New List(Of String)

        For Each myProperty In ObjectItem.GetType.GetProperties
            If Not myProperty.PropertyType.Name.Contains("EntitySet") AndAlso Not myProperty.PropertyType.Name.Contains("EntityRef") Then
                If IsGoodProperty(myProperty.Name) Then
                    myColumns.Add(myProperty.Name)
                End If
            End If
        Next
        Return myColumns
    End Function
    Private Function AddComma(value As String) As String
        Dim mySB As New StringBuilder
        mySB.Append(", ")
        If value.Contains("'"c) Or value.Contains(""""c) Or value.Contains(",") Then
            mySB.Append(wpm_ClearLineFeeds(String.Format("""{0}""", value.Replace(","c, " "c).Replace(""""c, "'"c)).Replace(vbCrLf, " ").Replace(vbCr, " ")))
        Else
            mySB.Append(wpm_ClearLineFeeds(String.Format("{0}", value.Replace(","c, " "c).Replace(""""c, "'"c)).Replace(vbCrLf, " ").Replace(vbCr, " ")))
        End If
        Return mySB.ToString
    End Function
    Private Function GetItemRow(ByRef thisItem As wpmMineralCollection.CollectionItem, ByRef ColumnNames As List(Of String)) As String
        Dim returnSB As New StringBuilder()
        Dim myPhotos As New StringBuilder(String.Empty)
        Dim myMinerals As New StringBuilder(String.Empty)

        For Each colName In ColumnNames
            Select Case colName
                Case "Photos"
                    For Each myPhoto As wpmMineralCollection.CollectionItemImage In thisItem.CollectionItemImages
                        myPhotos.Append(myPhoto.ImageFileNM & "~")
                    Next
                    returnSB.Append(AddComma(myPhotos.ToString))
                Case "Minerals"
                    For Each myMineral As wpmMineralCollection.CollectionItemMineral In thisItem.CollectionItemMinerals
                        myMinerals.Append(myMineral.Mineral.MineralNM & "~")
                    Next

                Case Else
                    returnSB.Append(AddComma(GetProperty(thisItem, colName)))

            End Select
        Next
        Return returnSB.ToString()
    End Function

    Public Overloads Function GetProperty(ByRef objectItem As wpmMineralCollection.CollectionItem, ByVal PropertyName As String) As String
        Dim myValue As New Object
        Try
            For Each myProperty As System.Reflection.PropertyInfo In objectItem.GetType.GetProperties
                If myProperty.Name = PropertyName Then

                    myValue = myProperty.GetValue(objectItem, Nothing)
                    Exit For
                ElseIf wpm_CheckForMatch(myProperty.Name, PropertyName) Then
                    myValue = myProperty.GetValue(objectItem, Nothing)
                    Exit For
                End If
            Next
        Catch
            myValue = String.Empty

        End Try
        Try
            If String.IsNullOrEmpty(CStr(myValue)) Then
                myValue = String.Empty
            End If
        Catch ex As Exception
            myValue = String.Empty
        End Try
        Return myValue.ToString()
    End Function


    Private Function IsGoodProperty(ByVal PropertyName As String) As Boolean
        Dim bReturn As Boolean = True

        Select Case PropertyName
            Case "CollectionItem"
                bReturn = False
            Case "CollectionItemImage"
                bReturn = False
            Case "CollectionItemImages"
                bReturn = False
            Case "CollectionItemMineral"
                bReturn = False
            Case "CollectionItemMinerals"
                bReturn = False
            Case "Mineral"
                bReturn = False
            Case "Collection"
                bReturn = False
            Case "Company"
                bReturn = False
            Case "LocationCity"
                bReturn = False
            Case "LocationState"
                bReturn = False
            Case "LocationCountry"
                bReturn = False
            Case Else
                bReturn = True
        End Select
        Return bReturn
    End Function

End Class