Imports NETCoreBLB
Public Class FrmProbador

    Dim RH As NETCoreBLB.ItzBASLaboro = New NETCoreBLB.ClsBASLaboro("SERVIDORBLB\SQL2014", "400BLB_Prueba", "sa", "sa")
    Private MiAdo As New NETCoreADO.AdoNet("SERVIDORBLB\SQL2014", "400BLB_Prueba", "sa", "sa")

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

    Private Sub BtnRecibosDetalle_Click(sender As Object, e As EventArgs) Handles BtnRecibosDetalle.Click
        txtJson.Text = RH.GetRecibosDetalle(txtIdPersona.Text)
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

    Private Sub BtnReciboDescargado_Click_1(sender As Object, e As EventArgs) Handles BtnReciboVisualizado.Click

        RH.ReciboVisualizado(txtIdLiquidacion.Text, cmbIdLegajo.Text)

    End Sub

    Private Sub BtnReciboDescarga_Click(sender As Object, e As EventArgs) Handles BtnReciboDescarga.Click
        Dim Ds As DataSet = RH.GetReciboDescarga(txtIdLiquidacion.Text, cmbIdLegajo.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub CmdManagersYEmpleados_Click(sender As Object, e As EventArgs) Handles cmdManagersYEmpleados.Click
        Dim Ds As DataSet = RH.GetManagers(cmbIdLegajo.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub FrmProbador_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbIdLegajo.SelectedIndex = 0

        For Each Row As DataRow In RH.GetTipoLicencias.Tables.Item(0).Rows
            cmbTipoLicencia.Items.Add(Row("TipoLicencia"))
        Next

    End Sub
    Private Sub CmdSolicitarLicencia_Click(sender As Object, e As EventArgs) Handles cmdSolicitarLicencia.Click
        Dim DTAux As New DataTable()

        DTAux = MiAdo.Consultar.GetDataTable("SELECT s.IdSuceso, sc.IdClaseSuceso FROM Bl_Sucesos s JOIN Bl_SucesosClases sc ON s.IdClaseSuceso = sc.IdClaseSuceso WHERE s.CodSuceso = '" & cmbTipoLicencia.SelectedItem.ToString.Split("-")(0).Trim & "'", "Suceso")

        Dim DTRow As DataRow = DTAux.Rows(0)

        Dim nPedLic As New ClsPedidoLicencia(cmbIdLegajo.SelectedItem,
                                             DTRow("IdSuceso"),
                                             DTRow("IdClaseSuceso"),
                                             Now,
                                             dtpFecDesde.Value,
                                             dtpFecHasta.Value,
                                             CInt(txtCantDias.Text),
                                             cmbTipoLicencia.SelectedItem.ToString.Split("-")(0).Trim)

        RH.ValidarSolicitudLicencia(nPedLic)
    End Sub
End Class
