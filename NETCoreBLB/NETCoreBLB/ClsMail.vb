﻿Imports System.Net.Mail
Imports NETCoreLOG
'Imports EASendMail

Public Class ClsMail

    Public Sub New()
        '
    End Sub

    Public Overloads Shared Function Enviar(ByVal Asunto As String,
                                            ByVal Mensaje As String,
                                            ByVal Destinatarios As List(Of String),
                                            ByVal DestinatariosCC As List(Of String),
                                            ByVal Adjuntos As List(Of String),
                                            ByVal MailRemitente As String,
                                            ByVal NombreRemitente As String,
                                            ByVal Servidor As String,
                                            ByVal Puerto As Integer,
                                            ByVal Usuario As String,
                                            ByVal Contrasenia As String,
                                            ByVal HabilitarSSL As Boolean,
                                            ByRef Resultado As String,
                                            ByVal Optional EsHtml As Boolean = True) As Boolean

        Dim Email As New MailMessage()
        Dim SMTPServer As New SmtpClient

        Try

            ClsLogger.Logueo.Loguear("*****************************************************")
            ClsLogger.Logueo.Loguear("Entró en enviar mail")
            ClsLogger.Logueo.Loguear("*****************************************************")
            ClsLogger.Logueo.Loguear("Asunto: " & Asunto)
            ClsLogger.Logueo.Loguear("Mensaje de alerta", ClsLogger.TiposDeLog.LogDeAlerta, "<Mensaje>" & Mensaje & "</Mensaje>")

            If Not Destinatarios Is Nothing Then
                For Each Destinatario As String In Destinatarios
                    ClsLogger.Logueo.Loguear("Destinatario: " & Destinatario)
                Next
            End If

            If Not DestinatariosCC Is Nothing Then
                For Each Destinatario As String In DestinatariosCC
                    ClsLogger.Logueo.Loguear("DestinatarioCC: " & Destinatario)
                Next
            End If

            If Not Adjuntos Is Nothing Then
                For Each Adjunto As String In Adjuntos
                    ClsLogger.Logueo.Loguear("Adjunto: " & Adjunto)
                Next
            End If

            ClsLogger.Logueo.Loguear("MailRemitente: " & MailRemitente)
            ClsLogger.Logueo.Loguear("NombreRemitente: " & NombreRemitente)
            ClsLogger.Logueo.Loguear("Servidor: " & Servidor)
            ClsLogger.Logueo.Loguear("Puerto: " & Puerto)
            ClsLogger.Logueo.Loguear("Usuario: " & Usuario)
            ClsLogger.Logueo.Loguear("HabilitarSSL: " & HabilitarSSL)

            Email.From = New MailAddress(MailRemitente, NombreRemitente, System.Text.Encoding.UTF8)

            If Not Adjuntos Is Nothing Then
                For Each Attachment As String In Adjuntos
                    Email.Attachments.Add(New Attachment(Attachment))
                Next
            End If

            If Not Destinatarios Is Nothing Then
                For Each Recipient As String In Destinatarios
                    Email.To.Add(Recipient)
                Next
            End If

            If Not DestinatariosCC Is Nothing Then
                For Each Recipient As String In DestinatariosCC
                    Email.CC.Add(Recipient)
                Next
            End If

            Email.Subject = Asunto
            Email.SubjectEncoding = System.Text.Encoding.UTF8
            Email.Body = Mensaje
            Email.BodyEncoding = System.Text.Encoding.UTF8
            Email.IsBodyHtml = EsHtml

            SMTPServer.Host = Servidor
            SMTPServer.Port = Puerto
            SMTPServer.Credentials = New System.Net.NetworkCredential(Usuario, Contrasenia)
            SMTPServer.EnableSsl = HabilitarSSL
            SMTPServer.Send(Email)

            Resultado = "El email fue enviado."
            ClsLogger.Logueo.Loguear("Resultado: " & Resultado)
            ClsLogger.Logueo.Loguear("*****************************************************")
            ClsLogger.Logueo.Loguear("Salió de enviar mail")
            ClsLogger.Logueo.Loguear("*****************************************************")

            Return True

        Catch ex As Exception

            Resultado = "El envío del mail ha fallado. " & ex.Message
            ClsLogger.Logueo.Loguear(Resultado, ClsLogger.TiposDeLog.LogDeError)
            Return False

        Finally

            Email.Dispose()
            SMTPServer.Dispose()

        End Try

    End Function

    'Public Shared Function Enviar(ByVal Asunto As String,
    '                              ByVal Mensaje As String,
    '                              ByVal Destinatarios As List(Of String),
    '                              ByVal DestinatariosCC As List(Of String),
    '                              ByVal Adjuntos As List(Of String),
    '                              ByVal MailRemitente As String,
    '                              ByVal NombreRemitente As String,
    '                              ByVal Servidor As String,
    '                              ByVal Puerto As Integer,
    '                              ByVal Usuario As String,
    '                              ByVal Contrasenia As String,
    '                              ByVal HabilitarSSL As Boolean,
    '                              ByRef Resultado As String,
    '                              ByVal Optional EsHtml As Boolean = True) As Boolean


    '    Try

    '        ClsLogger.Logueo.Loguear("Entró en enviar")
    '        ClsLogger.Logueo.Loguear("Asunto: " & Asunto)
    '        ClsLogger.Logueo.Loguear("Mensaje de alerta", ClsLogger.TiposDeLog.LogDeAlerta, "<Mensaje>" & Mensaje & "</Mensaje>")

    '        If Not Destinatarios Is Nothing Then
    '            For Each Destinatario As String In Destinatarios
    '                ClsLogger.Logueo.Loguear("Destinatario: " & Destinatario)
    '            Next
    '        End If

    '        If Not DestinatariosCC Is Nothing Then
    '            For Each Destinatario As String In DestinatariosCC
    '                ClsLogger.Logueo.Loguear("DestinatarioCC: " & Destinatario)
    '            Next
    '        End If

    '        If Not Adjuntos Is Nothing Then
    '            For Each Adjunto As String In Adjuntos
    '                ClsLogger.Logueo.Loguear("Adjunto: " & Adjunto)
    '            Next
    '        End If

    '        ClsLogger.Logueo.Loguear("MailRemitente: " & MailRemitente)
    '        ClsLogger.Logueo.Loguear("NombreRemitente: " & NombreRemitente)
    '        ClsLogger.Logueo.Loguear("Servidor: " & Servidor)
    '        ClsLogger.Logueo.Loguear("Puerto: " & Puerto)
    '        ClsLogger.Logueo.Loguear("Usuario: " & Usuario)
    '        ClsLogger.Logueo.Loguear("HabilitarSSL: " & HabilitarSSL)

    '        Dim oServer As New SmtpServer(Servidor)
    '        oServer.AuthType = SmtpAuthType.AuthAuto
    '        oServer.User = Usuario
    '        oServer.Password = Contrasenia
    '        oServer.Port = Puerto
    '        oServer.ConnectType = SmtpConnectType.ConnectSSLAuto

    '        Dim oMail As SmtpMail = New SmtpMail("TryIt")
    '        oMail.From = New MailAddress(MailRemitente)

    '        If Not Destinatarios Is Nothing Then
    '            For Each Destinatario As String In Destinatarios
    '                oMail.To.Add(New MailAddress(Destinatario))
    '            Next
    '        End If

    '        If Not DestinatariosCC Is Nothing Then
    '            For Each Destinatario As String In DestinatariosCC
    '                oMail.Cc.Add(Destinatario)
    '            Next
    '        End If

    '        If Not Adjuntos Is Nothing Then
    '            For Each Adjunto As String In Adjuntos
    '                oMail.AddAttachment(Adjunto)
    '            Next
    '        End If

    '        oMail.Subject = Asunto

    '        If EsHtml Then
    '            oMail.HtmlBody = Mensaje
    '        Else
    '            oMail.TextBody = Mensaje
    '        End If

    '        Dim oSmtp As EASendMail.SmtpClient = New SmtpClient()
    '        oSmtp.SendMail(oServer, oMail)

    '        Resultado = "El email fue enviado."
    '        ClsLogger.Logueo.Loguear("Resultado: " & Resultado)
    '        Return True

    '    Catch ex As Exception

    '        Resultado = "El envío del mail ha fallado. " & ex.Message
    '        ClsLogger.Logueo.Loguear(Resultado, ClsLogger.TiposDeLog.LogDeError)
    '        Return False

    '    Finally

    '    End Try

    'End Function

End Class
