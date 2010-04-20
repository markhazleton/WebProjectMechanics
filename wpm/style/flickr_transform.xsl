<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:dc="http://purl.org/dc/elements/1.1/">
  <xsl:output method="html" encoding="utf-8"/>

  <xsl:template match="/">
    <xsl:apply-templates select="/atom:feed"/>
  </xsl:template>

  <xsl:template match="/atom:feed">
    <div id="content">
      <h1>
        <xsl:value-of select="atom:title"/>
      </h1>
      <p>
        <xsl:value-of select="atom:subtitle"/>
      </p>

      <ul id="entries">
        <xsl:apply-templates select="atom:entry"/>
      </ul>
    </div>
  </xsl:template>


  <xsl:template match="atom:icon">
    <xsl:element name="img">
      <xsl:attribute name="src">
        <xsl:value-of select="atom:icon"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>

  <xsl:template match="atom:entry">
    <li class="entry">
      <h2>
        <a href="{atom:link/@href}">
          <xsl:value-of select="atom:title"/>
        </a>
      </h2>
      <p class="date">(<xsl:value-of select="substring-before(atom:updated,'T')"/>)</p>
      <xsl:value-of select="atom:content" disable-output-escaping="yes"/>
      <br/>
      <xsl:apply-templates select="atom:category"/>
    </li>
  </xsl:template>

  <xsl:template match="atom:category">
    <xsl:for-each select=".">
      <xsl:element name="a">
        <xsl:attribute name="rel">
          <xsl:text>tag</xsl:text>
        </xsl:attribute>
        <xsl:attribute name="href">
          <xsl:value-of select="concat(@scheme, @term)"/>
        </xsl:attribute>
        <xsl:value-of select="@term"/>
      </xsl:element>
      <xsl:text> </xsl:text>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\XML\Jonathan Flickr RSS Feed-8-XML.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->