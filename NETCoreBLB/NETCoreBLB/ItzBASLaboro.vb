Imports System.Data

Public Interface ItzBASLaboro
    Function GetDatosPersonales(ByVal IdPersona As Integer) As DataSet
    Function GetLoguinsIni() As DataSet
    Sub Dispose()

End Interface
