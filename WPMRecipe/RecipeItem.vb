
Public Class RecipeItem
    Implements IEquatable(Of RecipeItem)

    Public Property RecipeID() As String
    Public Property CategoryID() As String
    Public Property Category() As String
    Public Property RecipeNM() As String
    Public Property RecipeDS() As String
    Public Property AuthorNM() As String
    Public Property IngredientDS() As String
    Public Property InstructionDS() As String
    Public Property ModifiedDT() As Date
    Public Property IsApproved() As Boolean
    Public Property LastViewDT() As Date
    Public Property ViewCount() As Integer
    Public Property AverageRating() As Integer
    Public Property RatingCount() As Integer
    Public Property CommentsCount() As Integer
    Public Property RecipeCommentList As New List(Of RecipeComment)
    Public Property RecipeImageList As New List(Of RecipeImageItem)

    Public Property FileName As String
    Public Property FileDescription As String
    Public Property RecipeImagePath As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal RecipeID As String)
        GetRecipeItem(CInt(RecipeID))
    End Sub

    Public Sub UpdateViewCount(ByVal reqRecipeID As Integer)
        Try
            Using mycon As New RecipeDataController()
                Dim myRecipe As Recipe = (From i In mycon.Recipes Where i.RecipeID = reqRecipeID).Single
                myRecipe.ViewCount = myRecipe.ViewCount + 1
                myRecipe.LastViewDT = Now()
                mycon.SubmitChanges()
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("RecipeItem.UpdateViewCount", String.Format("Error on Recipe.UpdateViewCount(ID) - {0} ({1})", RecipeID, ex.Message))
        End Try
    End Sub
    Public Function UpdateRating(ByVal reqRecipeID As Integer, ByVal NewRating As Integer, ByVal myComment As String, ByVal myCommentNM As String) As Double
        Try
            Using mycon As New RecipeDataController()
                mycon.UpdateRating(reqRecipeID, NewRating, myComment, myCommentNM)
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("RecipeItem.UpdateRating", String.Format("Error on Recipe.UpdateViewCount(ID) - {0} ({1})", RecipeID, ex.Message))
        End Try
        Return NewRating
    End Function
    Public Sub SaveRecipe()
        Try
            Using mycon As New RecipeDataController()
                mycon.SaveRecipe(Me)
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.SaveRecipe", String.Format("Error on Recipe.SaveRecipe(ID) - {0} ({1})", RecipeID, ex.Message))
        End Try
    End Sub
    Public Function Delete() As Boolean
        Dim myRecipeID As Integer = wpm_GetDBInteger(Me.RecipeID)
        Dim myRecipe As Recipe
        Try
            Using mycon As New RecipeDataController()
                If myRecipeID > 0 Then
                    myRecipe = (From i In mycon.Recipes Where i.RecipeID = wpm_GetDBInteger(Me.RecipeID)).Single
                Else
                    Return False
                End If
                If myRecipe.RecipeID > 0 Then
                    mycon.Recipes.DeleteOnSubmit(myRecipe)
                    mycon.SubmitChanges()
                End If
            End Using
            Return True
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.Delete", String.Format("Error on Recipe.Delete(ID) - {0} ({1})", RecipeID, ex.Message))
            Return False
        End Try
    End Function

    Public Function GetRecipeItem(ByVal reqRecipeID As Integer) As RecipeItem
        Try
            Using myCon As New RecipeDataController()
                With myCon.GetRecipeByRecipeID(reqRecipeID)
                    RecipeID = .RecipeID
                    CategoryID = .CategoryID
                    Category = .Category
                    RecipeNM = .RecipeNM
                    RecipeDS = .RecipeDS
                    AuthorNM = .AuthorNM
                    IngredientDS = .IngredientDS
                    InstructionDS = .InstructionDS
                    ModifiedDT = .ModifiedDT
                    IsApproved = .IsApproved
                    ViewCount = .ViewCount
                    AverageRating = .AverageRating
                    RatingCount = .RatingCount
                    CommentsCount = .CommentsCount
                    LastViewDT = .LastViewDT
                    RecipeCommentList.AddRange(.RecipeCommentList)
                    RecipeImageList.AddRange(.RecipeImageList)
                End With
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeItem.GetRecipeItem", String.Format("Error on Recipe.New(ID) - {0} ({1})", RecipeID, ex.Message))
        End Try
        Return Me
    End Function
    Public Function Equals1(ByVal other As RecipeItem) As Boolean Implements System.IEquatable(Of RecipeItem).Equals
        Return RecipeID.Equals(other.RecipeID)
    End Function
End Class

Public Class RecipeImageItem
    Implements IRecipeImage

    Public Sub New(ByVal reqRecipeImageID As String)
        GetRecipeImageItem(wpm_GetDBInteger(reqRecipeImageID))
    End Sub
    Public Sub New()

    End Sub


    Public Property DisplayOrder As Integer Implements IRecipeImage.DisplayOrder

    Public Property FileDescription As String Implements IRecipeImage.FileDescription

    Public Property FileName As String Implements IRecipeImage.FileName

    Public Property ModifiedDT As Date Implements IRecipeImage.ModifiedDT

    Public Property ModifiedID As Integer Implements IRecipeImage.ModifiedID

    Public Property RecipeID As String Implements IRecipeImage.RecipeID
    Public Property RecipeNM As String
    Public Property RecipeDS As String
    Public Property RecipeImageID As String Implements IRecipeImage.RecipeImageID

    Private Sub GetRecipeImageItem(reqRecipeImageID As Integer)
        Using myCon As New RecipeDataController()
            For Each myRecipeImg In (From i In myCon.RecipeImages Where i.RecipeImageID = reqRecipeImageID).ToList()
                With Me
                    .RecipeImageID = wpm_GetDBString(myRecipeImg.RecipeImageID)
                    .RecipeID = wpm_GetDBString(myRecipeImg.RecipeID)
                    .RecipeNM = wpm_GetDBString(myRecipeImg.Recipe.RecipeNM)
                    .RecipeDS = wpm_GetDBString(myRecipeImg.Recipe.RecipeDS)
                    .FileName = wpm_GetDBString(myRecipeImg.FileName)
                    .FileDescription = wpm_GetDBString(myRecipeImg.FileDescription)
                    .DisplayOrder = wpm_GetDBInteger(myRecipeImg.DisplayOrder)
                    .ModifiedID = wpm_GetDBInteger(myRecipeImg.ModifiedID)
                    .ModifiedDT = wpm_GetDBDate(myRecipeImg.ModifiedDT)
                End With
            Next
        End Using
    End Sub
    Public Sub Save()
        Try
            Using mycon As New RecipeDataController()
                mycon.SaveRecipeImageItem(Me)
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeImageItem.Save", String.Format("Error on RecipeImage.Save(ID) - {0} ({1})", RecipeImageID, ex.Message))
        End Try
    End Sub

Sub Delete()
        Try
            Using mycon As New RecipeDataController()
                mycon.DeleteRecipeImageItem(Me)
            End Using
        Catch ex As Exception
            ApplicationLogging.SQLAudit("RecipeImageItem.Delete", String.Format("Error on RecipeImage.Delete(ID) - {0} ({1})", RecipeImageID, ex.Message))
        End Try
 End Sub 

End Class