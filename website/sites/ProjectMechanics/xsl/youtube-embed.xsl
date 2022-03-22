<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0" xmlns:Q="http://www.w3.org/1999/XSL/Transform">
	<Q:output method="html"/>
	<Q:template match="/">
		<Q:variable name="C" select="count(/rss/channel/item)"/>
		<Q:if test="$C = 0">
		</Q:if>
		<Q:for-each select="/rss/channel/item">
			<Q:if test="position() &lt;11">
				<object width="150" height="200">
					<param name="movie" value="{enclosure/@url}"></param>
					<param name="allowFullScreen" value="true"></param>
					<embed src="{enclosure/@url}" type="application/x-shockwave-flash" allowfullscreen="true" width="200" height="175"></embed>
				</object>
				<blockquote>
					<Q:value-of disable-output-escaping="yes" select="description"/>
				</blockquote>
				<br/>
			</Q:if>
		</Q:for-each>
	</Q:template>

	<Q:template match="@url">
	</Q:template>
</Q:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\YouTube Cloud Computing RSS Feed-32-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->