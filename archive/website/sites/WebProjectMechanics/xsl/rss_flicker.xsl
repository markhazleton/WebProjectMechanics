
<xsl:stylesheet version="1.0" xmlns:media="http://search.yahoo.com/mrss/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/rss">
    <xsl:for-each select="channel">
      <div class="blog-title">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="*[local-name()='channel']/*[local-name()='link']"/>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:text>top</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:text>top</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="/rss/channel/title"/>
        </a>
      </div>
      <br/>
      <xsl:for-each select="item">
        <a href="{media:content/@url}" rel="lightbox[flicker]" title="{title}" target="_blank">
          <img alt="{title}" width="{media:thumbnail/@width}" height="{media:thumbnail/@height}" src="{media:thumbnail/@url}"/>
        </a>
      </xsl:for-each>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="TEST" userelativepaths="yes" externalpreview="no" url="..\..\access_db\xml\Cym Lowell Flickr RSS-23-XML.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->