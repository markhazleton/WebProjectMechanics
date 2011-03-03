Public Class wpmLocationTrailList
    Inherits List(Of wpmLocationTrail)
    Public Sub New()
        
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
        
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of wpmLocationTrail))
        MyBase.New(collection)
        
    End Sub
End Class
