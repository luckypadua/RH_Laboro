﻿Imports System.Data
Imports Microsoft.VisualBasic

Public Class clsPedidoLicencia

    Private MiAdo As New NETCoreADO.AdoNet("SERVIDORBLB\SQL2014", "400BLB_Prueba", "sa", "sa")

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

    Private vTipoLic As String
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
            Return mIdClaseSuceso
        End Get
        Set(ByVal value As Long)
            mIdClaseSuceso = value
        End Set
    End Property

    Private vObservaciones As String
    Public Property Observaciones() As String
        Get
            Return vObservaciones
        End Get
        Set(ByVal value As String)
            vObservaciones = value
        End Set
    End Property

    Private vCodSuceso As String
    Public Property CodSuceso() As String
        Get
            If vCodSuceso = String.Empty Then
                vCodSuceso = vTipoLic.Split("-")(0).Trim
            End If
            Return vCodSuceso
        End Get
        Set(ByVal value As String)
            vCodSuceso = value
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

    Public Sub New()
    End Sub
    Public Sub New(ByVal IdLegajo As Integer, ByVal IdSuceso As Integer, ByVal IdClaseSuceso As Integer, ByVal FecSolicitud As DateTime, ByVal FecDesde As DateTime, ByVal FecHasta As DateTime, ByVal CantDias As Integer, ByVal CodSuceso As String)
        Me.New
        Me.IdLegajo = IdLegajo
        Me.IdSuceso = IdSuceso
        Me.IdClaseSuceso = IdClaseSuceso
        Me.FechaDeSolicitud = FecSolicitud
        Me.FechaDesde = FecDesde
        Me.FechaHasta = FechaHasta
        Me.CantidadDias = CantDias
        Me.CodSuceso = CodSuceso
    End Sub

    Public Function Validar() As Boolean

        Try
            Return Me.ValidarSuceso & Me.ValidarFechasYDias & Me.ValidarTopes
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:Validar" & ex.Message)
        End Try

    End Function

    Private Function ValidarSuceso() As Boolean
        Return (MiAdo.Ejecutar.GetSQLInteger("SELECT COUNT(*) FROM Bl_Sucesos WHERE CodSuceso = '" & CodSuceso.Trim & "'") > 0)
    End Function

    Private Function ValidarFechasYDias() As Boolean
        Return (Me.CantidadDias <> 0) And (FechaDesde.CompareTo(FechaHasta) <= 0)
    End Function

    Private Function ValidarTopes(Optional ByRef MsjFinal As String = "") As Boolean

        Try
            Dim Excede As Boolean

            With MiAdo.Ejecutar.Parametros
                .RemoveAll()
                .Add("Tratamiento", MiAdo.Ejecutar.GetSQLString("SELECT Tratamiento FROM Bl_SucesosClases WHERE IdClaseSuceso = " & mIdClaseSuceso), SqlDbType.Char)
                .Add("CodSuceso", Me.CodSuceso, SqlDbType.VarChar)
                .Add("CodClaseSuceso", MiAdo.Ejecutar.GetSQLString("SELECT CodClaseSuceso FROM Bl_SucesosClases WHERE IdClaseSuceso = " & mIdClaseSuceso), SqlDbType.VarChar)
                .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
                .Add("CodAtr", DBNull.Value, SqlDbType.Char)
                .Add("CodAtrVal", DBNull.Value, SqlDbType.Char)
                .Add("FechaOcurrencia", Me.FechaDeSolicitud, SqlDbType.DateTime)
                .Add("FechaSuspDesde", DBNull.Value, SqlDbType.SmallDateTime)
                .Add("Importe", DBNull.Value, SqlDbType.Decimal)
                .Add("Dias", Me.CantidadDias, SqlDbType.Int)
                .Add("VacAfectacion", Me.VacAfectacion, SqlDbType.Char)
                .Add("IdOcurrencia", DBNull.Value, SqlDbType.Int)
                .Add("Excede", Excede, SqlDbType.Bit)
                .Add("MensajeFinal", MsjFinal, SqlDbType.VarChar)
            End With

            MiAdo.Ejecutar.Procedimiento("BL_NovedadesTopes", NETCoreADO.AdoNet.TipoDeRetornoEjecutar.NotReturn)

            Return Excede
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:ValidarTopes" & ex.Message)
        End Try
    End Function

    Public Sub Grabar()

        Try
            With MiAdo.Ejecutar.Parametros
                .Add("IdLegajo", Me.IdLegajo, SqlDbType.Int)
                .Add("FecSolicitud", Me.FechaDeSolicitud, SqlDbType.Int)
                .Add("IdSuceso", Me.IdSuceso, SqlDbType.Int)
                .Add("FecDesde", Me.FechaDesde, SqlDbType.Int)
                .Add("FecHasta", Me.FechaHasta, SqlDbType.Int)
                .Add("Cantidad", Me.CantidadDias, SqlDbType.Int)
                .Add("Observaciones", Me.Observaciones, SqlDbType.Int)
            End With

            MiAdo.Ejecutar.Insertar("BL_NovedadesPedidos")
        Catch ex As Exception
            Throw New ArgumentException("NETCoreBLB:clsPedidoLicencia:Grabar" & ex.Message)
        End Try

    End Sub

End Class