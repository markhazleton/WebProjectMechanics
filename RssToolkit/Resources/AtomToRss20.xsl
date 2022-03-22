<?xml version="1.0" ?>
<xsl:stylesheet version="1.1" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xml:output mode="xml" encoding="UTF-8"/>

  <xsl:template match="/">
    <xsl:element name="rss">
      <xsl:attribute name="version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:element name="generator">
          <xsl:value-of select="//*[name()='generator']"/>
        </xsl:element>
        <xsl:element name="title">
          <xsl:value-of select="//*[name()='title']"/>
        </xsl:element>
        <xsl:element name="link">
          <xsl:value-of select="//*[name()='link']/@href"/>
        </xsl:element>
        <xsl:element name="description">
          <xsl:value-of select="//*[name()='tagline']"/>
        </xsl:element>
        <xsl:element name="copyright">
          <xsl:value-of select="//*[name()='copyright']"/>
        </xsl:element>
        <xsl:element name="pubDate">
          <xsl:value-of select="//*[name()='modified']"/>
        </xsl:element>
        <xsl:element name="lastBuildDate">
          <xsl:value-of select="//*[name()='modified']"/>
        </xsl:element>
        <xsl:call-template name="items"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>


  <xsl:template name="items">
    <xsl:for-each select="//*[name()='entry']">
      <xsl:element name="item">
        <xsl:element name="title">
          <xsl:value-of select="child::*[name()='title']"/>
        </xsl:element>
        <xsl:element name="link">
          <xsl:value-of select="//*[name()='link']/@href"/>
        </xsl:element>
        <xsl:element name="guid">
          <xsl:value-of select="child::*[name()='id']"/>
        </xsl:element>
        <xsl:element name="category">
          <xsl:value-of select="child::*[name()='summary']"/>
        </xsl:element>
        <xsl:element name="description">
          <xsl:value-of select="child::*[name()='content']"/>
        </xsl:element>
        <xsl:element name="pubDate">
          <xsl:value-of select="child::*[name()='modified']"/>
        </xsl:element>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>
