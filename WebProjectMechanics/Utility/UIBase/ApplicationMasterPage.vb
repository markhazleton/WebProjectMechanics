Imports System.Text
Imports System.Web.UI.WebControls

Public Class ApplicationMasterPage
    Inherits MasterPage
    Public sExport As String = String.Empty
    Public StartTimer As Long
    Public myCompany As ActiveCompany
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        StartTimer = Environment.TickCount
        '        mySession = New ApplicationSession(Session)
        myCompany = New ActiveCompany()
    End Sub

    ''' <summary>
    ''' GetFileMenu - Returns HTML List of all files in the current directory
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetFileMenu() As String
        Dim sbReturn As New StringBuilder()
        sbReturn.Append(FileProcessing.GetDirectoryMenuHTML(Page.TemplateSourceDirectory))
        sbReturn.Append(FileProcessing.GetASPXMenuHTML(HttpContext.Current.Server.MapPath(Page.TemplateSourceDirectory)))
        Return sbReturn.ToString
    End Function
    ''' <summary>
    ''' Returns a MenuItem for a given name and path
    ''' </summary>
    ''' <param name="MenuName"></param>
    ''' <param name="MenuPath"></param>
    ''' <returns></returns>
    Public Function GetMenuItem(ByVal MenuName As String, ByVal MenuPath As String) As MenuItem
        Dim myMenuItem As New MenuItem() With {.Text = MenuName, .Value = MenuName, .NavigateUrl = MenuPath}
        If myMenuItem.NavigateUrl = Page.TemplateSourceDirectory Then
            myMenuItem.Selected = True
        Else
            myMenuItem.Selected = False
        End If
        Return myMenuItem
    End Function
    Public Function CalcElapsedTime(ByVal tm As Long) As String
        Return String.Format("<div>page processing time: {0} seconds</div>", (Environment.TickCount - tm) / 1000)
    End Function



End Class
