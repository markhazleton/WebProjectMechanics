<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="US-ASCII" indent="yes"/>
	<xsl:param name="HierarchyTypeID"/>
	<xsl:param name="HierarchyShortNM"/>
	<xsl:template match="/">
		<xsl:for-each select="ppms_narratives">
			<table class="NarrativeTable" cellspacing="5" cellpadding="5" width="725" >
			<tbody>
				<tr>
					<td colspan="4" valign="top" align="right">
                                    <div class="ppmsNavigation">
                  			  <a>
            					<xsl:attribute name="ID">h_<xsl:value-of select="HierarchyID"/></xsl:attribute>
							<xsl:attribute name="href">ppms.aspx?HierarchyTypeID=<xsl:value-of select="$HierarchyTypeID"/></xsl:attribute>
							<strong>Return To View</strong>
			 			  </a>
						</div><br clear="all" />
		 				<hr class="NarrativeHeaderLine"/>
				  	</td>
				</tr>
				<tr>
						<td rowspan="2" style="background-color:#e8e8a2;">
						<img src="/app_themes/chi/chi_small.gif" width="211" height="53" alt="Catholic Health Initiatives" />
						</td>
						<td class="NarrativeValue" width="355" colspan="3" style="background-color:#e8e8a2;">
							<strong>
								<xsl:value-of select="Narrative/NarrativeNM"/>-<xsl:value-of select="Narrative/NarrativeTypeNM"/></strong>
						</td>
				</tr>
				<tr>
						<td class="NarrativeValue" colspan="3">
							<xsl:value-of select="Narrative/Objective" disable-output-escaping="yes"/>
						</td>
				</tr>
				<tr>
						<td colspan="4">
							<hr class="NarrativeHeaderLine"/>
						</td>
				</tr>
				<tr>
						<td class="NarrativeLabel">Author:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:value-of select="Narrative/RevisedByContactNM"/>
						</td>
				</tr>
				<tr>
						<td class="NarrativeLabel">Department:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:value-of select="Narrative/FunctionalGroupNM"/>
						</td>
				</tr>
				<tr>
						<td class="NarrativeLabel">Effective Date:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:value-of select="Narrative/VersionEffectiveDT"/>
						</td>
				</tr>
				<tr>
						<td class="NarrativeLabel">Version:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:value-of select="Narrative/VersionNumber"/>
						</td>
				</tr>
				<xsl:if test="Narrative/Statement!=''">
					<tr>
							<td class="NarrativeLabel">Statement -<br/>Importance:</td>
							<td class="NarrativeValue" colspan="3">
								<xsl:value-of select="Narrative/Statement" disable-output-escaping="yes"/>
								<hr/>
							</td>
					</tr>
				</xsl:if>
				<xsl:for-each select="Narrative/NarrativeSteps/NarrativeStep">
					<tr>
							<td class="NarrativeLabel">
								<xsl:value-of select="StepNarrativeNM" disable-output-escaping="yes"/>
								<br/>
							</td>
							<td class="NarrativeValue" colspan="3">
								<xsl:value-of select="StepNarrative" disable-output-escaping="yes"/>
								<br/>
								<hr/>
							</td>
					</tr>
				</xsl:for-each>
				<tr>
						<td class="NarrativeLabel">Related Links:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:for-each select="Narrative/NarrativeLinks/NarrativeLink">
								<a>
									<xsl:attribute name="ID">link_<xsl:value-of select="LinkTitle" /></xsl:attribute>
									<xsl:attribute name="href"><xsl:value-of select="LinkURL" /></xsl:attribute>
									<xsl:attribute name="target">_blank</xsl:attribute>
									<xsl:value-of select="LinkTitle"></xsl:value-of>
									</a>
								<br/>
							</xsl:for-each>
						</td>
				</tr>
				<tr>
						<td class="NarrativeLabel">Relevant P&amp;Ps:</td>
						<td class="NarrativeValue" colspan="3">
							<xsl:if test="Narrative/SecurityGroupID=Narrative/ParentSecurityGroupID">
								<xsl:if test="Narrative/ParentNarrativeNM!=''">
									<a>
										<xsl:attribute name="ID">a_<xsl:value-of select="NarrativeID"/></xsl:attribute>
										<xsl:attribute name="href">ppms.aspx?url=<xsl:value-of select="Narrative/ParentNarrativeFileName"/></xsl:attribute>
										<xsl:value-of select="Narrative/ParentNarrativeNM"/>-<xsl:value-of select="Narrative/ParentNarrativeTypeNM"/></a>
									<br/>
									<hr/>
								</xsl:if>
							</xsl:if>
							<xsl:for-each select="Narrative/Child_Narratives/Narrative">
								<xsl:if test="Narrative/SecurityGroupID=Narrative/Child_Narratives/Narrative/SecurityGroupID">
								</xsl:if>

								<a>
									<xsl:attribute name="ID">b_<xsl:value-of select="NarrativeID"/></xsl:attribute>
									<xsl:attribute name="href">ppms.aspx?url=<xsl:value-of select="NarrativeFileXML"/>&amp;HierarchyShortNM=<xsl:value-of select="$HierarchyShortNM"/></xsl:attribute>
									<xsl:value-of select="NarrativeNM"/>-<xsl:value-of select="NarrativeTypeNM"/></a>
								<br/>
							</xsl:for-each>
						</td>
					</tr>
				</tbody>
			</table>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="SingleNarrative" userelativepaths="yes" externalpreview="no" url="..\index\IT Internal\Communication_Management-Policy.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="yes" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no" ><SourceSchema srcSchemaPath="..\IT\Application_Lifecycle-Policy.xml" srcSchemaRoot="ppms_narratives" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/></MapperInfo><MapperBlockPosition><template match="/"></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->