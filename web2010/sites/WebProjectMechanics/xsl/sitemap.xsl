<?xml version="1.0" encoding="UTF-8"?>

<!-- Sitmaps Stylesheets (sitemaps.org)
     Project Home: http://www.codeplex.com/wpm
     Copyright (c) 2008 Mark Hazleton (http://www.projectmechanics.com)
    
 -->

<xsl:stylesheet version="2.0" 
                xmlns:html="http://www.w3.org/TR/REC-html40"
                xmlns:sitemap="http://www.google.com/schemas/sitemap/0.9"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" version="1.0" encoding="iso-8859-1" indent="yes"/>
  <!-- Root template -->    
  <xsl:template match="/">
    <html>     
      <head>  
        <script language="JavaScript">
		  <![CDATA[
			var selectedColor = "blue";
			var defaultColor = "black";
			var hdrRows = 1;
			var numeric = '..';
			var desc = '..';
			var html = '..';
			var freq = '..';
			
			function initXsl(tabName,fileType) {
				hdrRows = 1;
			
			  if(fileType=="sitemap") {
			  	numeric = ".3.";
			  	desc = ".1.";
			  	html = ".0.";
			  	freq = ".2.";
			  	initTable(tabName);
				  setSort(tabName, 2, 1);
			  }
			  else {
			  	desc = ".1.";
			  	html = ".0.";
			  	initTable(tabName);
				  setSort(tabName, 1, 1);
			  }
			
			}
			
			function initTable(tabName) {
			  var theTab = document.getElementById(tabName);
			  for(r=0;r<hdrRows;r++)
			   for(c=0;c<theTab.rows[r].cells.length;c++)
			     if((r+theTab.rows[r].cells[c].rowSpan)>hdrRows)
			       hdrRows=r+theTab.rows[r].cells[c].rowSpan;
			  for(r=0;r<hdrRows; r++){
			    colNum = 0;
			    for(c=0;c<theTab.rows[r].cells.length;c++, colNum++){
			      if(theTab.rows[r].cells[c].colSpan<2){
			        theCell = theTab.rows[r].cells[c];
			        rTitle = theCell.innerHTML.replace(/<[^>]+>|&nbsp;/g,'');
			        if(rTitle>""){
			          theCell.title = "Change sort order for " + rTitle;
			          theCell.onmouseover = function(){setCursor(this, "selected")};
			          theCell.onmouseout = function(){setCursor(this, "default")};
			          var sortParams = 15; // bitmapped: numeric|desc|html|freq
			          if(numeric.indexOf("."+colNum+".")>-1) sortParams -= 1;
			          if(desc.indexOf("."+colNum+".")>-1) sortParams -= 2;
			          if(html.indexOf("."+colNum+".")>-1) sortParams -= 4;
			          if(freq.indexOf("."+colNum+".")>-1) sortParams -= 8;
			          theCell.onclick = new Function("sortTable(this,"+(colNum+r)+","+hdrRows+","+sortParams+")");
			        }
			      } else {
			        colNum = colNum+theTab.rows[r].cells[c].colSpan-1;
			      }
			    }
			  }
			}
			
			function setSort(tabName, colNum, sortDir) {
				var theTab = document.getElementById(tabName);
				theTab.rows[0].sCol = colNum;
				theTab.rows[0].sDir = sortDir;
				if (sortDir) 
					theTab.rows[0].cells[colNum].className='sortdown'
				else
					theTab.rows[0].cells[colNum].className='sortup';
			}
			
			function setCursor(theCell, mode){
			  rTitle = theCell.innerHTML.replace(/<[^>]+>|&nbsp;|\W/g,'');
			  if(mode=="selected"){
			    if(theCell.style.color!=selectedColor) 
			      defaultColor = theCell.style.color;
			    theCell.style.color = selectedColor;
			    theCell.style.cursor = "hand";
			    window.status = "Click to sort by '"+rTitle+"'";
			  } else {
			    theCell.style.color = defaultColor;
			    theCell.style.cursor = "";
			    window.status = "";
			  }
			}
			
			function sortTable(theCell, colNum, hdrRows, sortParams){
			  var typnum = !(sortParams & 1);
			  sDir = !(sortParams & 2);
			  var typhtml = !(sortParams & 4);
			  var typfreq = !(sortParams & 8);
			  var tBody = theCell.parentNode;
			  while(tBody.nodeName!="TBODY"){
			    tBody = tBody.parentNode;
			  }
			  var tabOrd = new Array();
			  if(tBody.rows[0].sCol==colNum) sDir = !tBody.rows[0].sDir;
			  if (tBody.rows[0].sCol>=0)
			    tBody.rows[0].cells[tBody.rows[0].sCol].className='';
			  tBody.rows[0].sCol = colNum;
			  tBody.rows[0].sDir = sDir;
			  if (sDir) 
			  	 tBody.rows[0].cells[colNum].className='sortdown'
			  else 
			     tBody.rows[0].cells[colNum].className='sortup';
			  for(i=0,r=hdrRows;r<tBody.rows.length;i++,r++){
			    colCont = tBody.rows[r].cells[colNum].innerHTML;
			    if(typhtml) colCont = colCont.replace(/<[^>]+>/g,'');
			    if(typnum) {
			      colCont*=1;
			      if(isNaN(colCont)) colCont = 0;
			    }
			    if(typfreq) {
					switch(colCont.toLowerCase()) {
						case "always":  { colCont=0; break; }
						case "hourly":  { colCont=1; break; }
						case "daily":   { colCont=2; break; }
						case "weekly":  { colCont=3; break; }
						case "monthly": { colCont=4; break; }
						case "yearly":  { colCont=5; break; }
						case "never":   { colCont=6; break; }
					}
				}
			    tabOrd[i] = [r, tBody.rows[r], colCont];
			  }
			  tabOrd.sort(compRows);
			  for(i=0,r=hdrRows;r<tBody.rows.length;i++,r++){
			    tBody.insertBefore(tabOrd[i][1],tBody.rows[r]);
			  } 
			  window.status = ""; 
			}
			
			function compRows(a, b){
			  if(sDir){
			    if(a[2]>b[2]) return -1;
			    if(a[2]<b[2]) return 1;
			  } else {
			    if(a[2]>b[2]) return 1;
			    if(a[2]<b[2]) return -1;
			  }
			  return 0;
			}

		  ]]>
		</script>
        
      </head>

      <!-- Store in $fileType if we are in a sitemap or in a siteindex -->
      <xsl:variable name="fileType">
        <xsl:choose>
		  <xsl:when test="//sitemap:url">sitemap</xsl:when>
		  <xsl:otherwise>siteindex</xsl:otherwise>
        </xsl:choose>      
      </xsl:variable>            

      <!-- Body -->
      <body onLoad="initXsl('table0','{$fileType}');">  

        <!-- Text and table -->
        <h1 id="head1">XML Sitemap (Sitemaps.org) </h1>        
        <xsl:choose>
	      <xsl:when test="$fileType='sitemap'"><xsl:call-template name="sitemapTable"/></xsl:when>
	      <xsl:otherwise><xsl:call-template name="siteindexTable"/></xsl:otherwise>
  		</xsl:choose>
      </body>
    </html>
  </xsl:template>     

  <!-- siteindexTable template -->
  <xsl:template name="siteindexTable">
    <h2>Number of URLs in this xml sitemap index: <xsl:value-of select="count(sitemap:sitemapindex/sitemap:sitemap)"></xsl:value-of></h2>          
    <p class="sml">Click on the table headers to change sorting.</p>
    <table border="1" width="100%" class="data" id="table0">
      <tr class="header">
        <td>Location</td>
        <td>Priority</td>
        <td>Change Freq</td>
        <td>Last modification date</td>
      </tr>
      <xsl:apply-templates select="sitemap:sitemapindex/sitemap:sitemap">
        <xsl:sort select="sitemap:lastmod" order="descending"/>              
      </xsl:apply-templates>  
    </table>            
  </xsl:template>  
  
  <!-- sitemapTable template -->  
  <xsl:template name="sitemapTable">
    <h2>Number of URLs in this Web Project Mechanics Sitemap: <xsl:value-of select="count(sitemap:urlset/sitemap:url)"></xsl:value-of></h2>          
    <p class="sml">Click on the table headers to change sorting.</p>
    <table border="1" width="100%" class="ewTable" id="table0">
	  <tr class="ewTableHeader">
            <td nowrap="nowrap">Location</td>
            <td nowrap="nowrap">Priority</td>
            <td nowrap="nowrap">Change Freq</td>
            <td nowrap="nowrap">Date Modified</td>
	  </tr>
	  <xsl:apply-templates select="sitemap:urlset/sitemap:url">
	    <xsl:sort select="sitemap:priority" order="descending"/>              
	  </xsl:apply-templates>
	</table>  
  </xsl:template>    
  
  <!-- sitemap:url template -->  
  <xsl:template match="sitemap:url">
    <tr class="ewTableRow">  
      <td nowrap="nowrap" class="ewTableRow"><xsl:variable name="sitemapURL"><xsl:value-of select="sitemap:loc"/></xsl:variable>  
        <a href="{$sitemapURL}" target="_blank" ref="nofollow"><xsl:value-of select="sitemap:loc"></xsl:value-of></a></td>
      <td nowrap="nowrap" class="ewTableRow"><xsl:value-of select="concat(sitemap:priority*100,'%')"/></td>
      <td nowrap="nowrap" class="ewTableRow"><xsl:value-of select="sitemap:changefreq"/></td>
      <td nowrap="nowrap" class="ewTableRow"><xsl:value-of select="sitemap:lastmod"/></td>
    </tr>  
  </xsl:template>
  
  <!-- sitemap:sitemap template -->
  <xsl:template match="sitemap:sitemap">
    <tr>  
      <td>        
        <xsl:variable name="sitemapURL"><xsl:value-of select="sitemap:loc"/></xsl:variable>  
        <a href="{$sitemapURL}"><xsl:value-of select="$sitemapURL"></xsl:value-of></a>
      </td>
      <td><xsl:value-of select="sitemap:lastmod"/></td>
    </tr>  
  </xsl:template>  
</xsl:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="http://localhost/sitemap.xml" htmlbaseurl="" outputurl="" processortype="msxmldotnet2" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator=""/></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->