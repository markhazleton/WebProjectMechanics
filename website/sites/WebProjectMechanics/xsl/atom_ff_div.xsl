<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:feedburner="http://rssnamespace.org/feedburner/ext/1.0">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no"/>
  <xsl:template match="atom:feed">
    <div class="rightnav">
<xsl:for-each select="atom:link">
<xsl:value-of select="@title"/>  - 
<xsl:value-of select="@href"/><br/>
</xsl:for-each>

	<img><xsl:attribute name="src">
	  <xsl:value-of select="atom:icon"/></xsl:attribute></img>
      <strong><xsl:value-of select="atom:title"/></strong>
      <xsl:apply-templates select="atom:entry"/>
    </div>
  </xsl:template>

  <xsl:template match="atom:entry">
    <xsl:if test="position() &lt;101">
      <div class="blog-entry">
	  
        <!--<div class="blog-name">
          <xsl:choose>
            <xsl:when test="atom:link[@rel='alternate' or not(@rel)]">
              <a href="{normalize-space(atom:link[@rel='alternate' or not(@rel)]/@href)}">
                <xsl:call-template name="outputTitle"/>
              </a>
            </xsl:when>
            <xsl:when test="atom:content[@src]">
              <a href="{normalize-space(atom:content/@src)}">
                <xsl:call-template name="outputTitle"/>
              </a>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="outputTitle"/>
            </xsl:otherwise>
          </xsl:choose>
        </div>-->
        <div class="blog-body">
          <xsl:call-template name="outputContent"/>
        </div>
      </div>
	  <hr/>
    </xsl:if>
  </xsl:template>
  <xsl:template name="outputTitle">
    <xsl:choose>
      <xsl:when test="atom:title[@type='xhtml']">
        <xsl:copy-of select="atom:title[@type='xhtml']/xhtml:div/child::node()"/>
      </xsl:when>
      <xsl:when test="atom:title[@type='html']">
        <xsl:attribute name="name">decodeable</xsl:attribute>
        <xsl:value-of select="atom:title" disable-output-escaping="yes"/>
      </xsl:when>
      <xsl:when test="atom:title[@type='text' or not(@type)]">
        <xsl:value-of select="atom:title"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="outputContent">
    <xsl:choose>
      <xsl:when test="atom:content[@type='xhtml']">
        <xsl:copy-of select="atom:content[@type='xhtml']/xhtml:div/child::node()"/>
      </xsl:when>
      <xsl:when test="atom:content[@type='html']">
        <xsl:attribute name="name">decodeable</xsl:attribute>
        <xsl:value-of select="atom:content" disable-output-escaping="yes"/>
      </xsl:when>
      <xsl:when test="atom:content[@type='text' or not(@type)]">
        <xsl:value-of select="atom:content"/>
      </xsl:when>
      <xsl:when test="atom:summary[@type='xhtml']">
        <xsl:copy-of select="atom:summary[@type='xhtml']/xhtml:div/child::node()"/>
      </xsl:when>
      <xsl:when test="atom:summary[@type='html']">
        <xsl:attribute name="name">decodeable</xsl:attribute>
        <xsl:value-of select="atom:summary" disable-output-escaping="yes"/>
      </xsl:when>
      <xsl:when test="atom:summary[@type='text' or not(@type)]">
        <xsl:value-of select="atom:summary"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="http://friendfeed.com/markhazleton?format=atom" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->