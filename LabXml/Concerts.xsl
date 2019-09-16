<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output
     method="html"></xsl:output>
  <xsl:template match="/">
    <html>
      <body>
        <table border = "1">
          <TR>
            <th>Artist</th>
            <th>Date</th>
            <th>PriceRange</th>
            <th>Location</th>
            <th>Style</th>
            <th>ArtistBand</th>
          </TR>
          <xsl:for-each select = "Concerts/Concert">
            <tr>
              <td>
                <xsl:value-of select = "@Artist"/>
              </td>
              <td>
                <xsl:value-of select = "@Date"/>
              </td>
              <td>
                <xsl:value-of select = "@PriceRange"/>
              </td>
              <td>
                <xsl:value-of select = "@Location"/>
              </td>
              <td>
                <xsl:value-of select = "@Style"/>
              </td>
              <td>
                <xsl:value-of select = "@ArtistBand"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
