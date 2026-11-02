<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" encoding="utf-8" indent="yes"/>
    
    <xsl:template match="/Dormitory">
        <html>
            <head>
                <title>Гуртожиток</title>
                <style>
                    body { font-family: Segoe UI, Arial, sans-serif; margin: 16px; }
                    table { border-collapse: collapse; }
                    th, td { border: 1px solid #999; padding: 4px 8px; vertical-align: top; }
                    th { background: #e8eef7; }
                    p { margin: 0 0 4px 0; }
                </style>
            </head>
            <body>
                <h2>
                    Мешканці: <xsl:value-of select="@Name"/>
                </h2>
                <p>
                    Усього студентів: <xsl:value-of select="count(Student)"/>
                </p>
                <table>
                    <tr>
                        <th>ПІБ</th>
                        <th>Факультет</th>
                        <th>Спеціальність</th>
                        <th>Курс</th>
                        <th>Додатково</th>
                        <th>Проживання</th>
                    </tr>
                    <xsl:apply-templates select="Student">
                        <xsl:sort select="@FullName"/>
                    </xsl:apply-templates>
                </table>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="Student">
        <tr>
            <td>
                <b>
                    <xsl:value-of select="@FullName"/>
                </b>
            </td>
            <td>
                <xsl:value-of select="@Faculty"/>
            </td>
            <td>
                <xsl:value-of select="@Specialty"/>
            </td>
            <td>
                <xsl:value-of select="@Course"/>
            </td>
            <td>
                <xsl:if test="@Group">
                    <p>
                        Група: <xsl:value-of select="@Group"/>
                    </p>
                </xsl:if>
                <xsl:if test="@Benefit">
                    <p>
                        Пільга: <xsl:value-of select="@Benefit"/>
                    </p>
                </xsl:if>
                <xsl:if test="@Phone">
                    <p>
                        Тел.: <xsl:value-of select="@Phone"/>
                    </p>
                </xsl:if>
            </td>
            <td>
                <xsl:for-each select="Residence">
                    <xsl:sort select="@CheckIn" order="descending"/>
                    <p>
                        Корпус <xsl:value-of select="@Building"/>, кімн. <xsl:value-of select="@Room"/>:
                        <xsl:value-of select="@CheckIn"/> –
                        <xsl:choose>
                            <xsl:when test="@CheckOut">
                                <xsl:value-of select="@CheckOut"/>
                            </xsl:when>
                            <xsl:otherwise>досі</xsl:otherwise>
                        </xsl:choose>
                    </p>
                </xsl:for-each>
            </td>
        </tr>
    </xsl:template>
</xsl:stylesheet>