Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

'
' ASP.NET Maker 7 - Project Configuration
'
Public Partial Class AspNetMaker7_WPMGen
	Inherits wpmPage

	' Database connection string
	Public Shared EW_DB_CONNECTION_STRING As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & HttpContext.Current.Server.MapPath("~/access_db/ProjectMechanics.mdb") & ";"

	' Database SQL parameter symbol
	Public Const EW_DB_SQLPARAM_SYMBOL As String = "?"

	' Database type	
	Public Const EW_IS_MSACCESS As Boolean = True ' Access	

	Public Const EW_IS_MSSQL As Boolean = False ' MS SQL	

	Public Const EW_IS_MYSQL As Boolean = False ' MySQL	

	Public Const EW_IS_ORACLE As Boolean = False ' Oracle

	' Debug flag
	Public Const EW_DEBUG_ENABLED As Boolean = False ' Changed to True to debug

	' Response.Buffer		
	Public Const EW_RESPONSE_BUFFER As Boolean = True 

	' Project name
	Public Const EW_PROJECT_NAME As String = "WPMGen"	

	' Remove XSS
	Public Const EW_REMOVE_XSS As Boolean = False ' Remove XSS

	' Note: Remove accepted elements in the following array at your own risk. 
	' Public Shared EW_REMOVE_XSS_KEYWORDS() As String = New String(){"javascript", "vbscript", "expression", "<applet", "<meta", "<xml", "<blink", "<link", "<style", "<script", "<embed", "<object", "<iframe", "<frame", "<frameset", "<ilayer", "<layer", "<bgsound", "<title", "<base", "onabort", "onactivate", "onafterprint", "onafterupdate", "onbeforeactivate", "onbeforecopy", "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus", "onbeforepaste", "onbeforeprint", "onbeforeunload", "onbeforeupdate", "onblur", "onbounce", "oncellchange", "onchange", "onclick", "oncontextmenu", "oncontrolselect", "oncopy", "oncut", "ondataavailable", "ondatasetchanged", "ondatasetcomplete", "ondblclick", "ondeactivate", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover", "ondragstart", "ondrop", "onerror", "onerrorupdate", "onfilterchange", "onfinish", "onfocus", "onfocusin", "onfocusout", "onhelp", "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete", "onload", "onlosecapture", "onmousedown", "onmouseenter", "onmouseleave", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheel", "onmove", "onmoveend", "onmovestart", "onpaste", "onpropertychange", "onreadystatechange", "onreset", "onresize", "onresizeend", "onresizestart", "onrowenter", "onrowexit", "onrowsdelete", "onrowsinserted", "onscroll", "onselect", "onselectionchange", "onselectstart", "onstart", "onstop", "onsubmit", "onunload"}	
	Public Shared EW_REMOVE_XSS_KEYWORDS() As String = New String(){}	

	' Session names
	Public Const EW_SESSION_STATUS As String = EW_PROJECT_NAME & "_Status" ' Login status	

	Public Const EW_SESSION_USER_NAME As String = EW_SESSION_STATUS & "_UserName" ' User name	

	Public Const EW_SESSION_USER_ID As String = EW_SESSION_STATUS & "_UserID" ' User ID	

	Public Const EW_SESSION_USER_LEVEL_ID As String = EW_SESSION_STATUS & "_UserLevel" ' User level ID	

	Public Const EW_SESSION_USER_LEVEL As String = EW_SESSION_STATUS & "_UserLevelValue" ' User level	

	Public Const EW_SESSION_PARENT_USER_ID As String = EW_SESSION_STATUS & "_ParentUserID" ' Parent user ID	

	Public Const EW_SESSION_SYS_ADMIN As String = EW_PROJECT_NAME & "_SysAdmin" ' System admin	

	Public Const EW_SESSION_AR_USER_LEVEL As String = EW_PROJECT_NAME & "_arUserLevel" ' User level ArrayList	

	Public Const EW_SESSION_AR_USER_LEVEL_PRIV As String = EW_PROJECT_NAME & "_arUserLevelPriv" ' User level privilege ArrayList

	Public Const EW_SESSION_SECURITY As String = EW_PROJECT_NAME & "_Security" ' Security srray	

	Public Const EW_SESSION_MESSAGE As String = EW_PROJECT_NAME & "_Message" ' System message	

	Public Const EW_SESSION_INLINE_MODE As String = EW_PROJECT_NAME & "_InlineMode" ' Inline mode

	' Paging
	Public Const EW_PAGER_RANGE As Integer = 10

	Public Const EW_GRIDADD_ROWS As Integer = 10	

	' Delimiters/Separators
	Public Const EW_RECORD_DELIMITER As String = vbCr

	Public Const EW_FIELD_DELIMITER As String = "|"

	Public Const EW_COMPOSITE_KEY_SEPARATOR As String = "," ' Composite key separator	

	Public Const EW_EMAIL_KEYWORD_SEPARATOR As String = "|" ' Email keyword separator

	' Date format
	Public Const EW_DATE_SEPARATOR As String = "/"	

	Public Const EW_DEFAULT_DATE_FORMAT As Short = 6

	' Highlight	
	Public Const EW_HIGHLIGHT_COMPARE As Boolean = True ' Case-insensitive

	' Data type (DO NOT CHANGE!)		
	Public Const EW_DATATYPE_NUMBER As Short = 1	

	Public Const EW_DATATYPE_DATE As Short = 2			

	Public Const EW_DATATYPE_STRING As Short = 3	

	Public Const EW_DATATYPE_BOOLEAN As Short = 4	

	Public Const EW_DATATYPE_GUID As Short = 5	

	Public Const EW_DATATYPE_OTHER As Short = 6

	Public Const EW_DATATYPE_TIME As Short = 7

	Public Const EW_DATATYPE_BLOB As Short = 8

	Public Const EW_DATATYPE_MEMO As Short = 9			

	' Row type
	Public Const EW_ROWTYPE_VIEW As Short = 1 ' Row type view	

	Public Const EW_ROWTYPE_ADD As Short = 2 ' Row type add	

	Public Const EW_ROWTYPE_EDIT As Short = 3 ' Row type edit	

	Public Const EW_ROWTYPE_SEARCH As Short = 4 ' Row type search	

	Public Const EW_ROWTYPE_MASTER As Short = 5 ' Row type master record

	' Table specific
	Public Const EW_TABLE_REC_PER_PAGE As String = "RecPerPage" ' Records per page	

	Public Const EW_TABLE_START_REC As String = "start" ' Start record	

	Public Const EW_TABLE_PAGE_NO As String = "pageno" ' number	

	Public Const EW_TABLE_BASIC_SEARCH As String = "psearch" ' Basic search keyword	

	Public Const EW_TABLE_BASIC_SEARCH_TYPE As String = "psearchtype" ' Basic search type	

	Public Const EW_TABLE_ADVANCED_SEARCH As String = "advsrch" ' Advanced search	

	Public Const EW_TABLE_SEARCH_WHERE As String = "searchwhere" ' Search where clause	

	Public Const EW_TABLE_WHERE As String = "where" ' Table where	

	Public Const EW_TABLE_ORDER_BY As String = "orderby" ' Table order by	

	Public Const EW_TABLE_SORT As String = "sort" ' Table sort	

	Public Const EW_TABLE_KEY As String = "key" ' Table key	

	Public Const EW_TABLE_SHOW_MASTER As String = "showmaster" ' Table show master	

	Public Const EW_TABLE_MASTER_TABLE As String = "MasterTable" ' Master table	

	Public Const EW_TABLE_MASTER_FILTER As String = "MasterFilter" ' Master filter	

	Public Const EW_TABLE_DETAIL_FILTER As String = "DetailFilter" ' Detail filter	

	Public Const EW_TABLE_RETURN_URL As String = "return" ' Return URL

	' Audit trail
	Public Const EW_AUDIT_TRAIL_PATH As String = "~/access_db/log/" ' Audit trail path	

	' Security
	Public Const EW_ADMIN_USER_NAME As String = "" ' Administrator user name	

	Public Const EW_ADMIN_PASSWORD As String = "" ' Administrator password	

	Public Const EW_MD5_PASSWORD As Boolean = False ' MD5 password	

	Public Const EW_CASE_SENSITIVE_PASSWORD As Boolean = False ' Case sensitive password

	Public Shared EW_REPORT_TABLE_PREFIX As String = "||ASPNETReportMaker||" ' Reserved for report maker

	' User level
	Public Const EW_USER_LEVEL_COMPAT As Boolean = True ' Use old user level values

	Public Const EW_ALLOW_ADD As Short = 1 ' Add	

	Public Const EW_ALLOW_DELETE As Short = 2 ' Delete	

	Public Const EW_ALLOW_EDIT As Short = 4 ' Edit	

	Public Const EW_ALLOW_LIST As Short = 8 ' List	

	Public Const EW_ALLOW_VIEW As Integer = 8 ' View (for EW_USER_LEVEL_COMPAT = True)

	Public Const EW_ALLOW_SEARCH As Integer = 8 ' Search (for EW_USER_LEVEL_COMPAT = True)

	'Public Const EW_ALLOW_VIEW As Integer = 32 ' View (for EW_USER_LEVEL_COMPAT = False)
	'Public Const EW_ALLOW_SEARCH As Integer = 64 ' Search (for EW_USER_LEVEL_COMPAT = False)
	Public Const EW_ALLOW_REPORT As Short = 8 ' Report	

	Public Const EW_ALLOW_ADMIN As Short = 16 ' Admin	

	' Hierarchical User ID
	Public Const EW_USER_ID_IS_HIERARCHICAL As Boolean = True ' Change to False to show one level

	' Use subquery for master/detail user id checking
	Public Const EW_USE_SUBQUERY_FOR_MASTER_USER_ID As Boolean = False	

	' Email
	Public Const EW_SMTP_SERVER As String = "smtpout.secureserver.net" ' SMTP server	

	Public Const EW_SMTP_SERVER_PORT As Integer = 25 ' SMTP server port	

	Public Const EW_SMTP_SERVER_USERNAME As String = "mark.hazleton@projectmechanics.com" ' SMTP server user name	

	Public Const EW_SMTP_SERVER_PASSWORD As String = "goforit" ' SMTP server password	

	Public Const EW_SENDER_EMAIL As String = "mark.hazleton@projectmechanics.com" ' Sender email	

	Public Const EW_RECIPIENT_EMAIL As String = "mark.hazleton@projectmechanics.com " ' Recipient email

	' File upload
	Public Const EW_UPLOAD_DEST_PATH As String = "~/App_Upload/" ' Upload destination path	

	Public Const EW_UPLOAD_ALLOWED_FILE_EXT As String = "gif,jpg,jpeg,bmp,png,doc,xls,pdf,zip" ' Allowed file extensions	

	Public Const EW_MAX_FILE_SIZE As Integer = 2000000 ' Max file size	

	Public Const EW_THUMBNAIL_FILE_PREFIX As String = "tn_" ' Thumbnail file prefix	

	Public Const EW_THUMBNAIL_FILE_SUFFIX As String = "" ' Thumbnail file suffix	

	Public Const EW_THUMBNAIL_DEFAULT_WIDTH As Short = 0 ' Thumbnail default width	

	Public Const EW_THUMBNAIL_DEFAULT_HEIGHT As Short = 0 ' Thumbnail default height	

	Public Const EW_THUMBNAIL_DEFAULT_INTERPOLATION As Short = 1 ' Thumbnail default interpolation	

	' Export
	Public Const EW_EXPORT_ALL As Boolean = True ' Export all records	

	'Public Const EW_EXPORT_ALL As Boolean = False ' Export 1 page only
	Public Const EW_EXPORT_ORIGINAL_VALUE As Boolean = False ' Do not export original value

	'Public Const EW_EXPORT_ORIGINAL_VALUE As Boolean = True ' Export original value
	' Use token in URL (reserved only)	
	Public Const EW_USE_TOKEN_IN_URL As Boolean = False ' Do not use token in URL	

	' Public Const EW_USE_TOKEN_IN_URL As Boolean = True ' Use token in URL
	' Search multi value option
	' 1 - no multi value
	' 2 - AND all multi values
	' 3 - OR all multi values	
	Public Const EW_SEARCH_MULTI_VALUE_OPTION As Short = 3

	' Validate option
	Public Const EW_CLIENT_VALIDATE As Boolean = True	

	Public Const EW_SERVER_VALIDATE As Boolean = True

	' Random key
	Public Const EW_RANDOM_KEY As String = "3Vb5HjqUiLUz6CPT"

	' Checkbox/RadioButton template/table
	Public Const EW_ITEM_TEMPLATE_CLASSNAME As String = "ewTemplate"	

	Public Const EW_ITEM_TABLE_CLASSNAME As String = "ewItemTable"

	' StyleSheet
	Public Const EW_PROJECT_CSSFILE As String = "WPMGen.css"	

	Public Const EW_ROWTYPE_PREVIEW As Integer = 6 ' Preview record

	' Menu
	Public Const EW_MENUBAR_VERTICAL_CLASSNAME As String = "MenuBarVertical"

	Public Const EW_MENUBAR_SUBMENU_CLASSNAME As String = "MenuBarItemSubmenu"

	Public Const EW_MENUBAR_RIGHTHOVER_IMAGE As String = "images/SpryMenuBarRightHover.gif"
End Class
