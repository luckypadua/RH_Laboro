Imports System.Data

Public Interface ItzBASLaboro
    Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet
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
    Function GetVacaciones(ByVal IdLegajo As Long) As DataSet
    Function GetSolicitudVacaciones(ByVal IdLegajo As Long) As DataSet
    Function GrabarSolicitudVacaciones(ByRef PedidoLicenci As ClsPedidoLicencia) As Boolean
    Function GetVacacionesDetalle(ByVal IdLegajo As Long, Optional ByVal AnioDesde As Integer = 0) As DataSet
    Function GetCambiosEnUsuarios(Optional ByVal EsConfiguracionInicial As Boolean = False) As DataSet
    Sub OkCambiosUsuarios(IdCambios As Long)
    Function GetSucesoDeVacaciones() As Long
    Sub BorrarCambiosUsuariosPendientes(Optional ByVal IdCambios As Long = 0)
    Sub Dispose()

End Interface
