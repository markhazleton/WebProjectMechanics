# Web Project Mechanics - Visual Architecture Diagrams

This document contains detailed visual representations of the WebProjectMechanics architecture in ASCII and Mermaid diagram formats.

---

## 1. System Context Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                         INTERNET                                 │
│                                                                  │
│  Multiple Users → Multiple Domains:                             │
│  • frogsfolly.com                                                │
│  • jmshawminerals.com                                           │
│  • webprojectmechanics.com                                      │
│  • ... (36 total domains)                                       │
└──────────────────┬───────────────────────────────────────────────┘
                   │
                   │ HTTP/HTTPS Requests
                   ▼
┌──────────────────────────────────────────────────────────────────┐
│              SINGLE WEB PROJECT MECHANICS SERVER                 │
│                   (IIS + ASP.NET Web Forms)                      │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │         HTTP MODULE (Request Pipeline)                      │ │
│  │  • WWW removal                                              │ │
│  │  • URL normalization                                        │ │
│  └────────────────────┬───────────────────────────────────────┘ │
│                       │                                           │
│  ┌────────────────────▼───────────────────────────────────────┐ │
│  │      DOMAIN RESOLUTION ENGINE                               │ │
│  │  • Parse incoming domain                                    │ │
│  │  • Load XML configuration                                   │ │
│  │  • Map to database                                          │ │
│  │  • Cache configuration                                      │ │
│  └────────────────────┬───────────────────────────────────────┘ │
│                       │                                           │
│  ┌────────────────────▼───────────────────────────────────────┐ │
│  │      SHARED APPLICATION CODE                                │ │
│  │  • Company/Site logic                                       │ │
│  │  • Content rendering                                        │ │
│  │  • Navigation generation                                    │ │
│  │  • Template processing                                      │ │
│  └────────────────────┬───────────────────────────────────────┘ │
└────────────────────────┼──────────────────────────────────────────┘
                         │
         ┌───────────────┴───────────────┐
         │                               │
         ▼                               ▼
┌─────────────────┐           ┌──────────────────────┐
│  XML Configs    │           │  MS Access Databases │
│  (36 files)     │           │  (16 .mdb files)     │
│                 │           │                      │
│  domain1.xml ──┼──────────>│  database1.mdb       │
│  domain2.xml ──┼──────────>│  database1.mdb       │
│  domain3.xml ──┼──────────>│  database2.mdb       │
│  ...            │           │  ...                 │
└─────────────────┘           └──────────────────────┘
   /App_Data/Sites/             /App_Data/
```

---

## 2. Request Flow Sequence Diagram

```
User Browser    IIS/HTTP Module    Domain Resolver    Database Layer    Response
     |                |                    |                  |             |
     |   GET /page    |                    |                  |             |
     |───────────────>|                    |                  |             |
     |                |                    |                  |             |
     |                | Parse domain       |                  |             |
     |                | (SERVER_NAME)      |                  |             |
     |                |                    |                  |             |
     |                | Remove "www."      |                  |             |
     |                |─────────────────┐  |                  |             |
     |                |                 │  |                  |             |
     |                |<────────────────┘  |                  |             |
     |                |                    |                  |             |
     |                |  Resolve domain    |                  |             |
     |                |───────────────────>|                  |             |
     |                |                    |                  |             |
     |                |                    | Check cache      |             |
     |                |                    |─────────┐        |             |
     |                |                    |         │        |             |
     |                |                    |<────────┘        |             |
     |                |                    |                  |             |
     |                |                    | Load XML config  |             |
     |                |                    |─────────┐        |             |
     |                |                    |         │        |             |
     |                |                    |<────────┘        |             |
     |                |                    |                  |             |
     |                |   DomainConfig     |                  |             |
     |                |<───────────────────|                  |             |
     |                |  (DB connection    |                  |             |
     |                |   string)          |                  |             |
     |                |                    |                  |             |
     |                | Set connection     |                  |             |
     |                |────────────────────┼─────────────────>|             |
     |                |                    |                  |             |
     |                |                    | Query Company    |             |
     |                |────────────────────┼─────────────────>|             |
     |                |                    |                  |             |
     |                |                    | Company data     |             |
     |                |<───────────────────┼──────────────────|             |
     |                |                    |                  |             |
     |                |                    | Query Locations  |             |
     |                |────────────────────┼─────────────────>|             |
     |                |                    |                  |             |
     |                |                    | Location data    |             |
     |                |<───────────────────┼──────────────────|             |
     |                |                    |                  |             |
     |                | Render HTML        |                  |             |
     |                |─────────┐          |                  |             |
     |                |         │          |                  |             |
     |                |<────────┘          |                  |             |
     |                |                    |                  |             |
     |                | Apply template     |                  |             |
     |                |─────────┐          |                  |             |
     |                |         │          |                  |             |
     |                |<────────┘          |                  |             |
     |                |                    |                  |             |
     |                |                    |                  | HTML page   |
     |                |────────────────────┼──────────────────┼────────────>|
     |                |                    |                  |             |
     |   HTML page    |                    |                  |             |
     |<───────────────|                    |                  |             |
     |                |                    |                  |             |
```

---

## 3. Domain to Database Mapping Diagram

```
┌────────────────────────────────────────────────────────────────────┐
│                     DOMAIN → DATABASE MAPPING                      │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  ┌─────────────────────┐           ┌───────────────────────────┐  │
│  │  frogsfolly.com     │──────────>│  wpmFrogsFolly.mdb        │  │
│  │  CompanyID: 1       │           │  8 MB                     │  │
│  └─────────────────────┘           │  - Company (1 record)     │  │
│                                     │  - Locations (150)        │  │
│  ┌─────────────────────┐           │  - Articles (50)          │  │
│  │ family.frogsfolly   │──────────>└───────────────────────────┘  │
│  │ .com                │                                           │
│  │ CompanyID: 2        │           ┌───────────────────────────┐  │
│  └─────────────────────┘           │  FrogsFolly-Family.mdb    │  │
│                          ┌────────>│  3 MB                     │  │
│  ┌─────────────────────┐ │         │  - Company (2 records)    │  │
│  │ studentcouncil.     │ │         │  - Locations (75)         │  │
│  │ frogsfolly.com      │─┘         │  - Articles (25)          │  │
│  │ CompanyID: 3        │           └───────────────────────────┘  │
│  └─────────────────────┘                                           │
│                                                                    │
│  ┌─────────────────────┐           ┌───────────────────────────┐  │
│  │ lauren.frogsfolly   │           │  FrogsFollyKids.mdb       │  │
│  │ .com                │──┐        │  4 MB                     │  │
│  │ CompanyID: 4        │  │        │  - Company (7 records)    │  │
│  └─────────────────────┘  │        │  - Locations (200)        │  │
│                            │        │  - Articles (80)          │  │
│  ┌─────────────────────┐  │        └───────────────────────────┘  │
│  │ sarah.frogsfolly    │  │                                        │
│  │ .com                │──┤                                        │
│  │ CompanyID: 5        │  │                                        │
│  └─────────────────────┘  │                                        │
│                            │                                        │
│  ┌─────────────────────┐  │                                        │
│  │ jordan.frogsfolly   │  │                                        │
│  │ .com                │──┤                                        │
│  │ CompanyID: 6        │  │                                        │
│  └─────────────────────┘  │                                        │
│                            │                                        │
│  ┌─────────────────────┐  │                                        │
│  │ ... (4 more)        │──┘                                        │
│  │ CompanyID: 7-10     │                                           │
│  └─────────────────────┘                                           │
│                                                                    │
│  ┌─────────────────────┐           ┌───────────────────────────┐  │
│  │ jmshawminerals.com  │──────────>│  MineralCollection.mdb    │  │
│  │ CompanyID: 25       │           │  15 MB                    │  │
│  └─────────────────────┘           │  - Company (1 record)     │  │
│                                     │  - Locations (500+)       │  │
│                                     │  - Articles (200+)        │  │
│                                     │  - Parts (100+)           │  │
│                                     └───────────────────────────┘  │
│                                                                    │
│  ┌─────────────────────┐           ┌───────────────────────────┐  │
│  │ webprojectmechanics │──────────>│  ProjectMechanics.mdb     │  │
│  │ .com                │           │  12 MB                    │  │
│  │ CompanyID: 17       │           │  - Company (1 record)     │  │
│  └─────────────────────┘           │  - Locations (300+)       │  │
│                                     │  - Articles (150+)        │  │
│                                     └───────────────────────────┘  │
│                                                                    │
│  ... (30 more domains mapped to remaining 11 databases)           │
│                                                                    │
└────────────────────────────────────────────────────────────────────┘

LEGEND:
──────>  One domain to one database (1:1)
  ──┐
    ├──>  Multiple domains to one database (N:1)
  ──┘
```

---

## 4. Configuration Loading Flow

```
Request Arrives: http://frogsfolly.com/page.aspx
│
├─> Step 1: Extract Domain
│   ├─> HttpContext.Request.ServerVariables("SERVER_NAME")
│   ├─> Result: "frogsfolly.com" or "www.frogsfolly.com"
│   └─> Normalize: Remove "www." → "frogsfolly.com"
│
├─> Step 2: Check Application Cache
│   ├─> Key: Application("frogsfolly.com")
│   ├─> Cache Hit?
│   │   ├─> YES: Return cached DomainConfiguration ✓
│   │   └─> NO: Continue to Step 3
│   │
│   └─> Cache Miss → Load from file
│
├─> Step 3: Build Configuration File Path
│   ├─> ConfigFolder: "~/App_Data/"
│   ├─> Server.MapPath("~/App_Data/")
│   ├─> Result: "C:\inetpub\wwwroot\App_Data\"
│   ├─> Append: "sites\frogsfolly.com.xml"
│   └─> Full Path: "C:\inetpub\wwwroot\App_Data\sites\frogsfolly.com.xml"
│
├─> Step 4: Load and Parse XML
│   ├─> File Exists?
│   │   ├─> YES: Read XML file
│   │   └─> NO: Use default configuration
│   │
│   ├─> Deserialize XML to DomainConfigurations object
│   └─> Extract Configuration properties:
│       ├─> CompanyID: "1"
│       ├─> DomainName: "frogsfolly.com"
│       ├─> AccessDatabasePath: "/App_Data/wpmFrogsFolly.mdb"
│       └─> SQLDBConnString: "Provider=Microsoft.ACE.OLEDB.12.0;..."
│
├─> Step 5: Create DomainConfiguration Object
│   ├─> Set CompanyID
│   ├─> Set SQLDBConnString
│   ├─> Set AccessDatabasePath
│   └─> Create new DomainConfiguration instance
│
├─> Step 6: Cache Configuration
│   ├─> Application("frogsfolly.com") = DomainConfiguration
│   ├─> Cache Duration: Until application restart
│   └─> Thread-safe: Application state is locked during write
│
└─> Step 7: Return Configuration
    ├─> Properties available globally:
    │   ├─> wpm_SQLDBConnString
    │   ├─> wpm_AccessDatabasePath
    │   └─> wpm_DomainConfig
    │
    └─> Used by:
        ├─> Data access layer
        ├─> Page initialization
        └─> Data source controls
```

---

## 5. Database Entity Relationship Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                    MS ACCESS DATABASE SCHEMA                    │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────┐
│      Company         │ (One per domain/site)
├──────────────────────┤
│ PK CompanyID         │
│    CompanyName       │
│    SiteURL           │◄───┐ (Domain name)
│    SiteTitle         │    │
│    SiteTemplate      │    │
│    HomePageID        │────┼────┐
│    DefaultArticleID  │────┼───┐│
│    ActiveFL          │    │   ││
│    UseBreadCrumbURL  │    │   ││
│    FromEmail         │    │   ││
│    SMTP              │    │   ││
│    City, State, Zip  │    │   ││
└──────────────────────┘    │   ││
           │                │   ││
           │ 1              │   ││
           │                │   ││
           │ *              │   ││
           ▼                │   ││
┌──────────────────────┐    │   ││
│      Location        │◄───┘   ││ (Pages/Content hierarchy)
├──────────────────────┤        ││
│ PK LocationID        │        ││
│ FK ParentLocationID  │─┐      ││ (Self-referencing)
│ FK CompanyID         │ │      ││ (Implicit via context)
│    RecordSource      │ │      ││ ("Page", "Article", "Part")
│    LocationName      │ │      ││
│    LocationTitle     │ │      ││
│    LocationBody      │ │      ││ (HTML content)
│    LocationURL       │ │      ││ (URL slug)
│    ArticleID        │◄┼──────┘│
│    LevelNBR          │ │       │
│    DefaultOrder      │ │       │
│    ActiveFL          │ │       │
│    BreadCrumbURL     │ │       │
│    ModifiedDT        │ │       │
└──────────────────────┘ │       │
           ▲             │       │
           └─────────────┘       │
                                 │
┌──────────────────────┐         │
│      Article         │◄────────┘ (Content articles)
├──────────────────────┤
│ PK ArticleID         │
│ FK ArticlePageID     │──────────┐ (→ Location)
│ FK CompanyID         │          │ (Implicit)
│    ArticleName       │          │
│    ArticleTitle      │          │
│    ArticleBody       │          │ (HTML content)
│    ArticleAuthor     │          │
│    ArticleURL        │          │
│    IsArticleActive   │          │
│    IsArticleDefault  │          │
│    ArticleModDate    │          │
└──────────────────────┘          │
                                  │
┌──────────────────────┐          │
│      Part            │          │ (Reusable components)
├──────────────────────┤          │
│ PK PartID            │          │
│ FK PartCategoryID    │          │
│ FK CompanyID         │          │
│    PartName          │          │
│    PartTitle         │          │
│    PartBody          │          │ (HTML content)
│    ActiveFL          │          │
└──────────────────────┘          │
           │                      │
           │ *                    │
           │                      │
           │ 1                    │
           ▼                      │
┌──────────────────────┐          │
│   PartCategory       │          │
├──────────────────────┤          │
│ PK PartCategoryID    │          │
│ FK CompanyID         │          │
│    CategoryName      │          │
│    CategoryDesc      │          │
└──────────────────────┘          │
                                  │
┌──────────────────────┐          │
│   LocationGroup      │          │ (Category grouping)
├──────────────────────┤          │
│ PK LocationGroupID   │          │
│ FK CompanyID         │          │
│    GroupName         │          │
│    GroupDescription  │          │
│    ActiveFL          │          │
└──────────────────────┘          │
                                  │
┌──────────────────────┐          │
│    Parameter         │          │ (Key-value config)
├──────────────────────┤          │
│ PK ParameterID       │          │
│ FK CompanyID         │          │
│    ParameterName     │          │
│    ParameterValue    │          │
│    ParameterDesc     │          │
└──────────────────────┘          │
                                  │
┌──────────────────────┐          │
│   CompanyImage       │          │ (Image gallery)
├──────────────────────┤          │
│ PK ImageID           │          │
│ FK CompanyID         │          │
│    ImageFileName     │          │
│    ImageTitle        │          │
│    ImageDescription  │          │
│    ImagePath         │          │
└──────────────────────┘          │
                                  │
┌──────────────────────┐          │
│  LocationAlias       │          │ (URL redirects)
├──────────────────────┤          │
│ PK AliasID           │          │
│ FK LocationID        │──────────┘
│ FK CompanyID         │
│    AliasURL          │
│    ActiveFL          │
└──────────────────────┘

RELATIONSHIPS:
─────> One-to-Many (1:M)
═════> Many-to-Many (M:M) (via junction table - not shown)
- - -> Optional/Nullable foreign key
```

---

## 6. Application Class Hierarchy

```
System.Web.UI.Page (ASP.NET Base)
        ▲
        │
        │ Inherits
        │
┌───────┴──────────────────────────────────────────────┐
│   ApplicationPage (Base class for all pages)         │
├──────────────────────────────────────────────────────┤
│  Properties:                                         │
│    + masterPage: ApplicationMasterPage               │
│    + curCompany: ActiveCompany                       │
│  Methods:                                            │
│    + Page_Init()                                     │
│    + UpdateDataSourceConnection()                    │
│    + CheckAdmin()                                    │
│    + GetPageHistory()                                │
└──────────────────────────────────────────────────────┘
        ▲
        │ Inherits
        ├─────────────────┬─────────────────┬──────────
        │                 │                 │
        │                 │                 │
┌───────┴─────┐  ┌────────┴─────┐  ┌───────┴──────┐
│  Default    │  │  Article     │  │  Admin       │
│  .aspx.vb   │  │  .aspx.vb    │  │  pages       │
└─────────────┘  └──────────────┘  └──────────────┘


System.Web.UI.MasterPage (ASP.NET Base)
        ▲
        │
        │ Inherits
        │
┌───────┴──────────────────────────────────────────────┐
│   ApplicationMasterPage (Base master page)           │
├──────────────────────────────────────────────────────┤
│  Properties:                                         │
│    + myCompany: ActiveCompany                        │
│  Methods:                                            │
│    + Page_Load()                                     │
│    + BuildNavigation()                               │
│    + RenderMenu()                                    │
└──────────────────────────────────────────────────────┘
        ▲
        │ Inherits
        ├─────────────────┬─────────────────
        │                 │
┌───────┴─────┐  ┌────────┴─────┐
│  Site       │  │  Admin       │
│  .master    │  │  .master     │
└─────────────┘  └──────────────┘


┌──────────────────────────────────────────────────────┐
│   Company (Domain entity)                            │
├──────────────────────────────────────────────────────┤
│  Properties:                                         │
│    + CompanyID: String                               │
│    + DomainName: String                              │
│    + CompanyTitle: String                            │
│    + Locations: LocationList                         │
│    + Articles: ArticleList                           │
│    + Parts: PartList                                 │
│  Methods:                                            │
│    + GetCompanyFromDB()                              │
│    + GetAllLocations()                               │
│    + BuildNavigation()                               │
└──────────────────────────────────────────────────────┘
                    │
                    │ Contains
                    ▼
        ┌───────────────────────┐
        │   LocationList        │
        │   (Collection)        │
        └───────────────────────┘
                    │
                    │ Contains
                    ▼
        ┌───────────────────────┐
        │   Location            │
        ├───────────────────────┤
        │ + LocationID          │
        │ + LocationName        │
        │ + LocationBody        │
        │ + ParentLocationID    │
        │ + LevelNBR            │
        │ + Children: List<Loc> │
        └───────────────────────┘
```

---

## 7. Deployment Topology

```
┌────────────────────────────────────────────────────────────────┐
│                        PRODUCTION SERVER                       │
│                       (Windows Server 2019)                     │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌──────────────────────────────────────────────────────────┐ │
│  │                 Internet Information Services (IIS)       │ │
│  │                         (Port 80, 443)                    │ │
│  └──────────────────────┬───────────────────────────────────┘ │
│                         │                                      │
│  ┌──────────────────────▼───────────────────────────────────┐ │
│  │              Application Pool (.NET 4.8)                  │ │
│  │              Identity: ApplicationPoolIdentity            │ │
│  │              Pipeline Mode: Integrated                    │ │
│  └──────────────────────┬───────────────────────────────────┘ │
│                         │                                      │
│  ┌──────────────────────▼───────────────────────────────────┐ │
│  │         Web Application (WebProjectMechanics)            │ │
│  │                                                           │ │
│  │  Virtual Path: /                                         │ │
│  │  Physical Path: C:\inetpub\wwwroot\                      │ │
│  │                                                           │ │
│  │  ┌─────────────────────────────────────────────────────┐ │ │
│  │  │  /bin/                                               │ │ │
│  │  │    WebProjectMechanics.dll                          │ │ │
│  │  │    RemoveWWW.dll                                     │ │ │
│  │  │    Dependencies...                                   │ │ │
│  │  └─────────────────────────────────────────────────────┘ │ │
│  │                                                           │ │
│  │  ┌─────────────────────────────────────────────────────┐ │ │
│  │  │  /App_Data/                                          │ │ │
│  │  │    ├─ Sites/                                         │ │ │
│  │  │    │    ├─ domain1.xml (100 KB)                     │ │ │
│  │  │    │    ├─ domain2.xml (100 KB)                     │ │ │
│  │  │    │    └─ ... (36 files total, ~3.6 MB)           │ │ │
│  │  │    │                                                 │ │ │
│  │  │    ├─ database1.mdb (8 MB)                          │ │ │
│  │  │    ├─ database2.mdb (4 MB)                          │ │ │
│  │  │    ├─ database3.mdb (15 MB)                         │ │ │
│  │  │    └─ ... (16 files total, ~120 MB)                │ │ │
│  │  └─────────────────────────────────────────────────────┘ │ │
│  └───────────────────────────────────────────────────────────┘ │
│                                                                │
│  ┌──────────────────────────────────────────────────────────┐ │
│  │           DNS BINDINGS (All point to same IP)            │ │
│  ├──────────────────────────────────────────────────────────┤ │
│  │  • frogsfolly.com                    → 192.168.1.100     │ │
│  │  • www.frogsfolly.com                → 192.168.1.100     │ │
│  │  • family.frogsfolly.com             → 192.168.1.100     │ │
│  │  • jmshawminerals.com                → 192.168.1.100     │ │
│  │  • webprojectmechanics.com           → 192.168.1.100     │ │
│  │  • ... (36 total)                    → 192.168.1.100     │ │
│  └──────────────────────────────────────────────────────────┘ │
│                                                                │
│  ┌──────────────────────────────────────────────────────────┐ │
│  │           IIS SITE BINDINGS                               │ │
│  ├──────────────────────────────────────────────────────────┤ │
│  │  HTTP  (Port 80)                                          │ │
│  │    • *:80 (All domains)                                   │ │
│  │                                                            │ │
│  │  HTTPS (Port 443)                                         │ │
│  │    • *:443 (All domains)                                  │ │
│  │    • SSL Certificate: Wildcard or SAN cert               │ │
│  └──────────────────────────────────────────────────────────┘ │
│                                                                │
└────────────────────────────────────────────────────────────────┘

EXTERNAL RESOURCES:
┌────────────────────┐
│   DNS Servers      │
│   (Hosted zones)   │
├────────────────────┤
│ A Records:         │
│  domain1.com → IP  │
│  domain2.com → IP  │
│  ...               │
└────────────────────┘

BACKUP STRATEGY:
┌────────────────────┐
│  Backup Location   │
├────────────────────┤
│ /App_Data/*.mdb    │ → Daily backup
│ /App_Data/Sites/   │ → Daily backup
│ /bin/              │ → Version control
│ Web.config         │ → Version control
└────────────────────┘
```

---

## 8. Memory and Caching Architecture

```
┌──────────────────────────────────────────────────────────────┐
│                  ASP.NET APPLICATION                          │
│                 (Single Application Pool)                      │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│            APPLICATION STATE (Global Cache)                   │
│                (Thread-safe, Application lifetime)            │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  Application["wpm_SiteConfig"] = SiteConfig                   │
│    ├─ ConfigFolder: "~/App_Data/"                            │
│    ├─ AdminFolder: "/admin/"                                 │
│    └─ RemoveWWW: true                                        │
│                                                               │
│  Application["frogsfolly.com"] = DomainConfiguration          │
│    ├─ CompanyID: "1"                                         │
│    ├─ SQLDBConnString: "Provider=...;Data Source=..."       │
│    └─ AccessDatabasePath: "/App_Data/wpmFrogsFolly.mdb"     │
│                                                               │
│  Application["jmshawminerals.com"] = DomainConfiguration      │
│    ├─ CompanyID: "25"                                        │
│    ├─ SQLDBConnString: "Provider=...;Data Source=..."       │
│    └─ AccessDatabasePath: "/App_Data/MineralCollection.mdb" │
│                                                               │
│  Application["domain3.com"] = DomainConfiguration             │
│  Application["domain4.com"] = DomainConfiguration             │
│  ... (up to 36 cached configurations)                        │
│                                                               │
│  CACHE INVALIDATION:                                          │
│    • Application restart: ALL configs cleared                │
│    • Web.config change: ALL configs cleared                  │
│    • Manual: Not supported (requires app pool recycle)       │
│                                                               │
│  ESTIMATED MEMORY:                                            │
│    • SiteConfig: ~1 KB                                       │
│    • DomainConfiguration: ~2 KB each × 36 = ~72 KB          │
│    • Total: ~73 KB (negligible)                             │
│                                                               │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│               SESSION STATE (Per-user cache)                  │
│                   (20 minute timeout)                          │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  Session["wpm_UserGroupID"] = "1" or "2" or "3" or "4"       │
│  Session["wpm_UserID"] = "12345"                             │
│  Session["wpm_UserName"] = "john.doe"                        │
│  Session["wpm_UserEmail"] = "john@example.com"               │
│  Session["wpm_CurrentPageID"] = "42"                         │
│  Session["wpm_CurrentArticleID"] = "15"                      │
│  Session["wpm_PageHistory"] = LocationHistoryList            │
│    └─ List of recently visited pages (breadcrumb trail)      │
│                                                               │
│  ESTIMATED MEMORY PER USER:                                   │
│    • User info: ~1 KB                                        │
│    • Page history: ~5 KB (10 pages)                          │
│    • Total per user: ~6 KB                                   │
│                                                               │
│  FOR 100 CONCURRENT USERS:                                    │
│    • Total session memory: ~600 KB                           │
│                                                               │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│                  DATA CACHING STRATEGY                        │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  CURRENTLY:                                                   │
│    ❌ No company data caching                                │
│    ❌ No location data caching                               │
│    ❌ No article caching                                     │
│    ❌ Every request = Database query                         │
│                                                               │
│  RECOMMENDATION FOR MODERN STACK:                             │
│    ✅ Redis distributed cache                                │
│    ✅ Company data: 1 hour TTL                               │
│    ✅ Location hierarchy: 30 min TTL                         │
│    ✅ Articles: 15 min TTL                                   │
│    ✅ Cache invalidation on update                           │
│                                                               │
└──────────────────────────────────────────────────────────────┘

MEMORY FLOW:
┌─────────────┐
│   Request   │
└──────┬──────┘
       │
       ▼
┌──────────────────┐    Cache Hit?
│ Check Application│────────► YES ──┐
│ Cache for Domain │                │
└──────────────────┘                │
       │                            │
       │ Cache Miss                 │
       ▼                            │
┌──────────────────┐                │
│  Load XML File   │                │
└──────┬───────────┘                │
       │                            │
       ▼                            │
┌──────────────────┐                │
│ Parse & Store in │                │
│ Application Cache│                │
└──────┬───────────┘                │
       │                            │
       └────────────────────────────┘
       │
       ▼
┌──────────────────┐
│ Use Configuration│
│ for DB queries   │
└──────────────────┘
```

---

## 9. Technology Stack Layers

```
┌────────────────────────────────────────────────────────────────┐
│                      PRESENTATION LAYER                        │
├────────────────────────────────────────────────────────────────┤
│  • ASP.NET Web Forms (.aspx, .aspx.vb)                         │
│  • Master Pages (.master)                                      │
│  • User Controls (.ascx)                                       │
│  • HTTP Handlers (.ashx)                                       │
│  • Client-side JavaScript                                      │
│  • CSS Styling                                                 │
│  • Bootstrap 3.x for UI components                             │
└────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                     APPLICATION LAYER                          │
├────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐ │
│  │  HTTP Pipeline                                            │ │
│  │    • ApplicationHttpModule (WWW removal, URL routing)    │ │
│  └──────────────────────────────────────────────────────────┘ │
│                              │                                 │
│  ┌──────────────────────────▼─────────────────────────────┐  │
│  │  Request Processing                                      │  │
│  │    • ApplicationPage (base page class)                   │  │
│  │    • ApplicationMasterPage (base master)                 │  │
│  │    • ApplicationUserControl (base control)               │  │
│  └──────────────────────────────────────────────────────────┘ │
│                              │                                 │
│  ┌──────────────────────────▼─────────────────────────────┐  │
│  │  Domain Resolution                                       │  │
│  │    • App.wpm_DomainConfig                                │  │
│  │    • DomainConfigurations.Load()                         │  │
│  └──────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                     BUSINESS LOGIC LAYER                       │
├────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐ │
│  │  Domain Models                                            │ │
│  │    • Company (site entity)                                │ │
│  │    • Location (page/content)                              │ │
│  │    • Article (content articles)                           │ │
│  │    • Part (reusable components)                           │ │
│  │    • Parameter (configuration)                            │ │
│  └──────────────────────────────────────────────────────────┘ │
│                              │                                 │
│  ┌──────────────────────────▼─────────────────────────────┐  │
│  │  Business Services                                       │  │
│  │    • Company.GetCompanyFromDB()                          │  │
│  │    • Location.BuildNavigationTree()                      │  │
│  │    • Article.LoadArticles()                              │  │
│  └──────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                    DATA ACCESS LAYER                           │
├────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐ │
│  │  ApplicationDAL Module                                    │ │
│  │    • GetCompanyData()                                     │ │
│  │    • GetLocationList()                                    │ │
│  │    • GetArticleList()                                     │ │
│  └──────────────────────────────────────────────────────────┘ │
│                              │                                 │
│  ┌──────────────────────────▼─────────────────────────────┐  │
│  │  UtilityDB Module                                        │  │
│  │    • wpm_GetDataTable() - SELECT queries                │  │
│  │    • wpm_RunInsertSQL() - INSERT/UPDATE/DELETE          │  │
│  │    • wpm_SQLDBConnString - Connection string provider   │  │
│  └──────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                      DATABASE LAYER                            │
├────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐ │
│  │  OLE DB Provider (Microsoft.ACE.OLEDB.12.0)              │ │
│  └──────────────────────────────────────────────────────────┘ │
│                              │                                 │
│  ┌──────────────────────────▼─────────────────────────────┐  │
│  │  MS Access Databases (.mdb files)                        │  │
│  │    • wpmFrogsFolly.mdb                                   │  │
│  │    • MineralCollection.mdb                               │  │
│  │    • ProjectMechanics.mdb                                │  │
│  │    • ... (16 total databases)                            │  │
│  └──────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────┘
```

---

## 10. Migration Approach Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│              CURRENT ARCHITECTURE (Legacy)                      │
├─────────────────────────────────────────────────────────────────┤
│  ASP.NET Web Forms + VB.NET                                     │
│  MS Access Databases (.mdb)                                     │
│  XML Configuration Files                                        │
│  Application State Caching                                      │
│  No API Layer                                                   │
└─────────────────────────────────────────────────────────────────┘
                          │
                          │ MIGRATION PATH
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│              TARGET ARCHITECTURE (Modern)                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  FRONTEND (Decoupled)                                     │  │
│  │  • React / Vue.js / Angular + TypeScript                 │  │
│  │  • Next.js / Nuxt.js for SSR                             │  │
│  │  • Tailwind CSS                                           │  │
│  │  • Component-based architecture                           │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                      │
│                          │ REST API / GraphQL                   │
│                          ▼                                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  BACKEND API                                              │  │
│  │  • ASP.NET Core 8.0 (C#) or Node.js/Express             │  │
│  │  • RESTful API design                                     │  │
│  │  • JWT authentication                                     │  │
│  │  • OpenAPI/Swagger documentation                          │  │
│  │  • Middleware for tenant resolution                       │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                      │
│                          │ Entity Framework Core / Dapper       │
│                          ▼                                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  DATABASE                                                 │  │
│  │  • PostgreSQL or SQL Server                              │  │
│  │  • Normalized schema with foreign keys                   │  │
│  │  • Indexes for performance                                │  │
│  │  • Multi-tenancy via tenant_id column                    │  │
│  │  • Row-level security                                     │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  CACHING LAYER                                            │  │
│  │  • Redis for distributed caching                          │  │
│  │  • CDN for static assets                                  │  │
│  │  • Application-level caching                              │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  INFRASTRUCTURE                                           │  │
│  │  • Docker containers                                       │  │
│  │  • Kubernetes orchestration                               │  │
│  │  • CI/CD pipelines (GitHub Actions, Azure DevOps)        │  │
│  │  • Monitoring & logging (Application Insights, ELK)      │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘

MIGRATION PHASES:

Phase 1: Database Migration
  MS Access (.mdb) → PostgreSQL/SQL Server
  ├─ Export data from each .mdb
  ├─ Transform schema to relational model
  ├─ Import to new database
  └─ Add tenant_id to all tables

Phase 2: API Development
  ├─ Design RESTful endpoints
  ├─ Implement tenant middleware
  ├─ Develop CRUD operations
  └─ Add authentication/authorization

Phase 3: Frontend Development
  ├─ Setup React/Next.js project
  ├─ Create reusable components
  ├─ Implement routing
  └─ Connect to API

Phase 4: Testing & Deployment
  ├─ Unit tests
  ├─ Integration tests
  ├─ Performance testing
  └─ Gradual rollout

Phase 5: Data Sync & Cutover
  ├─ Run both systems in parallel
  ├─ Sync data between systems
  ├─ Validate data integrity
  └─ DNS cutover
```

---

## Conclusion

These diagrams provide visual representations of:
1. System architecture and topology
2. Request flow and processing
3. Domain-to-database mapping
4. Configuration loading mechanism
5. Database schema and relationships
6. Class hierarchy and object model
7. Deployment structure
8. Memory and caching architecture
9. Technology stack layers
10. Migration path to modern architecture

Use these diagrams alongside the main documentation for a complete understanding of the Web Project Mechanics multi-domain, multi-database system.
