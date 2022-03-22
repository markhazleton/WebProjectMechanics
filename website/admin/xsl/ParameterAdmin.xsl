<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com" >
  <xsl:output omit-xml-declaration="yes" method="html" />
  <xsl:template match="/">
    <h1>Site Parameter</h1>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover dataTable no-footer" id="dataTables-parameter">
        <thead>
          <tr>
            <th>RecordSource</th>
            <th>Company</th>
            <th>Sort Order</th>
            <th>Type Name</th>
            <th>PageID</th>
            <th>Type Description</th>
            <th>Value</th>
          </tr>
        </thead>
        <xsl:for-each select="a:Company/a:Parameters/a:Parameter">
          <xsl:sort select="a:CompanyID" order="descending"/>
          <xsl:sort select="a:SortOrder"/>
          <tr>
            <td class="left">
              <a>
                <xsl:attribute name="href">
                  <xsl:if test="a:CompanyID!=''">
                    /admin/maint/default.aspx?type=Parameter&amp;CompanySiteParameterID=<xsl:value-of select="a:CompanySiteParameterID"/>
                  </xsl:if>
                  <xsl:if test="a:CompanyID=''">
                    /admin/maint/default.aspx?type=Parameter&amp;ParameterTypeID=<xsl:value-of select="a:ParameterTypeID"/>
                  </xsl:if>
                </xsl:attribute>
                <xsl:value-of select="a:RecordSource"/>-<xsl:value-of select="a:CompanySiteParameterID"/>-<xsl:value-of select="a:ParameterTypeID"/>
              </a>

            </td>
            <td class="left">
              <xsl:if test="a:CompanyID!=''">
                <xsl:value-of select="/a:Company/a:CompanyName"/>
              </xsl:if>
            </td>
            <td class="numric">
              <xsl:value-of select="a:SortOrder"/>
            </td>
            <td class="left">
              <a>
                <xsl:attribute name="href">
                  /wpm/dxAdmin/dxSiteParameterTypeEdit.aspx?SiteParameterTypeID=<xsl:value-of select="a:SiteParameterTypeID"/>
                </xsl:attribute>
                <xsl:value-of select="a:ParameterTypeNM"/>
              </a>
            </td>
            <td class="left">
              <xsl:value-of select="a:LocationID"/>
            </td>
            <td class="left">
              <xsl:value-of select="a:ParameterTypeDS"/>
            </td>
            <td class="left">
              <pre>
                <xsl:value-of select="a:ParameterValue"/>
              </pre>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>
</xsl:stylesheet>