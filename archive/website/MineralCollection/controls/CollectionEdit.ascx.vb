Imports WebProjectMechanics
Imports wpmMineralCollection

Public Class MineralCollection_CollectionEdit
    Inherits MineralCollection_EditUserControl

    Public Const STR_ListURL As String = "/MineralCollection/admin.aspx?Type=Collection"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfCollectionID.Value = GetProperty("CollectionID", -1)
        wpm_SetContactID("999")
        If Not IsPostBack Then
            If CInt(hfCollectionID.Value) > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New DataController()
                    Dim myCollections = (From i In myCon.Collections Where i.CollectionID = hfCollectionID.Value).ToList()
                    Dim MyCollection As New wpmMineralCollection.Collection
                    If myCollections.Count > 0 Then
                        MyCollection = myCollections(0)
                    Else
                        Throw New Exception("CollectionID is not found")
                    End If

                    With MyCollection
                        tbCollectionDS.Text = .CollectionDS
                        tbCollectionNM.Text = .CollectionNM
                    End With
                End Using
            ElseIf CInt(hfCollectionID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                tbCollectionDS.Text = String.Empty
                tbCollectionNM.Text = String.Empty
                cmd_Update.Visible = False
                cmd_Insert.Visible = True
                cmd_Delete.Visible = False
                cmd_Cancel.Visible = True
            Else
                ' Show the list
                pnlEdit.Visible = False
                dtList.Visible = True

                Dim myObjects As New List(Of Object)
                Using myCon As New DataController()
                    myObjects.AddRange((From i In myCon.Collections Select i.CollectionID, i.CollectionNM, i.CollectionDS, i.ModifiedDT, i.ModifiedID, i.CollectionItems.Count).ToList())
                End Using
                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Collections  (<a href='/MineralCollection/admin.aspx?Type=Collection&CollectionID=0'>Add New Collection</a>  )",
                                                                    .DetailFieldName = "CollectionNM",
                                                                    .DetailKeyName = "CollectionID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=Collection&CollectionID={0}"}

                myTableHeader.AddHeaderItem("CollectionDS", "CollectionDS")
                myTableHeader.AddHeaderItem("ModifiedDT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("Specimens", "Count", "/MineralCollection/Admin.aspx?Type=Specimen&CollectionID={0}", "CollectionID", "Count")
                dtList.BuildTable(myTableHeader, myObjects)


            End If
        End If

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        OnCancelled(Me)
        Response.Redirect(STR_ListURL)

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs) Handles cmd_Update.Click
        Dim myCon As New DataController()
        Dim myCollection = (From i In myCon.Collections Where i.CollectionID = hfCollectionID.Value).Single
        With myCollection
            .CollectionNM = tbCollectionNM.Text.Trim
            .CollectionDS = tbCollectionDS.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCon As New DataController()
        Dim myCollection As New wpmMineralCollection.Collection() With {.CollectionNM = tbCollectionNM.Text.Trim,
                                                                        .CollectionDS = tbCollectionDS.Text.Trim,
                                                                        .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1),
                                                                        .ModifiedDT = Now()}
        myCon.Collections.InsertOnSubmit(myCollection)
        If SaveChanges(AlertBox) Then
            OnUpdated(Me)
            Response.Redirect(STR_ListURL)
        End If
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCon As New DataController()
        Dim myCollection = (From i In myCon.Collections Where i.CollectionID = hfCollectionID.Value).Single

        myCon.Collections.DeleteOnSubmit(myCollection)
        If SaveChanges(AlertBox) Then
            OnUpdated(Me)
            Response.Redirect(STR_ListURL)
        End If
    End Sub
End Class
