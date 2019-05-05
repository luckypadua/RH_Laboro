Imports System.Data

Public Interface ItzBASLaboro

    Function GetPersona(ByVal IdPersona As Integer) As DataSet
    Function GetLegajo(ByVal IdPersona As Integer) As DataSet
    Sub Dispose()

End Interface
