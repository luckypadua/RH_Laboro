Public Class frmProbador

    Dim RH As NETCoreBLB.ItzBASLaboro = New NETCoreBLB.ClsBASLaboro("SERVIDORBLB\SQL2014", "test360_BocaOrganigrama", "sa", "sa")

    Private Sub BtnDatosPersonales_Click(sender As Object, e As EventArgs) Handles BtnDatosPersonales.Click
        Dim Ds As DataSet = RH.GetDatosPersonales(txtIdPersona.Text)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)
    End Sub

    Protected Overrides Sub Finalize()
        RH.Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

End Class
