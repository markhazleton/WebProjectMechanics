Imports WebProjectMechanics
Public Class admin_maint_PageType
    Inherits ApplicationUserControl
    ' Company 
    Public Const STR_PageTypeID As String = "PageTypeID"
    Public Const STR_SELECTPageTypeList As String = "SELECT PageType.[PageTypeID], PageType.[PageTypeCD], PageType.[PageTypeDesc], PageType.[PageFileName] FROM PageType;"
    Public Const STR_SELECT_PageTypeByPageTypeID As String = "SELECT PageType.[PageTypeID], PageType.[PageTypeCD], PageType.[PageTypeDesc], PageType.[PageFileName] FROM PageType where [PageType].[PageTypeID]={0};"
    Public Const STR_UPDATE_PageType As String = "UPDATE PageType SET PageType.PageTypeCD = @PageTypeCD , PageType.PageTypeDesc = @PageTypeDesc, PageType.PageFileName = @PageFileName WHERE (((PageType.PageTypeID)=@PageTypeID));"
    Public Const STR_INSERT_PageType As String = "INSERT INTO PageType ( PageType.PageTypeCD , PageType.PageTypeDesc , PageType.PageFileName ) VALUES ( @PageTypeCD ,@PageTypeDesc, @PageFileName ); "
    Public Const STR_DELETE_PageType As String = "DELETE FROM [PageType] WHERE [PageType].[PageTypeID]={0};"
    Private Property reqPageTypeID As String
    Public Class PageType
        Public Property PageTypeID As String
        Public Property PageTypeCD As String
        Public Property PageTypeDesc As String
        Public Property PageFileName As String
    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqPageTypeID = GetProperty(STR_PageTypeID, String.Empty)
        If Not IsPostBack Then
            If reqPageTypeID <> String.Empty Then
                If reqPageTypeID = "NEW" Or reqPageTypeID = 0 Then
                Else
                End If
            Else
                ' Show the list
                Dim myPageTypeList As New List(Of PageType)
                For Each myRow In wpm_GetDataTable(STR_SELECTPageTypeList, "PageType").Rows
                    myPageTypeList.Add(New PageType With {.PageTypeID = wpm_GetDBInteger(myRow("PageTypeID")),
                                                          .PageTypeCD = wpm_GetDBString(myRow("PageTypeCD")),
                                                          .PageFileName = wpm_GetDBString(myRow("PageFileName")),
                                                          .PageTypeDesc = wpm_GetDBString(myRow("PageTypeDesc"))})
                Next
                Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Location Type", .DetailKeyName = "PageTypeID", .DetailFieldName = "PageTypeCD", .DetailPath = "/admin/maint/default.aspx?type=LocationType&PageTypeID={0}"}
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "PageTypeDS", .Value = "PageTypeDesc"})
                myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.Name = "PageFileName", .Value = "PageFileName"})
                Dim myList As New List(Of Object)
                myList.AddRange(myPageTypeList)
                dtList.BuildTable(myListHeader, myList)
            End If
        End If
    End Sub
End Class
