using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NETCoreMail
{
    class MailConfig : MailCfg
    {
       
        string _FileConfig;

        public MailConfig()
        {
            string Ruta = System.IO.Path.GetDirectoryName(_FileConfig);
            System.IO.Directory.CreateDirectory(Ruta);
        }

        public MailConfig(string FileConfig)
        {
            _FileConfig = FileConfig;
            Read();
        }
                
        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(_FileConfig, json);
            }
            catch (Exception ex)
            {
              throw new Exception ($"Error en NETCoreMail.MailConfig.Save : {ex.Message}");
            }
        }

        private void Read()
        {
            try
            {

                if (!System.IO.File.Exists(_FileConfig))
                {
                    this.FROM = "alertasbas@bas.com.ar";
                    this.SMTP_USERNAME = "alertasbas";
                    this.SMTP_PASSWORD = "nuncacaduca";
                    this.SSL  = false;
                    this.HOST = "mail.bas.com.ar";
                    this.PORT = 587;
                    Save();
                    
                }

                else
                {
                    var json = System.IO.File.ReadAllText(_FileConfig);
                    MailConfig Cfg = JsonConvert.DeserializeObject<MailConfig>(json);
                    this.FROM = Cfg.FROM;
                    this.SMTP_USERNAME = Cfg.SMTP_USERNAME;
                    this.SMTP_PASSWORD = Cfg.SMTP_PASSWORD;
                    this.HOST = Cfg.HOST;
                    this.PORT = Cfg.PORT;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en NETCoreMail.MailConfig.Read : {ex.Message}");
            }
        }
    }

    class MailCfg
    {
        public  MailCfg()
        {

        }

        public MailCfg(string FROM,
                       string SMTP_USERNAME,
                       string SMTP_PASSWORD,
                       bool SSL,
                       string HOST,
                       int PORT)
        {
            this.FROM = FROM;
            this.SMTP_USERNAME = SMTP_USERNAME;
            this.SMTP_PASSWORD = SMTP_PASSWORD;
            this.SSL = SSL;
            this.HOST = HOST;
            this.PORT = PORT;
        }

        public string FROM = string.Empty;
        public string SMTP_USERNAME = string.Empty;
        public string SMTP_PASSWORD = string.Empty;
        public bool SSL = false;
        public string HOST = string.Empty;
        public int PORT = 0;
    }
}
