<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:template match="/">
		<h1>Site Navigation</h1>
		<table class="sortable autostripe ">
			<thead>
				<tr>
					<th>RecordSource</th>
					<th>Level</th>
					<th>Group Name</th>
					<th>Parent</th>
					<th>PageName</th>
					<th>Page Keywords</th>
					<th>Page Description</th>
					<th>Page Title</th>
				</tr>
			</thead>
			<xsl:for-each select="wpmSiteProfile/LocationList/wpmLocation">
				<xsl:sort select="PageName"/>
				<tr>
					<td class="left">
						<xsl:value-of select="RecordSource"/>
					</td>
					<td class="numeric">
						<xsl:value-of select="LevelNBR"/>
					</td>
					<td class="left">
						<xsl:value-of select="SiteCategoryGroupName"/>
					</td>
					<td class="left" style="white-space: nowrap;">
						<xsl:if test="LevelNBR=1">
							<xsl:if test="ParentPageID!=''">
								<strong>Z - Level One With Parent<br/>
									<xsl:value-of select="ParentPageID"/>
								</strong>
							</xsl:if>
						</xsl:if>
						<xsl:for-each select="LocationTrailList/wpmLocationTrail">
							<xsl:sort select="MenuLevelNBR" order="descending"/>
							<xsl:if test="MenuLevelNBR!=../../LevelNBR">
								<xsl:value-of select="PageName"/>(<xsl:value-of select="MenuLevelNBR"/>)<br/></xsl:if>
							<xsl:if test="MenuLevelNBR=../../LevelNBR">
								<xsl:if test="PageName!=../../PageName">
									<strong>
										<xsl:value-of select="PageName"/>(<xsl:value-of select="MenuLevelNBR"/>)</strong>
									<br/>
								</xsl:if>
							</xsl:if>
						</xsl:for-each>
					</td>
					<td class="left">
						<xsl:if test="RecordSource='Page'">
							<a>
								<xsl:attribute name="href">/wpmgen/page_edit.aspx?PageID=<xsl:value-of select="PageID"/></xsl:attribute>
								<xsl:value-of select="PageName"/>
							</a>
						</xsl:if>
						<xsl:if test="RecordSource='Category'">
							<a>
								<xsl:attribute name="href">/wpmgen/sitecategory_edit.aspx?SiteCategoryID=<xsl:value-of select="SiteCategoryID"/></xsl:attribute>
								<xsl:value-of select="PageName"/>
							</a>
						</xsl:if>
						<xsl:if test="RecordSource='Article'">
							<a>
								<xsl:attribute name="href">/wpmgen/article_edit.aspx?ArticleID=<xsl:value-of select="ArticleID"/></xsl:attribute>
								<xsl:value-of select="PageName"/>
							</a>
						</xsl:if>
						<xsl:if test="RecordSource='Image'">
							<a>
								<xsl:attribute name="href">/wpmgen/image_edit.aspx?ImageID=<xsl:value-of select="ArticleID"/></xsl:attribute>
								<xsl:value-of select="PageName"/>
							</a>
						</xsl:if>
					</td>
					<td class="left">
						<xsl:value-of select="PageKeywords"/>
						<br/>
					</td>
					<td class="left">
						<xsl:value-of select="PageDescription"/>
						<br/>
					</td>
					<td class="left">
						<xsl:value-of select="PageTitle"/>
						<br/>
					</td>
				</tr>
			</xsl:for-each>
		</table>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\index\The Frog's Folly-site-file.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->