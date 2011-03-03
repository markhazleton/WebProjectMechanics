<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0" xmlns:Q="http://www.w3.org/1999/XSL/Transform">
  <Q:output method="html"/>
  <Q:template match="/">
    <Q:variable name="C" select="count(/rss/channel/item)"/>
    <ul class="Items">
      <Q:if test="$C = 0">
      </Q:if>
      <Q:for-each select="/rss/channel/item">
        <Q:if test="position() &lt;11">
          <li>
            <object width="425" height="349"><param name="movie" value="{enclosure/@url}"></param><param name="allowFullScreen" value="true"></param><embed src="{enclosure/@url}" type="application/x-shockwave-flash" allowfullscreen="true" width="425" height="349"></embed></object>
            <a href="{enclosure/@url}">
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
                  <Q:value-of select="title"/> - (<Q:value-of select="length_seconds"/> Seconds)</Q:otherwise>
              </Q:choose>
              <br/>
              <img>
                <Q:attribute name="src">
                  <Q:value-of disable-output-escaping="yes" select="thumbnail_url"/>
                </Q:attribute>
                <Q:attribute name="alt">
                  <Q:value-of disable-output-escaping="yes" select="description"/>
                </Q:attribute>
              </img>
            </a>
          </li>
        </Q:if>
      </Q:for-each>
    </ul>
  </Q:template>

  <Q:template match="@url">
  </Q:template>
</Q:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\YouTube Cloud Computing RSS Feed-32-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->