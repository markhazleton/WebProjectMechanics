<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:feedburner="http://rssnamespace.org/feedburner/ext/1.0">
  <xsl:output method="html"/>
  <xsl:variable name="title" select="/rss/channel/title"/>
  <xsl:variable name="feedUrl" select="/rss/channel/atom10:link[@rel='self']/@href" xmlns:atom10="http://www.w3.org/2005/Atom"/>
  <xsl:template match="/">
    <xsl:apply-templates select="rss/channel"/>
  </xsl:template>
  <xsl:template match="channel">
    <div class="blog">
      <h1>
        <xsl:choose>
          <xsl:when test="link">
            <a href="{normalize-space(link)}" title="Link to original website">
              <xsl:value-of select="$title"/>
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$title"/>
          </xsl:otherwise>
        </xsl:choose>
      </h1>
      <hr/>
      <div class="blog-posts">
        <xsl:apply-templates select="item"/>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="feedburner:feedFlare">
    <xsl:variable name="alttext" select="."/>
    <a href="{@href}" onclick="this.href = subscribeNowUltra(this.href,'{$alttext}');return true">
      <img src="{@src}" alt="{$alttext}"/>
    </a>
  </xsl:template>
  <xsl:template match="item" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <div class="blog-entry">
      <div class="blog-name"><h2>
        <xsl:choose>
          <xsl:when test="guid[@isPermaLink='true' or not(@isPermaLink)]">
            <a href="{normalize-space(guid)}">
              <xsl:value-of select="title"/>
            </a>
          </xsl:when>
          <xsl:when test="link">
            <a href="{normalize-space(link)}">
              <xsl:value-of select="title"/>
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="title"/>
          </xsl:otherwise>
        </xsl:choose>
        </h2>
      </div>
      <div class="blog-body">
        <xsl:call-template name="outputContent"/>
      </div>
      <div class="blog-date">
        <xsl:if test="count(child::pubDate)=1">
          <span>Posted:</span>
          <xsl:value-of select="pubDate"/>
        </xsl:if>
        <xsl:if test="count(child::dc:date)=1">
          <span>Posted:</span>
          <xsl:value-of select="dc:date"/>
        </xsl:if>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="image">
    <a href="{normalize-space(link)}" title="Link to original website">
      <img src="{url}" id="feedimage" alt="Link to {title}"/>
    </a>
    <xsl:text/>
  </xsl:template>
  <xsl:template match="feedburner:browserFriendly">
    <p class="about">
      <span style="color:#000">A message from this feed's publisher:</span>
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  <xsl:template name="outputContent">
    <xsl:choose>
      <xsl:when xmlns:xhtml="http://www.w3.org/1999/xhtml" test="xhtml:body">
        <xsl:copy-of select="xhtml:body/*"/>
      </xsl:when>
      <xsl:when xmlns:xhtml="http://www.w3.org/1999/xhtml" test="xhtml:div">
        <xsl:copy-of select="xhtml:div"/>
      </xsl:when>
      <xsl:when xmlns:content="http://purl.org/rss/1.0/modules/content/" test="content:encoded">
        <xsl:value-of select="content:encoded" disable-output-escaping="yes"/>
      </xsl:when>
      <xsl:when test="description">
        <xsl:value-of select="description" disable-output-escaping="yes"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="http://feeds.feedburner.com/ProjectMechanics-blogspot" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->