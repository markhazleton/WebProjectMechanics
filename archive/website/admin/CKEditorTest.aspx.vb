Imports WebProjectMechanics

Partial Class admin_CKEditorTest
    Inherits System.Web.UI.Page
    Private myControl As IHTMLControl

    Protected Sub cmd_SaveHTML_Click(sender As Object, e As EventArgs)
        myControl = DirectCast(HTMLTextBox, IHTMLControl)
        litHTML.Text = String.Format("<xmp>{0}</xmp>", myControl.GetHTML())
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        myControl = DirectCast(HTMLTextBox, IHTMLControl)
        If IsPostBack Then
            myControl.SetHTML(myControl.GetHTML())
        Else
            myControl.SetHTML("<h1>Hi Mom</h1>")
        End If
    End Sub
End Class
