<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="*">
    <div class="box">
<h2><a>
        <xsl:attribute name="href">
          <xsl:value-of select="*[local-name()='channel']/*[local-name()='link']"/>
        </xsl:attribute>
        <xsl:attribute name="alt">
          <xsl:text>top</xsl:text>
        </xsl:attribute>
        <xsl:attribute name="target">
          <xsl:text>top</xsl:text>
        </xsl:attribute>
        <xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
      </a></h2>
      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
      <xsl:value-of select="*[local-name()='channel']/*[local-name()='lastBuildDate']"/>
    <xsl:for-each select="//*[local-name()='item']">
    <div>
        <xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
        </div>
    </xsl:for-each>
    </div>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="TEST" userelativepaths="yes" externalpreview="no" url="..\XML\Keller Texas Local News-28-XML.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="bWarnings" value="true"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no" ><SourceSchema srcSchemaPath="..\XML\Keller Texas Local News-28-XML.xml" srcSchemaRoot="rss" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/></MapperInfo><MapperBlockPosition><template match="*"><block path="div/a/xsl:attribute/xsl:value-of" x="570" y="131"/><block path="div/a/xsl:value-of" x="531" y="54"/><block path="div/xsl:value-of" x="531" y="18"/><block path="div[1]/xsl:for-each/div/div/a/xsl:attribute/xsl:value-of" x="438" y="214"/><block path="div[1]/xsl:for-each/div/div/a/xsl:value-of" x="450" y="136"/><block path="div[1]/xsl:for-each/div/div[1]/xsl:value-of" x="496" y="274"/></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->