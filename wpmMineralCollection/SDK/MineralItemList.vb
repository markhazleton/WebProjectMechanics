
Public Class MineralItemListView
    Property myResults As New List(Of MineralItem)
    Private Const INT_IndexBase As Integer = 0
    Property CurrentIndex As Integer
    ReadOnly Property MaxIndex As Integer
        Get
            Return myResults.Count() - 1
        End Get
    End Property
    Property MySQLFilter As New SQLFilterList

    Public Function GetList() As List(Of MineralItem)
        myResults.Clear()
        Using mycon As New DataController()
            Try
                Dim myObjects As New List(Of Object)
                myObjects.AddRange((From i In mycon.Minerals Select i.MineralID, i.MineralNM, i.MineralDS, i.ModifiedDT, i.ModifiedID, i.WikipediaURL, PrimaryMineralCount = i.CollectionItems.Count, SecondaryMineralCount = i.CollectionItemMinerals.Count).ToList())

            Catch ex As Exception
                ApplicationLogging.ErrorLog("MineralItemListView.GetList", ex.ToString)
            End Try
        End Using
        Return myResults

    End Function



End Class



Public Class MineralItemList
    Inherits List(Of MineralItem)


End Class
