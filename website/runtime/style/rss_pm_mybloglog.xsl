<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="*">
		<div class="leftbox">
			<h3>
				<a>
					<xsl:attribute name="href">
						<xsl:value-of select="channel/link"/>
					</xsl:attribute>
					<xsl:attribute name="title">
						<xsl:value-of select="channel/title"/>
					</xsl:attribute>

					<img>
						<xsl:attribute name="src">
							<xsl:value-of select="channel/image/url"/>
						</xsl:attribute>
						<xsl:attribute name="align">left</xsl:attribute>
						<xsl:attribute name="hspace">5</xsl:attribute>
					</img>Mark Hazleton<br/>New With Me</a>
			</h3>
			<br clear="all"/>
			<div class="blog-posts">
				<xsl:for-each select="//*[local-name()='item']">
					<div class="blog-body">
						<xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
					</div>
					<small>
						<xsl:call-template name="getDate">
							<xsl:with-param name="dateTime" select="*[local-name()='pubDate']"/>
							<xsl:with-param name="tzOffset" select="0"/>
						</xsl:call-template>
					</small>
					<hr/>
				</xsl:for-each>
			</div>
		</div>
		<br/>
	</xsl:template>
	<xsl:template match="/">
		<xsl:apply-templates/>
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
					<xsl:value-of select="number($orighour)"/>
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
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\App_Data\xml\Mark Hazleton My Blog Log-8-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->