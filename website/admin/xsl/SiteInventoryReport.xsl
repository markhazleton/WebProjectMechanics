<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com">
  <xsl:output omit-xml-declaration="yes" method="html"/>
  <xsl:template match="/">
    <h1>Site Inventory</h1>
    <h2>Locations</h2>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-locations">
        <thead>
          <tr>
            <th>RecordSource</th>
            <th>Level</th>
            <th>PageName</th>
            <th>Page Keywords</th>
            <th>Page Description</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="a:Company/a:Locations/a:Location">
            <xsl:sort select="a:LocationName"/>
            <tr>
              <td>
                <xsl:value-of select="a:RecordSource"/>
              </td>
              <td>
                <xsl:value-of select="a:LevelNBR"/>
              </td>
              <td>
                <xsl:if test="a:RecordSource='Page'">
                  <a>
                    <xsl:attribute name="href">
                      /admin/maint/default.aspx?type=Location&amp;LocationID=<xsl:value-of select="a:LocationID"/>
                    </xsl:attribute>
                    <xsl:value-of select="a:LocationName"/>
                  </a>
                </xsl:if>
                <xsl:if test="a:RecordSource='Category'">
                  <a>
                    <xsl:attribute name="href">
                      /admin/maint/default.aspx?type=Location&amp;LocationID=<xsl:value-of select="a:LocationID"/>
                    </xsl:attribute>
                    <xsl:value-of select="a:LocationName"/>
                  </a>
                </xsl:if>
                <xsl:if test="a:RecordSource='Article'">
                  <a>
                    <xsl:attribute name="href">
                      /admin/maint/default.aspx?type=Location&amp;LocationID=<xsl:value-of select="a:LocationID"/>
                    </xsl:attribute>
                    <xsl:value-of select="a:LocationName"/>
                  </a>
                </xsl:if>
              </td>
              <td>
                <xsl:value-of select="a:LocationKeywords"/>
                <br/>
              </td>
              <td>
                <xsl:value-of select="a:LocationDescription"/>
                <br/>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>

      <h2>Parts</h2>
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-parts">
        <thead>
          <tr>
            <th>Part Title</th>
            <th>Category</th>
            <th>Type</th>
            <th>Content</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="a:Company/a:Parts/a:Part">
            <tr>
              <td valign="top" style="white-space: nowrap;">
                  <a>
                    <xsl:attribute name="href">
                      /admin/maint/default.aspx?type=Part&amp;partid=<xsl:value-of select="a:PartID"/>
                    </xsl:attribute>
                    <xsl:value-of select="a:Description"/>
                  </a>
              </td>
              <td valign="top" style="white-space: nowrap;">
                <xsl:value-of select="a:Title"/>
              </td>
              <td valign="top" style="white-space: nowrap;">
                <xsl:value-of select="a:PartTypeCD"/>
              </td>
              <td valign="top" style="white-space:wrap;">
                <pre>
                  <xsl:value-of select="a:URL" disable-output-escaping="no"/>
                </pre>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>
