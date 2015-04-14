Public Class PartList
    Inherits List(Of Part)

    Private Property SearchPartID As String

    Public Function GetCompanyCategoryParts(ByVal CompanyID As String, ByVal SiteCategoryTypeID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each PartDataRow As DataRow In ApplicationDAL.GetSiteCategoryLinks(CompanyID, SiteCategoryTypeID).Rows
                AddPart(PartDataRow)
            Next
        Catch ex As Exception
            bReturn = False
            ApplicationLogging.ErrorLog("ERROR ON PartList.PopulateSiteLinkRows-Cateogry()", ex.ToString)
        End Try
        Return bReturn
    End Function
    Public Function GetCompanyParts(ByVal CompanyID As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            For Each PartDataRow As DataRow In ApplicationDAL.GetSiteParts(CompanyID).Rows
                AddPart(PartDataRow)
            Next
        Catch innerex As Exception
            bReturn = False
            ApplicationLogging.ErrorLog("ERROR ON PartList.PopulateSiteLinkRows()", innerex.ToString)
        End Try
        Return bReturn
    End Function

    Private Sub AddPart(ByVal PartDataRow As DataRow)
        Me.Add(New Part() With {.PartID = wpm_GetDBString(PartDataRow("ID")),
                                                     .PartTypeCD = wpm_GetDBString(PartDataRow("LinkTypeCD")),
                                                     .PartCategoryTitle = wpm_GetDBString(PartDataRow("LinkCategoryTitle")),
                                                     .PartCategoryID = wpm_GetDBString(PartDataRow("CategoryID")),
                                                     .LocationID = wpm_GetDBString(PartDataRow("PageID")),
                                                     .Title = wpm_GetDBString(PartDataRow("LinkTitle")),
                                                     .Description = wpm_GetDBString(PartDataRow("Description")),
                                                     .URL = wpm_GetDBString(PartDataRow("URL")),
                                                     .ModifiedDT = wpm_GetDBDate(PartDataRow("DateAdd")),
                                                     .PartSortOrder = wpm_GetDBInteger(PartDataRow("Ranks")),
                                                     .View = wpm_GetDBBoolean(PartDataRow("Views")),
                                                     .UserID = wpm_GetDBString(PartDataRow("UserID")),
                                                     .AmazonIndex = wpm_GetDBString(PartDataRow("ASIN")),
                                                     .PartSource = wpm_GetDBString(PartDataRow("LinkSource")),
                                                     .CompanyID = wpm_GetDBString(PartDataRow("CompanyID")),
                                                     .SiteCategoryGroupID = wpm_GetDBString(PartDataRow("SiteCategoryGroupID")),
                                                     .SiteCategoryTypeID = wpm_GetDBString(PartDataRow("SiteCategoryTypeID"))})
    End Sub
    '
    '  Find 
    '
    Function FindPart(thePartID As String) As Part
        Dim foundPart As New Part
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
            foundPart.SiteCategoryTypeID = fromPart.SiteCategoryTypeID
            foundPart.SiteCategoryGroupID = fromPart.SiteCategoryGroupID
            foundPart.LocationID = fromPart.LocationID
            foundPart.View = fromPart.View
            foundPart.ModifiedDT = Now()
            foundPart.PartSortOrder = fromPart.PartSortOrder
            foundPart.UserID = fromPart.UserID
            foundPart.PartSource = fromPart.PartSource
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
