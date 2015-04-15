
Public Class admin_UserControls_HTMLTextBox
    Inherits System.Web.UI.UserControl
    Implements WebProjectMechanics.IHTMLControl

    Public Function GetHTML() As String Implements WebProjectMechanics.IHTMLControl.GetHTML
        Return Request.Form("editor1")
    End Function

    Public Sub SetHTML(myHTML As String) Implements WebProjectMechanics.IHTMLControl.SetHTML
        litHTMLControl.Text = String.Format("<textarea name=""editor1"">{0}</textarea>", myHTML)
    End Sub
End Class
