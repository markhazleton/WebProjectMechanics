Public Interface IRecipeItem
    Property RecipeID() As String
    Property CategoryID() As String
    Property Category() As String
    Property RecipeNM() As String
    Property RecipeDS() As String
    Property AuthorNM() As String
    Property IngredientDS() As String
    Property InstructionDS() As String
    Property ModifiedDT() As Date
    Property IsApproved() As Boolean
    Property ViewCount() As Integer
    Property AverageRating() As Integer
    Property RatingCount() As Integer
    Property CommentsCount() As Integer
    Property LastViewDT() As Date
End Interface

Public Interface IRecipeImage
    Property RecipeID() As String 
    Property RecipeImageID as string 
    Property FileName As String
    Property FileDescription As String 
    Property DisplayOrder As Integer
    Property ModifiedID As Integer
    Property ModifiedDT As DateTime

End Interface