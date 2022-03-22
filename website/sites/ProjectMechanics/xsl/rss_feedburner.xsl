   1. <?xml version="1.0" encoding="utf-8"?>  
   2. <!DOCTYPE xsl:Stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>  
   3. <xsl:stylesheet  
   4.     version="1.0"  
   5.     xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  
   6.     xmlns:msxml="urn:schemas-microsoft-com:xslt"  
   7.     xmlns:feedburner="http://rssnamespace.org/feedburner/ext/1.0"  
   8.     xmlns:umbraco.library="urn:umbraco.library"  
   9.     xmlns:atom="http://www.w3.org/2005/Atom"  
  10.     xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns"  
  11.     exclude-result-prefixes="msxml rdf umbraco.library">  
  12.   
  13. <xsl:output method="xml" omit-xml-declaration="yes" />  
  14.   
  15.     <xsl:variable name="url" select="/macro/url" />  
  16.         <xsl:variable name="max">  
  17.         <xsl:choose>  
  18.             <xsl:when test="/macro/max != ''"><xsl:value-of select="/macro/max" /></xsl:when>  
  19.             <xsl:otherwise>5</xsl:otherwise>  
  20.         </xsl:choose>  
  21.     </xsl:variable>  
  22.       
  23.     <xsl:template match="/">  
  24.         <xsl:if test="$url != ''">  
  25.             <xsl:variable name="xmlcontent" select="umbraco.library:GetXmlDocumentByUrl($url)" />  
  26.             <ul>  
  27.                 <xsl:apply-templates select="$xmlcontent/atom:feed/atom:entry" />  
  28.             </ul>  
  29.         </xsl:if>  
  30.     </xsl:template>  
  31.       
  32.     <xsl:template match="atom:feed/atom:entry">  
  33.         <xsl:if test="position() &lt; number($max)">  
  34.             <li>  
  35.                 <a href="{atom:id}">  
  36.                     <xsl:value-of select="atom:title"/>  
  37.                 </a>  
  38.                 <br/>  
  39.                 <xsl:value-of select="atom:published"/>  
  40.             </li>  
  41.         </xsl:if>  
  42.     </xsl:template>  
  43.   
  44. </xsl:stylesheet>  <!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="TEST" userelativepaths="yes" externalpreview="no" url="..\XML\ProjectMechanics.Blogspot-8-XML.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->