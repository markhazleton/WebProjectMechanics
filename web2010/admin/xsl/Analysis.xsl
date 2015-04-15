<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://projectmechanics.com" >
<xsl:output omit-xml-declaration="yes" method="html" />

	<xsl:template match="/" >
		<ul>
			<xsl:for-each select="a:Company/a:Locations/a:Location">
				<li>
					<xsl:value-of select="a:LocationName"/>
				</li>
			</xsl:for-each>
		</ul>
	</xsl:template>
</xsl:stylesheet>