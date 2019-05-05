Imports System.Data

Public Class ClsBASLaboro

    Implements IDisposable, ItzBASLaboro

    Private MiAdo As New NETCoreADO.AdoNet

    Public Sub New(ByVal Server As String,
                   ByVal Database As String,
                   Optional ByVal Uid As String = "",
                   Optional ByVal Pwd As String = "")

        MiAdo.Configurar.Server = Server
        MiAdo.Configurar.Database = Database
        MiAdo.Configurar.Uid = Uid
        MiAdo.Configurar.Pwd = Pwd

    End Sub

    Public Function GetPersonaLegajos(ByVal IdPersona As Integer) As DataSet Implements ItzBASLaboro.GetPersonaLegajos

        Dim Ds As DataSet = MiAdo.Consultar.GetDataset(String.Format("Select * from Portal_Personas Where IdPersona = {0}", IdPersona), "Persona")
        Ds.DataSetName = "PersonaLegajos"
        Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from Portal_Legajos Where IdPersona = {0}", IdPersona), "Legajo", Ds)
        Ds = MiAdo.Consultar.GetDataset(String.Format("Select * from Portal_Puestos Where IdPersona = {0}", IdPersona), "Puestos", Ds)
        Return Ds

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
