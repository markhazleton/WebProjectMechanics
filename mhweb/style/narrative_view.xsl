<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="US-ASCII" indent="yes"/>
	<xsl:param name="HierarchyTypeID"/>
	<xsl:param name="HierarchyShortNM"/>
	<xsl:template match="/">
		<xsl:for-each select="ppms_narratives">
			<table class="NarrativeTable" cellspacing="5" cellpadding="5" width="725">
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
						<td rowspan="2">
						<img src="/app_themes/pm/logo.gif" width="70" height="70" alt="Project Mechanics" />
						</td>
						<td class="NarrativeValue" width="355" colspan="3">
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
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="SingleNarrative" userelativepaths="yes" externalpreview="no" url="..\narrative\IT External\Account_Management&#x2D;Policy.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="yes" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no" ><SourceSchema srcSchemaPath="..\IT\Application_Lifecycle&#x2D;Policy.xml" srcSchemaRoot="ppms_narratives" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/></MapperInfo><MapperBlockPosition><template match="/"><block path="xsl:for&#x2D;each" x="16" y="20"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/xsl:for&#x2D;each" x="398" y="86"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[2]/td[1]/xsl:for&#x2D;each" x="398" y="46"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[2]/td[1]/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of" x="318" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[2]/td[1]/xsl:for&#x2D;each/a/xsl:value&#x2D;of" x="78" y="86"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[2]/td[1]/xsl:for&#x2D;each/a/xsl:value&#x2D;of[1]" x="38" y="86"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[3]/td[1]/xsl:for&#x2D;each" x="248" y="56"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each" x="358" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of" x="158" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of[1]" x="118" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of[2]" x="78" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:value&#x2D;of" x="118" y="46"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:value&#x2D;of[1]" x="78" y="46"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/a/xsl:value&#x2D;of[2]" x="38" y="46"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/ul/xsl:for&#x2D;each" x="38" y="126"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/ul/xsl:for&#x2D;each/li/a/xsl:attribute/xsl:value&#x2D;of" x="358" y="86"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[4]/td[1]/xsl:for&#x2D;each/ul/xsl:for&#x2D;each/li/a/xsl:value&#x2D;of" x="358" y="86"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/div/table/tbody/tr[1]/td/table/tbody/tr[5]/td[1]/xsl:value&#x2D;of" x="198" y="86"/></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->