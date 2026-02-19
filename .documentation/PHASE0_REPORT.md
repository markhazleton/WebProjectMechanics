# Phase 0 — Migration Readiness Report

**Generated:** 2026-02-17 14:01
**Source:** `archive/website/App_Data/`

---

## 1. Site Configuration Map (from XML configs)

**Total domains configured:** 36

| Domain | CompanyID | MDB File | Config File |
|--------|----------|----------|-------------|
| family.frogsfolly.com | 1 | FrogsFolly-Family.mdb | family.frogsfolly.com.xml |
| travel.frogsfolly.com | 1 | FrogsFolly-Travel.mdb | localhost.xml |
| travel.frogsfolly.com | 1 | FrogsFolly-Travel.mdb | travel.frogsfolly.com.xml |
| dramaeducator.com | 21 | FrogsFolly.mdb | dramaeducator.com.xml |
| MacCloskeyAndMyers.com | 29 | FrogsFolly.mdb | MacCloskeyAndMyers.com.xml |
| TexEcon.com | 32 | FrogsFolly.mdb | TexEcon.com.xml |
| houstonlakeshore.com | 37 | FrogsFolly.mdb | houstonlakeshore.com.xml |
| marlis.frogsfolly.com | 36 | FrogsFollyKids.mdb | marlis.frogsfolly.com.xml |
| ian.frogsfolly.com | 37 | FrogsFollyKids.mdb | ian.frogsfolly.com.xml |
| berit.frogsfolly.com | 39 | FrogsFollyKids.mdb | berit.frogsfolly.com.xml |
| jordan.frogsfolly.com | 40 | FrogsFollyKids.mdb | jordan.frogsfolly.com.xml |
| lauren.frogsfolly.com | 41 | FrogsFollyKids.mdb | lauren.frogsfolly.com.xml |
| sarah.frogsfolly.com | 42 | FrogsFollyKids.mdb | sarah.frogsfolly.com.xml |
| studentcouncil.frogsfolly.com | 43 | FrogsFollyKids.mdb | studentcouncil.frogsfolly.com.xml |
| mateus.frogsfolly.com | 44 | FrogsFollyKids.mdb | mateus.frogsfolly.com.xml |
| jmshawminerals.com | 29 | MineralCollection.mdb | jmshawminerals.com.xml |
| nrc.controlorigins.com | 30 | MineralCollection.mdb | nrc.controlorigins.com.xml |
| pm.controlorigins.com | 8 | ProjectMechanics.mdb | pm.controlorigins.com.xml |
| controlorigins.com | 13 | ProjectMechanics.mdb | controlorigins.com.xml |
| wpm.markhazleton.com | 17 | ProjectMechanics.mdb | wpm.controlorigins.com.xml |
| dearinggroup.com | 26 | ProjectMechanics.mdb | dearinggroup.com.xml |
| osmcinc.projectmechanics.com | 29 | ProjectMechanics.mdb | osmcinc.projectmechanics.com.xml |
| originscorp.com | 30 | ProjectMechanics.mdb | originscorp.com.xml |
| mc.controlorigins.com | 31 | ProjectMechanics.mdb | mc.controlorigins.com.xml |
| clients.controlorigins.com | 32 | ProjectMechanics.mdb | clients.controlorigins.com.xml |
| blog.controlorigins.com | 33 | ProjectMechanics.mdb | blog.controlorigins.com.xml |
| webprojectmechanics.com | 31 | wpm-demo.mdb | webprojectmechanics.com.xml |
| frogsfolly.com | 1 | wpmFrogsFolly.mdb | frogsfolly.com.xml |
| LakeClaiborneHouse.com | 23 | wpmLiving.mdb | LakeClaiborneHouse.com.xml |
| inkslakeliving.com | 25 | wpmLiving.mdb | inkslakeliving.com.xml |
| burnetcountyliving.com | 26 | wpmLiving.mdb | burnetcountyliving.com.xml |
| kellertexasliving.com | 28 | wpmLiving.mdb | kellertexasliving.com.xml |
| lakebuchananliving.com | 35 | wpmLiving.mdb | lakebuchananliving.com.xml |
| GraysonGeorgiaLiving.com | 36 | wpmLiving.mdb | GraysonGeorgiaLiving.com.xml |
| mechanicsofmotherhood.com | 18 | wpmMOM.mdb | mechanicsofmotherhood.com.xml |
| holdcompany.com | 19 | wpmMOM.mdb | holdcompany.com.xml |

### Databases by Domain Count

| MDB File | Domains | CompanyIDs |
|----------|---------|------------|
| FrogsFolly-Family.mdb | 1 | 1 |
| FrogsFolly-Travel.mdb | 2 | 1, 1 |
| FrogsFolly.mdb | 4 | 21, 29, 32, 37 |
| FrogsFollyKids.mdb | 8 | 36, 37, 39, 40, 41, 42, 43, 44 |
| MineralCollection.mdb | 2 | 29, 30 |
| ProjectMechanics.mdb | 9 | 8, 13, 17, 26, 29, 30, 31, 32, 33 |
| wpm-demo.mdb | 1 | 31 |
| wpmFrogsFolly.mdb | 1 | 1 |
| wpmLiving.mdb | 6 | 23, 25, 26, 28, 35, 36 |
| wpmMOM.mdb | 2 | 18, 19 |

### Database Classification

| MDB File | Size (KB) | Status |
|----------|----------|--------|
| 1-MineralCollection.mdb | 1,184 | **Unreferenced** — backup/template? |
| DramaEducator.mdb | 1,224 | **Unreferenced** — backup/template? |
| FrogsFolly-Family.mdb | 2,580 | Active (referenced by XML config) |
| FrogsFolly-Travel.mdb | 2,760 | Active (referenced by XML config) |
| FrogsFolly.mdb | 3,448 | Active (referenced by XML config) |
| FrogsFollyBase.mdb | 2,612 | **Unreferenced** — backup/template? |
| FrogsFollyKids.mdb | 1,760 | Active (referenced by XML config) |
| InformationCenter.mdb | 6,756 | **Unreferenced** — backup/template? |
| MARKInformationCenter.mdb | 7,080 | **Unreferenced** — backup/template? |
| MineralCollection.mdb | 1,204 | Active (referenced by XML config) |
| osmcinc.mdb | 1,148 | **Unreferenced** — backup/template? |
| ProjectMechanics.mdb | 2,336 | Active (referenced by XML config) |
| wpm-demo.mdb | 1,164 | Active (referenced by XML config) |
| wpmFrogsFolly.mdb | 3,812 | Active (referenced by XML config) |
| wpmLiving.mdb | 2,288 | Active (referenced by XML config) |
| wpmMOM.mdb | 1,332 | Active (referenced by XML config) |

---

## 2. Schema Inventory

### 1-MineralCollection.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 5 |
| Company | 25 | 2 |
| CompanySiteParameter | 7 | 0 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 6 |
| Group | 3 | 3 |
| Image | 19 | 0 |
| Link | 15 | 0 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Message | 10 | 0 |
| Page | 19 | 8 |
| PageAlias | 5 | 1 |
| PageImage | 4 | 0 |
| PageRole | 4 | 1 |
| PageType | 4 | 5 |
| role | 5 | 2 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 9 |
| SiteTemplate | 5 | 2 |

<details><summary>Column details</summary>

**Article** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 50 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 50 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 50 |
| VersionNo | Integer | Yes | - |

**Link** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Message** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Author | WChar (Text) | Yes | 50 |
| Body | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| Email | WChar (Text) | Yes | 100 |
| MessageDate | Date | Yes | - |
| MessageID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentMessageID | Integer | Yes | - |
| Subject | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | 50 |

**Page** (8 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 255 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### DramaEducator.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 4 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 2 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 23 |
| Group | 3 | 4 |
| Image | 19 | 0 |
| Link | 15 | 1 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 4 |
| PageAlias | 5 | 0 |
| PageImage | 4 | 0 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 7 |
| SiteTemplate | 5 | 2 |

<details><summary>Column details</summary>

**Article** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (23 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### FrogsFolly-Family.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 22 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 5 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 27 | 22 |
| Group | 3 | 4 |
| Image | 19 | 3,064 |
| Link | 15 | 3 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 91 |
| PageAlias | 5 | 11 |
| PageImage | 4 | 2,902 |
| PageType | 4 | 10 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 6 |
| SiteTemplate | 5 | 48 |

<details><summary>Column details</summary>

**Article** (22 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (22 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (3,064 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (91 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (2,902 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (48 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### FrogsFolly-Travel.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 87 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 5 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 1 |
| Group | 3 | 4 |
| Image | 19 | 1,804 |
| Link | 15 | 10 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 121 |
| PageAlias | 5 | 13 |
| PageImage | 4 | 1,738 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 9 |
| SiteTemplate | 5 | 29 |

<details><summary>Column details</summary>

**Article** (87 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | No | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (1,804 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 254 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (121 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (1,738 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (29 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### FrogsFolly.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 48 |
| Company | 25 | 5 |
| CompanySiteParameter | 7 | 13 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 30 |
| Group | 3 | 4 |
| Image | 19 | 80 |
| Link | 15 | 7 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 42 |
| PageAlias | 5 | 0 |
| PageImage | 4 | 66 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 259 |
| SiteCategoryGroup | 4 | 12 |
| SiteCategoryType | 7 | 3 |
| SiteLink | 16 | 6 |
| SiteParameterType | 5 | 14 |
| SiteTemplate | 5 | 51 |

<details><summary>Column details</summary>

**Article** (48 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (30 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (80 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (42 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (66 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (259 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (12 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (14 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (51 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### FrogsFollyBase.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 98 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 2 |
| CompanySiteTypeParameter | 8 | 1 |
| Contact | 27 | 1 |
| Group | 3 | 4 |
| Image | 19 | 354 |
| Link | 15 | 68 |
| LinkCategory | 5 | 9 |
| LinkRank | 6 | 2 |
| LinkType | 4 | 13 |
| Page | 19 | 32 |
| PageAlias | 5 | 1,650 |
| PageImage | 4 | 0 |
| PageType | 4 | 11 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 4 |
| SiteTemplate | 5 | 50 |

<details><summary>Column details</summary>

**Article** (98 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (354 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (68 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (32 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (1,650 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageType** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (50 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### FrogsFollyKids.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 27 |
| Company | 25 | 8 |
| CompanySiteParameter | 7 | 3 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 9 |
| Group | 3 | 4 |
| Image | 19 | 0 |
| Link | 15 | 6 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 16 |
| PageAlias | 5 | 0 |
| PageImage | 4 | 0 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 3 |
| SiteTemplate | 5 | 54 |

<details><summary>Column details</summary>

**Article** (27 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (8 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (16 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | No | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (54 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### InformationCenter.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 3 |
| Company | 25 | 73 |
| CompanySiteParameter | 7 | 739 |
| CompanySiteTypeParameter | 8 | 2,858 |
| Contact | 28 | 7 |
| Group | 3 | 4 |
| Image | 19 | 0 |
| Link | 15 | 6 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 9 |
| Page | 19 | 75 |
| PageAlias | 5 | 68 |
| PageImage | 4 | 0 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 414 |
| SiteCategoryGroup | 4 | 56 |
| SiteCategoryType | 7 | 11 |
| SiteLink | 16 | 409 |
| SiteParameterType | 5 | 286 |
| sites | 10 | 0 |
| SiteTemplate | 5 | 13 |

<details><summary>Column details</summary>

**Article** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | - |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (73 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (739 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (2,858 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (75 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (68 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (414 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | No | - |

**SiteCategoryGroup** (56 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (409 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | No | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (286 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**sites** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| active | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| frequency | Integer | Yes | - |
| name | WChar (Text) | Yes | 50 |
| Order | Integer | Yes | - |
| PageID | Integer | Yes | - |
| siteID | Integer | No | - |
| update_ts | Date | Yes | - |
| url | WChar (Text) | Yes | 100 |
| xsl | WChar (Text) | Yes | 50 |

**SiteTemplate** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### MARKInformationCenter.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 3 |
| Company | 25 | 73 |
| CompanySiteParameter | 7 | 740 |
| CompanySiteTypeParameter | 8 | 2,513 |
| Contact | 28 | 7 |
| Group | 3 | 4 |
| Image | 19 | 0 |
| Link | 15 | 6 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 9 |
| Page | 19 | 75 |
| PageAlias | 5 | 68 |
| PageImage | 4 | 0 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 501 |
| SiteCategoryGroup | 4 | 56 |
| SiteCategoryType | 7 | 11 |
| SiteLink | 16 | 330 |
| SiteParameterType | 5 | 267 |
| sites | 10 | 0 |
| SiteTemplate | 5 | 13 |

<details><summary>Column details</summary>

**Article** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | - |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (73 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (740 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (2,513 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (75 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (68 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (501 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | No | - |

**SiteCategoryGroup** (56 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (330 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | No | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (267 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**sites** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| active | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| frequency | Integer | Yes | - |
| name | WChar (Text) | Yes | 50 |
| Order | Integer | Yes | - |
| PageID | Integer | Yes | - |
| siteID | Integer | No | - |
| update_ts | Date | Yes | - |
| url | WChar (Text) | Yes | 100 |
| xsl | WChar (Text) | Yes | 50 |

**SiteTemplate** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### MineralCollection.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 6 |
| Company | 25 | 2 |
| CompanySiteParameter | 7 | 0 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 27 | 6 |
| Group | 3 | 3 |
| Image | 19 | 2 |
| Link | 15 | 2 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Message | 10 | 0 |
| Page | 19 | 9 |
| PageAlias | 5 | 1 |
| PageImage | 4 | 0 |
| PageType | 4 | 5 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 9 |
| SiteTemplate | 5 | 5 |

<details><summary>Column details</summary>

**Article** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 50 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 50 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 50 |
| VersionNo | Integer | Yes | - |

**Link** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Message** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Author | WChar (Text) | Yes | 50 |
| Body | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| Email | WChar (Text) | Yes | 100 |
| MessageDate | Date | Yes | - |
| MessageID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentMessageID | Integer | Yes | - |
| Subject | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | 50 |

**Page** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 255 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageType** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### osmcinc.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 1 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 0 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 1 |
| Group | 3 | 3 |
| Image | 19 | 0 |
| Link | 15 | 0 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Message | 10 | 0 |
| Page | 19 | 3 |
| PageAlias | 5 | 0 |
| PageImage | 4 | 0 |
| PageRole | 4 | 1 |
| PageType | 4 | 5 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 9 |
| SiteTemplate | 5 | 1 |

<details><summary>Column details</summary>

**Article** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 50 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 50 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 50 |
| VersionNo | Integer | Yes | - |

**Link** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Message** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Author | WChar (Text) | Yes | 50 |
| Body | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| Email | WChar (Text) | Yes | 100 |
| MessageDate | Date | Yes | - |
| MessageID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentMessageID | Integer | Yes | - |
| Subject | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | 50 |

**Page** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 255 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### ProjectMechanics.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 130 |
| Company | 25 | 9 |
| CompanySiteParameter | 7 | 12 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 27 | 4 |
| Group | 3 | 4 |
| Image | 19 | 43 |
| Link | 15 | 54 |
| LinkCategory | 5 | 8 |
| LinkRank | 6 | 2 |
| LinkType | 4 | 13 |
| Message | 10 | 1 |
| Page | 19 | 49 |
| PageAlias | 5 | 100 |
| PageImage | 4 | 8 |
| PageType | 4 | 7 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 1 |
| SiteParameterType | 5 | 12 |
| SiteTemplate | 5 | 14 |

<details><summary>Column details</summary>

**Article** (130 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (12 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (43 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 50 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 50 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 50 |
| VersionNo | Integer | Yes | - |

**Link** (54 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (8 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Message** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Author | WChar (Text) | Yes | 50 |
| Body | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| Email | WChar (Text) | Yes | 100 |
| MessageDate | Date | Yes | - |
| MessageID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentMessageID | Integer | Yes | - |
| Subject | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | 50 |

**Page** (49 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 255 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (100 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (8 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageType** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (12 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (14 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### wpm-demo.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 4 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 3 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 27 | 1 |
| Group | 3 | 4 |
| Image | 19 | 3 |
| Link | 15 | 3 |
| LinkCategory | 5 | 3 |
| LinkType | 4 | 12 |
| Page | 19 | 4 |
| PageAlias | 5 | 8 |
| PageImage | 4 | 3 |
| PageType | 4 | 10 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 3 |
| SiteTemplate | 5 | 7 |

<details><summary>Column details</summary>

**Article** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkType** (12 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (8 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### wpmFrogsFolly.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 166 |
| Company | 25 | 1 |
| CompanySiteParameter | 7 | 5 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 57 |
| Group | 3 | 4 |
| Image | 19 | 4,445 |
| Link | 15 | 70 |
| LinkCategory | 5 | 9 |
| LinkRank | 6 | 2 |
| LinkType | 4 | 13 |
| Page | 19 | 205 |
| PageAlias | 5 | 11 |
| PageImage | 4 | 4,436 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 6 |
| SiteTemplate | 5 | 47 |

<details><summary>Column details</summary>

**Article** (166 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (5 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (57 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (4,445 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (70 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (9 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (205 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (4,436 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (47 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### wpmLiving.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 47 |
| Company | 25 | 6 |
| CompanySiteParameter | 7 | 7 |
| CompanySiteTypeParameter | 8 | 1 |
| Contact | 28 | 6 |
| Group | 3 | 4 |
| Image | 19 | 134 |
| Link | 15 | 34 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Page | 19 | 41 |
| PageAlias | 5 | 0 |
| PageImage | 4 | 127 |
| PageRole | 4 | 0 |
| PageType | 4 | 10 |
| role | 5 | 1 |
| SiteCategory | 10 | 255 |
| SiteCategoryGroup | 4 | 12 |
| SiteCategoryType | 7 | 2 |
| SiteLink | 16 | 6 |
| SiteParameterType | 5 | 13 |
| SiteTemplate | 5 | 49 |

<details><summary>Column details</summary>

**Article** (47 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | No | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (7 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (134 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 255 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 255 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 255 |
| VersionNo | Integer | Yes | - |

**Link** (34 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Page** (41 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 50 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (127 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (10 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (255 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (12 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (49 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### wpmMOM.mdb

| Table | Columns | Row Count |
|-------|---------|-----------|
| Article | 17 | 11 |
| Company | 25 | 2 |
| CompanySiteParameter | 7 | 1 |
| CompanySiteTypeParameter | 8 | 0 |
| Contact | 28 | 2 |
| Group | 3 | 4 |
| Image | 19 | 0 |
| Link | 15 | 15 |
| LinkCategory | 5 | 3 |
| LinkRank | 6 | 0 |
| LinkType | 4 | 13 |
| Message | 10 | 0 |
| Page | 19 | 4 |
| PageAlias | 5 | 1 |
| PageImage | 4 | 0 |
| PageRole | 4 | 1 |
| PageType | 4 | 6 |
| role | 5 | 1 |
| SiteCategory | 10 | 0 |
| SiteCategoryGroup | 4 | 0 |
| SiteCategoryType | 7 | 1 |
| SiteLink | 16 | 0 |
| SiteParameterType | 5 | 6 |
| SiteTemplate | 5 | 6 |

<details><summary>Column details</summary>

**Article** (11 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| ArticleBody | WChar (Text) | Yes | - |
| ArticleID | Integer | No | - |
| ArticleSummary | WChar (Text) | Yes | - |
| Author | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| Counter | Integer | Yes | - |
| Description | WChar (Text) | Yes | 255 |
| EndDT | Date | Yes | - |
| ExpireDT | Date | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageID | Integer | Yes | - |
| StartDT | Date | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| userID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**Company** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ActiveFL | Boolean | No | 2 |
| Address | WChar (Text) | Yes | 255 |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | No | - |
| CompanyName | WChar (Text) | No | 50 |
| Component | WChar (Text) | Yes | 50 |
| Country | WChar (Text) | Yes | 50 |
| DefaultArticleID | Integer | Yes | - |
| DefaultInvoiceDescription | WChar (Text) | Yes | - |
| DefaultPaymentTerms | WChar (Text) | Yes | 255 |
| DefaultSiteTemplate | WChar (Text) | Yes | 50 |
| FaxNumber | WChar (Text) | Yes | 30 |
| FromEmail | WChar (Text) | Yes | 50 |
| GalleryFolder | WChar (Text) | Yes | 50 |
| HomePageID | Integer | Yes | - |
| PhoneNumber | WChar (Text) | Yes | 30 |
| PostalCode | WChar (Text) | Yes | 20 |
| SingleSiteGallery | Boolean | No | 2 |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteTemplate | WChar (Text) | Yes | 50 |
| SiteTitle | WChar (Text) | Yes | 255 |
| SiteURL | WChar (Text) | Yes | 255 |
| SMTP | WChar (Text) | Yes | 50 |
| StateOrProvince | WChar (Text) | Yes | 20 |
| UseBreadCrumbURL | Boolean | No | 2 |

**CompanySiteParameter** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | No | - |
| CompanySiteParameterID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**CompanySiteTypeParameter** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| CompanySiteTypeParameterID | Integer | No | - |
| ParameterValue | WChar (Text) | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| SiteParameterTypeID | Integer | No | - |
| SortOrder | Integer | Yes | - |

**Contact** (2 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| Address1 | WChar (Text) | Yes | 50 |
| Address2 | WChar (Text) | Yes | 50 |
| Biography | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | No | - |
| Country | WChar (Text) | Yes | 50 |
| CreateDT | Date | Yes | - |
| EMail | WChar (Text) | Yes | 50 |
| email_subscribe | Boolean | No | 2 |
| FirstName | WChar (Text) | Yes | 50 |
| GroupID | Integer | Yes | - |
| HomePhone | WChar (Text) | Yes | 50 |
| LastName | WChar (Text) | Yes | 50 |
| LogonName | WChar (Text) | No | 50 |
| LogonPassword | WChar (Text) | Yes | 50 |
| MiddleInitial | WChar (Text) | Yes | 50 |
| MobilPhone | WChar (Text) | Yes | 50 |
| OfficePhone | WChar (Text) | Yes | 50 |
| Pager | WChar (Text) | Yes | 50 |
| Paid | Integer | Yes | - |
| PostalCode | WChar (Text) | Yes | 50 |
| PrimaryContact | WChar (Text) | Yes | 255 |
| RoleID | Integer | Yes | - |
| State | WChar (Text) | Yes | 50 |
| TemplatePrefix | WChar (Text) | Yes | 50 |
| URL | WChar (Text) | Yes | 50 |

**Group** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| GroupComment | WChar (Text) | Yes | 50 |
| GroupID | Integer | No | - |
| GroupName | WChar (Text) | Yes | 50 |

**Image** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| color | WChar (Text) | Yes | 255 |
| CompanyID | Integer | Yes | - |
| ContactID | Integer | Yes | - |
| ImageComment | WChar (Text) | Yes | - |
| ImageDate | Date | Yes | - |
| ImageDescription | WChar (Text) | Yes | - |
| ImageFileName | WChar (Text) | No | 254 |
| ImageID | Integer | No | - |
| ImageName | WChar (Text) | No | 50 |
| ImageThumbFileName | WChar (Text) | Yes | 254 |
| medium | WChar (Text) | Yes | 50 |
| ModifiedDT | Date | Yes | - |
| price | WChar (Text) | Yes | 255 |
| size | WChar (Text) | Yes | 50 |
| sold | Boolean | No | 2 |
| subject | WChar (Text) | Yes | 255 |
| title | WChar (Text) | Yes | 50 |
| VersionNo | Integer | Yes | - |

**Link** (15 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | No | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| PageID | Integer | Yes | - |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**LinkCategory** (3 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 50 |

**LinkRank** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CateID | Integer | Yes | - |
| Comment | WChar (Text) | Yes | 255 |
| ID | Integer | No | - |
| LinkID | Integer | Yes | - |
| RankNum | Integer | Yes | - |
| UserID | Integer | Yes | - |

**LinkType** (13 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| LinkTypeCD | WChar (Text) | Yes | 50 |
| LinkTypeComment | WChar (Text) | Yes | - |
| LinkTypeDesc | WChar (Text) | Yes | 255 |
| LinkTypeTarget | WChar (Text) | Yes | 50 |

**Message** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Author | WChar (Text) | Yes | 50 |
| Body | WChar (Text) | Yes | - |
| City | WChar (Text) | Yes | 50 |
| Email | WChar (Text) | Yes | 100 |
| MessageDate | Date | Yes | - |
| MessageID | Integer | No | - |
| PageID | Integer | Yes | - |
| ParentMessageID | Integer | Yes | - |
| Subject | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | 50 |

**Page** (4 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Active | Boolean | No | 2 |
| AllowMessage | Boolean | No | 2 |
| CompanyID | Integer | Yes | - |
| GroupID | Integer | Yes | - |
| ImagesPerRow | Integer | Yes | - |
| ModifiedDT | Date | Yes | - |
| PageDescription | WChar (Text) | Yes | 255 |
| PageFileName | WChar (Text) | Yes | 255 |
| PageID | Integer | No | - |
| PageKeywords | WChar (Text) | Yes | 255 |
| PageName | WChar (Text) | No | 50 |
| PageOrder | SmallInt | No | - |
| PageTitle | WChar (Text) | Yes | 255 |
| PageTypeID | Integer | Yes | - |
| ParentPageID | Integer | Yes | - |
| RowsPerPage | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| VersionNo | Integer | Yes | - |

**PageAlias** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| AliasType | WChar (Text) | Yes | 10 |
| CompanyID | Integer | Yes | - |
| PageAliasID | Integer | No | - |
| PageURL | WChar (Text) | Yes | 255 |
| TargetURL | WChar (Text) | Yes | 255 |

**PageImage** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ImageID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageImageID | Integer | No | - |
| PageImagePosition | Integer | Yes | - |

**PageRole** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CompanyID | Integer | Yes | - |
| PageID | Integer | Yes | - |
| PageRoleID | Integer | No | - |
| RoleID | Integer | Yes | - |

**PageType** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| PageFileName | WChar (Text) | Yes | 50 |
| PageTypeCD | WChar (Text) | Yes | 50 |
| PageTypeDesc | WChar (Text) | Yes | 50 |
| PageTypeID | Integer | No | - |

**role** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| FilterMenu | Boolean | No | 2 |
| RoleComment | WChar (Text) | Yes | 50 |
| RoleID | Integer | No | - |
| RoleName | WChar (Text) | Yes | 50 |
| RoleTitle | WChar (Text) | Yes | 50 |

**SiteCategory** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| CategoryDescription | WChar (Text) | Yes | 255 |
| CategoryFileName | WChar (Text) | Yes | 255 |
| CategoryKeywords | WChar (Text) | Yes | 255 |
| CategoryName | WChar (Text) | Yes | 255 |
| CategoryTitle | WChar (Text) | Yes | 255 |
| GroupOrder | Double | Yes | - |
| ParentCategoryID | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | No | - |
| SiteCategoryTypeID | Integer | Yes | - |

**SiteCategoryGroup** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteCategoryGroupDS | WChar (Text) | Yes | 255 |
| SiteCategoryGroupID | Integer | No | - |
| SiteCategoryGroupNM | WChar (Text) | Yes | 255 |
| SiteCategoryGroupOrder | Integer | Yes | - |

**SiteCategoryType** (1 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| DefaultSiteCategoryID | Integer | Yes | - |
| SiteCategoryComment | WChar (Text) | Yes | 255 |
| SiteCategoryFileName | WChar (Text) | Yes | 255 |
| SiteCategoryTransferURL | WChar (Text) | Yes | 255 |
| SiteCategoryTypeDS | WChar (Text) | Yes | 255 |
| SiteCategoryTypeID | Integer | No | - |
| SiteCategoryTypeNM | WChar (Text) | Yes | 255 |

**SiteLink** (0 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| ASIN | WChar (Text) | Yes | 50 |
| CategoryID | Integer | Yes | - |
| CompanyID | Integer | Yes | - |
| DateAdd | Date | Yes | - |
| Description | WChar (Text) | Yes | - |
| ID | Integer | No | - |
| LinkTypeCD | WChar (Text) | Yes | 50 |
| Ranks | Integer | Yes | - |
| SiteCategoryGroupID | Integer | Yes | - |
| SiteCategoryID | Integer | Yes | - |
| SiteCategoryTypeID | Integer | Yes | - |
| Title | WChar (Text) | Yes | 255 |
| URL | WChar (Text) | Yes | - |
| UserID | Integer | Yes | - |
| UserName | WChar (Text) | Yes | 50 |
| Views | Boolean | No | 2 |

**SiteParameterType** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| SiteParameterTemplate | WChar (Text) | Yes | - |
| SiteParameterTypeDS | WChar (Text) | Yes | 255 |
| SiteParameterTypeID | Integer | No | - |
| SiteParameterTypeNM | WChar (Text) | Yes | 255 |
| SiteParameterTypeOrder | Integer | Yes | - |

**SiteTemplate** (6 rows)

| Column | Type | Nullable | MaxLen |
|--------|------|----------|--------|
| Bottom | WChar (Text) | Yes | - |
| CSS | WChar (Text) | Yes | - |
| Name | WChar (Text) | No | 50 |
| TemplatePrefix | WChar (Text) | Yes | 10 |
| Top | WChar (Text) | Yes | - |

</details>

### Schema Comparison: Table Presence Across Databases

| Table | 1-MineralCollection | DramaEducator | FrogsFolly-Family | FrogsFolly-Travel | FrogsFolly | FrogsFollyBase | FrogsFollyKids | InformationCenter | MARKInformationCenter | MineralCollection | osmcinc | ProjectMechanics | wpm-demo | wpmFrogsFolly | wpmLiving | wpmMOM |
|-------|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| Article | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Company | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| CompanySiteParameter | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| CompanySiteTypeParameter | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Contact | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Group | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Image | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Link | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| LinkCategory | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| LinkRank | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | - | Y | Y | Y |
| LinkType | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| Message | Y | - | - | - | - | - | - | - | - | Y | Y | Y | - | - | - | Y |
| Page | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| PageAlias | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| PageImage | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| PageRole | Y | Y | - | Y | Y | - | Y | Y | Y | - | Y | - | - | Y | Y | Y |
| PageType | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| role | Y | Y | - | Y | Y | - | Y | Y | Y | - | Y | - | - | Y | Y | Y |
| SiteCategory | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| SiteCategoryGroup | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| SiteCategoryType | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| SiteLink | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| SiteParameterType | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |
| sites | - | - | - | - | - | - | - | Y | Y | - | - | - | - | - | - | - |
| SiteTemplate | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y | Y |

---

## 3. CompanyID → Data Volume

| MDB File | CompanyID | Domain | Pages | Articles | Images | Aliases | Parts | Params |
|----------|----------|--------|-------|----------|--------|---------|-------|--------|
| FrogsFolly-Family.mdb | 1 | family.frogsfolly.com | 91 | 22 | 3064 | 11 | 0 | 0 |
| FrogsFolly-Travel.mdb | 1 | travel.frogsfolly.com | 121 | 87 | 1804 | 13 | 0 | 0 |
| FrogsFolly-Travel.mdb | 1 | travel.frogsfolly.com | 121 | 87 | 1804 | 13 | 0 | 0 |
| FrogsFolly.mdb | 21 | dramaeducator.com | 19 | 25 | 0 | 0 | 0 | 0 |
| FrogsFolly.mdb | 29 | MacCloskeyAndMyers.com | 7 | 7 | 25 | 0 | 0 | 0 |
| FrogsFolly.mdb | 32 | TexEcon.com | 2 | 2 | 0 | 0 | 0 | 0 |
| FrogsFolly.mdb | 37 | houstonlakeshore.com | 3 | 4 | 14 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 36 | marlis.frogsfolly.com | 1 | 1 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 37 | ian.frogsfolly.com | 5 | 12 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 39 | berit.frogsfolly.com | 2 | 8 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 40 | jordan.frogsfolly.com | 2 | 1 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 41 | lauren.frogsfolly.com | 3 | 1 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 42 | sarah.frogsfolly.com | 2 | 1 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 43 | studentcouncil.frogsfolly.com | 0 | 1 | 0 | 0 | 0 | 0 |
| FrogsFollyKids.mdb | 44 | mateus.frogsfolly.com | 1 | 2 | 0 | 0 | 0 | 0 |
| MineralCollection.mdb | 29 | jmshawminerals.com | 8 | 6 | 2 | 0 | 0 | 0 |
| MineralCollection.mdb | 30 | nrc.controlorigins.com | 1 | 0 | 0 | 1 | 0 | 0 |
| ProjectMechanics.mdb | 8 | pm.controlorigins.com | 13 | 78 | 32 | 85 | 0 | 0 |
| ProjectMechanics.mdb | 13 | controlorigins.com | 17 | 30 | 8 | 7 | 0 | 0 |
| ProjectMechanics.mdb | 17 | wpm.markhazleton.com | 11 | 9 | 3 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 26 | dearinggroup.com | 0 | 2 | 0 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 29 | osmcinc.projectmechanics.com | 3 | 1 | 0 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 30 | originscorp.com | 1 | 1 | 0 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 31 | mc.controlorigins.com | 2 | 2 | 0 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 32 | clients.controlorigins.com | 0 | 0 | 0 | 0 | 0 | 0 |
| ProjectMechanics.mdb | 33 | blog.controlorigins.com | 1 | 5 | 0 | 7 | 0 | 0 |
| wpm-demo.mdb | 31 | webprojectmechanics.com | 4 | 4 | 3 | 0 | 0 | 0 |
| wpmFrogsFolly.mdb | 1 | frogsfolly.com | 205 | 166 | 4445 | 11 | 0 | 0 |
| wpmLiving.mdb | 23 | LakeClaiborneHouse.com | 9 | 24 | 36 | 0 | 0 | 0 |
| wpmLiving.mdb | 25 | inkslakeliving.com | 12 | 11 | 41 | 0 | 0 | 0 |
| wpmLiving.mdb | 26 | burnetcountyliving.com | 0 | 1 | 0 | 0 | 0 | 0 |
| wpmLiving.mdb | 28 | kellertexasliving.com | 12 | 2 | 36 | 0 | 0 | 0 |
| wpmLiving.mdb | 35 | lakebuchananliving.com | 1 | 1 | 0 | 0 | 0 | 0 |
| wpmLiving.mdb | 36 | GraysonGeorgiaLiving.com | 7 | 8 | 21 | 0 | 0 | 0 |
| wpmMOM.mdb | 18 | mechanicsofmotherhood.com | 4 | 11 | 0 | 1 | 0 | 0 |
| wpmMOM.mdb | 19 | holdcompany.com | 0 | 0 | 0 | 0 | 0 | 0 |

### Company Table Details

| MDB File | CompanyID | CompanyName | SiteURL | GalleryFolder |
|----------|----------|-------------|---------|---------------|
| FrogsFolly-Family.mdb | 1 | Frog's Folly Family | http://family.frogsfolly.com | /sites/frogsfolly/ |
| FrogsFolly-Travel.mdb | 1 | Frog's Folly Travel | http://travel.frogsfolly.com | /sites/frogsfolly/ |
| FrogsFolly.mdb | 21 | DramaEducator.com | www.dramaeducator.com | /sites/dramaeducator/ |
| FrogsFolly.mdb | 25 | Inks Lake Living | http://inkslakeliving.com | /sites/InksLakeLiving/ |
| FrogsFolly.mdb | 29 | MacCloskey and Myers | MacCloskeyAndMyers.com | /sites/MacCloskeyAndMyers/ |
| FrogsFolly.mdb | 32 | TexEcon.com | www.TexEcon.com | /sites/TexEcon/ |
| FrogsFolly.mdb | 37 | Houston Lakeshore | http://houstonlakeshore.com | /sites/houstonlakeshore/ |
| FrogsFollyKids.mdb | 36 | Marlis | http://marlis.frogsfolly.com | /sites/marlis |
| FrogsFollyKids.mdb | 37 | Ian | ian.frogsfolly.com | /sites/ian |
| FrogsFollyKids.mdb | 39 | Berit | berit.frogsfolly.com | /sites/berit |
| FrogsFollyKids.mdb | 40 | Jordan | jordan.frogsfolly.com | /sites/jordan  |
| FrogsFollyKids.mdb | 41 | Lauren | lauren.frogsfolly.com | /sites/lauren |
| FrogsFollyKids.mdb | 42 | Sarah | sarah.frogsfolly.com | /sites/sarah |
| FrogsFollyKids.mdb | 43 | StudentCouncil | studentcouncil.frogsfolly.com | /sites/marlis/ |
| FrogsFollyKids.mdb | 44 | Mateus | http://mateus.frogsfolly.com | /sites/mateus |
| MineralCollection.mdb | 29 | J.M. Shaw Mineral Collection | http://jmshawminerals.com | /sites/nrc |
| MineralCollection.mdb | 30 | nrc.controlorigins.com | http://nrc.controlorigins.com | /sites/nrc/ |
| ProjectMechanics.mdb | 8 | Project Mechanics | http://pm.controlorigins.com | /sites/ProjectMechanics/ |
| ProjectMechanics.mdb | 13 | Control Origins | http://controlorigins.com | /sites/ControlOrigins/ |
| ProjectMechanics.mdb | 17 | Web Project Mechanics | http://wpm.markhazleton.com | /sites/webprojectmechanics/ |
| ProjectMechanics.mdb | 26 | The Dearing Group | http://dearinggroup.com | /sites/DearingGroup/ |
| ProjectMechanics.mdb | 29 | One Source Materials and Construction, Inc. | http://osmcinc.projectmechanics.com | /sites/osmcinc |
| ProjectMechanics.mdb | 30 | Orgins Corp | http://originscorp.com | /sites/ControlOrigins/ |
| ProjectMechanics.mdb | 31 | Hazleton MineCraft | http://mc.controlorigins.com | /sites/HazletonMC/ |
| ProjectMechanics.mdb | 32 | clients.controlorigins.com | http://clients.controlorigins.com | /sites/ControlOrigins |
| ProjectMechanics.mdb | 33 | Sidetracked By Sizzle | http://blog.controlorigins.com | /sites/ProjectMechanics/ |
| wpm-demo.mdb | 31 | WebProjectMechanics.com | http://webprojectmechanics.com | /sites/WebProjectMechanics/ |
| wpmFrogsFolly.mdb | 1 | The Frog's Folly | www.frogsfolly.com | /sites/frogsfolly/ |
| wpmLiving.mdb | 23 | Lake Claiborne House | http://LakeClaiborneHouse.com | /sites/LakeClaiborneHouse/ |
| wpmLiving.mdb | 25 | Inks Lake Living | http://inkslakeliving.com | /sites/InksLakeLiving/ |
| wpmLiving.mdb | 26 | BurnetCountyLiving.com | http://burnetcountyliving.com | /sites/BurnetCountyLiving/ |
| wpmLiving.mdb | 28 | KellerTexasLiving.com | http://kellertexasliving.com | /sites/InksLakeLiving/ |
| wpmLiving.mdb | 35 | LakeBuchananliving.com | http://lakebuchananliving.com | /sites/lakebuchananliving |
| wpmLiving.mdb | 36 | GraysonGeorgiaLiving.com | http://GraysonGeorgiaLiving.com | /sites/GraysonGeorgiaLiving/ |
| wpmMOM.mdb | 18 | Mechanics of Motherhood | http://mechanicsofmotherhood.com | /sites/MechanicsOfMotherhood/ |
| wpmMOM.mdb | 19 | HoldCompany | http://http://holdcompany.com |  |

---

## 4. PageAlias Inventory (URL Redirects)

**Total PageAlias entries across all databases:** 137

### Sample Aliases (first 20)

| MDB File | CompanyID | AliasPath | PageID |
|----------|----------|-----------|--------|

---

## 5. Image Inventory

**Total Image records across all databases:** 0

| Domain | CompanyID | Image Records | GalleryFolder |
|--------|----------|---------------|---------------|
| berit.frogsfolly.com | 39 | 0 | /sites/berit |
| blog.controlorigins.com | 33 | 0 | /sites/ProjectMechanics/ |
| burnetcountyliving.com | 26 | 0 | /sites/BurnetCountyLiving/ |
| clients.controlorigins.com | 32 | 0 | /sites/ControlOrigins |
| controlorigins.com | 13 | 0 | /sites/ControlOrigins/ |
| dearinggroup.com | 26 | 0 | /sites/DearingGroup/ |
| dramaeducator.com | 21 | 0 | /sites/dramaeducator/ |
| family.frogsfolly.com | 1 | 0 | /sites/frogsfolly/ |
| frogsfolly.com | 1 | 0 | /sites/frogsfolly/ |
| GraysonGeorgiaLiving.com | 36 | 0 | /sites/GraysonGeorgiaLiving/ |
| holdcompany.com | 19 | 0 |  |
| houstonlakeshore.com | 37 | 0 | /sites/houstonlakeshore/ |
| ian.frogsfolly.com | 37 | 0 | /sites/ian |
| inkslakeliving.com | 25 | 0 | /sites/InksLakeLiving/ |
| jmshawminerals.com | 29 | 0 | /sites/nrc |
| jordan.frogsfolly.com | 40 | 0 | /sites/jordan  |
| kellertexasliving.com | 28 | 0 | /sites/InksLakeLiving/ |
| lakebuchananliving.com | 35 | 0 | /sites/lakebuchananliving |
| LakeClaiborneHouse.com | 23 | 0 | /sites/LakeClaiborneHouse/ |
| lauren.frogsfolly.com | 41 | 0 | /sites/lauren |
| MacCloskeyAndMyers.com | 29 | 0 | /sites/MacCloskeyAndMyers/ |
| marlis.frogsfolly.com | 36 | 0 | /sites/marlis |
| mateus.frogsfolly.com | 44 | 0 | /sites/mateus |
| mc.controlorigins.com | 31 | 0 | /sites/HazletonMC/ |
| mechanicsofmotherhood.com | 18 | 0 | /sites/MechanicsOfMotherhood/ |
| nrc.controlorigins.com | 30 | 0 | /sites/nrc/ |
| originscorp.com | 30 | 0 | /sites/ControlOrigins/ |
| osmcinc.projectmechanics.com | 29 | 0 | /sites/osmcinc |
| pm.controlorigins.com | 8 | 0 | /sites/ProjectMechanics/ |
| sarah.frogsfolly.com | 42 | 0 | /sites/sarah |
| studentcouncil.frogsfolly.com | 43 | 0 | /sites/marlis/ |
| TexEcon.com | 32 | 0 | /sites/TexEcon/ |
| travel.frogsfolly.com | 1 | 0 | /sites/frogsfolly/ |
| travel.frogsfolly.com | 1 | 0 | /sites/frogsfolly/ |
| webprojectmechanics.com | 31 | 0 | /sites/WebProjectMechanics/ |
| wpm.markhazleton.com | 17 | 0 | /sites/webprojectmechanics/ |

---

## 6. Orphan CompanyID Detection

CompanyIDs found in Page table but NOT mapped to any XML config:

| MDB File | CompanyID | CompanyName | Page Count |
|----------|----------|-------------|------------|
| FrogsFolly.mdb | 25 | Inks Lake Living | 11 |
| ProjectMechanics.mdb | 25 | ? | 1 |

---

## 7. Summary Statistics

| Metric | Value |
|--------|-------|
| Total domains (from XML configs) | 36 |
| Total .mdb files | 16 |
| Referenced .mdb files | 10 |
| Unreferenced .mdb files | 6 |
| Total Page rows | 582 |
| Total Article rows | 548 |
| Total Image rows | 0 |
| Total PageAlias rows | 137 |
| Total Part rows | 0 |
| Total Parameter rows | 0 |

### Unreferenced Databases (candidates to skip during migration)

- **1-MineralCollection.mdb** — Not referenced by any XML site config. Likely a backup or template.
- **DramaEducator.mdb** — Not referenced by any XML site config. Likely a backup or template.
- **FrogsFollyBase.mdb** — Not referenced by any XML site config. Likely a backup or template.
- **InformationCenter.mdb** — Not referenced by any XML site config. Likely a backup or template.
- **MARKInformationCenter.mdb** — Not referenced by any XML site config. Likely a backup or template.
- **osmcinc.mdb** — Not referenced by any XML site config. Likely a backup or template.

