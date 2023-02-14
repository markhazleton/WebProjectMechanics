Imports WebProjectMechanics

Public Class MineralCollection_CollectionItemMineralEdit
    Inherits ApplicationUserControl
    Public Property reqCollectionItemMineralID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqCollectionItemMineralID = GetIntegerProperty("CollectionItemMineralID", -1)
        hfCollectionItemID.Value = GetIntegerProperty("CollectionItemID", -1)
        hfCollectionItemMineralID.Value = GetIntegerProperty("CollectionItemMineralID", -1)

        If Not IsPostBack Then
            If reqCollectionItemMineralID > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                LoadMineralDropDown()

                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myCollectionItemMinerals = (From i In myCon.CollectionItemMinerals Where i.CollectionItemMineralID = reqCollectionItemMineralID).ToList()
                    Dim myCollectionItemMineral As New wpmMineralCollection.CollectionItemMineral
                    If myCollectionItemMinerals.Count > 0 Then
                        myCollectionItemMineral = myCollectionItemMinerals(0)
                    Else
                        Throw New Exception("CompanyID is not found")
                    End If

                    With myCollectionItemMineral
                        ddlMineral.SelectedValue = .MineralID
                        tbPosition.Text = .Position
                    End With
                End Using
            ElseIf reqCollectionItemMineralID = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                LoadMineralDropDown()
                ' Insert Mode
                tbPosition.Text = 1
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Using myCon As New wpmMineralCollection.DataController()
                    Dim myMinerals As New List(Of Object)
                    myMinerals.AddRange((From i In myCon.CollectionItemMinerals Where i.CollectionItemID = hfCollectionItemID.Value Select i.CollectionItemMineralID, i.CollectionItem.CollectionItemID, i.Mineral.MineralID, i.Mineral.MineralNM).ToList())
                    Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Associated Minerals (<a href='/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=" & hfCollectionItemID.Value &"&CollectionItemMineralID=0'>Add Associated Mineral</a>)",
                                                                        .DetailFieldName = "MineralNM",
                                                                        .DetailKeyName = "CollectionItemMineralID",
                                                                        .DetailPath = "/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=" & hfCollectionItemID.Value & "&CollectionItemMineralID={0}"}
                    dtList.BuildTable(myTableHeader, myMinerals)
                End Using





            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionMineralItem = (From i In myCon.CollectionItemMinerals Where i.CollectionItemMineralID = reqCollectionItemMineralID).Single
            With myCollectionMineralItem
                .MineralID = ddlMineral.SelectedValue
                .Position = tbPosition.Text
                .ModifiedDT = Now()
                .ModifiedID = wpm_GetDBInteger(Session("ContactID"), 1)
            End With
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionMineralItem As New wpmMineralCollection.CollectionItemMineral
            With myCollectionMineralItem
                .CollectionItemID = hfCollectionItemID.Value
                .MineralID = ddlMineral.SelectedValue
                .Position = tbPosition.Text
                .ModifiedDT = Now()
                .ModifiedID = wpm_GetDBInteger(Session("ContactID"), 1)
            End With
            myCon.CollectionItemMinerals.InsertOnSubmit(myCollectionMineralItem)
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Using myCon As New wpmMineralCollection.DataController()
            Dim myCollectionMineralItem = (From i In myCon.CollectionItemMinerals Where i.CollectionItemMineralID = reqCollectionItemMineralID).Single
            myCon.CollectionItemMinerals.DeleteOnSubmit(myCollectionMineralItem)
            myCon.SubmitChanges()
        End Using
        OnUpdated(Me)
        Response.Redirect(String.Format("/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}", hfCollectionItemID.Value))
    End Sub

    Private Sub LoadMineralDropDown()
        Using myCon As New wpmMineralCollection.DataController()
            ddlMineral.DataSource = (From i In myCon.Minerals Order By i.MineralNM).ToList()
            ddlMineral.DataTextField = "MineralNM"
            ddlMineral.DataValueField = "MineralID"
            ddlMineral.DataBind()
        End Using
    End Sub

End Class
