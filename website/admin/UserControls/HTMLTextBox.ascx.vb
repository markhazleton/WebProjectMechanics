Public Class admin_UserControls_HTMLTextBox
    Inherits UserControl
    Implements WebProjectMechanics.IHTMLControl

    ' Method to get HTML content from the editor
    Public Function GetHTML() As String Implements WebProjectMechanics.IHTMLControl.GetHTML
        Return Request.Form("editor1")
    End Function

    ' Method to set the initial HTML content in the editor
    Public Sub SetHTML(myHTML As String) Implements WebProjectMechanics.IHTMLControl.SetHTML
        ' CKEditor 5 uses a standard textarea with an ID that matches the element ID in the JS
        litHTMLControl.Text = String.Format("<textarea id=""editor1"" name=""editor1"">{0}</textarea>", myHTML)
    End Sub
End Class
