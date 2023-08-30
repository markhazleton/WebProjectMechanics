

Public Class RecipeDataController
    Inherits RecipeLibraryDataContext
    Public Property ReturnValue As String
    Public Sub New()
        MyBase.New("Data Source=controlorigins1.cnggm5xnvplw.us-west-2.rds.amazonaws.com;Initial Catalog=WebProjectMechanics;User ID=codb;Password=P@ssword1")

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
            WebProjectMechanics.ApplicationLogging.SQLExceptionLog("RecipeDataController.SubmitChanges", ex)
            ReturnValue = String.Format("ERROR ON SUBMIT: {0}", ex.Message)
        End Try
    End Sub

#Region "Recipe"

    Function GetRecipeByRecipeID(ByVal reqRecipeID As Integer) As RecipeItem
        Dim myRecipe As New RecipeItem
        If reqRecipeID > 0 Then
            Try
                With (From i In Recipes Where i.RecipeID = reqRecipeID Order By i.RecipeNM).Single()
                    myRecipe.RecipeID = CStr(wpm_GetDBInteger(.RecipeID))
                    myRecipe.CategoryID = CStr(wpm_GetDBInteger(.RecipeCategoryID))
                    myRecipe.Category = wpm_GetDBString(.RecipeCategory.RecipeCategoryNM)
                    myRecipe.RecipeNM = wpm_GetDBString(.RecipeNM)
                    myRecipe.RecipeDS = wpm_GetDBString(.RecipeDS)
                    myRecipe.AuthorNM = wpm_GetDBString(.AuthorNM)
                    myRecipe.IngredientDS = wpm_GetDBString(.IngredientDS)
                    myRecipe.InstructionDS = wpm_GetDBString(.InstructionDS)
                    myRecipe.ModifiedDT = wpm_GetDBDate(.ModifiedDT)
                    myRecipe.IsApproved = wpm_GetDBBoolean(.IsApproved)
                    myRecipe.ViewCount = wpm_GetDBInteger(.ViewCount)
                    myRecipe.AverageRating = wpm_GetDBInteger(.AverageRating)
                    myRecipe.RatingCount = wpm_GetDBInteger(.RatingCount)
                    myRecipe.CommentsCount = wpm_GetDBInteger(.RecipeComments.Count)
                    myRecipe.LastViewDT = wpm_GetDBDate(.LastViewDT)
                    myRecipe.RecipeCommentList.AddRange(.RecipeComments)
                    myRecipe.RecipeImageList.AddRange(
                        (From i In .RecipeImages
                         Select New RecipeImageItem With {.RecipeID = wpm_GetDBString(i.RecipeID),
                                                          .RecipeImageID = wpm_GetDBString(i.RecipeImageID),
                                                          .FileName = i.FileName,
                                                          .DisplayOrder = i.DisplayOrder,
                                                          .FileDescription = i.FileDescription,
                                                          .ModifiedID = i.ModifiedID,
                                                          .ModifiedDT = i.ModifiedDT}).ToList())
                End With
            Catch ex As Exception
                ApplicationLogging.SQLExceptionLog(String.Format("RecipeDataController.GetRecipeByRecipeID(ID-{0})", reqRecipeID), ex)
                Return New RecipeItem
            End Try
        End If
        Return myRecipe
    End Function

    Public Function UpdateRating(ByVal reqRecipeID As Integer, ByVal NewRating As Integer, ByVal myComment As String, ByVal myCommentNM As String) As Double
        Try
            Dim myRecipe As Recipe = (From i In Recipes Where i.RecipeID = reqRecipeID).Single
            Dim TotalRating As Double = myRecipe.AverageRating * myRecipe.RatingCount
            TotalRating = TotalRating + NewRating
            myRecipe.RatingCount = myRecipe.RatingCount + 1
            myRecipe.AverageRating = Math.Round(TotalRating / myRecipe.RatingCount, 2)
            If myRecipe.AverageRating > 5 Then
                myRecipe.AverageRating = 5
            End If
            myRecipe.ModifiedDT = Now()
            myRecipe.LastViewDT = Now()
            SubmitChanges()
            If Not String.IsNullOrEmpty(myComment) AndAlso Not String.IsNullOrEmpty(myCommentNM) Then
                Dim newRecipeComment As New RecipeComment()
                With newRecipeComment
                    .Comment = myComment
                    .RecipeID = reqRecipeID
                    .ModifiedDT = Now()
                    .AuthorEmail = "unknown"
                    .AuthorNM = myCommentNM
                End With
                RecipeComments.InsertOnSubmit(newRecipeComment)
                SubmitChanges()
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog("RecipeItem.UpdateRating", String.Format("Error on Recipe.UpdateViewCount(ID) - {0} ({1})", reqRecipeID, ex.Message))
        End Try
        Return NewRating
    End Function
    Public Sub SaveRecipe(myRecipeItem As RecipeItem)
        Dim myRecipeID As Integer = wpm_GetDBInteger(myRecipeItem.RecipeID)
        Dim myRecipe As Recipe
        Try
            If myRecipeID > 0 Then
                myRecipe = (From i In Recipes Where i.RecipeID = wpm_GetDBInteger(myRecipeItem.RecipeID)).Single
            Else
                myRecipe = New Recipe
            End If
            With myRecipe
                .IsApproved = myRecipeItem.IsApproved
                .RecipeNM = myRecipeItem.RecipeNM
                .RecipeDS = myRecipeItem.RecipeDS
                .AuthorNM = myRecipeItem.AuthorNM
                .RecipeCategoryID = wpm_GetDBInteger(myRecipeItem.CategoryID)
                .IngredientDS = myRecipeItem.IngredientDS
                .InstructionDS = myRecipeItem.InstructionDS
                .ModifiedDT = Now()
                .ModifiedID = 99

                If myRecipeID = 0 Then
                    .LastViewDT = Now()
                    .ViewCount = 0
                    Recipes.InsertOnSubmit(myRecipe)
                End If
            End With
            SubmitChanges()
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.SaveRecipe", String.Format("Error on Recipe.SaveRecipe(ID) - {0} ({1})", myRecipeID, ex.Message))
        End Try
    End Sub
    Public Sub SaveCategory(myCategoryItem As RecipeCategoryItem)
        Dim myCategoryID As Integer = wpm_GetDBInteger(myCategoryItem.CategoryID)
        Dim myCategory As RecipeCategory
        Try
            If myCategoryID > 0 Then
                myCategory = (From i In RecipeCategories Where i.RecipeCategoryID = myCategoryID).SingleOrDefault
            Else
                myCategory = New RecipeCategory
            End If
            With myCategory
                .RecipeCategoryNM = myCategoryItem.CategoryNM
                .ModifiedDT = Now()
                .ModifiedID = 99
                If .RecipeCategoryID = 0 Then
                    RecipeCategories.InsertOnSubmit(myCategory)
                End If
            End With
            SubmitChanges()
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.SaveCategory", String.Format("Error on Recipe.SaveCategory(ID) - {0} ({1})", myCategoryID, ex.Message))
        End Try
    End Sub
    Public Sub DeleteCategory(myCategoryItem As RecipeCategoryItem)
        Dim myCategoryID As Integer = wpm_GetDBInteger(myCategoryItem.CategoryID)
        Dim myCategory As RecipeCategory
        Try
            If myCategoryID > 0 Then
                myCategory = (From i In RecipeCategories Where i.RecipeCategoryID = myCategoryID).SingleOrDefault
                RecipeCategories.DeleteOnSubmit(myCategory)
                SubmitChanges()
            End If
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.SaveCategory", String.Format("Error on Recipe.SaveCategory(ID) - {0} ({1})", myCategoryID, ex.Message))
        End Try

    End Sub
#End Region


#Region "Category"




#End Region

    Sub SaveRecipeImageItem(recipeImageItem As RecipeImageItem)
        Dim myRecipeImageID As Integer = wpm_GetDBInteger(recipeImageItem.RecipeImageID)
        If myRecipeImageID < 1 Then
            myRecipeImageID = 0
        End If

        Dim myRecipeImage As RecipeImage
        Try
            If myRecipeImageID > 0 Then
                myRecipeImage = (From i In RecipeImages Where i.RecipeImageID = myRecipeImageID).Single
            Else
                myRecipeImage = New RecipeImage
            End If
            With myRecipeImage
                .RecipeImageID = myRecipeImageID
                .RecipeID = wpm_GetDBInteger(recipeImageItem.RecipeID)
                .FileName = wpm_GetDBString(recipeImageItem.FileName)
                .FileDescription = wpm_GetDBString(recipeImageItem.FileDescription)
                .DisplayOrder = wpm_GetDBInteger(recipeImageItem.DisplayOrder)

                .ModifiedDT = Now()
                .ModifiedID = 99

                If myRecipeImageID = 0 Then
                    RecipeImages.InsertOnSubmit(myRecipeImage)
                End If
            End With
            SubmitChanges()
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.SaveRecipe", String.Format("Error on Recipe.SaveRecipe(ID) - {0} ({1})", myRecipeImageID, ex.Message))
        End Try


    End Sub

    Sub DeleteRecipeImageItem(recipeImageItem As RecipeImageItem)
        Dim myRecipeImageID As Integer = wpm_GetDBInteger(recipeImageItem.RecipeImageID)
        Dim myRecipeImage As RecipeImage
        Try
            If myRecipeImageID > 0 Then
                myRecipeImage = (From i In RecipeImages Where i.RecipeImageID = wpm_GetDBInteger(recipeImageItem.RecipeImageID)).SingleOrDefault
                RecipeImages.DeleteOnSubmit(myRecipeImage)
                SubmitChanges()
            End If
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeImageItem.DeleteRecipeImageItem", String.Format("Error on RecipeImageItem.DeleteRecipeImageItem(ID) - {0} ({1})", myRecipeImageID, ex.Message))
        End Try
    End Sub

End Class
