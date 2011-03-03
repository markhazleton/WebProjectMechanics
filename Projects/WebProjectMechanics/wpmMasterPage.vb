Imports System.Text
Imports System.Web.UI.WebControls

Public Class wpmMasterPage
    Inherits System.Web.UI.MasterPage
    Public sExport As String = ""

    ''' <summary>
    ''' GetFileMenu - Returns HTML List of all files in the current directory
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetFileMenu() As String
        Dim sbReturn As New StringBuilder()
        sbReturn.Append(WebProjectMechanics.wpmFileProcessing.GetDirectoryMenuHTML(Page.TemplateSourceDirectory))
        sbReturn.Append(WebProjectMechanics.wpmFileProcessing.GetASPXMenuHTML(HttpContext.Current.Server.MapPath(Page.TemplateSourceDirectory)))
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


End Class
