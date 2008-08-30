Public Class mhXMLWrite
    Public writer As XmlTextWriter
    Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
        writer = New XmlTextWriter(stream, encoding)
        writer.Formatting = Formatting.Indented
    End Sub
    Public Sub New(ByVal w As System.IO.StreamWriter)
        writer = New XmlTextWriter(w)
        writer.Formatting = Formatting.Indented
    End Sub
    Public Sub WriteStartDocument(ByVal DocumentType As String)
        writer.WriteStartDocument()
        writer.WriteStartElement(DocumentType)
    End Sub
    Public Sub WriteEndDocument()
        writer.WriteEndElement()
        writer.WriteEndDocument()
    End Sub
    Public Sub WriteStartElement(ByVal ElementName As String)
        writer.WriteStartElement(ElementName)
    End Sub
    Public Sub WriteEndElement()
        writer.WriteEndElement()
    End Sub
    Public Sub Close()
        writer.Flush()
        writer.Close()
    End Sub
    Public Sub WriteCDataElement(ByVal sElementName As String, ByRef sData As String)
        writer.WriteStartElement(sElementName)
        writer.WriteCData(Replace(sData, vbCrLf, "<br/>"))
        writer.WriteEndElement()
    End Sub
End Class


