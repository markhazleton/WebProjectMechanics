<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Site Navigation</h1>

    <table border="1">
      <tr>
        <td>RecordSource</td>
        <td>Level</td>
        <td>PageName</td>
        <td>Page Keywords</td>
        <td>Page Description</td>
      </tr>
      <xsl:for-each select="CompanyProfile/LocationList/Location">
        <xsl:sort select="PageName"/>
        <tr>
          <td>
            <xsl:value-of select="RecordSource"/>
          </td>
          <td>
            <xsl:value-of select="LevelNBR"/>
          </td>
          <td>
            <xsl:if test="RecordSource='Page'">
              <a>
                <xsl:attribute name="href">/wpm/dxAdmin/dxPageEdit.aspx?PageID=<xsl:value-of select="PageID"/></xsl:attribute>
                <xsl:value-of select="PageName"/>
              </a>
            </xsl:if>
            <xsl:if test="RecordSource='Category'">
              <a>
                <xsl:attribute name="href">/wpm/dxAdmin/dxLocationEdit.aspx?SiteCategoryID=<xsl:value-of select="SiteCategoryID"/></xsl:attribute>
                <xsl:value-of select="PageName"/>
              </a>
            </xsl:if>
          </td>
          <td>
            <xsl:value-of select="PageKeywords"/>
            <br/>
          </td>
          <td>
            <xsl:value-of select="PageDescription"/>
            <br/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>