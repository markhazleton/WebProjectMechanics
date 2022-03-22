<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE xsl:Stylesheet [ <!ENTITY nbsp " "> ]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:feedburner="http://rssnamespace.org/feedburner/ext/1.0"
	xmlns:umbraco.library="urn:umbraco.library"
	xmlns:atom="http://www.w3.org/2005/Atom"
	xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns"
	exclude-result-prefixes="msxml rdf umbraco.library">

<xsl:output method="xml" omit-xml-declaration="yes" />

	<xsl:variable name="url" select="/macro/url" />
		<xsl:variable name="max">
		<xsl:choose>
			<xsl:when test="/macro/max != ''"><xsl:value-of select="/macro/max" /></xsl:when>
			<xsl:otherwise>5</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	
	<xsl:template match="/">
		<xsl:if test="$url != ''">
			<xsl:variable name="xmlcontent" select="umbraco.library:GetXmlDocumentByUrl($url)" />
			<ul>
				<xsl:apply-templates select="$xmlcontent/atom:feed/atom:entry" />
			</ul>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="atom:feed/atom:entry">
			<li>
				<a href="{atom:id}">
					<xsl:value-of select="atom:title"/>
				</a>
				<br/>
				<xsl:value-of select="atom:published"/>
			</li>
	</xsl:template>

</xsl:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="TEST" userelativepaths="yes" externalpreview="no" url="..\..\access_db\XML\Electoral-Vote RSS-8-XML.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="bWarnings" value="true"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->