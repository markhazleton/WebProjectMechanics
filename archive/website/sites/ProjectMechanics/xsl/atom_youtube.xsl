<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:feedburner="http://rssnamespace.org/feedburner/ext/1.0">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no"/>
  <xsl:template match="atom:feed">
    <h3><xsl:value-of select="atom:title"/></h3>
      <xsl:apply-templates select="atom:entry"/>
  </xsl:template>
  <xsl:template match="atom:entry">
    <xsl:if test="position() &lt;11">
      <div class="blog-entry">
        <div class="blog-body">
          <xsl:call-template name="outputContent"/>
        </div>
      </div>
    </xsl:if>
  </xsl:template>
  <xsl:template name="outputContent">
    <embed>
      <xsl:attribute name="width">100%</xsl:attribute>
      <xsl:attribute name="height">100%</xsl:attribute>
      <xsl:attribute name="src"><xsl:value-of select="*[local-name()='group']/*[local-name()='content']/@url"/></xsl:attribute>
      <xsl:attribute name="type"><xsl:value-of select="*[local-name()='group']/*[local-name()='content']/@type"/></xsl:attribute>
    </embed>
    
    <blockquote>
      <xsl:value-of select="*[local-name()='group']/*[local-name()='description']"/>
    </blockquote>
 
  </xsl:template>
  
  
  
  
  
  
</xsl:stylesheet>