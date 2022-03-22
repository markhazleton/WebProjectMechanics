<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com">
  <xsl:output omit-xml-declaration="yes" method="html"/>
  <xsl:template match="/">
    <h1>Site Profile</h1>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-admin">
        <thead>
          <tr>
            <th>RecordSource</th>
            <th>LocationID</th>
            <th>ParentLocationID</th>
            <th>Level NBR</th>
            <th>LocationName</th>
            <th>CategoryGroupName</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="a:Company/a:Locations/a:Location">
            <xsl:sort select="a:SiteCategoryName"/>
            <xsl:sort select="a:ParentLocationID"/>
            <xsl:sort select="a:LocationID"/>
            <tr>
              <td>
                <xsl:value-of select="a:RecordSource"/>
              </td>
              <td>
                <a>
                  <xsl:attribute name="href">
                    /admin/maint/default.aspx?type=Location&amp;LocationID=<xsl:value-of select="a:LocationID"/>
                  </xsl:attribute>
                  <xsl:attribute name="title">
                    <xsl:value-of select="a:LocationName"/>
                  </xsl:attribute>
                  <xsl:value-of select="a:LocationID"/>

                </a>
              </td>
              <td>
                <xsl:value-of select="a:ParentLocationID"/>
                <br/>
              </td>
              <td>
                <xsl:value-of select="a:LevelNBR"/>
              </td>
              <td>
                <a>
                  <xsl:attribute name="href">
                    /admin/maint/default.aspx?type=Location&amp;LocationID=<xsl:value-of select="a:LocationID"/>
                  </xsl:attribute>
                  <xsl:attribute name="title">
                    <xsl:value-of select="a:LocationID"/>
                  </xsl:attribute>

                  <xsl:value-of select="a:LocationName"/>
                </a>
              </td>
              <td>
                <xsl:value-of select="a:SiteCategoryGroupName"/>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>