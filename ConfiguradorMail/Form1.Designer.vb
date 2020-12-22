<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.txtSufijoURL = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtRemitenteNombre = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRemitenteMail = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtServidor = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtContrasenia = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.BtmAceptar = New System.Windows.Forms.Button()
        Me.BtnCancelar = New System.Windows.Forms.Button()
        Me.chkHabilitarSSL = New System.Windows.Forms.CheckBox()
        Me.txtPuerto = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtConfirmar = New System.Windows.Forms.TextBox()
        CType(Me.txtPuerto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtSufijoURL
        '
        Me.txtSufijoURL.Location = New System.Drawing.Point(144, 33)
        Me.txtSufijoURL.Name = "txtSufijoURL"
        Me.txtSufijoURL.Size = New System.Drawing.Size(225, 20)
        Me.txtSufijoURL.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(41, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Sufijo URL:"
        '
        'txtRemitenteNombre
        '
        Me.txtRemitenteNombre.Location = New System.Drawing.Point(144, 60)
        Me.txtRemitenteNombre.Name = "txtRemitenteNombre"
        Me.txtRemitenteNombre.Size = New System.Drawing.Size(226, 20)
        Me.txtRemitenteNombre.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(40, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Remitente Nombre:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(40, 89)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Remitente Mail:"
        '
        'txtRemitenteMail
        '
        Me.txtRemitenteMail.Location = New System.Drawing.Point(144, 86)
        Me.txtRemitenteMail.Name = "txtRemitenteMail"
        Me.txtRemitenteMail.Size = New System.Drawing.Size(226, 20)
        Me.txtRemitenteMail.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(40, 141)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Puerto:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(40, 115)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Servidor:"
        '
        'txtServidor
        '
        Me.txtServidor.Location = New System.Drawing.Point(144, 112)
        Me.txtServidor.Name = "txtServidor"
        Me.txtServidor.Size = New System.Drawing.Size(226, 20)
        Me.txtServidor.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(40, 167)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Usuario:"
        '
        'txtUsuario
        '
        Me.txtUsuario.Location = New System.Drawing.Point(144, 164)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(226, 20)
        Me.txtUsuario.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(40, 193)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Contraseña:"
        '
        'txtContrasenia
        '
        Me.txtContrasenia.Location = New System.Drawing.Point(144, 190)
        Me.txtContrasenia.Name = "txtContrasenia"
        Me.txtContrasenia.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtContrasenia.Size = New System.Drawing.Size(226, 20)
        Me.txtContrasenia.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(40, 244)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Habilitar SSL:"
        '
        'BtmAceptar
        '
        Me.BtmAceptar.Image = CType(resources.GetObject("BtmAceptar.Image"), System.Drawing.Image)
        Me.BtmAceptar.Location = New System.Drawing.Point(261, 250)
        Me.BtmAceptar.Name = "BtmAceptar"
        Me.BtmAceptar.Size = New System.Drawing.Size(49, 39)
        Me.BtmAceptar.TabIndex = 12
        Me.BtmAceptar.UseVisualStyleBackColor = True
        '
        'BtnCancelar
        '
        Me.BtnCancelar.Image = CType(resources.GetObject("BtnCancelar.Image"), System.Drawing.Image)
        Me.BtnCancelar.Location = New System.Drawing.Point(320, 250)
        Me.BtnCancelar.Name = "BtnCancelar"
        Me.BtnCancelar.Size = New System.Drawing.Size(49, 39)
        Me.BtnCancelar.TabIndex = 13
        Me.BtnCancelar.UseVisualStyleBackColor = True
        '
        'chkHabilitarSSL
        '
        Me.chkHabilitarSSL.AutoSize = True
        Me.chkHabilitarSSL.Location = New System.Drawing.Point(144, 244)
        Me.chkHabilitarSSL.Name = "chkHabilitarSSL"
        Me.chkHabilitarSSL.Size = New System.Drawing.Size(15, 14)
        Me.chkHabilitarSSL.TabIndex = 8
        Me.chkHabilitarSSL.UseVisualStyleBackColor = True
        '
        'txtPuerto
        '
        Me.txtPuerto.Location = New System.Drawing.Point(144, 139)
        Me.txtPuerto.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.txtPuerto.Name = "txtPuerto"
        Me.txtPuerto.Size = New System.Drawing.Size(78, 20)
        Me.txtPuerto.TabIndex = 4
        Me.txtPuerto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(40, 219)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Confirmar:"
        '
        'txtConfirmar
        '
        Me.txtConfirmar.Location = New System.Drawing.Point(144, 216)
        Me.txtConfirmar.Name = "txtConfirmar"
        Me.txtConfirmar.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.txtConfirmar.Size = New System.Drawing.Size(226, 20)
        Me.txtConfirmar.TabIndex = 7
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(379, 298)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtConfirmar)
        Me.Controls.Add(Me.txtPuerto)
        Me.Controls.Add(Me.chkHabilitarSSL)
        Me.Controls.Add(Me.BtnCancelar)
        Me.Controls.Add(Me.BtmAceptar)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtContrasenia)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtUsuario)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtServidor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtRemitenteMail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtRemitenteNombre)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSufijoURL)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Configurador Mail"
        CType(Me.txtPuerto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSufijoURL As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtRemitenteNombre As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtRemitenteMail As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtServidor As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtUsuario As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtContrasenia As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents BtmAceptar As Button
    Friend WithEvents BtnCancelar As Button
    Friend WithEvents chkHabilitarSSL As CheckBox
    Friend WithEvents txtPuerto As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents txtConfirmar As TextBox
End Class
