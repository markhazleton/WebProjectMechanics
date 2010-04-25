<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:template match="/">
		<h1>Site Parameter</h1>
		<table class="sortable autostripe ">
			<thead>
				<tr>
					<th></th>
					<th>Company</th>
					<th>Sort Order</th>
					<th>Type Name</th>
					<th>PageID</th>
					<th>Type Description</th>
				</tr>
			</thead>
			<xsl:for-each select="wpmSiteProfile/SiteParameterList/wpmSiteParameter">
				<xsl:sort select="CompanyID" order="descending"/>
				<xsl:sort select="SortOrder"/>
				<tr>
					<td class="left">
						<a>
							<xsl:attribute name="href">
								<xsl:if test="CompanyID!=''">/wpmgen/companysiteparameter_edit.aspx?CompanySiteParameterID=<xsl:value-of select="CompanySiteParameterID"/></xsl:if>
								<xsl:if test="CompanyID=''">/wpmgen/siteparametertype_edit.aspx?SiteParameterTypeID=<xsl:value-of select="SiteParameterTypeID"/></xsl:if>
							</xsl:attribute>Edit</a>
						<br/>
						<xsl:value-of select="RecordSource"/>
						</td>
					<td class="left">
						<xsl:if test="CompanyID!=''">
							<xsl:value-of select="/wpmSiteProfile/CompanyName"/>
						</xsl:if>
					</td>
					<td class="numric">
						<xsl:value-of select="SortOrder"/>
					</td>
					<td class="left">
						<a>
							<xsl:attribute name="href">/wpmgen/siteparametertype_edit.aspx?SiteParameterTypeID=<xsl:value-of select="SiteParameterTypeID"/></xsl:attribute>
							<xsl:value-of select="SiteParameterTypeNM"/>
						</a>
					</td>
					<td class="left">
						<xsl:value-of select="PageID"/>
					</td>
					<td class="left">
						<xsl:value-of select="SiteParameterTypeDS"/>
					</td>
				</tr>
			</xsl:for-each>
		</table>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\index\The Frog's Folly-site-file.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->