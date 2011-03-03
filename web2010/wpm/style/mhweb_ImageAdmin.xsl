<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <h1>Image Catalog</h1>
    <table class="sortable autostripe ">
      <thead>
        <tr>
          <th>Thumbnail</th>
          <th>Image Name</th>
          <th>Description</th>
          <th>Title</th>
          <th>Medium</th>
          <th>Size</th>
          <th>Price</th>
          <th>Color</th>
          <th>Subject</th>
          <th>Sold</th>
        </tr>
      </thead>
      <xsl:for-each select="wpmSiteProfile/ImageList/wpmImage">
        <xsl:sort select="ImageFileName"/>
        <tr>
          <td>
          <img>
          <xsl:attribute name="src">
          /wpm/catalog/ImageResize.aspx?w=75&amp;img=<xsl:value-of select="../../SiteGallery"/><xsl:value-of select="ImageFileName"/>
          </xsl:attribute>
          </img>
          </td>
          <td class="left">
            <a>
              <xsl:attribute name="href">/wpm/admin/mhimageedit.aspx?a=<xsl:value-of select="ImageID"/></xsl:attribute>
              <xsl:value-of select="ImageName"/>
            </a>
          </td>
          <td class="left">
            <xsl:value-of select="ImageDescription"/>
          </td>
          <td class="left">
            <xsl:value-of select="Title"/>
          </td>
          <td class="left" style="white-space: nowrap;">
            <xsl:value-of select="Medium"/>
          </td>
          <td class="left">
            <xsl:value-of select="Size"/>
          </td>
          <td class="left">
            <xsl:value-of select="Price"/>
            <br/>
          </td>
          <td class="left">
            <xsl:value-of select="Color"/>
            <br/>
          </td>
          <td class="left">
            <xsl:value-of select="Subject"/>
          </td>
          <td class="left">
            <xsl:value-of select="Sold"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\access_db\index\The Frog's Folly-site-file.xml" htmlbaseurl="" outputurl="" processortype="internal" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->