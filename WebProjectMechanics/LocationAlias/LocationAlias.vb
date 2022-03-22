
Public Class LocationAlias
    Implements ILocationAlias
    Public Property PageAliasID() As String Implements ILocationAlias.PageAliasID
    Public Property PageURL() As String Implements ILocationAlias.PageURL
    Public Property TargetURL() As String Implements ILocationAlias.TransferURL
    Public Property AliasType() As String Implements ILocationAlias.AliasType
    Public Property CompanyID As String

    Public Function SetLocationAliasValue(ByVal myrow As DataRow) As LocationAlias
        PageAliasID = wpm_GetDBString(myrow("PageAliasID"))
        PageURL = wpm_GetDBString(myrow("PageURL"))
        TargetURL = wpm_GetDBString(myrow("TargetURL"))
        CompanyID = wpm_GetDBString(myrow("CompanyID"))
        AliasType = wpm_GetDBString(myrow("AliasType"))
        Return Me
    End Function

    ' Page Alias
    Public Const STR_PageAliasID As String = "PageAliasID"
    Public Const STR_SelectPageAliasList As String = "SELECT [PageAliasID],[PageURL], [TargetURL], [AliasType], [CompanyID] FROM [PageAlias]"
    Public Const STR_SelectPageAliasByPageAliasID As String = "SELECT [PageAliasID],[PageURL], [TargetURL], [AliasType], [CompanyID] FROM [PageAlias] where [PageAliasID] = @PageAliasID "
    Public Const STR_UPDATE_PageAlias As String = "UPDATE [PageAlias] SET [PageURL] = @PageURL, [TargetURL] = @TargetURL, [AliasType] = @AliasType, [CompanyID] = @CompanyID WHERE [PageAliasID] = @PageAliasID "
    Public Const STR_INSERT_PageAlias As String = "INSERT INTO [PageAlias] ([PageURL], [TargetURL], [AliasType], [CompanyID]) VALUES (@PageURL, @TargetURL, @AliasType, @CompanyID)"
    Public Const STR_DELETE_PageAlias As String = "DELETE FROM [PageAlias] WHERE [PageAliasID] = @PageAliasID"

End Class
