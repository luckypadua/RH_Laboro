Imports System.Data
Imports NETCoreCrypto

Public Class ClsBASLaboro

    Implements IDisposable, ItzBASLaboro

    Private MiAdo As New NETCoreADO.AdoNet
    Const Semilla As String = "tir4n0sAuri0"

    Public Sub New(ByVal Server As String,
                   ByVal Database As String,
                   Optional ByVal Uid As String = "",
                   Optional ByVal Pwd As String = "")

        MiAdo.Configurar.Server = Server
        MiAdo.Configurar.Database = Database
        MiAdo.Configurar.Uid = Uid
        MiAdo.Configurar.Pwd = Pwd

    End Sub

    Public Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetDatosPersonales

        Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Personas] Where IdPersona = {0}", IdPersona), "Persona")
        Ds.DataSetName = "DatosPersonales"
        Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Legajos] Where IdPersona = {0}", IdPersona), "Legajo", Ds)
        Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Puestos] Where IdPersona = {0}", IdPersona), "Puestos", Ds)
        Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Familiares] Where IdPersona = {0}", IdPersona), "Familiares", Ds)
        Return Ds

    End Function

    Public Function GetRecibos(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetRecibos

        Dim Dt As DataTable = GetParametros()
        Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Recibos] Where IdPersona = {0}", IdPersona), "Recibos")
        Ds.DataSetName = "Recibos"
        Ds.Merge(Dt)

        Return Ds

    End Function

    Private Function GetParametros() As DataTable

        Dim DtRta As New DataTable("FTPParametros")
        DtRta.Columns.Add("Servidor")
        DtRta.Columns.Add("Usuario")
        DtRta.Columns.Add("Contrasenia")
        Dim Dt As DataTable = MiAdo.Consultar.GetDataTable("SELECT [PARAMETRO],[STRVALOR] FROM [dbo].[BL_PARAMETROS] where Parametro Like 'Autogestion\FTP%'", "Parametros")
        If Dt.Rows.Count > 0 Then

            Dim DrRta As DataRow = DtRta.Rows.Add

            For Each Dr As DataRow In Dt.Rows
                Select Case Dr("Parametro").ToString
                    Case "Autogestion\FTP_Servidor"
                        DrRta("Servidor") = ClsCrypto.AES_Encrypt(Dr("STRVALOR").ToString, Semilla)
                    Case "Autogestion\FTP_Usuario"
                        DrRta("Usuario") = ClsCrypto.AES_Encrypt(Dr("STRVALOR").ToString, Semilla)
                    Case "Autogestion\FTP_Contrasenia"
                        DrRta("Contrasenia") = ClsCrypto.AES_Encrypt(Dr("STRVALOR").ToString, Semilla)
                End Select
            Next

        End If

        Return DtRta

    End Function

    Public Function GetLoguinsIni() As DataSet Implements ItzBASLaboro.GetLoguinsIni

        Dim Ds As DataSet = MiAdo.Consultar.GetDataset("Select IdPersona,Usuario,Contrasenia from [vAutogestion_LoguinsIni] Order By IdPersona", "LoguinsIni")
        Ds.DataSetName = "LoguinsIni"
        Return Ds

    End Function

    Public Function ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "") As Boolean Implements ItzBASLaboro.ReciboFirmado

        Try

            If FirmaConforme Then
                MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = 1, Firmado_Fecha = GetDate(), Observacion = Null Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            Else
                MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = 2, Firmado_Fecha = GetDate(), Observacion = '{2}' Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo, Observacion))
            End If
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function ReciboDescargado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As Boolean Implements ItzBASLaboro.ReciboDescargado

        Try

            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set FTPDownLoad = GetDate() Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: elimine el estado administrado (objetos administrados).
                MiAdo.Dispose()
                MiAdo = Nothing
            End If

            ' TODO: libere los recursos no administrados (objetos no administrados) y reemplace Finalize() a continuación.
            ' TODO: configure los campos grandes en nulos.
        End If
        disposedValue = True
    End Sub

    ' TODO: reemplace Finalize() solo si el anterior Dispose(disposing As Boolean) tiene código para liberar recursos no administrados.
    'Protected Overrides Sub Finalize()
    '    ' No cambie este código. Coloque el código de limpieza en el anterior Dispose(disposing As Boolean).
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic agrega este código para implementar correctamente el patrón descartable.
    Public Sub Dispose() Implements IDisposable.Dispose, ItzBASLaboro.Dispose
        ' No cambie este código. Coloque el código de limpieza en el anterior Dispose(disposing As Boolean).
        Dispose(True)
        ' TODO: quite la marca de comentario de la siguiente línea si Finalize() se ha reemplazado antes.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
