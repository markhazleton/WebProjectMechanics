Imports WebProjectMechanics

Partial Class SiteList
    Inherits wpmPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            file_DatabindListbox(Server.MapPath(wpmApp.Config.ConfigFolder), ".mdb")
        End If
    End Sub

    Private Sub file_DatabindListbox(ByVal sPath As String, ByVal sExt As String)
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                Dim li As New ListItem
                li.Text = IO.Path.GetFileName(baseFile)
                li.Value = baseFile
                myFileListBox.Items.Add(li)
            End If
        Next
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        AccessDataSource.DataFile = myFileListBox.SelectedValue
        GridView1.DataBind()
    End Sub


    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        AccessDataSource.DataFile = myFileListBox.SelectedValue
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim mySB As New StringBuilder(String.Empty)
        mySB.Append(GridView1.SelectedRow.Cells(1).Text & "<br/>")
        mySB.Append(GridView1.SelectedRow.Cells(2).Text & "<br/>")
        mySB.Append(GridView1.SelectedRow.Cells(3).Text & "<br/>")
        MyHTML.Text = mySB.ToString
    End Sub
End Class
