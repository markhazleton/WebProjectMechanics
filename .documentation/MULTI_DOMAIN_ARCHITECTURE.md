# Web Project Mechanics - Multi-Domain Multi-Database Architecture
## Comprehensive Technical Specification and Requirements Document

**Version:** 1.0  
**Date:** February 2026  
**Author:** Reverse Engineered Documentation  
**Purpose:** Complete technical specification for recreating the system in a modern technology stack

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [System Architecture Overview](#system-architecture-overview)
3. [Multi-Domain Support](#multi-domain-support)
4. [Multi-Database Architecture](#multi-database-architecture)
5. [Core Components](#core-components)
6. [Request Flow](#request-flow)
7. [Data Model](#data-model)
8. [Configuration Management](#configuration-management)
9. [Security Considerations](#security-considerations)
10. [Migration Guide](#migration-guide)
11. [Appendices](#appendices)

---

## 1. Executive Summary

### 1.1 System Purpose

Web Project Mechanics (WPM) is a web content management system designed to host multiple independent websites from a single codebase. Each website can have its own:
- Domain name
- MS Access database
- Content hierarchy
- Configuration settings
- Visual theme

### 1.2 Key Features

- **Multi-Domain Support**: Host 30+ domains from single application instance
- **Multi-Database**: Each domain connects to its own MS Access database
- **Single Codebase**: One application serves all sites with shared business logic
- **Dynamic Routing**: Request routing based on incoming domain name
- **Content Management**: Hierarchical page/location structure with articles
- **Caching**: Application-level caching for configuration and company data

### 1.3 Technology Stack

**Current Implementation:**
- **Framework**: ASP.NET Web Forms (VB.NET), .NET Framework 4.8
- **Database**: Microsoft Access (.mdb files) via OLE DB
- **Web Server**: IIS with custom HTTP Module
- **Configuration**: XML files for domain mapping
- **Data Access**: Custom DAL using ADO.NET (OleDbConnection)

### 1.4 Scale

- **Domains Supported**: 36 configured domains
- **Databases**: 16 unique MS Access databases
- **Many-to-One**: Multiple domains can share a single database
- **Application State**: Configuration cached at application level
- **Request Lifecycle**: Per-request domain resolution

---

## 2. System Architecture Overview

### 2.1 High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Internet Users                          │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────┐
│                  IIS Web Server                             │
│  ┌──────────────────────────────────────────────────────┐   │
│  │   ApplicationHttpModule (HTTP Pipeline)              │   │
│  │   - WWW Removal                                       │   │
│  │   - URL Normalization                                 │   │
│  └──────────────────┬───────────────────────────────────┘   │
│                     │                                         │
│                     ▼                                         │
│  ┌──────────────────────────────────────────────────────┐   │
│  │   Domain Resolution (App.wpm_DomainConfig)           │   │
│  │   - Parse SERVER_NAME                                 │   │
│  │   - Load XML Config from /App_Data/Sites/{domain}.xml│   │
│  │   - Cache in Application State                        │   │
│  └──────────────────┬───────────────────────────────────┘   │
│                     │                                         │
│                     ▼                                         │
│  ┌──────────────────────────────────────────────────────┐   │
│  │   Database Connection Setup                           │   │
│  │   - Set wpm_SQLDBConnString (OLE DB)                 │   │
│  │   - Set wpm_AccessDatabasePath                        │   │
│  └──────────────────┬───────────────────────────────────┘   │
│                     │                                         │
│                     ▼                                         │
│  ┌──────────────────────────────────────────────────────┐   │
│  │   Application Page Processing                         │   │
│  │   - ApplicationPage.Page_Init                         │   │
│  │   - UpdateDataSourceConnection()                      │   │
│  │   - Set DataSource.ConnectionString                   │   │
│  └──────────────────┬───────────────────────────────────┘   │
│                     │                                         │
│                     ▼                                         │
│  ┌──────────────────────────────────────────────────────┐   │
│  │   Content Rendering                                   │   │
│  │   - Load Company Data                                 │   │
│  │   - Load Location/Page Data                           │   │
│  │   - Render HTML                                       │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────┐
│              File System Storage                            │
│  ┌──────────────────┐  ┌──────────────────┐                │
│  │  App_Data/       │  │  App_Data/Sites/ │                │
│  │  *.mdb files     │  │  *.xml configs   │                │
│  └──────────────────┘  └──────────────────┘                │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 Directory Structure

```
WebProjectMechanics/
├── .documentation/           # Documentation files
├── .github/                  # GitHub workflows and configs
├── WebProjectMechanics/      # Core library (VB.NET Class Library)
│   ├── Company/              # Company/Domain models
│   │   ├── Company.vb
│   │   ├── DomainConfiguration.vb
│   │   └── DomainConfigurations.vb
│   ├── Location/             # Page/Content models
│   ├── Article/              # Article models
│   ├── Part/                 # Component models
│   ├── Interfaces/           # Interface definitions
│   └── Utility/              # Utility modules
│       ├── Modules/
│       │   ├── App.vb        # Core app functions (domain resolution)
│       │   ├── ApplicationDAL.vb  # Data access layer
│       │   └── UtilityDB.vb  # Database utilities
│       ├── UIBase/
│       │   ├── ApplicationPage.vb  # Base page class
│       │   └── ApplicationMasterPage.vb
│       └── HttpModule/
│           └── ApplicationHttpModule.vb  # Request pipeline
├── website/                  # Web application
│   ├── App_Data/             # Data storage
│   │   ├── Sites/            # Domain configuration XML files
│   │   │   ├── localhost.xml
│   │   │   ├── frogsfolly.com.xml
│   │   │   └── ... (36 total)
│   │   ├── *.mdb             # MS Access databases (16 files)
│   │   └── wpm_configfile.xml
│   ├── admin/                # Administrative interface
│   │   ├── BuildSiteConfig.ashx  # Config generation tool
│   │   └── SiteList.aspx     # Site management
│   ├── api/                  # API endpoints
│   ├── runtime/              # Runtime pages
│   └── Web.config            # Application configuration
└── RemoveWWW/                # HTTP Module for WWW removal
```

---

## 3. Multi-Domain Support

### 3.1 Domain Resolution Mechanism

The system uses a sophisticated domain resolution mechanism that operates during every HTTP request:

**Process Flow:**

1. **Request Arrival**: User requests `http://frogsfolly.com/page.aspx`
2. **SERVER_NAME Extraction**: System reads `HttpContext.Current.Request.ServerVariables("SERVER_NAME")`
3. **WWW Normalization**: Remove "www." prefix if present
4. **Configuration Lookup**: Look for XML file at `/App_Data/Sites/{domain}.xml`
5. **Parse Configuration**: Load domain-specific settings
6. **Cache in Application State**: Store in `HttpContext.Current.Application(domain)`
7. **Set Connection Strings**: Configure database connection for this domain

### 3.2 DomainConfiguration Class

**File**: `WebProjectMechanics/Company/DomainConfiguration.vb`

```vb
<Serializable()>
Public Class DomainConfiguration
    Public Property AccessDatabasePath As String
    Public Property CompanyID As String
    Public Property DomainName As String
    Public Property SQLDBConnString As String
End Class
```

**Properties:**
- **AccessDatabasePath**: Relative path to .mdb file (e.g., `/App_Data/FrogsFolly.mdb`)
- **CompanyID**: Unique identifier for the company/site
- **DomainName**: The domain name without www prefix
- **SQLDBConnString**: OLE DB connection string for MS Access

### 3.3 Domain Configuration XML Format

**Location**: `/App_Data/Sites/{domain}.xml`

**Example**: `localhost.xml`
```xml
<?xml version="1.0" encoding="utf-8"?>
<DomainConfigurations xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
                      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Configuration>
    <AccessDatabasePath>/App_Data/FrogsFolly-Travel.mdb</AccessDatabasePath>
    <CompanyID>1</CompanyID>
    <DomainName>travel.frogsfolly.com</DomainName>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|FrogsFolly-Travel.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

**Key Points:**
- Each domain has its own XML configuration file
- `|DataDirectory|` token resolves to `/App_Data/` at runtime
- Connection string uses ACE.OLEDB.12.0 provider (Access 2007+)
- Multiple domains can reference the same database

### 3.4 Configuration Loading Process

**File**: `WebProjectMechanics/Utility/Modules/App.vb`

**Code Flow:**
```vb
Public ReadOnly Property wpm_DomainConfig() As DomainConfiguration
    Get
        ' 1. Get config folder path (default: ~/App_Data/)
        Dim ConfigFile = String.Format("{0}sites\{1}.xml", 
            HttpContext.Current.Server.MapPath(ConfigFolder),
            Replace(HttpContext.Current.Request.ServerVariables("SERVER_NAME"), "www.", String.Empty))
        
        ' 2. Check application cache
        If HttpContext.Current.Application(wpm_HostName) Is Nothing Then
            ' 3. Load from XML file
            mySiteSettings = DomainConfigurations.Load(ConfigFile)
            
            ' 4. Set default values if not found
            If mySiteSettings?.Configuration?.CompanyID Is Nothing Then
                mySiteSettings.Configuration.CompanyID = "17"
                mySiteSettings.Configuration.SQLDBConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|projectmechanics.mdb;"
                mySiteSettings.Configuration.AccessDatabasePath = "/App_Data/projectmechanics.mdb"
            End If
            
            ' 5. Cache in application state
            HttpContext.Current.Application(wpm_HostName) = mySite
        End If
        
        Return HttpContext.Current.Application(wpm_HostName)
    End Get
End Property
```

### 3.5 Domain-to-Database Mapping

**Current Configuration (36 Domains → 16 Databases):**

| Database File | Domains Using It | Company IDs |
|---------------|-----------------|-------------|
| FrogsFolly.mdb | frogsfolly.com, family.frogsfolly.com, travel.frogsfolly.com | 1, 2, 3 |
| FrogsFollyKids.mdb | lauren.frogsfolly.com, sarah.frogsfolly.com, jordan.frogsfolly.com, etc. | 4-10 |
| ProjectMechanics.mdb | webprojectmechanics.com, localhost | 17, 18 |
| InformationCenter.mdb | dearinggroup.com, houstonlakeshore.com | 20, 21 |
| MineralCollection.mdb | jmshawminerals.com | 25 |
| wpmMOM.mdb | mechanicsofmotherhood.com | 30 |
| ... | ... | ... |

**Many-to-One Relationship**: Multiple subdomains (lauren.frogsfolly.com, sarah.frogsfolly.com) can share the same database but serve different content based on CompanyID.

### 3.6 Domain Configuration Generator

**File**: `website/admin/BuildSiteConfig.ashx`

**Purpose**: Automatically scans all .mdb files and generates domain XML configurations

**Process:**
1. Scan `/App_Data/*.mdb` files
2. Connect to each database
3. Query `Company` table for site information
4. Generate XML config for each valid domain
5. Save to `/App_Data/Sites/{domain}.xml`

**SQL Query Used:**
```sql
SELECT Company.CompanyID, Company.CompanyName, Company.GalleryFolder, 
       Company.SiteURL, Company.SiteTitle, Company.SiteTemplate, 
       Company.DefaultSiteTemplate, Company.HomePageID, Company.DefaultArticleID, 
       Company.ActiveFL, Company.UseBreadCrumbURL, Company.City, 
       Company.StateOrProvince, Company.PostalCode, Company.Country, 
       Company.FromEmail, Company.SMTP, Company.Component 
FROM Company
```

---

## 4. Multi-Database Architecture

### 4.1 Database Technology

**Microsoft Access (.mdb)**
- **Format**: Access 2007/2010 format (.mdb, not .accdb)
- **Provider**: Microsoft.ACE.OLEDB.12.0
- **Location**: `/App_Data/` directory
- **Connection**: OLE DB via ADO.NET
- **Concurrent Access**: Multiple read connections supported
- **File Size**: Typical databases 2-50 MB

### 4.2 Connection String Format

```
Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|{filename}.mdb;
```

**Components:**
- **Provider**: ACE OLE DB provider for Access 2007+
- **Data Source**: Path to .mdb file
- **|DataDirectory|**: ASP.NET substitution token → `/App_Data/`

### 4.3 Database Connection Management

**File**: `WebProjectMechanics/Utility/Modules/UtilityDB.vb`

**Key Functions:**

1. **wpm_GetDataTable**: Execute SELECT query, return DataTable
```vb
Public Function wpm_GetDataTable(ByVal sSQL As String, ByVal sTableName As String) As DataTable
    Using dataTable As New DataTable
        Using RecConn As New OleDbConnection With {.ConnectionString = wpm_SQLDBConnString}
            RecConn.Open()
            Using myCommand As New OleDbCommand(sSQL, RecConn)
                Dim myDR As OleDbDataReader = myCommand.ExecuteReader
                dataTable.Load(myDR)
            End Using
            RecConn.Close()
        End Using
        Return dataTable
    End Using
End Function
```

2. **wpm_RunInsertSQL**: Execute INSERT/UPDATE/DELETE
3. **wpm_SQLDBConnString**: Property that returns domain-specific connection string

**Connection Lifecycle:**
- Connections are opened per-query
- No connection pooling implemented
- Each request may open multiple connections
- Connections closed in `Using` blocks

### 4.4 Data Access Layer

**File**: `WebProjectMechanics/Utility/Modules/ApplicationDAL.vb`

**Key Functions:**

```vb
Public Function GetCompanyData(ByVal CompanyID As String) As DataTable
    Return wpm_GetDataTable(
        String.Format("SELECT Company.CompanyID, Company.CompanyName, ... 
                       FROM Company WHERE Company.CompanyID={0}", CompanyID), 
        "GetCompanyData")
End Function

Public Function GetLocationList(ByVal CompanyID As String) As DataTable
    ' Returns hierarchical page structure
End Function

Public Function GetArticleList(ByVal LocationID As String) As DataTable
    ' Returns articles for a location
End Function
```

### 4.5 Dynamic DataSource Configuration

**File**: `WebProjectMechanics/Utility/UIBase/ApplicationPage.vb`

During page initialization, all data source controls are updated with domain-specific connections:

```vb
Private Function UpdateDataSourceConnection() As Boolean
    ' Iterate through all controls in page
    For Each childControl As Control In contentControl.Controls
        If TypeOf childControl Is AccessDataSource Then
            ' Set AccessDataSource.DataFile
            TryCast(childControl, AccessDataSource).DataFile = wpm_AccessDatabasePath
        ElseIf TypeOf childControl Is SqlDataSource Then
            ' Set SqlDataSource.ConnectionString
            TryCast(childControl, SqlDataSource).ConnectionString = wpm_SQLDBConnString
        End If
    Next
    Return True
End Function
```

This ensures all data-bound controls use the correct database for the current domain.

---

## 5. Core Components

### 5.1 Company Entity

**Purpose**: Represents a website/domain configuration

**File**: `WebProjectMechanics/Company/Company.vb`

**Key Properties:**
```vb
Public Class Company
    Public Property CompanyID As String
    Public Property CompanyNM As String
    Public Property DomainName As String         ' Primary domain
    Public Property CompanyTitle As String        ' Site title
    Public Property CompanyKeywords As String     ' SEO keywords
    Public Property CompanyDescription As String  ' SEO description
    Public Property HomeLocationID As String      ' Home page ID
    Public Property DefaultArticleID As Integer   ' Default article
    Public Property SitePrefix As String          ' Template prefix
    Public Property DefaultSitePrefix As String   ' Fallback template
    Public Property SiteGallery As String         ' Image gallery path
    Public Property UseBreadCrumbURL As Boolean   ' URL structure mode
    Public Property FromEmail As String           ' Email sender
    Public Property SMTP As String                ' SMTP server
    Public Property Component As String           ' Custom components
    
    ' Collections
    Public ReadOnly Property Locations As LocationList
    Public ReadOnly Property Articles As ArticleList
    Public ReadOnly Property Parts As PartList
    Public ReadOnly Property Parameters As ParameterList
End Class
```

**Database Table**: `Company`

**Loading Process:**
```vb
Public Function GetCompanyFromDB(ByVal reqCompanyID As String, 
                                  ByVal GroupID As String, 
                                  ByVal OrderBy As String) As Boolean
    ' 1. Load company data
    CompanyID = reqCompanyID
    Dim bReturn = GetCompanyValues(CompanyID)
    
    ' 2. Load related data
    GetAllLocations(CompanyID, GroupID, OrderBy)
    GetAllParts(CompanyID)
    GetAllParameters(CompanyID)
    GetAllPartCategories(CompanyID)
    GetAllLocationGroups(CompanyID)
    GetAllImages(CompanyID)
    
    Return bReturn
End Function
```

### 5.2 Location Entity

**Purpose**: Represents a page or content node in the site hierarchy

**File**: `WebProjectMechanics/Location/Location.vb`

**Key Properties:**
```vb
Public Class Location
    Public Property RecordSource As String        ' Type: "Page", "Article", "Part"
    Public Property LocationID As String          ' Unique ID
    Public Property ArticleID As Integer          ' Article link
    Public Property LocationName As String        ' Display name
    Public Property LocationTitle As String       ' Page title (SEO)
    Public Property LocationDescription As String ' Meta description
    Public Property LocationKeywords As String    ' Meta keywords
    Public Property LocationBody As String        ' HTML content
    Public Property LocationSummary As String     ' Summary text
    Public Property LocationURL As String         ' URL segment
    Public Property ParentLocationID As String    ' Parent in hierarchy
    Public Property LevelNBR As Integer          ' Tree depth
    Public Property DefaultOrder As Integer       ' Sort order
    Public Property ActiveFL As Boolean          ' Published status
    Public Property IncludeInNavigation As Boolean
    Public Property BreadCrumbURL As String      ' Full path
    Public Property TransferURL As String        ' Redirect URL
    
    ' Navigation helpers
    Public Property MainMenuURL As String
    Public Property MainMenuLocationID As String
    Public Property MainMenuLocationName As String
End Class
```

**Database Table**: `Location`

**Hierarchy Structure:**
- Tree structure using ParentLocationID
- LevelNBR indicates depth (0 = root, 1 = top-level, etc.)
- DefaultOrder controls display sequence
- Supports unlimited nesting levels

### 5.3 Article Entity

**Purpose**: Content articles attached to locations

**Properties:**
```vb
Public Class Article
    Public Property ArticleID As Integer
    Public Property ArticleName As String
    Public Property ArticleTitle As String
    Public Property ArticleBody As String
    Public Property ArticleSummary As String
    Public Property ArticleDescription As String
    Public Property ArticleAuthor As String
    Public Property ArticlePageID As String       ' Parent location
    Public Property ArticleURL As String
    Public Property ArticleModDate As DateTime
    Public Property IsArticleActive As Boolean
    Public Property IsArticleDefault As Boolean
    Public Property RowsPerPage As Integer
End Class
```

### 5.4 Application Lifecycle

**HTTP Module Registration**

**File**: `Web.config`
```xml
<system.webServer>
  <modules>
    <add name="ApplicationHttpModule" 
         type="RemoveWWW.ApplicationHttpModule" 
         preCondition="" />
  </modules>
</system.webServer>
```

**File**: `WebProjectMechanics/Utility/HttpModule/ApplicationHttpModule.vb`

```vb
Public Class ApplicationHttpModule
    Implements IHttpModule
    
    Public Sub Init(application As HttpApplication) Implements IHttpModule.Init
        AddHandler application.BeginRequest, AddressOf Application_BeginRequest
    End Sub
    
    Private Sub Application_BeginRequest(source As Object, e As EventArgs)
        Dim application As HttpApplication = DirectCast(source, HttpApplication)
        Dim context As HttpContext = application.Context
        Dim url As String = context.Request.Url.ToString()
        
        ' Remove www. if present
        If url.Contains("://www.") AndAlso wpm_SiteConfig.RemoveWWW Then
            RemoveWww(context)
        End If
    End Sub
End Class
```

**Page Initialization**

**File**: `WebProjectMechanics/Utility/UIBase/ApplicationPage.vb`

```vb
Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    MyBase.OnPreInit(e)
    
    ' 1. Update all data source connections for current domain
    UpdateDataSourceConnection()
    
    ' 2. Get reference to master page
    masterPage = DirectCast(Me.Page.Master, ApplicationMasterPage)
End Sub
```

### 5.5 Caching Strategy

**Application-Level Cache:**
- Domain configurations cached in `HttpContext.Current.Application(domain)`
- Company data cached after first load
- Cache persists until application restart
- No expiration policy implemented

**Session-Level State:**
- User authentication (UserGroupID, UserName)
- Current page/article context
- Page history for navigation

---

## 6. Request Flow

### 6.1 Complete Request Lifecycle

```
1. HTTP Request Arrives
   └─> IIS receives request for frogsfolly.com/page.aspx

2. HTTP Module: ApplicationHttpModule.BeginRequest
   ├─> Extract URL
   ├─> Check for "www." prefix
   └─> Redirect if needed (301 Permanent)

3. Domain Resolution: App.wpm_DomainConfig
   ├─> Get SERVER_NAME from request
   ├─> Normalize (remove "www.")
   ├─> Check Application cache
   └─> If not cached:
       ├─> Build config file path: /App_Data/Sites/frogsfolly.com.xml
       ├─> Load and parse XML
       ├─> Extract configuration:
       │   ├─> CompanyID: "1"
       │   ├─> AccessDatabasePath: "/App_Data/FrogsFolly.mdb"
       │   └─> SQLDBConnString: "Provider=...FrogsFolly.mdb"
       └─> Store in Application(domain)

4. Page Processing: ApplicationPage.Page_Init
   ├─> UpdateDataSourceConnection()
   │   ├─> Iterate all page controls
   │   ├─> Find AccessDataSource controls
   │   ├─> Set DataFile = wpm_AccessDatabasePath
   │   ├─> Find SqlDataSource controls
   │   └─> Set ConnectionString = wpm_SQLDBConnString
   └─> Load master page reference

5. Company Data Loading: Company.GetCompanyFromDB
   ├─> Query database using wpm_SQLDBConnString
   ├─> SELECT * FROM Company WHERE CompanyID = {current domain's CompanyID}
   ├─> Load company properties
   └─> Load related data:
       ├─> Locations (pages hierarchy)
       ├─> Articles
       ├─> Parts (reusable components)
       └─> Parameters (key-value settings)

6. Content Rendering
   ├─> Resolve current LocationID (from URL or default)
   ├─> Query Location table
   ├─> Build navigation menus (hierarchical)
   ├─> Render page content (LocationBody)
   ├─> Apply template/theme (SitePrefix)
   └─> Generate HTML

7. HTTP Response
   └─> Send rendered HTML to browser
```

### 6.2 URL Routing Patterns

**Standard Page Access:**
```
http://domain.com/Default.aspx?c={LocationID}
http://domain.com/Default.aspx?c={LocationID}&a={ArticleID}
```

**BreadcrumbURL Mode** (SEO-friendly):
```
http://domain.com/category/subcategory/page.aspx
```

**URL Parameters:**
- `c`: Current page/location ID
- `a`: Article ID
- Direct .aspx file access for specific pages

### 6.3 Error Handling

**404 Errors:**
- Custom 404 handler: `/404.ashx`
- Configured in Web.config

**500 Errors:**
- Custom 500 page: `/500.aspx`

**Application Logging:**
- `ApplicationLogging.ErrorLog()`: General errors
- `ApplicationLogging.SQLSelectError()`: Database errors
- `ApplicationLogging.ConfigLog()`: Configuration errors

---

## 7. Data Model

### 7.1 Core Database Schema

#### Company Table
```sql
CREATE TABLE Company (
    CompanyID AUTOINCREMENT PRIMARY KEY,
    CompanyName TEXT(255),
    SiteURL TEXT(255),              -- Domain name
    SiteTitle TEXT(255),
    SiteTemplate TEXT(100),         -- Current template
    DefaultSiteTemplate TEXT(100),  -- Fallback template
    GalleryFolder TEXT(255),
    HomePageID INTEGER,             -- Default location
    DefaultArticleID INTEGER,
    ActiveFL YES/NO,
    UseBreadCrumbURL YES/NO,
    City TEXT(100),
    StateOrProvince TEXT(100),
    PostalCode TEXT(20),
    Country TEXT(100),
    FromEmail TEXT(255),
    SMTP TEXT(255),
    Component TEXT(100),
    SiteCategoryTypeID INTEGER,
    DefaultPaymentTerms TEXT(255),
    DefaultInvoiceDescription TEXT
)
```

#### Location Table
```sql
CREATE TABLE Location (
    LocationID AUTOINCREMENT PRIMARY KEY,
    LocationName TEXT(255),
    LocationTitle TEXT(255),
    LocationDescription TEXT,
    LocationKeywords TEXT,
    LocationBody MEMO,              -- HTML content
    LocationSummary MEMO,
    LocationURL TEXT(255),
    ParentLocationID INTEGER,       -- Self-referencing FK
    LevelNBR INTEGER,               -- Hierarchy depth
    DefaultOrder INTEGER,           -- Sort order
    RecordSource TEXT(50),          -- "Page", "Article", "Part"
    ArticleID INTEGER,
    ImageID INTEGER,
    LocationTypeCD TEXT(20),
    LocationTypeID INTEGER,
    GroupID INTEGER,
    ImageFileName TEXT(255),
    LocationFileName TEXT(255),
    LocationAuthor TEXT(255),
    IncludeInNavigation YES/NO,
    HideGlobalContent YES/NO,
    ActiveFL YES/NO,
    RowsPerPage INTEGER,
    ImagesPerRow INTEGER,
    SiteCategoryID INTEGER,
    SiteCategoryName TEXT(100),
    LocationGroupID INTEGER,
    LocationGroupNM TEXT(100),
    TransferURL TEXT(255),
    BreadCrumbURL TEXT(500),
    MainMenuURL TEXT(500),
    MainMenuLocationID INTEGER,
    MainMenuLocationName TEXT(255),
    ModifiedDT DATETIME
)
```

#### Article Table
```sql
CREATE TABLE Article (
    ArticleID AUTOINCREMENT PRIMARY KEY,
    ArticleName TEXT(255),
    ArticleTitle TEXT(255),
    ArticleBody MEMO,
    ArticleSummary MEMO,
    ArticleDescription TEXT,
    ArticleAuthor TEXT(255),
    ArticlePageID INTEGER,          -- FK to Location
    ArticleURL TEXT(255),
    ArticleModDate DATETIME,
    CompanyID INTEGER,
    ContactID INTEGER,
    IsArticleActive YES/NO,
    IsArticleDefault YES/NO,
    RowsPerPage INTEGER
)
```

#### Part Table
```sql
CREATE TABLE Part (
    PartID AUTOINCREMENT PRIMARY KEY,
    PartName TEXT(255),
    PartTitle TEXT(255),
    PartBody MEMO,
    PartDescription TEXT,
    PartCategoryID INTEGER,
    CompanyID INTEGER,
    ActiveFL YES/NO
)
```

#### Parameter Table
```sql
CREATE TABLE Parameter (
    ParameterID AUTOINCREMENT PRIMARY KEY,
    ParameterName TEXT(255),
    ParameterValue TEXT,
    ParameterDescription TEXT,
    CompanyID INTEGER
)
```

#### LocationGroup Table
```sql
CREATE TABLE LocationGroup (
    LocationGroupID AUTOINCREMENT PRIMARY KEY,
    LocationGroupName TEXT(255),
    LocationGroupDescription TEXT,
    CompanyID INTEGER,
    ActiveFL YES/NO
)
```

### 7.2 Relationships

```
Company (1) ──────── (M) Location
    │                     │
    │                     └── ParentLocationID (self-ref)
    │
    ├─── (M) Article ──── (M) Location (via ArticlePageID)
    │
    ├─── (M) Part
    │
    ├─── (M) Parameter
    │
    └─── (M) LocationGroup
```

### 7.3 Data Flow Patterns

**Hierarchical Location Loading:**
```vb
' Recursive query to build tree structure
Private Function BuildLocationTree(parentID As String) As List(Of Location)
    Dim locations = From loc In AllLocations
                    Where loc.ParentLocationID = parentID
                    OrderBy loc.DefaultOrder
    
    For Each loc In locations
        loc.Children = BuildLocationTree(loc.LocationID)
    Next
    
    Return locations.ToList()
End Function
```

---

## 8. Configuration Management

### 8.1 Web.config Settings

**Key Sections:**

```xml
<connectionStrings>
    <!-- Default connection (fallback) -->
    <add name="AccessConnectionString" 
         connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\wpm-demo.mdb;" />
</connectionStrings>

<appSettings>
    <!-- Optional: Override default config folder -->
    <add key="wpm_ConfigFolder" value="~/App_Data/" />
</appSettings>

<system.web>
    <compilation debug="true" targetFramework="4.8" />
    <httpRuntime maxRequestLength="1048576" />  <!-- 1 GB upload limit -->
</system.web>

<system.webServer>
    <modules>
        <add name="ApplicationHttpModule" type="RemoveWWW.ApplicationHttpModule" />
    </modules>
    <httpErrors errorMode="Custom">
        <error statusCode="404" path="/404.ashx" responseMode="ExecuteURL" />
        <error statusCode="500" path="/500.aspx" responseMode="ExecuteURL" />
    </httpErrors>
</system.webServer>
```

### 8.2 Site Configuration Object

**File**: `WebProjectMechanics/Company/SiteConfig.vb` (inferred)

```vb
Public Class SiteConfig
    Public Property ConfigFolder As String = "~/App_Data/"
    Public Property ConfigFolderPath As String  ' Physical path
    Public Property AdminFolder As String = "/admin/"
    Public Property RemoveWWW As Boolean = True
    Public Property DefaultDatabase As String = "projectmechanics.mdb"
End Class
```

### 8.3 Configuration Generation Tool

**Purpose**: Scan databases and auto-generate domain configs

**URL**: `/admin/BuildSiteConfig.ashx`

**Algorithm:**
```
1. Scan /App_Data/ for all .mdb files
2. For each database:
   a. Open connection
   b. Query Company table
   c. For each Company record:
      - Extract SiteURL (domain)
      - Validate domain format
      - Create DomainConfiguration object
      - Serialize to XML
      - Save to /App_Data/Sites/{domain}.xml
3. Return HTML report of created configs
```

---

## 9. Security Considerations

### 9.1 Authentication

**User Roles:**
- UserGroupID = 1: Administrator (full access)
- UserGroupID = 2: Editor (content management)
- UserGroupID = 3: User (limited access)
- UserGroupID = 4: Anonymous (public)

**Session Management:**
```vb
' Stored in session
HttpContext.Current.Session("wpm_UserGroupID")
HttpContext.Current.Session("wpm_UserID")
HttpContext.Current.Session("wpm_UserName")
HttpContext.Current.Session("wpm_UserEmail")
```

**Authorization Checks:**
```vb
Public ReadOnly Property wpm_IsAdmin As Boolean
    Get
        Return (Session("wpm_UserGroupID") = "1")
    End Get
End Property

Public Sub CheckAdmin()
    If Not wpm_IsAdmin Then
        Response.Redirect("/admin/login/login.aspx")
    End If
End Sub
```

### 9.2 Security Vulnerabilities

**Known Issues:**

1. **SQL Injection Risk**: String concatenation in queries
   ```vb
   ' VULNERABLE:
   String.Format("SELECT * FROM Company WHERE CompanyID={0}", userInput)
   
   ' SHOULD BE:
   Using cmd As New OleDbCommand("SELECT * FROM Company WHERE CompanyID=?", conn)
       cmd.Parameters.AddWithValue("@CompanyID", userInput)
   ```

2. **Path Traversal**: Direct file path construction
   ```vb
   ' VULNERABLE:
   Server.MapPath("/App_Data/" & userInput & ".mdb")
   ```

3. **XSS Risk**: Direct HTML rendering without encoding
   ```vb
   ' LocationBody rendered directly without sanitization
   Response.Write(location.LocationBody)
   ```

4. **Session Fixation**: No session regeneration on login

5. **No CSRF Protection**: Forms lack anti-forgery tokens

### 9.3 Database Security

- **File System Access**: .mdb files accessible if web server misconfigured
- **No Encryption**: Databases not encrypted at rest
- **Weak Connection Strings**: Stored in plain text
- **No Audit Logging**: Changes not tracked

---

## 10. Migration Guide

### 10.1 Target Modern Stack

**Recommended Technology Stack:**

```
Frontend:
- React/Vue.js/Angular with TypeScript
- Next.js or Nuxt.js for SSR
- Tailwind CSS for styling

Backend:
- ASP.NET Core 8.0 (C#) or Node.js (Express/Nest.js)
- RESTful API or GraphQL
- Entity Framework Core or Dapper

Database:
- PostgreSQL or SQL Server
- Entity Framework Core migrations
- Consider multi-tenancy architecture

Infrastructure:
- Docker containers
- Kubernetes for orchestration
- Azure App Service or AWS ECS

Caching:
- Redis for distributed caching
- Content Delivery Network (CDN)

Authentication:
- Identity Server / Auth0 / Okta
- JWT tokens
- OAuth 2.0 / OpenID Connect
```

### 10.2 Migration Strategy

#### Phase 1: Database Migration

**Convert MS Access to SQL Server/PostgreSQL:**

```sql
-- Create tenant/company table
CREATE TABLE companies (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    domain VARCHAR(255) UNIQUE NOT NULL,
    config JSONB,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Create locations table (was Location)
CREATE TABLE locations (
    id SERIAL PRIMARY KEY,
    company_id INT REFERENCES companies(id),
    parent_id INT REFERENCES locations(id),
    name VARCHAR(255) NOT NULL,
    title VARCHAR(255),
    slug VARCHAR(255),
    body TEXT,
    metadata JSONB,
    level INT DEFAULT 0,
    sort_order INT DEFAULT 0,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    UNIQUE(company_id, slug)
);

-- Create articles table
CREATE TABLE articles (
    id SERIAL PRIMARY KEY,
    location_id INT REFERENCES locations(id),
    title VARCHAR(255) NOT NULL,
    slug VARCHAR(255),
    body TEXT,
    author VARCHAR(255),
    is_active BOOLEAN DEFAULT true,
    published_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX idx_locations_company ON locations(company_id);
CREATE INDEX idx_locations_parent ON locations(parent_id);
CREATE INDEX idx_articles_location ON articles(location_id);
CREATE INDEX idx_companies_domain ON companies(domain);
```

**Data Migration Script:**
```csharp
// Pseudocode for migration
foreach (var mdbFile in Directory.GetFiles("/App_Data", "*.mdb"))
{
    using var accessConn = new OleDbConnection($"Provider=...;Data Source={mdbFile}");
    using var pgConn = new NpgsqlConnection(postgresConnectionString);
    
    // Migrate Company table
    var companies = accessConn.Query("SELECT * FROM Company");
    foreach (var company in companies)
    {
        pgConn.Execute(@"
            INSERT INTO companies (name, domain, config) 
            VALUES (@name, @domain, @config)",
            new { 
                name = company.CompanyName,
                domain = company.SiteURL,
                config = JsonSerializer.Serialize(company)
            });
    }
    
    // Migrate Locations
    var locations = accessConn.Query("SELECT * FROM Location");
    foreach (var loc in locations)
    {
        pgConn.Execute(@"
            INSERT INTO locations (company_id, parent_id, name, title, body, ...) 
            VALUES (...)", loc);
    }
    
    // Migrate Articles, Parts, etc.
}
```

#### Phase 2: Multi-Tenancy Architecture

**Approach 1: Separate Database per Tenant**
```csharp
public class TenantService
{
    private readonly Dictionary<string, string> _tenantConnections;
    
    public string GetConnectionString(string domain)
    {
        // Resolve domain to connection string
        return _tenantConnections[NormalizeDomain(domain)];
    }
}

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;
    
    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_connectionString);
    }
}
```

**Approach 2: Shared Database with Tenant ID** (Recommended)
```csharp
public class ApplicationDbContext : DbContext
{
    private readonly ITenantService _tenantService;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Global query filter for multi-tenancy
        modelBuilder.Entity<Location>()
            .HasQueryFilter(l => l.CompanyId == _tenantService.CurrentTenantId);
        
        modelBuilder.Entity<Article>()
            .HasQueryFilter(a => a.Location.CompanyId == _tenantService.CurrentTenantId);
    }
}
```

#### Phase 3: Domain Routing Middleware

**ASP.NET Core Middleware:**

```csharp
public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITenantStore _tenantStore;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var host = context.Request.Host.Host;
        var domain = NormalizeDomain(host); // Remove www
        
        // Resolve tenant from domain
        var tenant = await _tenantStore.GetByDomainAsync(domain);
        
        if (tenant == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Site not found");
            return;
        }
        
        // Store tenant in context
        context.Items["Tenant"] = tenant;
        
        await _next(context);
    }
    
    private string NormalizeDomain(string host)
    {
        return host.Replace("www.", "").ToLowerInvariant();
    }
}

// Startup.cs
app.UseMiddleware<TenantResolutionMiddleware>();
```

#### Phase 4: API Design

**RESTful API Structure:**

```
GET    /api/companies              - List all companies (admin)
GET    /api/companies/{id}         - Get company details
POST   /api/companies              - Create company
PUT    /api/companies/{id}         - Update company
DELETE /api/companies/{id}         - Delete company

GET    /api/locations              - Get location hierarchy (filtered by tenant)
GET    /api/locations/{id}         - Get location details
POST   /api/locations              - Create location
PUT    /api/locations/{id}         - Update location
DELETE /api/locations/{id}         - Delete location

GET    /api/articles               - List articles (filtered by tenant)
GET    /api/articles/{id}          - Get article
POST   /api/articles               - Create article
PUT    /api/articles/{id}          - Update article
DELETE /api/articles/{id}          - Delete article
```

**Example Controller:**

```csharp
[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantService _tenantService;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
    {
        // Query filter automatically applies tenant filter
        var locations = await _context.Locations
            .Include(l => l.Children)
            .Where(l => l.ParentId == null) // Root level only
            .OrderBy(l => l.SortOrder)
            .ToListAsync();
        
        return Ok(locations);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDto>> GetLocation(int id)
    {
        var location = await _context.Locations
            .Include(l => l.Articles)
            .FirstOrDefaultAsync(l => l.Id == id);
        
        if (location == null)
            return NotFound();
        
        return Ok(location);
    }
}
```

#### Phase 5: Frontend Architecture

**React with Next.js:**

```typescript
// lib/api/locations.ts
export async function getLocations(): Promise<Location[]> {
  const response = await fetch('/api/locations');
  return response.json();
}

export async function getLocation(id: number): Promise<Location> {
  const response = await fetch(`/api/locations/${id}`);
  return response.json();
}

// pages/[...slug].tsx - Dynamic routing
export async function getServerSideProps(context) {
  const { slug } = context.params;
  const host = context.req.headers.host;
  
  // Resolve location by slug
  const location = await getLocationBySlug(slug.join('/'));
  
  return {
    props: {
      location,
      domain: host
    }
  };
}

export default function LocationPage({ location }) {
  return (
    <Layout>
      <h1>{location.title}</h1>
      <div dangerouslySetInnerHTML={{ __html: location.body }} />
    </Layout>
  );
}
```

#### Phase 6: Configuration Management

**JSON Configuration:**

```json
// appsettings.json
{
  "MultiTenancy": {
    "Strategy": "SharedDatabase",
    "Tenants": [
      {
        "Id": 1,
        "Name": "Frog's Folly",
        "Domains": ["frogsfolly.com", "www.frogsfolly.com"],
        "Theme": "default",
        "Features": {
          "Blog": true,
          "Gallery": true
        }
      },
      {
        "Id": 2,
        "Name": "Project Mechanics",
        "Domains": ["webprojectmechanics.com"],
        "Theme": "technical",
        "Features": {
          "Blog": true,
          "Gallery": false
        }
      }
    ]
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=wpm;Username=wpm;Password=..."
  }
}
```

**Redis Caching:**

```csharp
public class CachedTenantStore : ITenantStore
{
    private readonly IDistributedCache _cache;
    private readonly IDbTenantStore _dbStore;
    
    public async Task<Tenant> GetByDomainAsync(string domain)
    {
        var cacheKey = $"tenant:{domain}";
        var cached = await _cache.GetStringAsync(cacheKey);
        
        if (cached != null)
            return JsonSerializer.Deserialize<Tenant>(cached);
        
        var tenant = await _dbStore.GetByDomainAsync(domain);
        
        if (tenant != null)
        {
            await _cache.SetStringAsync(cacheKey, 
                JsonSerializer.Serialize(tenant),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
        }
        
        return tenant;
    }
}
```

### 10.3 Migration Checklist

**Pre-Migration:**
- [ ] Inventory all .mdb databases
- [ ] Document all domain→database mappings
- [ ] Export all Company records
- [ ] Export all Location hierarchies
- [ ] Export all Articles
- [ ] Backup all databases
- [ ] Document custom SQL queries
- [ ] List all HTTP handlers (.ashx files)
- [ ] Catalog all master pages and templates

**Database Migration:**
- [ ] Design new schema (normalized, indexed)
- [ ] Create migration scripts
- [ ] Test migration with sample data
- [ ] Validate data integrity
- [ ] Create seed data scripts
- [ ] Setup database backup strategy

**Application Development:**
- [ ] Setup project structure
- [ ] Implement tenant resolution
- [ ] Create data access layer
- [ ] Build RESTful API
- [ ] Implement authentication/authorization
- [ ] Create admin interface
- [ ] Build frontend components
- [ ] Implement caching strategy
- [ ] Add logging and monitoring

**Testing:**
- [ ] Unit tests for all services
- [ ] Integration tests for API
- [ ] End-to-end tests for critical flows
- [ ] Load testing for performance
- [ ] Security testing (OWASP Top 10)
- [ ] Cross-browser testing

**Deployment:**
- [ ] Setup CI/CD pipeline
- [ ] Configure staging environment
- [ ] Setup monitoring (Application Insights, etc.)
- [ ] Configure CDN
- [ ] DNS configuration for all domains
- [ ] SSL certificates for all domains
- [ ] Database connection pooling
- [ ] Error logging (Sentry, etc.)

**Post-Migration:**
- [ ] Monitor performance metrics
- [ ] User acceptance testing
- [ ] Document new architecture
- [ ] Train administrators
- [ ] Decommission old system

---

## 11. Appendices

### Appendix A: Domain Configuration Examples

**Example 1: FrogsFolly.com**
```xml
<?xml version="1.0" encoding="utf-8"?>
<DomainConfigurations>
  <Configuration>
    <AccessDatabasePath>/App_Data/wpmFrogsFolly.mdb</AccessDatabasePath>
    <CompanyID>1</CompanyID>
    <DomainName>frogsfolly.com</DomainName>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|wpmFrogsFolly.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

**Example 2: JM Shaw Minerals**
```xml
<?xml version="1.0" encoding="utf-8"?>
<DomainConfigurations>
  <Configuration>
    <AccessDatabasePath>/App_Data/MineralCollection.mdb</AccessDatabasePath>
    <CompanyID>25</CompanyID>
    <DomainName>jmshawminerals.com</DomainName>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|MineralCollection.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

### Appendix B: Database Inventory

**Complete List (16 Databases, 36 Domains):**

| Database File | Size | Domains Served | Company IDs |
|---------------|------|----------------|-------------|
| wpmFrogsFolly.mdb | 8 MB | frogsfolly.com | 1 |
| FrogsFollyKids.mdb | 4 MB | lauren.frogsfolly.com, sarah.frogsfolly.com, jordan.frogsfolly.com, berit.frogsfolly.com, mateus.frogsfolly.com, marlis.frogsfolly.com, ian.frogsfolly.com | 4-10 |
| FrogsFolly-Family.mdb | 3 MB | family.frogsfolly.com, studentcouncil.frogsfolly.com | 2-3 |
| FrogsFolly-Travel.mdb | 2 MB | travel.frogsfolly.com, localhost | 1 |
| ProjectMechanics.mdb | 12 MB | webprojectmechanics.com | 17 |
| InformationCenter.mdb | 6 MB | dearinggroup.com, houstonlakeshore.com | 20-21 |
| MARKInformationCenter.mdb | 5 MB | lakebuchananliving.com, inkslakeliving.com, burnetcountyliving.com, kellertexasliving.com, GraysonGeorgiaLiving.com | 22-26 |
| MineralCollection.mdb | 15 MB | jmshawminerals.com | 25 |
| wpmMOM.mdb | 4 MB | mechanicsofmotherhood.com | 30 |
| DramaEducator.mdb | 3 MB | dramaeducator.com | 31 |
| osmcinc.mdb | 5 MB | osmcinc.projectmechanics.com | 40 |
| wpmLiving.mdb | 4 MB | LakeClaiborneHouse.com, TexEcon.com, MacCloskeyAndMyers.com, holdcompany.com | 35-38 |
| wpm-demo.mdb | 2 MB | (default fallback) | 17 |
| FrogsFollyBase.mdb | 1 MB | (template/backup) | - |
| FrogsFolly.mdb | 10 MB | (legacy version) | - |
| 1-MineralCollection.mdb | 14 MB | (backup) | - |

### Appendix C: Code Samples

**Company Loading:**
```vb
Public Function GetCompanyFromDB(ByVal reqCompanyID As String, 
                                  ByVal GroupID As String, 
                                  ByVal OrderBy As String) As Boolean
    CompanyID = reqCompanyID
    Dim bReturn As Boolean = GetCompanyValues(CompanyID)
    
    If Trim(OrderBy) = String.Empty Then
        OrderBy = "ORDER"
    End If
    
    If bReturn Then
        GetAllLocations(CompanyID, GroupID, OrderBy)
        GetAllParts(CompanyID)
        GetAllParameters(CompanyID)
        GetAllPartCategories(CompanyID)
        GetAllLocationGroups(CompanyID)
        GetAllImages(CompanyID)
        GetAllLocationAliases(CompanyID)
    End If
    
    Return bReturn
End Function
```

**Location Hierarchy Query:**
```sql
SELECT Location.RecordSource, Location.LocationID, Location.ArticleID, 
       Location.LocationTypeCD, Location.LocationName, Location.LocationTitle,
       Location.LocationDescription, Location.LocationKeywords, 
       Location.LocationBody, Location.LocationSummary, Location.ParentLocationID,
       Location.LevelNBR, Location.DefaultOrder, Location.LocationURL,
       Location.SiteCategoryID, Location.SiteCategoryName, Location.LocationGroupID,
       Location.LocationGroupNM, Location.ActiveFL, Location.IncludeInNavigation,
       Location.TransferURL, Location.BreadCrumbURL, Location.MainMenuURL,
       Location.MainMenuLocationID, Location.MainMenuLocationName, Location.ModifiedDT,
       Location.RowsPerPage, Location.LocationAuthor
FROM Location
WHERE Location.GroupID = {GroupID}
  AND Location.ActiveFL = True
ORDER BY {OrderBy}
```

### Appendix D: Key Files Reference

**Core Application Files:**
- `/WebProjectMechanics/Company/DomainConfiguration.vb` - Domain config model
- `/WebProjectMechanics/Company/DomainConfigurations.vb` - XML serialization
- `/WebProjectMechanics/Company/Company.vb` - Company entity
- `/WebProjectMechanics/Location/Location.vb` - Page/content entity
- `/WebProjectMechanics/Utility/Modules/App.vb` - Domain resolution
- `/WebProjectMechanics/Utility/Modules/ApplicationDAL.vb` - Data access
- `/WebProjectMechanics/Utility/Modules/UtilityDB.vb` - Database utilities
- `/WebProjectMechanics/Utility/UIBase/ApplicationPage.vb` - Base page class
- `/WebProjectMechanics/Utility/HttpModule/ApplicationHttpModule.vb` - HTTP pipeline
- `/website/admin/BuildSiteConfig.ashx` - Config generator
- `/website/Web.config` - Application configuration

**Configuration Files:**
- `/website/App_Data/Sites/*.xml` - Domain configurations (36 files)
- `/website/App_Data/*.mdb` - MS Access databases (16 files)

### Appendix E: Glossary

- **Company**: A website/domain entity in the system
- **Location**: A page or content node in the site hierarchy
- **Article**: Content piece attached to a location
- **Part**: Reusable content component
- **Parameter**: Key-value configuration setting
- **DomainConfiguration**: XML file mapping domain to database
- **wpm_**: Prefix for Web Project Mechanics functions/properties
- **|DataDirectory|**: ASP.NET token resolving to /App_Data/
- **CompanyID**: Unique identifier for each site
- **LocationID**: Unique identifier for each page/content node
- **ParentLocationID**: Reference to parent in hierarchy
- **BreadcrumbURL**: SEO-friendly URL path
- **RecordSource**: Type indicator ("Page", "Article", "Part")

---

## Summary

This document provides a complete specification of the Web Project Mechanics multi-domain, multi-database architecture. The system demonstrates an elegant solution for hosting multiple independent websites from a single codebase, using:

1. **Dynamic Domain Resolution**: Runtime detection and configuration loading
2. **Per-Request Database Routing**: Each request connects to the appropriate database
3. **XML-Based Configuration**: Simple, file-based domain mapping
4. **MS Access Databases**: Lightweight, file-based storage
5. **Application-Level Caching**: Performance optimization through in-memory caching
6. **Hierarchical Content Model**: Flexible page/article organization
7. **Template System**: Theme support via SitePrefix

The migration guide provides a clear path to modernize this architecture using:
- ASP.NET Core or Node.js
- PostgreSQL or SQL Server
- Multi-tenancy patterns
- RESTful APIs
- React/Next.js frontend
- Redis caching
- Docker containers

This documentation should enable a development team to:
1. Understand the current system completely
2. Maintain and extend the existing implementation
3. Plan and execute a migration to a modern stack
4. Preserve all functional requirements in the new system
