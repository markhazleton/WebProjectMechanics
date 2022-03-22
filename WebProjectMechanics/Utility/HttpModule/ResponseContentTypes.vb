Imports System.Linq


''' <summary>
''' Represent the list of supported response content types
''' </summary>
Public Structure ResponseContentTypes
    Public Const JSON As String = "application/json"
    Public Const XML As String = "application/xml"
    Public Const HTML As String = "text/html"

    Public Const Image_JPG As String = "image/jpeg"
    Public Const Image_PNG As String = "image/x-png"
    Public Const Image_GIF As String = "image/gif"
    Public Const Image_BMP As String = "image/x-ms-bmp"

    Public Const Video_MPG As String = "video/mpeg"
    Public Const Video_MPV2 As String = "video/mpeg-2"
    Public Const Video_MOV As String = "video/quicktime"
    Public Const Video_AVI As String = "video/x-msvideo"

    Public Const Application_RTF As String = "application/rtf"
    Public Const Application_PDF As String = "application/pdf"
    Public Const Application_MSWORD As String = "application/msword"
    Public Const Application_MSEXCEL As String = "application/vnd.ms-excel"
    Public Const Application_MSPOWERPOINT As String = "application/mspowerpoint"
    Public Const Application_MSPROJECT As String = "application/vnd.ms-project"
    Public Const Application_ZIP As String = "application/zip"
End Structure
