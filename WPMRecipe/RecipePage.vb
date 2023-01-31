Imports System.Text

Public Class RecipePage
    Inherits ApplicationPage
End Class

Public Class RecipeUserControl
    Inherits ApplicationUserControl
    Private _RecipeList As RecipeList
    Private _CategoryList As RecipeCategoryList
    Private _RecipeImageList As RecipeImageList
    Public Property myRecipelist As RecipeList
        Get
            If _RecipeList Is Nothing Then
                SetData()
            End If
            Return _RecipeList
        End Get
        Set(value As RecipeList)
            _RecipeList = value
        End Set
    End Property
    Public Property myCategoryList As RecipeCategoryList
        Get
            If _CategoryList Is Nothing Then
                SetData()
            End If
            Return _CategoryList
        End Get
        Set(value As RecipeCategoryList)
            _CategoryList = value
        End Set
    End Property

    Public Property myRecipeImageList As RecipeImageList
    Get
            If _RecipeImageList is Nothing then 
                SetData()
            End If
            Return _RecipeImageList
    End Get
    Set(value As RecipeImageList)
            _RecipeImageList = value
    End Set
    End Property



    Public Sub RefreshData()
        HttpContext.Current.Session.Item("RecipeList") = New RecipeList()
        HttpContext.Current.Session.Item("CategoryList") = New RecipeCategoryList()
    End Sub
    Public Sub SetData()
        If HttpContext.Current.Session.Item("RecipeList") Is Nothing Then
            HttpContext.Current.Session.Item("RecipeList") = New RecipeList()
        End If
        _RecipeList = CType(HttpContext.Current.Session.Item("RecipeList"), RecipeList)

        If HttpContext.Current.Session.Item("CategoryList") Is Nothing Then
            HttpContext.Current.Session.Item("CategoryList") = New RecipeCategoryList()
        End If
        _CategoryList = CType(HttpContext.Current.Session.Item("CategoryList"), RecipeCategoryList)
    End Sub
    Public Function GetAtoZCategoryLinks() As String
        Dim sbReturn As New StringBuilder(String.Empty)
        Dim myString As String
        For i = 65 To 90
            myString = Microsoft.VisualBasic.Chr(i)
            sbReturn.Append(String.Format("<a href='/recipe/default.aspx?letter={0}' class=""letter"" title={0}>{0}</a>&nbsp;&nbsp;", myString, Microsoft.VisualBasic.Chr(34)))
        Next
        Return sbReturn.ToString
    End Function



End Class