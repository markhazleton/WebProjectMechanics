<?xml version="1.0" encoding="iso-8859-1"?>
<Q:stylesheet version="1.0" xmlns:Q="http://www.w3.org/1999/XSL/Transform">
  <Q:output method="html"/>
  <Q:template match="/">
    <Q:variable name="C" select="count(/rss/channel/item)"/>
    <div class="items">
    <dl class="items">
      <Q:if test="$C = 0">
        <dt>(Empty)</dt>
      </Q:if>
      <Q:for-each select="/rss/channel/item">
        <dt>
          <a href="{link}">
            <Q:if test="position() &lt; 6">
              <Q:attribute name="accesskey">
                <Q:value-of select="position()"/>
              </Q:attribute>
            </Q:if>
            <Q:choose>
              <Q:when test="not(title) or title = ''">
                <em>(No title)</em>
              </Q:when>
              <Q:otherwise>
                <Q:value-of select="title"/>
              </Q:otherwise>
            </Q:choose>
          </a>
        </dt>
        <dd>
          <p>
            <Q:value-of disable-output-escaping="yes" select="description"/>
            <Q:value-of disable-output-escaping="yes" select="content"/>
          </p>
        </dd>
      </Q:for-each>
    </dl>
    </div>
  </Q:template>
</Q:stylesheet>