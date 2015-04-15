<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="*">
    <br/>
    <br/>
    <div class="MainContent">
      <h2>
        <xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
      </h2>
      <div class="blog-posts">
        <xsl:for-each select="//*[local-name()='item']">
          <div class="blog-title">
            <p>
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="*[local-name()='link']"/>
                </xsl:attribute>
                <xsl:attribute name="target">
                  <xsl:text>_new</xsl:text>
                </xsl:attribute>
                <xsl:value-of select="*[local-name()='title']"/>
              </a>
              <br/>
              <xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
            </p>
          </div>
        </xsl:for-each>
      </div>
    </div>
    <br/>
    <br/>
  </xsl:template>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\Funny Quotes-1-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->