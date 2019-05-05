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
        Me.BtnPersona = New System.Windows.Forms.Button()
        Me.WebBrowserInput = New System.Windows.Forms.WebBrowser()
        Me.BtnLegajo = New System.Windows.Forms.Button()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnPersona
        '
        Me.BtnPersona.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPersona.Location = New System.Drawing.Point(21, 22)
        Me.BtnPersona.Name = "BtnPersona"
        Me.BtnPersona.Size = New System.Drawing.Size(107, 41)
        Me.BtnPersona.TabIndex = 0
        Me.BtnPersona.Text = "&Persona"
        Me.BtnPersona.UseVisualStyleBackColor = True
        '
        'WebBrowserInput
        '
        Me.WebBrowserInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrowserInput.Location = New System.Drawing.Point(163, 12)
        Me.WebBrowserInput.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowserInput.Name = "WebBrowserInput"
        Me.WebBrowserInput.Size = New System.Drawing.Size(861, 569)
        Me.WebBrowserInput.TabIndex = 11
        '
        'BtnLegajo
        '
        Me.BtnLegajo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnLegajo.Location = New System.Drawing.Point(21, 69)
        Me.BtnLegajo.Name = "BtnLegajo"
        Me.BtnLegajo.Size = New System.Drawing.Size(107, 41)
        Me.BtnLegajo.TabIndex = 12
        Me.BtnLegajo.Text = "&Legajo"
        Me.BtnLegajo.UseVisualStyleBackColor = True
        '
        'BtnSalir
        '
        Me.BtnSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.Blue
        Me.BtnSalir.Location = New System.Drawing.Point(21, 116)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(107, 41)
        Me.BtnSalir.TabIndex = 13
        Me.BtnSalir.Text = "&Salir"
        Me.BtnSalir.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1036, 593)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.BtnLegajo)
        Me.Controls.Add(Me.WebBrowserInput)
        Me.Controls.Add(Me.BtnPersona)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnPersona As Button
    Friend WithEvents WebBrowserInput As WebBrowser
    Friend WithEvents BtnLegajo As Button
    Friend WithEvents BtnSalir As Button
End Class
