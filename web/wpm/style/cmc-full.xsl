<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0" xmlns:Q="http://www.w3.org/1999/XSL/Transform">
	<Q:output method="html"/>
	<Q:template match="/">
        <h2><Q:value-of select="rss/channel/title"/></h2>
		<Q:variable name="C" select="count(/rss/channel/item)"/>
		<div class="rssitems">
			<dl>
				<Q:if test="$C = 0">
					<dt>(Empty)</dt>
				</Q:if>
				<Q:for-each select="/rss/channel/item">
					<dt>
						<a href="{link}">
							<Q:if test="position() &lt; 10">
								<Q:attribute name="accesskey">
									<Q:value-of select="position()"/>
								</Q:attribute>
							</Q:if>
							<Q:choose>
								<Q:when test="not(title) or title = ''">
									<em>(No title)</em>
								</Q:when>
								<Q:otherwise>
									<Q:value-of select="title"/>
								</Q:otherwise>
							</Q:choose>
						</a>
					</dt>
					<dd>
						<Q:if test="author">
							<strong>Post by
								<Q:value-of disable-output-escaping="no" select="author"/>
							</strong>
						</Q:if>
						<br/>
						<Q:value-of disable-output-escaping="yes" select="description"/>
						<Q:value-of disable-output-escaping="yes" select="content"/>
					</dd>
				</Q:for-each>
			</dl>
		</div>
	</Q:template>
</Q:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\xml\YouTube Cloud Computing RSS Feed-32-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->