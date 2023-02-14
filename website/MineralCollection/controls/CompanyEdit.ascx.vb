Imports WebProjectMechanics
Public Class MineralCollection_CompanyEdit
    Inherits ApplicationUserControl

    Public Const STR_ListURL As String = "/MineralCollection/admin.aspx?Type=Company"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            hfCompanyID.Value = GetProperty("CompanyID", "-1")
            If CInt(hfCompanyID.Value) > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCompanies = (From i In myCon.Companies Where i.CompanyID = hfCompanyID.Value).ToList()
                    Dim MyCompany As New wpmMineralCollection.Company
                    If myCompanies.Count > 0 Then
                        MyCompany = myCompanies(0)
                    Else
                        Throw New Exception("CompanyID is not found")
                    End If
                    With MyCompany
                        tbCompanyDS.Text = .CompanyDS
                        tbCompanyNM.Text = .CompanyNM
                    End With
                End Using
            ElseIf CInt(hfCompanyID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                tbCompanyDS.Text = String.Empty
                tbCompanyNM.Text = String.Empty
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True
                Dim myObjects As New List(Of Object)
                Using myCon As New wpmMineralCollection.DataController()
                    myObjects.AddRange((From i In myCon.Companies Select i.CompanyID,i.CompanyNM,i.CompanyDS,i.ModifiedDT,i.ModifiedID,i.CollectionItems.Count).ToList())
                End Using
                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Companies (Purchased From) (<a href='/MineralCollection/admin.aspx?Type=Company&CompanyID=0'>Add New Company</a>  )",
                                                                    .DetailFieldName = "CompanyNM",
                                                                    .DetailKeyName = "CompanyID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=Company&CompanyID={0}"}

                myTableHeader.AddHeaderItem("CompanyDS", "CompanyDS")
                myTableHeader.AddHeaderItem("ModifiedDT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("Specimens Purchased", "Count","/MineralCollection/Admin.aspx?Type=Specimen&CompanyID={0}","CompanyID","Count")
                dtList.BuildTable(myTableHeader, myObjects)
            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myCompany = (From i In myCon.Companies Where i.CompanyID = CInt(hfCompanyID.Value)).Single
        With myCompany
            .CompanyNM = tbCompanyNM.Text.Trim
            .CompanyDS = tbCompanyDS.Text.Trim
            .ModifiedID = wpm_GetDBInteger(Session("ContactID"), 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myCompany As New wpmMineralCollection.Company
        With myCompany
            .CompanyNM = tbCompanyNM.Text.Trim
            .CompanyDS = tbCompanyDS.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.Companies.InsertOnSubmit(myCompany)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myCompany = (From i In myCon.Companies Where i.CompanyID = CInt(hfCompanyID.Value)).Single
        myCon.Companies.DeleteOnSubmit(myCompany)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

End Class
