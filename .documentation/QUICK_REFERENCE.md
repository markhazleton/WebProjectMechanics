# Web Project Mechanics - Quick Reference Guide

## Developer Quick Start

### Purpose
This is a condensed reference for developers working with or migrating from the Web Project Mechanics multi-domain system.

---

## System at a Glance

**What it does**: Hosts 36+ domains from one codebase, each with its own MS Access database

**Key Concept**: Domain name in URL → XML config → Database connection

**Example Flow**:
```
User visits: frogsfolly.com
     ↓
System reads: SERVER_NAME = "frogsfolly.com"
     ↓
Loads config: /App_Data/Sites/frogsfolly.com.xml
     ↓
Connects to: /App_Data/wpmFrogsFolly.mdb
     ↓
Shows content for CompanyID = 1
```

---

## Key Files & Locations

### Configuration
- **Domain configs**: `/website/App_Data/Sites/{domain}.xml`
- **Web config**: `/website/Web.config`
- **Databases**: `/website/App_Data/*.mdb`

### Core Code
- **Domain resolution**: `/WebProjectMechanics/Utility/Modules/App.vb`
- **Data access**: `/WebProjectMechanics/Utility/Modules/UtilityDB.vb`
- **Company model**: `/WebProjectMechanics/Company/Company.vb`
- **Location model**: `/WebProjectMechanics/Location/Location.vb`
- **Base page**: `/WebProjectMechanics/Utility/UIBase/ApplicationPage.vb`
- **HTTP module**: `/WebProjectMechanics/Utility/HttpModule/ApplicationHttpModule.vb`

---

## Domain Configuration

### XML Structure
```xml
<?xml version="1.0" encoding="utf-8"?>
<DomainConfigurations>
  <Configuration>
    <AccessDatabasePath>/App_Data/database.mdb</AccessDatabasePath>
    <CompanyID>1</CompanyID>
    <DomainName>example.com</DomainName>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|database.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

### Key Properties
- **AccessDatabasePath**: Path to .mdb file (relative to web root)
- **CompanyID**: Unique site identifier
- **DomainName**: Domain without "www."
- **SQLDBConnString**: OLE DB connection string

---

## Database Schema (Simplified)

### Core Tables

**Company** (One per site)
```
CompanyID (PK)
CompanyName
SiteURL (domain name)
SiteTitle
HomePageID
ActiveFL
```

**Location** (Pages/Content)
```
LocationID (PK)
ParentLocationID (FK to self)
LocationName
LocationBody (HTML)
LevelNBR (hierarchy depth)
DefaultOrder (sort)
ActiveFL
```

**Article** (Content)
```
ArticleID (PK)
ArticlePageID (FK to Location)
ArticleName
ArticleBody (HTML)
IsArticleActive
```

**Part** (Reusable components)
```
PartID (PK)
PartName
PartBody (HTML)
ActiveFL
```

---

## Common Code Patterns

### Get Current Domain Configuration
```vb
Dim config As DomainConfiguration = wpm_DomainConfig
Dim companyId As String = config.CompanyID
Dim connString As String = config.SQLDBConnString
```

### Query Database
```vb
' Get data table
Dim sql As String = "SELECT * FROM Location WHERE CompanyID = " & wpm_DomainConfig.CompanyID
Dim dt As DataTable = wpm_GetDataTable(sql, "LocationData")

' Process results
For Each row As DataRow In dt.Rows
    Dim locationName = wpm_GetDBString(row("LocationName"))
Next
```

### Load Company Data
```vb
Dim company As New Company()
company.GetCompanyFromDB(companyId, groupId, orderBy)

' Access data
Dim locations = company.Locations
Dim articles = company.Articles
```

---

## Request Lifecycle

1. **HTTP Request arrives**
   - User requests any .aspx page

2. **ApplicationHttpModule.BeginRequest**
   - Removes "www." if present
   - Normalizes URL

3. **Domain Resolution** (App.vpm_DomainConfig)
   - Extract domain from SERVER_NAME
   - Check Application cache
   - If not cached: Load XML → Parse → Cache
   - Return DomainConfiguration

4. **Page_Init** (ApplicationPage)
   - UpdateDataSourceConnection()
   - Sets all DataSource controls to use domain's database

5. **Content Loading**
   - Query Company table
   - Query Location table (hierarchy)
   - Query Article table
   - Render HTML

6. **Response**
   - Send HTML to browser

---

## Application State Caching

### What's Cached
```vb
Application("wpm_SiteConfig") = SiteConfig
Application("frogsfolly.com") = DomainConfiguration
Application("jmshawminerals.com") = DomainConfiguration
' ... one entry per domain
```

### Cache Lifetime
- **Duration**: Until application restart
- **Invalidation**: Web.config change, app pool recycle
- **Thread-safe**: Yes (Application state is locked)

### Cache Keys
```vb
Dim cacheKey As String = wpm_HostName  ' Current domain
Dim config = Application(cacheKey)
```

---

## Important Properties & Functions

### Global Properties
```vb
wpm_DomainConfig        ' Current domain configuration
wpm_SQLDBConnString     ' Connection string for current domain
wpm_AccessDatabasePath  ' Path to .mdb for current domain
wpm_SiteConfig          ' Global site configuration
wpm_HostName            ' Current domain name (normalized)
wpm_IsAdmin             ' Is user admin?
wpm_IsEditor            ' Is user editor?
wpm_CurrentPageID       ' Current page ID
wpm_CurrentArticleID    ' Current article ID
```

### Key Functions
```vb
wpm_GetDataTable(sql, tableName)           ' Execute SELECT
wpm_RunInsertSQL(sql, tableName)           ' Execute INSERT/UPDATE/DELETE
wpm_GetDBString(value)                     ' Safe string from DB
wpm_GetDBInteger(value)                    ' Safe integer from DB
wpm_IsValidURL(url)                        ' Validate URL format
```

---

## Domain-to-Database Mapping (Examples)

| Domain | Database | CompanyID |
|--------|----------|-----------|
| frogsfolly.com | wpmFrogsFolly.mdb | 1 |
| family.frogsfolly.com | FrogsFolly-Family.mdb | 2 |
| lauren.frogsfolly.com | FrogsFollyKids.mdb | 4 |
| sarah.frogsfolly.com | FrogsFollyKids.mdb | 5 |
| jmshawminerals.com | MineralCollection.mdb | 25 |
| webprojectmechanics.com | ProjectMechanics.mdb | 17 |

**Note**: Multiple domains can share the same database with different CompanyIDs.

---

## Adding a New Site

### Step 1: Create or Update Database
```sql
-- In MS Access
INSERT INTO Company (CompanyName, SiteURL, SiteTitle, ActiveFL)
VALUES ('New Site', 'newsite.com', 'New Site Title', True)
-- Note the new CompanyID
```

### Step 2: Create XML Configuration
```xml
<!-- /website/App_Data/Sites/newsite.com.xml -->
<?xml version="1.0" encoding="utf-8"?>
<DomainConfigurations>
  <Configuration>
    <AccessDatabasePath>/App_Data/database.mdb</AccessDatabasePath>
    <CompanyID>42</CompanyID>
    <DomainName>newsite.com</DomainName>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|database.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

### Step 3: Configure DNS
Point newsite.com A record to server IP

### Step 4: Add IIS Binding (if needed)
Usually not needed; system accepts all domains

### Step 5: Restart Application
Recycle app pool to clear cache

### Alternative: Use Config Generator
Visit: `/admin/BuildSiteConfig.ashx`
- Automatically scans databases
- Generates XML for all companies
- Creates missing config files

---

## Debugging Tips

### Check Current Domain Configuration
```vb
' In code-behind
Response.Write("Domain: " & wpm_DomainConfig.DomainName)
Response.Write("CompanyID: " & wpm_DomainConfig.CompanyID)
Response.Write("Database: " & wpm_DomainConfig.AccessDatabasePath)
Response.Write("ConnString: " & wpm_DomainConfig.SQLDBConnString)
```

### Verify Database Connection
```vb
Try
    Dim dt = wpm_GetDataTable("SELECT * FROM Company WHERE CompanyID = " & wpm_DomainConfig.CompanyID, "Test")
    Response.Write("Records found: " & dt.Rows.Count)
Catch ex As Exception
    Response.Write("Error: " & ex.Message)
End Try
```

### Check Application Cache
```vb
Response.Write("Cached domains:<br/>")
For Each key In Application.AllKeys
    Response.Write(key & "<br/>")
Next
```

### View Raw XML Config
Navigate to: `/App_Data/Sites/{domain}.xml` (if IIS allows)

---

## Security Considerations

### Known Vulnerabilities

1. **SQL Injection** - Queries use string concatenation
   ```vb
   ' VULNERABLE:
   String.Format("SELECT * FROM Location WHERE LocationID={0}", userInput)
   
   ' BETTER: Use parameterized queries
   ```

2. **XSS** - HTML rendered without encoding
   ```vb
   ' VULNERABLE:
   Response.Write(location.LocationBody)  ' No sanitization
   ```

3. **Path Traversal** - File paths constructed from input
   ```vb
   ' VULNERABLE:
   Server.MapPath("/App_Data/" & userInput & ".mdb")
   ```

4. **Session Fixation** - No session regeneration on login

5. **No CSRF Protection** - Forms lack anti-forgery tokens

### User Roles
- **UserGroupID = 1**: Administrator (full access)
- **UserGroupID = 2**: Editor (content management)
- **UserGroupID = 3**: User (limited access)
- **UserGroupID = 4**: Anonymous (public)

---

## Migration to Modern Stack

### Recommended Target Stack

**Backend**
- ASP.NET Core 8.0 (C#) or Node.js
- PostgreSQL or SQL Server
- Entity Framework Core
- Redis for caching
- JWT authentication

**Frontend**
- React with TypeScript
- Next.js for SSR
- Tailwind CSS
- REST API or GraphQL

### Migration Strategy

1. **Database Migration**
   - Convert .mdb to PostgreSQL/SQL Server
   - Add tenant_id column to all tables
   - Create proper foreign keys and indexes

2. **Multi-Tenancy Approach**
   ```csharp
   // Middleware to resolve tenant
   public class TenantMiddleware
   {
       public async Task InvokeAsync(HttpContext context)
       {
           var domain = context.Request.Host.Host;
           var tenant = await _tenantService.GetByDomainAsync(domain);
           context.Items["Tenant"] = tenant;
           await _next(context);
       }
   }
   ```

3. **API Design**
   ```
   GET  /api/companies
   GET  /api/locations
   GET  /api/locations/{id}
   POST /api/articles
   ```

4. **Global Query Filter**
   ```csharp
   modelBuilder.Entity<Location>()
       .HasQueryFilter(l => l.CompanyId == _tenant.Id);
   ```

---

## Common Tasks

### Update Domain Configuration
1. Edit `/App_Data/Sites/{domain}.xml`
2. Recycle application pool
3. Configuration reloads on next request

### Add Content
1. Insert into Location table with appropriate CompanyID
2. Set ParentLocationID for hierarchy
3. Set LevelNBR and DefaultOrder
4. Set ActiveFL = True

### Change Database for Domain
1. Update CompanyID in XML config
2. Update AccessDatabasePath and SQLDBConnString
3. Recycle app pool

### Debug 404 Errors
1. Check domain XML exists
2. Verify CompanyID in database
3. Check HomePageID points to valid Location
4. Verify ActiveFL = True for content

---

## File Locations Reference

```
/website/
├── App_Data/
│   ├── Sites/
│   │   ├── domain1.xml (36 files)
│   │   └── ...
│   └── *.mdb (16 database files)
├── admin/
│   ├── BuildSiteConfig.ashx (config generator)
│   └── SiteList.aspx (site management)
├── api/
│   └── *.ashx (API handlers)
├── Default.aspx (main page)
└── Web.config

/WebProjectMechanics/
├── Company/
│   ├── Company.vb
│   ├── DomainConfiguration.vb
│   └── DomainConfigurations.vb
├── Location/
│   └── Location.vb
├── Article/
│   └── Article.vb
└── Utility/
    ├── Modules/
    │   ├── App.vb (domain resolution)
    │   ├── ApplicationDAL.vb (data access)
    │   └── UtilityDB.vb (DB utilities)
    ├── UIBase/
    │   └── ApplicationPage.vb (base page)
    └── HttpModule/
        └── ApplicationHttpModule.vb
```

---

## Useful SQL Queries

### Get All Sites
```sql
SELECT CompanyID, CompanyName, SiteURL, ActiveFL
FROM Company
ORDER BY CompanyName
```

### Get Site Hierarchy
```sql
SELECT LocationID, LocationName, ParentLocationID, LevelNBR, DefaultOrder
FROM Location
WHERE CompanyID = {companyId}
  AND ActiveFL = True
ORDER BY LevelNBR, DefaultOrder
```

### Get Articles for Location
```sql
SELECT ArticleID, ArticleName, ArticleTitle, IsArticleActive
FROM Article
WHERE ArticlePageID = {locationId}
  AND IsArticleActive = True
ORDER BY ArticleModDate DESC
```

### Find Domain by CompanyID
```sql
SELECT CompanyID, CompanyName, SiteURL
FROM Company
WHERE CompanyID = {id}
```

---

## Environment Variables & Config

### Web.config Keys
```xml
<appSettings>
    <add key="wpm_ConfigFolder" value="~/App_Data/" />
</appSettings>
```

### Connection String Token
```
|DataDirectory| = /App_Data/
```

### Server Variables Used
```vb
Request.ServerVariables("SERVER_NAME")  ' Domain name
```

---

## Testing Checklist

### New Site Verification
- [ ] XML config created in /App_Data/Sites/
- [ ] Database has Company record with matching CompanyID
- [ ] Company.SiteURL matches domain name
- [ ] Database has at least one Location record
- [ ] Company.HomePageID points to valid Location
- [ ] DNS points to server
- [ ] Site loads without errors
- [ ] Navigation renders correctly
- [ ] Content displays properly

### Migration Testing
- [ ] All domains mapped correctly
- [ ] Database connections working
- [ ] Content loads for each domain
- [ ] Navigation works
- [ ] Search functionality
- [ ] Admin access
- [ ] Authentication
- [ ] File uploads
- [ ] Form submissions

---

## Performance Tips

### Current System
- No data caching (every request queries database)
- Application-level config caching only
- MS Access has limited concurrency
- No CDN for static assets

### Optimization Opportunities
1. Cache Company data (rarely changes)
2. Cache Location hierarchy (changes infrequently)
3. Use CDN for images and static files
4. Implement output caching for pages
5. Add connection pooling
6. Migrate to SQL Server for better performance

---

## Support & Resources

### Key Concepts to Understand
1. **Domain Resolution**: How domain name maps to database
2. **Multi-Tenancy**: One codebase, multiple sites
3. **Hierarchical Content**: Location parent-child relationships
4. **Application State**: Global caching mechanism
5. **OLE DB**: Database access technology

### Documentation Files
- `MULTI_DOMAIN_ARCHITECTURE.md` - Complete specification
- `VISUAL_DIAGRAMS.md` - System diagrams
- `README.md` - Project overview

### Admin Tools
- `/admin/BuildSiteConfig.ashx` - Generate configs
- `/admin/SiteList.aspx` - Manage sites
- `/admin/` - Admin dashboard

---

## Glossary

- **Company**: A website/domain in the system
- **Location**: A page or content node
- **Article**: Content piece attached to location
- **Part**: Reusable content component
- **Parameter**: Configuration key-value pair
- **CompanyID**: Unique site identifier
- **LocationID**: Unique page identifier
- **ParentLocationID**: Parent page in hierarchy
- **LevelNBR**: Depth in content tree (0 = root)
- **DefaultOrder**: Sort sequence
- **ActiveFL**: Published status flag
- **RecordSource**: Type ("Page", "Article", "Part")
- **BreadcrumbURL**: SEO-friendly URL path
- **|DataDirectory|**: Resolves to /App_Data/
- **wpm_**: Prefix for Web Project Mechanics functions

---

## Quick Commands

### View All Configured Domains
```bash
# Windows
dir C:\inetpub\wwwroot\App_Data\Sites\*.xml
```

### View All Databases
```bash
# Windows  
dir C:\inetpub\wwwroot\App_Data\*.mdb
```

### Recycle App Pool
```powershell
# PowerShell
Restart-WebAppPool -Name "DefaultAppPool"
```

### Clear Application Cache
Recycle app pool or touch Web.config

---

## Common Errors & Solutions

### "Site not found" / 404 Error
- **Cause**: No XML config for domain
- **Solution**: Create config or use BuildSiteConfig.ashx

### "Invalid Site Config"
- **Cause**: Malformed XML or missing CompanyID
- **Solution**: Validate XML structure, check CompanyID

### "Could not find file"
- **Cause**: Database file path wrong
- **Solution**: Verify AccessDatabasePath in XML

### Database Connection Errors
- **Cause**: Wrong connection string or missing ACE provider
- **Solution**: Install Access Database Engine, fix connection string

### Content Not Showing
- **Cause**: ActiveFL = False or wrong CompanyID
- **Solution**: Check Company and Location tables for ActiveFL and CompanyID

---

This quick reference provides the essential information for working with or migrating the Web Project Mechanics system. Refer to the complete documentation for detailed explanations.
