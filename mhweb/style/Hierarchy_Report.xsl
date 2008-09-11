<?xml version="1.0" encoding="US-ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="US-ASCII" indent="yes"/>
	<xsl:param name="HierarchyTypeID"/>
	<xsl:param name="ParentShortNM"/>
	<xsl:param name="HierarchyShortNM"/>
	<xsl:template match="/">
		<xsl:for-each select="ArrayOfPpmsHierarchyType/ppmsHierarchyType">
			<h1>
				<xsl:value-of select="HierarchyTypeNM"/>
			</h1>
			<span class="ppms_parentHierarchyHeader">
				<xsl:value-of select="HierarchyTypeDS" disable-output-escaping="yes"></xsl:value-of>
			</span>
			<ol>
				<xsl:for-each select="HierarchyColl/ppmsHierarchy">
					<xsl:if test="HierarchyLevel=1">
						<li class="ppms_parentHierarchy">
							<span class="ppms_parentHierarchyHeader">
								<xsl:value-of select="HierarchyShortNM"/>- <xsl:value-of select="HierarchyNM"/>
								<br/>
							</span>
							<xsl:value-of select="HierarchyDS" disable-output-escaping="yes"/>
							<br/>
							<xsl:value-of select="HierarchyComment" disable-output-escaping="yes"/>
							<br/>
							<ul>
								<xsl:for-each select="../ppmsHierarchy">
									<li>
										<xsl:value-of select="HierarchyNM"/>
									</li>
								</xsl:for-each>
							</ul>
						</li>
					</xsl:if>
				</xsl:for-each>
			</ol>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="ppmsHierarchy" userelativepaths="yes" externalpreview="no" url="..\index\IT FAQ\Hierarchy.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no" ><SourceSchema srcSchemaPath="..\index\IT FAQ\Hierarchy.xml" srcSchemaRoot="ArrayOfPpmsHierarchyType" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/></MapperInfo><MapperBlockPosition><template match="/"><block path="xsl:for-each" x="210" y="26"/><block path="xsl:for-each/ol/xsl:for-each" x="242" y="118"/><block path="xsl:for-each/ol/xsl:for-each/xsl:if/=[0]" x="246" y="154"/><block path="xsl:for-each/ol/xsl:for-each/xsl:if" x="396" y="138"/><block path="xsl:for-each/ol/xsl:for-each/xsl:if/li/xsl:value-of[1]" x="396" y="88"/><block path="xsl:for-each/ol/xsl:for-each/xsl:if/li/ul/xsl:for-each" x="412" y="226"/></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->