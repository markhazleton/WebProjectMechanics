<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="*">
    <div class="MainContent">
      <h1 style="font-size:20px;text-align:center;border-bottom:3px solid #ff9900;margin-bottom:5px;"><a href="http://markhazleton.myplaxo.com/" target="_blank" title="My Plaxo Pulse">My Plaxo Pulse</a></h1>
      <div class="blog-posts">
        <xsl:for-each select="//*[local-name()='item']">
          <div class="blog-title">
            <xsl:value-of select="*[local-name()='title']" disable-output-escaping="yes"/>
          </div>
          <small><xsl:value-of select="*[local-name()='pubDate']" disable-output-escaping="no" /></small><br/>
          <div class="blog-body">
            <xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
          </div>
          <hr/>
        </xsl:for-each>
      </div>
    </div>
    <br/>
  </xsl:template>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\Mark Hazleton Pulse-1-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->