<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <xsl:for-each select="ppms_narratives/Narrative">
      <blockquote>
        <xsl:value-of select="Objective" disable-output-escaping="yes"/>
        <br/>
        <a>
          <xsl:attribute name="href">/portal/ppms.aspx?url=<xsl:value-of select="NarrativeFileName"/></xsl:attribute>
          <xsl:value-of select="NarrativeNM" />
        </a>
        <a>
          <xsl:attribute name="target">_blank</xsl:attribute>
          <xsl:attribute name="href">/portal/ppms.aspx?url=<xsl:value-of select="NarrativeFileName"/></xsl:attribute>
          <img src="/app_themes/pm/pdf.gif" border="0" alt="PDF Document"/>
        </a>
      </blockquote>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="SingleNarrative" userelativepaths="yes" externalpreview="no" url="..\narrative\IT Internal\Application_Development_Security&#x2D;Policy.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="yes" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no" ><SourceSchema srcSchemaPath="IT\Account_Passwords&#x2D;Policy&#x2D;2006&#x2D;08&#x2D;01.xml" srcSchemaRoot="ppms" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/></MapperInfo><MapperBlockPosition><template match="/"><block path="xsl:for&#x2D;each" x="16" y="20"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/xsl:for&#x2D;each" x="271" y="66"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/strong/xsl:value&#x2D;of" x="311" y="66"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/blockquote/xsl:value&#x2D;of" x="231" y="66"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/table/tbody/tr[1]/td[3]/span/a/xsl:attribute/xsl:value&#x2D;of" x="111" y="66"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/table/tbody/tr[2]/td[3]/span/xsl:for&#x2D;each" x="151" y="66"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/table/tbody/tr[2]/td[3]/span/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of" x="64" y="46"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/table/tbody/tr[2]/td[3]/span/xsl:for&#x2D;each/a/xsl:attribute/xsl:value&#x2D;of[1]" x="26" y="48"/><block path="xsl:for&#x2D;each/table/tbody/tr[1]/td/table/tbody/tr[2]/td[3]/span/xsl:for&#x2D;each/a/xsl:value&#x2D;of" x="191" y="66"/></template></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->