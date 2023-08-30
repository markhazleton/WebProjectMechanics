Imports LINQHelper.System.Linq.Dynamic

Public Class RecipeList
    Inherits List(Of RecipeItem)

    Public Sub New(ByVal ListType As RecipeListType)
        Using myCon As New RecipeDataController
            Select Case ListType
                Case RecipeListType.AdminAll
                    AddRange((From i In myCon.vwRecipes
                              Select New RecipeItem With {
                                  .RecipeID = wpm_GetDBString(i.RecipeID),
                                  .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                                            }).ToList)
                Case RecipeListType.AllApproved
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True
                              Select New RecipeItem With {
                                  .RecipeID = wpm_GetDBString(i.RecipeID),
                                  .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).ToList)
                Case RecipeListType.Top10Hits
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True
                              Order By i.ViewCount Descending
                              Select New RecipeItem With {
                                  .RecipeID = wpm_GetDBString(i.RecipeID),
                                  .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(10).ToList())
                Case RecipeListType.Top10Newest
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Order By i.ModifiedDT Descending Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(10).ToList())
                Case RecipeListType.Top10Popular
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Order By i.ViewCount Descending Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(10).ToList())
                Case RecipeListType.Top15Hits
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Order By i.ViewCount Descending Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(15).ToList())
                Case RecipeListType.Top15Newest
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Order By i.ModifiedDT Descending Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(15).ToList())
                Case RecipeListType.Topt5Hits
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Order By i.ViewCount Descending Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).Take(5).ToList())
                Case Else
                    AddRange((From i In myCon.vwRecipes Where i.IsApproved = True Select New RecipeItem With {
                                .RecipeID = wpm_GetDBString(i.RecipeID),
                                .CategoryID = wpm_GetDBString(i.RecipeCategoryID),
                                  .Category = wpm_GetDBString(i.RecipeCategoryNM),
                                  .AuthorNM = wpm_GetDBString(i.AuthorNM),
                                  .IngredientDS = wpm_GetDBString(i.IngredientDS),
                                  .InstructionDS = wpm_GetDBString(i.InstructionDS),
                                  .RecipeNM = wpm_GetDBString(i.RecipeNM),
                                  .RecipeDS = wpm_GetDBString(i.RecipeDS),
                                  .ModifiedDT = wpm_GetDBDate(i.ModifiedDT),
                                  .IsApproved = wpm_GetDBBoolean(i.IsApproved),
                                  .ViewCount = wpm_GetDBInteger(i.ViewCount),
                                  .LastViewDT = wpm_GetDBDate(i.LastViewDT),
                                  .AverageRating = wpm_GetDBInteger(i.AverageRating),
                                  .RatingCount = wpm_GetDBInteger(i.RatingCount),
                                  .CommentsCount = wpm_GetDBInteger(i.CommentCount),
                                  .FileDescription = wpm_GetDBString(i.FileDescription),
                                  .FileName = wpm_GetDBString(i.FileName)
                                }).ToList)

            End Select
        End Using

    End Sub
    Public Sub New()
        Me.New(RecipeListType.AllApproved)
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)

    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of RecipeItem))
        MyBase.New(collection)
    End Sub

    Public Function GetRSS() As XDocument
        Dim outputxml = <?xml version="1.0" encoding="UTF-8"?>
                        <rss version="2.0">
                            <channel>
                                <title>Recipe Library</title>
                                <description>Recipe Library</description>
                                <link><%= ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & "/recipe/") %></link>
                                <lastBuildDate><%= Now.ToLongDateString %></lastBuildDate>
                                <pubDate><%= Now.ToLongDateString %></pubDate>
                                <ttl>1800</ttl>
                                <%= From i In Me Order By i.ViewCount Descending
                                    Select <item>
                                               <title><%= i.RecipeNM %></title>
                                               <description><%= i.RecipeDS %></description>
                                               <link><%= ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & "/recipe/" & WebProjectMechanics.wpm_FormatPageNameForURL(i.RecipeNM)) %></link>
                                               <author><%= i.AuthorNM %></author>
                                               <guid isPermaLink="true"><%= ("http://" & HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME") & "/recipe/" & WebProjectMechanics.wpm_FormatPageNameForURL(i.RecipeNM)) %></guid>
                                               <pubDate><%= Now.ToLongDateString %></pubDate>
                                           </item> %>
                            </channel>
                        </rss>
        Return outputxml
    End Function

End Class

Public Enum RecipeListType
    Topt5Hits
    Top10Hits
    Top15Hits
    Top10Newest
    Top15Newest
    Top10Popular
    AllApproved
    AdminAll
End Enum

