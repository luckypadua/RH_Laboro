Imports System
Imports System.Net
Imports System.Net.Mail

Public Class Mail

    Public Shared Function Enviar(ByVal FileConfig As String, ByVal FROMNAME As String, ByVal [TO] As String, ByVal SUBJECT As String, ByVal HTMLBODY As String) As String

        Dim Cfg As MailConfig = New MailConfig(FileConfig)
        Dim CONFIGSET As String = "ConfigSet"
        Dim message As MailMessage = New MailMessage()
        message.IsBodyHtml = True
        message.From = New MailAddress(Cfg.FROM, FROMNAME)

        For Each xTO As String In [TO].Split(";"c)
            message.[To].Add(New MailAddress(xTO.Trim()))
        Next

        message.Subject = SUBJECT
        message.Body = HTMLBODY
        message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET)

        Using client = New System.Net.Mail.SmtpClient(Cfg.HOST, Cfg.PORT)
            client.Credentials = New NetworkCredential(Cfg.SMTP_USERNAME, Cfg.SMTP_PASSWORD)
            client.EnableSsl = Cfg.SSL

            Try
                client.Send(message)
            Catch ex As Exception
                Return $"Error : {ex.Message}"
            End Try
        End Using

        Return String.Empty

    End Function

End Class

