﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProbador
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtIdLiquidacion = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BtnReciboVisualizado = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BtnReciboDescarga = New System.Windows.Forms.Button()
        Me.cmbIdLegajo = New System.Windows.Forms.ComboBox()
        Me.cmdManagersYEmpleados = New System.Windows.Forms.Button()
        Me.BtnRecibosDetalle = New System.Windows.Forms.Button()
        Me.txtJson = New System.Windows.Forms.TextBox()
        Me.frLicencia = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbTipoLicencia = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpFecHasta = New System.Windows.Forms.DateTimePicker()
        Me.dtpFecDesde = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmdSolicitarLicencia = New System.Windows.Forms.Button()
        Me.txtCantDias = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.frLicencia.SuspendLayout()
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
        Me.WebBrowserInput.Location = New System.Drawing.Point(299, 12)
        Me.WebBrowserInput.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowserInput.Name = "WebBrowserInput"
        Me.WebBrowserInput.Size = New System.Drawing.Size(792, 525)
        Me.WebBrowserInput.TabIndex = 3
        '
        'BtnSalir
        '
        Me.BtnSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.Blue
        Me.BtnSalir.Location = New System.Drawing.Point(13, 603)
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
        Me.txtIdPersona.Text = "118324"
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
        Me.BtnRecibos.Size = New System.Drawing.Size(72, 41)
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
        Me.txtIdLiquidacion.Text = "1488"
        Me.txtIdLiquidacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BtnReciboVisualizado)
        Me.GroupBox2.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox2.Location = New System.Drawing.Point(3, 417)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(148, 57)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Recibo visualizado"
        '
        'BtnReciboVisualizado
        '
        Me.BtnReciboVisualizado.ForeColor = System.Drawing.Color.Blue
        Me.BtnReciboVisualizado.Location = New System.Drawing.Point(9, 19)
        Me.BtnReciboVisualizado.Name = "BtnReciboVisualizado"
        Me.BtnReciboVisualizado.Size = New System.Drawing.Size(133, 29)
        Me.BtnReciboVisualizado.TabIndex = 23
        Me.BtnReciboVisualizado.Text = "Recibo Visualizado"
        Me.BtnReciboVisualizado.UseVisualStyleBackColor = True
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
        'cmbIdLegajo
        '
        Me.cmbIdLegajo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbIdLegajo.FormattingEnabled = True
        Me.cmbIdLegajo.Items.AddRange(New Object() {"8286", "7726", "7746", "7751", "53034", "76225", "79377", "116080", "117691", "113838", "115682"})
        Me.cmbIdLegajo.Location = New System.Drawing.Point(88, 258)
        Me.cmbIdLegajo.Name = "cmbIdLegajo"
        Me.cmbIdLegajo.Size = New System.Drawing.Size(59, 21)
        Me.cmbIdLegajo.TabIndex = 33
        '
        'cmdManagersYEmpleados
        '
        Me.cmdManagersYEmpleados.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdManagersYEmpleados.ForeColor = System.Drawing.Color.Black
        Me.cmdManagersYEmpleados.Location = New System.Drawing.Point(15, 543)
        Me.cmdManagersYEmpleados.Name = "cmdManagersYEmpleados"
        Me.cmdManagersYEmpleados.Size = New System.Drawing.Size(125, 54)
        Me.cmdManagersYEmpleados.TabIndex = 34
        Me.cmdManagersYEmpleados.Text = "&Managers y Empleados"
        Me.cmdManagersYEmpleados.UseVisualStyleBackColor = True
        '
        'BtnRecibosDetalle
        '
        Me.BtnRecibosDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecibosDetalle.Location = New System.Drawing.Point(85, 164)
        Me.BtnRecibosDetalle.Name = "BtnRecibosDetalle"
        Me.BtnRecibosDetalle.Size = New System.Drawing.Size(72, 41)
        Me.BtnRecibosDetalle.TabIndex = 35
        Me.BtnRecibosDetalle.Text = "&Detalle"
        Me.BtnRecibosDetalle.UseVisualStyleBackColor = True
        '
        'txtJson
        '
        Me.txtJson.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtJson.Location = New System.Drawing.Point(299, 543)
        Me.txtJson.Multiline = True
        Me.txtJson.Name = "txtJson"
        Me.txtJson.Size = New System.Drawing.Size(792, 83)
        Me.txtJson.TabIndex = 36
        '
        'frLicencia
        '
        Me.frLicencia.Controls.Add(Me.Label7)
        Me.frLicencia.Controls.Add(Me.cmbTipoLicencia)
        Me.frLicencia.Controls.Add(Me.Label6)
        Me.frLicencia.Controls.Add(Me.Label5)
        Me.frLicencia.Controls.Add(Me.dtpFecHasta)
        Me.frLicencia.Controls.Add(Me.dtpFecDesde)
        Me.frLicencia.Controls.Add(Me.Label4)
        Me.frLicencia.Controls.Add(Me.cmdSolicitarLicencia)
        Me.frLicencia.Controls.Add(Me.txtCantDias)
        Me.frLicencia.ForeColor = System.Drawing.Color.Blue
        Me.frLicencia.Location = New System.Drawing.Point(163, 19)
        Me.frLicencia.Name = "frLicencia"
        Me.frLicencia.Size = New System.Drawing.Size(130, 151)
        Me.frLicencia.TabIndex = 25
        Me.frLicencia.TabStop = False
        Me.frLicencia.Text = "Solicitar Licencia"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(6, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(28, 13)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Tipo"
        '
        'cmbTipoLicencia
        '
        Me.cmbTipoLicencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoLicencia.FormattingEnabled = True
        Me.cmbTipoLicencia.Location = New System.Drawing.Point(45, 19)
        Me.cmbTipoLicencia.Name = "cmbTipoLicencia"
        Me.cmbTipoLicencia.Size = New System.Drawing.Size(78, 21)
        Me.cmbTipoLicencia.TabIndex = 37
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(6, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "Desde"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(6, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 35
        Me.Label5.Text = "Hasta"
        '
        'dtpFecHasta
        '
        Me.dtpFecHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecHasta.Location = New System.Drawing.Point(45, 67)
        Me.dtpFecHasta.Name = "dtpFecHasta"
        Me.dtpFecHasta.Size = New System.Drawing.Size(79, 20)
        Me.dtpFecHasta.TabIndex = 34
        Me.dtpFecHasta.Value = New Date(2019, 7, 2, 0, 0, 0, 0)
        '
        'dtpFecDesde
        '
        Me.dtpFecDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecDesde.Location = New System.Drawing.Point(45, 44)
        Me.dtpFecDesde.Name = "dtpFecDesde"
        Me.dtpFecDesde.Size = New System.Drawing.Size(79, 20)
        Me.dtpFecDesde.TabIndex = 33
        Me.dtpFecDesde.Value = New Date(2019, 7, 2, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(6, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "Días"
        '
        'cmdSolicitarLicencia
        '
        Me.cmdSolicitarLicencia.ForeColor = System.Drawing.Color.Blue
        Me.cmdSolicitarLicencia.Location = New System.Drawing.Point(7, 116)
        Me.cmdSolicitarLicencia.Name = "cmdSolicitarLicencia"
        Me.cmdSolicitarLicencia.Size = New System.Drawing.Size(116, 29)
        Me.cmdSolicitarLicencia.TabIndex = 23
        Me.cmdSolicitarLicencia.Text = "Solicitar Licencia"
        Me.cmdSolicitarLicencia.UseVisualStyleBackColor = True
        '
        'txtCantDias
        '
        Me.txtCantDias.Location = New System.Drawing.Point(45, 90)
        Me.txtCantDias.Name = "txtCantDias"
        Me.txtCantDias.Size = New System.Drawing.Size(79, 20)
        Me.txtCantDias.TabIndex = 21
        '
        'frmProbador
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1103, 638)
        Me.Controls.Add(Me.frLicencia)
        Me.Controls.Add(Me.txtJson)
        Me.Controls.Add(Me.BtnRecibosDetalle)
        Me.Controls.Add(Me.cmdManagersYEmpleados)
        Me.Controls.Add(Me.cmbIdLegajo)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label3)
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
        Me.frLicencia.ResumeLayout(False)
        Me.frLicencia.PerformLayout()
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
    Friend WithEvents Label2 As Label
    Friend WithEvents txtIdLiquidacion As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents BtnReciboVisualizado As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents BtnReciboDescarga As Button
    Friend WithEvents cmbIdLegajo As ComboBox
    Friend WithEvents cmdManagersYEmpleados As Button
    Friend WithEvents BtnRecibosDetalle As Button
    Friend WithEvents txtJson As TextBox
    Friend WithEvents frLicencia As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpFecHasta As DateTimePicker
    Friend WithEvents dtpFecDesde As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents cmdSolicitarLicencia As Button
    Friend WithEvents txtCantDias As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbTipoLicencia As ComboBox
End Class
