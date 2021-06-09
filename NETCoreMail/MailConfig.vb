Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Public Class MailConfig

    Inherits MailCfg

    Private _FileConfig As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal FileConfig As String)
        _FileConfig = FileConfig
        Dim Ruta As String = System.IO.Path.GetDirectoryName(_FileConfig)
        System.IO.Directory.CreateDirectory(Ruta)
        Read()
    End Sub

    Public Sub Save()
        Try
            Dim json = JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.Indented)
            System.IO.File.WriteAllText(_FileConfig, json)
        Catch ex As Exception
            Throw New Exception($"Error en NETCoreMail.MailConfig.Save : {ex.Message}")
        End Try
    End Sub

    Private Sub Read()
        Try

            If Not System.IO.File.Exists(_FileConfig) Then
                Me.FROM = "alertasbas@bas.com.ar"
                Me.SMTP_USERNAME = "alertasbas"
                Me.SMTP_PASSWORD = "nuncacaduca"
                Me.SSL = False
                Me.HOST = "mail.bas.com.ar"
                Me.PORT = 587
                Save()
            Else
                Dim json = System.IO.File.ReadAllText(_FileConfig)
                Dim Cfg As MailConfig = JsonConvert.DeserializeObject(Of MailConfig)(json)
                Me.FROM = Cfg.FROM
                Me.SMTP_USERNAME = Cfg.SMTP_USERNAME
                Me.SMTP_PASSWORD = Cfg.SMTP_PASSWORD
                Me.HOST = Cfg.HOST
                Me.PORT = Cfg.PORT
            End If

        Catch ex As Exception
            Throw New Exception($"Error en NETCoreMail.MailConfig.Read : {ex.Message}")
        End Try
    End Sub
End Class

Public Class MailCfg
    Public Sub New()

    End Sub

    Public Sub New(ByVal FROM As String, ByVal SMTP_USERNAME As String, ByVal SMTP_PASSWORD As String, ByVal SSL As Boolean, ByVal HOST As String, ByVal PORT As Integer)
        Me.FROM = FROM
        Me.SMTP_USERNAME = SMTP_USERNAME
        Me.SMTP_PASSWORD = SMTP_PASSWORD
        Me.SSL = SSL
        Me.HOST = HOST
        Me.PORT = PORT
    End Sub

    Public FROM As String = String.Empty
    Public SMTP_USERNAME As String = String.Empty
    Public SMTP_PASSWORD As String = String.Empty
    Public SSL As Boolean = False
    Public HOST As String = String.Empty
    Public PORT As Integer = 0
End Class

