<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com" >
<xsl:output omit-xml-declaration="yes" method="html" />
	<xsl:template match="/">
		<h1>Site Locations (Category, Page, Article, Image )</h1>
    <div class="table-responsive">
      <table class="table table-striped table-bordered table-hover" id="dataTables-admin">
			<thead>
				<tr>
          <th>Location Name</th>
          <th>RecordSource</th>
          <th>Level</th>
					<th>Group Name</th>
					<th>Parent(s)</th>
					<th>Location Keywords</th>
					<th>Location Title</th>
				</tr>
			</thead>
			<xsl:for-each select="a:Company/a:Locations/a:Location">
				<xsl:sort select="a:LocationName"/>
				<tr>
          <td class="left">
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
                  /admin/maint/default.aspx?type=Article&amp;ArticleID=<xsl:value-of select="a:ArticleID"/>
                </xsl:attribute>
                <xsl:value-of select="a:LocationName"/>
              </a>
            </xsl:if>
            <xsl:if test="a:RecordSource='Image'">
              <a>
                <xsl:attribute name="href">
                  /admin/maint/default.aspx?type=Image&amp;ImageID=<xsl:value-of select="a:ArticleID"/>
                </xsl:attribute>
                <xsl:value-of select="a:LocationName"/>
              </a>
            </xsl:if>
          </td>
          <td class="left">
						<xsl:value-of select="a:RecordSource"/>
					</td>
					<td class="numeric">
						<xsl:value-of select="a:LevelNBR"/>
					</td>
					<td class="left">
						<xsl:value-of select="a:SiteCategoryGroupName"/>
					</td>
					<td class="left" style="white-space: nowrap;">
						<xsl:if test="a:LevelNBR=1">
							<xsl:if test="a:ParentLocationID!=''">
								<strong>Z - Level One With Parent<br/>
									<xsl:value-of select="a:ParentLocationID"/>
								</strong>
							</xsl:if>
						</xsl:if>
						<xsl:for-each select="a:LocationTrailList/a:LocationTrail">
							<xsl:sort select="a:MenuLevelNBR" order="descending"/>
							<xsl:if test="a:MenuLevelNBR!=../../a:LevelNBR">
								<xsl:value-of select="a:Name"/>-<xsl:value-of select="a:LocationID"/>    (<xsl:value-of select="a:MenuLevelNBR"/>)<br/></xsl:if>
							<xsl:if test="a:MenuLevelNBR=../../a:LevelNBR">
								<xsl:if test="a:Name!=../../a:Name">
									<strong>
										<xsl:value-of select="a:Name"/>-<xsl:value-of select="a:LocationID"/>   (<xsl:value-of select="a:MenuLevelNBR"/>)</strong>
									<br/>
								</xsl:if>
							</xsl:if>
						</xsl:for-each>
					</td>
					<td class="left">
						<xsl:value-of select="a:LocationKeywords"/>
						<br/>
					</td>
					<td class="left">
						<xsl:value-of select="a:LocationTitle"/>
						<br/>
					</td>
				</tr>
			</xsl:for-each>
		</table>
      
    </div>
	</xsl:template>
</xsl:stylesheet>