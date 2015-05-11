Imports System.Web.UI.WebControls

Public Class ApplicationUserControl
    Inherits UserControl
    Event cmd_Canceled(sender As Object)
    Event cmd_Updated(sender As Object)
    Public masterPage As ApplicationMasterPage
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        masterPage = DirectCast(Me.Parent.Page.Master, ApplicationMasterPage)
    End Sub

    Public Const sIndexFileNameTemplate As String = "{0}\sites\{1}\{1}-site-file.xml"
    Protected Overridable Sub OnUpdated(ByVal sender As Object)
        RaiseEvent cmd_Updated(sender)
    End Sub
    Protected Overridable Sub OnCancelled(ByVal sender As Object)
        RaiseEvent cmd_Canceled(sender)
    End Sub

    Public ReturnURL As String = String.Empty

    Public Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function
    Protected Function GetIntegerProperty(ByVal myProperty As String, ByVal curValue As Integer) As Integer
        Dim myValue As Integer
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = CInt(Request.QueryString(myProperty))
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = CInt(Request.Form.Item(myProperty))
        Else
            myValue = curValue
        End If
        Return myValue
    End Function

    Public Shared Function GetContentCell(ByVal CSSClass As String, ByVal Content As String) As TableCell
        Dim mycell As New TableCell() With {.CssClass = CSSClass, .Width = New Unit(300)}
        mycell.Controls.Add(New Label With {.Text = Content})
        Return mycell
    End Function
    Public Shared Function GetControlCell(ByVal CSSClass As String, ByVal Content As Control) As TableCell
        Dim mycell As New TableCell() With {.CssClass = CSSClass, .Width = New Unit(500)}
        mycell.Controls.Add(Content)
        Return mycell
    End Function


    Public Function wpm_LoadArticleDropDown(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByRef Articles As ArticleList, ByVal bRequired As Boolean) As Boolean
        myCMB.Items.Clear()
        myCMB.Enabled = True
        If Articles.Count > 1 Then
            If Not bRequired Then
                myCMB.Items.Add(New ListItem() With {.Value = String.Empty, .Text = "Please Select", .Selected = True})
            End If
            For Each myArticle As Article In Articles
                myCMB.Items.Add(New ListItem() With {.Text = myArticle.ArticleName, .Value = myArticle.ArticleID.ToString, .Selected = False})
            Next
            myCMB.SelectedValue = CurrentValue
        Else
            If Articles.Count = 1 Then
                myCMB.Items.Add(New ListItem() With {.Text = Articles(0).ArticleName, .Value = Articles(0).ArticleID.ToString, .Selected = True})
                myCMB.Enabled = False
            End If
        End If
        Return True
    End Function

    Public Function wpm_LoadLocationDropDown(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByRef Locations As LocationList, ByVal bRequired As Boolean) As Boolean
        myCMB.Items.Clear()
        myCMB.Enabled = True
        If Locations.Count > 1 Then
            If Not bRequired Then
                myCMB.Items.Add(New ListItem() With {.Value = String.Empty, .Text = "Please Select", .Selected = True})
            End If
            For Each myLocation As Location In Locations
                myCMB.Items.Add(New ListItem() With {.Text = myLocation.LocationName, .Value = myLocation.LocationID, .Selected = False})
            Next
            myCMB.SelectedValue = CurrentValue
        Else
            If Locations.Count = 1 Then
                myCMB.Items.Add(New ListItem() With {.Text = Locations(0).LocationName, .Value = Locations(0).LocationID, .Selected = True})
                myCMB.Enabled = False
            End If
        End If
        Return True
    End Function


    Public Function wmp_LoadCompanyDropDow(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByVal bRequired As Boolean) As Boolean
        Dim CompanyList As List(Of LookupItem) = ApplicationDAL.GetCompanyLookupList()

        myCMB.Items.Clear()
        myCMB.Enabled = True
        If CompanyList.Count > 0 Then
            If bRequired Then
                If CompanyList.Count = 1 Then
                    myCMB.Items.Add(New ListItem() With {.Text = CompanyList(0).Name, .Value = CompanyList(0).Value, .Selected = True})
                    myCMB.Enabled = False
                Else
                    For Each myCompany As LookupItem In CompanyList
                        myCMB.Items.Add(New ListItem() With {.Text = myCompany.Name, .Value = myCompany.Value, .Selected = False})
                    Next
                End If
            Else
                myCMB.Items.Add(New ListItem() With {.Value = String.Empty, .Text = "All Sites", .Selected = True})
                For Each myCompany As LookupItem In CompanyList
                    myCMB.Items.Add(New ListItem() With {.Text = myCompany.Name, .Value = myCompany.Value, .Selected = False})
                Next
            End If
            myCMB.SelectedValue = CurrentValue
        Else
            myCMB.Enabled = False
        End If
        Return True
    End Function

    Public Sub SetupDropdown(ByRef LookupItemList As List(Of LookupItem), ByRef cmb As DropDownList)
        cmb.AppendDataBoundItems = True
        cmb.DataSource = LookupItemList
        cmb.DataTextField = "Name"
        cmb.DataValueField = "Value"
        cmb.DataBind()
        cmb.SelectedIndex = -1
    End Sub

    Public Shared Function GetDBDate(ByVal dbObject As Object) As Nullable(Of Date)
        If IsDBNull(dbObject) Then
            Return New Nullable(Of Date)()
        ElseIf dbObject Is Nothing Then
            Return New Nullable(Of Date)()
        ElseIf String.IsNullOrWhiteSpace(dbObject.ToString) Then
            Return New Nullable(Of Date)()
        ElseIf Information.IsDate(dbObject.ToString) Then
            Return CDate(dbObject)
        Else
            Return New Nullable(Of Date)()
        End If
    End Function
    Public Sub LoadDisplyTableParameter(ByVal CompanyID As Integer, ByVal ParameterTypeID As Integer, ByRef myControl As UserControl)
        Dim myListHeader As New DisplayTableHeader()
        Dim myList As New List(Of Object)()
        Dim sWhere As String = String.Format("where ( Company.CompanyID is null or Company.CompanyID = {0}) and (SiteCategoryType.SiteCategoryTypeID={1} or SiteCategoryType.siteCategoryTypeID is null) ", wpm_CurrentSiteID, masterPage.myCompany.SiteCategoryTypeID)
        If GetProperty("ALL", "FALSE") = "TRUE" Then
            sWhere = String.Empty
        ElseIf ParameterTypeID > 0 Then
            sWhere = String.Format("where SiteParameterType.SiteParameterTypeID = {0} ", ParameterTypeID )
        ElseIf CompanyID > 0 Then
            sWhere = String.Format("where Company.CompanyID = {0} ", CompanyID)
        ElseIf wpm_GetIntegerProperty("SiteCategoryTypeID", 0) > 0 Then
            sWhere = String.Format("where SiteCategoryType.SiteCategoryTypeID = {0} ", wpm_GetIntegerProperty("SiteCategoryTypeID", 0))
        End If
        Dim STR_SELECT_ParameterList As String = String.Format("SELECT 'TP-' & CompanySiteTypeParameter.CompanySiteTypeParameterID AS ParameterID, CompanySiteTypeParameter.CompanyID, CompanySiteTypeParameter.SiteParameterTypeID, CompanySiteTypeParameter.SortOrder, CompanySiteTypeParameter.ParameterValue, CompanySiteTypeParameter.SiteCategoryID AS LocationID, CompanySiteTypeParameter.SiteCategoryGroupID AS LocationGroupID, CompanySiteTypeParameter.SiteCategoryTypeID, Company.CompanyName AS CompanyNM, SiteCategoryGroup.SiteCategoryGroupNM AS LocationGroupNM, SiteCategory.CategoryName AS LocationNM, SiteParameterType.SiteParameterTypeNM, 'CompanySiteTypeParameter' AS RecordSource, SiteCategoryType.SiteCategoryTypeNM FROM (SiteParameterType RIGHT JOIN (SiteCategory RIGHT JOIN (SiteCategoryGroup RIGHT JOIN (CompanySiteTypeParameter LEFT JOIN Company ON CompanySiteTypeParameter.CompanyID = Company.CompanyID) ON SiteCategoryGroup.SiteCategoryGroupID = CompanySiteTypeParameter.SiteCategoryGroupID) ON SiteCategory.SiteCategoryID = CompanySiteTypeParameter.SiteCategoryID) ON SiteParameterType.SiteParameterTypeID = CompanySiteTypeParameter.SiteParameterTypeID) LEFT JOIN SiteCategoryType ON CompanySiteTypeParameter.SiteCategoryTypeID = SiteCategoryType.SiteCategoryTypeID {0}  union SELECT 'SP-' & CompanySiteParameter.CompanySiteParameterID AS Expr1, CompanySiteParameter.CompanyID, CompanySiteParameter.SiteParameterTypeID, CompanySiteParameter.SortOrder, CompanySiteParameter.ParameterValue, CompanySiteParameter.PageID, CompanySiteParameter.SiteCategoryGroupID, Company.SiteCategoryTypeID, Company.CompanyName, SiteCategoryGroup.SiteCategoryGroupNM, Page.PageName, SiteParameterType.SiteParameterTypeNM, 'CompanySiteParameter' AS RecordSource, SiteCategoryType.SiteCategoryTypeNM FROM SiteCategoryType RIGHT JOIN (SiteParameterType RIGHT JOIN (Page RIGHT JOIN (SiteCategoryGroup RIGHT JOIN (CompanySiteParameter LEFT JOIN Company ON CompanySiteParameter.CompanyID = Company.CompanyID) ON SiteCategoryGroup.SiteCategoryGroupID = CompanySiteParameter.SiteCategoryGroupID) ON Page.PageID = CompanySiteParameter.PageID) ON SiteParameterType.SiteParameterTypeID = CompanySiteParameter.SiteParameterTypeID) ON SiteCategoryType.SiteCategoryTypeID = Company.SiteCategoryTypeID  {0} ", sWhere)
        myListHeader = New DisplayTableHeader() With {.TableTitle = "Parameter (<a href='/admin/maint/default.aspx?type=Parameter&ALL=TRUE'>All Parameters</a> , <a href='/admin/maint/default.aspx?type=Parameter&ParameterID=NEW'>Add New Parameter</a>)"}
        myListHeader.AddHeaderItem("Parameter Type", "ParameterTypeNM", "/admin/maint/default.aspx?Type=Parameter&ParameterTypeID={0}", "ParameterTypeID", "ParameterTypeNM")
        myListHeader.AddHeaderItem("Location", "LocationNM" )
        myListHeader.AddHeaderItem("Location Group", "LocationGroupID", "/admin/maint/default.aspx?Type=Parameter&LocationGroupID={0}", "LocationGroupID", "LocationGroupNM")
        myListHeader.AddHeaderItem("Site", "CompanyNM", "/admin/maint/default.aspx?Type=Parameter&CompanyID={0}", "CompanyID", "CompanyNM")
        myListHeader.AddHeaderItem("SiteCategoryTypeNM", "SiteCategoryTypeNM", "/admin/maint/default.aspx?Type=Parameter&SiteCategoryTypeID={0}", "SiteCategoryTypeID", "SiteCategoryTypeNM")
        myListHeader.AddHeaderItem("SortOrder", "SortOrder")
        myListHeader.DetailKeyName = "ParameterID"
        myListHeader.DetailFieldName = "ParameterNM"
        myListHeader.DetailPath = "/admin/maint/default.aspx?type=Parameter&ParameterID={0}"
        myList = New List(Of Object)()
        For Each myRow As DataRow In wpm_GetDataTable(String.Format("{0} ", STR_SELECT_ParameterList, wpm_CurrentSiteID), "Parameter").Rows
            myList.Add(New Parameter() With {.ParameterID = wpm_GetDBString(myRow("ParameterID")), .ParameterNM = String.Format("{0}-{1}", wpm_GetDBString(myRow("SiteParameterTypeNM")), wpm_GetDBString(myRow("ParameterID"))), .RecordSource = wpm_GetDBString(myRow("RecordSource")), .ParameterTypeID = wpm_GetDBInteger(myRow("SiteParameterTypeID")), .ParameterTypeNM = wpm_GetDBString(myRow("SiteParameterTypeNM")), .SiteCategoryTypeID = wpm_GetDBString(myRow("SiteCategoryTypeID")), .SiteCategoryTypeNM = wpm_GetDBString(myRow("SiteCategoryTypeNM")), .SortOrder = wpm_GetDBInteger(myRow("SortOrder")), .CompanyID = wpm_GetDBString(myRow("CompanyID")), .CompanyNM = wpm_GetDBString(myRow("CompanyNM")), .LocationID = wpm_GetDBString(myRow("LocationID")), .LocationNM = wpm_GetDBString(myRow("LocationNM")), .LocationGroupID = wpm_GetDBString(myRow("LocationGroupID")), .LocationGroupNM = wpm_GetDBString(myRow("LocationGroupNM"))})
        Next
        Dim myDisplayTable = TryCast(myControl, Icontrols_DisplayTable)
        If Not myDisplayTable Is Nothing Then
            myDisplayTable.BuildTable(myListHeader, myList)
        End If
    End Sub
End Class