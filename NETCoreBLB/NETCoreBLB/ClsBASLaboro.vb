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


        Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Recibos] Where IdPersona = {0}", IdPersona), "Recibos")
        Ds.DataSetName = "Recibos"

        Return Ds

    End Function

    Public Function GetReciboDescarga(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetReciboDescarga

        Dim Archivo As String = MiAdo.Ejecutar.GetSQLString(String.Format("SELECT PDF_RutaFTP FROM vAutogestion_Recibos WHERE IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
        Dim ArchivoAlias As String = IO.Path.GetFileName(Archivo)
        Dim Dt As DataTable = GetParametros(Archivo, ArchivoAlias)
        Dim Ds As New DataSet("GetReciboDescarga")
        Ds.Merge(Dt)
        Return Ds

    End Function

    Private Function GetParametros(ByVal Archivo As String, ByVal ArchivoAlias As String) As DataTable

        Dim DtRta As New DataTable("GetReciboDescarga")
        DtRta.Columns.Add("Servidor")
        DtRta.Columns.Add("Usuario")
        DtRta.Columns.Add("Contrasenia")
        DtRta.Columns.Add("Archivo")
        DtRta.Columns.Add("ArchivoAlias")

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

            DrRta("Archivo") = Archivo
            DrRta("ArchivoAlias") = ArchivoAlias

        End If

        Return DtRta

    End Function

    Public Function GetLoguinsIni() As DataSet Implements ItzBASLaboro.GetLoguinsIni

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("Select IdPersona,Usuario,Contrasenia from [vAutogestion_LoguinsIni] Order By IdPersona", "LoguinsIni")
            Ds.DataSetName = "LoguinsIni"
            Return Ds
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetLoguinsIni" & ex.Message)
        End Try

    End Function

    Public Function GetManagers(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetManagers

        Try
            Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & IdLegajo)

            With MiAdo.Ejecutar.Parametros
                .Add("IdLegajo", IdLegajo, SqlDbType.Int)
                .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
                .Add("IncluirManagers", 1, SqlDbType.SmallInt)
                .Add("IncluirEmpleadosACargo", 0, SqlDbType.SmallInt)
                .Add("CodEmp", CodEmp, SqlDbType.SmallInt)
            End With

            'Faltaría que el .Ejecutar.Procedimiento pueda devolver un valor distinto a un int.Por eso es rompe. Esto devuelve un Select...
            Dim Ds As DataSet = MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnValue)

            Return Ds

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetManagers" & ex.Message)
        End Try

    End Function

    Public Function GetTipoLicencias() As DataSet Implements ItzBASLaboro.GetTipoLicencias

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT CodSuceso + ' - ' + Descripcion As TipoLicencia, IsNull(HabilitadoSoloManager,0) As HabilitadoSoloManager FROM BL_SUCESOS WHERE HabilitadoAutogestion = 1", "BL_SUCESOS")
            Ds.DataSetName = "TipoLicencias"
            Return Ds

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetTipoLicencias" & ex.Message)
        End Try

    End Function

    Public Function GetLicencias(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetLicencias

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT  ", "BL_SUCESOS")
            Ds.DataSetName = "HistorialLicencias"
            Return Ds

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetTipoLicencias" & ex.Message)
        End Try

    End Function

    Public Sub ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "") Implements ItzBASLaboro.ReciboFirmado

        Try

            'If FirmaConforme Then
            '    MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = 1, Firmado_Fecha = GetDate(), Observacion = Null Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            'Else
            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = 2, Firmado_Fecha = GetDate(), Observacion = '{2}' Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo, Observacion))
            'End If

        Catch ex As Exception

            Throw New Exception("NETCoreBLB:ReciboFirmado: " & ex.Message)

        End Try

    End Sub

    Public Sub ReciboDescargado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) Implements ItzBASLaboro.ReciboDescargado

        Try

            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set FTPDownLoad = GetDate() Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))

        Catch ex As Exception

            Throw New Exception("NETCoreBLB:ReciboDescargado: " & ex.Message)

        End Try

    End Sub

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
