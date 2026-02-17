<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:Demographics="http://www.zillow.com/static/xsd/Demographics.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xsi:schemaLocation="http://www.zillow.com/static/xsd/Demographics.xsd /vstatic/c79a81c686bad6b5d0708f4592cf8091/static/xsd/Demographics.xsd">
	<xsl:output method="html"/>
	<xsl:template match="/">
		<a>
			<xsl:attribute name="href">
				<xsl:value-of select="./Demographics:demographics/response/links/main"/>
			</xsl:attribute>Zillow Main Page for <xsl:value-of select="./Demographics:demographics/response/region/city"/></a>
		<dl>
			<xsl:for-each select="./Demographics:demographics/response/charts/chart">
				<dt>
					<xsl:value-of select="name"/>
				</dt>
				<dd>
					<img>
						<xsl:attribute name="src">
							<xsl:value-of select="url"/>
						</xsl:attribute>
					</img>
				</dd>
			</xsl:for-each>
		</dl>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\App_Data\xml\28-Keller TX Demographics - Zillow.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->