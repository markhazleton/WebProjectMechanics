<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="*">
		<h2><xsl:value-of select="/rss/channel/title"></xsl:value-of></h2>
		<xsl:for-each select="//*[local-name()='item']">
			<div class="box">
				<xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
			</div>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\..\access_db\xml\httpwww.facebook.comfeedspage.phpformatrss20id185839381457608.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->