Imports System.Data

Public Interface ItzBASLaboro
    Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet
    Function GetLoguinsIni() As DataSet
    Function GetRecibos(ByVal IdPersona As Integer) As DataSet
    Function GetReciboDescarga(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As DataSet
    Sub ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "")
    Sub ReciboDescargado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long)
    Function GetManagers(ByVal IdLegajo As Long) As DataSet
    Function GetLicencias(ByVal IdLegajo As Long) As DataSet
    Function GetTipoLicencias() As DataSet
    Sub Dispose()

End Interface
