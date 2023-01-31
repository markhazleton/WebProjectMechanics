
Public Class RecipeCategoryList
    Inherits List(Of RecipeCategoryItem)

    Public Sub New()
        Using myCon As New RecipeDataController()
            Me.AddRange((From i In myCon.vwRecipeCategories
                         Order By i.DisplayOrder Ascending
                         Select New RecipeCategoryItem With
                             {
                             .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                             .CategoryNM = wpm_GetDBString(i.RecipeCategoryNM),
                             .CategoryCM = wpm_GetDBString(i.RecipeCategoryCM),
                             .DisplayOrder = wpm_GetDBInteger(i.DisplayOrder),
                             .IsActive = wpm_GetDBBoolean(i.IsActive),
                             .MaxDate = wpm_GetDBDate(i.NewestRecipeDT),
                             .FileDescription = wpm_GetDBString(i.FileDescription),
                             .FileName = wpm_GetDBString(i.FileName),
                             .RecipeID = wpm_GetDBInteger(i.RecipeID),
                             .RecipeNM = wpm_GetDBString(i.RecipeNM),
                             .RecipeCount = wpm_GetDBInteger(i.RecipeCount)}).ToList())
        End Using
    End Sub
    Private Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Private Sub New(ByVal collection As IEnumerable(Of RecipeCategoryItem))
        MyBase.New(collection)

    End Sub

End Class

Public Class RecipeImageList
    Inherits List(Of RecipeImageItem)

    Sub New()

    End Sub

    Public Sub LoadAll()
        Using myCon As New RecipeDataController()
            For Each myI In myCon.RecipeImages
                Me.Add(New RecipeImageItem With {.RecipeImageID = wpm_GetDBString(myI.RecipeImageID),
                                                 .RecipeID = wpm_GetDBString(myI.RecipeID),
                                                 .RecipeNM = myI.Recipe.RecipeNM,
                                                 .DisplayOrder = wpm_GetDBInteger(myI.DisplayOrder),
                                                 .FileName = wpm_GetDBString(myI.FileName),
                                                 .FileDescription = wpm_GetDBString(myI.FileDescription),
                                                 .ModifiedID = wpm_GetDBInteger(myI.ModifiedID),
                                                 .ModifiedDT = wpm_GetDBDate(myI.ModifiedDT)})
            Next
        End Using
    End Sub

    Public Sub LoadRecipe(ByVal RecipeID As String)
        Using myCon As New RecipeDataController()
            For Each myI In (From I In myCon.RecipeImages Where I.RecipeID = wpm_GetDBInteger(RecipeID)).ToList()
                Me.Add(New RecipeImageItem With {.RecipeImageID = wpm_GetDBString(myI.RecipeImageID),
                                                 .RecipeID = wpm_GetDBString(myI.RecipeID),
                                                 .RecipeNM = myI.Recipe.RecipeNM,
                                                 .DisplayOrder = wpm_GetDBInteger(myI.DisplayOrder),
                                                 .FileName = wpm_GetDBString(myI.FileName),
                                                 .FileDescription = wpm_GetDBString(myI.FileDescription),
                                                 .ModifiedID = wpm_GetDBInteger(myI.ModifiedID),
                                                 .ModifiedDT = wpm_GetDBDate(myI.ModifiedDT)})
            Next
        End Using
    End Sub


    Private Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub
    Private Sub New(ByVal collection As IEnumerable(Of RecipeImageItem))
        MyBase.New(collection)
    End Sub

End Class