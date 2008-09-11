<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet exclude-result-prefixes="rdf rss l dc admin content xsl"
  version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
                  xmlns:rss="http://purl.org/rss/1.0/"
                xmlns:dc="http://purl.org/dc/elements/1.1/"
                  xmlns:admin="http://webns.net/mvcb/"
                  xmlns:l="http://purl.org/rss/1.0/modules/link/"
                  xmlns:content="http://purl.org/rss/1.0/modules/content/">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/rdf:RDF">
    <div id="newslist">
            <xsl:apply-templates select="rss:channel" />
        <ul>
            <xsl:apply-templates select="rss:item" />
        </ul>
    </div>
</xsl:template>

<xsl:template match="rss:channel">
  <br/>
  <h2>
    <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="@rdf:about"/>
            </xsl:attribute>
            <xsl:value-of select="rss:title"/>
    </xsl:element>
    </h2>
  
</xsl:template>


<xsl:template match="rss:item">
    <li>
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:apply-templates select="rss:link"/>
            </xsl:attribute>
            <xsl:attribute name="title">
                <xsl:apply-templates select="rss:description" />
            </xsl:attribute>
            <xsl:value-of select="rss:title"/>
        </xsl:element>
    </li>
</xsl:template>


</xsl:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="RDF RSS 1.0" userelativepaths="yes" externalpreview="no" url="..\XML\Mark Hazleton del.icio.us RSS List-8-XML.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->