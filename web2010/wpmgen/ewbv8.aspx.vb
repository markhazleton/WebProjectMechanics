Imports System.Data
Imports System.Data.Common
Imports System.Xml
Imports System.IO
Imports System.Data.OleDb

'
' ASP.NET code-behind class (Blob View) 
'

Partial Class ewbv8
	Inherits AspNetMaker8_wpmWebsite

	'
	' ASP.NET Page_Load event
	'

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		Dim tbl As String = "", fld As String = ""
		Dim ft As String, fn As String, fs As Long
		Dim width As Integer, height As Integer, interpolation As Integer

		' Get resize parameters
		Dim resize As Boolean = (ew_Get("resize") <> "")
		If ew_Get("width") <> "" Then
			width = ew_ConvertToInt(ew_Get("width"))
		End If
		If ew_Get("height") <> "" Then
			height = ew_ConvertToInt(ew_Get("height"))
		End If
		If width <= 0 AndAlso height < 0 Then
			width = EW_THUMBNAIL_DEFAULT_WIDTH
			height = EW_THUMBNAIL_DEFAULT_HEIGHT
		End If
		If ew_Get("interpolation") <> "" Then
			interpolation = ew_ConvertToInt(ew_Get("interpolation"))
		Else
			interpolation = EW_THUMBNAIL_DEFAULT_INTERPOLATION
		End If

		' Resize image from physical file
		If ew_Get("fn") <> "" Then
			fn = ew_Get("fn")
			fn = Server.MapPath(fn)
			If File.Exists(fn) AndAlso ew_CheckFileType(fn) Then
				Response.BinaryWrite(ew_ResizeFileToBinary(fn, width, height, interpolation))
			End If
			Response.End()

		' Display image from Session
		Else
			If ew_Get("tbl") = "" OrElse ew_Get("fld") = "" Then Response.End()
			tbl = ew_Get("tbl")
			fld = ew_Get("fld")

			' Get blob field	
			Dim obj As cUpload = New cUpload(tbl, fld)
			obj.RestoreFromSession()
			Dim b As Object = obj.Value
			If IsDBNull(b) OrElse b Is Nothing Then Response.End()
			ft = obj.ContentType
			fn = obj.FileName

			'If ft <> "" Then
			'	Response.ContentType = ft
			'End If
			'If fn <> "" Then
			'	Response.AddHeader("Content-Disposition", "attachment; filename=""" & fn & """")
			'End If

			If resize Then obj.Resize(width, height, interpolation)
			Response.BinaryWrite(obj.Value)
			Response.End()
		End If
	End Sub
End Class
