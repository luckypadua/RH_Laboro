Public Class Form1

    ' Hola Erizo Cachola

    Dim RH As NETCoreBLB.ItzBASLaboro = New NETCoreBLB.ClsBASLaboro("SERVIDORBLB\SQL2014", "test360_BocaOrganigrama", "sa", "sa")
    Dim IdPersona As Integer = 2883

    Private Sub BtnPersona_Click(sender As Object, e As EventArgs) Handles BtnPersona.Click

        Dim Ds As DataSet = RH.GetPersona(IdPersona)
        Me.WebBrowserInput.DocumentStream = GetStream(Ds.GetXml)

    End Sub

    Private Sub BtnLegajo_Click(sender As Object, e As EventArgs) Handles BtnLegajo.Click

        Dim Ds As DataSet = RH.GetLegajo(IdPersona)
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
