# WebProjectMechanics Multi-Domain Architecture Documentation

## Documentation Overview

This documentation package provides a complete specification and implementation guide for the Web Project Mechanics (WPM) system - a sophisticated multi-domain, multi-database web content management platform.

---

## Documentation Files

This package consists of three comprehensive documents:

### 1. MULTI_DOMAIN_ARCHITECTURE.md
**Primary technical specification** - 60+ pages of detailed documentation

**Contents:**
- Executive Summary
- Complete System Architecture
- Multi-Domain Support Mechanism
- Multi-Database Architecture (MS Access)
- Core Components & Data Models
- Request Flow & Lifecycle
- Complete Database Schema
- Configuration Management
- Security Analysis
- Migration Guide to Modern Stack
- Code Examples & Appendices

**Audience**: Architects, senior developers, project managers planning migration

**Use Case**: Complete understanding of how the system works, migration planning, architecture decisions

---

### 2. VISUAL_DIAGRAMS.md
**Visual architecture reference** - 10 comprehensive ASCII/text diagrams

**Contents:**
1. System Context Diagram
2. Request Flow Sequence Diagram
3. Domain-to-Database Mapping
4. Configuration Loading Flow
5. Database Entity Relationship Diagram
6. Application Class Hierarchy
7. Deployment Topology
8. Memory & Caching Architecture
9. Technology Stack Layers
10. Migration Approach Diagram

**Audience**: All technical staff, visual learners

**Use Case**: Quick visualization of system architecture, training, presentations

---

### 3. QUICK_REFERENCE.md
**Developer quick start guide** - Condensed reference manual

**Contents:**
- System at a Glance
- Key Files & Locations
- Domain Configuration Format
- Database Schema Summary
- Common Code Patterns
- Request Lifecycle
- Important Properties & Functions
- Adding New Sites
- Debugging Tips
- Common Tasks
- Troubleshooting Guide

**Audience**: Developers actively working with the system

**Use Case**: Day-to-day development, troubleshooting, quick lookups

---

## What is Web Project Mechanics?

### System Purpose

Web Project Mechanics is a web content management system that hosts **36+ independent websites** from a **single codebase**, where each website can have:

- ✅ Its own domain name
- ✅ Its own MS Access database  
- ✅ Its own content hierarchy
- ✅ Its own configuration settings
- ✅ Its own visual theme

### Key Architecture Highlights

```
36 Domains → 16 MS Access Databases → 1 Application Codebase
```

**Technology:**
- ASP.NET Web Forms (VB.NET)
- .NET Framework 4.8
- Microsoft Access (.mdb) databases
- XML configuration files
- IIS with custom HTTP Module

**Scale:**
- 36 configured domains
- 16 unique databases (many-to-one mapping)
- Single application serving all sites
- Domain resolution at request time

---

## How It Works (30-Second Overview)

```
1. User visits: frogsfolly.com
   ↓
2. System reads domain name from HTTP request
   ↓
3. Loads XML config: /App_Data/Sites/frogsfolly.com.xml
   ↓
4. Config specifies:
   - Database: wpmFrogsFolly.mdb
   - CompanyID: 1
   ↓
5. Application connects to that database
   ↓
6. Queries Company table for CompanyID=1
   ↓
7. Loads content hierarchy for that company
   ↓
8. Renders page with company-specific content
```

**Result**: Each domain gets its own content from its own database, but all using the same application code.

---

## Key Features

### 1. Dynamic Domain Resolution
- Runtime detection of incoming domain
- Automatic configuration loading
- Application-level caching

### 2. Multi-Database Support  
- Each domain can use separate database
- Multiple domains can share one database
- OLE DB connection management

### 3. Hierarchical Content Model
- Nested page/location structure
- Articles attached to locations
- Reusable parts/components
- Navigation auto-generation

### 4. Single Codebase
- All sites share business logic
- Shared UI components
- Centralized updates
- Consistent functionality

### 5. XML-Based Configuration
- Simple file-based setup
- No code changes for new sites
- Easy to backup and version

---

## Example Domain Configurations

### Example 1: Single Domain, Single Database
```
Domain: jmshawminerals.com
Database: MineralCollection.mdb (15 MB)
CompanyID: 25
Content: 500+ locations, 200+ articles
```

### Example 2: Multiple Domains, Shared Database
```
Database: FrogsFollyKids.mdb (4 MB)
Contains 7 companies:
  - lauren.frogsfolly.com (CompanyID: 4)
  - sarah.frogsfolly.com (CompanyID: 5)  
  - jordan.frogsfolly.com (CompanyID: 6)
  - berit.frogsfolly.com (CompanyID: 7)
  - mateus.frogsfolly.com (CompanyID: 8)
  - marlis.frogsfolly.com (CompanyID: 9)
  - ian.frogsfolly.com (CompanyID: 10)
```

---

## Documentation Use Cases

### For Architects
**Read**: MULTI_DOMAIN_ARCHITECTURE.md (full document)
**Focus on**: 
- Section 2: System Architecture Overview
- Section 3: Multi-Domain Support
- Section 4: Multi-Database Architecture
- Section 10: Migration Guide

**Goal**: Understand the complete architecture for planning migration or improvements

---

### For Developers (New to System)
**Start with**: QUICK_REFERENCE.md
**Then review**: VISUAL_DIAGRAMS.md
**Reference**: MULTI_DOMAIN_ARCHITECTURE.md (specific sections as needed)

**Goal**: Get productive quickly, understand common patterns

---

### For Developers (Maintaining System)
**Primary**: QUICK_REFERENCE.md (keep open while coding)
**Reference**: MULTI_DOMAIN_ARCHITECTURE.md (when deep dive needed)
**Visual aid**: VISUAL_DIAGRAMS.md (when explaining to others)

**Goal**: Efficient day-to-day maintenance and enhancements

---

### For Project Managers
**Read**: 
- MULTI_DOMAIN_ARCHITECTURE.md (Executive Summary, Migration Guide)
- VISUAL_DIAGRAMS.md (all diagrams)

**Goal**: Understand scope, effort, and approach for migration project

---

### For Migration Team
**Complete package required**
**Study order**:
1. MULTI_DOMAIN_ARCHITECTURE.md - Section 1-9 (understand current system)
2. VISUAL_DIAGRAMS.md - All diagrams (visualize architecture)
3. MULTI_DOMAIN_ARCHITECTURE.md - Section 10 (migration strategy)
4. QUICK_REFERENCE.md - Reference during data extraction

**Goal**: Successfully migrate to modern technology stack

---

## Key Concepts to Understand

### 1. Domain Configuration
Each domain has an XML file mapping it to a database:
```xml
<DomainConfigurations>
  <Configuration>
    <CompanyID>1</CompanyID>
    <DomainName>frogsfolly.com</DomainName>
    <AccessDatabasePath>/App_Data/wpmFrogsFolly.mdb</AccessDatabasePath>
    <SQLDBConnString>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|wpmFrogsFolly.mdb;</SQLDBConnString>
  </Configuration>
</DomainConfigurations>
```

### 2. Company Entity
Represents a website in the system:
- One Company record per domain (in database)
- Contains site title, home page, settings
- Links to all content via CompanyID

### 3. Location Hierarchy
Tree structure of pages/content:
- Self-referencing via ParentLocationID
- LevelNBR tracks depth
- Supports unlimited nesting

### 4. Application Cache
- Domain configurations cached in Application state
- Lifetime: Until application restart
- Thread-safe access
- Minimal memory footprint

### 5. Request-Time Resolution
- Domain resolved on every request
- Configuration retrieved from cache (or loaded)
- Database connection set dynamically
- All data sources updated automatically

---

## Migration Recommendations

### Target Modern Stack

**Backend:**
- ASP.NET Core 8.0 (C#) or Node.js/Express
- PostgreSQL or SQL Server (not Access)
- Entity Framework Core or Dapper
- Redis for distributed caching
- JWT authentication

**Frontend:**
- React with TypeScript
- Next.js for SSR (Server-Side Rendering)
- Tailwind CSS for styling
- RESTful API or GraphQL

**Infrastructure:**
- Docker containers
- Kubernetes orchestration  
- CI/CD pipelines (GitHub Actions)
- Cloud hosting (Azure, AWS)

### Migration Approach

**Phase 1**: Database Migration
- Convert 16 .mdb files to PostgreSQL/SQL Server
- Add tenant_id column to all tables
- Create proper foreign keys and indexes

**Phase 2**: API Development  
- Build RESTful API layer
- Implement tenant resolution middleware
- Add authentication/authorization

**Phase 3**: Frontend Development
- Create React components
- Implement routing
- Connect to API

**Phase 4**: Testing & Deployment
- Comprehensive testing (unit, integration, E2E)
- Gradual rollout
- DNS cutover

**Estimated Effort**: 6-12 months for complete migration (depending on team size)

---

## Database Statistics

**Total Databases**: 16 .mdb files
**Total Size**: ~120 MB
**Total Domains**: 36 configured
**Largest Database**: MineralCollection.mdb (15 MB, 500+ locations)
**Most Shared Database**: FrogsFollyKids.mdb (serves 7 domains)

### Database Breakdown
```
16 MS Access databases containing:
  - 36+ Company records (one per domain)
  - 2000+ Location records (pages/content)
  - 500+ Article records
  - 200+ Part records (reusable components)
  - 100+ Parameter records (configuration)
```

---

## File Locations

```
.documentation/
├── README.md (this file)
├── MULTI_DOMAIN_ARCHITECTURE.md (60+ pages)
├── VISUAL_DIAGRAMS.md (10 diagrams)
└── QUICK_REFERENCE.md (condensed guide)

website/
├── App_Data/
│   ├── Sites/ (36 XML domain configs)
│   └── *.mdb (16 Access databases)
└── Web.config

WebProjectMechanics/ (Core library)
├── Company/ (Domain models)
├── Location/ (Content models)
├── Article/ (Article models)
└── Utility/ (Core functions)
```

---

## Quick Navigation

### Need to understand...

**How domain resolution works?**
→ MULTI_DOMAIN_ARCHITECTURE.md, Section 3.1
→ VISUAL_DIAGRAMS.md, Diagram 4

**How to add a new site?**  
→ QUICK_REFERENCE.md, "Adding a New Site"

**Database schema?**
→ MULTI_DOMAIN_ARCHITECTURE.md, Section 7.1  
→ VISUAL_DIAGRAMS.md, Diagram 5

**Request flow?**
→ MULTI_DOMAIN_ARCHITECTURE.md, Section 6.1
→ VISUAL_DIAGRAMS.md, Diagram 2

**How to migrate to modern stack?**
→ MULTI_DOMAIN_ARCHITECTURE.md, Section 10
→ VISUAL_DIAGRAMS.md, Diagram 10

**Common development tasks?**
→ QUICK_REFERENCE.md, "Common Tasks"

**Code patterns?**
→ QUICK_REFERENCE.md, "Common Code Patterns"
→ MULTI_DOMAIN_ARCHITECTURE.md, Appendix C

**Troubleshooting?**
→ QUICK_REFERENCE.md, "Debugging Tips"
→ QUICK_REFERENCE.md, "Common Errors & Solutions"

---

## Security Notice

The current system has several known security vulnerabilities documented in:
- MULTI_DOMAIN_ARCHITECTURE.md, Section 9.2
- QUICK_REFERENCE.md, "Security Considerations"

**Key Issues:**
- SQL injection risk (string concatenation)
- XSS vulnerabilities (unencoded HTML)
- Path traversal risks
- No CSRF protection
- Session fixation vulnerability

**Recommendation**: These should be addressed immediately if the system handles sensitive data or is publicly accessible.

---

## Support & Contribution

### For Questions
Refer to the specific documentation sections listed above

### For Issues or Updates
- Document any system changes
- Update affected documentation files
- Keep architecture diagrams in sync with code

### For New Features
- Update MULTI_DOMAIN_ARCHITECTURE.md
- Add examples to QUICK_REFERENCE.md
- Update diagrams if architecture changes

---

## Document Versions

**Version**: 1.0  
**Date**: February 2026  
**Generated**: Through comprehensive code analysis and reverse engineering  
**Covers**: WebProjectMechanics as of February 2026

---

## Summary

This documentation package provides everything needed to:

✅ **Understand** the complete architecture  
✅ **Maintain** the current system  
✅ **Develop** new features  
✅ **Troubleshoot** issues  
✅ **Plan** a migration to modern stack  
✅ **Train** new team members  
✅ **Present** architecture to stakeholders

Total documentation: **100+ pages** covering all aspects of the system from high-level architecture to low-level implementation details.

---

## Next Steps

### If you're new to the system:
1. Start with QUICK_REFERENCE.md
2. Read MULTI_DOMAIN_ARCHITECTURE.md sections 1-2
3. Review VISUAL_DIAGRAMS.md
4. Deep dive into specific sections as needed

### If you're planning a migration:
1. Read MULTI_DOMAIN_ARCHITECTURE.md in full
2. Study VISUAL_DIAGRAMS.md carefully
3. Use QUICK_REFERENCE.md during data extraction
4. Follow migration guide in Section 10

### If you're maintaining the system:
1. Keep QUICK_REFERENCE.md handy
2. Reference specific sections of MULTI_DOMAIN_ARCHITECTURE.md
3. Use VISUAL_DIAGRAMS.md for training

---

**Last Updated**: February 16, 2026  
**Documentation Status**: Complete ✓

This documentation represents a complete reverse-engineered specification of the Web Project Mechanics multi-domain, multi-database architecture and provides all information necessary to understand, maintain, or migrate the system.
