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

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles BtnAceptar.Click

        RH.ReciboFirmado(117, 7856, RbConforme.Checked, txtObservación.Text)
        BtnRecibos.PerformClick()

    End Sub

    Private Sub BtnDescargar_Click(sender As Object, e As EventArgs) Handles BtnDescargar.Click

        RH.ReciboDescargado(117, 7856)
        BtnRecibos.PerformClick()

    End Sub

End Class
