<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/rss">
<html>
<head>
	<link href="xsl.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
	body {
		font-size:0.83em;
	}
	</style>
</head>
<body>
	<div id="logo">
		<xsl:element name="a">
			<xsl:attribute name="href">
				<xsl:value-of select="channel/link" />
			</xsl:attribute>
			<xsl:value-of select="channel/title" />
		</xsl:element>
	</div>
	<div class="Snippet" style="border-width:0; background-color:#FFF; margin:1em">
		<div class="titleWithLine">
			<xsl:value-of select="channel/description" />
		</div>
		<dl style="padding-right:1em">
			<xsl:for-each select="channel/item">
				<dd>
					<xsl:element name="a">
						<xsl:attribute name="href">
							<xsl:value-of select="link"/>
						</xsl:attribute>
						<xsl:value-of select="title"/>
					</xsl:element>
				</dd>
				<dt>
					<xsl:value-of select="description" /><br />
					<span class="comments"><xsl:value-of select="pubDate" /></span>
				</dt>
			</xsl:for-each>
		</dl>
	</div>
	<div id="footer">
		<xsl:value-of select="channel/copyright" />
	</div>
</body>
</html>
</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\Amazon-TEST-TEST.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->