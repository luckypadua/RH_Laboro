Public Class frmProbador

    Dim RH As NETCoreBLB.ItzBASLaboro = New NETCoreBLB.ClsBASLaboro("SERVIDORBLB\SQL2014", "400BLB_Prueba", "sa", "sa")

    Private Sub BtnDatosPersonales_Click(sender As Object, e As EventArgs) Handles BtnDatosPersonales.Click
        Dim Ds As DataSet = RH.GetDatosPersonales(txtIdPersona.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub BtnLoguinsIni_Click(sender As Object, e As EventArgs) Handles BtnLoguinsIni.Click
        Dim Ds As DataSet = RH.GetLoguinsIni
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub BtnRecibos_Click(sender As Object, e As EventArgs) Handles BtnRecibos.Click
        Dim Ds As DataSet = RH.GetRecibos(txtIdPersona.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Protected Overrides Sub Finalize()
        RH.Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub BtnFirmar_Click(sender As Object, e As EventArgs) Handles BtnFirmar.Click

        RH.ReciboFirmado(txtIdLiquidacion.Text, cmbIdLegajo.Text, RbConforme.Checked, txtObservación.Text)
        BtnRecibos.PerformClick()

    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub BtnReciboDescargado_Click_1(sender As Object, e As EventArgs) Handles BtnReciboDescargado.Click

        RH.ReciboDescargado(txtIdLiquidacion.Text, cmbIdLegajo.Text)

    End Sub

    Private Sub BtnReciboDescarga_Click(sender As Object, e As EventArgs) Handles BtnReciboDescarga.Click
        Dim Ds As DataSet = RH.GetReciboDescarga(txtIdLiquidacion.Text, cmbIdLegajo.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub cmdManagersYEmpleados_Click(sender As Object, e As EventArgs) Handles cmdManagersYEmpleados.Click
        Dim Ds As DataSet = RH.GetManagers(txtIdLegajo.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub
End Class
