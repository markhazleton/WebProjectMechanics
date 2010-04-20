<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Site Links</h1>
    <table class="sortable autostripe">
      <thead>
        <tr>
          <th>Link Information</th>
          <th>Category</th>
          <th>Type</th>
          <th>URL</th>
        </tr>
      </thead>
      <xsl:for-each select="wpmSiteProfile/PartList/wpmPart">
        <tr>
          <td>
              <xsl:if test="LinkSource='SiteLink'">
                <a>
                  <xsl:attribute name="href">/wpmgen/sitelink_edit.aspx?ID=<xsl:value-of select="LinkID"/></xsl:attribute>
                  <xsl:value-of select="LinkTitle"/>
                </a>
              </xsl:if>
              <xsl:if test="LinkSource='Link'">
                <a>
                  <xsl:attribute name="href">/wpmgen/link_edit.aspx?ID=<xsl:value-of select="LinkID"/></xsl:attribute>
                  <xsl:value-of select="LinkTitle"/>
                </a>
              </xsl:if>
            <br/>Page - <xsl:value-of select="PageID"/><br/>
            Source - <xsl:value-of select="LinkSource"/><br/>
            <p>
              <xsl:value-of select="LinkDescription"/>
            </p>
          </td>
          <td>
            <xsl:value-of select="LinkCategoryTitle"/>
          </td>
          <td>
            <xsl:value-of select="LinkTypeCD"/>
          </td>
          <td>
            <xsl:value-of select="LinkURL" disable-output-escaping="no"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\index\The Frog's Folly-site-file.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->