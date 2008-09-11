<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>
<xsl:template match="*">
<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
<table border="0" align="center" valign="top" ><tr>
  <xsl:for-each select="//*[local-name()='Details']">
    <td align="center" style="font-size: 10px;">
    <a >
      <xsl:attribute name="href"><xsl:value-of select="@url"/></xsl:attribute>
      <xsl:attribute name="target"><xsl:text>top</xsl:text></xsl:attribute>
      <xsl:attribute name="alt"><xsl:value-of select="*[local-name()='ProductName']" /></xsl:attribute>
    <img>
      <xsl:attribute name="src"><xsl:value-of select="*[local-name()='ImageUrlSmall']"/></xsl:attribute>
      <xsl:attribute name="alt"><xsl:value-of select="*[local-name()='ProductName']" /></xsl:attribute>
    </img>
    <br />
    <xsl:value-of select="*[local-name()='ProductName']" />
    </a>
    <br />
    <xsl:for-each select="*[local-name()='Authors']" >
     <xsl:value-of select="*[local-name()='Author']" />
    </xsl:for-each>
    <br />
    <xsl:value-of select="*[local-name()='OurPrice']" />
    </td>
  </xsl:for-each>
</tr></table>
</xsl:template>
<xsl:template match="/">
<xsl:apply-templates/>
</xsl:template>
</xsl:stylesheet>
