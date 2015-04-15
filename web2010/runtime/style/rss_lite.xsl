<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
<xsl:template match="*">
<div class="leftnav">
<strong><xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/></strong>
<div class="blog-posts">
  <xsl:for-each select="//*[local-name()='item']">
  <div class="blog-title">
    <a >
      <xsl:attribute name="href">
      <xsl:value-of select="*[local-name()='link']"/>
      </xsl:attribute>
      <xsl:attribute name="target">
      <xsl:text>top</xsl:text>
      </xsl:attribute>
      <xsl:value-of select="*[local-name()='title']"/>
    </a>
</div>
<div class="blog-body">
      <xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
</div>
  </xsl:for-each>
</div>
</div>
</xsl:template>
<xsl:template match="/">
<xsl:apply-templates/>
</xsl:template>
</xsl:stylesheet>


<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\App_Data\XML\Mark Hazleton del.icio.us RSS List-8-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->