<Q:stylesheet version="1.0" xmlns:media="http://search.yahoo.com/mrss/" xmlns:Q="http://www.w3.org/1999/XSL/Transform" >
  <Q:output method="xhtml" />
  <media:output method="xhtml" />
  <Q:template match="/rss" >
  <Q:for-each select="channel">
        <Q:for-each select="item">
        <Q:if test="position()&lt;13">
          <a href="{media:content/@url}" rel="lightbox[flicker]" title="{title}" target="_blank">
            <img>
            <Q:attribute name="border">0</Q:attribute>
            <Q:attribute name="alt"><Q:value-of select="title"></Q:value-of></Q:attribute>
            <Q:attribute name="width"><Q:value-of select="media:thumbnail/@width"></Q:value-of></Q:attribute>
            <Q:attribute name="height"><Q:value-of select="media:thumbnail/@height"></Q:value-of></Q:attribute>
            <Q:attribute name="src"><Q:value-of select="media:thumbnail/@url"></Q:value-of></Q:attribute>
            </img>
          </a>
          </Q:if>
        </Q:for-each>
    </Q:for-each>
  </Q:template>
</Q:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="TEST" userelativepaths="yes" externalpreview="no" url="..\XML\Cym Flickr RSS-23-XML.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->