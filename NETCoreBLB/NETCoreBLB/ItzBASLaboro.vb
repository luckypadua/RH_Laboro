﻿Imports System.Data

Public Interface ItzBASLaboro
    Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet
    Function GetLoguinsIni() As DataSet
    Function GetRecibos(ByVal IdPersona As Integer) As DataSet
    Function GetRecibosDetalle(ByVal IdPersona As Integer) As String
    Function GetReciboDescarga(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As DataSet
    Sub ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "")
    Sub ReciboVisualizado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long)
    Function GetManagers(ByVal IdLegajo As Long) As DataSet
    Function GetEmpleadosACargo(ByVal IdLegajo As Long) As DataSet
    Function GetLicencias(ByVal IdLegajo As Long) As DataSet
    Function GetTipoLicencias() As DataSet
    Function ValidarSolicitudLicencia(ByRef PedidoLicencia As clsPedidoLicencia) As Boolean
    Function EliminarSolicitudLicencia(ByVal IdPedidoLicencia As Long) As Boolean
    Function GrabarSolicitudLicencia(ByRef PedidoLicencia As clsPedidoLicencia) As Boolean
    Function GetSolicitudesLicencias(ByVal IdLegajo As Long) As DataSet
    Function AceptarSolicitudLicencia(ByVal IdPedidoLicencia As Long, ByVal IdManager As Long, ByVal Observaciones As String) As Boolean
    Function RechazarSolicitudLicencia(ByVal IdPedidoLicencia As Long, ByVal IdManager As Long, ByVal Observaciones As String) As Boolean
    Function GetSolicitudesLicenciasManager(ByVal IdLegajoManager As Long) As DataSet
    Sub Dispose()

End Interface
