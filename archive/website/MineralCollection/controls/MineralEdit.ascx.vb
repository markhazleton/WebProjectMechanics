Imports System
Imports WebProjectMechanics
Public Class MineralCollection_MineralEdit
    Inherits ApplicationUserControl
    Public Const STR_ListURL As String = "/MineralCollection/admin.aspx?Type=Mineral"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hfMineralID.Value = GetIntegerProperty("MineralID", -1)
        If Not IsPostBack Then
            If CInt(hfMineralID.Value) > 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Edit Mode
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
                Using myCon As New wpmMineralCollection.DataController()
                    Dim myMinerals = (From i In myCon.Minerals Where i.MineralID = hfMineralID.Value).ToList()
                    Dim MyMineral As New wpmMineralCollection.Mineral
                    If myMinerals.Count > 0 Then
                        MyMineral = myMinerals(0)
                    Else
                        Throw New Exception("MineralID is not found")
                    End If

                    With MyMineral
                        tbMineralDS.Text = .MineralDS
                        tbMineralNM.Text = .MineralNM
                        tbWikipediaURL.Text = .WikipediaURL
                    End With
                End Using
            ElseIf CInt(hfMineralID.Value) = 0 Then
                pnlEdit.Visible = True
                dtList.Visible = False
                ' Insert Mode
                tbMineralDS.Text = String.Empty
                tbMineralNM.Text = String.Empty
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
                    myObjects.AddRange((From i In myCon.Minerals Select i.MineralID, i.MineralNM, i.MineralDS, i.ModifiedDT, i.ModifiedID, i.WikipediaURL, PrimaryMineralCount = i.CollectionItems.Count, SecondaryMineralCount = i.CollectionItemMinerals.Count).ToList())
                End Using
                Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Minerals  (<a href='/MineralCollection/admin.aspx?Type=Mineral&MineralID=0'>Add New Mineral</a>  )",
                                                                    .DetailFieldName = "MineralNM",
                                                                    .DetailKeyName = "MineralID",
                                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=Mineral&MineralID={0}"}

                myTableHeader.AddLinkHeaderItem("Wikipedia URL", "WikipediaURL","{0}","WikipediaURL","WikipediaURL")
                myTableHeader.AddHeaderItem("Modified DT", "ModifiedDT")
                myTableHeader.AddLinkHeaderItem("Primary Count", "PrimaryMineralCount", "/MineralCollection/Admin.aspx?Type=Specimen&MineralID={0}", "MineralID", "PrimaryMineralCount")
                myTableHeader.AddLinkHeaderItem("Secondary Count", "SecondaryMineralCount", "/MineralCollection/Admin.aspx?Type=Specimen&MineralID={0}", "MineralID", "SecondaryMineralCount")
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
        Dim myMineral = (From i In myCon.Minerals Where i.MineralID = hfMineralID.Value).Single
        With myMineral
            .MineralNM = tbMineralNM.Text.Trim
            .MineralDS = tbMineralDS.Text.Trim
            .WikipediaURL = tbWikipediaURL.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs) Handles cmd_Insert.Click
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myMineral As New wpmMineralCollection.Mineral
        With myMineral
            .MineralNM = tbMineralNM.Text.Trim
            .MineralDS = tbMineralDS.Text.Trim
            .WikipediaURL = tbWikipediaURL.Text.Trim
            .ModifiedID = wpm_GetDBInteger(wpm_ContactID, 1)
            .ModifiedDT = Now()
        End With
        myCon.Minerals.InsertOnSubmit(myMineral)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs) Handles cmd_Delete.Click
        ' Remind to set JS Are You Sure on Delete!
        Dim myCon As New wpmMineralCollection.DataController()
        Dim myMineral = (From i In myCon.Minerals Where i.MineralID = hfMineralID.Value).Single
        myCon.Minerals.DeleteOnSubmit(myMineral)
        myCon.SubmitChanges()
        OnUpdated(Me)
        Response.Redirect(STR_ListURL)
    End Sub

End Class
