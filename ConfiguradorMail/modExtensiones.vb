Imports System.Xml
Imports System.Runtime.CompilerServices

Public Module Extensiones


    ''' <summary>
    ''' Serializa un objeto a un string XML 
    ''' </summary>
    ''' <param name="Objeto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Serializar(ByVal Objeto As Object) As String

        Dim ser As Serialization.XmlSerializer = New Serialization.XmlSerializer(Objeto.GetType())
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        Try
            Dim writer As New StringWriterWithEncoding(System.Text.Encoding.UTF8, sb)
            ser.Serialize(writer, Objeto)
            Return sb.ToString()
        Catch _ex As Exception
            Throw New Exception(_ex.Message)
        End Try

        Return ""

    End Function

    ''' <summary>
    ''' Deserializa un objeto desde un string
    ''' </summary>
    ''' <param name="Objeto"></param>
    ''' <param name="Tipo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Deserializar(ByVal Objeto As String, ByVal Tipo As System.Type) As Object

        Dim _Obj As New Object
        Dim _rdr As New System.IO.StringReader(Objeto)
        Dim ser As Serialization.XmlSerializer = New System.Xml.Serialization.XmlSerializer(Tipo)

        Try
            _Obj = ser.Deserialize(_rdr)
            Return _Obj
        Catch _ex As Exception
            Throw New Exception(_ex.Message)
        Finally
            If Not _rdr Is Nothing Then _rdr.Close()
        End Try

    End Function

End Module

Public Class StringWriterWithEncoding

    Inherits IO.StringWriter

    Private _encoding As System.Text.Encoding

    Public Sub New(encoding As System.Text.Encoding)
        MyBase.New()
        _encoding = encoding
    End Sub

    Public Sub New(encoding As System.Text.Encoding, formatProvider As IFormatProvider)
        MyBase.New(formatProvider)
        _encoding = encoding
    End Sub

    Public Sub New(encoding As System.Text.Encoding, sb As System.Text.StringBuilder)
        MyBase.New(sb)
        _encoding = encoding
    End Sub

    Public Sub New(encoding As System.Text.Encoding, sb As System.Text.StringBuilder, formatProvider As IFormatProvider)
        MyBase.New(sb, formatProvider)
        _encoding = encoding
    End Sub

    Public Overrides ReadOnly Property Encoding As System.Text.Encoding
        Get
            Return _encoding
        End Get
    End Property

End Class