Imports System.Data
Imports NETCoreCrypto
Imports System.Globalization
Imports BASCoreLogs.Logger

Public Class ClsBASLaboro

    Implements IDisposable, ItzBASLaboro

    Private MiAdo As New NETCoreADO.AdoNet
    Const Semilla As String = "tir4n0sAuri0"

    Public ReadOnly Property Conexion() As NETCoreADO.AdoNet
        Get
            Return MiAdo
        End Get
    End Property

    Public Sub New(ByVal Server As String,
                   ByVal Database As String,
                   Optional ByVal Uid As String = "",
                   Optional ByVal Pwd As String = "")

        MiAdo.Configurar.Server = Server
        MiAdo.Configurar.Database = Database
        MiAdo.Configurar.Uid = Uid
        MiAdo.Configurar.Pwd = Pwd

        Dim Ds As DataSet = ClsLogger.Logueo.DatasetOneRow("SQLConexion", "Server", Server, "Database", Database)

        ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.New ClsBaslaboro", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)

    End Sub

    Public Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetDatosPersonales

        Try

            Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Personas] Where IdPersona = {0}", IdPersona), "Persona")
            Ds.DataSetName = "DatosPersonales"
            Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Legajos] Where IdPersona = {0}", IdPersona), "Legajo", Ds)
            Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Puestos] Where IdPersona = {0}", IdPersona), "Puestos", Ds)
            Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Familiares] Where IdPersona = {0}", IdPersona), "Familiares", Ds)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetDatosPersonales", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetDatosPersonales", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Public Function GetRecibos(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetRecibos

        Try

            Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from [vAutogestion_Recibos] Where IdPersona = {0}", IdPersona), "Recibos")
            Ds.DataSetName = "Recibos"
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibos", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibos", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Public Function GetRecibosDetalle(ByVal IdPersona As Integer) As String Implements ItzBASLaboro.GetRecibosDetalle

        Try

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

            Dim s As String = Newtonsoft.Json.JsonConvert.SerializeObject(ListaCD)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibosDetalle", ClsLogger.TiposDeLog.LogDetalleNormal, s)
            Return s

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibosDetalle", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Private Function CampoExcluido(ByVal Campo As String) As Boolean

        Dim CamposExcluidos As String = "Clave,IdLiquidacion,IdLegajo,IdPersona"
        For Each C As String In CamposExcluidos.Split(",")
            If C.ToUpper = Campo.ToUpper Then Return True
        Next
        Return False

    End Function

    Public Function GetReciboDescarga(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetReciboDescarga

        Try

            Dim Archivo As String = MiAdo.Ejecutar.GetSQLString(String.Format("SELECT PDF_RutaFTP FROM vAutogestion_Recibos WHERE IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            Dim ArchivoAlias As String = IO.Path.GetFileName(Archivo)
            Dim Dt As DataTable = GetParametros(Archivo, ArchivoAlias)
            Dim Ds As New DataSet("GetReciboDescarga")
            Ds.Merge(Dt)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetReciboDescarga", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetReciboDescarga", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Public Function GetBASLaboroVersion() As DataSet Implements ItzBASLaboro.GetBASLaboroVersion

        Try

            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("select top 1 BASLaboroVersion = isNull(STRVALOR,'')  from BL_PARAMETROS where Parametro = 'version'", "BASLaboroVersion")
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetBASLaboroVersion", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetBASLaboroVersion", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Private Function GetParametros(ByVal Archivo As String, ByVal ArchivoAlias As String) As DataTable

        Try

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
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetParametros.", ClsLogger.TiposDeLog.LogDetalleNormal, DtRta.DataSet)
            Return DtRta

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetParametros", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Public Function ValidarSolicitudLicencia(ByRef PedidoLic As ClsPedidoLicencia) As Boolean Implements ItzBASLaboro.ValidarSolicitudLicencia
        Try
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.ValidarSolicitudLicencia.")
            Return PedidoLic.Validar
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.ValidarSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function EliminarSolicitudLicencia(ByVal IdPedidoLicencia As Long) As Boolean Implements ItzBASLaboro.EliminarSolicitudLicencia
        Try
            Dim sLic As New ClsPedidoLicencia(IdPedidoLicencia, MiAdo)
            sLic.Borrar()
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.EliminarSolicitudLicencia.  IdPedidoLicencia = {0}", IdPedidoLicencia))
            Return True
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.EliminarSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function AceptarSolicitudLicencia(ByVal IdPedidoLicencia As Long, ByVal IdManager As Long, ByVal Observaciones As String) As Boolean Implements ItzBASLaboro.AceptarSolicitudLicencia
        Try
            Dim sLic As New ClsPedidoLicencia(IdPedidoLicencia, MiAdo)
            With sLic
                .Estado = ClsPedidoLicencia.eEstadoPedidoLic.Autorizada
                .IdAutorizadoPor = IdManager
                .ObservacionesManager = Observaciones
                .Grabar()
            End With
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.AceptarSolicitudLicencia.  IdPedidoLicencia = {0}  IdManager = {1}", IdPedidoLicencia, IdManager))
            Return True
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.AceptarSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function RechazarSolicitudLicencia(ByVal IdPedidoLicencia As Long, ByVal IdManager As Long, ByVal Observaciones As String) As Boolean Implements ItzBASLaboro.RechazarSolicitudLicencia
        Try
            Dim sLic As New ClsPedidoLicencia(IdPedidoLicencia, MiAdo)
            With sLic
                .Estado = ClsPedidoLicencia.eEstadoPedidoLic.NoAutorizada
                .IdAutorizadoPor = IdManager
                .ObservacionesManager = Observaciones
                .Grabar()
            End With
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.RechazarSolicitudLicencia.  IdPedidoLicencia = {0}  IdManager = {1}", IdPedidoLicencia, IdManager))
            Return True
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.RechazarSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function GrabarSolicitudLicencia(ByRef PedidoLicencia As ClsPedidoLicencia) As Boolean Implements ItzBASLaboro.GrabarSolicitudLicencia
        Try
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GrabarSolicitudLicencia.   PedidoLicencia = {0}", PedidoLicencia))
            PedidoLicencia.Grabar()
            Return True
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GrabarSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function GrabarSolicitudVacaciones(ByRef PedidoLicencia As ClsPedidoLicencia) As Boolean Implements ItzBASLaboro.GrabarSolicitudVacaciones

        Try
            Dim SucesoVacaciones As Long = MiAdo.Ejecutar.GetSQLInteger("SELECT IdSuceso FROM Bl_Sucesos WHERE HabilitadoAutogestion = 1 AND IsNull(HabilitadoSoloManager,0) = 0 AND EsVacacion = 1")
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GrabarSolicitudVacaciones")
            If SucesoVacaciones = 0 Then
                Throw New Exception("@No existe un Suceso de Vacaciones configurado para Autogestión. No es posible solicitar vacaciones.")
            Else
                PedidoLicencia.IdSuceso = SucesoVacaciones
                Return GrabarSolicitudLicencia(PedidoLicencia)
            End If
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GrabarSolicitudVacaciones", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetSolicitudesLicencias(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetSolicitudesLicencias

        Try
            Dim DS As DataSet = ObtenerPedidosDeLicencias(IdLegajo, 0)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GetSolicitudesLicencias   IdLegajo = {0}", IdLegajo), ClsLogger.TiposDeLog.LogDetalleNormal, DS)
            Return DS
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSolicitudesLicencias", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Private Function ObtenerPedidosDeLicencias(ByVal IdLegajo As Long, ByVal EsVacacion As Byte) As DataSet
        Dim DS As DataSet = MiAdo.Consultar.GetDataset("SELECT np.IdOcurrenciaPedido, np.IdLegajo, l.Legajo, p.Nombre, FecSolicitud, np.IdSuceso, ISNULL(AliasAutogestion,CodSuceso + ' - ' + Descripcion) As TipoLicencia, FecDesde, FecHasta, Cantidad, Estado, np.Observaciones, np.ObservacionesManager, IsNull(np.ObservacionesAdmin, IsNull(n.Observaciones,'')) AS ObservacionesAdmin,  s.EsVacacion, n.LicFecDesde, n.LicFecHasta, Convert(Int,IsNull(nl.SancionDias, nl.DiasGozados)) AS CantidadNovedad FROM BL_NovedadesPedidos np JOIN Bl_Sucesos s ON np.IdSuceso = s.IdSuceso JOIN Bl_Legajos l ON np.IdLegajo = l.IdLegajo JOIN Bl_Personas p ON l.IdPersona = p.IdPersona LEFT JOIN Bl_Novedades n ON np.IdOcurrenciaPedido = n.IdOcurrenciaPedido LEFT JOIN Bl_NovedadesLegajos nl ON n.IdOcurrencia = nl.IdOcurrencia WHERE s.EsVacacion = " & EsVacacion & " AND np.IdLegajo = " & IdLegajo, "BL_NovedadesPedidos")
        DS.DataSetName = "PedidosDeLicencias"
        ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.ObtenerPedidosDeLicencias  IdLegajo = {0}", IdLegajo), ClsLogger.TiposDeLog.LogDetalleNormal, DS)
        Return DS
    End Function

    Public Function GetSolicitudVacaciones(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetSolicitudVacaciones

        Try
            Dim DS As DataSet = ObtenerPedidosDeLicencias(IdLegajo, 1)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GetSolicitudVacaciones  IdLegajo = {0}", IdLegajo), ClsLogger.TiposDeLog.LogDetalleNormal, DS)
            Return DS
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSolicitudVacaciones", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetSolicitudesLicenciasManager(ByVal IdPersonaManager As Long, ByVal TraerPendientes As Boolean) As DataSet Implements ItzBASLaboro.GetSolicitudesLicenciasManager

        Try
            Dim sIds As String = ""
            Dim WhereEstado As String
            Dim DS As DataSet = New DataSet

            If TraerPendientes = True Then
                WhereEstado = " Estado = " & ClsPedidoLicencia.eEstadoPedidoLic.Pendiente
            Else
                WhereEstado = " Estado <> " & ClsPedidoLicencia.eEstadoPedidoLic.Pendiente
            End If

            For Each Dr As DataRow In GetEmpleadosACargo(IdPersonaManager).Tables(0).Rows
                sIds = sIds & Dr("IdLegajo").ToString & ","
            Next

            If sIds.Length > 0 Then
                sIds = sIds.Remove(sIds.Length - 1)

                DS = MiAdo.Consultar.GetDataset("SELECT IdOcurrenciaPedido, np.IdLegajo, l.Legajo, p.Nombre, FecSolicitud, np.IdSuceso, ISNULL(AliasAutogestion,CodSuceso + ' - ' + Descripcion) As TipoLicencia, FecDesde, FecHasta, Cantidad, Estado, np.Observaciones, np.ObservacionesManager, np.ObservacionesAdmin FROM BL_NovedadesPedidos np JOIN Bl_Sucesos s ON np.IdSuceso = s.IdSuceso JOIN Bl_Legajos l ON np.IdLegajo = l.IdLegajo JOIN Bl_Personas p ON l.IdPersona = p.IdPersona WHERE np.IdLegajo In (" & sIds & ") AND " & WhereEstado, "BL_NovedadesPedidos")
            End If

            DS.DataSetName = "PedidosDeLicencias"
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSolicitudesLicenciasManager", ClsLogger.TiposDeLog.LogDetalleNormal, DS)
            Return DS
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSolicitudesLicenciasManager", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetEmpleadosACargo(ByVal IdPersona As Long) As DataSet Implements ItzBASLaboro.GetEmpleadosACargo

        Try
            'Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & IdLegajo)

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("IdPersona", IdPersona, SqlDbType.Int)
                .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
                .Add("IncluirManagers", 0, SqlDbType.SmallInt)
                .Add("IncluirEmpleadosACargo", 1, SqlDbType.SmallInt)
                '.Add("CodEmp", CodEmp, SqlDbType.SmallInt)
            End With

            Dim Ds As DataSet = MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetEmpleadosACargo", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetEmpleadosACargo", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function IsManager(ByVal IdPersona As Long) As Boolean Implements ItzBASLaboro.IsManager

        Try

            IsManager = False

            If GetEmpleadosACargo(IdPersona).Tables(0).Rows.Count > 0 Then
                ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.IsManager = True  IdPersona = {0}", IdPersona), ClsLogger.TiposDeLog.LogDetalleNormal)
                IsManager = True
                Exit Function
            End If
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.IsManager = False  IdPersona = {0}", IdPersona), ClsLogger.TiposDeLog.LogDetalleNormal)

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.IsManager", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function
    Public Function GetManagers(ByVal IdPersona As Long, Optional ByVal CodEmp As Integer = -1) As DataSet Implements ItzBASLaboro.GetManagers

        Try
            'Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & IdLegajo)

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("IdPersona", IdPersona, SqlDbType.Int)
                .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
                .Add("IncluirManagers", 1, SqlDbType.SmallInt)
                .Add("IncluirEmpleadosACargo", 0, SqlDbType.SmallInt)
                If CodEmp <> -1 Then .Add("CodEmp", CodEmp, SqlDbType.SmallInt)
            End With

            Dim Ds As DataSet = MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GetManagers  IdPersona = {0}", IdPersona), ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetManagers", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetTipoLicencias() As DataSet Implements ItzBASLaboro.GetTipoLicencias

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT IdSuceso, ISNULL(AliasAutogestion,CodSuceso + ' - ' + Descripcion) As TipoLicencia, IsNull(HabilitadoSoloManager,0) As HabilitadoSoloManager FROM BL_SUCESOS WHERE HabilitadoAutogestion = 1 AND EsVacacion = 0", "BL_SUCESOS")
            Ds.DataSetName = "TipoLicencias"
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetTipoLicencias", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetTipoLicencias", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
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
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetLicencias", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetLicencias", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Sub ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "") Implements ItzBASLaboro.ReciboFirmado

        Try

            Dim Firmado As Integer = 1
            If Not FirmaConforme Then Firmado = 2
            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Firmado = {3}, Firmado_Fecha = GetDate(), Observacion = '{2}' Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo, Observacion, Firmado))

            Dim IdAccion As Integer
            If Firmado = 1 Then
                IdAccion = 5
            Else
                IdAccion = 6
            End If

            MiAdo.Ejecutar.Parametros.RemoveAll()
            MiAdo.Ejecutar.Parametros.Add("IdLiquidacion", IdLiquidacion, SqlDbType.Int)
            MiAdo.Ejecutar.Parametros.Add("IdLegajo", IdLegajo, SqlDbType.Int)
            MiAdo.Ejecutar.Parametros.Add("IdAccion", IdAccion, SqlDbType.Int)
            MiAdo.Ejecutar.Parametros.Add("Firmado_Fecha", DateTime.Now, SqlDbType.DateTime)
            MiAdo.Ejecutar.Insertar("BL_RECIBOS_AUDITORIAWEB")

            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.ReciboFirmado. IdLegajo = {0}  IdLiquidacion = {1}", IdLegajo, IdLiquidacion))

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.ReciboFirmado", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Sub
    Public Sub ReciboVisualizado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) Implements ItzBASLaboro.ReciboVisualizado

        Try

            MiAdo.Ejecutar.Instruccion(String.Format("Update BL_RECIBOS Set Visualizado = GetDate() Where IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.ReciboVisualizado.  IdLegajo = {0}  IdLiquidacion = {1}", IdLegajo, IdLiquidacion))

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.ReciboVisualizado", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Sub

    Public Function GetVacaciones(ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetVacaciones

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT Anio, Case When DiasAcr < DiasAsig Then DiasAsig Else DiasAcr End As DiasAcreditados, DiasGozados FROM BL_LEGAJOSVACS WHERE IdLegajo = " & IdLegajo & " UNION ALL SELECT 0 As Anio, SUM(Case When DiasAcr < DiasAsig Then DiasAsig Else DiasAcr End) As DiasAcreditados, SUM(DiasGozados) FROM BL_LEGAJOSVACS WHERE IdLegajo = " & IdLegajo & "GROUP BY IdLegajo ORDER BY Anio DESC", "BL_LEGAJOSVACS")
            Ds.DataSetName = "GrillaVacaciones"
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetVacaciones", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetVacaciones", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetVacacionesDetalle(ByVal IdLegajo As Long, Optional ByVal AnioDesde As Integer = 0) As DataSet Implements ItzBASLaboro.GetVacacionesDetalle

        Try
            Dim SucesoVacaciones As Long = MiAdo.Ejecutar.GetSQLInteger("SELECT IdSuceso FROM Bl_Sucesos WHERE HabilitadoAutogestion = 1 AND HabilitadoSoloManager = 0 AND EsVacacion = 1")

            If AnioDesde = 0 Then AnioDesde = Date.Today.Year

            Dim DS As DataSet = MiAdo.Consultar.GetDataset("SELECT nl.IdLegajo, Year(LicFecDesde) As Anio, Month(LicFecDesde) As Mes, LicFecDesde, LicFecHasta, DiasGozados 
                                    FROM Bl_Novedades n
                                    JOIN Bl_NovedadesLegajos nl ON n.IdOcurrencia = nl.IdOcurrencia AND nl.Idlegajo = " & IdLegajo & "
                                    WHERE n.IdSuceso = " & SucesoVacaciones & " AND Year(LicFecDesde) >= " & AnioDesde & "
                                    ORDER BY LicFecDesde DESC", "HistorialVacaciones",)

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetVacacionesDetalle", ClsLogger.TiposDeLog.LogDetalleNormal, DS)
            Return DS
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetVacacionesDetalle", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetCambiosEnUsuarios(Optional ByVal EsConfiguracionInicial As Boolean = False) As DataSet Implements ItzBASLaboro.GetCambiosEnUsuarios

        Try
            'Si llama a la "Configuración Inicial" hay que hacer un Delete a Bl_AutogestionCambiosPer antes de hacer nada
            If EsConfiguracionInicial Then
                MiAdo.Ejecutar.Borrar("Bl_AutogestionCambiosPer")
            End If

            Dim ProxId As Long = MiAdo.Ejecutar.GetSQLInteger("SELECT MAX(IdCambios)+1 FROM Bl_AutogestionCambiosPerPendientes")

            'Las Altas son las que no están en la Tabla de cambios, o sea, nunca se incorporaron todavía, y además están habilitados para autogestión
            Dim DS As DataSet = MiAdo.Consultar.GetDataset("
                    SELECT " & ProxId & " AS IdCambios, Nombre = p.Nombre, Usuario = p.EmailPersonal, Contrasenia = p.CUIL, IdPersona, p.EmailPersonal As Email,                                                            CASE WHEN p.HabilitadoAutogestion = 1 THEN 1 ELSE 0 END As Habilitado, Tipo = 'A'
                    FROM Bl_Personas p
                    WHERE p.HabilitadoAutogestion = 1 And p.IdPersona Not IN (Select IdPersona FROM Bl_AutogestionCambiosPer) AND p.EmailPersonal IS NOT NULL AND RTRIM(p.EmailPersonal) <> ''
                    UNION ALL
                    SELECT " & ProxId & " AS IdCambios, Nombre = p.Nombre, Usuario = p.EmailPersonal, Contrasenia = NULL, p.IdPersona, p.EmailPersonal As Email, CASE WHEN p.HabilitadoAutogestion = 1 THEN 1 ELSE 0 END As Habilitado, Tipo = 'M'
                    FROM Bl_AutogestionCambiosPer pa
                    JOIN Bl_Personas p ON pa.IdPersona = p.IdPersona
                    WHERE (p.EmailPersonal <> pa.Email Or p.HabilitadoAutogestion <> pa.HabilitadoAutogestion) AND p.EmailPersonal IS NOT NULL AND RTRIM(p.EmailPersonal) <> ''", "Bl_Personas")

            For Each Row As DataRow In DS.Tables(0).Rows
                MiAdo.Ejecutar.Instruccion("INSERT INTO Bl_AutogestionCambiosPerPendientes (IdCambios, IdPersona, Tipo, FecOcurrencia, Email, HabilitadoAutogestion) VALUES (" & ProxId & "," & Row("IdPersona").ToString & ",'" & Row("Tipo").ToString & "','" & Date.Today.ToString("yyyyMMdd") & "','" & Row("Email").ToString & "'," & Row("Habilitado").ToString & ")")
            Next
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetCambiosEnUsuarios", ClsLogger.TiposDeLog.LogDetalleNormal, DS)
            Return DS
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetCambiosEnUsuarios", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Sub OkCambiosUsuarios(IdCambios As Long) Implements ItzBASLaboro.OkCambiosUsuarios

        Try
            Dim DS As DataSet = MiAdo.Consultar.GetDataset("SELECT IdPersona, FecOcurrencia, Email, CASE HabilitadoAutogestion WHEN 1 THEN 1 ELSE 0 END AS HabilitadoAutogestion, Tipo FROM Bl_AutogestionCambiosPerPendientes WHERE IdCambios = " & IdCambios, "Pendientes")

            For Each Row As DataRow In DS.Tables(0).Rows
                If Row("Tipo").ToString = "A" Then
                    MiAdo.Ejecutar.Instruccion("INSERT INTO Bl_AutogestionCambiosPer (IdPersona, FecOcurrencia, Email, HabilitadoAutogestion) VALUES (" & Row("IdPersona").ToString & ",'" & Convert.ToDateTime(Row("FecOcurrencia")).ToString("yyyyMMdd") & "','" & Row("Email").ToString & "'," & Row("HabilitadoAutogestion").ToString & ")")
                Else
                    MiAdo.Ejecutar.Instruccion("UPDATE Bl_AutogestionCambiosPer SET Email = '" & Row("Email").ToString & "', HabilitadoAutogestion = " & Row("HabilitadoAutogestion").ToString & " WHERE IdPersona = " & Row("IdPersona").ToString)
                End If
            Next

            MiAdo.Ejecutar.Borrar("Bl_AutogestionCambiosPerPendientes", " IdCambios = " & IdCambios)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.OkCambiosUsuarios   IdCambios = {0}", IdCambios))

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.OkCambiosUsuarios", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Sub

    Public Sub BorrarCambiosUsuariosPendientes(Optional ByVal IdCambios As Long = 0) Implements ItzBASLaboro.BorrarCambiosUsuariosPendientes

        Try
            If IdCambios <> 0 Then
                MiAdo.Ejecutar.Borrar("Bl_AutogestionCambiosPerPendientes", " IdCambios = " & IdCambios)
            Else
                MiAdo.Ejecutar.Borrar("Bl_AutogestionCambiosPerPendientes", "")
            End If
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.BorrarCambiosUsuariosPendientes   IdCambios = {0}", IdCambios))
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.BorrarCambiosUsuariosPendientes", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Sub

    Public Function GetSucesoDeVacaciones() As Long Implements ItzBASLaboro.GetSucesoDeVacaciones

        Try
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSucesoDeVacaciones")
            Return MiAdo.Ejecutar.GetSQLInteger("SELECT IdSuceso FROM Bl_Sucesos WHERE EsVacacion = 1 AND HabilitadoAutogestion = 1")
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetSucesoDeVacaciones", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetNuevoPedidoLicencia(ByVal IdPedidoLicencia As Long) As ClsPedidoLicencia Implements ItzBASLaboro.GetNuevoPedidoLicencia
        Try
            Dim mNuevoPedidoLic As New ClsPedidoLicencia(IdPedidoLicencia, MiAdo)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GetNuevoPedidoLicencia   IdPedidoLicencia = {0}", IdPedidoLicencia))
            Return mNuevoPedidoLic
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetNuevoPedidoLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try
    End Function

    Public Function GetNuevoPedidoLicencia(ByVal IdLegajo As Long, ByVal IdSuceso As Long, ByVal FecSolicitud As DateTime, ByVal FecDesde As DateTime, ByVal FecHasta As DateTime, ByVal CantDias As Integer, ByVal Observaciones As String) As ClsPedidoLicencia Implements ItzBASLaboro.GetNuevoPedidoLicencia
        Try

            Dim mNuevoPedidoLic As New ClsPedidoLicencia(IdLegajo, IdSuceso, FecSolicitud, FecDesde, FecHasta, CantDias, Observaciones, MiAdo)
            ClsLogger.Logueo.Loguear(String.Format("NETCoreBLB.ClsBASLaboro.GetNuevoPedidoLicencia   IdLegajo = {0}   IdSuceso = {1}   FecDesde = {2}   FecHasta = {3}   CantDias = {4}", IdLegajo, IdSuceso, FecDesde, FecHasta, CantDias))
            Return mNuevoPedidoLic
        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetNuevoPedidoLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
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