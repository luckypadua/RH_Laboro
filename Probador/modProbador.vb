Imports System.Xml
Imports System.IO

Module modProbador

    Public Function GetStream(ByVal sXML As String) As Stream

        Dim xmlDoc As New Xml.XmlDocument
        xmlDoc.LoadXml(sXML)
        Dim xmldecl As XmlDeclaration
        xmldecl = xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
        xmldecl.Encoding = "UTF-8"
        xmldecl.Standalone = "yes"
        Dim root As XmlElement = xmlDoc.DocumentElement
        xmlDoc.InsertBefore(xmldecl, root)
        Dim xmlStream = New MemoryStream()
        xmlDoc.Save(xmlStream)
        xmlStream.Flush()
        xmlStream.Position = 0
        Return xmlStream

    End Function

End Module
