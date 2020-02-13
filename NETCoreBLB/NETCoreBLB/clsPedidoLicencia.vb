Imports System.Data
Imports Microsoft.VisualBasic
Public Class ClsPedidoLicencia
    Public Enum eEstadoPedidoLic
        Pendiente = 0
        Autorizada = 1
        NoAutorizada = 2
        Aceptada = 3
        Rechazada = 4
        AceptadaConModificaciones = 5
        Eliminada = 6
        Otro = 7
    End Enum

    'Private MiAdo As New NETCoreADO.AdoNet("srvsueldos\sql08r2", "400_Microsules", "sa", "admin1*")
    Private MiAdo As New NETCoreADO.AdoNet

    Private mIdAutorizadoPor As Long
    Public Property IdAutorizadoPor() As Long
        Get
            Return mIdAutorizadoPor
        End Get
        Set(ByVal value As Long)
            mIdAutorizadoPor = value
        End Set
    End Property

    Private IdPedidoLicencia As Long
    Public Property Id() As Long
        Get
            Return IdPedidoLicencia
        End Get
        Set(ByVal value As Long)
            IdPedidoLicencia = value
        End Set
    End Property

    Private vIdLegajo As Long
    Public Property IdLegajo() As Long
        Get
            Return vIdLegajo
        End Get
        Set(ByVal value As Long)
            vIdLegajo = value
        End Set
    End Property

    Private vFecDesde As Date
    Public Property FechaDesde() As Date
        Get
            Return vFecDesde
        End Get
        Set(ByVal value As Date)
            vFecDesde = value
        End Set
    End Property

    Private vFecHasta As Date
    Public Property FechaHasta() As Date
        Get
            Return vFecHasta
        End Get
        Set(ByVal value As Date)
            vFecHasta = value
        End Set
    End Property

    Private mFecSolicitud As Date
    Public Property FechaDeSolicitud() As Date
        Get
            Return mFecSolicitud
        End Get
        Set(ByVal value As Date)
            mFecSolicitud = value
        End Set
    End Property

    Private cDias As Integer
    Public Property CantidadDias() As Integer
        Get
            Return cDias
        End Get
        Set(ByVal value As Integer)
            cDias = value
        End Set
    End Property

    Private vTipoLic As String = ""
    Public Property TipoLic() As String
        Get
            Return vTipoLic
        End Get
        Set(ByVal value As String)
            vTipoLic = value
        End Set
    End Property

    Private mIdSuceso As Long
    Public Property IdSuceso() As Long
        Get
            Return mIdSuceso
        End Get
        Set(ByVal value As Long)
            mIdSuceso = value
        End Set
    End Property

    Private mIdClaseSuceso As Long
    Public Property IdClaseSuceso() As Long
        Get
            If mIdClaseSuceso = 0 And Me.IdSuceso <> 0 Then
                mIdClaseSuceso = MiAdo.Ejecutar.GetSQLInteger("SELECT IdClaseSuceso FROM Bl_Sucesos WHERE IdSuceso = " & Me.IdSuceso)
            End If
            Return mIdClaseSuceso
        End Get
        Set(ByVal value As Long)
            mIdClaseSuceso = value
        End Set
    End Property

    Private vObservaciones As String = ""
    Public Property Observaciones() As String
        Get
            Return vObservaciones
        End Get
        Set(ByVal value As String)
            vObservaciones = value
        End Set
    End Property

    Private vObservacionesManager As String = ""
    Public Property ObservacionesManager() As String
        Get
            Return vObservacionesManager
        End Get
        Set(ByVal value As String)
            vObservacionesManager = value
        End Set
    End Property

    Private vObservacionesAdmin As String = ""
    Public Property ObservacionesAdmin() As String
        Get
            Return vObservacionesAdmin
        End Get
        Set(ByVal value As String)
            vObservacionesAdmin = value
        End Set
    End Property

    Private vCodSuceso As String = ""
    Public ReadOnly Property CodSuceso() As String
        Get
            vCodSuceso = MiAdo.Ejecutar.GetSQLString("SELECT CodSuceso FROM Bl_Sucesos WHERE IdSuceso = " & Me.IdSuceso).Trim
            Return vCodSuceso
        End Get
        'Set(ByVal value As String)
        '    vCodSuceso = value
        'End Set
    End Property

    Private mEstado As eEstadoPedidoLic
    Public Property Estado() As eEstadoPedidoLic
        Get
            Return mEstado
        End Get
        Set(ByVal value As eEstadoPedidoLic)
            mEstado = value
        End Set
    End Property

    Private vVacAfectacion As String
    Public ReadOnly Property VacAfectacion() As String
        Get
            vVacAfectacion = MiAdo.Ejecutar.GetSQLString("SELECT CASE 
                                                                     WHEN EsVacacion = 1 AND AfectaLiquidadas = 1 AND AfectaGozadas = 1 THEN '2'
                                                                     WHEN EsVacacion = 1 AND AfectaLiquidadas = 1 AND AfectaGozadas = 0 THEN 'L'
                                                                     WHEN EsVacacion = 1 AND AfectaLiquidadas = 0 AND AfectaGozadas = 1 THEN 'G'
                                                                     ELSE ''
                                                                 END
                                                          FROM Bl_Sucesos 
                                                          WHERE IdSuceso = " & mIdSuceso)
            Return vVacAfectacion
        End Get
    End Property

    Private Sub New()
    End Sub
    Friend Sub New(ByVal IdPedidoLicencia As Long, ByVal Conexion As NETCoreADO.AdoNet)

        Me.New()
        Me.Id = IdPedidoLicencia
        Me.MiAdo = Conexion

        Try
            Dim Ds As DataSet = MiAdo.Consultar.GetDataset("SELECT * FROM BL_NovedadesPedidos WHERE IdOcurrenciaPedido = " & Me.IdPedidoLicencia, "BL_NovedadesPedidos")

            Me.IdLegajo = Ds.Tables(0).Rows(0)("IdLegajo")
            Me.IdSuceso = Ds.Tables(0).Rows(0)("IdSuceso")
            Me.IdClaseSuceso = MiAdo.Ejecutar.GetSQLInteger("SELECT IdClaseSuceso FROM Bl_Sucesos WHERE IdSuceso = " & Me.IdSuceso)
            Me.FechaDeSolicitud = Ds.Tables(0).Rows(0)("FecSolicitud")
            Me.FechaDesde = Ds.Tables(0).Rows(0)("FecDesde")
            Me.FechaHasta = Ds.Tables(0).Rows(0)("FecHasta")
            Me.CantidadDias = Ds.Tables(0).Rows(0)("Cantidad")
            vCodSuceso = MiAdo.Ejecutar.GetSQLString("SELECT CodSuceso FROM Bl_Sucesos WHERE IdSuceso = " & Me.IdSuceso)
            Me.Observaciones = Ds.Tables(0).Rows(0)("Observaciones").ToString
            Me.ObservacionesManager = Ds.Tables(0).Rows(0)("ObservacionesManager").ToString
            Me.Estado = Ds.Tables(0).Rows(0)("Estado")
        Catch ex As Exception
            Throw New Exception("NETCoreBLB:clsPedidoLicencia:New" & ex.Message)
        End Try

    End Sub
    'Public Sub New(ByVal IdLegajo As Long, ByVal IdSuceso As Long, ByVal IdClaseSuceso As Long, ByVal FecSolicitud As DateTime, ByVal FecDesde As DateTime, ByVal FecHasta As DateTime, ByVal CantDias As Integer, ByVal CodSuceso As String)

    '    Me.New
    '    Me.IdLegajo = IdLegajo
    '    Me.IdSuceso = IdSuceso
    '    Me.IdClaseSuceso = IdClaseSuceso
    '    Me.FechaDeSolicitud = FecSolicitud
    '    Me.FechaDesde = FecDesde
    '    Me.FechaHasta = FecHasta
    '    Me.CantidadDias = CantDias
    '    vCodSuceso = CodSuceso
    '    Me.Observaciones = ""
    '    Me.ObservacionesManager = ""

    '    If AutorizarAutomaticamente() Then
    '        Me.Estado = eEstadoPedidoLic.Autorizada
    '    Else
    '        Me.Estado = eEstadoPedidoLic.Pendiente
    '    End If
    'End Sub

    Friend Sub New(ByVal IdLegajo As Long, ByVal IdSuceso As Long, ByVal FecSolicitud As DateTime, ByVal FecDesde As DateTime, ByVal FecHasta As DateTime, ByVal CantDias As Integer, ByVal Observaciones As String, ByVal Conexion As NETCoreADO.AdoNet)

        Me.New
        Me.MiAdo = Conexion
        Me.IdLegajo = IdLegajo
        Me.IdSuceso = IdSuceso
        Me.FechaDeSolicitud = FecSolicitud
        Me.FechaDesde = FecDesde
        Me.FechaHasta = FecHasta
        Me.CantidadDias = CantDias
        Me.Observaciones = Observaciones
        Me.ObservacionesManager = ""

        If AutorizarAutomaticamente() Then
            Me.Estado = eEstadoPedidoLic.Autorizada
        Else
            Me.Estado = eEstadoPedidoLic.Pendiente
        End If
    End Sub

    Private Function AutorizarAutomaticamente() As Boolean
        'Esta función devuelve True en el caso de que el Legajo solicitado no posea manager. En ese caso, la licencia es autorizada automáticamente porque se asume que no requiere autorización

        Dim CodEmp As Long = MiAdo.Ejecutar.GetSQLTinyInt("SELECT CodEmp FROM Bl_Legajos WHERE IdLegajo = " & Me.IdLegajo)

        With MiAdo.Ejecutar.Parametros
            .RemoveAll()
            .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
            .Add("IdCarpeta", DBNull.Value, SqlDbType.Int)
            .Add("IncluirManagers", 1, SqlDbType.SmallInt)
            .Add("IncluirEmpleadosACargo", 0, SqlDbType.SmallInt)
            .Add("CodEmp", CodEmp, SqlDbType.SmallInt)
        End With

        Dim DS As DataSet = MiAdo.Ejecutar.Procedimiento("SP_GetManagersYEmpleados", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)

        Return (DS.Tables(0).Rows.Count = 0)

    End Function

    Public Function Validar() As Boolean

        Try
            Return (Me.ValidarSuceso And Me.ValidarFechasYDias And Me.ValidarTopes And Me.ValidarOtrasLicenciasEnElPeriodo And Me.ValidarOtrosPedidosEnElPeriodo And Me.ValidarLegajoActivo)
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:Validar" & ex.Message)
        End Try

    End Function

    Private Function ValidarSuceso() As Boolean
        Try
            If MiAdo.Ejecutar.GetSQLInteger("SELECT COUNT(*) FROM Bl_Sucesos WHERE IdSuceso = " & Me.IdSuceso) = 0 Then
                Throw New Exception("@El suceso solicitado no existe")
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("NETCoreBLB:clsPedidoLicencia:ValidarSuceso" & ex.Message)
        End Try
    End Function

    Private Function ValidarFechasYDias() As Boolean
        If Me.CantidadDias = 0 Or FechaDesde.CompareTo(FechaHasta) > 0 Then
            Throw New Exception("@La cantidad de días no puede ser 0 y/o la fecha desde debe ser menor a la fecha hasta")
        End If
        Return True
    End Function

    Private Function ValidarTopes(Optional ByRef MsjFinal As String = "") As Boolean

        Try
            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("Tratamiento", MiAdo.Ejecutar.GetSQLString("SELECT Tratamiento FROM Bl_SucesosClases WHERE IdClaseSuceso = " & Me.IdClaseSuceso), SqlDbType.Char)
                .Add("CodSuceso", Me.CodSuceso, SqlDbType.VarChar)
                .Add("CodClaseSuceso", MiAdo.Ejecutar.GetSQLString("SELECT CodClaseSuceso FROM Bl_SucesosClases WHERE IdClaseSuceso = " & Me.IdClaseSuceso), SqlDbType.VarChar)
                .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
                .Add("CodAtr", DBNull.Value, SqlDbType.Char)
                .Add("CodAtrVal", DBNull.Value, SqlDbType.Char)
                .Add("FechaOcurrencia", Me.FechaDeSolicitud, SqlDbType.DateTime)
                .Add("FechaSuspDesde", DBNull.Value, SqlDbType.DateTime)
                .Add("Importe", DBNull.Value, SqlDbType.Decimal)
                .Add("Dias", Me.CantidadDias, SqlDbType.Int)
                .Add("VacAfectacion", Me.VacAfectacion, SqlDbType.Char)
                .Add("IdOcurrencia", DBNull.Value, SqlDbType.Int)
                .Add("Excede", False, SqlDbType.Bit, ParameterDirection.Output)
                .Add("MensajeFinal", String.Empty, SqlDbType.VarChar, ParameterDirection.Output)
            End With

            MiAdo.Ejecutar.Procedimiento("BL_NovedadesTopes", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.NotReturn)

            MsjFinal = MiAdo.Ejecutar.Parametros("MensajeFinal").Valor

            If CInt(MiAdo.Ejecutar.Parametros("Excede").Valor) <> 0 Then
                Throw New Exception("@" & MiAdo.Ejecutar.Parametros("MensajeFinal").Valor.ToString)
            End If

            Return True

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:ValidarTopes" & ex.Message)
        End Try

    End Function

    Private Function ValidarOtrasLicenciasEnElPeriodo() As Boolean

        Try
            Dim DS As DataSet

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("FecDesde", Me.FechaDesde, SqlDbType.DateTime)
                .Add("FecHasta", Me.FechaHasta, SqlDbType.DateTime)
                .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
            End With

            DS = MiAdo.Ejecutar.Procedimiento("SP_TraerLicenciasEnPeriodo2", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.ReturnDataset)

            If DS.Tables(0).Rows.Count <> 0 Then
                Throw New Exception("@Existen otras licencias y/o vacaciones en el período solicitado")
            End If

            Return True

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:ValidarOtrasLicenciasEnElPeriodo" & ex.Message)
        End Try

    End Function

    Private Function ValidarOtrosPedidosEnElPeriodo() As Boolean

        Try
            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                '.Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
                '.Add("FecDesde", Me.FechaDesde, SqlDbType.DateTime)
                '.Add("FecHasta", Me.FechaHasta, SqlDbType.DateTime)
            End With

            If MiAdo.Ejecutar.GetSQLInteger("SELECT COUNT(*) FROM BL_NovedadesPedidos WHERE IdLegajo = " & Me.IdLegajo & " AND IdOcurrenciaPedido <> " & IdPedidoLicencia & " AND Estado <> " & eEstadoPedidoLic.Rechazada & " AND Estado <> " & eEstadoPedidoLic.Eliminada & " AND Estado <> " & eEstadoPedidoLic.NoAutorizada & " AND ((Convert(Date,FecDesde) >= '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND Convert(Date,FecDesde) <= '" & Me.FechaHasta.ToString("yyyyMMdd") & "') OR (Convert(Date,FecHasta) >= '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND Convert(Date,FecHasta) <= '" & Me.FechaHasta.ToString("yyyyMMdd") & "') OR (Convert(Date,FecDesde) <= '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND Convert(Date,FecHasta) >= '" & Me.FechaHasta.ToString("yyyyMMdd") & "'))") > 0 Then
                Throw New Exception("@Existen otros pedidos de licencia para el período solicitado")
            End If

            Return True

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:ClsPedidoLicencia:ValidarOtrosPedidosEnElPeriodo" & ex.Message)
        End Try

    End Function
    Private Function ValidarLegajoActivo() As Boolean

        Try
            If MiAdo.Ejecutar.GetSQLInteger("SELECT COUNT(*) FROM Bl_Legajos WHERE IdLegajo = " & Me.IdLegajo & " AND (Cast(FecIngreso as Datetime) BETWEEN '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND '" & Me.FechaHasta.ToString("yyyyMMdd") & "' OR Cast(IsNull(FecEgreso,'20790101') as Datetime) BETWEEN '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND '" & Me.FechaHasta.ToString("yyyyMMdd") & "' OR (Cast(FecIngreso as Datetime) <= '" & Me.FechaDesde.ToString("yyyyMMdd") & "' AND Cast(IsNull(FecEgreso,'20790101') as Datetime) >= '" & Me.FechaHasta.ToString("yyyyMMdd") & "'))") = 0 Then
                Throw New Exception("@El legajo está inactivo en el período solicitado")
            End If

            Return True
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:ClsPedidoLicencia:ValidarLegajoActivo" & ex.Message)
        End Try

    End Function
    Public Sub Grabar()

        Try
            If Me.Validar() Then
                Dim mEstadoAnterior As eEstadoPedidoLic = MiAdo.Ejecutar.GetSQLTinyInt("SELECT Estado FROM Bl_NovedadesPedidos WHERE IdOcurrenciaPedido = " & Me.IdPedidoLicencia)

                With MiAdo.Ejecutar.Parametros
                    .RemoveAll()
                    .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
                    .Add("FecSolicitud", Me.FechaDeSolicitud, SqlDbType.DateTime)
                    .Add("IdSuceso", Me.IdSuceso, SqlDbType.Int)
                    .Add("FecDesde", Me.FechaDesde, SqlDbType.DateTime)
                    .Add("FecHasta", Me.FechaHasta, SqlDbType.DateTime)
                    .Add("Cantidad", Me.CantidadDias, SqlDbType.SmallInt)
                    .Add("Estado", Me.Estado, SqlDbType.SmallInt)
                    .Add("Observaciones", Me.Observaciones, SqlDbType.VarChar)
                    .Add("ObservacionesManager", Me.ObservacionesManager, SqlDbType.VarChar)
                End With

                If Me.IdPedidoLicencia <> 0 Then
                    MiAdo.Ejecutar.Modificar("BL_NovedadesPedidos", "IdOcurrenciaPedido = " & Me.IdPedidoLicencia)

                    If mEstadoAnterior <> Me.Estado Then
                        With MiAdo.Ejecutar.Parametros
                            .RemoveAll()
                            .Add("IdOcurrenciaPedido", Me.IdPedidoLicencia, SqlDbType.Int)
                            .Add("FecOcurrencia", Date.Now, SqlDbType.DateTime)
                            .Add("EstadoAnterior", mEstadoAnterior, SqlDbType.SmallInt)
                            .Add("EstadoNuevo", Me.Estado, SqlDbType.SmallInt)
                            If Me.Estado = eEstadoPedidoLic.Autorizada Or Me.Estado = eEstadoPedidoLic.NoAutorizada Then
                                .Add("Observaciones", Me.ObservacionesManager, SqlDbType.VarChar)
                            ElseIf Me.Estado = eEstadoPedidoLic.Aceptada Or Me.Estado = eEstadoPedidoLic.AceptadaConModificaciones Or Me.Estado = eEstadoPedidoLic.Eliminada Or Me.Estado = eEstadoPedidoLic.Rechazada Then
                                .Add("Observaciones", Me.ObservacionesAdmin, SqlDbType.VarChar)
                            Else
                                .Add("Observaciones", Me.Observaciones, SqlDbType.VarChar)
                            End If
                            .Add("IdLegajoCambioDeEstado", Me.IdAutorizadoPor, SqlDbType.Int)
                        End With

                        MiAdo.Ejecutar.Insertar("BL_NovedadesPedidos_Detalle")
                    End If

                Else
                    MiAdo.Ejecutar.Insertar("BL_NovedadesPedidos")
                End If

            End If
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:Grabar" & ex.Message)
        End Try

    End Sub

    Public Sub Borrar()
        Try
            If Me.Estado <> eEstadoPedidoLic.Pendiente Then
                Throw New ArgumentException("@No es posible eliminar un pedido cuyo estado sea diferente de 'Pendiente'")
            End If

            MiAdo.Ejecutar.Borrar("BL_NovedadesPedidos", "IdOcurrenciaPedido = " & Me.Id)

        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:Borrar" & ex.Message)
        End Try
    End Sub

End Class
