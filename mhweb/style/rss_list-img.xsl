<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0"
  xmlns:Q = "http://www.w3.org/1999/XSL/Transform"
>
<Q:output method="html" />
<Q:template match="/">
<div class="rightnav" style="min-width:300px;width:400px;">
<strong class="feedtitle"><a accesskey="0" href="{/rss/channel/link}" target="_new" >
  <Q:value-of select="/rss/channel/title"/>
</a></strong>


<Q:for-each select="/rss/channel/description">
  <Q:if test=". != /rss/channel/title" >
  <!-- no point in printing them both if they're the same -->
    <p class='desc'><Q:value-of select="."/></p>
  </Q:if>
</Q:for-each>

<Q:variable name="C" select="count(/rss/channel/item)" />
<dl class='Items'>
<Q:if test='$C = 0'>  <dt>(Empty)</dt> </Q:if>
<Q:for-each select="/rss/channel/item">
<dt>
  <Q:for-each select="enclosure">
     <!-- There can be 0, 1, or many enclosures for each item. -->
     <span class="enclosure"><a href="{@url}" type="{@type}" 
       title="Click to download a '{@type}' file of about {@length} bytes"
     ><img
       alt  ="Click to download a '{@type}' file of about {@length} bytes"
       src="http://interglacial.com/rss/dl_icon.gif"
       width="35" height="36" border="0"
     /></a></span>
  </Q:for-each>

  <a target="_new" href="{link}">
    <Q:if test="position() &lt; 10">
      <Q:attribute name='accesskey'><Q:value-of select="position()" /></Q:attribute>
    </Q:if>

<img>
<Q:attribute name="src">
<Q:value-of select="m1Image/url"/>
</Q:attribute>
</img><br/>
    <Q:choose>
      <Q:when test="not(title) or title = ''" ><em>(No title)</em></Q:when>
      <Q:otherwise		><Q:value-of select="title"/></Q:otherwise>
    </Q:choose>

  </a>
</dt>
</Q:for-each>
</dl>
<blockquote class='aboutThisFeed'>
<Q:for-each select="/rss/channel/lastBuildDate"><p><em>
 Last feed update:</em>
 <span id="lastBuildDate"><Q:value-of select="."/></span></p></Q:for-each>
<Q:for-each select="/rss/channel/webMaster"><p><em>
 Feed admin:</em> <Q:value-of select="." /></p></Q:for-each>
</blockquote>
</div>
		</Q:template>
			</Q:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\XML\Colbert Videos-1-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->