<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:ht="http://www.google.com/trends/hottrends">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no"/>
  <xsl:template match="atom:feed">
    <div class="MainContent">
      <strong>
        <xsl:value-of select="atom:title"/>
      </strong>
      <br/>
      <blockquote>
        <xsl:value-of select="atom:subtitle"/>
      </blockquote>

      <ul>
        <xsl:for-each select="atom:entry">

          <xsl:if test="atom:content/@type='html'">
            <li>
              <xsl:value-of select="atom:content" disable-output-escaping="yes"/>
            </li>
          </xsl:if>
        </xsl:for-each>
      </ul>
      <ol>
        <xsl:for-each select="atom:entry/atom:content/ht:root/ht:entry">
          <li><h1>
          <a>
          <xsl:attribute name="href"><xsl:value-of select="ht:source_url" /></xsl:attribute>
          <xsl:value-of select="ht:pos"/>  - <xsl:value-of select="ht:title" disable-output-escaping="yes"/>
          </a>
          </h1>
            <xsl:if test="ht:image/@url!=''">
              <img>
                <xsl:attribute name="hspace">3</xsl:attribute>
                <xsl:attribute name="vspace">3</xsl:attribute>
                <xsl:attribute name="src">
                  <xsl:value-of select="ht:image/@url"/>
                </xsl:attribute>
                <xsl:attribute name="alt">
                  <xsl:value-of select="ht:source" disable-output-escaping="no"/>
                </xsl:attribute>
                <xsl:attribute name="height">
                  <xsl:value-of select="ht:image/@height"/>
                </xsl:attribute>
                <xsl:attribute name="width">
                  <xsl:value-of select="ht:image/@width"/>
                </xsl:attribute>
                <xsl:attribute name="align">left</xsl:attribute>
              </img>
            </xsl:if>
              <xsl:value-of select="ht:snippet" disable-output-escaping="yes"/><br/><br/>
          </li>
        </xsl:for-each>
      </ol>
    </div>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\Google Trends-1-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->