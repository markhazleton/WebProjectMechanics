<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:google="http://base.google.com/ns/1.0">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="*">
		<xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
		<br/>
		<div>
			<xsl:for-each select="//*[local-name()='item']">
				<div>

					<a>
						<xsl:attribute name="href">
							<xsl:value-of select="*[local-name()='guid']"/>
						</xsl:attribute>
						<xsl:attribute name="target">
							<xsl:text>top</xsl:text>
						</xsl:attribute>
						<img>
							<xsl:attribute name="src">
								<xsl:value-of select="google:image_link"/>
							</xsl:attribute>
							<xsl:attribute name="width">48</xsl:attribute>
							<xsl:attribute name="height">48</xsl:attribute>
						</img>
					</a>
				</div>
				<div>

					<a>
						<xsl:attribute name="href">
							<xsl:value-of select="*[local-name()='link']"/>
						</xsl:attribute>
						<xsl:attribute name="target">
							<xsl:text>top</xsl:text>
						</xsl:attribute>
						<xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>(
						<xsl:call-template name="getDate">
							<xsl:with-param name="dateTime" select="pubDate"/>
							<xsl:with-param name="tzOffset" select="-6"/>
						</xsl:call-template>)</a>
				</div>
			</xsl:for-each>
		</div>
	</xsl:template>

	<xsl:template name="getDate">
		<xsl:param name="dateTime"/>
		<xsl:param name="tzOffset"/>
		<xsl:value-of select="substring($dateTime,9,4)"/>
		<xsl:value-of select="substring($dateTime,6,2)"/>
		<xsl:text>,</xsl:text>
		<xsl:value-of select="substring($dateTime,12,6)"/>
		<xsl:variable name="orighour">
			<xsl:choose>
				<xsl:when test="(number(substring($dateTime,18,2))+number($tzOffset)) &lt; 0">
					<xsl:value-of select="(12+(number(substring($dateTime,18,2))+number($tzOffset)))+12"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="number(substring($dateTime,18,2))+number($tzOffset)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="hour">
			<xsl:choose>
				<xsl:when test="$orighour &gt; 12">
					<xsl:value-of select="number($orighour)-12"/>
				</xsl:when>
				<xsl:when test="$orighour = 0">
					<xsl:value-of select="'12'"/>
				</xsl:when>
				<xsl:when test="$orighour = 12">
					<xsl:value-of select="'12'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="number($orighour)-12"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="ampm">
			<xsl:choose>
				<xsl:when test="$orighour &gt; 11">
					<xsl:value-of select="' PM'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="' AM'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$hour"/>
		<xsl:value-of select="substring($dateTime,20,3)"/>
		<xsl:value-of select="$ampm"/>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\xml\1-Facebook Twitter RSS.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->