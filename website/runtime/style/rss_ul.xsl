<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
<xsl:template match="*">
<div class="rss_ul">
<h2>
<a>
<xsl:attribute name="href">
<xsl:value-of select="link"/>
</xsl:attribute>

<xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
</a></h2>
<ul>
  <xsl:for-each select="//*[local-name()='item']">
  <li>
    <a >
      <xsl:attribute name="href">
      <xsl:value-of select="*[local-name()='link']"/>
      </xsl:attribute>
      <xsl:attribute name="target">
      <xsl:text>top</xsl:text>
      </xsl:attribute>
      <xsl:value-of select="*[local-name()='title']"/>
    </a>
</li>
  </xsl:for-each>
</ul>
</div>
</xsl:template>
<xsl:template match="/">
<xsl:apply-templates/>
</xsl:template>

</xsl:stylesheet>


<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\App_Data\XML\ElectoralVote RSS-8-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->