<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="UTF-8" indent="yes"/>
  <xsl:param name="ViewID"/>
  <xsl:param name="HierarchyShortNM"/>
  <xsl:param name="HierarchyTypeID"/>
  <xsl:template match="/">
    <table style="border:0;" width="750">
      <xsl:for-each select="ArrayOfPpmsView/ppmsView[viewID=$ViewID]">
        <xsl:for-each select="NarrativeTypeColl/ppmsViewNarrativeType">
          <xsl:sort select="NarrativeTypeID"/>
          <xsl:variable name="myNarrativeTypeID" select="NarrativeTypeID"/>
          <xsl:variable name="myNarrativeTypeNM" select="NarrativeTypeNM"/>
          <xsl:variable name="myDisplayHeader" select="DisplayHeaderIND"/>
          <xsl:if test="DisplayHeaderIND='True'">
            <xsl:for-each select="/ArrayOfPpmsView/ppmsView[viewID=$ViewID]">
              <xsl:for-each select="HierarchyTypeColl/ppmsHierarchyType[HierarchyTypeID=$HierarchyTypeID]">
                <xsl:for-each select="HierarchyColl/ppmsHierarchy[$HierarchyShortNM=HierarchyShortNM]">
                  <tr>
                    <td class="ppms_NarrativeType" colspan="2">
                      <span class="ppms_NarrativeType">
                        <xsl:value-of select="$myNarrativeTypeNM"/>
                      </span>
                    </td>
                  </tr>
                  <xsl:for-each select="MappedNarratives/ppmsNarrativeHierarchy">
                    <xsl:if test="NarrativeTypeID=$myNarrativeTypeID">
                      <tr>
                        <td class="ppms_Narrative" align="left" valign="top">
                          <span class="ppms_NarrativeLink">
                            <a class="ppms_NarrativeLink">
                              <xsl:attribute name="ID">
                                <xsl:value-of select="NarrativeID"/>
                              </xsl:attribute>
                              <xsl:attribute name="href">ppms.aspx?url=<xsl:value-of select="NarrativeXMLFile"/>&amp;ParentShortNM=<xsl:value-of select="ParentShortNM"/>&amp;HierarchyTypeID=<xsl:value-of select="$HierarchyTypeID"/></xsl:attribute>
                              <xsl:value-of select="NarrativeNM"/> - <xsl:value-of select="NarrativeTypeNM"/></a> 
                          </span>
                          <span class="ppms_NarrativeObjective"><br/>
                            <xsl:value-of select="Objective" disable-output-escaping="yes"/>
                          </span>
                  
                  <xsl:if test="DisplayChildrenIND='True'" >
                          <ul>
                            <xsl:for-each select="RelatedNarrativeColl/ppmsRelatedNarrative">
                              <xsl:sort select="NarrativeNM"/>
                              <xsl:if test="DisplayHeaderIND='False'">
                              <li>
                                <span class="ppms_NarrativeLink">
                                  <a class="ppms_NarrativeLink">
                                    <xsl:attribute name="ID">
                                      <xsl:value-of select="NarrativeID"/>
                                    </xsl:attribute>
                                    <xsl:attribute name="href">ppms.aspx?url=<xsl:value-of select="NarrativeXMLFile"/>&amp;ParentShortNM=<xsl:value-of select="ParentShortNM"/>&amp;HierarchyTypeID=<xsl:value-of select="$HierarchyTypeID"/></xsl:attribute>
                                    <xsl:value-of select="NarrativeNM"/> - <xsl:value-of select="NarrativeTypeNM"/></a>
                                </span>
                              </li>
                              </xsl:if>
                            </xsl:for-each>
                          </ul>
                  </xsl:if>
                        </td>
                        <td align="right" valign="top">
                          <span class="ppms_PDFLink">
                            <a class="ppms_PDFLink" target="_blank">
                              <xsl:attribute name="ID">PDF_<xsl:value-of select="NarrativeID"/></xsl:attribute>
                              <xsl:attribute name="href">/config/<xsl:value-of select="NarrativePDFFile"/></xsl:attribute>
                              <img border="0" src="/app_themes/pm/pdf.gif">
                                <xsl:attribute name="alt">PDF of <xsl:value-of select="NarrativeNM"/> - <xsl:value-of select="NarrativeTypeNM"/></xsl:attribute>
                              </img>
                            </a>
                          </span>
                        </td>
                      </tr>
                    </xsl:if>
                  </xsl:for-each>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:for-each>
          </xsl:if>
        </xsl:for-each>
      </xsl:for-each>
    </table>
</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\index\View.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->