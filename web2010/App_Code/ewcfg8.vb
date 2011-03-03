Imports System.Data
Imports System.Data.Common
Imports WebProjectMechanics
Imports System.Data.OleDb

'
' ASP.NET Maker 8 - Project Configuration
'
Public Partial Class AspNetMaker8_wpmWebsite
	Inherits wpmPage

    ' Database connection string
    Public Shared ReadOnly Property EW_DB_CONNECTION_STRING
        Get
            Return wpmApp.ConnStr
        End Get
    End Property

	Public Const EW_DB_QUOTE_START As String = "["

	Public Const EW_DB_QUOTE_END As String = "]"

	' Database SQL parameter symbol
	Public Const EW_DB_SQLPARAM_SYMBOL As String = "?"

	' Database type	
	Public Const EW_IS_MSACCESS As Boolean = True ' Access

	Public Const EW_IS_MSSQL As Boolean = False ' MS SQL

	Public Const EW_IS_MYSQL As Boolean = False ' MySQL

	Public Const EW_IS_POSTGRESQL As Boolean = False ' PostgreSQL

	Public Const EW_IS_ORACLE As Boolean = False ' Oracle

	' Debug flag
	Public Const EW_DEBUG_ENABLED As Boolean = True ' True to debug / False to skip

	' Response.Buffer		
	Public Const EW_RESPONSE_BUFFER As Boolean = True

	' Use DOM XML for language object
	Public Const EW_USE_DOM_XML As Boolean = False 

	' Project name
	Public Const EW_PROJECT_NAME As String = "wpmWebsite"

	' Remove XSS
	Public Const EW_REMOVE_XSS As Boolean = False ' True to Remove XSS / False to skip

	' Note: Remove accepted elements in the following array at your own risk. 
	Public Shared EW_REMOVE_XSS_KEYWORDS() As String = New String(){"javascript", "vbscript", "expression", "<applet", "<meta", "<xml", "<blink", "<link", "<style", "<script", "<embed", "<object", "<iframe", "<frame", "<frameset", "<ilayer", "<layer", "<bgsound", "<title", "<base", "onabort", "onactivate", "onafterprint", "onafterupdate", "onbeforeactivate", "onbeforecopy", "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus", "onbeforepaste", "onbeforeprint", "onbeforeunload", "onbeforeupdate", "onblur", "onbounce", "oncellchange", "onchange", "onclick", "oncontextmenu", "oncontrolselect", "oncopy", "oncut", "ondataavailable", "ondatasetchanged", "ondatasetcomplete", "ondblclick", "ondeactivate", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover", "ondragstart", "ondrop", "onerror", "onerrorupdate", "onfilterchange", "onfinish", "onfocus", "onfocusin", "onfocusout", "onhelp", "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete", "onload", "onlosecapture", "onmousedown", "onmouseenter", "onmouseleave", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheel", "onmove", "onmoveend", "onmovestart", "onpaste", "onpropertychange", "onreadystatechange", "onreset", "onresize", "onresizeend", "onresizestart", "onrowenter", "onrowexit", "onrowsdelete", "onrowsinserted", "onscroll", "onselect", "onselectionchange", "onselectstart", "onstart", "onstop", "onsubmit", "onunload"}	

	' Session names
	Public Const EW_SESSION_STATUS As String = EW_PROJECT_NAME & "_Status" ' Login status

	Public Const EW_SESSION_USER_NAME As String = EW_SESSION_STATUS & "_UserName" ' User name

	Public Const EW_SESSION_USER_ID As String = EW_SESSION_STATUS & "_UserID" ' User ID

	Public Const EW_SESSION_USER_PROFILE As String = EW_SESSION_STATUS & "_UserProfile" ' User Profile

	Public Const EW_SESSION_USER_PROFILE_USER_NAME As String = EW_SESSION_USER_PROFILE & "_UserName"

	Public Const EW_SESSION_USER_PROFILE_PASSWORD As String = EW_SESSION_USER_PROFILE & "_Password"

	Public Const EW_SESSION_USER_PROFILE_LOGIN_TYPE As String = EW_SESSION_USER_PROFILE & "_LoginType"

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

	Public Const EW_EMAIL_KEYWORD_SEPARATOR As String = "" ' Email keyword separator 

	Public Const EW_EMAIL_CHARSET As String = "" ' Email charset 

	' Date format
	Public Const EW_DATE_SEPARATOR As String = "/"	

	Public Const EW_DEFAULT_DATE_FORMAT As Short = 0

	' Highlight	
	Public Const EW_HIGHLIGHT_COMPARE As Boolean = True ' Case-insensitive

	' Language settings
	Public Const EW_LANGUAGE_FOLDER As String = "lang/"

	Public Shared EW_LANGUAGE_FILE()() As String = { _ 
New String() {"en", "", "english.xml"} _ 
}

	Public Const EW_LANGUAGE_DEFAULT_ID As String = "en"

	Public Const EW_SESSION_LANGUAGE_FILE_CACHE As String = EW_PROJECT_NAME & "_LanguageFile_CSx2o4AJVOsmPRpW" ' Language File Cache

	Public Const EW_SESSION_LANGUAGE_CACHE As String = EW_PROJECT_NAME & "_Language_CSx2o4AJVOsmPRpW" ' Language Cache

	Public Const EW_SESSION_LANGUAGE_ID As String = EW_PROJECT_NAME & "_LanguageId" ' Language ID

	' Css file name
	Public Const EW_PROJECT_STYLESHEET_FILENAME As String = "wpmwebsite.css"

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

	Public Const EW_ROWTYPE_AGGREGATEINIT As Short = 6 ' Row type aggregate init 

	Public Const EW_ROWTYPE_AGGREGATE As Short = 7 ' Row type aggregate	

	' Table specific
	Public Const EW_TABLE_REC_PER_PAGE As String = "RecPerPage" ' Records per page

	Public Const EW_TABLE_START_REC As String = "start" ' Start record

	Public Const EW_TABLE_PAGE_NO As String = "pageno" ' number

	Public Const EW_TABLE_BASIC_SEARCH As String = "psearch" ' Basic search keyword

	Public Const EW_TABLE_BASIC_SEARCH_TYPE As String = "psearchtype" ' Basic search type

	Public Const EW_TABLE_ADVANCED_SEARCH As String = "advsrch" ' Advanced search

	Public Const EW_TABLE_SEARCH_WHERE As String = "searchwhere" ' Search where clause

	Public Const EW_TABLE_WHERE As String = "where" ' Table where

	Public Const EW_TABLE_WHERE_LIST As String = "where_list" ' Table where (list page) 

	Public Const EW_TABLE_ORDER_BY As String = "orderby" ' Table order by

	Public Const EW_TABLE_ORDER_BY_LIST As String = "orderby_list" ' Table order by (list page) 

	Public Const EW_TABLE_SORT As String = "sort" ' Table sort

	Public Const EW_TABLE_KEY As String = "key" ' Table key

	Public Const EW_TABLE_SHOW_MASTER As String = "showmaster" ' Table show master

	Public Const EW_TABLE_MASTER_TABLE As String = "MasterTable" ' Master table

	Public Const EW_TABLE_MASTER_FILTER As String = "MasterFilter" ' Master filter

	Public Const EW_TABLE_DETAIL_FILTER As String = "DetailFilter" ' Detail filter

	Public Const EW_TABLE_RETURN_URL As String = "return" ' Return URL

	Public Const EW_TABLE_EXPORT_RETURN_URL As String = "exportreturn" ' Export return url 

	' Audit Trail
	Public Const EW_AUDIT_TRAIL_TO_DATABASE As Boolean = False ' Write audit trail to DB

	Public Const EW_AUDIT_TRAIL_TABLE_NAME As String = "" ' Audit trail table name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_DATETIME As String = "" ' Audit trail DateTime field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_SCRIPT As String = "" ' Audit trail Script field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_USER As String = "" ' Audit trail User field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_ACTION As String = "" ' Audit trail Action field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_TABLE As String = "" ' Audit trail Table field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_FIELD As String = "" ' Audit trail Field field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_KEYVALUE As String = "" ' Audit trail Key Value field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_OLDVALUE As String = "" ' Audit trail Old Value field name

	Public Const EW_AUDIT_TRAIL_FIELD_NAME_NEWVALUE As String = "" ' Audit trail New Value field name

	' Audit trail
	Public Const EW_AUDIT_TRAIL_PATH As String = "~/AuditLog/" ' Audit trail path	

	' Security
	Public Const EW_ADMIN_USER_NAME As String = "wpmadmin" ' Administrator user name	

	Public Const EW_ADMIN_PASSWORD As String = "goforit" ' Administrator password	

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
	Public Const EW_USER_ID_IS_HIERARCHICAL As Boolean = True ' True to show all level / False to show 1 level

	' Use subquery for master/detail user id checking
	Public Const EW_USE_SUBQUERY_FOR_MASTER_USER_ID As Boolean = False ' True to use subquery / False to skip

	' User table filters
	Public Const EW_USER_NAME_FILTER As String = "([Bottom] = '%u')"

	Public Const EW_USER_ID_FILTER As String = ""

	Public Const EW_USER_EMAIL_FILTER As String = ""

	Public Const EW_USER_ACTIVATE_FILTER As String = ""

	Public Const EW_USER_PROFILE_FIELD_NAME As String = ""

	' User Profile Constants
	Public Const EW_USER_PROFILE_KEY_SEPARATOR As String = "="

	Public Const EW_USER_PROFILE_FIELD_SEPARATOR As String = ","

	Public Const EW_USER_PROFILE_SESSION_ID As String = "SessionID"

	Public Const EW_USER_PROFILE_LAST_ACCESSED_DATE_TIME As String = "LastAccessedDateTime"

	Public Const EW_USER_PROFILE_SESSION_TIMEOUT As Integer = 20

	Public Const EW_USER_PROFILE_LOGIN_RETRY_COUNT As String = "LoginRetryCount"

	Public Const EW_USER_PROFILE_LAST_BAD_LOGIN_DATE_TIME As String = "LastBadLoginDateTime"

	Public Const EW_USER_PROFILE_MAX_RETRY As Integer = 3

	Public Const EW_USER_PROFILE_RETRY_LOCKOUT As Integer = 20

	Public Const EW_USER_PROFILE_LAST_PASSWORD_CHANGED_DATE As String = "LastPasswordChangedDate"

	Public Const EW_USER_PROFILE_PASSWORD_EXPIRE As Integer = 90

	' Email
	Public Const EW_SMTP_SERVER As String = "localhost" ' SMTP server	

	Public Const EW_SMTP_SERVER_PORT As Integer = 25 ' SMTP server port	

	Public Const EW_SMTP_SERVER_USERNAME As String = "" ' SMTP server user name	

	Public Const EW_SMTP_SERVER_PASSWORD As String = "" ' SMTP server password	

	Public Const EW_SENDER_EMAIL As String = "" ' Sender email	

	Public Const EW_RECIPIENT_EMAIL As String = "" ' Recipient email

	Public Const EW_MAX_EMAIL_RECIPIENT As Integer = 3

	Public Const EW_MAX_EMAIL_SENT_COUNT As Integer = 3

	Public Const EW_EXPORT_EMAIL_COUNTER As String = EW_SESSION_STATUS & "_EmailCounter"

	' File upload
	Public Const EW_UPLOAD_DEST_PATH As String = "~/App_Upload/" ' Upload destination path	

	Public Const EW_UPLOAD_ALLOWED_FILE_EXT As String = "gif,jpg,jpeg,bmp,png,doc,xls,pdf,zip" ' Allowed file extensions	

	Public Const EW_MAX_FILE_SIZE As Integer = 2000000 ' Max file size	

	Public Const EW_THUMBNAIL_DEFAULT_WIDTH As Short = 0 ' Thumbnail default width

	Public Const EW_THUMBNAIL_DEFAULT_HEIGHT As Short = 0 ' Thumbnail default height

	Public Const EW_THUMBNAIL_DEFAULT_INTERPOLATION As Short = 1 ' Thumbnail default interpolation

	' Export
	Public Const EW_EXPORT_ORIGINAL_VALUE As Boolean = False ' True to export original value

	Public Const EW_EXPORT_FIELD_CAPTION As Boolean = False ' True to export field caption

	Public Const EW_EXPORT_CSS_STYLES As Boolean = True ' True to export css styles

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

	' Blob field byte count for Hash value calculation
	Public Const EW_BLOB_FIELD_BYTE_COUNT As Integer = 200

	' Cookie expiry time
	Public Shared EW_COOKIE_EXPIRY_TIME As DateTime = DateTime.Today.AddDays(365)

	' Random key
	Public Const EW_RANDOM_KEY As String = "0VJ4C2C2bvX4xXr1"

	' Checkbox/RadioButton template/table
	Public Const EW_ITEM_TEMPLATE_CLASSNAME As String = "ewTemplate"	

	Public Const EW_ITEM_TABLE_CLASSNAME As String = "ewItemTable"

	' StyleSheet
	Public Const EW_PROJECT_CSSFILE As String = "wpmwebsite.css"	

	Public Const EW_ROWTYPE_PREVIEW As Integer = 11 ' Preview record

	Public Const EW_MENUBAR_CLASSNAME As String = "MenuBarHorizontal"

	Public Const EW_MENUBAR_SUBMENU_CLASSNAME As String = "MenuBarItemSubmenu"
End Class
