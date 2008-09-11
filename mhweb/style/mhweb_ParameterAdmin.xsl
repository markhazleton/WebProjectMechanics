<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Site Parameter</h1>
    <table class="sortable autostripe ">
      <thead>
        <tr>
          <th>CompanyID</th>
          <th>Sort Order</th>
          <th>Type ID</th>
          <th>Type Name</th>
          <th>Type Description</th>
          <th>Parameter Value</th>
        </tr>
      </thead>
      <xsl:for-each select="mhSiteFile/SiteParameterList/mhSiteParameter">
        <xsl:sort select="CompanyID" order="descending"/>
        <xsl:sort select="SortOrder"/>
        <tr>
          <td class="left">
            <xsl:value-of select="CompanyID"/>
          </td>
          <td class="numric">
            <xsl:value-of select="SortOrder"/>
          </td>
          <td class="left">
            <xsl:value-of select="SiteParameterTypeID"/>
          </td>
          <td class="left">
            <xsl:value-of select="SiteParameterTypeNM"/>
          </td>
          <td class="left">
            <xsl:value-of select="SiteParameterTypeDS"/>
          </td>
          <td class="left">
            <pre>
              <xsl:value-of select="ParameterValue"/>
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