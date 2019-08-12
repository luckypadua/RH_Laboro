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

    Public Function GetRecibosDetalle(ByVal IdPersona As Integer) As String Implements ItzBASLaboro.GetRecibosDetalle

        Dim ListaCD As New List(Of ClsCamposDetalle)
        Dim Dt As DataTable = MiAdo.Consultar.GetDataTable(String.Format("Select * from [vAutogestion_RecibosDetalle] Where IdPersona = {0}", IdPersona), "RecibosDetalle")
        For Each Dr As DataRow In Dt.Rows
            Dim ListaCampos As New List(Of ClsCampos)
            For Each Col As DataColumn In Dt.Columns
                If Not CampoExcluido(Col.ColumnName) Then
                    'Or Dr(Col.ColumnName) is DBNull.Value
                    If Dr(Col.ColumnName) Is Nothing Or Dr(Col.ColumnName) Is DBNull.Value Then
                        ListaCampos.Add(New ClsCampos(Col.ColumnName, String.Empty))
                    Else
                        ListaCampos.Add(New ClsCampos(Col.ColumnName, Dr(Col.ColumnName)))
                    End If

                End If
            Next
            Dim CD As New ClsCamposDetalle(Dr("Clave"), ListaCampos)
            ListaCD.Add(CD)
        Next

        Return Newtonsoft.Json.JsonConvert.SerializeObject(ListaCD)

    End Function

    Private Function CampoExcluido(ByVal Campo As String) As Boolean

        Dim CamposExcluidos As String = "Clave,IdLiquidacion,IdLegajo,IdPersona"
        For Each C As String In CamposExcluidos.Split(",")
            If C.ToUpper = Campo.ToUpper Then Return True
        Next
        Return False

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
            Throw New ArgumentException("NETCoreBLB:GetLoguinsIni " & ex.Message)
        End Try

    End Function

    Public Function ValidarSolicitudLicencia(ByRef PedidoLic As clsPedidoLicencia) As Boolean Implements ItzBASLaboro.ValidarSolicitudLicencia
        Try
            Return PedidoLic.Validar
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsBASLaboro:ValidarSolicitudLicencia " & ex.Message)
        End Try
    End Function

    Public Function EliminarSolicitudLicencia(ByVal IdPedidoLicencia As Long) As Boolean Implements ItzBASLaboro.EliminarSolicitudLicencia
        Try
            Dim sLic As New ClsPedidoLicencia(IdPedidoLicencia)
            sLic.Borrar()
            Return True
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsBASLaboro:EliminarSolicitudLicencia " & ex.Message)
            Return False
        End Try
    End Function

    Public Function GrabarSolicitudLicencia(ByRef PedidoLicencia As clsPedidoLicencia) As Boolean Implements ItzBASLaboro.GrabarSolicitudLicencia
        Try
            PedidoLicencia.Grabar()
            Return True
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsBASLaboro:GrabarSolicitudLicencia " & ex.Message)
            Return False
        End Try
    End Function
    Public Function GetSolicitudesLicencias(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetSolicitudesLicencias

        Try
            Dim DS As DataSet = MiAdo.Consultar.GetDataset("SELECT IdOcurrenciaPedido, IdLegajo, FecSolicitud, IdSuceso, FecDesde, FecHasta, Cantidad, Estado, Observaciones FROM BL_NovedadesPedidos WHERE IdLegajo = " & IdLegajo, "BL_NovedadesPedidos")
            DS.DataSetName = "PedidosDeLicencias"
            Return DS

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsBASLaboro:GetSolicitudesLicencias " & ex.Message)
        End Try
    End Function
    Public Function GetEmpleadosACargo(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetEmpleadosACargo

        Try
            Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & IdLegajo)

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("IdLegajo", IdLegajo, SqlDbType.Int)
                .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
                .Add("IncluirManagers", 0, SqlDbType.SmallInt)
                .Add("IncluirEmpleadosACargo", 1, SqlDbType.SmallInt)
                .Add("CodEmp", CodEmp, SqlDbType.SmallInt)
            End With

            Return MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetEmpleadosACargo " & ex.Message)
        End Try

    End Function

    Public Function GetManagers(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetManagers

        Try
            Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & IdLegajo)

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("IdLegajo", IdLegajo, SqlDbType.Int)
                .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
                .Add("IncluirManagers", 1, SqlDbType.SmallInt)
                .Add("IncluirEmpleadosACargo", 0, SqlDbType.SmallInt)
                .Add("CodEmp", CodEmp, SqlDbType.SmallInt)
            End With

            Return MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetManagers " & ex.Message)
        End Try

    End Function

    Public Function GetTipoLicencias() As DataSet Implements ItzBASLaboro.GetTipoLicencias

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT IdSuceso, CodSuceso + ' - ' + Descripcion As TipoLicencia, IsNull(HabilitadoSoloManager,0) As HabilitadoSoloManager FROM BL_SUCESOS WHERE HabilitadoAutogestion = 1", "BL_SUCESOS")
            Ds.DataSetName = "TipoLicencias"
            Return Ds

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetTipoLicencias " & ex.Message)
        End Try

    End Function

    Public Function GetLicencias(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetLicencias

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT s.CodSuceso + ' - ' + s.Descripcion AS Tipo, FecOcurrencia As FechaDeOcurrencia, 
		                                                           LicFecDesde As FechaDesde, LicFecHasta As FechaHasta,
		                                                           CASE WHEN s.EsVacacion = 1 AND s.AfectaLiquidadas = 1 THEN nl.SancionDias
                                                                        ELSE nl.DiasGozados
		                                                           END AS Días			
                                                            FROM Bl_Novedades n 
                                                            JOIN Bl_NovedadesLegajos nl ON n.IdOcurrencia = nl.IdOcurrencia AND nl.IdLegajo = " & IdLegajo & "
                                                            JOIN Bl_Sucesos s ON n.IdSuceso = s.IdSuceso AND s.HabilitadoAutogestion = 1", "HistorialLicencias")
            Ds.DataSetName = "HistorialLicencias"
            Return Ds

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:GetTipoLicencias " & ex.Message)
        End Try

    End Function

    Public Sub ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "") Implements ItzBASLaboro.ReciboFirmado

        Try

            Dim Firmado As Integer = 1
            If Not FirmaConforme Then Firmado = 2
            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = {3}, Firmado_Fecha = GetDate(), Observacion = '{2}' Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo, Observacion, Firmado))

        Catch ex As Exception

            Throw New Exception("NETCoreBLB:ReciboFirmado: " & ex.Message)

        End Try

    End Sub

    Public Sub ReciboVisualizado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) Implements ItzBASLaboro.ReciboVisualizado

        Try

            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Visualizado = GetDate() Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))

        Catch ex As Exception

            Throw New Exception("NETCoreBLB:ReciboVisualizado: " & ex.Message)

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

Public Class ClsCamposDetalle

    Public Sub New()

    End Sub

    Public Sub New(ByVal Clave As String,
                   ByVal ListaCampos As List(Of ClsCampos))

        Me.Clave = Clave
        Me.ListaCampos = ListaCampos

    End Sub

    Property Clave As String
    Property ListaCampos As List(Of ClsCampos)

End Class

Public Class ClsCampos

    Public Sub New()

    End Sub

    Public Sub New(ByVal Nombre As String,
                   ByVal Valor As String)

        Me.Nombre = Nombre
        Me.Valor = Valor

    End Sub

    Public Property Nombre As String
    Public Property Valor As String

End Class