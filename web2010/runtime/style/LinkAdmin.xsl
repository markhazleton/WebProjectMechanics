<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Site Links</h1>
    <table class="" border="1">
      <thead>
        <tr>
          <th>Link Information</th>
          <th>Category</th>
          <th>Type</th>
          <th>URL</th>
        </tr>
      </thead>
      <xsl:for-each select="WebsiteProfile/PartList/wpmPart">
        <tr>
          <td>
              <xsl:if test="LinkSource='SiteLink'">
                <a>
                  <xsl:attribute name="href">/wpm/dxAdmin/dxPartEdit.aspx?ID=<xsl:value-of select="LinkID"/></xsl:attribute>
                  <xsl:value-of select="LinkTitle"/>
                </a>
              </xsl:if>
              <xsl:if test="LinkSource='Link'">
                <a>
                  <xsl:attribute name="href">/wpm/dxAdmin/dxPartEdit.aspx?ID=<xsl:value-of select="LinkID"/></xsl:attribute>
                  <xsl:value-of select="LinkTitle"/>
                </a>
              </xsl:if>
            <br/>Page - <xsl:value-of select="PageID"/><br/>
            Source - <xsl:value-of select="LinkSource"/><br/>
            <p>
              <xsl:value-of select="LinkDescription"/>
            </p>
          </td>
          <td>
            <xsl:value-of select="LinkCategoryTitle"/>
          </td>
          <td>
            <xsl:value-of select="LinkTypeCD"/>
          </td>
          <td>
            <xsl:value-of select="LinkURL" disable-output-escaping="no"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>