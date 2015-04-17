﻿Imports WebProjectMechanics

Public Class admin_maint_ParameterType
    Inherits ApplicationUserControl


    ' Parameter Type 
        Public Const STR_SiteParameterTypeID As String = "SiteParameterTypeID"
    Public Const STR_SELECTSiteParameterTypeList As String = "SELECT SiteParameterType.[SiteParameterTypeID], SiteParameterType.[SiteParameterTypeCD], SiteParameterType.[SiteParameterTypeDesc], SiteParameterType.[PageFileName] FROM SiteParameterType;"
    Public Const STR_SELECT_SiteParameterTypeBySiteParameterTypeID As String = "SELECT SiteParameterType.[SiteParameterTypeID], SiteParameterType.[SiteParameterTypeCD], SiteParameterType.[SiteParameterTypeDesc], SiteParameterType.[PageFileName] FROM SiteParameterType where [SiteParameterType].[SiteParameterTypeID]={0};"
    Public Const STR_UPDATE_SiteParameterType As String = "UPDATE SiteParameterType SET SiteParameterType.SiteParameterTypeCD = @SiteParameterTypeCD , SiteParameterType.SiteParameterTypeDesc = @SiteParameterTypeDesc, SiteParameterType.PageFileName = @PageFileName WHERE (((SiteParameterType.SiteParameterTypeID)=@SiteParameterTypeID));"
    Public Const STR_INSERT_SiteParameterType As String = "INSERT INTO SiteParameterType ( SiteParameterType.SiteParameterTypeCD , SiteParameterType.SiteParameterTypeDesc , SiteParameterType.PageFileName ) VALUES ( @SiteParameterTypeCD ,@SiteParameterTypeDesc, @PageFileName ); "
    Public Const STR_DELETE_SiteParameterType As String = "DELETE FROM [SiteParameterType] WHERE [SiteParameterType].[SiteParameterTypeID]={0};"

    Private Property reqSiteParameterTypeID As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        reqSiteParameterTypeID = wpm_GetIntegerProperty("ParameterTypeID", 0)
        litParameterTypeID.Text = reqSiteParameterTypeID
        If Not IsPostBack Then
            If reqSiteParameterTypeID > 0 Then
                ' Edit Mode
                pnlEdit.Visible = True
                dtList.Visible = False
                PopulateParameterType()
                cmd_Update.Visible = True
                cmd_Insert.Visible = False
                cmd_Delete.Visible = True
                cmd_Cancel.Visible = True
            Else
                If reqSiteParameterTypeID = -1 Then
                    pnlEdit.Visible = True
                    dtList.Visible = False
                    ' Insert Mode
                    cmd_Update.Visible = False
                    cmd_Insert.Visible = True
                    cmd_Delete.Visible = False
                    cmd_Cancel.Visible = True
                Else
                    ' Show the list
                    pnlEdit.Visible = False
                    dtList.Visible = True
                    Dim myListHeader As New DisplayTableHeader() With {.TableTitle = "Parameter Types "}
                    myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "Description", .Value = "ParameterTypeDS"})
                    myListHeader.DetailKeyName = "ParameterTypeID"
                    myListHeader.DetailFieldName = "ParameterTypeNM"
                    myListHeader.DetailPath = "/admin/maint/default.aspx?type=ParameterType&ParameterTypeID={0}"
                    Dim myList As New List(Of Object)
                    myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.RecordSource = "SiteParameterType" Select i).ToList())

                    dtList.BuildTable(myListHeader, myList)
                End If
            End If
        End If
    End Sub


    Public Sub PopulateParameterType()
        Dim myParameterType = (From i In masterPage.myCompany.SiteParameterList Where i.RecordSource = "SiteParameterType" And i.ParameterTypeID = litParameterTypeID.Text).SingleOrDefault
        With myParameterType
            tbParameterTypeNM.Text = .ParameterTypeNM
            tbParameterTypeDS.Text = .ParameterTypeDS
            tbParameterTemplate.Text = .ParameterTemplate
            tbSortOrder.Text = .SortOrder
            hfRecordSource.Value = .RecordSource


        End With

        Dim myListHeader As New DisplayTableHeader() With {.TableTitle = String.Format("Parameter Usage ( <a href='/admin/maint/default.aspx?type=Parameter&ParameterID=NEW&ParameterTypeID={0}'>New Parameter Usage</a>)", myParameterType.ParameterTypeID)}
        myListHeader.AddHeaderItem("Location", "LocationNM", "/admin/maint/default.aspx?Type=Parameter&LocationID={0}", "LocationID", "LocationNM")
        myListHeader.AddHeaderItem("Company", "CompanyNM", "/admin/maint/default.aspx?Type=Parameter&CompanyID={0}", "CompanyID", "CompanyNM")
        myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "ParameterTypeDS", .Value = "ParameterTypeDS"})
        myListHeader.HeaderItems.Add(New DisplayTableHeaderItem With {.KeyField = False, .Name = "SortOrder", .Value = "SortOrder"})
        myListHeader.DetailKeyName = "ParameterID"
        myListHeader.DetailFieldName = "ParameterNM"
        myListHeader.DetailPath = "/admin/maint/default.aspx?type=Parameter&ParameterID={0}"
        Dim myList As New List(Of Object)
        myList.AddRange((From i In masterPage.myCompany.SiteParameterList Where i.ParameterTypeID = myParameterType.ParameterTypeID And i.ParameterID <> myParameterType.ParameterID Select i).ToList())
        dtParameterUsage.BuildTable(myListHeader, myList)

    End Sub

    Protected Sub cmd_Update_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)

    End Sub

    Protected Sub cmd_SaveNew_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)

    End Sub

    Protected Sub cmd_Insert_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)

    End Sub

    Protected Sub cmd_Cancel_Click(sender As Object, e As EventArgs)
        OnCancelled(me)

    End Sub

    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        OnUpdated(Me)

    End Sub
End Class
