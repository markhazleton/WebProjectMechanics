Public Class PartList
    Inherits List(Of Part)


    Public Function GetSiteParts(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each PartDataRow As DataRow In ApplicationDAL.GetSiteLinks(CompanyID).Rows
                AddPart(PartDataRow)
            Next
        Catch ex As Exception
            bReturn = False
            ApplicationLogging.ErrorLog("ERROR ON PartList.PopulateSiteLinkRows-Cateogry()", ex.ToString)
        End Try
        Return bReturn
    End Function

    Private Sub AddPart(ByVal PartDataRow As DataRow)
        Me.Add(New Part() With {.PartID = wpm_GetDBString(PartDataRow("PartID")),
                                .PartTypeCD = wpm_GetDBString(PartDataRow("PartTypeCD")),
                                .PartCategoryTitle = wpm_GetDBString(PartDataRow("PartCategoryTitle")),
                                .PartCategoryID = wpm_GetDBString(PartDataRow("PartCategoryID")),
                                .LocationID = wpm_GetDBString(PartDataRow("LocationID")),
                                .LocationNM = wpm_GetDBString(PartDataRow("LocationNM")),
                                .Title = wpm_GetDBString(PartDataRow("Title")),
                                .Description = wpm_GetDBString(PartDataRow("Description")),
                                .URL = wpm_GetDBString(PartDataRow("URL")),
                                .ModifiedDT = wpm_GetDBDate(PartDataRow("ModifiedDT")),
                                .PartSortOrder = wpm_GetDBInteger(PartDataRow("PartSortOrder")),
                                .View = wpm_GetDBBoolean(PartDataRow("View")),
                                .UserID = wpm_GetDBString(PartDataRow("UserID")),
                                .AmazonIndex = wpm_GetDBString(PartDataRow("AmazonIndex")),
                                .CompanyID = wpm_GetDBString(PartDataRow("CompanyID")),
                                .CompanyNM = wpm_GetDBString(PartDataRow("CompanyNM"))})
    End Sub

    Private Property SearchPartID As String
    '
    '  Find 
    '
    Function FindPart(thePartID As String) As Part
        Dim foundPart As New Part With {.PartID = "NEW"}
        SearchPartID = thePartID
        If Count = 1 Then
            '
            '  TO DO:  Handle situation when company only has one record
            '
        End If
        For Each fromPart As Part In FindAll(AddressOf FindPartByPartID)
            foundPart.Title = fromPart.Title
            foundPart.URL = fromPart.URL
            foundPart.Description = fromPart.Description
            foundPart.AmazonIndex = fromPart.AmazonIndex
            foundPart.PartCategoryID = fromPart.PartCategoryID
            foundPart.PartTypeCD = fromPart.PartTypeCD
            foundPart.PartCategoryTitle = fromPart.PartCategoryTitle
            foundPart.PartID = fromPart.PartID
            foundPart.SiteCategoryGroupID = fromPart.SiteCategoryGroupID
            foundPart.LocationID = fromPart.LocationID
            foundPart.View = fromPart.View
            foundPart.ModifiedDT = Now()
            foundPart.PartSortOrder = fromPart.PartSortOrder
            foundPart.UserID = fromPart.UserID
            foundPart.CompanyID = fromPart.CompanyID
        Next
        Return foundPart
    End Function


    Public Function GetActiveParts() As List(Of Part)
        Return Me.FindAll(AddressOf FindActiveParts)
    End Function
    Private Function FindActiveParts(ByVal Part As Part) As Boolean
        Dim bReturn As Boolean = False
        If Part.View Then
            If ((Part.PartCategoryTitle = "LeftColumnLinks") Or _
                (Part.PartCategoryTitle = "RightColumnLinks") Or _
                (Part.PartCategoryTitle = "CenterColumnLinks")) Then
                bReturn = True
            End If
        End If
        Return bReturn
    End Function
    Private Function FindPartByPartID(ByVal Part As Part) As Boolean
        If Part.PartID = SearchPartID Then
            Return True
        Else
            Return False
        End If
    End Function

End Class

Public Enum PartType
    MENU5
    MENU4
    MENU3
    MENU2
    MENU1
    SubMenu
    FILE
    REST
    XML
    RSS
    SiteCategory
    KeywordSearch
End Enum



