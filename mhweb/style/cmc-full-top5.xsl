<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0" xmlns:Q="http://www.w3.org/1999/XSL/Transform">
  <Q:output method="html"/>
  <Q:template match="/">
    <Q:variable name="C" select="count(/rss/channel/item)"/>
    <dl class="Items">
      <Q:if test="$C = 0">
        <dt>(Empty)</dt>
      </Q:if>
      <Q:for-each select="/rss/channel/item">
        <Q:if test="position() &lt;6">
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
          <dd name="decodeme">
            <Q:if test="author">
              Post by <strong>
                <Q:value-of disable-output-escaping="no" select="author"/>
              </strong>
            </Q:if>
            <p>
              <Q:value-of disable-output-escaping="yes" select="content"/>
            </p>
          </dd>
        </Q:if>
      </Q:for-each>
    </dl>
  </Q:template>
</Q:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\XML\Electoral-Vote RSS-8-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->