<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProbador
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
        Me.BtnDatosPersonales = New System.Windows.Forms.Button()
        Me.WebBrowserInput = New System.Windows.Forms.WebBrowser()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.txtIdPersona = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnLoguinsIni = New System.Windows.Forms.Button()
        Me.BtnRecibos = New System.Windows.Forms.Button()
        Me.RbConforme = New System.Windows.Forms.RadioButton()
        Me.RbDisconforme = New System.Windows.Forms.RadioButton()
        Me.txtObservación = New System.Windows.Forms.TextBox()
        Me.BtnDescargar = New System.Windows.Forms.Button()
        Me.BtnAceptar = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnDatosPersonales
        '
        Me.BtnDatosPersonales.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDatosPersonales.Location = New System.Drawing.Point(10, 70)
        Me.BtnDatosPersonales.Name = "BtnDatosPersonales"
        Me.BtnDatosPersonales.Size = New System.Drawing.Size(125, 41)
        Me.BtnDatosPersonales.TabIndex = 1
        Me.BtnDatosPersonales.Text = "&DatosPersonales"
        Me.BtnDatosPersonales.UseVisualStyleBackColor = True
        '
        'WebBrowserInput
        '
        Me.WebBrowserInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrowserInput.Location = New System.Drawing.Point(143, 12)
        Me.WebBrowserInput.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowserInput.Name = "WebBrowserInput"
        Me.WebBrowserInput.Size = New System.Drawing.Size(881, 569)
        Me.WebBrowserInput.TabIndex = 3
        '
        'BtnSalir
        '
        Me.BtnSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.Blue
        Me.BtnSalir.Location = New System.Drawing.Point(10, 216)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(125, 41)
        Me.BtnSalir.TabIndex = 2
        Me.BtnSalir.Text = "&Salir"
        Me.BtnSalir.UseVisualStyleBackColor = True
        '
        'txtIdPersona
        '
        Me.txtIdPersona.Location = New System.Drawing.Point(22, 35)
        Me.txtIdPersona.Name = "txtIdPersona"
        Me.txtIdPersona.Size = New System.Drawing.Size(100, 20)
        Me.txtIdPersona.TabIndex = 0
        Me.txtIdPersona.Text = "2173"
        Me.txtIdPersona.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "IdPersona"
        '
        'BtnLoguinsIni
        '
        Me.BtnLoguinsIni.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnLoguinsIni.Location = New System.Drawing.Point(10, 117)
        Me.BtnLoguinsIni.Name = "BtnLoguinsIni"
        Me.BtnLoguinsIni.Size = New System.Drawing.Size(125, 41)
        Me.BtnLoguinsIni.TabIndex = 16
        Me.BtnLoguinsIni.Text = "&LoguinsIni"
        Me.BtnLoguinsIni.UseVisualStyleBackColor = True
        '
        'BtnRecibos
        '
        Me.BtnRecibos.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecibos.Location = New System.Drawing.Point(10, 164)
        Me.BtnRecibos.Name = "BtnRecibos"
        Me.BtnRecibos.Size = New System.Drawing.Size(125, 41)
        Me.BtnRecibos.TabIndex = 17
        Me.BtnRecibos.Text = "&Recibos"
        Me.BtnRecibos.UseVisualStyleBackColor = True
        '
        'RbConforme
        '
        Me.RbConforme.AutoSize = True
        Me.RbConforme.Checked = True
        Me.RbConforme.Location = New System.Drawing.Point(7, 19)
        Me.RbConforme.Name = "RbConforme"
        Me.RbConforme.Size = New System.Drawing.Size(70, 17)
        Me.RbConforme.TabIndex = 18
        Me.RbConforme.TabStop = True
        Me.RbConforme.Text = "Conforme"
        Me.RbConforme.UseVisualStyleBackColor = True
        '
        'RbDisconforme
        '
        Me.RbDisconforme.AutoSize = True
        Me.RbDisconforme.Location = New System.Drawing.Point(7, 42)
        Me.RbDisconforme.Name = "RbDisconforme"
        Me.RbDisconforme.Size = New System.Drawing.Size(84, 17)
        Me.RbDisconforme.TabIndex = 19
        Me.RbDisconforme.Text = "Disconforme"
        Me.RbDisconforme.UseVisualStyleBackColor = True
        '
        'txtObservación
        '
        Me.txtObservación.Location = New System.Drawing.Point(0, 65)
        Me.txtObservación.Name = "txtObservación"
        Me.txtObservación.Size = New System.Drawing.Size(122, 20)
        Me.txtObservación.TabIndex = 21
        '
        'BtnDescargar
        '
        Me.BtnDescargar.ForeColor = System.Drawing.Color.Blue
        Me.BtnDescargar.Location = New System.Drawing.Point(7, 348)
        Me.BtnDescargar.Name = "BtnDescargar"
        Me.BtnDescargar.Size = New System.Drawing.Size(128, 29)
        Me.BtnDescargar.TabIndex = 22
        Me.BtnDescargar.Text = "Descargar"
        Me.BtnDescargar.UseVisualStyleBackColor = True
        '
        'BtnAceptar
        '
        Me.BtnAceptar.ForeColor = System.Drawing.Color.Blue
        Me.BtnAceptar.Location = New System.Drawing.Point(66, 91)
        Me.BtnAceptar.Name = "BtnAceptar"
        Me.BtnAceptar.Size = New System.Drawing.Size(56, 29)
        Me.BtnAceptar.TabIndex = 23
        Me.BtnAceptar.Text = "Aceptar"
        Me.BtnAceptar.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RbConforme)
        Me.GroupBox1.Controls.Add(Me.BtnAceptar)
        Me.GroupBox1.Controls.Add(Me.RbDisconforme)
        Me.GroupBox1.Controls.Add(Me.txtObservación)
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(7, 211)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(130, 131)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Firmar recibo"
        '
        'frmProbador
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1036, 593)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnDescargar)
        Me.Controls.Add(Me.BtnRecibos)
        Me.Controls.Add(Me.BtnLoguinsIni)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtIdPersona)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.WebBrowserInput)
        Me.Controls.Add(Me.BtnDatosPersonales)
        Me.Name = "frmProbador"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Probador"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnDatosPersonales As Button
    Friend WithEvents WebBrowserInput As WebBrowser
    Friend WithEvents BtnSalir As Button
    Friend WithEvents txtIdPersona As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnLoguinsIni As Button
    Friend WithEvents BtnRecibos As Button
    Friend WithEvents RbConforme As RadioButton
    Friend WithEvents RbDisconforme As RadioButton
    Friend WithEvents txtObservación As TextBox
    Friend WithEvents BtnDescargar As Button
    Friend WithEvents BtnAceptar As Button
    Friend WithEvents GroupBox1 As GroupBox
End Class
