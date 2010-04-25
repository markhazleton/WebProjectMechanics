<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <table border="1">
        <tr>
          <td>
            RecordSource
          </td>
          <td>
            PageID
          </td>
          <td>
            ParentPageID
          </td>
          <td>Level NBR</td>
          <td>
            PageName
          </td>
          <td>
            SiteCategoryID
          </td>
          <td>
            SiteCategoryName
          </td>
          <td>
            SiteCategoryGroupName
          </td>
        </tr>
      <xsl:for-each select="wpmSiteProfile/LocationList/wpmLocation">
      <xsl:sort select="SiteCategoryName"/>
      <xsl:sort select="ParentPageID"/>
      <xsl:sort select="PageID"/>
        <tr>
          <td >
            <xsl:value-of select="RecordSource"/> 
          </td>
          <td>
            <xsl:value-of select="PageID"/>
          </td>
          <td>
            <xsl:value-of select="ParentPageID"/><br/> 
          </td>
          <td>
            <xsl:value-of select="LevelNBR"/>
          </td>
          <td>
            <xsl:value-of select="PageName"/><br/>
          </td>
          <td>
            <xsl:value-of select="SiteCategoryID"/><br/>
          </td>
          <td>
            <xsl:value-of select="SiteCategoryName"/><br/>
          </td>
          <td>
            <xsl:value-of select="SiteCategoryGroupName"/><br/>
          </td>

        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="yes" url="..\..\access_db\index\The Frog's Folly-site-file.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->