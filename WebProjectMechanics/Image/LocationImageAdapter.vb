Imports System.ComponentModel

<DataObject(True)> _
Public Class LocationImageAdapter
    Private Shared Property _data() As LocationImageList
        Get
            If HttpContext.Current.Session("_LocationImageList") Is Nothing Then
                Return New LocationImageList()
            Else
                Return DirectCast(HttpContext.Current.Session("_LocationImageList"), LocationImageList)
            End If
        End Get
        Set(ByVal value As LocationImageList)
            HttpContext.Current.Session("_LocationImageList") = value
        End Set
    End Property

    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
    Public Shared Function GetAll() As LocationImageList
        Return _data
    End Function

    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Shared Sub DeleteImage(ByVal Image As LocationImage)
        Dim ImageList As LocationImageList = _data
        ImageList.Images.Remove(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Sub UpdateImage(ByVal Image As LocationImage)
        Dim ImageList As LocationImageList = _data
        ImageList.Images.Remove(Image)
        ImageList.Images.Add(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Sub InsertImage(ByVal Image As LocationImage)
        Dim ImageList As LocationImageList = _data
        ImageList.Images.Add(Image)
        _data = ImageList
    End Sub
End Class