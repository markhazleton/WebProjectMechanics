
Public Class RecipeCategoryItem
    Public Property CategoryID As String
    Public Property CategoryNM As String
    Public Property CategoryCM As String
    Public Property DisplayOrder As Integer
    Public Property IsActive As Boolean
    Public Property RecipeID As Integer
    Public Property RecipeNM As String
    Public Property FileName As String
    Public Property FileDescription As String

    Public Property RecipeCount As Integer
    Public Property MaxDate As Date

    Public Property CategoryImagePath As String

    Public Sub New()
        ' Do Nothing
    End Sub
    Public Sub New(ByVal CategoryID As Integer)
        Using myCon As New RecipeDataController
            For Each myCategory In (From i In myCon.vwRecipeCategories Where i.RecipeCategoryID = CategoryID Order By i.RecipeCategoryNM Descending)
                CategoryID = CInt(wpm_GetDBString(myCategory.RecipeCategoryID))
                CategoryNM = wpm_GetDBString(myCategory.RecipeCategoryNM)
                CategoryCM = wpm_GetDBString(myCategory.RecipeCategoryCM)
                DisplayOrder = wpm_GetDBInteger(myCategory.DisplayOrder)
                IsActive = wpm_GetDBBoolean(myCategory.IsActive)
                RecipeCount = wpm_GetDBInteger(myCategory.RecipeCount)
                MaxDate = wpm_GetDBDate(myCategory.NewestRecipeDT)
                FileName = wpm_GetDBString(myCategory.FileName)
                FileDescription = wpm_GetDBString(myCategory.FileDescription)
                RecipeID = wpm_GetDBInteger(myCategory.RecipeID)
                RecipeNM = wpm_GetDBString(myCategory.RecipeNM)
            Next
        End Using
    End Sub
    Public Sub SaveCategory()
        Try
            Using mycon As New RecipeDataController()
                mycon.SaveCategory(Me)
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeCategoryItem.SaveCategory", String.Format("RecipeCategoryItem.SaveCategory(ID) - {0} ({1})", CategoryID, ex.Message))
        End Try
    End Sub
    Public Sub DeleteCategory()
        Try
            Using mycon As New RecipeDataController()
                mycon.DeleteCategory(Me)
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeCategoryItem.DeleteCategory", String.Format("RecipeCategoryItem.DeleteCategory(ID) - {0} ({1})", CategoryID, ex.Message))
        End Try
    End Sub

End Class
