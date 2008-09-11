<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>
<xsl:template match="*">
<div id="rssnews">
<a>
  <xsl:attribute name="href">
  <xsl:value-of select="*[local-name()='channel']/*[local-name()='link']"/>
  </xsl:attribute>
  <xsl:attribute name="alt">
  <xsl:text>top</xsl:text>
  </xsl:attribute>
  <xsl:attribute name="target">
  <xsl:text>top</xsl:text>
  </xsl:attribute>
  <xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
</a>
<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
<xsl:value-of select="*[local-name()='channel']/*[local-name()='lastBuildDate']"/>
<ul>
  <xsl:for-each select="//*[local-name()='item']">
  <li>
    <a>
      <xsl:attribute name="href"><xsl:value-of select="*[local-name()='link']"/></xsl:attribute>
      <xsl:attribute name="target"><xsl:text>top</xsl:text></xsl:attribute>
      <xsl:value-of select="*[local-name()='title']"/>
    </a><hr />
    <xsl:value-of select="*[local-name()='description']" />
    <hr />
  </li>
  </xsl:for-each>
</ul></div>
</xsl:template>
<xsl:template match="/">
<xsl:apply-templates/>
</xsl:template>
</xsl:stylesheet>


