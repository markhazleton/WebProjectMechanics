<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Site Navigation</h1>

    <table border="1">
      <tr>
        <td>RecordSource</td>
        <td>Level</td>
        <td>PageName</td>
        <td>Page Keywords</td>
        <td>Page Description</td>
      </tr>
      <xsl:for-each select="mhSiteFile/SiteMapRows/mhSiteMapRow">
        <xsl:sort select="PageName"/>
        <tr>
          <td>
            <xsl:value-of select="RecordSource"/>
          </td>
          <td>
            <xsl:value-of select="LevelNBR"/>
          </td>
          <td>
            <xsl:if test="RecordSource='Page'">
              <a>
                <xsl:attribute name="href">/aspmaker/page_edit.aspx?PageID=<xsl:value-of select="PageID"/></xsl:attribute>
                <xsl:value-of select="PageName"/>
              </a>
            </xsl:if>
            <xsl:if test="RecordSource='Category'">
              <a>
                <xsl:attribute name="href">/aspmaker/sitecategory_edit.aspx?SiteCategoryID=<xsl:value-of select="SiteCategoryID"/></xsl:attribute>
                <xsl:value-of select="PageName"/>
              </a>
            </xsl:if>
          </td>
          <td>
            <xsl:value-of select="PageKeywords"/>
            <br/>
          </td>
          <td>
            <xsl:value-of select="PageDescription"/>
            <br/>
          </td>
        </tr>
      </xsl:for-each>
    </table>

    <h1>Site Links</h1>
    <table border="5">
      <xsl:for-each select="mhSiteFile/SiteLinkRows/mhSiteLinkRow">
        <tr>
          <td valign="top" style="white-space: nowrap;">
            <h2>
              <a>
                <xsl:attribute name="href">/aspmaker/sitelink_edit.aspx?ID=<xsl:value-of select="LinkID"/></xsl:attribute>
                <xsl:value-of select="LinkTitle"/>
              </a>
            </h2>
            <xsl:value-of select="PageID"/><br/>

            <p>
              <xsl:value-of select="LinkDescription"/>
            </p>
          </td>
          <td valign="top" style="white-space: nowrap;"><xsl:value-of select="LinkCategoryTitle"/></td>
          <td valign="top" style="white-space: nowrap;"><xsl:value-of select="LinkTypeCD"/></td>
          <td>
            <pre>
              <xsl:value-of select="LinkURL" disable-output-escaping="no"/>
            </pre>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\index\DallasInformationCenter.com-site-file.xml" htmlbaseurl="" outputurl="..\index\DallasInformationCenter.com-site-file.html" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->