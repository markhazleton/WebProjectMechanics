
Public Interface IPartEdit
    Event ArticleEditCancel()
    Event ArticleEditSave()
    Event ArticleEditDelete()
    Event ArticleEditSaveNew()
    WriteOnly Property UpdateComplete As Boolean
    WriteOnly Property UpdateError As String
    Sub SetPart(ByVal PartID As String)
    ReadOnly Property Title As String
    ReadOnly Property URL As String
    ReadOnly Property Description As String
    ReadOnly Property PartCategoryID As String
    ReadOnly Property PartTypeCD As String
    ReadOnly Property LinkCategoryTitle As String
    ReadOnly Property PartID As String
    ReadOnly Property SiteCategoryTypeID As String
    ReadOnly Property SiteCategoryGroupID As String
    ReadOnly Property LocationID As String
    ReadOnly Property View As Boolean
    ReadOnly Property ModifiedDT As Date
    ReadOnly Property PartSortOrder As Integer
    ReadOnly Property UserName As String
    ReadOnly Property UserID As String
    ReadOnly Property LinkASIN As String
    ReadOnly Property AmazonIndex As String
    ReadOnly Property PartSource As String
    ReadOnly Property CompanyID As String
End Interface
