using System;
using System.Net;
using System.Net.Mail;

namespace NETCoreMail
{
    class NETCoreMail
    {
        static string Enviar(string FileConfig,
                             string FROMNAME,
                             string TO,
                             string SUBJECT,
                             string HTMLBODY)
        {
            MailConfig Cfg = new MailConfig();
            string CONFIGSET = "ConfigSet";

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(Cfg.FROM, FROMNAME);
            foreach (string xTO in TO.Split(';'))
            {
                message.To.Add(new MailAddress(xTO.Trim()));
            }
            message.Subject = SUBJECT;
            message.Body = HTMLBODY;

            message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(Cfg.HOST, Cfg.PORT))
            {
                client.Credentials = new NetworkCredential(Cfg.SMTP_USERNAME, Cfg.SMTP_PASSWORD);
                client.EnableSsl = Cfg.SSL;
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    return $"Error : {ex.Message}";
                }
            }

            return string.Empty; 
        }
    }
}