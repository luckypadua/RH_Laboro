﻿Imports NETCoreBLB
Imports System.Globalization
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


        With MiAdo.Ejecutar.Parametros
            .RemoveAll()
            .Add("IdLegajo", 1000, SqlDbType.Int)
            .Add("FecSolicitud", "20170101", SqlDbType.DateTime)
            .Add("IdSuceso", 100, SqlDbType.Int)
            .Add("FecDesde", "20170101", SqlDbType.DateTime)
            .Add("FecHasta", "20170101", SqlDbType.DateTime)
            .Add("Cantidad", 20, SqlDbType.SmallInt)
            .Add("Observaciones", "xxxxxx", SqlDbType.VarChar)
        End With

        MiAdo.Ejecutar.Insertar("BL_NovedadesPedidos")


        MiAdo.Ejecutar.Insertar("BL_NovedadesPedidos")

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

        dtpFecDesde.CustomFormat = "dd/MM/yyyy"
        dtpFecHasta.CustomFormat = "dd/MM/yyyy"

    End Sub
    Private Sub CmdSolicitarLicencia_Click(sender As Object, e As EventArgs) Handles cmdSolicitarLicencia.Click
        Dim DTAux As New DataTable()

        DTAux = MiAdo.Consultar.GetDataTable("SELECT s.IdSuceso, sc.IdClaseSuceso FROM Bl_Sucesos s JOIN Bl_SucesosClases sc ON s.IdClaseSuceso = sc.IdClaseSuceso WHERE s.CodSuceso = '" & cmbTipoLicencia.SelectedItem.ToString.Split("-")(0).Trim & "'", "Suceso")

        Dim DTRow As DataRow = DTAux.Rows(0)

        Dim nPedLic As New ClsPedidoLicencia(MiAdo.Ejecutar.GetSQLInteger("SELECT IdLegajo FROM Bl_Legajos WHERE IdPersona = " & Me.txtIdPersona.Text & " AND CodEmp = 1"),
                                             DTRow("IdSuceso"),
                                             DTRow("IdClaseSuceso"),
                                             Now,
                                             dtpFecDesde.Text,
                                             dtpFecHasta.Text,
                                             CInt(txtCantDias.Text),
                                             cmbTipoLicencia.SelectedItem.ToString.Split("-")(0).Trim)

        If RH.ValidarSolicitudLicencia(nPedLic) Then RH.GrabarSolicitudLicencia(nPedLic)
    End Sub

    Private Sub cmdAceptarLicencia_Click(sender As Object, e As EventArgs) Handles cmdAceptarLicencia.Click
        RH.AceptarSolicitudLicencia(txtIdSolicitudLicencia.Text, 7746, "Nada por aqui")
    End Sub

    Private Sub cmdRechazarLicencia_Click(sender As Object, e As EventArgs) Handles cmdRechazarLicencia.Click
        RH.RechazarSolicitudLicencia(txtIdSolicitudLicencia.Text, 7746, "No te voy a permitir este pedido desacatao")
    End Sub

    Private Sub GetLicenciasEmpleadosACargo_Click(sender As Object, e As EventArgs) Handles GetLicenciasEmpleadosACargo.Click
        Dim Ds As DataSet = RH.GetSolicitudesLicenciasManager(txtIdPersona.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Private Sub cmdEliminarSolicitud_Click(sender As Object, e As EventArgs) Handles cmdEliminarSolicitud.Click
        RH.EliminarSolicitudLicencia(txtIdSolicitudLicencia.Text)
    End Sub
End Class
