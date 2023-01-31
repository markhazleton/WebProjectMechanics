Imports System.Web

Public Class CollectionUserControl
    Inherits ApplicationUserControl
    Public Const STR_ImageFolder As String = "images"

    Public ReadOnly Property userID As Integer
        Get
            Return 1
        End Get
    End Property
    Public ReadOnly Property CollectionItems As Dictionary(Of Integer, String)
        Get
            Dim myItems As New Dictionary(Of Integer, String)
            Using mycon As New DataController
                For Each item In (From i In mycon.CollectionItems Order By i.SpecimenNumber Select i.CollectionItemID, i.SpecimenNumber, i.Nickname, i.Mineral.MineralNM)
                    myItems.Add(item.CollectionItemID, $"{item.SpecimenNumber} {item.Nickname} ({item.MineralNM})")
                Next
            End Using
            Return myItems
        End Get
    End Property


    Public Function GetStringValue(ByRef myObject As Object) As String
        If myObject Is Nothing Then
            Return String.Empty
        Else
            Try
                Return myObject.ToString
            Catch
                Return String.Empty
            End Try
        End If
    End Function


End Class
