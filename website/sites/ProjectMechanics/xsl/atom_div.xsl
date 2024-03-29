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
        <div class="blog-name">
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
        </div>
        <div class="blog-body">
          <xsl:call-template name="outputContent"/>
        </div>
        <div class="blog-footer">Posted:
          <xsl:text> </xsl:text>
          <xsl:value-of select="substring(atom:published,1,10)"/>
          <xsl:text> </xsl:text>
        </div>
        <br/>
        <hr/>
      </div>
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

  <embed>
          <xsl:attribute name="width">420</xsl:attribute>
          <xsl:attribute name="height">520</xsl:attribute>
          <xsl:attribute name="src"><xsl:value-of select="atom:content/@src"/></xsl:attribute>
          <xsl:attribute name="type"><xsl:value-of select="atom:content/@type"/></xsl:attribute>
 </embed>
<blockquote>
<xsl:value-of select="*[local-name()='group']/*[local-name()='description']"/>
</blockquote>
 
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
  
  
  
  
  
  
</xsl:stylesheet>