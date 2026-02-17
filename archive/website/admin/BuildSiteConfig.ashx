<%@ WebHandler Language="VB" Class="BuildSiteConfig" %>

Imports System
Imports System.Web
Imports WebProjectMechanics
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Data


Public Class BuildSiteConfig : Implements IHttpHandler

    Dim sExt As Object = ".mdb"
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"
        Dim mySB As New StringBuilder(String.Empty)
        mySB.Append("<html><head><title>Build Site Configuration</title></head><body><h1>Database List</h1>")
        mySB.Append("<ul>")
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(context.Server.MapPath(wpm_SiteConfig.ConfigFolder))
            baseFile = baseFile.Replace(context.Server.MapPath(wpm_SiteConfig.ConfigFolder), String.Empty)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                mySB.Append(String.Format("<li>{0}", baseFile.ToLower))
                mySB.Append(String.Format("<ul>"))
                mySB.Append(GetSiteProfiles(baseFile.ToLower, context))
                mySB.Append(String.Format("</ul></li>"))
            End If
        Next
        mySB.Append("</ul></body></html>")
        context.Response.Write(mySB.ToString)
    End Sub
    
    Private Function GetSiteProfiles(ByVal DatabasePath As String, ByVal context As HttpContext) As String
        Dim myCompany As New Company
        If Not FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "sites") Then
            FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "sites")
        End If
        Dim mySB As New StringBuilder(String.Empty)
        Dim ConStr As String = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|{0};", DatabasePath)
        Dim sSQL As String = "SELECT Company.CompanyID,  Company.CompanyName,  Company.GalleryFolder,  Company.SiteURL,  Company.SiteTitle,  Company.SiteTemplate,  Company.DefaultSiteTemplate,  Company.HomePageID,  Company.DefaultArticleID,  Company.ActiveFL,  Company.UseBreadCrumbURL,  Company.SiteCategoryTypeID,  Company.DefaultPaymentTerms,  Company.DefaultInvoiceDescription,  Company.City,  Company.StateOrProvince,  Company.PostalCode,  Company.Country,  Company.FromEmail, Company.SMTP, Company.Component  FROM Company "
        For Each myRow As DataRow In GetDataTable(sSQL, "Company for Site Profile", ConStr).Rows
            myCompany.SetCompanyValue(myRow)
            If wpm_IsValidURL(myCompany.DomainName) Then
                Dim mylist As New DomainConfigurations
                mylist.Configuration.CompanyID = myCompany.CompanyID
                mylist.Configuration.SQLDBConnString = ConStr
                mylist.Configuration.DomainName = myCompany.DomainName
                mylist.Configuration.AccessDatabasePath = wpm_SiteConfig.ConfigFolder & Replace(Replace(DatabasePath.ToLower, wpm_SiteConfig.ConfigFolderPath.ToLower, String.Empty), "\", "/")
                DomainConfigurations.Save(String.Format("{1}\sites\{0}.xml", mylist.Configuration.DomainName, context.Server.MapPath(wpm_SiteConfig.ConfigFolder)), mylist)
                mySB.Append(String.Format("<li>{0} - {1} - {2} </li>", myCompany.DomainName, myCompany.CompanyID, myCompany.CompanyTitle))
            Else
                mySB.Append(String.Format("<li style='color:red;'>{0} - {1} - {2} </li>", myCompany.DomainName, myCompany.CompanyID, myCompany.CompanyTitle))
            End If
        Next
        Return mySB.ToString
    End Function
    
    Public Shared Function GetDataTable(ByVal sSQL As String, ByVal sTableName As String, ByVal ConnStr As String) As DataTable
        Using dataTable As New DataTable
            Using RecConn As New OleDbConnection() With {.ConnectionString = ConnStr}
                Try
                    RecConn.Open()
                    Using myCommand As New OleDbCommand(sSQL, RecConn)
                        Dim myDR As OleDbDataReader = myCommand.ExecuteReader
                        dataTable.Load(myDR)
                    End Using
                    RecConn.Close()
                Catch ex As Exception
                    ApplicationLogging.SQLSelectError(sSQL, String.Format("Error on UtilityDB.GetDataTable - {0} ({1})", sTableName, ex.Message))
                End Try
            End Using
            Return dataTable
        End Using
    End Function
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class