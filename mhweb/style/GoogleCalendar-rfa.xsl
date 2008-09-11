<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:gd="http://schemas.google.com/g/2005">
  <!--<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>-->
  <xsl:output method="html"/>
  <xsl:template match="/atom:feed">

    <style>div.hidden {display:none;}</style>
    <div id="myPanel">
      <div class="hd">
        <div class="tl"></div>
        <span id="myPanelTitle"></span>
        <div class="tr"></div>
      </div>
      <div class="bd" id="myPanelBody"></div>
      <div class="ft" id="myPanelFooter"></div>
    </div>
    <script type="text/javascript">YAHOO.namespace("example.container");

function ShowMyPanel(myTitle,myBody){
document.getElementById('myPanelTitle').innerHTML = myTitle;
document.getElementById('myPanelBody').innerHTML = document.getElementById(myBody).innerHTML ;
document.getElementById('myPanelFooter').innerHTML = "Reflection Fine Art - Event Calendar";
YAHOO.example.container.myPanel.show();
};	
	
YAHOO.util.Event.onDOMReady(function () {

     YAHOO.example.container.myPanel = new YAHOO.widget.Panel("myPanel", {
			width:"600px",
			visible:false,
			constraintoviewport:true,
			draggable:true
		});
		YAHOO.example.container.myPanel.render();
	});</script>


    <h2>
      <xsl:value-of select="*[local-name()='title']"/>
    </h2>
    <ul>
      <xsl:apply-templates select="atom:entry">
        <xsl:sort select="gd:when/@startTime" order="descending"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>

  <xsl:template match="atom:entry" name="feed">
    <xsl:variable name="myYear">
      <xsl:value-of select="substring(gd:when/@startTime,1,4)"/>
    </xsl:variable>
    <xsl:variable name="myMonth">
      <xsl:value-of select="substring(gd:when/@startTime, 6,2)"/>
    </xsl:variable>
    <xsl:variable name="myDay">
      <xsl:value-of select="substring(gd:when/@startTime, 9,2)"/>
    </xsl:variable>
    <xsl:variable name="gUID">
      <xsl:value-of select="gd:when/@startTime"/>
    </xsl:variable>
    <xsl:if test="$myYear='2008'">

      <li>
        <a href="" target="_blank">
          <xsl:attribute name="title">
          </xsl:attribute>
          <xsl:attribute name="onmouseover">ShowMyPanel('Reflection Fine Art | Events','<xsl:value-of select="$gUID"/>')</xsl:attribute>
          <xsl:value-of select="atom:title"/>(<xsl:value-of select="$myMonth"/>/<xsl:value-of select="$myDay"/>/<xsl:value-of select="$myYear"/>)</a>
        <div class="hidden">
          <xsl:attribute name="id">
            <xsl:value-of select="$gUID"/>
          </xsl:attribute>
          <br/>
          <dl>
            <dd>What</dd>
            <dt>
              <xsl:value-of select="atom:title"/>
            </dt>
            <dd>When</dd>
            <dt>
              <xsl:value-of select="$myMonth"/>/<xsl:value-of select="$myDay"/>/<xsl:value-of select="$myYear"/></dt>
            <dd>DESCRIPTION</dd>
            <dt>
              <xsl:value-of select="atom:content"/>
            </dt>
          </dl>
          <br/>
          <hr/>
          <a title="Google Calendar Entry">
            <xsl:attribute name="href">
              <xsl:for-each select="*[local-name()='link']">
                <xsl:if test="@title='alternate'">
                  <xsl:value-of select="@href"/>
                </xsl:if>
              </xsl:for-each>
            </xsl:attribute>Google Calendar Entry</a>
        </div>
      </li>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="http://www.google.com/calendar/feeds/dg6cm6ihavs8imppirm7jcurbc@group.calendar.google.com/public/full" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->