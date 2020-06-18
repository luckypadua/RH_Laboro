Imports System.Globalization

Public Class clsTimer

    Implements IDisposable

    Public Enum UnidadesDeTiempo
        Segundos = 0
        Minutos
        Horas
    End Enum

    Private _Frecuencia As Integer
    Private _UnidadDeTiempo As UnidadesDeTiempo
    Private _UnidadDeTiempoLetra As String
    Private WithEvents _ObjTimer As New Timers.Timer
    Public Event TimerExec()

    Public Sub New()

        _Frecuencia = 1
        _UnidadDeTiempo = UnidadesDeTiempo.Minutos
        _UnidadDeTiempoLetra = "m"

    End Sub

    Public Sub New(ByVal Frecuencia As Integer,
                   ByVal UnidadDeTiempo As UnidadesDeTiempo)

        _Frecuencia = Frecuencia
        _UnidadDeTiempo = UnidadDeTiempo
        _UnidadDeTiempoLetra = GetUnidadDeTiempoLetra(UnidadDeTiempo)

    End Sub

    Private Sub ExecEvent()
        If Pausar Then Exit Sub
        RaiseEvent TimerExec()
    End Sub

    Private Sub _ObjTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles _ObjTimer.Elapsed
        Call ExecEvent()
    End Sub

    Public Property Pausar As Boolean

    ''' <summary>
    ''' Iniciar la sincronización
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Iniciar()
        _ObjTimer.Interval = GetFrecuenciaMiliSegundos(Me.Frecuencia, Me.UnidadDeTiempo)
        _ObjTimer.Start()
        Pausar = False
    End Sub

    ''' <summary>
    ''' Iniciar la sincronización
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub IniciarAhora()
        Call ExecEvent()
        Iniciar()
    End Sub

    ''' <summary>
    ''' Detener la sincronización
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Detener()
        _ObjTimer.Stop()
        Pausar = True
    End Sub

    ''' <summary>
    ''' Frecuencia del tiempo de sincronización expresado en minutos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Frecuencia() As Integer
        Get
            Return _Frecuencia
        End Get
        Set(ByVal value As Integer)
            _Frecuencia = value
            If value = 0 Then Exit Property
            If _ObjTimer Is Nothing Then Exit Property
            _ObjTimer.Interval = GetFrecuenciaMiliSegundos(CInt(value), Me.UnidadDeTiempo)
        End Set
    End Property

    ''' <summary>
    ''' Unidad de tiempo de la frecuencia de sincronización (Hora, Minutos o segundos)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnidadDeTiempo() As UnidadesDeTiempo
        Get
            Return _UnidadDeTiempo
        End Get
        Set(ByVal value As UnidadesDeTiempo)
            _UnidadDeTiempo = value
            Me.Frecuencia = _Frecuencia
        End Set
    End Property

    ''' <summary>
    ''' Unidad de tiempo, expresada en texto, de la frecuencia de sincronización (Hora, Minutos o segundos)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnidadDeTiempoLetra() As String
        Get
            Return _UnidadDeTiempoLetra
        End Get
        Set(ByVal value As String)
            _UnidadDeTiempoLetra = value
            _UnidadDeTiempo = GetUnidadDeTiempo(value)
            Me.Frecuencia = _Frecuencia
        End Set
    End Property

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
                _ObjTimer = Nothing
            End If

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
    'Protected Overrides Sub Finalize()
    '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Shared Function GetFrecuenciaMiliSegundos(ByVal Frecuencia As Integer, ByVal UnidadTiempo As String) As Integer

        Return GetFrecuenciaMiliSegundos(Frecuencia, GetUnidadDeTiempo(UnidadTiempo))

    End Function

    Public Shared Function GetFrecuenciaMiliSegundos(ByVal Frecuencia As Integer, ByVal UnidadTiempo As UnidadesDeTiempo) As Integer

        Select Case UnidadTiempo
            Case UnidadesDeTiempo.Horas
                Return Frecuencia * 3600000
            Case UnidadesDeTiempo.Minutos
                Return Frecuencia * 60000
            Case UnidadesDeTiempo.Segundos
                Return Frecuencia * 1000
            Case Else
                Return 3600000 '1 hora
        End Select

    End Function

    Public Shared Function GetUnidadDeTiempoLetra(ByVal UnidadTiempo As UnidadesDeTiempo) As String

        Select Case UnidadTiempo
            Case UnidadesDeTiempo.Horas
                Return "h"
            Case UnidadesDeTiempo.Minutos
                Return "m"
            Case UnidadesDeTiempo.Segundos
                Return "s"
            Case Else
                Return "m"
        End Select

    End Function

    Public Shared Function GetUnidadDeTiempo(ByVal UnidadTiempoLetra As String) As UnidadesDeTiempo

        Select Case UnidadTiempoLetra
            Case "h", "H", "Hora", "Horas"
                Return UnidadesDeTiempo.Horas
            Case "m", "M", "Minuto", "Minutos"
                Return UnidadesDeTiempo.Minutos
            Case "s", "S", "Segundo", "Segundos"
                Return UnidadesDeTiempo.Segundos
            Case Else
                Return UnidadesDeTiempo.Minutos
        End Select

    End Function

End Class

Public Class clsTimerEvent

    Private WithEvents _Timer As New clsTimer

    Public Event EventoCumplido()

    Public Enum teDiasSemana

        te_Lunes = 1
        te_Martes
        te_Miercoles
        te_Jueves
        te_Viernes
        te_Sabado
        te_Domingo

    End Enum

    Public Enum teMesesAnio

        te_Enero = 1
        te_Febrero
        te_Marzo
        te_Abril
        te_Maryo
        te_Junio
        te_Julio
        te_Agosto
        te_Septiembre
        te_Octubre
        te_Noviembre
        te_Diciembre

    End Enum

    Private _MesesAnio As New List(Of teMesesAnio)
    Private _DiasSemana As New List(Of teDiasSemana)
    Private _DiaMes As New List(Of Integer)
    Private _HoraMinuto As String = "00:00"
    Private _Fecha As DateTime = Nothing
    Private _TimerFijo As Boolean

    Public Sub New()

        _TimerFijo = False
        _Timer.UnidadDeTiempo = clsTimer.UnidadesDeTiempo.Minutos
        _Timer.Frecuencia = 1

    End Sub

    Public Sub New(ByVal UnidadDeTiempo As clsTimer.UnidadesDeTiempo, ByVal Frecuencia As Integer)

        _TimerFijo = True
        _Timer.UnidadDeTiempo = UnidadDeTiempo
        _Timer.Frecuencia = Frecuencia

    End Sub

    Public Sub Iniciar()
        _Timer.Iniciar()
    End Sub

    Public Sub Detener()
        _Timer.Detener()
    End Sub

    Private Sub _Timer_TimerExec() Handles _Timer.TimerExec

        If _TimerFijo Then
            RaiseEvent EventoCumplido()
        Else
            If EsMesAnio() And EsDiaSemana() And EsDiaMes() And EsHoraMinuto() And EsFecha() Then RaiseEvent EventoCumplido()
        End If

    End Sub

    Private Function EsMesAnio() As Boolean
        If _MesesAnio.Count = 0 Then Return True
        For Each M As teMesesAnio In _MesesAnio
            If M = DateTime.Now.Month Then Return True
        Next
        Return False
    End Function

    Private Function EsDiaSemana() As Boolean
        If _DiasSemana.Count = 0 Then Return True
        For Each D As teDiasSemana In _DiasSemana
            If D = DateTime.Now.DayOfWeek Then Return True
        Next
        Return False
    End Function

    Private Function EsDiaMes() As Boolean
        If _DiaMes.Count = 0 Then Return True
        For Each D As Integer In _DiaMes
            If D = DateTime.Now.Day Then Return True
        Next
        Return False
    End Function

    Private Function EsFecha() As Boolean
        If _Fecha = Nothing Then Return True
        If _Fecha.ToString("dd/MM/yyyy hh:mm") = DateTime.Now.ToString("dd/MM/yyyy hh:mm") Then Return True
        Return False
    End Function

    Private Function EsHoraMinuto() As Boolean
        If _HoraMinuto.Length = 0 Then Return True
        If _HoraMinuto.Contains(":") Then
            Dim _Hora As Integer = _HoraMinuto.Split(":")(0)
            Dim _Minuto As Integer = _HoraMinuto.Split(":")(1)
            If _Hora = DateTime.Now.Hour And _Minuto = DateTime.Now.Minute Then Return True
        End If
        Return False
    End Function

    Public Property HoraMinuto() As String
        Get
            Return _HoraMinuto
        End Get
        Set(ByVal value As String)
            _HoraMinuto = value
        End Set
    End Property

    Public Property Fecha() As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
        End Set
    End Property

    Public Sub ClearDiaMes()
        _DiaMes.Clear()
    End Sub

    Public Sub ClearMesesAnio()
        _MesesAnio.Clear()
    End Sub

    Public Sub ClearDiasSemana()
        _DiasSemana.Clear()
    End Sub

    Public Sub AddDiaMes(ByVal DiaMes As Integer)
        _DiaMes.Add(DiaMes)
    End Sub

    Public Sub AddMesAnio(ByVal MesAnio As teMesesAnio)
        _MesesAnio.Add(MesAnio)
    End Sub

    Public Sub AddDiaSemana(ByVal DiaSemana As teDiasSemana)
        _DiasSemana.Add(DiaSemana)
    End Sub

End Class


