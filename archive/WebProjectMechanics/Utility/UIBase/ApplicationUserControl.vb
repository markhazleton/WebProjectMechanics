Imports System.Web.UI.WebControls

Public Class ApplicationUserControl
    Inherits UserControl
    Event cmd_Canceled(sender As Object)
    Event cmd_Updated(sender As Object)
    Public masterPage As ApplicationMasterPage
    Public strErrorMessage As String = String.Empty
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        masterPage = DirectCast(Me.Parent.Page.Master, ApplicationMasterPage)
    End Sub

    Public Const sIndexFileNameTemplate As String = "{0}\sites\{1}\{1}-site-file.xml"
    Protected Overridable Sub OnUpdated(ByVal sender As Object)
        If String.IsNullOrWhiteSpace(strErrorMessage) Then
            RaiseEvent cmd_Updated(sender)
        End If
    End Sub
    Protected Overridable Sub OnCancelled(ByVal sender As Object)
        RaiseEvent cmd_Canceled(sender)
    End Sub

    Public ReturnURL As String = String.Empty

    Public Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = String.Empty
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

    Public Function wpm_LoadRoleDropDown(ByRef myCMB As DropDownList, ByVal CurrentValue As String, ByVal bRequired As Boolean) As Boolean
        Dim CompanyList As List(Of LookupItem) = ApplicationDAL.GetRoleLookupList()
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
End Class