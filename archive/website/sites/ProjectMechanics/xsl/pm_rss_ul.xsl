<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="*">
		<div class="box">
			<h3>
				<a>
					<xsl:attribute name="href">
						<xsl:value-of select="link"/>
					</xsl:attribute>

					<xsl:value-of select="*[local-name()='channel']/*[local-name()='title']"/>
				</a>
			</h3>
			<ul>
				<xsl:for-each select="//*[local-name()='item']">
					<li>
						<a>
							<xsl:attribute name="href">
								<xsl:value-of select="*[local-name()='link']"/>
							</xsl:attribute>
							<xsl:attribute name="target">
								<xsl:text>top</xsl:text>
							</xsl:attribute>
							<xsl:value-of select="*[local-name()='title']"/>
						</a>
						<br/>
						<xsl:value-of select="*[local-name()='description']" disable-output-escaping="yes"/>
					</li>
				</xsl:for-each>
			</ul>
		</div>
		<div class="clear"></div>
	</xsl:template>
	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>
</xsl:stylesheet>