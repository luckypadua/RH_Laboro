Imports System.Data
Imports System.Data.SqlClient

Public Class AdoNet

    Implements IDisposable

    Public Enum TipoDeRetornoEjecutar
        ReturnValue = 0
        ReturnScalar
        NotReturn
        ReturnDataset
    End Enum

    Private mConn As SqlConnection
    Private mConfigurar As clsConfigurar
    Private mEjecutar As clsEjecutar
    Private mConsultar As clsConsultar
    Private mTransaccion As clsTransaccion

    Private _SQLError As String

    Public Sub New()
        '
    End Sub

    Public ReadOnly Property SQLError
        Get
            Return _SQLError
        End Get
    End Property

    Public Sub New(ByVal Config As clsConfigurar)

        With Configurar
            .Server = Config.Server
            .Database = Config.Database
            .Uid = Config.Uid
            .Pwd = Config.Pwd
            .ConnectionTimeout = Config.ConnectionTimeout
        End With

    End Sub

    Public Sub New(ByVal Server As String,
                   ByVal Database As String,
                   ByVal Uid As String,
                   ByVal Pwd As String,
                   Optional ByVal ConnectionTimeout As Integer = 15)

        With Configurar
            .Server = Server
            .Database = Database
            .Uid = Uid
            .Pwd = Pwd
            .ConnectionTimeout = ConnectionTimeout
        End With

    End Sub

    Private Function _OpenConn(ByVal _Conn As SqlConnection, ByVal _Configurar As clsConfigurar) As SqlConnection

        Try

            If _Conn Is Nothing Then

                _Conn = New SqlConnection
                _Conn.ConnectionString = _Configurar.ConnectionString
                _Conn.Open()

            Else

                If _Conn.State = ConnectionState.Broken Or
                   _Conn.State = ConnectionState.Closed Then

                    _Conn.Dispose()
                    _Conn = New SqlConnection(_Configurar.ConnectionString)
                    _Conn.Open()

                End If

            End If

            Return _Conn

        Catch ex As Exception
            _SQLError = ex.Message
            Throw New Exception("Error al crear la conexión: " & ex.Message)
        End Try

    End Function

    Private Sub _CloseConn(ByRef _Conn As SqlConnection)

        Try

            If Not _Conn Is Nothing Then
                If _Conn.State = Data.ConnectionState.Open Then _Conn.Close()
                _Conn.Dispose() : _Conn = Nothing
            End If

        Catch ex As Exception
            _SQLError = ex.Message
            Throw New Exception("Error al cerrar la conexión: " & ex.Message)
        End Try

    End Sub

    Friend Function Conn() As SqlConnection

        Try

            If mConfigurar Is Nothing Then Throw New Exception("Error al configurar la conexión.")
            mConn = _OpenConn(mConn, mConfigurar)
            Conn = mConn

        Catch ex As Exception
            _SQLError = ex.Message
            Throw New Exception("Error al obtener conexión: " & ex.Message)
        End Try

    End Function

    Friend Sub CloseConn()

        _CloseConn(mConn)

    End Sub

    Public Function CheckConexion(Optional ByRef SQLErr As String = "") As Boolean
        Try
            Call Conn()
            Return True
        Catch ex As Exception
            SQLErr = Me.SQLError
            Return False
        End Try
    End Function

    Public Function CheckConexion(ByVal Server As String,
                                  ByVal Database As String,
                                  ByVal Uid As String,
                                  ByVal Pwd As String) As Boolean

        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, Database, Uid, Pwd)
        Try
            _Conn = _OpenConn(_Conn, _Configurar)
            Return True
        Catch ex As Exception
            Return False
        Finally
            _Configurar = Nothing
            _CloseConn(_Conn)
        End Try

    End Function

    ''' <summary>
    ''' Devuelve si existe la tabla en la base de datos
    ''' </summary>
    ''' <param name="Server"></param>
    ''' <param name="Database"></param>
    ''' <param name="Uid"></param>
    ''' <param name="Pwd"></param>
    ''' <param name="NombreTabla">Nombre de tabla que se quiere verificar si existe</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteTabla(ByVal Server As String,
                                ByVal Database As String,
                                ByVal Uid As String,
                                ByVal Pwd As String,
                                ByVal NombreTabla As String) As Boolean

        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, Database, Uid, Pwd)

        Dim SQL As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " &
                            "WHERE TABLE_TYPE = 'BASE TABLE' " &
                            "AND TABLE_NAME = @NombreTabla"

        Try

            _Conn = _OpenConn(_Conn, _Configurar)
            Dim cmd As New SqlCommand(SQL, _Conn)
            cmd.Parameters.AddWithValue("@NombreTabla", NombreTabla)
            Return CInt(cmd.ExecuteScalar()) > 0

        Catch ex As Exception
            Return False
        Finally
            _Configurar = Nothing
            _CloseConn(_Conn)
        End Try

    End Function

    ''' <summary>
    ''' Devuelve si existe la tabla en la base de datos
    ''' </summary>
    ''' <param name="NombreTabla">Nombre de tabla que se quiere verificar si existe</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteTabla(ByVal NombreTabla As String) As Boolean

        Dim SQL As String = " SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES" &
                            " WHERE TABLE_TYPE = 'BASE TABLE'" &
                            " AND TABLE_NAME = @NombreTabla"

        Try

            Dim cmd As New SqlCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@NombreTabla", NombreTabla)
            Return CInt(cmd.ExecuteScalar()) > 0

        Catch ex As Exception

            Return False

        End Try

    End Function

    ''' <summary>
    ''' Devuelve si existe un campo en una tabla
    ''' </summary>
    ''' <param name="Server"></param>
    ''' <param name="Database"></param>
    ''' <param name="Uid"></param>
    ''' <param name="Pwd"></param>
    ''' <param name="NombreTabla"></param>
    ''' <param name="NombreCampo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteCampo(ByVal Server As String,
                                ByVal Database As String,
                                ByVal Uid As String,
                                ByVal Pwd As String,
                                ByVal NombreTabla As String,
                                ByVal NombreCampo As String) As Boolean

        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, Database, Uid, Pwd)

        Dim SQL As String = " SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS" &
                            " WHERE COLUMN_NAME = @NombreCampo And TABLE_NAME = @NombreTabla"

        Try

            _Conn = _OpenConn(_Conn, _Configurar)
            Dim cmd As New SqlCommand(SQL, _Conn)
            cmd.Parameters.AddWithValue("@NombreTabla", NombreTabla)
            cmd.Parameters.AddWithValue("@NombreCampo", NombreCampo)
            Return CInt(cmd.ExecuteScalar()) > 0

        Catch ex As Exception
            Return False
        Finally
            _Configurar = Nothing
            _CloseConn(_Conn)
        End Try

    End Function

    ''' <summary>
    ''' Devuelve si existe un campo en una tabla
    ''' </summary>
    ''' <param name="NombreTabla"></param>
    ''' <param name="NombreCampo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteCampo(ByVal NombreTabla As String, ByVal NombreCampo As String) As Boolean

        Dim SQL As String = " SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS" &
                            " WHERE COLUMN_NAME = @NombreCampo And TABLE_NAME = @NombreTabla"

        Try

            Dim cmd As New SqlCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@NombreTabla", NombreTabla)
            cmd.Parameters.AddWithValue("@NombreCampo", NombreCampo)
            Return CInt(cmd.ExecuteScalar()) > 0

        Catch ex As Exception

            Return False

        End Try

    End Function

    ''' <summary>
    ''' Crea una base de datos en la instancia actual
    ''' </summary>
    ''' <param name="BaseDeDatos"></param>
    ''' <param name="DataName"></param>
    ''' <param name="FileDataName"></param>
    ''' <param name="FileDataSize"></param>
    ''' <param name="FileDataMaxSize"></param>
    ''' <param name="FileDataGrowth"></param>
    ''' <param name="LogName"></param>
    ''' <param name="FileLogName"></param>
    ''' <param name="FileLogSize"></param>
    ''' <param name="FileLogMaxSize"></param>
    ''' <param name="FileLogGrowth"></param>
    ''' <remarks></remarks>
    Public Sub CrearBaseDeDatos(ByVal Server As String,
                                ByVal Uid As String,
                                ByVal Pwd As String,
                                ByVal BaseDeDatos As String,
                                ByVal DataName As String, ByVal FileDataName As String, ByVal FileDataSize As String, ByVal FileDataMaxSize As String, ByVal FileDataGrowth As String,
                                ByVal LogName As String, ByVal FileLogName As String, ByVal FileLogSize As String, ByVal FileLogMaxSize As String, ByVal FileLogGrowth As String)

        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, String.Empty, Uid, Pwd)

        Dim SQL As String = String.Format("CREATE DATABASE {0} ON PRIMARY " &
                                          "(NAME = {1}, FILENAME = '{2}', SIZE = {3}, MAXSIZE = {4}, FILEGROWTH = {5}) LOG ON " &
                                          "(NAME = {6}, FILENAME = '{7}', SIZE = {8}, MAXSIZE = {9}, FILEGROWTH = {10}) ",
                                          BaseDeDatos,
                                          DataName, FileDataName, FileDataSize, FileDataMaxSize, FileDataGrowth,
                                          LogName, FileLogName, FileLogSize, FileLogMaxSize, FileLogGrowth)

        Try

            _Conn = _OpenConn(_Conn, _Configurar)
            Dim Cmd As SqlCommand = New SqlCommand(SQL, _Conn)
            Cmd.ExecuteNonQuery()

        Catch ex As Exception

            Throw New Exception("Error al crear base de datos: " & ex.Message)

        Finally

            _Configurar = Nothing
            _CloseConn(_Conn)

        End Try

    End Sub

    ''' <summary>
    ''' Devuelve si existe la base de datos en la instancia actual
    ''' </summary>
    ''' <param name="BaseDeDatos">Especifica el nombre de la base de datos</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteBaseDeDatos(ByVal Server As String,
                                      ByVal Uid As String,
                                      ByVal Pwd As String,
                                      ByVal BaseDeDatos As String) As Boolean

        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, String.Empty, Uid, Pwd)

        Try

            _Conn = _OpenConn(_Conn, _Configurar)
            Dim cmdText As String = (String.Format("select * from master.dbo.sysdatabases where name= '{0}'", BaseDeDatos))
            Dim bRet As Boolean = False
            Using Cmd As SqlCommand = New SqlCommand(cmdText, _Conn)
                Using reader As SqlDataReader = Cmd.ExecuteReader
                    bRet = reader.HasRows
                End Using
            End Using

            Return bRet

        Catch ex As Exception

            Throw New Exception("Error en ExisteBaseDeDatos: " & ex.Message)
            Return False

        Finally

            _Configurar = Nothing
            _CloseConn(_Conn)

        End Try

    End Function

    Public Function ExisteBaseDeDatos(Ado As AdoNet, ByVal BaseDeDatos As String) As Boolean

        Try

            Dim cmdText As String = (String.Format("select * from master.dbo.sysdatabases where name= '{0}'", BaseDeDatos))
            Dim bRet As Boolean = False
            Using Cmd As SqlCommand = New SqlCommand(cmdText, Ado.Conn)
                Using reader As SqlDataReader = Cmd.ExecuteReader
                    bRet = reader.HasRows
                End Using
            End Using

            Return bRet

        Catch ex As Exception

            Throw New Exception("Error en ExisteBaseDeDatos: " & ex.Message)

        End Try

    End Function

    Public Function GetBasesDeDatos(ByVal Server As String,
                                    ByVal Uid As String,
                                    ByVal Pwd As String) As List(Of String)

        Dim Bases As New List(Of String)
        Dim _Conn As SqlConnection = Nothing
        Dim _Configurar As New clsConfigurar(Server, "Master", Uid, Pwd)

        Try

            _Conn = _OpenConn(_Conn, _Configurar)
            Dim cmdText As String = "Select name from dbo.sysdatabases Order by name"
            Dim bRet As Boolean = False
            Using Cmd As SqlCommand = New SqlCommand(cmdText, _Conn)
                Using reader As SqlDataReader = Cmd.ExecuteReader
                    Do While reader.Read()
                        If Not reader.IsDBNull(0) Then
                            Dim Nombre As String = reader.GetString(0)
                            Bases.Add(Nombre)
                        End If
                    Loop
                End Using
            End Using

            Return Bases

        Catch ex As Exception

            Throw New Exception("Error en GetBasesDeDatos: " & ex.Message)

        Finally

            _Configurar = Nothing
            _CloseConn(_Conn)

        End Try

    End Function

    ''' <summary>
    ''' Devuelve si el usuario de la conexión actual, es DBOwner de la base
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EsUsuarioDBOwner() As Boolean

        Try

            Dim cmd As SqlCommand = New SqlCommand("SELECT is_member('db_owner')", Conn)
            Dim result As Object = cmd.ExecuteScalar
            Return CType(result, Int32) = 1

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Devuelve la instrucción transact sql de creacion de un campo de una tabla 
    ''' </summary>
    ''' <param name="Tabla">Tabla a la que pertenecerá el campo a crear</param>
    ''' <param name="Campo">Nuevo campo a crear</param>
    ''' <param name="Tipo">Tipo de dato del nuevo campo</param>
    ''' <param name="ValorDefecto">Si lo especifica, el campo no admitirá valor nulo, caso contrario, si lo hará</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SQLCrearCampo(ByVal Tabla As String,
                                  ByVal Campo As String,
                                  ByVal Tipo As String,
                                  Optional ByVal ValorDefecto As String = Nothing) As String

        Dim SiExiste As String = String.Format("If (Select Count(*) from SysObjects " & vbCrLf &
                                               "  Inner Join SysColumns On SysObjects.Id = SysColumns.id" & vbCrLf &
                                               "  Where SysObjects.name = '{0}'" & vbCrLf &
                                               "  And SysColumns.name = '{1}') = 0" & vbCrLf,
                                               Tabla, Campo)

        If ValorDefecto Is Nothing Then

            Return SiExiste & String.Format("  Alter Table [{0}] Add [{1}] {2} Null", Tabla, Campo, Tipo)

        Else

            Return SiExiste & String.Format("  Alter Table [{0}] Add [{1}] {2} Not Null Default " & ValorDefecto, Tabla, Campo, Tipo)

        End If

    End Function

    ''' <summary>
    ''' Permite configurar datos de conexión a la base de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Configurar() As clsConfigurar
        Get
            If mConfigurar Is Nothing Then
                mConfigurar = New clsConfigurar
            End If
            Return mConfigurar
        End Get
        Set(ByVal value As clsConfigurar)
            mConfigurar = value
        End Set
    End Property

    ''' <summary>
    ''' Permite ingrersar a funcionalidades de actualización de base de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Ejecutar() As ClsEjecutar
        If mEjecutar Is Nothing Then
            mEjecutar = New ClsEjecutar()
            mEjecutar.Parent(Me)
        End If
        Ejecutar = mEjecutar
    End Function

    ''' <summary>
    ''' Permite ingrersar a funcionalidades para consulta la base de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Consultar() As clsConsultar
        If mConsultar Is Nothing Then
            mConsultar = New clsConsultar()
            mConsultar.Parent(Me)
        End If
        Consultar = mConsultar
    End Function

    ''' <summary>
    ''' Permite abrir y cerrar una transaccion con la base de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Transaccion() As clsTransaccion
        If mTransaccion Is Nothing Then
            mTransaccion = New clsTransaccion()
            mTransaccion.Parent(Me)
        End If
        Transaccion = mTransaccion
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.

            CloseConn()
            mConfigurar = Nothing
            mEjecutar = Nothing
            mConsultar = Nothing
            mTransaccion = Nothing

        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' Libera el espacio de memoria ocupado por el objeto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

Public Class clsTransaccion

    Private mParent As AdoNet

    Friend Sub Parent(ByRef MiCon As AdoNet)
        mParent = MiCon
    End Sub

    ''' <summary>
    ''' Abre una transacción
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BeginTransaction()
        mParent.Ejecutar.Instruccion("Begin Transaction")
        mParent.Ejecutar.Instruccion("SET IMPLICIT_TRANSACTIONS ON")
    End Sub

    ''' <summary>
    ''' Confirma la transacción activa
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CommitTransaction()
        mParent.Ejecutar.Instruccion("Commit Transaction")
        mParent.Ejecutar.Instruccion("SET IMPLICIT_TRANSACTIONS OFF")
    End Sub

    ''' <summary>
    ''' Deja sin efecto la actualización de la Base de Datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RollBackTransaction()
        mParent.Ejecutar.Instruccion("RollBack Transaction")
        mParent.Ejecutar.Instruccion("SET IMPLICIT_TRANSACTIONS OFF")
    End Sub


    ''' <summary>
    ''' Devuelve el número de transacciones abiertas que se han producido en la conexión actual.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TranCount() As Long

        TranCount = 0
        Dim reader As SqlDataReader = mParent.Consultar.GetDataReader("Select CAST(@@TRANCOUNT AS NVARCHAR(10))")
        If reader.HasRows Then
            Do While reader.Read()
                If Not reader.IsDBNull(0) Then
                    TranCount = CInt(reader.GetString(0))
                End If
            Loop
        End If
        If Not reader.IsClosed Then reader.Close()

    End Function

End Class

Public Class clsConfigurar

    Private mServer As String
    Private mDatabase As String
    Private mUid As String
    Private mPwd As String
    Private mConnectionTimeout As Integer
    Private mConnectionString As String = String.Empty

    Public Sub New()
        '
    End Sub

    Public Sub New(ByVal Server As String,
                   ByVal Database As String,
                   ByVal Uid As String,
                   ByVal Pwd As String,
                   Optional ByVal ConnectionTimeout As Integer = 15)

        mServer = Server
        mDatabase = Database
        mUid = Uid
        mPwd = Pwd
        mConnectionTimeout = ConnectionTimeout

    End Sub

    ''' <summary>
    ''' Nombre del servidor SQL Server
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Server() As String

    ''' <summary>
    ''' Nombre de la base de datos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Database() As String

    ''' <summary>
    ''' Usuario de la base de datos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Uid() As String

    ''' <summary>
    ''' Contraseña de la base de datos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Pwd() As String

    ''' <summary>
    ''' Tiempo de espera para realizar la conexión
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionTimeout() As Integer

    ''' <summary>
    ''' Retorna el string de conexión a la base de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConnectionString() As String

        If mConnectionString.Length > 0 Then Return mConnectionString

        If Me.Uid.Length = 0 Then
            ConnectionString = String.Format("Data Source={0}; Database={1}; Integrated Security=SSPI", Me.Server, Me.Database)
        Else
            ConnectionString = String.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3}", Me.Server, Me.Database, Me.Uid, Me.Pwd)
        End If

        If mConnectionTimeout > 0 Then
            ConnectionString &= ";Connect Timeout=" & Me.ConnectionTimeout
        End If

    End Function

    Public Sub ConnectionString(ByVal Valor As String)
        mConnectionString = Valor
    End Sub

End Class

Public Class clsConsultar

    Private mParent As AdoNet

    Friend Sub Parent(ByRef MiCon As AdoNet)
        mParent = MiCon
    End Sub

    ''' <summary>
    ''' Ejecuta la consulta y retorna un DataTable
    ''' </summary>
    ''' <param name="Script">Script de consulta SQL</param>
    ''' <param name="Tabla">Nombre de la tabla</param>
    ''' <returns>Retorna un DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal Script As String, ByVal Tabla As String) As System.Data.DataTable

        Dim Cm As New SqlCommand()

        Try

            With Cm

                .Connection = mParent.Conn()
                .CommandType = Data.CommandType.Text
                .CommandText = Script

                Dim Da As New SqlDataAdapter()
                Dim T As New System.Data.DataTable(Tabla)
                With Da
                    .SelectCommand = Cm
                    .Fill(T)
                End With

                Return T

            End With

        Catch ex As Exception

            Throw New Exception("Error en Consultar.GetDataTable: " & ex.Message)

        End Try

    End Function

    ''' <summary>
    ''' Ejecuta la consulta y retorna un DataSet
    ''' </summary>
    ''' <param name="Script">Script de consulta SQL</param>
    ''' <param name="Tabla">Nombre de la tabla en el Dataset</param>
    ''' <param name="Ds">DataSet al cual se le agregará el resultado de la consulta</param>
    ''' <returns>Retorna un DataSet</returns>
    ''' <remarks></remarks>
    Public Function GetDataset(ByVal Script As String, ByVal Tabla As String, Optional ByVal Ds As System.Data.DataSet = Nothing) As System.Data.DataSet

        Dim Cm As New SqlCommand()

        GetDataset = Nothing

        Try

            With Cm

                .Connection = mParent.Conn()
                .CommandType = Data.CommandType.Text
                .CommandText = Script

                Dim Da As New SqlDataAdapter()
                If Ds Is Nothing Then Ds = New DataSet
                With Da
                    .SelectCommand = Cm
                    .Fill(Ds, Tabla)
                End With
                GetDataset = Ds

            End With

        Catch ex As Exception

            Throw New Exception("Error en Consultar.GetDataset: " & ex.Message)

        End Try

    End Function

    ''' <summary>
    ''' Ejecuta la consulta y retorna un DataReader
    ''' </summary>
    ''' <param name="Script">Script de consulta SQL</param>
    ''' <returns>Retorna un DataReader</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(ByVal Script As String) As SqlDataReader


        Dim Cm As New SqlCommand()

        GetDataReader = Nothing

        Try

            With Cm

                .Connection = mParent.Conn()
                .CommandType = Data.CommandType.Text
                .CommandText = Script

                Dim reader As SqlDataReader = Cm.ExecuteReader()
                GetDataReader = reader

            End With

        Catch ex As Exception

            Throw New Exception("Error en Consultar.GetDataReader: " & ex.Message)

        End Try

    End Function

End Class

Public Class ClsEjecutar

    Public Parametros As New Parametros
    Private mParent As AdoNet

    Friend Sub Parent(ByRef MiCon As AdoNet)
        mParent = MiCon
    End Sub

    Private Function GetParametros() As Parametros

        Try

            Dim p As clsParametro
            Dim Parametros As New Parametros
            For Each p In Me.Parametros.GetLista
                Parametros.Add(p.Nombre, p.Valor, p.Tipo, p.Direccion)
            Next
            GetParametros = Parametros

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetParametros: " & ex.Message)

        End Try

    End Function

    Private Function AddSplit(ByVal Valor As String, ByVal Nuevo As String) As String

        If Valor.Length > 0 Then
            Valor &= "," & Nuevo
        Else
            Valor = Nuevo
        End If

        Return Valor

    End Function

    ''' <summary>
    ''' Ejecuta el comando "Insert" sobre la base de datos
    ''' </summary>
    ''' <param name="Tabla">Nombre de la tabla donde se insertará el registro</param>
    ''' <param name="ReturnIdentity">Determina si se devuelve o no el campo identidad</param>
    ''' <returns>Nuevo valor del campo identidad insertado</returns>
    ''' <remarks></remarks>
    Public Function Insertar(ByVal Tabla As String, Optional ByVal ReturnIdentity As Boolean = False) As Long

        Try

            Dim Script As String = String.Empty
            Dim Campos As String = String.Empty
            Dim Valores As String = String.Empty
            Dim i As Integer = 0

            Insertar = 0

            For Each Parametro In GetParametros.GetLista

                Campos = AddSplit(Campos, "[" + Parametro.Nombre + "]")
                i += 1
                Valores = AddSplit(Valores, "@Param" & i.ToString)

            Next

            If ReturnIdentity Then
                Script = " Set NoCount On" +
                         " Insert into [" + Tabla + "] (" + Campos + ") values (" + Valores + ")" +
                         " Select IsNull(@@Identity,0)" +
                         " Set NoCount Off"
            Else
                Script = " Insert into [" + Tabla + "] (" + Campos + ") values (" + Valores + ")"
            End If

            Dim Cm As New SqlCommand()
            With Cm
                .Connection = mParent.Conn
                .CommandType = Data.CommandType.Text
                .CommandText = Script
                AddParameters(Cm)
                If ReturnIdentity Then
                    Insertar = .ExecuteScalar()
                Else
                    .ExecuteNonQuery()
                End If
            End With

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.Insertar: " & ex.Message)

        End Try

    End Function

    Private Sub AddParameters(ByVal Cm As SqlCommand)

        Try

            Dim Nombre As String = ""
            Dim i As Integer = 0

            If Parametros.Count > 0 Then
                Dim p As clsParametro
                For Each p In Parametros.GetLista
                    i += 1 : Nombre = "@Param" & i.ToString
                    Cm.Parameters.AddWithValue(Nombre, p.Valor)
                Next
            End If

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.AddParameters: " & ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Ejecuta el comando "Update" sobre la base de datos
    ''' </summary>
    ''' <param name="Tabla">Nombre de la tabla donde se modificará el registro</param>
    ''' <param name="Where">Condición para seleccionar el registro</param>
    ''' <remarks></remarks>
    Public Sub Modificar(ByVal Tabla As String, Optional ByVal Where As String = "")

        Try

            Dim Script As String = String.Empty
            Dim CamposYValores As String = String.Empty
            Dim i As Integer = 0

            For Each Parametro In GetParametros.GetLista
                CamposYValores = AddSplit(CamposYValores, "[" + Parametro.Nombre + "]=")
                i += 1
                CamposYValores &= "@Param" & i.ToString
            Next

            If Where.Length > 0 Then Where = " Where " + Where

            Script = CStr(" Update [" + Tabla + "] Set " + CamposYValores + Where)

            Dim Cm As New SqlCommand()
            With Cm
                .Connection = mParent.Conn
                .CommandType = Data.CommandType.Text
                .CommandText = Script
                AddParameters(Cm)
                .ExecuteNonQuery()
            End With

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.Modificar: " & ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Ejecuta el comando "Delete" sobre la base de datos
    ''' </summary>
    ''' <param name="Tabla">Nombre de la tabla donde se eliminará el registro</param>
    ''' <param name="Where">Condición para seleccionar el registro</param>
    ''' <remarks></remarks>
    Public Sub Borrar(ByVal Tabla As String, Optional ByVal Where As String = "")

        Try

            Dim Script As String = String.Empty
            Dim CamposYValores As String = String.Empty

            If Where.Length > 0 Then Where = " Where " + Where
            Script = " Delete [" + Tabla + "]" + Where

            Dim Cm As New SqlCommand()
            With Cm
                .Connection = mParent.Conn
                .CommandType = Data.CommandType.Text
                .CommandText = Script
                .ExecuteNonQuery()
            End With

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.Borrar: " & ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Ejecuta la instrucción SQL sobre la base de datos
    ''' </summary>
    ''' <param name="Script">Script SQL que se ejecutará sobre la base de datos</param>
    ''' <param name="Retorno">Tipo de retorno que devuelve la función</param>
    ''' <returns>Valor de retorno</returns>
    ''' <remarks></remarks>
    Public Function Instruccion(ByVal Script As String, Optional ByVal Retorno As AdoNet.TipoDeRetornoEjecutar = AdoNet.TipoDeRetornoEjecutar.NotReturn) As Object

        Try

            Dim Cm As New SqlCommand()
            With Cm
                .Connection = mParent.Conn
                .CommandType = Data.CommandType.Text
                .CommandText = Script
            End With

            Instruccion = GetRetorno(Cm, Retorno)

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.Instruccion: " & ex.Message)

        End Try

    End Function

    ''' <summary>
    ''' Ejecuta el Store Procedure sobre la base de datos
    ''' </summary>
    ''' <param name="Nombre">Nombre del Strore Procedure</param>
    ''' <param name="Retorno">Tipo de retorno que devuelve la función</param>
    ''' <returns>Valor de retorno</returns>
    ''' <remarks></remarks>
    Public Function Procedimiento(ByVal Nombre As String, Optional ByVal Retorno As AdoNet.TipoDeRetornoEjecutar = AdoNet.TipoDeRetornoEjecutar.NotReturn) As Object

        Try

            Dim Pt As SqlParameter
            Dim Cm As New SqlCommand()

            Procedimiento = Nothing

            With Cm
                .Connection = mParent.Conn
                If Parametros.Count > 0 Then
                    Dim p As clsParametro
                    For Each p In Parametros.GetLista
                        Pt = New SqlParameter
                        With Pt
                            .ParameterName = p.Nombre
                            .Value = p.Valor
                            .DbType = p.Tipo
                            .Direction = p.Direccion
                        End With
                        .Parameters.Add(Pt)
                        Pt = Nothing
                    Next
                End If
                .CommandType = Data.CommandType.StoredProcedure
                .CommandText = Nombre
            End With

            If Retorno = AdoNet.TipoDeRetornoEjecutar.ReturnDataset Then
                Dim Da As SqlDataAdapter = New SqlDataAdapter(Cm)
                Dim Ds As New DataSet
                Da.Fill(Ds)
                Return Ds
            Else
                GetRetorno(Cm, Retorno)
            End If

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.Procedimiento: " & ex.Message)

        End Try

    End Function

    Public Function GetSQLDateTime(ByVal Script As String) As DateTime

        Try

            Dim Valor As DateTime = Nothing
            Dim reader As SqlDataReader = Me.mParent.Consultar.GetDataReader(Script)
            If reader.HasRows Then
                Do While reader.Read()
                    If Not reader.IsDBNull(0) Then
                        Valor = reader.GetDateTime(0)
                    Else
                        Valor = Nothing
                    End If
                Loop
            End If
            If Not reader.IsClosed Then reader.Close()

            Return Valor

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetSQLDateTime: " & ex.Message)

        End Try

    End Function

    Public Function GetSQLBoolean(ByVal Script As String) As Boolean

        Try

            Dim Valor As Boolean = False
            Dim reader As SqlDataReader = Me.mParent.Consultar.GetDataReader(Script)
            If reader.HasRows Then
                Do While reader.Read()
                    If Not reader.IsDBNull(0) Then
                        Valor = reader.GetBoolean(0)
                    Else
                        Valor = 0
                    End If
                Loop
            End If
            If Not reader.IsClosed Then reader.Close()

            Return Valor

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetSQLBoolean: " & ex.Message)

        End Try

    End Function

    Public Function GetSQLInteger(ByVal Script As String) As Integer

        Try

            Dim Valor As Integer = 0
            Dim reader As SqlDataReader = Me.mParent.Consultar.GetDataReader(Script)
            If reader.HasRows Then
                Do While reader.Read()
                    If Not reader.IsDBNull(0) Then
                        Valor = reader.GetInt32(0)
                    Else
                        Valor = 0
                    End If
                Loop
            End If
            If Not reader.IsClosed Then reader.Close()

            Return Valor

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetSQLInteger: " & ex.Message)

        End Try

    End Function
    Public Function GetSQLTinyInt(ByVal Script As String) As Short

        Try

            Dim Valor As Integer = 0
            Dim reader As SqlDataReader = Me.mParent.Consultar.GetDataReader(Script)
            If reader.HasRows Then
                Do While reader.Read()
                    If Not reader.IsDBNull(0) Then
                        Valor = reader.GetInt16(0)
                    Else
                        Valor = 0
                    End If
                Loop
            End If
            If Not reader.IsClosed Then reader.Close()

            Return Valor

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetSQLInteger: " & ex.Message)

        End Try

    End Function

    Public Function GetSQLString(ByVal Script As String) As String

        Try

            Dim Valor As String = String.Empty
            Dim reader As SqlDataReader = Me.mParent.Consultar.GetDataReader(Script)
            If reader.HasRows Then
                Do While reader.Read()
                    If Not reader.IsDBNull(0) Then
                        Valor = reader.GetString(0)
                    Else
                        Valor = String.Empty
                    End If
                Loop
            End If
            If Not reader.IsClosed Then reader.Close()

            Return Valor

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetSQLString: " & ex.Message)

        End Try

    End Function

    Private Function GetRetorno(ByVal Cm As SqlCommand, ByVal Retorno As AdoNet.TipoDeRetornoEjecutar) As Object

        Try

            GetRetorno = Nothing

            With Cm

                Select Case Retorno

                    Case AdoNet.TipoDeRetornoEjecutar.ReturnValue

                        Dim p As New SqlParameter
                        p.ParameterName = "RETURN"
                        p.DbType = SqlDbType.Int
                        p.Direction = ParameterDirection.ReturnValue
                        Cm.Parameters.Add(p)
                        p = Nothing
                        .ExecuteNonQuery()
                        GetRetorno = Cm.Parameters(0).Value

                    Case AdoNet.TipoDeRetornoEjecutar.ReturnScalar

                        GetRetorno = .ExecuteScalar()

                    Case AdoNet.TipoDeRetornoEjecutar.NotReturn

                        .ExecuteNonQuery()

                        If Parametros.Count > 0 Then
                            Dim p As SqlParameter
                            For Each p In Cm.Parameters
                                Parametros.Item(p.ParameterName).Valor = p.Value
                            Next
                        End If

                End Select

            End With

        Catch ex As Exception

            Throw New Exception("Error en Ejecutar.GetRetorno: " & ex.Message)

        End Try

    End Function

End Class

Public Class clsParametro

    Public Nombre As String
    Public Valor As Object
    Public Direccion As ParameterDirection
    Public Tipo As SqlDbType

End Class

Public Class Parametros

    Private lstObj As New List(Of clsParametro)
    ''' <summary>
    ''' Agrega un parámetro a la colección de parámetros
    ''' </summary>
    ''' <param name="Nombre">Nombre del parámetro</param>
    ''' <param name="Valor">Valor del parámetro</param>
    ''' <param name="Tipo">Tipo de dato del parámetro</param>
    ''' <param name="Direccion">Determina si el parámetro es de entrada y/o salida</param>
    ''' <returns>Retorna un objeto parámetro</returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal Nombre As String,
                        ByVal Valor As Object,
                        Optional ByVal Tipo As SqlDbType = Nothing,
                        Optional ByVal Direccion As ParameterDirection = ParameterDirection.Input = Nothing) As clsParametro

        Dim Obj = New clsParametro
        Obj.Nombre = Nombre
        Obj.Valor = Valor
        Obj.Tipo = Tipo
        Obj.Direccion = Direccion
        lstObj.Add(Obj)
        Add = Obj
        Obj = Nothing

    End Function

    ''' <summary>
    ''' Retorna un objeto parámetro por el nombre
    ''' </summary>
    ''' <param name="Nombre">Nombre del parámetro</param>
    ''' <value></value>
    ''' <returns>Retorna un objeto parámetro</returns>
    ''' <remarks></remarks>
    Default Public ReadOnly Property Item(ByVal Nombre As String) As clsParametro

        Get
            Item = Nothing
            If Me.Exist(Nombre) Then Return (From o In lstObj Where o.Nombre = Nombre).First
        End Get

    End Property

    ''' <summary>
    ''' Retorna la lista de parámetros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GetLista() As List(Of clsParametro)
        Get
            Return lstObj
        End Get
        Set(ByVal value As List(Of clsParametro))
            lstObj = value
        End Set
    End Property

    ''' <summary>
    ''' Retorna la cantidad de parámetros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count() As Long
        Get
            Return lstObj.Count
        End Get
    End Property

    ''' <summary>
    ''' Determina si existe un parámetro
    ''' </summary>
    ''' <param name="Nombre">Nombre del parámetro</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exist(ByVal Nombre As String) As Boolean

        Exist = (From o In lstObj Where o.Nombre = Nombre).Count > 0

    End Function

    ''' <summary>
    ''' Remueve todos los parámetros
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RemoveAll()

        lstObj = Nothing
        lstObj = New List(Of clsParametro)

    End Sub

    ''' <summary>
    ''' Remueve un parámetro en particular
    ''' </summary>
    ''' <param name="Nombre">Nombre del parámetro a eliminar</param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal Nombre As String)

        If Not Me.Exist(Nombre) Then Exit Sub
        lstObj.Remove(Me.Item(Nombre))

    End Sub

    ''' <summary>
    ''' Crea el objeto parámetros
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        lstObj = New List(Of clsParametro)

    End Sub

    ''' <summary>
    ''' Libera el espacio de memoria ocupado por el objeto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose()

        lstObj = Nothing

    End Sub

End Class
