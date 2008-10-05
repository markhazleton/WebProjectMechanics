<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Page Alias Setup</h1>
    <table class="sortable autostripe ">
      <thead>
        <tr>
          <th></th>
          <th>Page URL</th>
          <th>Transfer URL</th>
        </tr>
      </thead>
      <xsl:for-each select="mhSiteFile/PageAliasRows/mhPageAlias">
        <xsl:sort select="PageURL"/>
        <tr>
          <td><a>
          
          <xsl:attribute name="href" >
          /aspmaker/pagealias_edit.aspx?PageAliasID=<xsl:value-of select="PageAliasID" />
          </xsl:attribute>
          EDIT
          </a></td>
          <td class="left"><a>
          <xsl:attribute name="href">
          <xsl:value-of select="PageURL" />
          </xsl:attribute>
          <xsl:attribute name="target">_blank</xsl:attribute>
          <xsl:value-of select="PageURL" />
          </a></td>
          <td class="left"><xsl:value-of select="TransferURL" /></td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\index\ACoolerHouseDallas-site-file.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->