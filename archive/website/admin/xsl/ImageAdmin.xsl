<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com">
  <xsl:output omit-xml-declaration="yes" method="html"/>
  <xsl:template match="/">
    <h1>Image Admin</h1>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-admin">
        <thead>
          <tr>
            <th>Image Name</th>
            <th>Description</th>
            <th>Title</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="a:Company/a:Images/a:LocationImage">
            <xsl:sort select="a:ImageID" data-type="number"/>
            <tr>
              <td>
                <a>
                  <xsl:attribute name="href">
                    /admin/maint/default.aspx?type=Image&amp;ImageID=<xsl:value-of select="a:ImageID"/>
                  </xsl:attribute>
                  <xsl:attribute name="title">
                    <xsl:value-of select="a:ImageFileName"/>
                  </xsl:attribute>
                  <xsl:value-of select="a:ImageName"/>
                </a>
              </td>
              <td>
                <xsl:value-of select="a:ImageDescription"/>
                <br/>
              </td>
              <td>
                <xsl:value-of select="a:Title"/>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>