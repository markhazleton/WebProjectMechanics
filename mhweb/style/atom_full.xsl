<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:ht="http://www.google.com/trends/hottrends">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no"/>
  <xsl:template match="atom:feed">
  <div>
    <strong><xsl:value-of select="atom:title"/></strong><br/>
    <blockquote><xsl:value-of select="atom:subtitle"/></blockquote>

<ul>
<xsl:for-each select="atom:entry">

<xsl:if test="atom:content/@type='html'">
  <li>
<xsl:value-of select="atom:content" disable-output-escaping="yes"/>
  </li>
</xsl:if>
</xsl:for-each>

<xsl:for-each select="atom:entry/atom:content/ht:root/ht:entry">
<xsl:if test="ht:image/@url!=''">
<li>
<img>
<xsl:attribute name="src"><xsl:value-of select="ht:image/@url"/></xsl:attribute>
<xsl:attribute name="alt"><xsl:value-of select="ht:source" disable-output-escaping="no"/></xsl:attribute>
<xsl:attribute name="height"><xsl:value-of select="ht:image/@height"/></xsl:attribute>
<xsl:attribute name="width"><xsl:value-of select="ht:image/@width"/></xsl:attribute>
<xsl:attribute name="align">left</xsl:attribute>
</img>
<blockquote>
<strong><xsl:value-of select="ht:title" disable-output-escaping="yes"/></strong><br/>
<xsl:value-of select="ht:snippet" disable-output-escaping="yes"/>

</blockquote>
</li>
</xsl:if>


</xsl:for-each>
</ul>


  </div>
  </xsl:template>
  <xsl:template match="atom:entry">
    <xsl:if test="position() &lt;6">
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
      </xsl:if>
<xsl:value-of select="atom:content" disable-output-escaping="yes"/>
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
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\XML\Electoral-Vote RSS-8-XML.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->