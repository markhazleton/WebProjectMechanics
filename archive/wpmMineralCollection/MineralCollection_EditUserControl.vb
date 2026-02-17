
Public Class MineralCollection_EditUserControl
    Inherits ApplicationUserControl
    Public myCon As New DataController()

    Public Function SaveChanges(ByRef myAlert As IAlertBox) As Boolean
        Dim bReturn As Boolean = True
        myCon.SubmitChanges()
        If myCon.ReturnValue <> String.Empty Then
            bReturn = False
            myAlert.message = myCon.ReturnValue
            myAlert.alertType = AlertBoxType.danger
            myAlert.boldnote = "Save to Database Failed"
            myAlert.SetAlert()
        End If
        Return bReturn
    End Function

End Class




