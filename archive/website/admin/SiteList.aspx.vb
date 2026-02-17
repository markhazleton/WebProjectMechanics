Imports WebProjectMechanics
Imports System.Data
Imports System.Data.OleDb

Partial Class SiteList
    Inherits ApplicationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            file_DatabindListbox(Server.MapPath(wpm_SiteConfig.ConfigFolder), ".mdb")
        End If
    End Sub

    Private Sub file_DatabindListbox(ByVal sPath As String, ByVal sExt As String)
        Dim mySB As New StringBuilder
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                Dim li As New ListItem() With {.Text = IO.Path.GetFileName(baseFile), .Value = baseFile}
                myFileListBox.Items.Add(li)
            End If
        Next
        BindTheGridview()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        BindTheGridview()
    End Sub

    Private Sub BindTheGridview()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("AccessConnectionString").ToString()

        Dim dbPath As String = myFileListBox.SelectedItem.Text

        connStr = connStr.Replace("wpm-demo.mdb", dbPath)
        Using conn As New OleDbConnection(connStr)
            Dim query As String = "SELECT [CompanyID], [CompanyName], [SiteURL] FROM [Company] order by [CompanyName] asc"
            Using adapter As New OleDbDataAdapter(query, conn)
                Dim dataTable As New DataTable()
                adapter.Fill(dataTable)
                GridView1.DataSource = dataTable
                GridView1.DataBind()
            End Using
        End Using
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        BindTheGridview()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim mySB As New StringBuilder(String.Empty)
        Dim mylist As New DomainConfigurations
        mylist.Configuration.CompanyID = GridView1.SelectedRow.Cells(1).Text
        mylist.Configuration.SQLDBConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|{0};", myFileListBox.SelectedItem.Text)
        mylist.Configuration.DomainName = Replace(Replace(GridView1.SelectedRow.Cells(3).Text, "www.", ""), "http://", "")
        mylist.Configuration.AccessDatabasePath = Replace(Replace(myFileListBox.SelectedValue, HttpContext.Current.Server.MapPath("/"), "/"), "\", "/")
        If Not FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "sites") Then
            FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "sites")
        End If
        DomainConfigurations.Save(String.Format("{1}\sites\{0}.xml", mylist.Configuration.DomainName, Server.MapPath("/App_Data")), mylist)
        mySB.Append(GridView1.SelectedRow.Cells(1).Text & "<br/>")
        mySB.Append(GridView1.SelectedRow.Cells(2).Text & "<br/>")
        mySB.Append(GridView1.SelectedRow.Cells(3).Text & "<br/>")
        MyHTML.Text = mySB.ToString
    End Sub







End Class
