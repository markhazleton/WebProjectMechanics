<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com">
  <xsl:output omit-xml-declaration="yes" method="html"/>
  <xsl:template match="/">
    <h1>Page Alias Setup</h1>
    <h2>
      <a href="/admin/maint/default.aspx?Type=PageAlias&amp;PageAliasID=NEW">Add Page Alias</a>
    </h2>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover" id="dataTables-admin">
        <thead>
          <tr>
            <th></th>
            <th>Page URL</th>
            <th>Transfer URL</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="a:Company/a:LocationAliases/a:LocationAlias">
            <xsl:sort select="PageURL"/>
            <tr>
              <td>
                <a>
                  <xsl:attribute name="href">
                    /admin/maint/default.aspx?Type=PageAlias&amp;PageAliasID=<xsl:value-of select="a:PageAliasID"/>
                  </xsl:attribute>EDIT
                </a>
              </td>
              <td class="left">
                <a>
                  <xsl:attribute name="href">
                    <xsl:value-of select="a:PageURL"/>
                  </xsl:attribute>
                  <xsl:attribute name="target">_blank</xsl:attribute>
                  <xsl:value-of select="a:PageURL"/>
                </a>
              </td>
              <td class="left">
                <xsl:value-of select="a:TargetURL"/>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
        <tfoot></tfoot>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>