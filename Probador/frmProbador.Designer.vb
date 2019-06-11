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
        Me.BtnFirmar = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtIdLegajo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtIdLiquidacion = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BtnReciboDescargado = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BtnReciboDescarga = New System.Windows.Forms.Button()
        Me.cmdManagersYEmpleados = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnDatosPersonales
        '
        Me.BtnDatosPersonales.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDatosPersonales.Location = New System.Drawing.Point(10, 70)
        Me.BtnDatosPersonales.Name = "BtnDatosPersonales"
        Me.BtnDatosPersonales.Size = New System.Drawing.Size(147, 41)
        Me.BtnDatosPersonales.TabIndex = 1
        Me.BtnDatosPersonales.Text = "&DatosPersonales"
        Me.BtnDatosPersonales.UseVisualStyleBackColor = True
        '
        'WebBrowserInput
        '
        Me.WebBrowserInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrowserInput.Location = New System.Drawing.Point(310, 12)
        Me.WebBrowserInput.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowserInput.Name = "WebBrowserInput"
        Me.WebBrowserInput.Size = New System.Drawing.Size(781, 569)
        Me.WebBrowserInput.TabIndex = 3
        '
        'BtnSalir
        '
        Me.BtnSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.Blue
        Me.BtnSalir.Location = New System.Drawing.Point(13, 558)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(125, 23)
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
        Me.BtnLoguinsIni.Size = New System.Drawing.Size(147, 41)
        Me.BtnLoguinsIni.TabIndex = 16
        Me.BtnLoguinsIni.Text = "&LoguinsIni"
        Me.BtnLoguinsIni.UseVisualStyleBackColor = True
        '
        'BtnRecibos
        '
        Me.BtnRecibos.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecibos.Location = New System.Drawing.Point(10, 164)
        Me.BtnRecibos.Name = "BtnRecibos"
        Me.BtnRecibos.Size = New System.Drawing.Size(147, 41)
        Me.BtnRecibos.TabIndex = 17
        Me.BtnRecibos.Text = "&Recibos"
        Me.BtnRecibos.UseVisualStyleBackColor = True
        '
        'RbConforme
        '
        Me.RbConforme.AutoSize = True
        Me.RbConforme.Checked = True
        Me.RbConforme.Location = New System.Drawing.Point(6, 19)
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
        Me.RbDisconforme.Location = New System.Drawing.Point(6, 42)
        Me.RbDisconforme.Name = "RbDisconforme"
        Me.RbDisconforme.Size = New System.Drawing.Size(84, 17)
        Me.RbDisconforme.TabIndex = 19
        Me.RbDisconforme.Text = "Disconforme"
        Me.RbDisconforme.UseVisualStyleBackColor = True
        '
        'txtObservación
        '
        Me.txtObservación.Location = New System.Drawing.Point(6, 65)
        Me.txtObservación.Name = "txtObservación"
        Me.txtObservación.Size = New System.Drawing.Size(138, 20)
        Me.txtObservación.TabIndex = 21
        '
        'BtnFirmar
        '
        Me.BtnFirmar.ForeColor = System.Drawing.Color.Blue
        Me.BtnFirmar.Location = New System.Drawing.Point(6, 91)
        Me.BtnFirmar.Name = "BtnFirmar"
        Me.BtnFirmar.Size = New System.Drawing.Size(135, 29)
        Me.BtnFirmar.TabIndex = 23
        Me.BtnFirmar.Text = "Firmar"
        Me.BtnFirmar.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RbConforme)
        Me.GroupBox1.Controls.Add(Me.BtnFirmar)
        Me.GroupBox1.Controls.Add(Me.RbDisconforme)
        Me.GroupBox1.Controls.Add(Me.txtObservación)
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(7, 284)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(148, 127)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Firmar recibo"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(12, 261)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "IdLegajo"
        '
        'txtIdLegajo
        '
        Me.txtIdLegajo.ForeColor = System.Drawing.Color.Blue
        Me.txtIdLegajo.Location = New System.Drawing.Point(88, 258)
        Me.txtIdLegajo.Name = "txtIdLegajo"
        Me.txtIdLegajo.Size = New System.Drawing.Size(59, 20)
        Me.txtIdLegajo.TabIndex = 30
        Me.txtIdLegajo.Text = "7856"
        Me.txtIdLegajo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(12, 238)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "IdLiquidación"
        '
        'txtIdLiquidacion
        '
        Me.txtIdLiquidacion.ForeColor = System.Drawing.Color.Blue
        Me.txtIdLiquidacion.Location = New System.Drawing.Point(88, 235)
        Me.txtIdLiquidacion.Name = "txtIdLiquidacion"
        Me.txtIdLiquidacion.Size = New System.Drawing.Size(59, 20)
        Me.txtIdLiquidacion.TabIndex = 28
        Me.txtIdLiquidacion.Text = "117"
        Me.txtIdLiquidacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnReciboDescargado)
        Me.GroupBox2.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox2.Location = New System.Drawing.Point(3, 417)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(148, 57)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Recibo descargado"
        '
        'BtnReciboDescargado
        '
        Me.BtnReciboDescargado.ForeColor = System.Drawing.Color.Blue
        Me.BtnReciboDescargado.Location = New System.Drawing.Point(9, 19)
        Me.BtnReciboDescargado.Name = "BtnReciboDescargado"
        Me.BtnReciboDescargado.Size = New System.Drawing.Size(133, 29)
        Me.BtnReciboDescargado.TabIndex = 23
        Me.BtnReciboDescargado.Text = "Recibo Descargado"
        Me.BtnReciboDescargado.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.BtnReciboDescarga)
        Me.GroupBox3.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox3.Location = New System.Drawing.Point(7, 480)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(148, 57)
        Me.GroupBox3.TabIndex = 32
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Recibo para descargar"
        '
        'BtnReciboDescarga
        '
        Me.BtnReciboDescarga.ForeColor = System.Drawing.Color.Blue
        Me.BtnReciboDescarga.Location = New System.Drawing.Point(9, 19)
        Me.BtnReciboDescarga.Name = "BtnReciboDescarga"
        Me.BtnReciboDescarga.Size = New System.Drawing.Size(133, 29)
        Me.BtnReciboDescarga.TabIndex = 23
        Me.BtnReciboDescarga.Text = "Recibo Descarga"
        Me.BtnReciboDescarga.UseVisualStyleBackColor = True
        '
        'cmdManagersYEmpleados
        '
        Me.cmdManagersYEmpleados.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdManagersYEmpleados.Location = New System.Drawing.Point(163, 70)
        Me.cmdManagersYEmpleados.Name = "cmdManagersYEmpleados"
        Me.cmdManagersYEmpleados.Size = New System.Drawing.Size(141, 67)
        Me.cmdManagersYEmpleados.TabIndex = 33
        Me.cmdManagersYEmpleados.Text = "Managers y Empleados a cargo"
        Me.cmdManagersYEmpleados.UseVisualStyleBackColor = True
        '
        'frmProbador
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1103, 593)
        Me.Controls.Add(Me.cmdManagersYEmpleados)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtIdLegajo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtIdLiquidacion)
        Me.Controls.Add(Me.GroupBox1)
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
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
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
    Friend WithEvents BtnFirmar As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtIdLegajo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtIdLiquidacion As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents BtnReciboDescargado As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents BtnReciboDescarga As Button
    Friend WithEvents cmdManagersYEmpleados As Button
End Class
