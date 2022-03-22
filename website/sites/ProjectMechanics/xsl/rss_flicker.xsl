<xsl:stylesheet version="1.0" xmlns:media="http://search.yahoo.com/mrss/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes" media-type="string"/>
	<xsl:template match="/rss">
		<xsl:for-each select="channel">
			<div class="box">
				<h2>
					<a>
						<xsl:attribute name="href">
							<xsl:value-of select="/rss/channel/link"/>
						</xsl:attribute>
						<xsl:attribute name="target">
							<xsl:text>_blank</xsl:text>
						</xsl:attribute>Related Uploads from Flickr by Yahoo</a>
				</h2>
				<xsl:for-each select="item">
					<a href="{link}" rel="lightbox[flicker]" title="{title} by {media:credit}" target="_blank">
						<img alt="{title} by {media:credit}" width="{media:thumbnail/@width}" height="{media:thumbnail/@height}" src="{media:thumbnail/@url}"/>
					</a>
				</xsl:for-each>
			</div>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\..\access_db\xml\httpapi.flickr.comservicesfeedsphotos_public.gneformatrss_200langen-ustagsinks+lake+state+park.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->