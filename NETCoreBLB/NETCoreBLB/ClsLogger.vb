Imports System.Data
Imports System.IO

Public Class ClsLogger

    Public Enum NivelDetalleLog

        DetalleMaximo = 3
        DetalleNormal = 4
        DetalleBajo = 5

    End Enum

    Public Enum TiposDeLog

        LogDeAlerta = 1
        LogDeError = 2
        LogDetalleMaximo = NivelDetalleLog.DetalleMaximo
        LogDetalleNormal = NivelDetalleLog.DetalleNormal
        LogDetalleBajo = NivelDetalleLog.DetalleBajo

    End Enum

    Public Class ItemsLogueo

        Public IdLog As String = String.Empty
        Public FechaHora As Date
        Public Usuario As String = String.Empty
        Public ComputerName As String = String.Empty
        Public Tipo As TiposDeLog
        Public Descripcion As String = String.Empty
        Public DataSet As DataSet = Nothing
        Public XmlString As String = ""
        Public IsDataSet As Boolean = False
        Public IsXmlString As Boolean = False

    End Class

    Public Class ConfigurarLog

        Public Property NivelDeDetalle As NivelDetalleLog = NivelDetalleLog.DetalleNormal
        Public Property IdLog As String = "SIN-CONFIGURAR-IDLOG"
        Public Property RootFolder As String = "SIN-CONFIGURAR-ROOTFOLDER"
        Public Property EventID As Integer = 0

    End Class

    Public Shared Function Logueo() As Logueo

        Static _Logueo As Logueo

        If _Logueo Is Nothing Then

            _Logueo = New Logueo

            _Logueo.Configurar = New ConfigurarLog

            With _Logueo.Configurar
                .EventID = 4000
                .IdLog = "BLBAutogestion"
                .NivelDeDetalle = NivelDetalleLog.DetalleNormal
                .RootFolder = .IdLog
            End With

        End If

        Return _Logueo

    End Function

End Class

Public Class Logueo

    Private Const Quote As String = """"
    Private Property XMLHeader As String = String.Format("<?xml version={0}1.0{0} encoding={0}UTF-8{0}?> ", Quote)

    Public Function DatasetOneRow(ByVal Tabla As String, ByVal ParamArray Params() As Object) As DataSet

        Dim Ds As New Data.DataSet(Tabla)
        Dim DTable As Data.DataTable = Ds.Tables.Add("Detalle")
        Dim DRow As Data.DataRow

        Dim Orden As Integer = 0
        Dim Campo As String = String.Empty
        Dim Valor As String = String.Empty
        For p As Integer = 0 To Params.Length - 1
            Orden += 1
            Select Case Orden
                Case 1
                    Campo = CStr(Params(p))
                    DTable.Columns.Add(Campo, Type.GetType("System.String"))
                Case 2
                    Orden = 0
            End Select
        Next

        Orden = 0
        Campo = String.Empty
        Valor = String.Empty
        DRow = DTable.NewRow()
        For p As Integer = 0 To Params.Length - 1
            Orden += 1
            Select Case Orden
                Case 1
                    Valor = String.Empty
                    Campo = CStr(Params(p))
                Case 2
                    Valor = CStr(Params(p))
                    DRow(Campo) = Valor
                    Orden = 0
            End Select
        Next
        DTable.Rows.Add(DRow)

        Return Ds

    End Function

    Friend Property Configurar() As ClsLogger.ConfigurarLog

    Friend Function Loguear(ByVal Descripcion As String) As Boolean

        Try

            Dim ItemsLogs As ClsLogger.ItemsLogueo = GetLog(Me.Configurar.IdLog, Descripcion, ClsLogger.TiposDeLog.LogDetalleNormal, Nothing, String.Empty)
            If ItemsLogs Is Nothing Then Return False
            Dim LogFolder As String = GetLogFolder()
            Return LoguearHtml(ItemsLogs, LogFolder)

        Catch ex As Exception
            Throw New Exception("Error en Loguear : " & ex.Message)
        End Try

    End Function

    Friend Function Loguear(ByVal Descripcion As String,
                            ByVal Tipo As ClsLogger.TiposDeLog) As Boolean

        Try

            Dim ItemsLogs As ClsLogger.ItemsLogueo = GetLog(Me.Configurar.IdLog, Descripcion, Tipo, Nothing, String.Empty)
            If ItemsLogs Is Nothing Then Return False
            Dim LogFolder As String = GetLogFolder()
            Return LoguearHtml(ItemsLogs, LogFolder)

        Catch ex As Exception
            Throw New Exception("Error en Loguear : " & ex.Message)
        End Try

    End Function

    Friend Function Loguear(ByVal Descripcion As String,
                            ByVal Tipo As ClsLogger.TiposDeLog,
                            ByVal Ds As DataSet) As Boolean

        Try

            Dim ItemsLogs As ClsLogger.ItemsLogueo = GetLog(Me.Configurar.IdLog, Descripcion, Tipo, Ds)
            If ItemsLogs Is Nothing Then Return False
            Dim LogFolder As String = GetLogFolder()
            Return LoguearHtml(ItemsLogs, LogFolder)

        Catch ex As Exception
            Throw New Exception("Error en Loguear : " & ex.Message)
        End Try

    End Function

    Friend Function Loguear(ByVal Descripcion As String,
                            ByVal Tipo As ClsLogger.TiposDeLog,
                            ByVal Str As String) As Boolean

        Try

            Dim Ds As DataSet = ClsLogger.Logueo.DatasetOneRow("Mensaje", "Mensaje", Str)
            Dim ItemsLogs As ClsLogger.ItemsLogueo = GetLog(Me.Configurar.IdLog, Descripcion, Tipo, Ds)
            If ItemsLogs Is Nothing Then Return False
            Dim LogFolder As String = GetLogFolder()
            Return LoguearHtml(ItemsLogs, LogFolder)

        Catch ex As Exception
            Throw New Exception("Error en Loguear : " & ex.Message)
        End Try

    End Function

    Private Function GetFechaParaArchivo() As String
        Dim d As New DateTime(DateTime.Now.Ticks)
        Return String.Format("{0:0000}_{1:00}_{2:00}", d.Year, d.Month, d.Day)
    End Function

    Private Function GetFechaHoraMilisegundosParaArchivo() As String
        Dim d As New DateTime(DateTime.Now.Ticks)
        Return String.Format("{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}{6:000}", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond)
    End Function

    Private Function GetFechaHoraMilisegundos() As String
        Dim d As New DateTime(DateTime.Now.Ticks)
        Return String.Format("{0:00}/{1:00}/{2:0000} {3}", d.Day, d.Month, d.Year, GetHoraMilisegundos)
    End Function

    Private Function GetHoraMilisegundos() As String
        Dim d As New DateTime(DateTime.Now.Ticks)
        Return String.Format("{0:00}:{1:00}:{2:00}.{3:000}", d.Hour, d.Minute, d.Second, d.Millisecond)
    End Function

    Private Function GetHoraMilisegundos(ByVal Fecha As Date) As String
        Return String.Format("{0:00}:{1:00}:{2:00}.{3:000}", Fecha.Hour, Fecha.Minute, Fecha.Second, Fecha.Millisecond)
    End Function

    Private Function LoguearHtml(ByVal Log As ClsLogger.ItemsLogueo, ByVal LogFolder As String) As Boolean

        Dim sHtml As String = "<html>" & vbNewLine &
                              "<head>" & vbNewLine &
                              "<meta http-equiv=Content-Type content='text/html'; charset=UTF-8>" & vbNewLine &
                              "<meta name=Generator content='Microsoft Word 12 (filtered)'>" & vbNewLine &
                              "</head>" & vbNewLine &
                              "<body lang=ES-AR link=blue vlink=purple>" & vbNewLine &
                              "<p class=MsoNormal>"

        Try

            Static Secuencia As Integer

            Secuencia += 1

            Dim MiColor As System.Drawing.Color = GetColorImagen(Log.Tipo)

            'sHtml = System.String.Replace(sHtml, "'", Chr(34))
            sHtml = sHtml.Replace("'", """")
            Dim StrLine = New String("#", 150)
            Dim xDateTime As String = GetFechaHoraMilisegundosParaArchivo()
            Dim xDate As String = xDateTime.Split("-")(0)
            Dim xTime As String = xDateTime.Split("-")(1)

            Dim FileHtm As String = xDate & ".Htm"
            Dim FileObj As String = xDateTime & "_" & Secuencia.ToString & ".Xml"
            Dim FolderHtm As String
            Dim FolderObj As String
            Dim FolderObjDate As String

            FolderHtm = LogFolder
            FolderObj = Path.GetFullPath(Path.Combine(FolderHtm, "OBJ"))
            FolderObjDate = Path.GetFullPath(Path.Combine(FolderObj, xDate))

            If Not System.IO.Directory.Exists(FolderHtm) Then System.IO.Directory.CreateDirectory(FolderHtm)
            If Not System.IO.Directory.Exists(FolderObj) Then System.IO.Directory.CreateDirectory(FolderObj)
            If Not System.IO.Directory.Exists(FolderObjDate) Then System.IO.Directory.CreateDirectory(FolderObjDate)

            Dim PathFileNameHtm As String = Path.GetFullPath(Path.Combine(FolderHtm, FileHtm))
            Dim PathFileNameObj As String = Path.GetFullPath(Path.Combine(FolderObjDate, FileObj))
            Dim PathFileNameObjRelativo As String = "OBJ\" & xDate & "\" & FileObj

            If Not File.Exists(PathFileNameHtm) Then File.AppendAllText(PathFileNameHtm, sHtml)

            If Log.IsDataSet Or Log.IsXmlString Then
                sHtml = LogLine(GetFechaHoraMilisegundos(), Log.ComputerName, Log.Usuario, Log.Descripcion, MiColor, PathFileNameObjRelativo)
            Else
                sHtml = LogLine(GetFechaHoraMilisegundos(), Log.ComputerName, Log.Usuario, Log.Descripcion, MiColor, String.Empty)
            End If

            File.AppendAllText(PathFileNameHtm, sHtml)

            If Log.IsDataSet Then File.AppendAllText(PathFileNameObj, XMLHeader & Log.DataSet.GetXml)
            If Log.IsXmlString Then File.AppendAllText(PathFileNameObj, Log.XmlString)

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Private Function LogLine(ByVal Fecha As String,
                             ByVal Computadora As String,
                             ByVal Usuario As String,
                             ByVal Descripcion As String,
                             ByVal MiColor As System.Drawing.Color,
                             ByVal Link As String) As String

        Const Quote As String = """"
        Const MyFont As String = "Tahoma" ' "Arial"
        Dim s As String = String.Empty

        If Link.Length > 0 Then

            Dim RefLink As String
            If Descripcion.Length = 0 Then
                RefLink = "<a href=" & Quote & Link & Quote & ">" & "XML Objeto serializado"
            Else
                RefLink = "<a href=" & Quote & Link & Quote & ">" & Descripcion & "</a>"
            End If

            s = String.Format("<span style='color: " & MiColor.Name & "; line-height: 30%; font-color:red;font-size:10.0pt;font-family:" & Quote & MyFont & Quote & "'>" &
                              "<b>[{0}]</b>  {1}" &
                              "</span></p>", Fecha.Split(" ")(1), RefLink)

        Else

            For Each x As String In Descripcion.Split(vbNewLine)

                If s.Length = 0 Then

                    s = String.Format("<span style='color: " & MiColor.Name & "; line-height: 30%; font-color:red;font-size:10.0pt;font-family:" & Quote & MyFont & Quote & "'>" &
                        "<b>[{0}]</b>  {1}" &
                        "</span></p>", Fecha.Split(" ")(1), x)

                Else

                    s += vbNewLine &
                        String.Format("<span style='color: " & MiColor.Name & "; line-height: 30%; font-color:red;font-size:10.0pt;font-family:" & Quote & MyFont & Quote & "'>" &
                        "{0}" &
                        "</span></p>", x)

                End If

            Next

        End If

        Return s

    End Function

    Private Function GetColorImagen(ByVal TipoLog As ClsLogger.TiposDeLog) As System.Drawing.Color

        Select Case TipoLog

            Case ClsLogger.TiposDeLog.LogDeAlerta : Return System.Drawing.Color.Blue
            Case ClsLogger.TiposDeLog.LogDeError : Return System.Drawing.Color.Red
            Case ClsLogger.TiposDeLog.LogDetalleMaximo : Return System.Drawing.Color.Gray
            Case ClsLogger.TiposDeLog.LogDetalleNormal : Return System.Drawing.Color.Black
            Case ClsLogger.TiposDeLog.LogDetalleBajo : Return System.Drawing.Color.Black

        End Select

    End Function

    Private Function GetLog(ByVal IdLog As String,
                            ByVal Descripcion As Object,
                            ByVal Tipo As ClsLogger.TiposDeLog,
                            Optional ByVal Ds As DataSet = Nothing,
                            Optional ByVal XmlString As String = "") As ClsLogger.ItemsLogueo

        Dim _Log As New ClsLogger.ItemsLogueo

        Try

            Select Case Me.Configurar.NivelDeDetalle

                Case ClsLogger.NivelDetalleLog.DetalleNormal
                    If Tipo = ClsLogger.TiposDeLog.LogDetalleMaximo Then Return Nothing

                Case ClsLogger.NivelDetalleLog.DetalleBajo
                    If Tipo = ClsLogger.TiposDeLog.LogDetalleMaximo Or
                       Tipo = ClsLogger.TiposDeLog.LogDetalleNormal Then Return Nothing

            End Select
            _Log.IdLog = IdLog
            _Log.FechaHora = DateTime.Now
            _Log.ComputerName = Environment.MachineName
            _Log.Usuario = Environment.UserName
            _Log.Tipo = Tipo
            _Log.DataSet = Ds
            _Log.XmlString = XmlString
            _Log.Descripcion = Descripcion
            _Log.IsDataSet = (Not Ds Is Nothing)
            If Not _Log.IsDataSet Then _Log.IsXmlString = (XmlString.Trim.Length > 0)

            Return _Log

        Catch ex As Exception

            Return Nothing

        Finally

            _Log = Nothing

        End Try

    End Function

    Public Shared Function GetBaseFolder() As String

        Try

            Return "C:\Users\Public"

        Catch ex As Exception
            Throw New Exception("Error en GetBaseFolder : " & ex.Message)
        End Try

    End Function

    Public Function GetBASCoreFolder()

        Return IO.Path.Combine(Logueo.GetBaseFolder, Configurar.RootFolder)

    End Function

    Public Function GetLogFolder() As String

        Try

            Dim folderROOT As String = GetBASCoreFolder()
            If Not System.IO.Directory.Exists(folderROOT) Then Call System.IO.Directory.CreateDirectory(folderROOT)
            Dim folderLOG As String = System.IO.Path.Combine(folderROOT, "LOG")
            If Not System.IO.Directory.Exists(folderLOG) Then Call System.IO.Directory.CreateDirectory(folderLOG)
            Return folderLOG

        Catch ex As Exception
            Throw New Exception("Error en GetLogFolder : " & ex.Message)
        End Try

    End Function

End Class

