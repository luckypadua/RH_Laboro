Imports System.Data
Imports NETCoreCrypto
Imports NETCoreLOG
Imports NETCoreBLB.Extensiones

Public Class ClsBASLaboro

    Implements IDisposable, ItzBASLaboro

    Private WithEvents _TimerUnoPorDia_8Hs As New clsTimerEvent
    Private MiAdo As New NETCoreADO.AdoNet
    Public Const Semilla As String = "tir4n0sAuri0"
    Private Fmt As ClsFormatoHtml

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
        Call HorariosMails()
        Dim Ds As DataSet = ClsLogger.Logueo.DatasetOneRow("SQLConexion", "Server", Server, "Database", Database)
        ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.New ClsBaslaboro", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)

        Dim Cfg As New MailCfg
        Cfg.Leer()
        Fmt = New ClsFormatoHtml(Cfg.SufijoURL)
        Cfg = Nothing

        If Not ClsFormatoHtml.ExisteCarpetaFormatos Then
            ClsLogger.Logueo.Loguear($"NETCoreBLB.ClsBASLaboro.New ATENCION: No existe la carpeta de formatos {ClsFormatoHtml.CarpetaFormatos}")
        End If

    End Sub

    Private Sub HorariosMails()

        With _TimerUnoPorDia_8Hs
            .AddDiaSemana(clsTimerEvent.teDiasSemana.te_Lunes)
            .AddDiaSemana(clsTimerEvent.teDiasSemana.te_Martes)
            .AddDiaSemana(clsTimerEvent.teDiasSemana.te_Miercoles)
            .AddDiaSemana(clsTimerEvent.teDiasSemana.te_Jueves)
            .AddDiaSemana(clsTimerEvent.teDiasSemana.te_Viernes)
            .HoraMinuto = "08:00"
            .Iniciar()
        End With

    End Sub

    Private Sub _TimerUnoPorDia_8Hs_EventoCumplido() Handles _TimerUnoPorDia_8Hs.EventoCumplido

        Call RecibosPendientesFirmar()
        Call RecibosPublicados()

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

    Public Function GetRecibosYGanancias(ByVal IdPersona As Long) As DataSet

        Try

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("IdPersona", IdPersona, SqlDbType.Int)
            End With

            Dim Ds As DataSet = MiAdo.Ejecutar.Procedimiento("SP_RECIBOSyGANANCIAS", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibosYGanancias", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetRecibosYGanancias", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetRecibos(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetRecibos

        Try

            Dim Ds As DataSet = GetRecibosYGanancias(IdPersona)
            Ds.Tables(0).TableName = "Recibos"
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

        Try

            Dim CamposExcluidos As String = "Clave,IdLiquidacion,IdLegajo,IdPersona"
            For Each C As String In CamposExcluidos.Split(",")
                If C.ToUpper = Campo.ToUpper Then Return True
            Next
            Return False

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.CampoExcluido", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex
        End Try

    End Function

    Private Function PathFileName4ta(ByVal PathFileName As String) As String

        Return PathFileName.Replace(".pdf", "_4ta.pdf")

    End Function

    Public Function GetDocumentoDescarga(ByVal Clave As String) As DataSet Implements ItzBASLaboro.GetDocumentoDescarga

        Try

            Dim IdLiquidacion As Long = 0
            Dim IdLegajo As Long = 0
            Dim Tipo As String = ""
            If Clave.Split("-").Count = 3 Then
                IdLiquidacion = Clave.Split("-")(0)
                IdLegajo = Clave.Split("-")(1)
                Tipo = Clave.Split("-")(2)
            Else
                Throw New Exception("NetCoreBLB.ClsBaslaboro.GetDocumentoDescarga - La clave esta mal formada IdLiquidacion-IdLegajo-TipoDoc : " & Clave)
            End If

            Dim PublicoGanancias As Boolean = MiAdo.Ejecutar.GetSQLInteger(String.Format("SELECT Count(*) FROM vAutogestion_Recibos WHERE IdLiquidacion = {0} And IdLegajo = {1} And Not [ArchivoGananciasUpload] is Null", IdLiquidacion, IdLegajo)) > 0
            If Tipo = "G" And Not PublicoGanancias Then
                Throw New Exception($"NetCoreBLB.ClsBaslaboro.GetDocumentoDescarga - No se publicó la planilla de ganancias para IdLiquidacion = {IdLiquidacion} y IdLegajo = {IdLegajo}")
            End If

            Dim Archivo As String = MiAdo.Ejecutar.GetSQLString(String.Format("SELECT PDF_RutaFTP = IsNull(PDF_RutaFTP,PDF_RutaLOC) FROM vAutogestion_Recibos WHERE IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
            Dim ArchivoAlias As String = IO.Path.GetFileName(Archivo)

            Dim Dt As DataTable
            If Tipo = "G" Then
                Dt = GetParametros(PathFileName4ta(Archivo), PathFileName4ta(ArchivoAlias))
            Else
                Dt = GetParametros(Archivo, ArchivoAlias)
            End If

            Dim Ds As New DataSet("GetDocumentoDescarga")
            Ds.Merge(Dt)
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetDocumentoDescarga", ClsLogger.TiposDeLog.LogDetalleNormal, Ds)
            Return Ds

        Catch ex As Exception

            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetDocumentoDescarga", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Throw ex

        End Try

    End Function

    Public Function GetReciboDescarga(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As DataSet Implements ItzBASLaboro.GetReciboDescarga

        Try


            Dim Archivo As String = MiAdo.Ejecutar.GetSQLString(String.Format("SELECT PDF_RutaFTP = IsNull(PDF_RutaFTP,PDF_RutaLOC) FROM vAutogestion_Recibos WHERE IdLiquidacion = {0} And IdLegajo = {1}", IdLiquidacion, IdLegajo))
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

    Private Function GetParametros(ByVal Archivo As String,
                                   ByVal ArchivoAlias As String) As DataTable

        Try

            Dim DtRta As New DataTable("GetReciboDescarga")
            DtRta.Columns.Add("Servidor")
            DtRta.Columns.Add("Usuario")
            DtRta.Columns.Add("Contrasenia")
            DtRta.Columns.Add("Archivo")
            DtRta.Columns.Add("ArchivoAlias")


            If MiAdo.Ejecutar.GetSQLInteger("Select IsNull(INTVALOR,0) FROM [dbo].[BL_PARAMETROS] where Parametro = 'Autogestion\OpcionFTP' And CodEmp is null") = 0 Then

                Dim VacioEncriptado As String = ClsCrypto.AES_Encrypt(String.Empty, Semilla)
                Dim DrRta As DataRow = DtRta.Rows.Add
                DrRta("Servidor") = VacioEncriptado
                DrRta("Usuario") = VacioEncriptado
                DrRta("Contrasenia") = VacioEncriptado
                DrRta("Archivo") = Archivo
                DrRta("ArchivoAlias") = ArchivoAlias

                ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetParametros.", ClsLogger.TiposDeLog.LogDetalleNormal, DtRta.DataSet)
                Return DtRta

            End If

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

            MailSolicitudRespondida(sLic)

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

            MailSolicitudRespondida(sLic)

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
            MailNuevaSolicitudLicencia(PedidoLicencia)
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

            If Not FirmaConforme Then MailReciboNoConforme(IdLegajo, IdLiquidacion, Observacion)

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

    Private Function MailReciboNoConforme(ByVal IdLegajo As Integer,
                                          ByVal IdLiquidacion As Integer,
                                          ByVal Observacion As String) As Boolean

        Try

            Dim Dt As DataTable = MiAdo.Consultar.GetDataTable(
                                  String.Format(" Select " &
                                                " Legajo = '(' + L.LegajoCodigo + ') - ' + P.NombreCompleto " &
                                                " ,Liq = R.Liquidacion_Codigo + ' - ' + RTRIM(Month(R.Liquidacion_Mes)) + '/' + RTRIM(Year(R.Liquidacion_Mes)) " &
                                                " From vAutogestion_Recibos R " &
                                                " Join vAutogestion_Legajos L  On L.IdLegajo = R.IdLegajo " &
                                                " Join vAutogestion_Personas P On P.IdPersona = L.IdPersona " &
                                                " Where L.IdLegajo = {0} and R.IdLiquidacion = {1}", IdLegajo, IdLiquidacion), "Legajo")

            Dim Legajo As String = String.Empty
            Dim Liq As String = String.Empty
            Dim MailNov As String = GetMailNovedades()

            If Dt.Rows.Count > 0 Then
                Legajo = Dt.Rows(0).Item("Legajo")
                Liq = Dt.Rows(0).Item("Liq")
            End If

            If MailNov.Length > 0 Then
                Dim Contenido As String = String.Format("BAS Laboro autogestión le comunica que el legajo {0} firmó en no conformidad el recibo correspondiente a la liquidación {1}.<br>" & Environment.NewLine &
                                                        "Observación : {2}", Legajo, Liq, Observacion)
                Dim Destinatarios As New List(Of String)
                Destinatarios.Add(MailNov)
                Return EnviarMail("BAS Laboro Autogestión: Recibo firmado no conforme.", Destinatarios, Contenido)
            End If

            Return False

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.MailReciboNoConforme", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return False
        End Try

    End Function

    Private Function GetMailNovedades() As String

        Try

            Return MiAdo.Ejecutar.GetSQLString("Select StrValor from BL_PARAMETROS Where PARAMETRO = 'Autogestion\MailNovedades' And CodEmp Is Null")

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.GetMailNovedades", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return String.Empty
        End Try

    End Function

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

    'Public Function EnviarMailConFormato(ByVal Destinatarios As List(Of String),
    '                                     Optional ByRef Resultado As String = "") As Boolean Implements ItzBASLaboro.EnviarMailConFormato

    '    Try

    '        Dim ContenidoHTML As String
    '        Dim Asunto As String

    '        Asunto = "Solicitud de licencia"
    '        ContenidoHTML = Fmt.Fmt_Autorizacion_Licencia("Ricardo Mario", "Luciano Escudero", "Lic. por examen", "10/10/2020", "20/10/2020", "10", "tengo que zafar")
    '        If Not EnviarMail(Asunto, Destinatarios, ContenidoHTML, Resultado) Then Return False

    '        Asunto = "Aprobación de licencia"
    '        ContenidoHTML = Fmt.Fmt_Aprobacion_Licencia("Luciano Escudero", "Aprobada", "05/10/2020", "Lic. por examen", "10/10/2020", "20/10/2020", "10", "No tengo problemas")
    '        If Not EnviarMail(Asunto, Destinatarios, ContenidoHTML, Resultado) Then Return False

    '        Asunto = "Recibos pendientes"
    '        ContenidoHTML = Fmt.Fmt_Recibos_Pendientes("Luciano Escudero")
    '        If Not EnviarMail(Asunto, Destinatarios, ContenidoHTML, Resultado) Then Return False

    '        Asunto = "Recibos publicados"
    '        ContenidoHTML = Fmt.Fmt_Recibos_Publicados("Luciano Escudero")
    '        If Not EnviarMail(Asunto, Destinatarios, ContenidoHTML, Resultado) Then Return False

    '        Return True

    '    Catch ex As Exception

    '        ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.EnviarMailConFormato", ClsLogger.TiposDeLog.LogDeError, ex.Message)
    '        Return False

    '    End Try

    'End Function

    Public Shared Function EnviarMail(ByVal Asunto As String,
                                      ByVal Destinatarios As List(Of String),
                                      ByVal ContenidoHTML As String,
                                      Optional ByRef Resultado As String = "") As Boolean

        If ContenidoHTML.Length = 0 Then Return False

        If Not ClsFormatoHtml.ExisteCarpetaFormatos Then
            ClsLogger.Logueo.Loguear($"NETCoreBLB.ClsBASLaboro.EnviarMail  ATENCION: No existe la carpeta de formatos {ClsFormatoHtml.CarpetaFormatos}")
            Return False
        End If

        Try

            Dim Cfg As New MailCfg

            Cfg.Leer()

            Return ClsMail.Enviar(Asunto,
                                  ContenidoHTML,
                                  Destinatarios,
                                  Nothing,
                                  Nothing,
                                  Cfg.Remitente,
                                  Cfg.RemitenteNombre,
                                  Cfg.Servidor,
                                  Cfg.Puerto,
                                  Cfg.Usuario,
                                  Cfg.Contrasenia,
                                  Cfg.HabilitarSSL,
                                  Resultado)

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.EnviarMail", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return False
        End Try

    End Function

    Private Function RecibosPublicados() As Boolean

        Try

            Dim Dt As DataTable = MiAdo.Consultar.GetDataTable(" Select Distinct R.LegajoCodigo," &
                                                               "                 P.NombreCompleto," &
                                                               "                 P.EmailPersonal" &
                                                               " From  vAutogestion_Recibos  R" &
                                                               " Join  vAutogestion_Legajos  L On L.IdLegajo = R.IdLegajo" &
                                                               " Join  vAutogestion_Personas P On P.IdPersona = L.IdPersona " &
                                                               " Where Not R.FTPUpLoad is null and R.Firmado = 0 And DATEDIFF(Hour,R.FTPUpLoad, Getdate()) < 24", "Publicados")

            For Each Dr As DataRow In Dt.Rows

                'Dim Contenido As String = String.Format("BAS Laboro autogestión le comunica al legajo ({0}) - {1} que existen recibos publicados.",
                '                                         Dr("LegajoCodigo"),
                '                                         Dr("NombreCompleto"))

                Dim Contenido As String = Fmt.Fmt_Recibos_Pendientes(Dr("NombreCompleto").ToString)

                Dim Destinatarios As New List(Of String)
                Destinatarios.Add(Dr("EmailPersonal").ToString)
                Call EnviarMail("BAS Laboro Autogestión: Recibos Publicados", Destinatarios, Contenido)

            Next

            Dt.Dispose()

            Return True

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.RecibosPublicados", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return False
        End Try

    End Function

    Private Function RecibosPendientesFirmar() As Boolean

        Try

            Dim Dt As DataTable = MiAdo.Consultar.GetDataTable(" Select Distinct R.LegajoCodigo," &
                                                               "                 P.NombreCompleto," &
                                                               "                 P.EmailPersonal" &
                                                               " From  vAutogestion_Recibos  R" &
                                                               " Join  vAutogestion_Legajos  L On L.IdLegajo = R.IdLegajo" &
                                                               " Join  vAutogestion_Personas P On P.IdPersona = L.IdPersona " &
                                                               " Where Not R.FTPUpLoad is null and R.Firmado = 0 And DATEDIFF(day,R.FTPUpLoad, Getdate()) > 15", "Pendientes")

            For Each Dr As DataRow In Dt.Rows

                'Dim Contenido As String = String.Format("BAS Laboro autogestión le comunica al legajo ({0}) - {1} que existen recibos pendientes de firmar.",
                '                                         Dr("LegajoCodigo"),
                '                                         Dr("NombreCompleto"))
                Dim Contenido As String = Fmt.Fmt_Recibos_Pendientes(Dr("NombreCompleto").ToString)


                Dim Destinatarios As New List(Of String)
                Destinatarios.Add(Dr("EmailPersonal").ToString)
                Call EnviarMail("BAS Laboro Autogestión: Recibos Pendientes de Firmar", Destinatarios, Contenido)

            Next

            Dt.Dispose()

            Return True

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.RecibosPendientesFirmar", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return False
        End Try

    End Function

    Private Function MailNuevaSolicitudLicencia(ByRef pedidoLic As ClsPedidoLicencia) As Boolean

        Try

            Dim Dt As DataTable = MiAdo.Consultar.GetDataTable("SELECT DISTINCT l.Legajo," &
                                                               "                p.NombreCompleto," &
                                                               "                p.IdPersona," &
                                                               "                l.CodEmp " &
                                                               "FROM Bl_Legajos l " &
                                                               "JOIN vAutogestion_Personas p ON p.IdPersona = l.IdPersona " &
                                                               "WHERE IdLegajo = " & pedidoLic.IdLegajo, "DatosLegajo")
            Dim Dr As DataRow = Dt.Rows(0)

            'Dim Contenido As String = "BAS Laboro autogestión le comunica que el empleado " & Dr("Legajo").ToString & " - " & Dr("NombreCompleto").ToString & " ha solicitado una licencia.<br>" &
            '                          "Datos de la Licencia:<br>" &
            '                          "Tipo: " & pedidoLic.TipoLic.ToString & ChrW(13) & "<br>" &
            '                          " Fecha Desde: " & pedidoLic.FechaDesde.ToString("dd/MM/yyyy") & "<br>" &
            '                          " Fecha Hasta: " & pedidoLic.FechaHasta.ToString("dd/MM/yyyy") & "<br>" &
            '                          " Días: " & pedidoLic.CantidadDias.ToString & "<br>" &
            '                          " Observaciones: " & pedidoLic.Observaciones.ToString

            Dim Contenido As String = Fmt.Fmt_Aprobacion_Licencia(Dr("NombreCompleto").ToString,
                                                                  pedidoLic.Estado.ToString,
                                                                  pedidoLic.FechaDeSolicitud.ToString("dd/MM/yyyy"),
                                                                  pedidoLic.TipoLic.ToString,
                                                                  pedidoLic.FechaDesde.ToString("dd/MM/yyyy"),
                                                                  pedidoLic.FechaHasta.ToString("dd/MM/yyyy"),
                                                                  pedidoLic.CantidadDias.ToString,
                                                                  pedidoLic.Observaciones.ToString)

            Dim DSManager As DataTable = GetManagers(Dr("IdPersona"), Dr("CodEmp")).Tables(0)
            Dim Destinatarios As New List(Of String)

            For Each Drm As DataRow In DSManager.Rows
                '    Destinatarios.Add(MiAdo.Ejecutar.GetSQLString("SELECT EmailPersonal FROM Bl_Personas WHERE IdPersona = " & Drm("IdPersona")))
                Destinatarios.Add(MiAdo.Ejecutar.GetSQLString("SELECT EmailPersonal FROM Bl_Personas WHERE IdPersona = " & Drm("IdPersona").ToString))
            Next

            Call EnviarMail("BAS Laboro Autogestión: Nueva Solicitud de Licencia", Destinatarios, Contenido)

            Dt.Dispose()
            DSManager.Dispose()

            Return True

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.MailNuevaSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
            Return False
        End Try

    End Function

    Private Function MailSolicitudRespondida(ByRef pedidoLic As ClsPedidoLicencia) As Boolean

        Try

            Dim MailDestinatario As String = MiAdo.Ejecutar.GetSQLString(" SELECT DISTINCT p.EmailPersonal " &
                                                                         " FROM Bl_Legajos l " &
                                                                         " JOIN vAutogestion_Personas p ON p.IdPersona = l.IdPersona " &
                                                                         " WHERE IdLegajo = " & pedidoLic.IdLegajo)

            Dim NombreManager As String = MiAdo.Ejecutar.GetSQLString(" SELECT DISTINCT p.NombreCompleto " &
                                                                      " FROM Bl_Legajos l" &
                                                                      " JOIN vAutogestion_Personas p ON p.IdPersona = l.IdPersona " &
                                                                      " WHERE IdLegajo = " & pedidoLic.IdAutorizadoPor)

            Dim NombreLegajo As String = MiAdo.Ejecutar.GetSQLString(" SELECT DISTINCT p.NombreCompleto " &
                                                                     " FROM Bl_Legajos l" &
                                                                     " JOIN vAutogestion_Personas p ON p.IdPersona = l.IdPersona " &
                                                                     " WHERE IdLegajo = " & pedidoLic.IdLegajo)

            'Dim Contenido As String = "BAS Laboro autogestión le comunica que la licencia solicitada ha sido: " & vbCrLf & pedidoLic.Estado.ToString & vbCrLf & "<br>" &
            '                          "Datos de la Licencia:<br>" & vbCrLf &
            '                          "Fecha de Solicitud: " & pedidoLic.FechaDeSolicitud.ToString("dd/MM/yyyy") & vbCrLf & "<br>" &
            '                          "Tipo :" & pedidoLic.TipoLic & vbCrLf & "<br>" &
            '                          "Fecha Desde :" & pedidoLic.FechaDesde.ToString("dd/MM/yyyy") & vbCrLf & "<br>" &
            '                          "Fecha Hasta :" & pedidoLic.FechaHasta.ToString("dd/MM/yyyy") & vbCrLf & "<br>" &
            '                          "Días: " & pedidoLic.CantidadDias & vbCrLf & "<br>" &
            '                          "Observaciones: " & pedidoLic.ObservacionesManager

            Dim Contenido As String = Fmt.Fmt_Autorizacion_Licencia(NombreManager,
                                                                    NombreLegajo,
                                                                    pedidoLic.TipoLic.ToString,
                                                                    pedidoLic.FechaDesde.ToString("dd/MM/yyyy"),
                                                                    pedidoLic.FechaHasta.ToString("dd/MM/yyyy"),
                                                                    pedidoLic.CantidadDias.ToString,
                                                                    pedidoLic.Observaciones.ToString)


            Dim Destinatarios As New List(Of String)
            Destinatarios.Add(MailDestinatario)
            Call EnviarMail("BAS Laboro Autogestión: Solicitud de Licencia " & pedidoLic.Estado.ToString, Destinatarios, Contenido)

            Return True

        Catch ex As Exception
            ClsLogger.Logueo.Loguear("NETCoreBLB.ClsBASLaboro.MailNuevaSolicitudLicencia", ClsLogger.TiposDeLog.LogDeError, ex.Message)
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

Public Class MailCfg

    Dim FileName As String = IO.Path.Combine(NETCoreLOG.ClsLogger.GetBaseFolderRegisry, "MailCfg.xml")

    Public Sub New()


    End Sub

    Public Sub Leer()

        Try

            If IO.File.Exists(FileName) Then

                Dim s As String = IO.File.ReadAllText(FileName)
                Dim Cfg As MailCfg = s.Deserializar(Me.GetType)
                RemitenteNombre = Cfg.RemitenteNombre
                Remitente = Cfg.Remitente
                Servidor = Cfg.Servidor
                Puerto = Cfg.Puerto
                Usuario = Cfg.Usuario
                Contrasenia = NETCoreCrypto.ClsCrypto.AES_Decrypt(Cfg.Contrasenia, ClsBASLaboro.Semilla)
                HabilitarSSL = Cfg.HabilitarSSL
                SufijoURL = Cfg.SufijoURL
                Cfg = Nothing

            Else

                Dim Cfg As MailCfg = New MailCfg
                Cfg.Grabar()

            End If

        Catch ex As Exception
            'Nada
        End Try

    End Sub

    Public Sub Grabar()

        Contrasenia = NETCoreCrypto.ClsCrypto.AES_Encrypt(Contrasenia, ClsBASLaboro.Semilla)
        IO.File.WriteAllText(FileName, Me.Serializar)

    End Sub

    Public Property RemitenteNombre As String = "BASLaboroAlerta"
    Public Property Remitente As String = "alertasbas@bas.com.ar"
    Public Property Servidor As String = "mail.bas.com.ar"
    Public Property Puerto As Integer = 25
    Public Property Usuario As String = "alertasbas"
    Public Property Contrasenia As String = "nuncacaduca"
    Public Property SufijoURL As String = "BAS"
    Public Property HabilitarSSL As Boolean = True

End Class