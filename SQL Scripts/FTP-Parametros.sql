Delete dbo.[BL_PARAMETROS] Where [PARAMETRO] = 'Autogestion\FTP_Servidor'
Delete dbo.[BL_PARAMETROS] Where [PARAMETRO] = 'Autogestion\FTP_Usuario'
Delete dbo.[BL_PARAMETROS] Where [PARAMETRO] = 'Autogestion\FTP_Contrasenia'
Delete dbo.[BL_PARAMETROS] Where [PARAMETRO] = 'Autogestion\FTP_Directorio'

Insert into dbo.[BL_PARAMETROS] ([PARAMETRO],[STRVALOR],[INTVALOR],[DATEVALOR],[TIPOVALOR],[FLOATVALOR],[RegistrarVarSiEs0],[CodEmp]) Values  ('Autogestion\FTP_Servidor','ftp.bas.com.ar',Null,Null,'S',Null,0,Null)
Insert into dbo.[BL_PARAMETROS] ([PARAMETRO],[STRVALOR],[INTVALOR],[DATEVALOR],[TIPOVALOR],[FLOATVALOR],[RegistrarVarSiEs0],[CodEmp]) Values  ('Autogestion\FTP_Usuario','baslaborod',Null,Null,'S',Null,0,Null)
Insert into dbo.[BL_PARAMETROS] ([PARAMETRO],[STRVALOR],[INTVALOR],[DATEVALOR],[TIPOVALOR],[FLOATVALOR],[RegistrarVarSiEs0],[CodEmp]) Values  ('Autogestion\FTP_Contrasenia','piojonegro',Null,Null,'S',Null,0,Null)
Insert into dbo.[BL_PARAMETROS] ([PARAMETRO],[STRVALOR],[INTVALOR],[DATEVALOR],[TIPOVALOR],[FLOATVALOR],[RegistrarVarSiEs0],[CodEmp]) Values  ('Autogestion\FTP_Directorio','/BASLABOROD/RECIBOSDEMO',Null,Null,'S',Null,0,Null)


