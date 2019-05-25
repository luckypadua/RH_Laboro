Imports System.Data

Public Interface ItzBASLaboro
    Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet
    Function GetLoguinsIni() As DataSet
    Function GetRecibos(ByVal IdPersona As Integer) As DataSet
    Function ReciboFirmado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long, ByVal FirmaConforme As Boolean, Optional ByVal Observacion As String = "") As Boolean
    Function ReciboDescargado(ByVal IdLiquidacion As Long, ByVal IdLegajo As Long) As Boolean
    Sub Dispose()

End Interface
