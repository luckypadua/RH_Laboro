
Public Class ClsFormatoHtml

    Private _UrlSufijo As String

    Public Sub New(ByVal UrlSufijo As String)

        _UrlSufijo = UrlSufijo

    End Sub

    Public Shared Function CarpetaFormatos() As String

        Dim FmtFolder As String = IO.Path.Combine(NETCoreLOG.ClsLogger.GetBaseFolderRegisry, "FormatosHtmlMails")
        Return FmtFolder

    End Function

    Public Shared Function ExisteCarpetaFormatos() As Boolean

        Return IO.Directory.Exists(CarpetaFormatos)

    End Function


    Private Function GetHtmlParametro(ByVal HtmlText As String, ByVal Nombre As String, ByVal Valor As String) As String

        Const tag1 As String = "&lt;&lt;"
        Const tag2 As String = "&gt;&gt"
        Dim TxtFind As String = String.Format("{0}{1}{2}", tag1, Nombre.Trim, tag2)
        Dim TxtReplace As String = Valor.Trim
        Return HtmlText.Replace(TxtFind, TxtReplace)

    End Function

    Public Function Fmt_Autorizacion_Licencia(ByVal LegajoNombre As String,
                                              ByVal LegajoNombreCompleto As String,
                                              ByVal LicenciaTipo As String,
                                              ByVal LicenciaFechaDesde As String,
                                              ByVal LicenciaFechaHasta As String,
                                              ByVal LicenciaCantidadDias As String,
                                              ByVal LicenciaObservaciones As String)

        '1-Autorizacion_Licencia.html
        '<<Legajo-Nombre>>
        '<<Legajo-NombreCompleto>>
        '<<Licencia-Tipo>>
        '<<Licencia-FechaDesde>>
        '<<Licencia-FechaHasta>>
        '<<Licencia-CantidadDias>>
        '<<Licencia-Observaciones>>

        Try

            Dim HtmlText As String = IO.File.ReadAllText(IO.Path.Combine(ClsFormatoHtml.CarpetaFormatos, "1-Autorizacion_Licencia.html"))
            HtmlText = HtmlText.Replace("<URLSUFIJO>", _UrlSufijo)
            HtmlText = GetHtmlParametro(HtmlText, "Legajo-Nombre", LegajoNombre)
            HtmlText = GetHtmlParametro(HtmlText, "Legajo-NombreCompleto", LegajoNombreCompleto)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-Tipo", LicenciaTipo)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-FechaDesde", LicenciaFechaDesde)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-FechaHasta", LicenciaFechaHasta)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-CantidadDias", LicenciaCantidadDias)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-Observaciones", LicenciaObservaciones)
            Return HtmlText

        Catch ex As Exception
            Throw New Exception($"NETCoreBLB.ClsFormatoHtml.Fmt_Autorizacion_Licencia {Environment.NewLine}ERROR: {ex.Message}")
        End Try

    End Function

    Public Function Fmt_Aprobacion_Licencia(ByVal LegajoNombre As String,
                                            ByVal LicenciaEstado As String,
                                            ByVal LicenciaFechaSolicitud As String,
                                            ByVal LicenciaTipo As String,
                                            ByVal LicenciaFechaDesde As String,
                                            ByVal LicenciaFechaHasta As String,
                                            ByVal LicenciaCantidadDias As String,
                                            ByVal LicenciaObservaciones As String)

        '2-Aprobacion_Licencia.html
        '<<Legajo-Nombre>>
        '<<Licencia-Estado>>
        '<<Licencia-FechaSolicitud>>
        '<<Licencia-Tipo>>
        '<<Licencia-FechaDesde>>
        '<<Licencia-FechaHasta>>
        '<<Licencia-CantidadDias>>
        '<<Licencia-Observaciones>>

        Try

            Dim HtmlText As String = IO.File.ReadAllText(IO.Path.Combine(ClsFormatoHtml.CarpetaFormatos, "2-Aprobacion_Licencia.html"))
            HtmlText = HtmlText.Replace("<URLSUFIJO>", _UrlSufijo)
            HtmlText = GetHtmlParametro(HtmlText, "Legajo-Nombre", LegajoNombre)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-Estado", LicenciaEstado)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-FechaSolicitud", LicenciaFechaSolicitud)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-Tipo", LicenciaTipo)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-FechaDesde", LicenciaFechaDesde)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-FechaHasta", LicenciaFechaHasta)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-CantidadDias", LicenciaCantidadDias)
            HtmlText = GetHtmlParametro(HtmlText, "Licencia-Observaciones", LicenciaObservaciones)
            Return HtmlText

        Catch ex As Exception
            Throw New Exception($"NETCoreBLB.ClsFormatoHtml.Fmt_Aprobacion_Licencia {Environment.NewLine}ERROR: {ex.Message}")
        End Try

    End Function

    Public Function Fmt_Recibos_Pendientes(ByVal LegajoNombre As String) As String

        '3-Recibos_Pendientes.html
        '<<Legajo-Nombre>>

        Try

            Dim HtmlText As String = IO.File.ReadAllText(IO.Path.Combine(ClsFormatoHtml.CarpetaFormatos, "3-Recibos_Pendientes.html"))
            HtmlText = HtmlText.Replace("<URLSUFIJO>", _UrlSufijo)
            HtmlText = GetHtmlParametro(HtmlText, "Legajo-Nombre", LegajoNombre)
            Return HtmlText

        Catch ex As Exception
            Throw New Exception($"NETCoreBLB.ClsFormatoHtml.Fmt_Recibos_Pendientes {Environment.NewLine}ERROR: {ex.Message}")
        End Try

    End Function

    Public Function Fmt_Recibos_Publicados(ByVal LegajoNombre As String) As String

        '4-Recibos_Publicados.html
        '<<Legajo-Nombre>>

        Try

            Dim HtmlText As String = IO.File.ReadAllText(IO.Path.Combine(ClsFormatoHtml.CarpetaFormatos, "4-Recibos_Publicados.html"))
            HtmlText = HtmlText.Replace("<URLSUFIJO>", _UrlSufijo)
            HtmlText = GetHtmlParametro(HtmlText, "Legajo-Nombre", LegajoNombre)
            Return HtmlText

        Catch ex As Exception
            Throw New Exception($"NETCoreBLB.ClsFormatoHtml.Fmt_Recibos_Publicados {Environment.NewLine}ERROR: {ex.Message}")
        End Try

    End Function

End Class
