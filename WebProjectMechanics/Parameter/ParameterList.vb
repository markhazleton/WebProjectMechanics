Imports System.Text

Public Class ParameterList
    Inherits List(Of Parameter)
    Public Sub New()

    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of Parameter))
        MyBase.New(collection)

    End Sub


    Private Function LoadParameterList_Site(ByVal CompanyID As String) As Boolean
        Try
            For Each myrow As DataRow In ApplicationDAL.GetSiteParameterList(CompanyID).Rows
                Add(New Parameter() With {.RecordSource = wpm_GetDBString(myrow("RecordSource")),
                                          .CompanySiteParameterID = wpm_GetDBInteger(myrow("CompanySiteParameterID"), 0),
                                          .SortOrder = wpm_GetDBInteger(myrow("SortOrder")),
                                          .CompanyID = wpm_GetDBString(myrow("CompanyID")),
                                          .CompanyNM = wpm_GetDBString(myrow("CompanyName")),
                                          .ParameterValue = wpm_GetDBString(myrow("ParameterValue")),
                                          .ParameterTypeID = wpm_GetDBInteger(myrow("SiteParameterTypeID"), 0),
                                          .ParameterTypeNM = wpm_GetDBString(myrow("SiteParameterTypeNM")),
                                          .ParameterTypeDS = wpm_GetDBString(myrow("SiteParameterTypeDS")),
                                          .LocationID = wpm_GetDBString(myrow("PageID")),
                                          .LocationNM = wpm_GetDBString(myrow("PageName")),
                                          .LocationGroupID = wpm_GetDBString(myrow("SiteCategoryGroupID")),
                                          .LocationGroupNM = wpm_GetDBString(myrow("SiteCategoryGroupNM")),
                                          .ParameterNM = String.Format("{0}-{1}-{2}",
                                                                       wpm_GetDBString(myrow("SiteParameterTypeNM")),
                                                                       wpm_GetDBString(myrow("CompanyName")),
                                                                       wpm_GetDBString(myrow("PageName"))),
                                          .ParameterID = String.Format("SP-{0}", wpm_GetDBInteger(myrow("CompanySiteParameterID"), 0))})
            Next
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Error on ParameterList.LoadParameterList_Site", ex.ToString)
        End Try
        Return True
    End Function
    Private Function LoadParameterList_Default() As Boolean
        Dim bReturn As Boolean = False
        Try
            For Each myrow As DataRow In ApplicationDAL.GetParameterTypeList().Rows
                Dim mySiteParameter As New Parameter() With {.RecordSource = wpm_GetDBString(myrow("RecordSource")),
                                                             .CompanySiteParameterID = wpm_GetDBInteger(myrow("CompanySiteParameterID"), 0),
                                                             .SortOrder = wpm_GetDBInteger(myrow("PrimarySort")),
                                                             .CompanyID = String.Empty,
                                                             .CompanyNM = String.Empty,
                                                             .ParameterValue = wpm_GetDBString(myrow("SiteParameterTemplate")),
                                                             .ParameterTypeID = wpm_GetDBInteger(myrow("SiteParameterTypeID"), 0),
                                                             .ParameterTypeNM = wpm_GetDBString(myrow("SiteParameterTypeNM")),
                                                             .ParameterTypeDS = wpm_GetDBString(myrow("SiteParameterTypeDS")),
                                                             .LocationID = String.Empty,
                                                             .LocationNM = String.Empty,
                                                             .LocationGroupID = String.Empty,
                                                             .LocationGroupNM = String.Empty,
                                                             .ParameterNM = wpm_GetDBString(myrow("SiteParameterTypeNM")),
                                                             .ParameterID = String.Format("PT-{0}", wpm_GetDBInteger(myrow("SiteParameterTypeID"), 0))}
                Add(mySiteParameter)
            Next
            bReturn = True
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Error on PopulateParameterTypeList.GetParameterTypeList", ex.ToString)
        End Try
        Return bReturn
    End Function

    Public Function LoadParameterList(ByVal CompanyID As String) As Boolean
        LoadParameterList_Site(CompanyID)
        LoadParameterList_Default()
        Return True
    End Function

    Public Function ReplaceTags(ByRef sbContent As StringBuilder, ByRef mySiteMapRow As Location) As Boolean
        ' Find All Parameters that apply for a given SiteMapRow
        For Each mySiteParameter As Parameter In Me
            If (mySiteMapRow.LocationGroupID = mySiteParameter.LocationGroupID Or mySiteParameter.LocationGroupID = String.Empty) Then
                If (mySiteMapRow.LocationID = mySiteParameter.LocationID) Then
                    sbContent.Replace(String.Format("~~{0}~~", mySiteParameter.ParameterTypeNM), mySiteParameter.ParameterValue)
                End If
            End If
        Next
        For Each mySiteParameter As Parameter In Me
            If (mySiteMapRow.LocationGroupID = mySiteParameter.LocationGroupID Or mySiteParameter.LocationGroupID = String.Empty) Then
                If (mySiteMapRow.LocationID = mySiteParameter.LocationID Or mySiteParameter.LocationID = String.Empty) Then
                    sbContent.Replace(String.Format("~~{0}~~", mySiteParameter.ParameterTypeNM), mySiteParameter.ParameterValue)
                End If
            End If
        Next
        Return True
    End Function
End Class

