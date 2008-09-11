<%@ Page Language="VB" MasterPageFile="~/mhwcm.master" Explicit="true" EnableSessionState="True" Debug="true" Strict="false" Trace="true" Title="Test Master" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>
<script runat="server" type="text/VB">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Label1.Text = getSiteMap("http://" & Request.ServerVariables("SERVER_NAME") & "/sitemap.xml")
    End Sub
    Function getSiteMap(ByVal sURL As String) As String
        Dim myXmlDoc As XmlDocument = New XmlDocument()
        Dim strXslFile As String
        Dim myXslDoc As XslCompiledTransform = New XslCompiledTransform()
        Dim myStringBuilder As StringBuilder = New StringBuilder()
        Dim myStringWriter As StringWriter = New StringWriter(myStringBuilder)
        strXslFile = Server.MapPath("style/sitemap.xsl")
        myXmlDoc.Load(sURL)
        myXslDoc.Load(strXslFile)
        myXslDoc.Transform(myXmlDoc, Nothing, myStringWriter)
        myStringBuilder.Append(vbCrLf & "<p><em>Retrieved at: " & Now() & "</em> from " & sURL & "</p>" & vbCrLf)
        Return myStringBuilder.ToString
    End Function
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:literal id="Label1" runat="server" text=""/>
</asp:Content>





