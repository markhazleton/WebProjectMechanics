<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com" >
  <xsl:output omit-xml-declaration="yes" method="html" />
  <xsl:template match="/">
    <h1>Site Part (Content)</h1>
    <h2>
      <a href="/admin/maint/default.aspx?Type=Part&amp;PartID=NEW">Add Part</a>
    </h2>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-parts">
        <thead>
          <tr>
            <th>Title</th>
            <th>Type</th>
            <th>Category</th>
            <th>Order</th>
            <th>Value</th>
          </tr>
        </thead>
        <xsl:for-each select="a:Company/a:Parts/a:Part">
          <tr>
            <td>
              <a>
                <xsl:attribute name="href">
                  /admin/maint/default.aspx?Type=Part&amp;PartID=<xsl:value-of select="a:PartID"/>
                </xsl:attribute>
                <xsl:value-of select="a:Title"/>
              </a>
            </td>
            <td>
              <xsl:value-of select="a:PartTypeCD"/>
            </td>
            <td>
              <xsl:value-of select="a:PartCategoryTitle"/>
            </td>
            <td>
              <xsl:value-of select="a:PartSortOrder"/>
            </td>
            <td>
              <pre>
                <xsl:value-of select="a:URL"/>
              </pre>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>