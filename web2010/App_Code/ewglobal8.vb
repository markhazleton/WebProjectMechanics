Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 Project Class
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

	'
	' Global variables
	'
	Public Conn As cConnection

	Public Security As cAdvancedSecurity

	Public ObjForm As cFormObj	

	Public Language As cLanguage

	Public Shared gsLanguage As String

	Public Shared StartTimer As Long

	Public Rs As Object ' DataReader

	' Used by ValidateForm/ValidateSearch
	Public gsFormError As String, gsSearchError As String	

	' Used by *master.ascx
	Public gsMasterReturnUrl As String	

	' Used for export checking
	Public gsExport As String, gsExportFile As String

	Public gsEmailSender As String, gsEmailRecipient As String, gsEmailCc As String, gsEmailBcc As String, gsEmailSubject As String, gsEmailContent As String, gsEmailContentType AS String

	Public Shared gsEmailErrNo As String

	Public Shared gsEmailErrDesc As String

	' Used by system generated functions
	Public RsWrk As Object ' ArrayList of OrderedDictionary / DataReader

	Public sSqlWrk As String, sWhereWrk As String, jswrk As String, selwrk As String, emptywrk As Boolean

	Public arwrk As Object

	Public armultiwrk() As String

	' Global user functions
	' Page Loading event
	Public Sub Page_Loading()

		'HttpContext.Current.Response.Write("Page Loading")
	End Sub

	' Page Unloaded event
	Public Sub Page_Unloaded()

		'HttpContext.Current.Response.Write("Page Unloaded")
	End Sub

	Public Function MenuItem_Adding(ByRef Item As cMenuItem) As Boolean

		'HttpContext.Current.Response.Write(Item.AsString())
		' Return False if menu item not allowed

		Return True
	End Function
End Class