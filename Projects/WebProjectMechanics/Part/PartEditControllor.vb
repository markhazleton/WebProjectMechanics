
Public Class PartEditControllor
    Private ReadOnly myPartEdit As IPartEdit
    Private ReadOnly myPartDisplay As IPartDisplay
    Private myPart As IPart

    Public Sub New(myView As Object)
        myPartDisplay = TryCast(myView, IPartDisplay)
        myPartEdit = TryCast(myView, IPartEdit)
        myPart = TryCast(myView, IPart)
    End Sub
    Public Sub New()
        
    End Sub

    Public Sub SavePart()
        Dim myPartBL As PartBusinessLogic = New PartBusinessLogic(myPartEdit)
        Dim ErrorMessage As String = String.Empty
        If myPartBL.IsValid(ErrorMessage) Then
            myPartBL.UpdatePart()
            myPartEdit.UpdateComplete = True
        Else
            myPartEdit.UpdateError = ErrorMessage
        End If
    End Sub

    Public Sub GetPart(ByVal thePartID As String)
        If Not myPartDisplay Is Nothing Then
        End If
        If Not myPart Is Nothing Then
            myPart = New PartBusinessLogic(thePartID).SetPartI(myPart)
        End If
    End Sub

    Public Sub GetParts(ByRef myActiveSite As ActiveCompany)
        myPartDisplay.PartList = myActiveSite.PartList
    End Sub

End Class
