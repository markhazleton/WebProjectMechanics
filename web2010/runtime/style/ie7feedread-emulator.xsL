<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns="http://www.w3.org/1999/xhtml" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:my="http://whatever" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
	<xsl:param name="engine">http://www.w3.org/2005/08/online_xslt/xslt?</xsl:param>
	<xsl:param name="category"></xsl:param>
	<xsl:param name="sortby">date</xsl:param>
	<xsl:param name="date-order">ascending</xsl:param>
	<xsl:param name="title-order">ascending</xsl:param>
	<xsl:param name="xmlfile"></xsl:param>
	<xsl:param name="xslfile"></xsl:param>

	<xsl:template match="/rss/channel">
		<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
			<head>
				<title>IE7 Style RSS rendering</title>
				<base href="http://cse-mjmcl.cse.bris.ac.uk/blog/TagCloudXSLT/"/>
				<link rel="stylesheet" type="text/css" href="ie7.css"/>
			</head>
			<body>
				<div id="container">
					<div id="column1">
						<xsl:choose>
							<xsl:when test="$sortby = 'date'">
								<xsl:choose>
									<xsl:when test="$category">
										<xsl:for-each select="item[category = $category]">
											<xsl:sort select="my:rfc822date2other(pubDate)" order="{$date-order}"/>
											<p>
												<h3 class="itemlink">
													<a>
														<xsl:attribute name="href">
															<xsl:value-of select="link"/>
														</xsl:attribute>
														<xsl:value-of select="title"/>
													</a>
												</h3>
												<div class="pubDate">
													<xsl:value-of select="pubDate"/>
												</div>
												<xsl:value-of select="description"/>
											</p>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="item">
											<xsl:sort select="my:rfc822date2other(pubDate)" order="{$date-order}"/>
											<p>
												<h3 class="itemlink">
													<a>
														<xsl:attribute name="href">
															<xsl:value-of select="link"/>
														</xsl:attribute>
														<xsl:value-of select="title"/>
													</a>
												</h3>
												<div class="pubDate">
													<xsl:value-of select="pubDate"/>
												</div>
												<xsl:value-of select="description"/>
											</p>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$category">
										<xsl:for-each select="item[category = $category]">
											<xsl:sort select="title" order="{$title-order}"/>
											<p>
												<h3 class="itemlink">
													<a>
														<xsl:attribute name="href">
															<xsl:value-of select="link"/>
														</xsl:attribute>
														<xsl:value-of select="title"/>
													</a>
												</h3>
												<div class="pubDate">
													<xsl:value-of select="pubDate"/>
												</div>
												<xsl:value-of select="description"/>
											</p>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="item">
											<xsl:sort select="title" order="{$title-order}"/>
											<p>
												<h3 class="itemlink">
													<a>
														<xsl:attribute name="href">
															<xsl:value-of select="link"/>
														</xsl:attribute>
														<xsl:value-of select="title"/>
													</a>
												</h3>
												<div class="pubDate">
													<xsl:value-of select="pubDate"/>
												</div>
												<xsl:value-of select="description"/>
											</p>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</div>
					<div id="column2">

						<div class="shadow">
							<div class="shadow-content">
								<div class="filter-container" style="margin-bottom: 1em">
									<div class="filter-link">
										<b>Displaying</b>
									</div>
									<div class="count">
										<xsl:choose>
											<xsl:when test="$category">
												<xsl:value-of select="count(item[category = $category])"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="count(item)"/>
											</xsl:otherwise>
										</xsl:choose>/ <xsl:value-of select="count(item)"/></div>
								</div>
								<div class="filter-container">
									<div class="filter-link">
										<xsl:choose>
											<xsl:when test="not($category)">
												<img src="img/right.gif"/>
											</xsl:when>
											<xsl:otherwise>
												<img src="img/empty.gif"/>
											</xsl:otherwise>
										</xsl:choose>
										<a>
											<xsl:attribute name="href">
												<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;date-order=<xsl:value-of select="encode-for-uri($date-order)"/>&amp;title-order=<xsl:value-of select="encode-for-uri($title-order)"/></xsl:attribute>All</a>
									</div>
									<div class="count">
										<xsl:value-of select="count(item)"/>
									</div>
								</div>
								<p class="separator"/>
								<h3>Sort by:</h3>
								<xsl:choose>
									<xsl:when test="$sortby = 'date'">
										<xsl:choose>
											<xsl:when test="$date-order = 'ascending'">
												<img src="img/up.gif"/>
											</xsl:when>
											<xsl:otherwise>
												<img src="img/down.gif"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<img src="img/empty-wide.gif"/>
									</xsl:otherwise>
								</xsl:choose>
								<a>
									<xsl:choose>
										<xsl:when test="$date-order = 'ascending'">
											<xsl:attribute name="href">
												<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;category=<xsl:value-of select="encode-for-uri($category)"/>&amp;date-order=descending&amp;title-order=<xsl:value-of select="encode-for-uri($title-order)"/>&amp;sortby=date</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="href">
												<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;category=<xsl:value-of select="encode-for-uri($category)"/>&amp;date-order=ascending&amp;title-order=<xsl:value-of select="encode-for-uri($title-order)"/>&amp;sortby=date</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>Date</a>
								<br/>
								<xsl:choose>
									<xsl:when test="$sortby = 'title'">
										<xsl:choose>
											<xsl:when test="$title-order = 'ascending'">
												<img src="img/up.gif"/>
											</xsl:when>
											<xsl:otherwise>
												<img src="img/down.gif"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<img src="img/empty-wide.gif"/>
									</xsl:otherwise>
								</xsl:choose>
								<a>
									<xsl:choose>
										<xsl:when test="$title-order = 'ascending'">
											<xsl:attribute name="href">
												<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;category=<xsl:value-of select="encode-for-uri($category)"/>&amp;date-order=<xsl:value-of select="encode-for-uri($date-order)"/>&amp;title-order=descending&amp;sortby=title</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="href">
												<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;category=<xsl:value-of select="encode-for-uri($category)"/>&amp;date-order=<xsl:value-of select="encode-for-uri($date-order)"/>&amp;title-order=ascending&amp;sortby=title</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>Title</a>
								<p class="separator"/>
								<h3>Filter by category:</h3>
								<div id="categories">
									<xsl:for-each-group select="item" group-by="category">
										<xsl:sort select="current-grouping-key()"/>
										<xsl:variable name="count" select="count(current-group())"/>
										<xsl:variable name="current" select="current-grouping-key()"/>
										<div class="filter-container">
											<div class="filter-link">
												<xsl:choose>
													<xsl:when test="$category = $current">
														<img src="img/right.gif"/>
													</xsl:when>
													<xsl:otherwise>
														<img src="img/empty.gif"/>
													</xsl:otherwise>
												</xsl:choose>
												<a>
													<xsl:attribute name="href">
														<xsl:value-of select="$engine"/>xmlfile=<xsl:value-of select="$xmlfile"/>&amp;xslfile=<xsl:value-of select="$xslfile"/>&amp;category=<xsl:value-of select="encode-for-uri($current)"/></xsl:attribute>
													<xsl:value-of select="$current"/>
												</a>
											</div>
											<div class="count">
												<xsl:value-of select="$count"/>
											</div>
										</div>
									</xsl:for-each-group>
								</div>
							</div>
						</div>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

	<xsl:function name="my:rfc822date2other">
		<xsl:param name="rfc822date"/>
		<xsl:analyze-string regex="((Mon|Tue|Wed|Thu|Fri|Sat|Sun), *)?(\d\d?) +(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) +(\d\d(\d\d)?) +(\d\d):(\d\d):(\d\d)? +((\[+-\]?\d\d\d\d)|(UT)|(GMT)|(EST)|(EDT)|(CST)|(CDT)|(MST)|(MDT)|(PST)|(PDT)|\w)"
		                    select="$rfc822date">
			<xsl:matching-substring>
				<xsl:variable name="monthDigit">
					<xsl:value-of select="string-length(substring-before('JanFebMarAprMayJunJulAugSepOctNovDec', regex-group(4))) div 3 + 1"/>
				</xsl:variable>
				<xsl:value-of select="                 concat(                 regex-group(5),                 format-number($monthDigit, '00'),                 format-number(xs:integer(regex-group(3)), '00'),                 regex-group(7),                 regex-group(8),                 regex-group(9)                 )                 "/>
				<!-- "19700101000000" -->
			</xsl:matching-substring>
			<xsl:non-matching-substring>null</xsl:non-matching-substring>
		</xsl:analyze-string>
	</xsl:function>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\App_Data\xml\Star Trek Videos on AOL-1-XML.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="true"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/><advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->