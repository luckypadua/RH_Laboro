Imports NETCoreLOG

Public Class Form1

    Dim Cfg As New MailCfg

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Cfg.Leer()
        Cargar()

    End Sub

    Private Sub Cargar()

        Try
            txtSufijoURL.Text = Cfg.SufijoURL
            txtRemitenteNombre.Text = Cfg.RemitenteNombre
            txtRemitenteMail.Text = Cfg.Remitente
            txtServidor.Text = Cfg.Servidor
            txtPuerto.Text = Cfg.Puerto
            txtUsuario.Text = Cfg.Usuario
            txtContrasenia.Text = Cfg.Contrasenia
            txtConfirmar.Text = Cfg.Contrasenia
            chkHabilitarSSL.Checked = Cfg.HabilitarSSL
        Catch ex As Exception
            MsgBox($"Error en ConfiguradorMail.Cargar: {ex.Message}")
        End Try

    End Sub

    Private Sub Descargar()

        Try
            Cfg.SufijoURL = txtSufijoURL.Text.Trim
            Cfg.RemitenteNombre = txtRemitenteNombre.Text.Trim
            Cfg.Remitente = txtRemitenteMail.Text.Trim
            Cfg.Servidor = txtServidor.Text.Trim
            Cfg.Puerto = txtPuerto.Text
            Cfg.Usuario = txtUsuario.Text.Trim
            Cfg.Contrasenia = txtContrasenia.Text
            Cfg.HabilitarSSL = chkHabilitarSSL.Checked
        Catch ex As Exception
            MsgBox($"Error en ConfiguradorMail.Descargar: {ex.Message}")
        End Try

    End Sub

    Private Sub BtmAceptar_Click(sender As Object, e As EventArgs) Handles BtmAceptar.Click

        Try

            If txtContrasenia.Text <> txtConfirmar.Text Then
                MsgBox("Verifique la contraseña ingresada.", MsgBoxStyle.Exclamation, "Verificar contraseña")
                txtContrasenia.Text = String.Empty
                txtConfirmar.Text = String.Empty
                txtContrasenia.Focus()
                Exit Sub
            End If

            Descargar()
            Cfg.Grabar()

        Catch ex As Exception
            MsgBox($"Error en ConfiguradorMail.BtnAceptar: {ex.Message}")
        End Try

        Me.Close()

    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Close()
    End Sub

    Private Sub txtSufijoURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSufijoURL.KeyPress,
                                                                                        txtRemitenteNombre.KeyPress,
                                                                                        txtRemitenteMail.KeyPress,
                                                                                        txtServidor.KeyPress,
                                                                                        txtPuerto.KeyPress,
                                                                                        txtUsuario.KeyPress,
                                                                                        txtContrasenia.KeyPress,
                                                                                        txtConfirmar.KeyPress,
                                                                                        chkHabilitarSSL.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}")

    End Sub

End Class

Public Class MailCfg

    Dim FileName As String = IO.Path.Combine(NETCoreLOG.ClsLogger.GetBaseFolderRegisry, "MailCfg.xml")
    Const Semilla As String = "tir4n0sAuri0"

    Public Sub New()


    End Sub

    Public Sub Leer()

        Try

            If IO.File.Exists(FileName) Then

                ClsLogger.Logueo.Loguear($"NETCoreBLB.MailCfg.Leer (EXISTE): {FileName}")
                Dim s As String = IO.File.ReadAllText(FileName)
                Dim Cfg As MailCfg = s.Deserializar(Me.GetType)
                RemitenteNombre = Cfg.RemitenteNombre
                Remitente = Cfg.Remitente
                Servidor = Cfg.Servidor
                Puerto = Cfg.Puerto
                Usuario = Cfg.Usuario
                Contrasenia = NETCoreCrypto.ClsCrypto.AES_Decrypt(Cfg.Contrasenia, Semilla)
                HabilitarSSL = Cfg.HabilitarSSL
                SufijoURL = Cfg.SufijoURL
                ClsLogger.Logueo.Loguear($"NETCoreBLB.MailCfg.Leer SufijoURL : {Cfg.SufijoURL}")
                Cfg = Nothing

            Else

                ClsLogger.Logueo.Loguear($"NETCoreBLB.MailCfg.Leer (NO EXISTE): {FileName}")
                Dim Cfg As MailCfg = New MailCfg
                ClsLogger.Logueo.Loguear($"NETCoreBLB.MailCfg.Leer SufijoURL : {Cfg.SufijoURL}")
                Cfg.Grabar()

            End If

        Catch ex As Exception
            'Nada
        End Try

    End Sub

    Public Sub Grabar()

        Contrasenia = NETCoreCrypto.ClsCrypto.AES_Encrypt(Contrasenia, Semilla)
        IO.File.WriteAllText(FileName, Me.Serializar)

    End Sub

    Public Property RemitenteNombre As String = "BASLaboroAlerta"
    Public Property Remitente As String = "alertasbas@bas.com.ar"
    Public Property Servidor As String = "mail.bas.com.ar"
    Public Property Puerto As Integer = 25
    Public Property Usuario As String = "alertasbas"
    Public Property Contrasenia As String = "nuncacaduca"
    Public Property SufijoURL As String = "BAS"
    Public Property HabilitarSSL As Boolean = True

End Class
