Imports System.ComponentModel
Imports System.Web


<DataObject(True)> _
Public Class wpmImageAdapter
    Private Shared Property _data() As wpmImageList
        Get
            If HttpContext.Current.Session("_wpmImageList") Is Nothing Then
                Return New wpmImageList()
            Else
                Return DirectCast(HttpContext.Current.Session("_wpmImageList"), wpmImageList)
            End If
        End Get
        Set(ByVal value As wpmImageList)
            HttpContext.Current.Session("_wpmImageList") = value
        End Set
    End Property

    <DataObjectMethod(DataObjectMethodType.[Select], True)> _
    Public Shared Function GetAll() As wpmImageList
        Return _data
    End Function

    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Shared Sub DeleteImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Remove(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Sub UpdateImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Remove(Image)
        ImageList.Add(Image)
        _data = ImageList
    End Sub

    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Sub InsertImage(ByVal Image As wpmImage)
        Dim ImageList As wpmImageList = _data
        ImageList.Add(Image)
        _data = ImageList
    End Sub
End Class